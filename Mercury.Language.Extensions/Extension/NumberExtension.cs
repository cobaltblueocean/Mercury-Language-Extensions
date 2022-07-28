// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cern.Jet.Math;
using Mercury.Language.Log;

namespace System
{
    public static class NumberExtension
    {


        // Coefficients for the Taylor expansion of (e^x-1)/x and its first two derivatives
        private static double[] COEFF1;
        private static double[] COEFF2;
        private static double[] COEFF3;
        
        static NumberExtension()
        {
            COEFF1 = new double[] { 1d / 24, 1d / 6, 1d / 2, 1d };
            COEFF2 = new double[] { 1d / 144, 1d / 30, 1d / 8, 1d / 3, 1d / 2 };
            COEFF3 = new double[] { 1d / 168, 1d / 36, 1d / 10, 1d / 4, 1d / 3 };
        }

        public static T To<T>(this int i) where T : struct
        {
            return (T)Convert.ChangeType((object)i, typeof(T));
        }

        public static T To<T>(this double i) where T : struct
        {
            return (T)Convert.ChangeType((object)i, typeof(T));
        }

        public static T To<T>(this Single i) where T : struct
        {
            return (T)Convert.ChangeType((object)i, typeof(T));
        }

        public static T To<T>(this long i) where T : struct
        {
            return (T)Convert.ChangeType((object)i, typeof(T));
        }

        public static T To<T>(this decimal i) where T : struct
        {
            return (T)Convert.ChangeType((object)i, typeof(T));
        }

        public static Boolean IsNumber<T>(this T value) where T : struct, IEquatable<T>, IFormattable
        {
            //return value is sbyte
            //    || value is byte
            //    || value is short
            //    || value is ushort
            //    || value is int
            //    || value is uint
            //    || value is long
            //    || value is ulong
            //    || value is float
            //    || value is double
            //    || value is decimal;

            return double.TryParse(value.ToString(), out double num);
        }


        public static double Arg(this System.Numerics.Complex z)
        {
            return Math2.Atan2(z.Imaginary, z.Real);
        }

        /// <summary>
        /// This is the Taylor expansion of $$\frac{\exp(x)-1}{x}$$ - note for $$|x| > 10^{-10}$$ the expansion is note used
        /// </summary>
        /// <param name="x">value</param>
        /// <returns>result</returns>
        public static double Epsilon(this double x)
        {
            if (System.Math.Abs(x) > 1e-10)
            {
                return Expm1(x) / x;
            }
            return Taylor(x, COEFF1);
        }

        /// <summary>
        /// This is the Taylor expansion of the first derivative of $$\frac{\exp(x)-1}{x}$$
        /// </summary>
        /// <param name="x">value</param>
        /// <returns>result</returns>
        public static double EpsilonP(this double x)
        {

            if (System.Math.Abs(x) > 1e-7)
            {
                return ((x - 1) * Expm1(x) + x) / x / x;
            }
            return Taylor(x, COEFF2);
        }

        /// <summary>
        /// This is the Taylor expansion of the second derivative of $$\frac{\exp(x)-1}{x}$$
        /// </summary>
        /// <param name="x">value</param>
        /// <returns>result</returns>
        public static double EpsilonPP(this double x)
        {

            if (System.Math.Abs(x) > 1e-5)
            {
                double x2 = x * x;
                double x3 = x * x2;
                return (Expm1(x) * (x2 - 2 * x + 2) + x2 - 2 * x) / x3;
            }
            return Taylor(x, COEFF3);
        }

        private static double Taylor(this double x, double[] coeff)
        {
            double sum = coeff[0];
            int n = coeff.Length;
            for (int i = 1; i < n; i++)
            {
                sum = coeff[i] + x * sum;
            }
            return sum;
        }

        public static Boolean IsPowerOf2(this Int32 n)
        {
            if ((n > 0) && ((n & (n - 1)) == 0))
                return true;
            else
                return false;
        }

