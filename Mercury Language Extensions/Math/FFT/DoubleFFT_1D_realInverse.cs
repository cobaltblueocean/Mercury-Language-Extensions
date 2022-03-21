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
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the input data has to be as follows:<br>
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
        /// inverse transform, use <code>realInverseFull</code>.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// 
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverse(double[] a, Boolean scale)
        {
            realInverse(a, 0, scale);
        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the input data has to be as follows:<br>
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
        /// inverse transform, use <code>realInverseFull</code>.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// 
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverse(DoubleLargeArray a, Boolean scale)
        {
            realInverse(a, 0, scale);
        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the input data has to be as follows:<br>
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
        /// inverse transform, use <code>realInverseFull</code>.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// <param name="offa"> index of the first element in array <code>a</code></param>
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverse(double[] a, int offa, Boolean scale)
        {
            if (useLargeArrays)
            {
                realInverse(new DoubleLargeArray(a), offa, scale);
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
                        a[offa + 1] = 0.5 * (a[offa] - a[offa + 1]);
                        a[offa] -= a[offa + 1];
                        if (n > 4)
                        {
                            n.rftfsub(ref a, offa, nc, ref w, nw);
                            n.cftbsub(ref a, offa, ref ip, nw, ref w);
                        }
                        else if (n == 4)
                        {
                            a.cftxc020(offa);
                        }
                        if (scale)
                        {
                            n.scale(1.0 / (n / 2.0), ref a, offa, false);
                        }
                        break;
                    case Plans.MIXED_RADIX:
                        for (int k = 2; k < n; k++)
                        {
                            int idx = offa + k;
                            double tmp = a[idx - 1];
                            a[idx - 1] = a[idx];
                            a[idx] = tmp;
                        }
                        rfftb(a, offa);
                        if (scale)
                        {
                            n.scale(1.0 / n, ref a, offa, false);
                        }
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_real_inverse(a, offa);
                        if (scale)
                        {
                            n.scale(1.0 / n, ref a, offa, false);
                        }
                        break;
                }
            }

        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . The physical layout of the input data has to be as follows:<br>
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
        /// inverse transform, use <code>realInverseFull</code>.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// <param name="offa"> index of the first element in array <code>a</code></param>
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverse(DoubleLargeArray a, long offa, Boolean scale)
        {
            if (!useLargeArrays)
            {
                if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
                {
                    realInverse(a.ToArray(), (int)offa, scale);
                }
                else
                {
                    throw new ArgumentException(LocalizedResources.Instance().DATA_ARRAY_IS_TOO_BIG);
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
                        a[offa + 1] = 0.5 * (a[offa] - a[offa + 1]);
                        a[offa] = a[offa] - a[offa + 1];
                        if (nl > 4)
                        {
                            nl.rftfsub(ref a, offa, ncl, ref wl, nwl);
                            nl.cftbsub(ref a, offa, ref ipl, nwl, ref wl);
                        }
                        else if (nl == 4)
                        {
                            a.cftxc020(offa);
                        }
                        if (scale)
                        {
                            nl.scale(1.0 / (nl / 2.0), ref a, offa, false);
                        }
                        break;
                    case Plans.MIXED_RADIX:
                        for (long k = 2; k < nl; k++)
                        {
                            long idx = offa + k;
                            double tmp = a[idx - 1];
                            a[idx - 1] = a[idx];
                            a[idx] = tmp;
                        }
                        rfftb(a, offa);
                        if (scale)
                        {
                            nl.scale(1.0 / nl, ref a, offa, false);
                        }
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_real_inverse(a, offa);
                        if (scale)
                        {
                            nl.scale(1.0 / nl, ref a, offa, false);
                        }
                        break;
                }
            }

        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real inverse transform, i.e. you will get
        /// the same result as from <code>complexInverse</code> called with all
        /// imaginary part equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverseFull(double[] a, Boolean scale)
        {
            realInverseFull(a, 0, scale);
        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real inverse transform, i.e. you will get
        /// the same result as from <code>complexInverse</code> called with all
        /// imaginary part equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverseFull(DoubleLargeArray a, Boolean scale)
        {
            RealInverseFull(a, 0, scale);
        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real inverse transform, i.e. you will get
        /// the same result as from <code>complexInverse</code> called with all
        /// imaginary part equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// <param name="offa"> index of the first element in array <code>a</code></param>
        /// <param name="scale">if true then scaling is performed</param>
        public void realInverseFull(double[] a, int offa, Boolean scale)
        {
            if (useLargeArrays)
            {
                //realInverseFull(new DoubleLargeArray(a), offa, scale);
            }
            else
            {
                int twon = 2 * n;
                switch (plan)
                {
                    case Plans.SPLIT_RADIX:
                        realInverse(a, offa, scale);
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
                                //} catch (InterruptedException ex) {
                                //    Logger.getLogger(DoubleFFT_1D.class.Name).log(Level.SEVERE, null, ex);
                            }
                            catch (SystemException ex)
                            {
                                Logger.Error(ex.ToString());
                                //Logger.getLogger(DoubleFFT_1D.class.Name).log(Level.SEVERE, null, ex);
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
                        if (scale)
                        {
                            n.scale(1.0 / n, ref a, offa, false);
                        }
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
                            int idx1 = offa + 2 * k;
                            int idx2 = offa + twon - 2 * k;
                            a[idx1] = -a[idx1];
                            a[idx2 + 1] = -a[idx1];
                            a[idx2] = a[idx1 - 1];
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
                        bluestein_real_full(a, offa, 1);
                        if (scale)
                        {
                            n.scale(1.0 / n, ref a, offa, true);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Computes 1D inverse DFT of real data leaving the result in <code>a</code>
        /// . This method computes the full real inverse transform, i.e. you will get
        /// the same result as from <code>complexInverse</code> called with all
        /// imaginary part equal 0. Because the result is stored in <code>a</code>,
        /// the size of the input array must greater or equal 2*n, with only the
        /// first n elements filled with real data.
        /// 
        /// </summary>
        /// <param name="a">    data to transform</param>
        /// <param name="offa"> index of the first element in array <code>a</code></param>
        /// <param name="scale">if true then scaling is performed</param>
        public void RealInverseFull(DoubleLargeArray a, long offa, Boolean scale)
        {
            if (!useLargeArrays)
            {
                if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
                {
                    realInverseFull(a.ToArray(), (int)offa, scale);
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
                        realInverse(a, offa, scale);
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
                                    long idx1, idx2;
                                    for (long l = firstIdx; l < lastIdx; l++)
                                    {
                                        idx1 = 2 * l;
                                        idx2 = offa + ((twon - idx1) % twon);
                                        a[idx2] = a[offa + idx1];
                                        a[idx2 + 1] = -a[offa + idx1 + 1];
                                    }

                                });
                            }
                            try
                            {
                                Task.WaitAll(taskArray);
                                //} catch (InterruptedException ex) {
                                //    Logger.getLogger(DoubleFFT_1D.class.Name).log(Level.SEVERE, null, ex);
                            }
                            catch (SystemException ex)
                            {
                                Logger.Error(ex.ToString());
                                //Logger.getLogger(DoubleFFT_1D.class.Name).log(Level.SEVERE, null, ex);
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
                        if (scale)
                        {
                            nl.scale(1.0 / nl, ref a, offa, false);
                        }
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
                            long idx1 = offa + 2 * k;
                            long idx2 = offa + twon - 2 * k;
                            a[idx1] = -a[idx1];
                            a[idx2 + 1] = -a[idx1];
                            a[idx2] = a[idx1 - 1];
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
                        bluestein_real_full(a, offa, 1);
                        if (scale)
                        {
                            nl.scale(1.0 / nl, ref a, offa, true);
                        }
                        break;
                }
            }
        }

        #region Unused Code
        //protected void realInverse2(double[] a, int offa, Boolean scale)
        //{
        //    if (useLargeArrays)
        //    {
        //        realInverse2(new DoubleLargeArray(a), offa, scale);
        //    }
        //    else
        //    {
        //        if (n == 1)
        //        {
        //            return;
        //        }
        //        switch (plan)
        //        {
        //            case Plans.SPLIT_RADIX:
        //                double xi;

        //                if (n > 4)
        //                {
        //                    n.cftfsub(ref a, offa, ref ip, nw, ref w);
        //                    n.rftbsub(ref a, offa, nc, ref w, nw);
        //                }
        //                else if (n == 4)
        //                {
        //                    n.cftbsub(ref a, offa, ref ip, nw, ref w);
        //                }
        //                xi = a[offa] - a[offa + 1];
        //                a[offa] += a[offa + 1];
        //                a[offa + 1] = xi;
        //                if (scale)
        //                {
        //                    n.scale(1.0 / n, ref a, offa, false);
        //                }
        //                break;
        //            case Plans.MIXED_RADIX:
        //                rfftf(a, offa);
        //                for (int k = n - 1; k >= 2; k--)
        //                {
        //                    int idx = offa + k;
        //                    double tmp = a[idx];
        //                    a[idx] = a[idx - 1];
        //                    a[idx - 1] = tmp;
        //                }
        //                if (scale)
        //                {
        //                    n.scale(1.0 / n, ref a, offa, false);
        //                }
        //                int m;
        //                if (n % 2 == 0)
        //                {
        //                    m = n / 2;
        //                    for (int i = 1; i < m; i++)
        //                    {
        //                        int idx = offa + 2 * i + 1;
        //                        a[idx] = -a[idx];
        //                    }
        //                }
        //                else
        //                {
        //                    m = (n - 1) / 2;
        //                    for (int i = 0; i < m; i++)
        //                    {
        //                        int idx = offa + 2 * i + 1;
        //                        a[idx] = -a[idx];
        //                    }
        //                }
        //                break;
        //            case Plans.BLUESTEIN:
        //                bluestein_real_inverse2(a, offa);
        //                if (scale)
        //                {
        //                    n.scale(1.0 / n, ref a, offa, false);
        //                }
        //                break;
        //        }
        //    }
        //}

        //protected void realInverse2(DoubleLargeArray a, long offa, Boolean scale)
        //{
        //    if (!useLargeArrays)
        //    {
        //        if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
        //        {
        //            realInverse2(a.ToArray(), (int)offa, scale);
        //        }
        //        else
        //        {
        //            throw new ArgumentException(LocalizedResources.Instance().DATA_ARRAY_IS_TOO_BIG);
        //        }
        //    }
        //    else
        //    {
        //        if (nl == 1)
        //        {
        //            return;
        //        }
        //        switch (plan)
        //        {
        //            case Plans.SPLIT_RADIX:
        //                double xi;

        //                if (nl > 4)
        //                {
        //                    nl.cftfsub(ref a, offa, ref ipl, nwl, ref wl);
        //                    nl.rftbsub(ref a, offa, ncl, ref wl, nwl);
        //                }
        //                else if (nl == 4)
        //                {
        //                    nl.cftbsub(ref a, offa, ref ipl, nwl, ref wl);
        //                }
        //                xi = a[offa] - a[offa + 1];
        //                a[offa] = a[offa] + a[offa + 1];
        //                a[offa + 1] = xi;
        //                if (scale)
        //                {
        //                    nl.scale(1.0 / nl, ref a, offa, false);
        //                }
        //                break;
        //            case Plans.MIXED_RADIX:
        //                rfftf(a, offa);
        //                for (long k = nl - 1; k >= 2; k--)
        //                {
        //                    long idx = offa + k;
        //                    double tmp = a[idx];
        //                    a[idx] = a[idx - 1];
        //                    a[idx - 1] = tmp;
        //                }
        //                if (scale)
        //                {
        //                    nl.scale(1.0 / nl, ref a, offa, false);
        //                }
        //                long m;
        //                if (nl % 2 == 0)
        //                {
        //                    m = nl / 2;
        //                    for (long i = 1; i < m; i++)
        //                    {
        //                        long idx = offa + 2 * i + 1;
        //                        a[idx] = -a[idx];
        //                    }
        //                }
        //                else
        //                {
        //                    m = (nl - 1) / 2;
        //                    for (long i = 0; i < m; i++)
        //                    {
        //                        long idx = offa + 2 * i + 1;
        //                        a[idx] = -a[idx];
        //                    }
        //                }
        //                break;
        //            case Plans.BLUESTEIN:
        //                bluestein_real_inverse2(a, offa);
        //                if (scale)
        //                {
        //                    nl.scale(1.0 / nl, ref a, offa, false);
        //                }
        //                break;
        //        }
        //    }
        //}

        #endregion

    }
}
