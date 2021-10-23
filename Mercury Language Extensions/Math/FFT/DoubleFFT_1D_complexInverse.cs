using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math
{
    public sealed partial class DoubleFFT_1D
    {

        public void complexInverse(double[] a, Boolean scale)
        {
            complexInverse(a, 0, scale);
        }

        public void complexInverse(DoubleLargeArray a, Boolean scale)
        {
            complexInverse(a, 0, scale);
        }

        /**
 * Computes 1D inverse DFT of complex data leaving the result in
 * <code>a</code>. Complex number is stored as two double values in
 * sequence: the real and imaginary part, i.e. the size of the input array
 * must be greater or equal 2*n. The physical layout of the input data has
 * to be as follows:<br>
 *  
 * <pre>
 * a[offa+2*k] = Re[k], 
 * a[offa+2*k+1] = Im[k], 0&lt;=k&lt;n
 * </pre>
 *  
 * @param a     data to transform
 * @param offa  index of the first element in array <code>a</code>
 * @param scale if true then scaling is performed
 */
        public void complexInverse(double[] a, int offa, Boolean scale)
        {
            if (useLargeArrays)
            {
                complexInverse(new DoubleLargeArray(a), offa, scale);
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
                        (2 * n).cftfsub(ref a, offa, ref ip, nw, ref w);
                        break;
                    case Plans.MIXED_RADIX:
                        cfftf(a, offa, +1);
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_complex(a, offa, 1);
                        break;
                }
                if (scale)
                {
                    (n).scale(1.0 / (double)n, ref a, offa, true);
                }
            }
        }

        /**
         * Computes 1D inverse DFT of complex data leaving the result in
         * <code>a</code>. Complex number is stored as two double values in
         * sequence: the real and imaginary part, i.e. the size of the input array
         * must be greater or equal 2*n. The physical layout of the input data has
         * to be as follows:<br>
         *  
         * <pre>
         * a[offa+2*k] = Re[k], 
         * a[offa+2*k+1] = Im[k], 0&lt;=k&lt;n
         * </pre>
         *  
         * @param a     data to transform
         * @param offa  index of the first element in array <code>a</code>
         * @param scale if true then scaling is performed
         */
        public void complexInverse(DoubleLargeArray a, long offa, Boolean scale)
        {
            if (!useLargeArrays)
            {
                if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
                {
                    complexInverse(a.ToArray(), (int)offa, scale);
                }
                else
                {
                    throw new ArgumentException("The data array is too big.");
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
                        (2 * nl).cftfsub(ref a, offa, ref ipl, nwl, ref wl);
                        break;
                    case Plans.MIXED_RADIX:
                        cfftf(a, offa, +1);
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_complex(a, offa, 1);
                        break;
                }
                if (scale)
                {
                    nl.scale(1.0 / (double)nl, ref a, offa, true);
                }
            }
        }
    }
}