        public static Boolean IsPowerOf2(this Int64 n)
        {
            if ((n > 0) && ((n & (n - 1)) == 0))
                return true;
            else
                return false;
        }

        public static int LeadingZeros(this int x)
        {
            const int numIntBits = sizeof(int) * 8; //compile time constant
                                                    //do the smearing
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            //count the ones
            x -= x >> 1 & 0x55555555;
            x = (x >> 2 & 0x33333333) + (x & 0x33333333);
            x = (x >> 4) + x & 0x0f0f0f0f;
            x += x >> 8;
            x += x >> 16;
            return numIntBits - (x & 0x0000003f); //subtract # of 1s from 32
        }

        public static int NextPowerOf2(this int x)
        {
            if (x < 0)
                throw new ArgumentException();

            return x == 0 ? 0 : 32 - LeadingZeros(x - 1);
        }

        public static int NextPowerOf2(this long x)
        {
            if (x < 0)
                throw new ArgumentException();

            return x == 0 ? 0 : 32 - LeadingZeros((int)(x - 1));
        }

        public static long GetRemainder(this Int32 n, int[] factors)
        {
            return GetRemainder((Int64)n, factors);
        }

        public static long GetRemainder(this Int64 n, int[] factors)
        {
            long[] longfactors = Array.ConvertAll<int, long>(factors,
                    delegate (int i)
                    {
                        return (long)i;
                    });

            return GetRemainder(n, longfactors);
        }

        public static long GetRemainder(this Int64 n, long[] factors)
        {
            long reminder = n;

            if (n <= 0)
            {
                throw new ArgumentException(Mercury.Language.LocalizedResources.Instance().n_MUST_BE_POSITIVE_INT);
            }

            for (int i = 0; i < factors.Length && reminder != 1L; i++)
            {
                long factor = factors[i];
                while ((reminder % factor) == 0)
                {
                    reminder /= factor;
                }
            }
            return reminder;
        }

        public static void MakeIPT(this int nw, ref long[] ip)
        {
            long j, l, m, m2, p, q;

            ip[2] = 0;
            ip[3] = 16;
            m = 2;
            for (l = nw; l > 32; l >>= 2)
            {
                m2 = m << 1;
                q = m2 << 3;
                for (j = m; j < m2; j++)
                {
                    p = ip[j] << 2;
                    ip[m + j] = p;
                    ip[m2 + j] = p + q;
                }
                m = m2;
            }
        }

        public static void MakeIPT(this long nw, ref long[] ip)
        {
            long j, l, m, m2, p, q;

            ip[2] = 0;
            ip[3] = 16;
            m = 2;
            for (l = nw; l > 32; l >>= 2)
            {
                m2 = m << 1;
                q = m2 << 3;
                for (j = m; j < m2; j++)
                {
                    p = ip[j] << 2;
                    ip[m + j] = p;
                    ip[m2 + j] = p + q;
                }
                m = m2;
            }
        }

        public static void MakeIPT(this int nw, ref LongLargeArray ipl)
        {
            long j, l, m, m2, p, q;

            ipl[2] = 0;
            ipl[3] = 16;
            m = 2;
            for (l = nw; l > 32; l >>= 2)
            {
                m2 = m << 1;
                q = m2 << 3;
                for (j = m; j < m2; j++)
                {
                    p = ipl[j] << 2;
                    ipl[m + j] = p;
                    ipl[m2 + j] = p + q;
                }
                m = m2;
            }
        }

        public static void MakeIPT(this int nw, ref int[] ip)
        {
            int j, l, m, m2, p, q;

            ip[2] = 0;
            ip[3] = 16;
            m = 2;
            for (l = nw; l > 32; l >>= 2)
            {
                m2 = m << 1;
                q = m2 << 3;
                for (j = m; j < m2; j++)
                {
                    p = ip[j] << 2;
                    ip[m + j] = p;
                    ip[m2 + j] = p + q;
                }
                m = m2;
            }
        }

