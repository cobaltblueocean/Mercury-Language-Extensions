using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math
{
    public sealed partial class DoubleFFT_1D
    {
        public void ComplexForward(double[] a)
        {
            ComplexForward(a, 0);
        }

        public void ComplexForward(DoubleLargeArray a)
        {
            ComplexForward(a, 0);
        }

        public void ComplexForward(double[] a, int offa)
        {
            if (useLargeArrays)
            {
                ComplexForward(new DoubleLargeArray(a), offa);
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
                        (2 * n).cftbsub(ref a, offa, ref ip, nw, ref w);
                        break;
                    case Plans.MIXED_RADIX:
                        cfftf(a, offa, -1);
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_complex(a, offa, -1);
                        break;
                }
            }
        }

        public void ComplexForward(DoubleLargeArray a, long offa)
        {
            if (!useLargeArrays)
            {
                if (!a.IsLarge && !a.IsConstant && offa < Int32.MaxValue)
                {
                    ComplexForward(a.ToArray(), (int)offa);
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
                        (2 * nl).cftbsub(ref a, offa, ref ipl, nwl, ref wl);
                        break;
                    case Plans.MIXED_RADIX:
                        cfftf(a, offa, -1);
                        break;
                    case Plans.BLUESTEIN:
                        bluestein_complex(a, offa, -1);
                        break;
                }
            }
        }
    }
}
