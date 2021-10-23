using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Log;

namespace Mercury.Language.Math
{
    public sealed partial class DoubleFFT_1D
    {
        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the output data is as follows:<br>
        /// 
        /// if n is even then
        /// 
        /// <pre>
        /// a[2*k] = Re[k], 0&lt;=k&lt;n/2
        /// a[2*k+1] = Im[k], 0&lt;k&lt;n/2
        /// a[1] = Re[n/2]
        /// </pre>
        /// 
        /// if n is odd then
        /// 
        /// <pre>
        /// a[2*k] = Re[k], 0&lt;=k&lt;(n+1)/2
        /// a[2*k+1] = Im[k], 0&lt;k&lt;(n-1)/2
        /// a[1] = Im[(n-1)/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transform. The
        /// other half satisfies the symmetry condition. If you want the full real
        /// forward transform, use <code>realForwardFull</code>. To get back the
        /// original data, use <code>realInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">data to transform</param>
        public void RealForward(double[] a)
        {
            RealForward(a, 0);
        }

        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the output data is as follows:<br>
        /// 
        /// if n is even then
        /// 
        /// <pre>
        /// a[2*k] = Re[k], 0&lt;=k&lt;n/2
        /// a[2*k+1] = Im[k], 0&lt;k&lt;n/2
        /// a[1] = Re[n/2]
        /// </pre>
        /// 
        /// if n is odd then
        /// 
        /// <pre>
        /// a[2*k] = Re[k], 0&lt;=k&lt;(n+1)/2
        /// a[2*k+1] = Im[k], 0&lt;k&lt;(n-1)/2
        /// a[1] = Im[(n-1)/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transform. The
        /// other half satisfies the symmetry condition. If you want the full real
        /// forward transform, use <code>realForwardFull</code>. To get back the
        /// original data, use <code>realInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">data to transform</param>
        public void RealForward(DoubleLargeArray a)
        {
            RealForward(a, 0);
        }

        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the output data is as follows:<br>
        /// 
        /// if n is even then
        /// 
        /// <pre>
        /// a[offa+2*k] = Re[k], 0&lt;=k&lt;n/2
        /// a[offa+2*k+1] = Im[k], 0&lt;k&lt;n/2
        /// a[offa+1] = Re[n/2]
        /// </pre>
        /// 
        /// if n is odd then
        /// 
        /// <pre>
        /// a[offa+2*k] = Re[k], 0&lt;=k&lt;(n+1)/2
        /// a[offa+2*k+1] = Im[k], 0&lt;k&lt;(n-1)/2
        /// a[offa+1] = Im[(n-1)/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transform. The
        /// other half satisfies the symmetry condition. If you want the full real
        /// forward transform, use <code>realForwardFull</code>. To get back the
        /// original data, use <code>realInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">   data to transform</param>
        /// <param name="offa">index of the first element in array <code>a</code></param>
        public void RealForward(double[] a, int offa)
        {
            if (useLargeArrays)
            {
                RealForward(new DoubleLargeArray(a), offa);
            }
            else
            {
                if (n == 1)
                {
                    return;
                }

                switch (plan)
                {
                    case Plans.SPLIT_RADIX:
                        double xi;

                        if (n > 4)
                        {
                            n.cftfsub(ref a, offa, ref ip, nw, ref w);
                            n.rftfsub(ref a, offa, nc, ref w, nw);
                        }
                        else if (n == 4)
                        {
                            a.cftx020(offa);
                        }
                        xi = a[offa] - a[offa + 1];
                        a[offa] += a[offa + 1];
                        a[offa + 1] = xi;
                        break;
                    case Plans.MIXED_RADIX:
                        rfftf(a, offa);
                        for (int k = n - 1; k >= 2; k--)
                        {
                            int idx = offa + k;
                            double tmp = a[idx];
                            a[idx] = a[idx - 1];
                            a[idx - 1] = tmp;
                        }
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_real_forward(a, offa);
                        break;
                }
            }
        }

        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the output data is as follows:<br>
        /// 
        /// if n is even then
        /// 
        /// <pre>
        /// a[offa+2*k] = Re[k], 0&lt;=k&lt;n/2
        /// a[offa+2*k+1] = Im[k], 0&lt;k&lt;n/2
        /// a[offa+1] = Re[n/2]
        /// </pre>
        /// 
        /// if n is odd then
        /// 
        /// <pre>
        /// a[offa+2*k] = Re[k], 0&lt;=k&lt;(n+1)/2
        /// a[offa+2*k+1] = Im[k], 0&lt;k&lt;(n-1)/2
        /// a[offa+1] = Im[(n-1)/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transform. The
        /// other half satisfies the symmetry condition. If you want the full real
        /// forward transform, use <code>realForwardFull</code>. To get back the
        /// original data, use <code>realInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">   data to transform</param>
        /// <param name="offa">index of the first element in array <code>a</code></param>
        public void RealForward(DoubleLargeArray a, long offa)
        {
            if (!useLargeArrays)
            {
                if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
                {
                    RealForward(a.ToArray(), (int)offa);
                }
                else
                {
                    throw new ArgumentException(LocalizedResources.Instance().Utility_FFT_Double1D_ComplexForward_TheDataArrayIsTooBig);
                }
            }
            else
            {
                if (nl == 1)
                {
                    return;
                }

                switch (plan)
                {
                    case Plans.SPLIT_RADIX:
                        double xi;

                        if (nl > 4)
                        {
                            nl.cftfsub(ref a, offa, ref ipl, nwl, ref wl);
                            nl.rftfsub(ref a, offa, ncl, ref wl, nwl);
                        }
                        else if (nl == 4)
                        {
                            a.cftx020(offa);
                        }
                        xi = a[offa] - a[offa + 1];
                        a[offa] = a[offa] + a[offa + 1];
                        a[offa + 1] = xi;
                        break;
                    case Plans.MIXED_RADIX:
                        rfftf(a, offa);
                        for (long k = nl - 1; k >= 2; k--)
                        {
                            long idx = offa + k;
                            double tmp = a[idx];
                            a[idx] = a[idx - 1];
                            a[idx - 1] = tmp;
                        }
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_real_forward(a, offa);
                        break;
                }
            }
        }

        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real forward transform, i.e. you will get
        /// the same result as from <code>complexForward</code> called with all
        /// imaginary parts equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data. To get back the original data,
        /// use <code>complexInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">data to transform</param>
        public void RealForwardFull(double[] a)
        {
            RealForwardFull(a, 0);
        }

        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real forward transform, i.e. you will get
        /// the same result as from <code>complexForward</code> called with all
        /// imaginary part equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data. To get back the original data,
        /// use <code>complexInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">   data to transform</param>
        /// <param name="offa">index of the first element in array <code>a</code></param>
        public void RealForwardFull(double[] a, int offa)
        {

            if (useLargeArrays)
            {
                RealForwardFull(new DoubleLargeArray(a), offa);
            }
            else
            {
                int twon = 2 * n;
                switch (plan)
                {
                    case Plans.SPLIT_RADIX:
                        RealForward(a, offa);
                        int nthreads = Process.GetCurrentProcess().Threads.Count;
                        if ((nthreads > 1) && (n / 2 > THREADS_BEGIN_N_1D_FFT_2THREADS))
                        {
                            Task[] taskArray = new Task[nthreads];
                            int k = n / 2 / nthreads;
                            for (int i = 0; i < nthreads; i++)
                            {
                                int firstIdx = i * k;
                                int lastIdx = (i == (nthreads - 1)) ? n / 2 : firstIdx + k;
                                taskArray[i] = Task.Factory.StartNew(() =>
                                {
                                    {
                                        int idx1, idx2;
                                        for (int l = firstIdx; l < lastIdx; l++)
                                        {
                                            idx1 = 2 * l;
                                            idx2 = offa + ((twon - idx1) % twon);
                                            a[idx2] = a[offa + idx1];
                                            a[idx2 + 1] = -a[offa + idx1 + 1];
                                        }
                                    }
                                });
                            }
                            try
                            {
                                Task.WaitAll(taskArray);
                            }
                            catch (TaskCanceledException ex)
                            {
                                Logger.Error(ex.ToString());
                            }
                            catch (SystemException ex)
                            {
                                Logger.Error(ex.ToString());
                            }
                        }
                        else
                        {
                            int idx1, idx2;
                            for (int k = 0; k < n / 2; k++)
                            {
                                idx1 = 2 * k;
                                idx2 = offa + ((twon - idx1) % twon);
                                a[idx2] = a[offa + idx1];
                                a[idx2 + 1] = -a[offa + idx1 + 1];
                            }
                        }
                        a[offa + n] = -a[offa + 1];
                        a[offa + 1] = 0;
                        break;
                    case Plans.MIXED_RADIX:
                        rfftf(a, offa);
                        int m;
                        if (n % 2 == 0)
                        {
                            m = n / 2;
                        }
                        else
                        {
                            m = (n + 1) / 2;
                        }
                        for (int k = 1; k < m; k++)
                        {
                            int idx1 = offa + twon - 2 * k;
                            int idx2 = offa + 2 * k;
                            a[idx1 + 1] = -a[idx2];
                            a[idx1] = a[idx2 - 1];
                        }
                        for (int k = 1; k < n; k++)
                        {
                            int idx = offa + n - k;
                            double tmp = a[idx + 1];
                            a[idx + 1] = a[idx];
                            a[idx] = tmp;
                        }
                        a[offa + 1] = 0;
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_real_full(a, offa, -1);
                        break;
                }
            }
        }