        public static void MakeIPT(this long nw, ref LongLargeArray ipl)
        {
            long j, l, m, m2, p, q;

            ipl[2] = 0;
            ipl[3] = 16;
            m = 2;
            for (l = nw; l > 32; l >>= 2)
            {
                m2 = m << 1;
                q = m2 << 3;
                for (j = m; j < m2; j++)
                {
                    p = ipl[j] << 2;
                    ipl[m + j] = p;
                    ipl[m2 + j] = p + q;
                }
                m = m2;
            }
        }

        public static void MakeWT(this int nw, ref int[] ip, ref double[] w)
        {
            long[] lip = Array.ConvertAll<int, long>(ip,
            delegate (int i)
            {
                return (long)i;
            });

            MakeWT(nw, ref lip, ref w);

            ip = Array.ConvertAll<long, int>(lip,
            delegate (long i)
            {
                return (int)i;
            });
        }

        public static void MakeWT(this long nw, ref long[] ip, ref double[] w)
        {
            long j, nwh, nw0, nw1;
            double delta, wn4r, wk1r, wk1i, wk3r, wk3i;
            double delta2, deltaj, deltaj3;

            ip[0] = nw;
            ip[1] = 1;
            if (nw > 2)
            {
                nwh = nw >> 1;
                delta = 0.785398163397448278999490867136046290 / nwh;
                delta2 = delta * 2;
                wn4r = System.Math.Cos(delta * nwh);
                w[0] = 1;
                w[1] = wn4r;
                if (nwh == 4)
                {
                    w[2] = System.Math.Cos(delta2);
                    w[3] = System.Math.Sin(delta2);
                }
                else if (nwh > 4)
                {
                    nw.MakeIPT(ref ip);
                    w[2] = 0.5 / System.Math.Cos(delta2);
                    w[3] = 0.5 / System.Math.Cos(delta * 6);
                    for (j = 4; j < nwh; j += 4)
                    {
                        deltaj = delta * j;
                        deltaj3 = 3 * deltaj;
                        w[j] = System.Math.Cos(deltaj);
                        w[j + 1] = System.Math.Sin(deltaj);
                        w[j + 2] = System.Math.Cos(deltaj3);
                        w[j + 3] = -System.Math.Sin(deltaj3);
                    }
                }
                nw0 = 0;
                while (nwh > 2)
                {
                    nw1 = nw0 + nwh;
                    nwh >>= 1;
                    w[nw1] = 1;
                    w[nw1 + 1] = wn4r;
                    if (nwh == 4)
                    {
                        wk1r = w[nw0 + 4];
                        wk1i = w[nw0 + 5];
                        w[nw1 + 2] = wk1r;
                        w[nw1 + 3] = wk1i;
                    }
                    else if (nwh > 4)
                    {
                        wk1r = w[nw0 + 4];
                        wk3r = w[nw0 + 6];
                        w[nw1 + 2] = 0.5 / wk1r;
                        w[nw1 + 3] = 0.5 / wk3r;
                        for (j = 4; j < nwh; j += 4)
                        {
                            long idx1 = nw0 + 2 * j;
                            long idx2 = nw1 + j;
                            wk1r = w[idx1];
                            wk1i = w[idx1 + 1];
                            wk3r = w[idx1 + 2];
                            wk3i = w[idx1 + 3];
                            w[idx2] = wk1r;
                            w[idx2 + 1] = wk1i;
                            w[idx2 + 2] = wk3r;
                            w[idx2 + 3] = wk3i;
                        }
                    }
                    nw0 = nw1;
                }
            }
        }

        public static void MakeWT(this int nw, ref LongLargeArray ipl, ref DoubleLargeArray wl)
        {
            long j, nwh, nw0, nw1;
            double delta, wn4r, wk1r, wk1i, wk3r, wk3i;
            double delta2, deltaj, deltaj3;

            ipl[0] = nw;
            ipl[1] = 1L;
            if (nw > 2)
            {
                nwh = nw >> 1;
                delta = 0.785398163397448278999490867136046290 / nwh;
                delta2 = delta * 2;
                wn4r = System.Math.Cos(delta * nwh);
                wl[0] = 1;
                wl[1] = wn4r;
                if (nwh == 4)
                {
                    wl[2] = System.Math.Cos(delta2);
                    wl[3] = System.Math.Sin(delta2);
                }
                else if (nwh > 4)
                {
                    nw.MakeIPT(ref ipl);
                    wl[2] = 0.5 / System.Math.Cos(delta2);
                    wl[3] = 0.5 / System.Math.Cos(delta * 6);
                    for (j = 4; j < nwh; j += 4)
                    {
                        deltaj = delta * j;
                        deltaj3 = 3 * deltaj;
                        wl[j] = System.Math.Cos(deltaj);
                        wl[j + 1] = System.Math.Sin(deltaj);
                        wl[j + 2] = System.Math.Cos(deltaj3);
                        wl[j + 3] = -System.Math.Sin(deltaj3);
                    }
                }
                nw0 = 0;
                while (nwh > 2)
                {
                    nw1 = nw0 + nwh;
                    nwh >>= 1;
                    wl[nw1] = 1;
                    wl[nw1 + 1] = wn4r;
                    if (nwh == 4)
                    {
                        wk1r = wl[nw0 + 4];
                        wk1i = wl[nw0 + 5];
                        wl[nw1 + 2] = wk1r;
                        wl[nw1 + 3] = wk1i;
                    }
                    else if (nwh > 4)
                    {
                        wk1r = wl[nw0 + 4];
                        wk3r = wl[nw0 + 6];
                        wl[nw1 + 2] = 0.5 / wk1r;
                        wl[nw1 + 3] = 0.5 / wk3r;
                        for (j = 4; j < nwh; j += 4)
                        {
                            long idx1 = nw0 + 2 * j;
                            long idx2 = nw1 + j;
                            wk1r = wl[idx1];
                            wk1i = wl[idx1 + 1];
                            wk3r = wl[idx1 + 2];
                            wk3i = wl[idx1 + 3];
                            wl[idx2] = wk1r;
                            wl[idx2 + 1] = wk1i;
                            wl[idx2 + 2] = wk3r;
                            wl[idx2 + 3] = wk3i;
                        }
                    }
                    nw0 = nw1;
                }
            }
        }

        public static void MakeCT(this int nc, ref double[] c, int startc, ref int[] ip)
        {
            int j, nch;
            double delta, deltaj;

            ip[1] = nc;
            if (nc > 1)
            {
                nch = nc >> 1;
                delta = 0.785398163397448278999490867136046290 / nch;
                c[startc] = System.Math.Cos(delta * nch);
                c[startc + nch] = 0.5 * c[startc];
                for (j = 1; j < nch; j++)
                {
                    deltaj = delta * j;
                    c[startc + j] = 0.5 * System.Math.Cos(deltaj);
                    c[startc + nc - j] = 0.5 * System.Math.Sin(deltaj);
                }
            }
        }

        public static void MakeCT(this long nc, ref double[] c, long startc, ref long[] ipl)
        {
            long j, nch;
            double delta, deltaj;

            ipl[1] = nc;
            if (nc > 1)
            {
                nch = nc >> 1;
                delta = 0.785398163397448278999490867136046290 / nch;
                c[startc] = System.Math.Cos(delta * nch);
                c[startc + nch] = 0.5 * c[startc];
                for (j = 1; j < nch; j++)
                {
                    deltaj = delta * j;
                    c[startc + j] = 0.5 * System.Math.Cos(deltaj);
                    c[startc + nc - j] = 0.5 * System.Math.Sin(deltaj);
                }
            }
        }