        /// <summary>
        /// Computes 1D forward DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real forward transform, i.e. you will get
        /// the same result as from <code>complexForward</code> called with all
        /// imaginary part equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data. To get back the original data,
        /// use <code>complexInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a">   data to transform</param>
        /// <param name="offa">index of the first element in array <code>a</code></param>
        public void RealForwardFull(DoubleLargeArray a, long offa)
        {

            if (!useLargeArrays)
            {
                if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
                {
                    RealForwardFull(a.ToArray(), (int)offa);
                }
                else
                {
                    throw new ArgumentException(LocalizedResources.Instance().Utility_FFT_Double1D_ComplexForward_TheDataArrayIsTooBig);
                }
            }
            else
            {
                long twon = 2 * nl;
                switch (plan)
                {
                    case Plans.SPLIT_RADIX:
                        RealForward(a, offa);
                        int nthreads = Process.GetCurrentProcess().Threads.Count;
                        if ((nthreads > 1) && (nl / 2 > THREADS_BEGIN_N_1D_FFT_2THREADS))
                        {
                            Task[] taskArray = new Task[nthreads];
                            long k = nl / 2 / nthreads;
                            for (int i = 0; i < nthreads; i++)
                            {
                                long firstIdx = i * k;
                                long lastIdx = (i == (nthreads - 1)) ? nl / 2 : firstIdx + k;
                                taskArray[i] = Task.Factory.StartNew(() =>
                                {
                                    {
                                        long idx1, idx2;
                                        for (long l = firstIdx; l < lastIdx; l++)
                                        {
                                            idx1 = 2 * l;
                                            idx2 = offa + ((twon - idx1) % twon);
                                            a[idx2] = a[offa + idx1];
                                            a[idx2 + 1] = -a[offa + idx1 + 1];
                                        }
                                    }
                                });
                            }
                            try
                            {
                                Task.WaitAll(taskArray);
                            }
                            catch (TaskCanceledException ex)
                            {
                                Logger.Error(ex.ToString());
                            }
                            catch (SystemException ex)
                            {
                                Logger.Error(ex.ToString());
                            }
                        }
                        else
                        {
                            long idx1, idx2;
                            for (long k = 0; k < nl / 2; k++)
                            {
                                idx1 = 2 * k;
                                idx2 = offa + ((twon - idx1) % twon);
                                a[idx2] = a[offa + idx1];
                                a[idx2 + 1] = -a[offa + idx1 + 1];
                            }
                        }
                        a[offa + nl] = -a[offa + 1];
                        a[offa + 1] = 0;
                        break;
                    case Plans.MIXED_RADIX:
                        rfftf(a, offa);
                        long m;
                        if (nl % 2 == 0)
                        {
                            m = nl / 2;
                        }
                        else
                        {
                            m = (nl + 1) / 2;
                        }
                        for (long k = 1; k < m; k++)
                        {
                            long idx1 = offa + twon - 2 * k;
                            long idx2 = offa + 2 * k;
                            a[idx1 + 1] = -a[idx2];
                            a[idx1] = a[idx2 - 1];
                        }
                        for (long k = 1; k < nl; k++)
                        {
                            long idx = offa + nl - k;
                            double tmp = a[idx + 1];
                            a[idx + 1] = a[idx];
                            a[idx] = tmp;
                        }
                        a[offa + 1] = 0;
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_real_full(a, offa, -1);
                        break;
                }
            }
        }
    }
}