        public static void MakeCT(this long nc, ref DoubleLargeArray c, long startc, ref LongLargeArray ipl)
        {
            long j, nch;
            double delta, deltaj;

            ipl[1] = nc;
            if (nc > 1)
            {
                nch = nc >> 1;
                delta = 0.785398163397448278999490867136046290 / nch;
                c[startc] = System.Math.Cos(delta * nch);
                c[startc + nch] = 0.5 * c[startc];
                for (j = 1; j < nch; j++)
                {
                    deltaj = delta * j;
                    c[startc + j] = 0.5 * System.Math.Cos(deltaj);
                    c[startc + nc - j] = 0.5 * System.Math.Sin(deltaj);
                }
            }
        }

        public static void MakeCT(this int nc, ref float[] c, int startc, ref int[] ip)
        {
            int j, nch;
            float delta, deltaj;

            ip[1] = nc;
            if (nc > 1)
            {
                nch = nc >> 1;
                delta = 0.785398163397448278999490867136046290f / nch;
                c[startc] = (float)System.Math.Cos(delta * nch);
                c[startc + nch] = 0.5f * c[startc];
                for (j = 1; j < nch; j++)
                {
                    deltaj = delta * j;
                    c[startc + j] = 0.5f * (float)System.Math.Cos(deltaj);
                    c[startc + nc - j] = 0.5f * (float)System.Math.Sin(deltaj);
                }
            }
        }

        public static void MakeCT(this long nc, ref FloatLargeArray c, long startc, ref LongLargeArray ipl)
        {
            long j, nch;
            float delta, deltaj;

            ipl[1] = nc;
            if (nc > 1)
            {
                nch = nc >> 1;
                delta = 0.785398163397448278999490867136046290f / nch;
                c[startc] = (float)System.Math.Cos(delta * nch);
                c[startc + nch] = 0.5f * c[startc];
                for (j = 1; j < nch; j++)
                {
                    deltaj = delta * j;
                    c[startc + j] = 0.5f * (float)System.Math.Cos(deltaj);
                    c[startc + nc - j] = 0.5f * (float)System.Math.Sin(deltaj);
                }
            }
        }

        private static sbyte[] NTZ_TABLE = {
        32,  0,  1, 12,  2,  6, -1, 13,   3, -1,  7, -1, -1, -1, -1, 14,
        10,  4, -1, -1,  8, -1, -1, 25,  -1, -1, -1, -1, -1, 21, 27, 15,
        31, 11,  5, -1, -1, -1, -1, -1,   9, -1, -1, 24, -1, -1, 20, 26,
        30, -1, -1, -1, -1, 23, -1, 19,  29, -1, 22, 18, 28, 17, 16, -1
    };

        /// <summary>
        /// Determines the number of trailing zeros in the specified integer after
        /// the {@link #lowestOneBit(int) lowest one bit}.
        /// </summary>
        /// <param name="i">the integer to examine.</param>
        /// <returns>the number of trailing zeros in {@code i}.</returns>
        public static int NumberOfTrailingZeros(this int i)
        {
            return NTZ_TABLE[Math2.LogicalRightShift(((i & -i) * 0x0450FBAF), 26)];
        }

        private static double SQRT_1_5 = 1.224744871391589; // Long bits 0x3ff3988e1409212eL.
        private static double SQRT_2 = 1.4142135623730951; // Long bits 0x3ff6a09e667f3bcdL.
        private static double SQRT_3 = 1.7320508075688772; // Long bits 0x3ffbb67ae8584caaL.
        private static double EXP_LIMIT_H = 709.782712893384; // Long bits 0x40862e42fefa39efL.
        private static double EXP_LIMIT_L = -745.1332191019411; // Long bits 0xc0874910d52d3051L.
        private static double CP = 0.9617966939259756; // Long bits 0x3feec709dc3a03fdL.
        private static double CP_H = 0.9617967009544373; // Long bits 0x3feec709e0000000L.
        private static double CP_L = -7.028461650952758e-9; // Long bits 0xbe3e2fe0145b01f5L.
        private static double LN2 = 0.6931471805599453; // Long bits 0x3fe62e42fefa39efL.
        private static double LN2_H = 0.6931471803691238; // Long bits 0x3fe62e42fee00000L.
        private static double LN2_L = 1.9082149292705877e-10; // Long bits 0x3dea39ef35793c76L.
        private static double INV_LN2 = 1.4426950408889634; // Long bits 0x3ff71547652b82feL.
        private static double INV_LN2_H = 1.4426950216293335; // Long bits 0x3ff7154760000000L.
        private static double EXPM1_Q1 = -3.33333333333331316428e-02; // Long bits  0xbfa11111111110f4L
        private static double EXPM1_Q2 = 1.58730158725481460165e-03; // Long bits  0x3f5a01a019fe5585L
        private static double EXPM1_Q3 = -7.93650757867487942473e-05; // Long bits  0xbf14ce199eaadbb7L
        private static double EXPM1_Q4 = 4.00821782732936239552e-06; // Long bits  0x3ed0cfca86e65239L
        private static double EXPM1_Q5 = -2.01099218183624371326e-07; // Long bits  0xbe8afdb76e09c32dL

        /// <summary>
        /// Returns <em>e</em><sup>x</sup> - 1.
        /// Special cases:
        /// <ul>
        /// <li>If the argument is NaN, the result is NaN.</li>
        /// <li>If the argument is positive infinity, the result is positive
        /// infinity</li>
        /// <li>If the argument is negative infinity, the result is -1.</li>
        /// <li>If the argument is zero, the result is zero.</li>
        /// </ul>
        /// 
        /// </summary>
        /// <param Name="x">the argument to <em>e</em><sup>x</sup> - 1.</param>
        /// <returns><em>e</em> raised to the power <code>x</code> minus one.</returns>
        /// <see cref="#Exp(double)"></see>
        public static double Expm1(double x)
        {
            // Method
            //   1d Argument reduction:
            //    Given x, find r and integer k such that
            //
            //            x = k * ln(2) + r,  |r| <= 0.5 * ln(2)
            //
            //  Here a correction term c will be computed to compensate
            //    the error in r when rounded to a floating-point number.
            //
            //   2d Approximating expm1(r) by a special rational function on
            //    the interval [0, 0.5 * ln(2)]:
            //    Since
            //        r*(Exp(r)+1)/(Exp(r)-1) = 2 + r^2/6 - r^4/360 + ...
            //    we define R1(r*r) by
            //        r*(Exp(r)+1)/(Exp(r)-1) = 2 + r^2/6 * R1(r*r)
            //    That is,
            //        R1(r**2) = 6/r *((Exp(r)+1)/(Exp(r)-1) - 2/r)
            //             = 6/r * ( 1 + 2.0*(1/(Exp(r)-1) - 1/r))
            //             = 1 - r^2/60 + r^4/2520 - r^6/100800 + ...
            //  We use a special Remes algorithm on [0, 0.347] to generate
            //     a polynomial of degree 5 in r*r to approximate R1d The
            //    maximum error of this polynomial approximation is bounded
            //    by 2**-61d In other words,
            //        R1(z) ~ 1.0 + Q1*z + Q2*z**2 + Q3*z**3 + Q4*z**4 + Q5*z**5
            //    where     Q1  =  -1.6666666666666567384E-2,
            //         Q2  =   3.9682539681370365873E-4,
            //         Q3  =  -9.9206344733435987357E-6,
            //         Q4  =   2.5051361420808517002E-7,
            //         Q5  =  -6.2843505682382617102E-9;
            //      (where z=r*r, and Q1 to Q5 are called EXPM1_Qx in the source)
            //    with error bounded by
            //        |                  5           |     -61
            //        | 1.0+Q1*z+...+Q5*z   -  R1(z) | <= 2
            //        |                              |
            //
            //    expm1(r) = Exp(r)-1 is then computed by the following
            //     specific way which minimize the accumulation rounding error:
            //                   2     3
            //                  r     r    [ 3 - (R1 + R1*r/2)  ]
            //          expm1(r) = r + --- + --- * [--------------------]
            //                      2     2    [ 6 - r*(3 - R1*r/2) ]
            //
            //    To compensate the error in the argument reduction, we use
            //        expm1(r+c) = expm1(r) + c + expm1(r)*c
            //               ~ expm1(r) + c + r*c
            //    Thus c+r*c will be added in as the correction terms for
            //    expm1(r+c)d Now rearrange the term to avoid optimization
            //     screw up:
            //                (      2                                    2 )
            //                ({  ( r    [ R1 -  (3 - R1*r/2) ]  )  }    r  )
            //     expm1(r+c)~r - ({r*(--- * [--------------------]-c)-c} - --- )
            //                    ({  ( 2    [ 6 - r*(3 - R1*r/2) ]  )  }    2  )
            //                      (                                             )
            //
            //           = r - E
            //   3d Scale back to obtain expm1(x):
            //    From step 1, we have
            //       expm1(x) = either 2^k*[expm1(r)+1] - 1
            //            = or     2^k*[expm1(r) + (1-2^-k)]
            //   4d Implementation notes:
            //    (A)d To save one multiplication, we scale the coefficient Qi
            //         to Qi*2^i, and replace z by (x^2)/2.
            //    (B)d To achieve maximum accuracy, we compute expm1(x) by
            //      (i)   if x < -56*ln2, return -1.0, (raise inexact if x!=inf)
            //      (ii)  if k=0, return r-E
            //      (iii) if k=-1, return 0.5*(r-E)-0.5
            //        (iv)    if k=1 if r < -0.25, return 2*((r+0.5)- E)
            //                      else         return  1.0+2.0*(r-E);
            //      (v)   if (k<-2||k>56) return 2^K(1-(E-r)) - 1 (or Exp(x)-1)
            //      (vi)  if k <= 20, return 2^K((1-2^-k)-(E-r)), else
            //      (vii) return 2^K(1-((E+2^-k)-r))

            Boolean negative = (x < 0);
            double y, hi, lo, c, t, e, hxs, hfx, r1;
            int k;

            long bits;
            ulong h_bits;
            ulong l_bits;

            c = 0.0;
            y = System.Math.Abs(x);

            bits = BitConverter.DoubleToInt64Bits(y);
            h_bits = GetHighDWord(bits);
            l_bits = GetLowDWord(bits);

            // handle special cases and large arguments
            if (h_bits >= 0x4043687aL)        // if |x| >= 56 * ln(2)
            {
                if (h_bits >= 0x40862e42L)    // if |x| >= EXP_LIMIT_H
                {
                    if (h_bits >= 0x7ff00000L)
                    {
                        if (((h_bits & 0x000fffffL) | (l_bits & 0xffffffffL)) != 0)
                            return x;                        // Exp(NaN) = NaN
                        else
                            return negative ? -1.0 : x;      // Exp({+-inf}) = {+inf, -1}
                    }

                    if (x > EXP_LIMIT_H)
                        return Double.PositiveInfinity;     // overflow
                }

                if (negative)                // x <= -56 * ln(2)
                    return -1.0;
            }

            // argument reduction
            if (h_bits > 0x3fd62e42L)        // |x| > 0.5 * ln(2)
            {
                if (h_bits < 0x3ff0a2b2L)    // |x| < 1.5 * ln(2)
                {
                    if (negative)
                    {
                        hi = x + LN2_H;
                        lo = -LN2_L;
                        k = -1;
                    }
                    else
                    {
                        hi = x - LN2_H;
                        lo = LN2_L;
                        k = 1;
                    }
                }
                else
                {
                    k = (int)(INV_LN2 * x + (negative ? -0.5 : 0.5));
                    t = k;
                    hi = x - t * LN2_H;
                    lo = t * LN2_L;
                }

                x = hi - lo;
                c = (hi - x) - lo;

            }
            else if (h_bits < 0x3c900000L)   // |x| < 2^-54 return x
                return x;
            else
                k = 0;

            // x is now in primary range
            hfx = 0.5 * x;
            hxs = x * hfx;
            r1 = 1.0 + hxs * (EXPM1_Q1
                 + hxs * (EXPM1_Q2
                     + hxs * (EXPM1_Q3
                 + hxs * (EXPM1_Q4
                 + hxs * EXPM1_Q5))));
            t = 3.0 - r1 * hfx;
            e = hxs * ((r1 - t) / (6.0 - x * t));

            if (k == 0)
            {
                return x - (x * e - hxs);    // c == 0
            }
            else
            {
                e = x * (e - c) - c;
                e -= hxs;

                if (k == -1)
                    return 0.5 * (x - e) - 0.5;

                if (k == 1)
                {
                    if (x < -0.25)
                        return -2.0 * (e - (x + 0.5));
                    else
                        return 1.0 + 2.0 * (x - e);
                }

                if (k <= -2 || k > 56)       // sufficient to return Exp(x) - 1
                {
                    y = 1.0 - (e - x);

                    bits = BitConverter.DoubleToInt64Bits(y);
                    h_bits = GetHighDWord(bits);
                    l_bits = GetLowDWord(bits);

                    h_bits += (ulong)(k << 20);     // add k to y's exponent

                    y = BuildDouble(l_bits, h_bits);

                    return y - 1.0;
                }

                t = 1.0;
                if (k < 20)
                {
                    bits = BitConverter.DoubleToInt64Bits(t);
                    h_bits = (ulong)(0x3ff00000L - (0x00200000L >> k));
                    l_bits = GetLowDWord(bits);

                    t = BuildDouble(l_bits, h_bits);      // t = 1 - 2^(-k)
                    y = t - (e - x);

                    bits = BitConverter.DoubleToInt64Bits(y);
                    h_bits = GetHighDWord(bits);
                    l_bits = GetLowDWord(bits);

                    h_bits += (ulong)(k << 20);     // add k to y's exponent

                    y = BuildDouble(l_bits, h_bits);
                }
                else
                {
                    bits = BitConverter.DoubleToInt64Bits(t);
                    h_bits = (ulong)((0x000003ffL - k) << 20);
                    l_bits = GetLowDWord(bits);

                    t = BuildDouble(l_bits, h_bits);      // t = 2^(-k)

                    y = x - (e + t);
                    y += 1.0;

                    bits = BitConverter.DoubleToInt64Bits(y);
                    h_bits = GetHighDWord(bits);
                    l_bits = GetLowDWord(bits);

                    h_bits += (ulong)(k << 20);     // add k to y's exponent

                    y = BuildDouble(l_bits, h_bits);
                }
            }

            return y;
        }

        /// <summary>
        /// Returns the lower two words of a long. This is intended to be
        /// used like this: 
        /// <code>getLowDWord(Double.doubleToLongBits(x))</code>.
        /// </summary>
        private static ulong GetLowDWord(long x)
        {
            return (ulong)(x & 0x00000000ffffffffL);
        }

        /// <summary>
        /// Returns the higher two words of a long. This is intended to be
        /// used like this:
        /// <code>getHighDWord(Double.doubleToLongBits(x))</code>.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static ulong GetHighDWord(long x)
        {
            return ((ulong)x & 0xffffffff00000000L) >> 32;    // Java is using 0xffffffff00000000L (ulong) since the data type is different.
        }

        private static double BuildDouble(ulong lowDWord, ulong highDWord)
        {
            return BitConverter.Int64BitsToDouble((long)((highDWord & 0xffffffffL) << 32) | (long)(lowDWord & 0xffffffffL));
        }
    }
}
