// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math Inc.
//
// Copyright (C) 2012 - present by System.Math Incd and the System.Math group of companies
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math;
using Mercury.Language;
using Mercury.Language.Properties;

namespace System
{
    /// <summary>
    /// Math2 Description
    /// Additionally, added Decimal math logics ported from CSharp-Helper-Classes (Ramin Rahimzada)
    /// </summary>
    /// <see cref="https://github.com/raminrahimzada/CSharp-Helper-Classes/blob/master/Math/DecimalMath/DecimalMath.cs"/>
    public static class Math2
    {
        #region Local Variables
        public static double TWO_PI = 2 * System.Math.PI;
        public static double PI_SQUARED = System.Math.PI * System.Math.PI;
        public static double E = 2850325.0 / 1048576.0 + 8.254840070411028747e-8;
        public static long EXP_BIT_MASK = 0x7FF0000000000000L;
        public static double PI = 105414357.0 / 33554432.0 + 1.984187159361080883e-9;
        public static long SIGN_BIT_MASK = unchecked((long)0x8000000000000000); //0x8000000000000000L;
        public static long SIGNIF_BIT_MASK = 0x000FFFFFFFFFFFFfL;
        public static double EXP_LIMIT_H = 709.782712893384;
        public static double EXP_LIMIT_L = -745.1332191019411;
        public static double LN2 = 0.6931471805599453;
        public static double LN2_H = 0.6931471803691238;
        public static double LN2_L = 1.9082149292705877e-10;
        public static double INV_LN2 = 1.4426950408889634;
        public static double INV_LN2_H = 1.4426950216293335;
        public static double INV_LN2_L = 1.9259629911266175e-8;

        public static double TWO_16 = 0x10000; // Long bits 0x40f0000000000000L.
        public static double TWO_20 = 0x100000; // Long bits 0x4130000000000000L.
        public static double TWO_24 = 0x1000000; // Long bits 0x4170000000000000L.
        public static double TWO_27 = 0x8000000; // Long bits 0x41a0000000000000L.
        public static double TWO_28 = 0x10000000; // Long bits 0x41b0000000000000L.
        public static double TWO_29 = 0x20000000; // Long bits 0x41c0000000000000L.
        public static double TWO_31 = 0x80000000L; // Long bits 0x41e0000000000000L.
        public static double TWO_49 = 0x2000000000000L; // Long bits 0x4300000000000000L.
        public static double TWO_52 = 0x10000000000000L; // Long bits 0x4330000000000000L.
        public static double TWO_54 = 0x40000000000000L; // Long bits 0x4350000000000000L.
        public static double TWO_57 = 0x200000000000000L; // Long bits 0x4380000000000000L.
        public static double TWO_60 = 0x1000000000000000L; // Long bits 0x43b0000000000000L.
        public static double TWO_64 = 1.8446744073709552e19; // Long bits 0x43f0000000000000L.
        public static double TWO_66 = 7.378697629483821e19; // Long bits 0x4410000000000000L.
        public static double TWO_1023 = 8.98846567431158e307; // Long bits 0x7fe0000000000000L.

        public static double L1 = 0.5999999999999946; // Long bits 0x3fe3333333333303L.
        public static double L2 = 0.4285714285785502; // Long bits 0x3fdb6db6db6fabffL.
        public static double L3 = 0.33333332981837743; // Long bits 0x3fd55555518f264dL.
        public static double L4 = 0.272728123808534; // Long bits 0x3fd17460a91d4101L.
        public static double L5 = 0.23066074577556175; // Long bits 0x3fcd864a93c9db65L.
        public static double L6 = 0.20697501780033842; // Long bits 0x3fca7e284a454eefL.
        public static double P1 = 0.16666666666666602; // Long bits 0x3fc555555555553eL.
        public static double P2 = -2.7777777777015593e-3; // Long bits 0xbf66c16c16bebd93L.
        public static double P3 = 6.613756321437934e-5; // Long bits 0x3f11566aaf25de2cL.
        public static double P4 = -1.6533902205465252e-6; // Long bits 0xbebbbd41c5d26bf1L.
        public static double P5 = 4.1381367970572385e-8; // Long bits 0x3e66376972bea4d0L.
        public static double DP_H = 0.5849624872207642; // Long bits 0x3fe2b80340000000L.
        public static double DP_L = 1.350039202129749e-8; // Long bits 0x3e4cfdeb43cfd006L.
        public static double OVT = 8.008566259537294e-17; // Long bits 0x3c971547652b82feL.

        /** Archimede's constant PI, ratio of circle circumference to diameterd */
        /** Napier's constant e, base of the natural logarithmd */
        /** Index of exp(0) in the array of int exponentialsd */

        private static double[] CBRTTWO = { 0.6299605249474366,
                                            0.7937005259840998,
                                            1.0,
                                            1.2599210498948732,
                                            1.5874010519681994 };

        private static double[] COSINE_TABLE_A =
            {
        +1.0d,
        +0.9921976327896118d,
        +0.9689123630523682d,
        +0.9305076599121094d,
        +0.8775825500488281d,
        +0.8109631538391113d,
        +0.7316888570785522d,
        +0.6409968137741089d,
        +0.5403022766113281d,
        +0.4311765432357788d,
        +0.3153223395347595d,
        +0.19454771280288696d,
        +0.07073719799518585d,
        -0.05417713522911072d,
    };

        private static double[] COSINE_TABLE_B =
            {
        +0.0d,
        +3.4439717236742845E-8d,
        +5.865827662008209E-8d,
        -3.7999795083850525E-8d,
        +1.184154459111628E-8d,
        -3.43338934259355E-8d,
        +1.1795268640216787E-8d,
        +4.438921624363781E-8d,
        +2.925681159240093E-8d,
        -2.6437112632041807E-8d,
        +2.2860509143963117E-8d,
        -4.813899778443457E-9d,
        +3.6725170580355583E-9d,
        +2.0217439756338078E-10d,
    };

        private static double[] EIGHTHS = { 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1.0, 1.125, 1.25, 1.375, 1.5, 1.625 };
        private static int EXP_FRAC_TABLE_LEN = 1025;
        private static int EXP_INT_TABLE_MAX_INDEX = 750;
        private static int EXP_INT_TABLE_LEN = EXP_INT_TABLE_MAX_INDEX * 2;
        /** Length of the array of int exponentialsd */
        /** Logarithm table Lengthd */
        private static double F_1_11 = 1d / 11d;
        private static double F_1_13 = 1d / 13d;
        private static double F_1_15 = 1d / 15d;
        private static double F_1_17 = 1d / 17d;
        private static double F_1_2 = 1d / 2d;
        private static double F_1_3 = 1d / 3d;
        private static double F_1_4 = 1d / 4d;
        private static double F_1_5 = 1d / 5d;
        private static double F_1_7 = 1d / 7d;
        private static double F_1_9 = 1d / 9d;
        private static double F_11_12 = 11d / 12d;
        private static double F_13_14 = 13d / 14d;
        private static double F_15_16 = 15d / 16d;
        private static double F_3_4 = 3d / 4d;
        private static double F_5_6 = 5d / 6d;
        private static double F_7_8 = 7d / 8d;
        private static double F_9_10 = 9d / 10d;

        private static double[] FACT = new double[]
        {
        +1.0d,                        // 0
        +1.0d,                        // 1
        +2.0d,                        // 2
        +6.0d,                        // 3
        +24.0d,                       // 4
        +120.0d,                      // 5
        +720.0d,                      // 6
        +5040.0d,                     // 7
        +40320.0d,                    // 8
        +362880.0d,                   // 9
        +3628800.0d,                  // 10
        +39916800.0d,                 // 11
        +479001600.0d,                // 12
        +6227020800.0d,               // 13
        +87178291200.0d,              // 14
        +1307674368000.0d,            // 15
        +20922789888000.0d,           // 16
        +355687428096000.0d,          // 17
        +6402373705728000.0d,         // 18
        +121645100408832000.0d,       // 19
        };

        private static long HEX_40000000 = 0x40000000L;
        private static long IMPLICIT_HIGH_BIT = 0x0010000000000000L;
        private static double LN_2_A = 0.693147063255310059;
        private static double LN_2_B = 1.17304635250823482e-7;

        private static double[][] LN_HI_PREC_COEF = new double[6, 2] {{1.0, -6.032174644509064E-23},
        {-0.25, -0.25},
        {0.3333333134651184, 1.9868161777724352E-8},
        {-0.2499999701976776, -2.957007209750105E-8},
        {0.19999954104423523, 1.5830993332061267E-10},
        {-0.16624879837036133, -2.6033824355191673E-8}
    }.ToJagged();

        private static int LN_MANT_LEN = 1024;
        /** Exponential fractions table Lengthd */
        // 0, 1/1024, ..d 1024/1024

        /** StrictSystem.Math.Log(Double.MaxValue): {@value} */

        private static double[][] LN_QUICK_COEF = new double[,]{
        {1.0, 5.669184079525E-24},
        {-0.25, -0.25},
        {0.3333333134651184, 1.986821492305628E-8},
        {-0.25, -6.663542893624021E-14},
        {0.19999998807907104, 1.1921056801463227E-8},
        {-0.1666666567325592, -7.800414592973399E-9},
        {0.1428571343421936, 5.650007086920087E-9},
        {-0.12502530217170715, -7.44321345601866E-11},
        {0.11113807559013367, 9.219544613762692E-9},
    }.ToJagged();

        private static double[][] LN_SPLIT_COEF = new double[,]{
        {2.0, 0.0},
        {0.6666666269302368, 3.9736429850260626E-8},
        {0.3999999761581421, 2.3841857910019882E-8},
        {0.2857142686843872, 1.7029898543501842E-8},
        {0.2222222089767456, 1.3245471311735498E-8},
        {0.1818181574344635, 2.4384203044354907E-8},
        {0.1538461446762085, 9.140260083262505E-9},
        {0.13333332538604736, 9.220590270857665E-9},
        {0.11764700710773468, 1.2393345855018391E-8},
        {0.10526403784751892, 8.251545029714408E-9},
        {0.0952233225107193, 1.2675934823758863E-8},
        {0.08713622391223907, 1.1430250008909141E-8},
        {0.07842259109020233, 2.404307984052299E-9},
        {0.08371849358081818, 1.176342548272881E-8},
        {0.030589580535888672, 1.2958646899018938E-9},
        {0.14982303977012634, 1.225743062930824E-8},
    }.ToJagged();

        private static double LOG_MAX_VALUE = System.Math.Log(Double.MaxValue);

        /** log(2)(high bits)d */
        /** log(2)(low bits)d */
        /** Coefficients for log, when input 0.99 < x < 1.01d */
        /** Coefficients for log in the range of 1.0 < x < 1.0 + 2^-10d */
        /** Sine, Cosine, Tangent tables are for 0, 1/8, 2/8, ..d 13/8 = PI/2 approxd */
        private static long MASK_30BITS = -1L - (HEX_40000000 - 1);
        private static long MASK_DOUBLE_EXPONENT = 0x7ff0000000000000L;
        private static long MASK_DOUBLE_MANTISSA = 0x000fffffffffffffL;
        private static int MASK_NON_SIGN_INT = 0x7fffffff;
        private static long MASK_NON_SIGN_LONG = 0x7fffffffffffffffL;

        private static long[] PI_O_4_BITS = new long[] {
        (0xc90fdaa2L << 32) | 0x2168c234L,
        (0xc4c6628bL << 32) | 0x80dc1cd1L };

        private static long[] RECIP_2PI = new long[] {
        (0x28be60dbL << 32) | 0x9391054aL,
        (0x7f09d5f4L << 32) | 0x7d4d3770L,
        (0x36d8a566L << 32) | 0x4f10e410L,
        (0x7f9458eaL << 32) | 0xf7aef158L,
        (0x6dc91b8eL << 32) | 0x909374b8L,
        (0x01924bbaL << 32) | 0x82746487L,
        (0x3f877ac7L << 32) | 0x2c4a69cfL,
        (0xba208d7dL << 32) | 0x4baed121L,
        (0x3a671c09L << 32) | 0xad17df90L,
        (0x4e64758eL << 32) | 0x60d4ce7dL,
        (0x272117e2L << 32) | 0xef7e4a0eL,
        (0xc7fe25ffL << 32) | 0xf7816603L,
        (0xfbcbc462L << 32) | 0xd6829b47L,
        (0xdb4d9fb3L << 32) | 0xc9f2c26dL,
        (0xd3d18fd9L << 32) | 0xa797fa8bL,
        (0x5d49eeb1L << 32) | 0xfaf97c5eL,
        (0xcf41ce7dL << 32) | 0xe294a4baL,
         0x9afed7ecL << 32  };

        private static double[] SINE_TABLE_A =
            {
        +0.0d,
        +0.1246747374534607d,
        +0.24740394949913025d,
        +0.366272509098053d,
        +0.4794255495071411d,
        +0.5850973129272461d,
        +0.6816387176513672d,
        +0.7675435543060303d,
        +0.8414709568023682d,
        +0.902267575263977d,
        +0.9489846229553223d,
        +0.9808930158615112d,
        +0.9974949359893799d,
        +0.9985313415527344d,
    };

        private static double[] SINE_TABLE_B =
            {
        +0.0d,
        -4.068233003401932E-9d,
        +9.755392680573412E-9d,
        +1.9987994582857286E-8d,
        -1.0902938113007961E-8d,
        -3.9986783938944604E-8d,
        +4.23719669792332E-8d,
        -5.207000323380292E-8d,
        +2.800552834259E-8d,
        +1.883511811213715E-8d,
        -3.5997360512765566E-9d,
        +4.116164446561962E-8d,
        +5.0614674548127384E-8d,
        -1.0129027912496858E-9d,
    };

        //private static int SINE_TABLE_LEN = 14;

        /** Sine table (high bits)d */
        /** Sine table (low bits)d */
        /** Cosine table (high bits)d */
        /** Cosine table (low bits)d */
        /** Tangent table, used by atan()(high bits)d */

        private static double[] TANGENT_TABLE_A =
            {
        +0.0d,
        +0.1256551444530487d,
        +0.25534194707870483d,
        +0.3936265707015991d,
        +0.5463024377822876d,
        +0.7214844226837158d,
        +0.9315965175628662d,
        +1.1974215507507324d,
        +1.5574076175689697d,
        +2.092571258544922d,
        +3.0095696449279785d,
        +5.041914939880371d,
        +14.101419448852539d,
        -18.430862426757812d,
    };

        /** Tangent table, used by atan()(low bits)d */

        private static double[] TANGENT_TABLE_B =
            {
        +0.0d,
        -7.877917738262007E-9d,
        -2.5857668567479893E-8d,
        +5.2240336371356666E-9d,
        +5.206150291559893E-8d,
        +1.8307188599677033E-8d,
        -5.7618793749770706E-8d,
        +7.848361555046424E-8d,
        +1.0708593250394448E-7d,
        +1.7827257129423813E-8d,
        +2.893485277253286E-8d,
        +3.1660099222737955E-7d,
        +4.983191803254889E-7d,
        -3.356118100840571E-7d,
    };

        /** Bits of 1/(2*pi), need for reducePayneHanek()d */
        /** Bits of pi/4, need for reducePayneHanek()d */
        /** Eighths.
         * This is used by sinQ, because its faster to do a table lookup than
         * a multiply in this time-critical routine
         */
        /** Table of 2^((n+2)/3) */
        /*
         *  There are 52 bits in the mantissa of a double.
         *  For additional precision, the code splits double numbers into two parts,
         *  by clearing the low order 30 bits if possible, and then performs the arithmetic
         *  on each half separately.
         */

        /**
         * 0x40000000 - used to split a double into two parts, both with the low order bits cleared.
         * Equivalent to 2^30.
         */
        // 1073741824L

        /** Mask used to clear low order 30 bits */
        // 0xFFFFFFFFC0000000L;

        /** Mask used to clear the non-sign part of an intd */
        /** Mask used to clear the non-sign part of a longd */
        /** Mask used to extract exponent from double bitsd */
        /** Mask used to extract mantissa from double bitsd */
        /** Mask used to add implicit high order bit for normalized doubled */
        /** 2^52 - double numbers this large must be integral (no fraction) or NaN or Infinite */
        private static double TWO_POWER_52 = 4503599627370496.0;

        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Constant: {@value}d */
        /** Coefficients for slowLog. */


        private static Random rnd;

        /// <summary>
        /// Double's Mini Normal Value
        /// </summary>
        public static double DOUBLE_MIN_NORMAL = 2.2250738585072014E-308;
        #endregion Local Variables

        static Math2()
        {
            rnd = new Random();
        }

        /// <summary>
        /// Smallest positive number such that 1 - EPSILON is not numerically equal to 1.
        /// </summary>
        public const double DoubleEpsilon = 1.1102230246251565E-16;  //0x1.0p-53;

        public const double DoubleSafeMin = 2.2250738585072014E-308;   //0x1.0p-1022;

        #region "Decimal Math Helper Variables"
        /// <summary>
        /// represents PI
        /// </summary>
        public const decimal Pi = 3.14159265358979323846264338327950288419716939937510M;

        /// <summary>
        /// represents PI
        /// </summary>
        public const decimal DecimalEpsilon = 0.0000000000000000001M;

        /// <summary>
        /// represents 2*PI
        /// </summary>
        private const decimal PIx2 = 6.28318530717958647692528676655900576839433879875021M;

        /// <summary>
        /// represents E
        /// </summary>
        public const decimal DE = 2.7182818284590452353602874713526624977572470936999595749M;

        /// <summary>
        /// represents PI/2
        /// </summary>
        private const decimal PIdiv2 = 1.570796326794896619231321691639751442098584699687552910487M;

        /// <summary>
        /// represents PI/4
        /// </summary>
        private const decimal PIdiv4 = 0.785398163397448309615660845819875721049292349843776455243M;

        /// <summary>
        /// represents 1.0/E
        /// </summary>
        private const decimal Einv = 0.3678794411714423215955237701614608674458111310317678M;

        /// <summary>
        /// log(10,E) factor
        /// </summary>
        private const decimal Log10Inv = 0.434294481903251827651128918916605082294397005803666566114M;

        /// <summary>
        /// Zero
        /// </summary>
        public const decimal Zero = 0.0M;

        /// <summary>
        /// One
        /// </summary>
        public const decimal One = 1.0M;

        /// <summary>
        /// Represents 0.5M
        /// </summary>
        private const decimal Half = 0.5M;

        /// <summary>
        /// Max iterations count in Taylor series
        /// </summary>
        private const int MaxIteration = 100;
        #endregion

        #region trigonometric function

        public static double Acos(double x)
        {
            if (x > 1.0 || x < -1.0)
            {
                return Double.NaN;
            }

            if (x == -1.0)
            {
                return System.Math.PI;
            }

            if (x == 1.0)
            {
                return 0.0;
            }

            if (x == 0)
            {
                return System.Math.PI / 2.0;
            }

            /* Compute acos(x) = atan(sqrt(1-x*x)/x) */

            /* Split x */
            double temp = x * HEX_40000000;
            double xa = x + temp - temp;
            double xb = x - xa;

            /* Square it */
            double ya = xa * xa;
            double yb = xa * xb * 2.0 + xb * xb;

            /* Subtract from 1 */
            ya = -ya;
            yb = -yb;

            double za = 1.0 + ya;
            double zb = -(za - 1.0 - ya);

            temp = za + yb;
            zb += -(temp - za - yb);
            za = temp;

            /* Square root */
            double y = SquareRoot(za);
            temp = y * HEX_40000000;
            ya = y + temp - temp;
            yb = y - ya;

            /* Extend precision of sqrt */
            yb += (za - ya * ya - 2 * ya * yb - yb * yb) / (2.0 * y);

            /* Contribution of zb to sqrt */
            yb += zb / (2.0 * y);
            y = ya + yb;
            yb = -(y - ya - yb);

            // Compute ratio r = y/x
            double r = y / x;

            // Did r overflow?
            if (Double.IsInfinity(r))
            { // x is effectively zero
                return System.Math.PI / 2; // so return the appropriate value
            }

            double ra = DoubleHighPart(r);
            double rb = r - ra;

            rb += (y - ra * xa - ra * xb - rb * xa - rb * xb) / x;  // Correct for rounding in division
            rb += yb / x;  // Add in effect additional bits of sqrt.

            temp = ra + rb;
            rb = -(temp - ra - rb);
            ra = temp;

            return Atan(ra, rb, x < 0);
        }

        public static double Acosh(double a)
        {
            return System.Math.Log(a + System.Math.Sqrt(a * a - 1));
        }

        public static double Asin(double x)
        {
            if (x > 1.0 || x < -1.0)
            {
                return Double.NaN;
            }

            if (x == 1.0)
            {
                return System.Math.PI / 2.0;
            }

            if (x == -1.0)
            {
                return -System.Math.PI / 2.0;
            }

            if (x == 0.0)
            { // Matches +/- 0.0; return correct sign
                return x;
            }

            /* Compute asin(x) = atan(x/sqrt(1-x*x)) */

            /* Split x */
            double temp = x * HEX_40000000;
            double xa = x + temp - temp;
            double xb = x - xa;

            /* Square it */
            double ya = xa * xa;
            double yb = xa * xb * 2.0 + xb * xb;

            /* Subtract from 1 */
            ya = -ya;
            yb = -yb;

            double za = 1.0 + ya;
            double zb = -(za - 1.0 - ya);

            temp = za + yb;
            zb += -(temp - za - yb);
            za = temp;

            /* Square root */
            double y;
            y = SquareRoot(za);
            temp = y * HEX_40000000;
            ya = y + temp - temp;
            yb = y - ya;

            /* Extend precision of sqrt */
            yb += (za - ya * ya - 2 * ya * yb - yb * yb) / (2.0 * y);

            /* Contribution of zb to sqrt */
            double dx = zb / (2.0 * y);

            // Compute ratio r = x/y
            double r = x / y;
            temp = r * HEX_40000000;
            double ra = r + temp - temp;
            double rb = r - ra;

            rb += (x - ra * ya - ra * yb - rb * ya - rb * yb) / y;  // Correct for rounding in division
            rb += -x * dx / y / y;  // Add in effect additional bits of sqrt.

            temp = ra + rb;
            rb = -(temp - ra - rb);
            ra = temp;

            return Atan(ra, rb, false);
        }

        public static double Asinh(double a)
        {
            Boolean negative = false;
            if (a < 0)
            {
                negative = true;
                a = -a;
            }

            double absAsinh;
            if (a > 0.167)
            {
                absAsinh = System.Math.Log(System.Math.Sqrt(a * a + 1) + a);
            }
            else
            {
                double a2 = a * a;
                if (a > 0.097)
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * (F_1_9 - a2 * (F_1_11 - a2 * (F_1_13 - a2 * (F_1_15 - a2 * F_1_17 * F_15_16) * F_13_14) * F_11_12) * F_9_10) * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                }
                else if (a > 0.036)
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * (F_1_9 - a2 * (F_1_11 - a2 * F_1_13 * F_11_12) * F_9_10) * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                }
                else if (a > 0.0036)
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * F_1_9 * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                }
                else
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * F_1_5 * F_3_4) * F_1_2);
                }
            }

            return negative ? -absAsinh : absAsinh;
        }

        public static double Atan(double x)
        {
            return Atan(x, 0.0, false);
        }

        public static double Atan2(double y, double x)
        {
            if (y == 0)
            {
                double result1 = x * y;
                double invx = 1d / x;
                double invy = 1d / y;

                if (invx == 0)
                { // X is infinite
                    if (x > 0)
                    {
                        return y; // return +/- 0.0
                    }
                    else
                    {
                        return CopySign(System.Math.PI, y);
                    }
                }

                if (x < 0 || invx < 0)
                {
                    if (y < 0 || invy < 0)
                    {
                        return -System.Math.PI;
                    }
                    else
                    {
                        return System.Math.PI;
                    }
                }
                else
                {
                    return result1;
                }
            }

            // y cannot now be zero

            if (y == Double.PositiveInfinity)
            {
                if (x == Double.PositiveInfinity)
                {
                    return System.Math.PI * F_1_4;
                }

                if (x == Double.NegativeInfinity)
                {
                    return System.Math.PI * F_3_4;
                }

                return System.Math.PI * F_1_2;
            }

            if (y == Double.NegativeInfinity)
            {
                if (x == Double.PositiveInfinity)
                {
                    return -System.Math.PI * F_1_4;
                }

                if (x == Double.NegativeInfinity)
                {
                    return -System.Math.PI * F_3_4;
                }

                return -System.Math.PI * F_1_2;
            }

            if (x == Double.PositiveInfinity)
            {
                if (y > 0 || 1 / y > 0)
                {
                    return 0d;
                }

                if (y < 0 || 1 / y < 0)
                {
                    return -0d;
                }
            }

            if (x == Double.NegativeInfinity)
            {
                if (y > 0.0 || 1 / y > 0.0)
                {
                    return System.Math.PI;
                }

                if (y < 0 || 1 / y < 0)
                {
                    return -System.Math.PI;
                }
            }

            // Neither y nor x can be infinite or NAN here

            if (x == 0)
            {
                if (y > 0 || 1 / y > 0)
                {
                    return System.Math.PI * F_1_2;
                }

                if (y < 0 || 1 / y < 0)
                {
                    return -System.Math.PI * F_1_2;
                }
            }

            // Compute ratio r = y/x
            double r = y / x;
            if (Double.IsInfinity(r))
            { // bypass calculations that can create NaN
                return Atan(r, 0, x < 0);
            }

            double ra = DoubleHighPart(r);
            double rb = r - ra;

            // Split x
            double xa = DoubleHighPart(x);
            double xb = x - xa;

            rb += (y - ra * xa - ra * xb - rb * xa - rb * xb) / x;

            double temp = ra + rb;
            rb = -(temp - ra - rb);
            ra = temp;

            if (ra == 0)
            { // Fix up the sign so atan works correctly
                ra = CopySign(0d, y);
            }

            // Call atan
            double result = Atan(ra, rb, x < 0);

            return result;
        }

        public static double Atanh(double a)
        {
            Boolean negative = false;
            if (a < 0)
            {
                negative = true;
                a = -a;
            }

            double absAtanh;
            if (a > 0.15)
            {
                absAtanh = 0.5 * System.Math.Log((1 + a) / (1 - a));
            }
            else
            {
                double a2 = a * a;
                if (a > 0.087)
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * (F_1_9 + a2 * (F_1_11 + a2 * (F_1_13 + a2 * (F_1_15 + a2 * F_1_17))))))));
                }
                else if (a > 0.031)
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * (F_1_9 + a2 * (F_1_11 + a2 * F_1_13))))));
                }
                else if (a > 0.003)
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * F_1_9))));
                }
                else
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * F_1_5));
                }
            }

            return negative ? -absAtanh : absAtanh;
        }

        public static double Cos(double x)
        {
            int quadrant = 0;

            /* Take absolute value of the input */
            double xa = x;
            if (x < 0)
            {
                xa = -xa;
            }

            if (xa == Double.PositiveInfinity)
            {
                return Double.NaN;
            }

            /* Perform any argument reduction */
            double xb = 0;
            if (xa > 3294198.0)
            {
                // PI * (2**20)
                // Argument too big for CodyWaite reductiond  Must use
                // PayneHanek.
                double[] reduceResults = new double[3];
                ReducePayneHanek(xa, out reduceResults);
                quadrant = ((int)reduceResults[0]) & 3;
                xa = reduceResults[1];
                xb = reduceResults[2];
            }
            else if (xa > 1.5707963267948966)
            {
                CodyWaite cw = new CodyWaite(xa);
                quadrant = cw.K & 3;
                xa = cw.RemA;
                xb = cw.RemB;
            }

            //if (negative)
            //  quadrant = (quadrant + 2) % 4;

            switch (quadrant)
            {
                case 0:
                    return CosQ(xa, xb);

                case 1:
                    return -SinQ(xa, xb);

                case 2:
                    return -CosQ(xa, xb);

                case 3:
                    return SinQ(xa, xb);

                default:
                    return Double.NaN;
            }
        }

        public static double Cosh(double x)
        {
            // cosh[z] = (exp(z) + exp(-z))/2

            // for numbers with magnitude 20 or so,
            // exp(-z) can be ignored in comparison with exp(z)

            if (x > 20)
            {
                if (x >= LOG_MAX_VALUE)
                {
                    // Avoid overflow (MATH-905).
                    double t = Math.Exp(0.5 * x);
                    return (0.5 * t) * t;
                }
                else
                {
                    return 0.5 * Math.Exp(x);
                }
            }
            else if (x < -20)
            {
                if (x <= -LOG_MAX_VALUE)
                {
                    // Avoid overflow (MATH-905).
                    double t = Math.Exp(-0.5 * x);
                    return (0.5 * t) * t;
                }
                else
                {
                    return 0.5 * Math.Exp(-x);
                }
            }

            var hiPrec = new double[2];
            if (x < 0.0)
            {
                x = -x;
            }
            Exp(x, 0.0, hiPrec);

            double ya = hiPrec[0] + hiPrec[1];
            double yb = -(ya - hiPrec[0] - hiPrec[1]);

            double temp = ya * HEX_40000000;
            double yaa = ya + temp - temp;
            double yab = ya - yaa;

            // recip = 1/y
            double recip = 1.0 / ya;
            temp = recip * HEX_40000000;
            double recipa = recip + temp - temp;
            double recipb = recip - recipa;

            // Correct for rounding in division
            recipb += (1.0 - yaa * recipa - yaa * recipb - yab * recipa - yab * recipb) * recip;
            // Account for yb
            recipb += -yb * recip * recip;

            // y = y + 1/y
            temp = ya + recipa;
            yb += -(temp - ya - recipa);
            ya = temp;
            temp = ya + recipb;
            yb += -(temp - ya - recipb);
            ya = temp;

            double result = ya + yb;
            result *= 0.5;
            return result;
        }

        public static double Sin(double x)
        {
            Boolean negative = false;
            int quadrant = 0;
            double xa;
            double xb = 0.0;

            /* Take absolute value of the input */
            xa = x;
            if (x < 0)
            {
                negative = true;
                xa = -xa;
            }

            /* Check for zero and negative zero */
            if (xa == 0.0)
            {
                long bits = BitConverter.DoubleToInt64Bits(x);
                if (bits < 0)
                {
                    return -0.0;
                }
                return 0.0;
            }

            if (Double.IsNaN(xa) || xa == Double.PositiveInfinity)
            {
                return Double.NaN;
            }

            /* Perform any argument reduction */
            if (xa > 3294198.0)
            {
                // PI * (2**20)
                // Argument too big for CodyWaite reductiond  Must use
                // PayneHanek.
                double[] reduceResults = new double[3];
                ReducePayneHanek(xa, out reduceResults);
                quadrant = ((int)reduceResults[0]) & 3;
                xa = reduceResults[1];
                xb = reduceResults[2];
            }
            else if (xa > 1.5707963267948966)
            {
                CodyWaite cw = new CodyWaite(xa);
                quadrant = cw.K & 3;
                xa = cw.RemA;
                xb = cw.RemB;
            }

            if (negative)
            {
                quadrant ^= 2;  // Flip bit 1
            }

            switch (quadrant)
            {
                case 0:
                    return SinQ(xa, xb);

                case 1:
                    return CosQ(xa, xb);

                case 2:
                    return -SinQ(xa, xb);

                case 3:
                    return -CosQ(xa, xb);

                default:
                    return Double.NaN;
            }
        }

        public static double Sinh(double x)
        {
            Boolean negate = false;
            // sinh[z] = (exp(z) - exp(-z) / 2

            // for values of z larger than about 20,
            // exp(-z) can be ignored in comparison with exp(z)

            if (x > 20)
            {
                if (x >= LOG_MAX_VALUE)
                {
                    // Avoid overflow (MATH-905).
                    double t = Math.Exp(0.5 * x);
                    return (0.5 * t) * t;
                }
                else
                {
                    return 0.5 * Math.Exp(x);
                }
            }
            else if (x < -20)
            {
                if (x <= -LOG_MAX_VALUE)
                {
                    // Avoid overflow (MATH-905).
                    double t = Math.Exp(-0.5 * x);
                    return (-0.5 * t) * t;
                }
                else
                {
                    return -0.5 * Math.Exp(-x);
                }
            }

            if (x == 0)
            {
                return x;
            }

            if (x < 0.0)
            {
                x = -x;
                negate = true;
            }

            double result;

            if (x > 0.25)
            {
                var hiPrec = new double[2];
                Exp(x, 0.0, hiPrec);

                double ya = hiPrec[0] + hiPrec[1];
                double yb = -(ya - hiPrec[0] - hiPrec[1]);

                double temp = ya * HEX_40000000;
                double yaa = ya + temp - temp;
                double yab = ya - yaa;

                // recip = 1/y
                double recip = 1.0 / ya;
                temp = recip * HEX_40000000;
                double recipa = recip + temp - temp;
                double recipb = recip - recipa;

                // Correct for rounding in division
                recipb += (1.0 - yaa * recipa - yaa * recipb - yab * recipa - yab * recipb) * recip;
                // Account for yb
                recipb += -yb * recip * recip;

                recipa = -recipa;
                recipb = -recipb;

                // y = y + 1/y
                temp = ya + recipa;
                yb += -(temp - ya - recipa);
                ya = temp;
                temp = ya + recipb;
                yb += -(temp - ya - recipb);
                ya = temp;

                result = ya + yb;
                result *= 0.5;
            }
            else
            {
                var hiPrec = new double[2];
                Expm1(x, hiPrec);

                double ya = hiPrec[0] + hiPrec[1];
                double yb = -(ya - hiPrec[0] - hiPrec[1]);

                /* Compute expm1(-x) = -expm1(x) / (expm1(x) + 1) */
                double denom = 1.0 + ya;
                double denomr = 1.0 / denom;
                double denomb = -(denom - 1.0 - ya) + yb;
                double ratio = ya * denomr;
                double temp = ratio * HEX_40000000;
                double ra = ratio + temp - temp;
                double rb = ratio - ra;

                temp = denom * HEX_40000000;
                double za = denom + temp - temp;
                double zb = denom - za;

                rb += (ya - za * ra - za * rb - zb * ra - zb * rb) * denomr;

                // Adjust for yb
                rb += yb * denomr;                        // numerator
                rb += -ya * denomb * denomr * denomr;   // denominator

                // y = y - 1/y
                temp = ya + ra;
                yb += -(temp - ya - ra);
                ya = temp;
                temp = ya + rb;
                yb += -(temp - ya - rb);
                ya = temp;

                result = ya + yb;
                result *= 0.5;
            }

            if (negate)
            {
                result = -result;
            }

            return result;
        }

        public static double Tan(double x)
        {
            Boolean negative = false;
            int quadrant = 0;

            /* Take absolute value of the input */
            double xa = x;
            if (x < 0)
            {
                negative = true;
                xa = -xa;
            }

            /* Check for zero and negative zero */
            if (xa == 0.0)
            {
                long bits = BitConverter.DoubleToInt64Bits(x);
                if (bits < 0)
                {
                    return -0.0;
                }
                return 0.0;
            }

            if (xa == Double.PositiveInfinity)
            {
                return Double.NaN;
            }

            /* Perform any argument reduction */
            double xb = 0;
            if (xa > 3294198.0)
            {
                // PI * (2**20)
                // Argument too big for CodyWaite reductiond  Must use
                // PayneHanek.
                var reduceResults = new double[3];
                ReducePayneHanek(xa, out reduceResults);
                quadrant = ((int)reduceResults[0]) & 3;
                xa = reduceResults[1];
                xb = reduceResults[2];
            }
            else if (xa > 1.5707963267948966)
            {
                CodyWaite cw = new CodyWaite(xa);
                quadrant = cw.K & 3;
                xa = cw.RemA;
                xb = cw.RemB;
            }

            if (xa > 1.5)
            {
                // Accuracy suffers between 1.5 and PI/2
                double pi2a = 1.5707963267948966;
                double pi2b = 6.123233995736766E-17;

                double a = pi2a - xa;
                double b = -(a - pi2a + xa);
                b += pi2b - xb;

                xa = a + b;
                xb = -(xa - a - b);
                quadrant ^= 1;
                negative ^= true;
            }

            double result;
            if ((quadrant & 1) == 0)
            {
                result = TanQ(xa, xb, false);
            }
            else
            {
                result = -TanQ(xa, xb, true);
            }

            if (negative)
            {
                result = -result;
            }

            return result;
        }

        public static double Tanh(double x)
        {
            Boolean negate = false;
            // tanh[z] = sinh[z] / cosh[z]
            // = (exp(z) - exp(-z)) / (exp(z) + exp(-z))
            // = (exp(2x) - 1) / (exp(2x) + 1)

            // for magnitude > 20, sinh[z] == cosh[z] in double precision

            if (x > 20.0)
            {
                return 1.0;
            }

            if (x < -20)
            {
                return -1.0;
            }

            if (x == 0)
            {
                return x;
            }

            if (x < 0.0)
            {
                x = -x;
                negate = true;
            }

            double result;
            if (x >= 0.5)
            {
                var hiPrec = new double[2];
                // tanh(x) = (exp(2x) - 1) / (exp(2x) + 1)
                Exp(x * 2.0, 0.0, hiPrec);

                double ya = hiPrec[0] + hiPrec[1];
                double yb = -(ya - hiPrec[0] - hiPrec[1]);

                /* Numerator */
                double na = -1.0 + ya;
                double nb = -(na + 1.0 - ya);
                double temp = na + yb;
                nb += -(temp - na - yb);
                na = temp;

                /* Denominator */
                double da = 1.0 + ya;
                double db = -(da - 1.0 - ya);
                temp = da + yb;
                db += -(temp - da - yb);
                da = temp;

                temp = da * HEX_40000000;
                double daa = da + temp - temp;
                double dab = da - daa;

                // ratio = na/da
                double ratio = na / da;
                temp = ratio * HEX_40000000;
                double ratioa = ratio + temp - temp;
                double ratiob = ratio - ratioa;

                // Correct for rounding in division
                ratiob += (na - daa * ratioa - daa * ratiob - dab * ratioa - dab * ratiob) / da;

                // Account for nb
                ratiob += nb / da;
                // Account for db
                ratiob += -db * na / da / da;

                result = ratioa + ratiob;
            }
            else
            {
                var hiPrec = new double[2];
                // tanh(x) = expm1(2x) / (expm1(2x) + 2)
                Expm1(x * 2.0, hiPrec);

                double ya = hiPrec[0] + hiPrec[1];
                double yb = -(ya - hiPrec[0] - hiPrec[1]);

                /* Numerator */
                double na = ya;
                double nb = yb;

                /* Denominator */
                double da = 2.0 + ya;
                double db = -(da - 2.0 - ya);
                double temp = da + yb;
                db += -(temp - da - yb);
                da = temp;

                temp = da * HEX_40000000;
                double daa = da + temp - temp;
                double dab = da - daa;

                // ratio = na/da
                double ratio = na / da;
                temp = ratio * HEX_40000000;
                double ratioa = ratio + temp - temp;
                double ratiob = ratio - ratioa;

                // Correct for rounding in division
                ratiob += (na - daa * ratioa - daa * ratiob - dab * ratioa - dab * ratiob) / da;

                // Account for nb
                ratiob += nb / da;
                // Account for db
                ratiob += -db * na / da / da;

                result = ratioa + ratiob;
            }

            if (negate)
            {
                result = -result;
            }

            return result;
        }

        private static double Atan(double xa, double xb, Boolean leftPlane)
        {
            if (xa == 0.0)
            { // Matches +/- 0.0; return correct sign
                return leftPlane ? CopySign(System.Math.PI, xa) : xa;
            }

            Boolean negate;
            if (xa < 0)
            {
                // negative
                xa = -xa;
                xb = -xb;
                negate = true;
            }
            else
            {
                negate = false;
            }

            if (xa > 1.633123935319537E16)
            { // Very large input
                return (negate ^ leftPlane) ? (-System.Math.PI * F_1_2) : (System.Math.PI * F_1_2);
            }

            /* Estimate the closest tabulated arctan value, compute eps = xa-tangentTable */
            int idx;
            if (xa < 1)
            {
                idx = (int)(((-1.7168146928204136 * xa * xa + 8.0) * xa) + 0.5);
            }
            else
            {
                double oneOverXa = 1 / xa;
                idx = (int)(-((-1.7168146928204136 * oneOverXa * oneOverXa + 8.0) * oneOverXa) + 13.07);
            }

            double ttA = TANGENT_TABLE_A[idx];
            double ttB = TANGENT_TABLE_B[idx];

            double epsA = xa - ttA;
            double epsB = -(epsA - xa + ttA);
            epsB += xb - ttB;

            double temp = epsA + epsB;
            epsB = -(temp - epsA - epsB);
            epsA = temp;

            /* Compute eps = eps / (1.0 + xa*tangent) */
            temp = xa * HEX_40000000;
            double ya = xa + temp - temp;
            double yb = xb + xa - ya;
            xa = ya;
            xb += yb;

            //if (idx > 8 || idx == 0)
            if (idx == 0)
            {
                /* If the slope of the arctan is gentle enough (< 0.45), this approximation will suffice */
                //double denom = 1.0 / (1.0 + xa*tangentTableA[idx] + xb*tangentTableA[idx] + xa*tangentTableB[idx] + xb*tangentTableB[idx]);
                double denom = 1d / (1d + (xa + xb) * (ttA + ttB));
                //double denom = 1.0 / (1.0 + xa*tangentTableA[idx]);
                ya = epsA * denom;
                yb = epsB * denom;
            }
            else
            {
                double temp2 = xa * ttA;
                double za1 = 1d + temp2;
                double zb1 = -(za1 - 1d - temp2);
                temp2 = xb * ttA + xa * ttB;
                temp = za1 + temp2;
                zb1 += -(temp - za1 - temp2);
                za1 = temp;

                zb1 += xb * ttB;
                ya = epsA / za1;

                temp = ya * HEX_40000000;
                double yaa = (ya + temp) - temp;
                double yab = ya - yaa;

                temp = za1 * HEX_40000000;
                double zaa = (za1 + temp) - temp;
                double zab = za1 - zaa;

                /* Correct for rounding in division */
                yb = (epsA - yaa * zaa - yaa * zab - yab * zaa - yab * zab) / za1;

                yb += -epsA * zb1 / za1 / za1;
                yb += epsB / za1;
            }

            epsA = ya;
            epsB = yb;

            /* Evaluate polynomial */
            double epsA2 = epsA * epsA;

            /*
        yb = -0.09001346640161823;
        yb = yb * epsA2 + 0.11110718400605211;
        yb = yb * epsA2 + -0.1428571349122913;
        yb = yb * epsA2 + 0.19999999999273194;
        yb = yb * epsA2 + -0.33333333333333093;
        yb = yb * epsA2 * epsA;
             */

            yb = 0.07490822288864472;
            yb = yb * epsA2 - 0.09088450866185192;
            yb = yb * epsA2 + 0.11111095942313305;
            yb = yb * epsA2 - 0.1428571423679182;
            yb = yb * epsA2 + 0.19999999999923582;
            yb = yb * epsA2 - 0.33333333333333287;
            yb = yb * epsA2 * epsA;

            ya = epsA;

            temp = ya + yb;
            yb = -(temp - ya - yb);
            ya = temp;

            /* Add in effect of epsBd   atan'(x) = 1/(1+x^2) */
            yb += epsB / (1d + epsA * epsA);

            double eighths = EIGHTHS[idx];

            //result = yb + eighths[idx] + ya;
            double za2 = eighths + ya;
            double zb2 = -(za2 - eighths - ya);
            temp = za2 + yb;
            zb2 += -(temp - za2 - yb);
            za2 = temp;

            double result = za2 + zb2;

            if (leftPlane)
            {
                // Result is in the left plane
                double resultb = -(result - za2 - zb2);
                double pia = 1.5707963267948966 * 2;
                double pib = 6.123233995736766E-17 * 2;

                za2 = pia - result;
                zb2 = -(za2 - pia + result);
                zb2 += pib - resultb;

                result = za2 + zb2;
            }

            if (negate ^ leftPlane)
            {
                result = -result;
            }

            return result;
        }

        private static double CosQ(double xa, double xb)
        {
            double pi2a = 1.5707963267948966;
            double pi2b = 6.123233995736766E-17;

            double a = pi2a - xa;
            double b = -(a - pi2a + xa);
            b += pi2b - xb;

            return SinQ(a, b);
        }

        private static double SinQ(double xa, double xb)
        {
            int idx = (int)((xa * 8.0) + 0.5);
            double epsilon = xa - EIGHTHS[idx]; //idx*0.125;

            // Table lookups
            double sintA = SINE_TABLE_A[idx];
            double sintB = SINE_TABLE_B[idx];
            double costA = COSINE_TABLE_A[idx];
            double costB = COSINE_TABLE_B[idx];

            // Polynomial eval of sin(epsilon), cos(epsilon)
            double sinEpsA = epsilon;
            double sinEpsB = PolySine(epsilon);
            double cosEpsA = 1.0;
            double cosEpsB = PolyCosine(epsilon);

            // Split epsilon   xa + xb = x
            double temp = sinEpsA * HEX_40000000;
            double temp2 = (sinEpsA + temp) - temp;
            sinEpsB += sinEpsA - temp2;
            sinEpsA = temp2;

            /* Compute sin(x) by angle addition formula */
            double result;

            /* Compute the following sum:
             *
             * result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
             *          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;
             *
             * Ranges of elements
             *
             * xxxtA   0            PI/2
             * xxxtB   -1.5e-9      1.5e-9
             * sinEpsA -0.0625      0.0625
             * sinEpsB -6e-11       6e-11
             * cosEpsA  1.0
             * cosEpsB  0           -0.0625
             *
             */

            //result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
            //          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;

            //result = sintA + sintA*cosEpsB + sintB + sintB * cosEpsB;
            //result += costA*sinEpsA + costA*sinEpsB + costB*sinEpsA + costB * sinEpsB;
            double a = 0;
            double b = 0;

            double t = sintA;
            double c = a + t;
            double d = -(c - a - t);
            a = c;
            b += d;

            t = costA * sinEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            b = b + sintA * cosEpsB + costA * sinEpsB;
            /*
        t = sintA*cosEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;

        t = costA*sinEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
             */

            b = b + sintB + costB * sinEpsA + sintB * cosEpsB + costB * sinEpsB;
            /*
        t = sintB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;

        t = costB*sinEpsA;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;

        t = sintB*cosEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;

        t = costB*sinEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
             */

            if (xb != 0.0)
            {
                t = ((costA + costB) * (cosEpsA + cosEpsB) -
                     (sintA + sintB) * (sinEpsA + sinEpsB)) * xb;  // approximate cosine*xb
                c = a + t;
                d = -(c - a - t);
                a = c;
                b += d;
            }

            result = a + b;

            return result;
        }

        private static double TanQ(double xa, double xb, Boolean cotanFlag)
        {
            int idx = (int)((xa * 8.0) + 0.5);
            double epsilon = xa - EIGHTHS[idx]; //idx*0.125;

            // Table lookups
            double sintA = SINE_TABLE_A[idx];
            double sintB = SINE_TABLE_B[idx];
            double costA = COSINE_TABLE_A[idx];
            double costB = COSINE_TABLE_B[idx];

            // Polynomial eval of sin(epsilon), cos(epsilon)
            double sinEpsA = epsilon;
            double sinEpsB = PolySine(epsilon);
            double cosEpsA = 1.0;
            double cosEpsB = PolyCosine(epsilon);

            // Split epsilon   xa + xb = x
            double temp = sinEpsA * HEX_40000000;
            double temp2 = (sinEpsA + temp) - temp;
            sinEpsB += sinEpsA - temp2;
            sinEpsA = temp2;

            /* Compute sin(x) by angle addition formula */

            /* Compute the following sum:
             *
             * result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
             *          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;
             *
             * Ranges of elements
             *
             * xxxtA   0            PI/2
             * xxxtB   -1.5e-9      1.5e-9
             * sinEpsA -0.0625      0.0625
             * sinEpsB -6e-11       6e-11
             * cosEpsA  1.0
             * cosEpsB  0           -0.0625
             *
             */

            //result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
            //          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;

            //result = sintA + sintA*cosEpsB + sintB + sintB * cosEpsB;
            //result += costA*sinEpsA + costA*sinEpsB + costB*sinEpsA + costB * sinEpsB;
            double a = 0;
            double b = 0;

            // Compute sine
            double t = sintA;
            double c = a + t;
            double d = -(c - a - t);
            a = c;
            b += d;

            t = costA * sinEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            b += sintA * cosEpsB + costA * sinEpsB;
            b += sintB + costB * sinEpsA + sintB * cosEpsB + costB * sinEpsB;

            double sina = a + b;
            double sinb = -(sina - a - b);

            // Compute cosine

            a = b = c = d = 0.0;

            t = costA * cosEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            t = -sintA * sinEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            b += costB * cosEpsA + costA * cosEpsB + costB * cosEpsB;
            b -= sintB * sinEpsA + sintA * sinEpsB + sintB * sinEpsB;

            double cosa = a + b;
            double cosb = -(cosa - a - b);

            if (cotanFlag)
            {
                double tmp;
                tmp = cosa; cosa = sina; sina = tmp;
                tmp = cosb; cosb = sinb; sinb = tmp;
            }

            /* estimate and correct, compute 1.0/(cosa+cosb) */
            /*
        double est = (sina+sinb)/(cosa+cosb);
        double err = (sina - cosa*est) + (sinb - cosb*est);
        est += err/(cosa+cosb);
        err = (sina - cosa*est) + (sinb - cosb*est);
             */

            // f(x) = 1/x,   f'(x) = -1/x^2

            double est = sina / cosa;

            /* Split the estimate to get more accurate read on division rounding */
            temp = est * HEX_40000000;
            double esta = (est + temp) - temp;
            double estb = est - esta;

            temp = cosa * HEX_40000000;
            double cosaa = (cosa + temp) - temp;
            double cosab = cosa - cosaa;

            //double err = (sina - est*cosa)/cosa;  // Correction for division rounding
            double err = (sina - esta * cosaa - esta * cosab - estb * cosaa - estb * cosab) / cosa;  // Correction for division rounding
            err += sinb / cosa;                     // Change in est due to sinb
            err += -sina * cosb / cosa / cosa;    // Change in est due to cosb

            if (xb != 0.0)
            {
                // tan' = 1 + tan^2      cot' = -(1 + cot^2)
                // Approximate impact of xb
                double xbadj = xb + est * est * xb;
                if (cotanFlag)
                {
                    xbadj = -xbadj;
                }

                err += xbadj;
            }

            return est + err;
        }

        #endregion

        #region Utility Methods

        public static int UnsignedRightBitShiftOperation(long left, int right)
        {
            return (int)(UInt64.Parse(left.ToString()) >> right);
        }

        /// <summary>
        /// Add two integers, checking for overflow.
        /// 
        /// </summary>
        /// <param Name="x">an addend</param>
        /// <param Name="y">an addend</param>
        /// <returns>the sum <code>x+y</code></returns>
        /// <exception cref="ArithmeticException">if the result can not be represented as an </exception>
        ///         int
        /// @since 1.1
        public static int AddAndCheck(int x, int y)
        {
            long s = (long)x + (long)y;
            if (s < Int32.MinValue || s > Int32.MaxValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, x, y));
            }
            return (int)s;
        }

        /// <summary>
        /// Add two long integers, checking for overflow.
        /// 
        /// </summary>
        /// <param Name="a">an addend</param>
        /// <param Name="b">an addend</param>
        /// <returns>the sum <code>a+b</code></returns>
        /// <exception cref="ArithmeticException">if the result can not be represented as an </exception>
        ///         long
        /// @since 1.2
        public static long AddAndCheck(long a, long b)
        {
            return AddAndCheck(a, b, LocalizedResources.Instance().OVERFLOW_IN_ADDITION);
        }

        /// <summary>
        /// Add two long integers, checking for overflow.
        /// 
        /// </summary>
        /// <param Name="a">an addend</param>
        /// <param Name="b">an addend</param>
        /// <param Name="pattern">the pattern to use for any thrown exception.</param>
        /// <returns>the sum <code>a+b</code></returns>
        /// <exception cref="ArithmeticException">if the result can not be represented as an </exception>
        ///         long
        /// @since 1.2
        private static long AddAndCheck(long a, long b, String pattern)
        {
            long ret;
            if (a > b)
            {
                // use symmetry to reduce boundary cases
                ret = AddAndCheck(b, a, pattern);
            }
            else
            {
                // assert a <= b

                if (a < 0)
                {
                    if (b < 0)
                    {
                        // check for negative overflow
                        if (long.MinValue - b <= a)
                        {
                            ret = a + b;
                        }
                        else
                        {
                            throw new MathArithmeticException(String.Format(pattern, a, b));
                        }
                    }
                    else
                    {
                        // opposite sign addition is always safe
                        ret = a + b;
                    }
                }
                else
                {
                    // assert a >= 0
                    // assert b >= 0

                    // check for positive overflow
                    if (a <= long.MaxValue - b)
                    {
                        ret = a + b;
                    }
                    else
                    {
                        throw new MathArithmeticException(String.Format(pattern, a, b));
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Subtract two integers, checking for overflow.
        /// 
        /// </summary>
        /// <param Name="x">the minuend</param>
        /// <param Name="y">the subtrahend</param>
        /// <returns>the difference <code>x-y</code></returns>
        /// <exception cref="ArithmeticException">if the result can not be represented as an </exception>
        ///         int
        /// @since 1.1
        public static int SubAndCheck(int x, int y)
        {
            long s = (long)x - (long)y;
            if (s < Int32.MinValue || s > Int32.MaxValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_SUBTRACTION, x, y));
            }
            return (int)s;
        }

        /// <summary>
        /// Subtract two long integers, checking for overflow.
        /// 
        /// </summary>
        /// <param Name="a">first value</param>
        /// <param Name="b">second value</param>
        /// <returns>the difference <code>a-b</code></returns>
        /// <exception cref="ArithmeticException">if the result can not be represented as an </exception>
        ///         long
        /// @since 1.2
        public static long SubAndCheck(long a, long b)
        {
            long ret;
            String msg = "overflow: subtract";
            if (b == long.MinValue)
            {
                if (a < 0)
                {
                    ret = a - b;
                }
                else
                {
                    throw new ArithmeticException(msg);
                }
            }
            else
            {
                // use additive inverse
                ret = AddAndCheck(a, -b, LocalizedResources.Instance().OVERFLOW_IN_ADDITION);
            }
            return ret;
        }

        /// <summary>
        /// <p>
        /// Returns the least common multiple of the absolute value of two numbers,
        /// using the formula <code>lcm(a,b) = (a / gcd(a,b)) * b</code>.
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations <code>lcm(Int32.MinValue, n)</code> and
        /// <code>lcm(n, Int32.MinValue)</code>, where <code>Abs(n)</code> is a
        /// power of 2, throw an <code>ArithmeticException</code>, because the result
        /// would be 2^31, which is too large for an int value.</li>
        /// <li>The result of <code>lcm(0, x)</code> and <code>lcm(x, 0)</code> is
        /// <code>0</code> for any <code>x</code>.
        /// </ul>
        /// 
        /// </summary>
        /// <param Name="a">any number</param>
        /// <param Name="b">any number</param>
        /// <returns>the least common multiple, never negative</returns>
        /// <exception cref="ArithmeticException"></exception>
        ///             if the result cannot be represented as a nonnegative int
        ///             value
        /// @since 1.1
        public static int LeastCommonMultiply(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            int lcm = Math.Abs(MultiplyAndCheck(a / GreatestCommonDivisor(a, b), b));
            if (lcm == Int32.MinValue)
            {
                throw new MathArithmeticException(String.Format(                    LocalizedResources.Instance().LCM_OVERFLOW_32_BITS,                    a, b));
            }
            return lcm;
        }

        /// <summary>
        /// <p>
        /// Returns the least common multiple of the absolute value of two numbers,
        /// using the formula <code>lcm(a,b) = (a / gcd(a,b)) * b</code>.
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations <code>lcm(Long.MinValue, n)</code> and
        /// <code>lcm(n, Long.MinValue)</code>, where <code>Abs(n)</code> is a
        /// power of 2, throw an <code>ArithmeticException</code>, because the result
        /// would be 2^63, which is too large for an int value.</li>
        /// <li>The result of <code>lcm(0L, x)</code> and <code>lcm(x, 0L)</code> is
        /// <code>0L</code> for any <code>x</code>.
        /// </ul>
        /// 
        /// </summary>
        /// <param Name="a">any number</param>
        /// <param Name="b">any number</param>
        /// <returns>the least common multiple, never negative</returns>
        /// <exception cref="ArithmeticException">if the result cannot be represented as a nonnegative long </exception>
        /// value
        /// @since 2.1
        public static long LeastCommonMultiply(long a, long b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            long lcm = Math.Abs(MultiplyAndCheck(a / GreatestCommonDivisor(a, b), b));
            if (lcm == long.MinValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().LCM_OVERFLOW_64_BITS, a, b));
            }
            return lcm;
        }

        /// <summary>
        /// <p>
        /// Gets the greatest common divisor of the absolute value of two numbers,
        /// using the "binary gcd" method which avoids division and modulo
        /// operationsd See Knuth 4.5.2 algorithm Bd This algorithm is due to Josef
        /// Stein (1961).
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations
        /// <code>gcd(Int32.MinValue, Int32.MinValue)</code>,
        /// <code>gcd(Int32.MinValue, 0)</code> and
        /// <code>gcd(0, Int32.MinValue)</code> throw an
        /// <code>ArithmeticException</code>, because the result would be 2^31, which
        /// is too large for an int value.</li>
        /// <li>The result of <code>gcd(x, x)</code>, <code>gcd(0, x)</code> and
        /// <code>gcd(x, 0)</code> is the absolute value of <code>x</code>, except
        /// for the special cases above.
        /// <li>The invocation <code>gcd(0, 0)</code> is the only one which returns
        /// <code>0</code>.</li>
        /// </ul>
        /// 
        /// </summary>
        /// <param Name="p">any number</param>
        /// <param Name="q">any number</param>
        /// <returns>the greatest common divisor, never negative</returns>
        /// <exception cref="ArithmeticException">if the result cannot be represented as a </exception>
        /// nonnegative int value
        /// @since 1.1
        public static int GreatestCommonDivisor(int p, int q)
        {
            int u = p;
            int v = q;
            if ((u == 0) || (v == 0))
            {
                if ((u == Int32.MinValue) || (v == Int32.MinValue))
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_32_BITS, p, q));
                }
                return Math.Abs(u) + Math.Abs(v);
            }
            // keep u and v negative, as negative integers range down to
            // -2^31, while positive numbers can only be as large as 2^31-1
            // (i.ed we can't necessarily negate a negative number without
            // overflow)
            /* assert u!=0 && v!=0; */
            if (u > 0)
            {
                u = -u;
            } // make u negative
            if (v > 0)
            {
                v = -v;
            } // make v negative
              // B1d [Find power of 2]
            int k = 0;
            while ((u & 1) == 0 && (v & 1) == 0 && k < 31)
            { // while u and v are
              // both even...
                u /= 2;
                v /= 2;
                k++; // cast out twos.
            }
            if (k == 31)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_32_BITS, p, q));
            }
            // B2d Initialize: u and v have been divided by 2^k and at least
            // one is odd.
            int t = ((u & 1) == 1) ? v : -(u / 2)/* B3 */;
            // t negative: u was odd, v may be even (t replaces v)
            // t positive: u was even, v is odd (t replaces u)
            do
            {
                /* assert u<0 && v<0; */
                // B4/B3: cast out twos from t.
                while ((t & 1) == 0)
                { // while t is even..
                    t /= 2; // cast out twos
                }
                // B5 [reset max(u,v)]
                if (t > 0)
                {
                    u = -t;
                }
                else
                {
                    v = t;
                }
                // B6/B3d at this point both u and v should be odd.
                t = (v - u) / 2;
                // |u| larger: t positive (replace u)
                // |v| larger: t negative (replace v)
            } while (t != 0);
            return -u * (1 << k); // gcd is u*2^k
        }

        /// <summary>
        /// <p>
        /// Gets the greatest common divisor of the absolute value of two numbers,
        /// using the "binary gcd" method which avoids division and modulo
        /// operationsd See Knuth 4.5.2 algorithm Bd This algorithm is due to Josef
        /// Stein (1961).
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations
        /// <code>gcd(Long.MinValue, Long.MinValue)</code>,
        /// <code>gcd(Long.MinValue, 0L)</code> and
        /// <code>gcd(0L, Long.MinValue)</code> throw an
        /// <code>ArithmeticException</code>, because the result would be 2^63, which
        /// is too large for a long value.</li>
        /// <li>The result of <code>gcd(x, x)</code>, <code>gcd(0L, x)</code> and
        /// <code>gcd(x, 0L)</code> is the absolute value of <code>x</code>, except
        /// for the special cases above.
        /// <li>The invocation <code>gcd(0L, 0L)</code> is the only one which returns
        /// <code>0L</code>.</li>
        /// </ul>
        /// 
        /// </summary>
        /// <param Name="p">any number</param>
        /// <param Name="q">any number</param>
        /// <returns>the greatest common divisor, never negative</returns>
        /// <exception cref="ArithmeticException">if the result cannot be represented as a nonnegative long </exception>
        /// value
        /// @since 2.1
        public static long GreatestCommonDivisor(long p, long q)
        {
            long u = p;
            long v = q;
            if ((u == 0) || (v == 0))
            {
                if ((u == long.MinValue) || (v == long.MinValue))
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_64_BITS, p, q));
                }
                return Math.Abs(u) + Math.Abs(v);
            }
            // keep u and v negative, as negative integers range down to
            // -2^63, while positive numbers can only be as large as 2^63-1
            // (i.ed we can't necessarily negate a negative number without
            // overflow)
            /* assert u!=0 && v!=0; */
            if (u > 0)
            {
                u = -u;
            } // make u negative
            if (v > 0)
            {
                v = -v;
            } // make v negative
              // B1d [Find power of 2]
            int k = 0;
            while ((u & 1) == 0 && (v & 1) == 0 && k < 63)
            { // while u and v are
              // both even...
                u /= 2;
                v /= 2;
                k++; // cast out twos.
            }
            if (k == 63)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_64_BITS, p, q));
            }
            // B2d Initialize: u and v have been divided by 2^k and at least
            // one is odd.
            long t = ((u & 1) == 1) ? v : -(u / 2)/* B3 */;
            // t negative: u was odd, v may be even (t replaces v)
            // t positive: u was even, v is odd (t replaces u)
            do
            {
                /* assert u<0 && v<0; */
                // B4/B3: cast out twos from t.
                while ((t & 1) == 0)
                { // while t is even..
                    t /= 2; // cast out twos
                }
                // B5 [reset max(u,v)]
                if (t > 0)
                {
                    u = -t;
                }
                else
                {
                    v = t;
                }
                // B6/B3d at this point both u and v should be odd.
                t = (v - u) / 2;
                // |u| larger: t positive (replace u)
                // |v| larger: t negative (replace v)
            } while (t != 0);
            return -u * (1L << k); // gcd is u*2^k
        }

        /// <summary>
        /// Multiply two integers, checking for overflow.
        /// </summary>
        /// <param name="x">a factor</param>
        /// <param name="y">a factor</param>
        /// <returns>the product <code>x*y</code></returns>
        public static int MultiplyAndCheck(int x, int y)
        {
            long m = ((long)x) * ((long)y);
            if (m < int.MinValue || m > int.MaxValue)
            {
                throw new ArithmeticException("overflow: mul");
            }
            return (int)m;
        }

        /// <summary>
        /// Multiply two long integers, checking for overflow.
        /// </summary>
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// <returns>the product <code>a * b</code></returns>
        public static long MultiplyAndCheck(long a, long b)
        {
            long ret;
            String msg = "overflow: multiply";
            if (a > b)
            {
                // use symmetry to reduce boundary cases
                ret = MultiplyAndCheck(b, a);
            }
            else
            {
                if (a < 0)
                {
                    if (b < 0)
                    {
                        // check for positive overflow with negative a, negative b
                        if (a >= long.MaxValue / b)
                        {
                            ret = a * b;
                        }
                        else
                        {
                            throw new ArithmeticException(msg);
                        }
                    }
                    else if (b > 0)
                    {
                        // check for negative overflow with negative a, positive b
                        if (long.MinValue / b <= a)
                        {
                            ret = a * b;
                        }
                        else
                        {
                            throw new ArithmeticException(msg);

                        }
                    }
                    else
                    {
                        // assert b == 0
                        ret = 0;
                    }
                }
                else if (a > 0)
                {
                    // assert a > 0
                    // assert b > 0

                    // check for positive overflow with positive a, positive b
                    if (a <= long.MaxValue / b)
                    {
                        ret = a * b;
                    }
                    else
                    {
                        throw new ArithmeticException(msg);
                    }
                }
                else
                {
                    // assert a == 0
                    ret = 0;
                }
            }
            return ret;
        }

        #endregion

        #region Exact Functions

        public static int AddExact(int a, int b)
        {
            // compute sum
            int sum = a + b;

            // check for overflow
            if ((a ^ b) >= 0 && (sum ^ b) < 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, a, b));
            }

            return sum;
        }

        public static long AddExact(long a, long b)
        {
            // compute sum
            long sum = a + b;

            // check for overflow
            if ((a ^ b) >= 0 && (sum ^ b) < 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, a, b));
            }

            return sum;
        }

        public static int DecrementExact(int n)
        {
            if (n == int.MinValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_SUBTRACTION, n, 1));
            }

            return n - 1;
        }

        public static long DecrementExact(long n)
        {
            if (n == long.MinValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_SUBTRACTION, n, 1));
            }

            return n - 1;
        }

        public static int IncrementExact(int n)
        {
            if (n == int.MaxValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, n, 1));
            }

            return n + 1;
        }

        public static long IncrementExact(long n)
        {
            if (n == long.MaxValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, n, 1));
            }

            return n + 1;
        }

        public static int MultiplyExact(int a, int b)
        {
            if (((b > 0) && (a > int.MaxValue / b || a < int.MinValue / b)) ||
                ((b < -1) && (a > int.MinValue / b || a < int.MaxValue / b)) ||
                ((b == -1) && (a == int.MinValue)))
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_MULTIPLICATION, a, b));
            }
            return a * b;
        }

        public static long MultiplyExact(long a, long b)
        {
            if (((b > 0L) && (a > long.MaxValue / b || a < long.MinValue / b)) ||
                ((b < -1L) && (a > long.MinValue / b || a < long.MaxValue / b)) ||
                ((b == -1L) && (a == long.MinValue)))
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_MULTIPLICATION, a, b));
            }
            return a * b;
        }

        public static int SubtractExact(int a, int b)
        {
            // compute subtraction
            int sub = a - b;

            // check for overflow
            if ((a ^ b) < 0 && (sub ^ b) >= 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_SUBTRACTION, a, b));
            }

            return sub;
        }

        public static long SubtractExact(long a, long b)
        {
            // compute subtraction
            long sub = a - b;

            // check for overflow
            if ((a ^ b) < 0 && (sub ^ b) >= 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_SUBTRACTION, a, b));
            }

            return sub;
        }

        #endregion

        /// <summary>
        /// Specification of ordering direction.
        /// </summary>
        public enum OrderDirection
        {
            /// <summary>Constant for increasing directiond */
            INCREASING,
            /// <summary>Constant for decreasing directiond */
            DECREASING
        }

        /// <summary>
        /// Checks that the given array is sorted.
        /// 
        /// </summary>
        /// <param Name="val">Values.</param>
        /// <param Name="dir">Ordering direction.</param>
        /// <param Name="strict">Whether the order should be strict.</param>
        /// <exception cref="NonMonotonousSequenceException">if the array is not sortedd </exception>
        /// @since 2.2
        public static void CheckOrder(double[] val, OrderDirection dir, Boolean strict)
        {
            double previous = val[0];
            Boolean ok = true;

            int max = val.Length;
            for (int i = 1; i < max; i++)
            {
                switch (dir)
                {
                    case OrderDirection.INCREASING:
                        if (strict)
                        {
                            if (val[i] <= previous)
                            {
                                ok = false;
                            }
                        }
                        else
                        {
                            if (val[i] < previous)
                            {
                                ok = false;
                            }
                        }
                        break;
                    case OrderDirection.DECREASING:
                        if (strict)
                        {
                            if (val[i] >= previous)
                            {
                                ok = false;
                            }
                        }
                        else
                        {
                            if (val[i] > previous)
                            {
                                ok = false;
                            }
                        }
                        break;
                    default:
                        // Should never happen.
                        throw new ArgumentException();
                }

                if (!ok)
                {
                    throw new NonMonotonousSequenceException(val[i], previous, i, dir, strict);
                }
                previous = val[i];
            }
        }

        /// <summary>
        /// Checks that the given array is sorted in strictly increasing order.
        /// 
        /// </summary>
        /// <param Name="val">Values.</param>
        /// <exception cref="NonMonotonousSequenceException">if the array is not sortedd </exception>
        /// @since 2.2
        public static void CheckOrder(double[] val)
        {
            CheckOrder(val, OrderDirection.INCREASING, true);
        }

        /// <summary>
        /// Compares two numbers given some amount of allowed error.
        /// </summary>
        /// <param name="x">the first number</param>
        /// <param name="y">the second number</param>
        /// <param name="eps">the amount of error to allow when checking for equality</param>
        /// <returns>
        /// <ul><li>0 if  {@link #equals(double, double, double) equals(x, y, eps)}</li>
        /// <li>&lt; 0 if !{@link #equals(double, double, double) equals(x, y, eps)} &amp;&amp; x &lt; y</li>
        /// <li>> 0 if !{@link #equals(double, double, double) equals(x, y, eps)} &amp;&amp; x > y</li></ul>
        /// </returns>
        public static int CompareTo(double x, double y, double eps)
        {
            if (x.AlmostEquals( y, eps))
            {
                return 0;
            }
            else if (x < y)
            {
                return -1;
            }
            return 1;
        }

        #region Common Functions
        public static int Abs(int x)
        {
            int i = (int)LogicalRightShift(x, 31); //x >>> 31
            return (x ^ (~i + 1)) + i;
        }

        public static long Abs(long x)
        {
            long l = (long)LogicalRightShift(x, 63); // x >>> 63;
            // l is one if x negative zero else
            // ~l+1 is zero if x is positive, -1 if x is negative
            // x^(~l+1) is x is x is positive, ~x if x is negative
            // add around
            return (x ^ (~l + 1)) + l;
        }

        public static float Abs(float x)
        {
            return BitConverter2.IntBitsToFloat(MASK_NON_SIGN_INT & BitConverter2.FloatToRawIntBits(x));
        }

        public static double Abs(double x)
        {
            return BitConverter.Int64BitsToDouble(MASK_NON_SIGN_LONG & BitConverter.DoubleToInt64Bits(x));
        }

        /// <summary>
        /// Safely adds two int values.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
        /// <exception cref="ArithmeticException">if the result overflows an int</exception>
        public static int SafeAdd(int a, int b)
        {
            int sum = a + b;
            // check for a change of sign in the result when the inputs have the same sign
            if ((a ^ sum) < 0 && (a ^ b) >= 0)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_ADDITION_OVERFLOWS_INT, a, b));
            }
            return sum;
        }

        /// <summary>
        /// Safely adds two long values.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
        /// <exception cref="ArithmeticException">if the result overflows an long</exception>
        public static long SafeAdd(long a, long b)
        {
            long sum = a + b;
            // check for a change of sign in the result when the inputs have the same sign
            if ((a ^ sum) < 0 && (a ^ b) >= 0)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_ADDITION_OVERFLOWS_LONG, a, b));
            }
            return sum;
        }

        /// <summary>
        /// Safely subtracts one int from another.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value to subtract from the first</param>
        /// <returns>the result</returns>
        /// <exception cref="ArithmeticException">if the result overflows an int</exception>
        public static int SafeSubtract(int a, int b)
        {
            int result = a - b;
            // check for a change of sign in the result when the inputs have the different signs
            if ((a ^ result) < 0 && (a ^ b) < 0)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_SUBTRACTION_OVERFLOWS_INT, a, b));
            }
            return result;
        }

        /// <summary>
        /// Safely subtracts one long from another.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value to subtract from the first</param>
        /// <returns>the result</returns>
        /// <exception cref="ArithmeticException">if the result overflows an long</exception>
        public static long SafeSubtract(long a, long b)
        {
            long result = a - b;
            // check for a change of sign in the result when the inputs have the different signs
            if ((a ^ result) < 0 && (a ^ b) < 0)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_SUBTRACTION_OVERFLOWS_LONG, a, b));
            }
            return result;
        }

        /// <summary>
        /// Safely multiply one int by another.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
        /// <exception cref="ArithmeticException">if the result overflows an int</exception>
        public static int SafeMultiply(int a, int b)
        {
            long total = (long)a * (long)b;
            if (total < int.MinValue || total > int.MaxValue)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_MULTIPLICTION_OVERFLOWS_INT, a, b));
            }
            return (int)total;
        }

        /// <summary>
        /// Safely multiply a long by an int.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the new total</returns>
        /// <exception cref="ArithmeticException">if the result overflows a long</exception>
        public static long SafeMultiply(long a, int b)
        {
            switch (b)
            {
                case -1:
                    if (a == long.MinValue)
                    {
                        throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_MULTIPLICTION_OVERFLOWS_LONG, a, b));
                    }
                    return -a;
                case 0:
                    return 0L;
                case 1:
                    return a;
            }
            long total = a * b;
            if (total / b != a)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_MULTIPLICTION_OVERFLOWS_LONG, a, b));
            }
            return total;
        }

        /// <summary>
        /// Multiply two values throwing an exception if overflow occurs.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the new total</returns>
        /// <exception cref="ArithmeticException">if the result overflows a long</exception>
        public static long SafeMultiply(long a, long b)
        {
            if (b == 1)
            {
                return a;
            }
            if (a == 1)
            {
                return b;
            }
            if (a == 0 || b == 0)
            {
                return 0;
            }
            long total = a * b;
            if (total / b != a || (a == long.MinValue && b == -1) || (b == long.MinValue && a == -1))
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_MULTIPLICTION_OVERFLOWS_LONG, a, b));
            }
            return total;
        }

        public static double CubeRoot(double x)
        {
            /* Convert input double to bits */
            long inbits = BitConverter.DoubleToInt64Bits(x);
            int exponent = (int)((inbits >> 52) & 0x7ff) - 1023;
            Boolean subnormal = false;

            if (exponent == -1023)
            {
                if (x == 0)
                {
                    return x;
                }

                /* Subnormal, so normalize */
                subnormal = true;
                x *= 1.8014398509481984E16;  // 2^54
                inbits = BitConverter.DoubleToInt64Bits(x);
                exponent = (int)((inbits >> 52) & 0x7ff) - 1023;
            }

            if (exponent == 1024)
            {
                // Nan or infinityd  Don't care which.
                return x;
            }

            /* Divide the exponent by 3 */
            int exp3 = exponent / 3;

            /* p2 will be the nearest power of 2 to x with its exponent divided by 3 */
            double p2 = BitConverter.Int64BitsToDouble((long)((ulong)inbits & 0x8000000000000000L) | (long)(((exp3 + 1023) & 0x7ff)) << 52);

            /* This will be a number between 1 and 2 */
            double mant = BitConverter.Int64BitsToDouble((inbits & 0x000fffffffffffffL) | 0x3ff0000000000000L);

            /* Estimate the cube root of mant by polynomial */
            double est = -0.010714690733195933;
            est = est * mant + 0.0875862700108075;
            est = est * mant + -0.3058015757857271;
            est = est * mant + 0.7249995199969751;
            est = est * mant + 0.5039018405998233;

            est *= CBRTTWO[exponent % 3 + 2];

            // est should now be good to about 15 bits of precisiond   Do 2 rounds of
            // Newton's method to get closer,  this should get us full double precision
            // Scale down x for the purpose of doing newtons methodd  This avoids over/under flows.
            double xs = x / (p2 * p2 * p2);
            est += (xs - est * est * est) / (3 * est * est);
            est += (xs - est * est * est) / (3 * est * est);

            // Do one round of Newton's method in extended precision to get the last bit right.
            double temp = est * HEX_40000000;
            double ya = est + temp - temp;
            double yb = est - ya;

            double za = ya * ya;
            double zb = ya * yb * 2.0 + yb * yb;
            temp = za * HEX_40000000;
            double temp2 = za + temp - temp;
            zb += za - temp2;
            za = temp2;

            zb = za * yb + ya * zb + zb * yb;
            za *= ya;

            double na = xs - za;
            double nb = -(na - xs + za);
            nb -= zb;

            est += (na + nb) / (3 * est * est);

            /* Scale by a power of two, so this is exactd */
            est *= p2;

            if (subnormal)
            {
                est *= 3.814697265625E-6;  // 2^-18
            }

            return est;
        }

        //public static double CubeRoot(double num)
        //{
        //    return System.Math.Ceiling(System.Math.Pow(num, (double)1 / 3));
        //}

        public static double Ceil(double x)
        {
            double y;

            if (Double.IsNaN(x))
            { // NaN
                return x;
            }

            y = Floor(x);
            if (y == x)
            {
                return y;
            }

            y += 1.0;

            if (y == 0)
            {
                return x * y;
            }

            return y;
        }

        /**
         * Check that the argument is a real number.
         *
         * @param x Argument.
         * @throws NotFiniteNumberException if {@code x} is not a
         * finite real number.
         */
        public static void CheckFinite(double x)
        {
            if (Double.IsInfinity(x) || Double.IsNaN(x))
            {
                throw new NotFiniteNumberException(x);
            }
        }

        /**
         * Check that all the elements are real numbers.
         *
         * @param val Arguments.
         * @throws NotFiniteNumberException if any values of the array is not a
         * finite real number.
         */
        public static void CheckFinite(double[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                double x = val[i];
                if (Double.IsInfinity(x) || Double.IsNaN(x))
                {
                    throw new NotFiniteNumberException(String.Format(LocalizedResources.Instance().ARRAY_ELEMENT, x, i));
                }
            }
        }

        /**
         * Checks that an object is not null.
         *
         * @param o Object to be checked.
         * @param pattern Message pattern.
         * @param args Arguments to replace the placeholders in {@code pattern}.
         * @throws ArgumentNullException if {@code o} is {@code null}.
         */
        public static void CheckNotNull(Object o, String pattern, params Object[] args)
        {
            if (o == null)
            {
                throw new ArgumentNullException(String.Format(pattern, args));
            }
        }

        /**
         * Checks that an object is not null.
         *
         * @param o Object to be checked.
         * @throws ArgumentNullException if {@code o} is {@code null}.
         */
        public static void CheckNotNull(Object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static double CopySign(double magnitude, double sign)
        {
            // The highest order bit is going to be zero if the
            // highest order bit of m and s is the same and one otherwise.
            // So (m^s) will be positive if both m and s have the same sign
            // and negative otherwise.
            //long m = BitConverter.DoubleToInt64Bits(magnitude); // don't care about NaN
            //long s = BitConverter.DoubleToInt64Bits(sign);
            //if ((m ^ s) >= 0)
            //{
            //    return magnitude;
            //}
            //return -magnitude; // flip sign

            //Taking code from Microsoft .NET Core code.
            // https://github.com/dotnet/runtime/blob/1b14c945a9c9ff8c90b8507fe8f0f9cccd20ad73/src/libraries/System.Private.CoreLib/src/System/Math.cs
            //if (Sse2.IsSupported || AdvSimd.IsSupported)
            //{
            //    return VectorMath.ConditionalSelectBitwise(Vector128.CreateScalarUnsafe(-0.0), Vector128.CreateScalarUnsafe(sign), Vector128.CreateScalarUnsafe(magnitude)).ToScalar();
            //}
            //else
            //{
            const long signMask = 1L << 63;

            // This method is required to work for all inputs,
            // including NaN, so we operate on the raw bits.
            long xbits = BitConverter.DoubleToInt64Bits(magnitude);
            long ybits = BitConverter.DoubleToInt64Bits(sign);

            // Remove the sign from x, and remove everything but the sign from y
            xbits &= ~signMask;
            ybits &= signMask;

            // Simply OR them to get the correct sign
            return BitConverter.Int64BitsToDouble(xbits | ybits);
            //}
        }

        public static double CopySign2(double magnitude, double sign)
        {
            return BitConverter.DoubleToInt64Bits((BitConverter.DoubleToInt64Bits(sign) &
                                        (Math2.SIGN_BIT_MASK)) |
                                       (BitConverter.DoubleToInt64Bits(magnitude) &
                                        (Math2.EXP_BIT_MASK |
                                         Math2.SIGNIF_BIT_MASK)));
        }

        public static float CopySign(float magnitude, float sign)
        {
            // The highest order bit is going to be zero if the
            // highest order bit of m and s is the same and one otherwise.
            // So (m^s) will be positive if both m and s have the same sign
            // and negative otherwise.
            int m = BitConverter2.FloatToRawIntBits(magnitude);
            int s = BitConverter2.FloatToRawIntBits(sign);
            if ((m ^ s) >= 0)
            {
                return magnitude;
            }
            return -magnitude; // flip sign
        }

        /**
         * Returns the first argument with the sign of the second argument.
         *
         * @param magnitude Magnitude of the returned value.
         * @param sign Sign of the returned value.
         * @return a value with magnitude equal to {@code magnitude} and with the
         * same sign as the {@code sign} argument.
         * @throws MathArithmeticException if {@code magnitude == Byte.MinValue}
         * and {@code sign >= 0}.
         */
        public static byte CopySign(byte magnitude, byte sign)
        {
            if ((magnitude >= 0 && sign >= 0) ||
                (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 &&
                     magnitude == Byte.MinValue)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW);
            }
            else
            {
                return (byte)-magnitude; // Flip sign.
            }
        }

        /**
         * Returns the first argument with the sign of the second argument.
         *
         * @param magnitude Magnitude of the returned value.
         * @param sign Sign of the returned value.
         * @return a value with magnitude equal to {@code magnitude} and with the
         * same sign as the {@code sign} argument.
         * @throws MathArithmeticException if {@code magnitude == Short.MinValue}
         * and {@code sign >= 0}.
         */
        public static short CopySign(short magnitude, short sign)
        {
            if ((magnitude >= 0 && sign >= 0) ||
                (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 &&
                         magnitude == short.MinValue)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW);
            }
            else
            {
                return (short)-magnitude; // Flip sign.
            }
        }

        /**
         * Returns the first argument with the sign of the second argument.
         *
         * @param magnitude Magnitude of the returned value.
         * @param sign Sign of the returned value.
         * @return a value with magnitude equal to {@code magnitude} and with the
         * same sign as the {@code sign} argument.
         * @throws MathArithmeticException if {@code magnitude == int.MinValue}
         * and {@code sign >= 0}.
         */
        public static int CopySign(int magnitude, int sign)
        {
            if ((magnitude >= 0 && sign >= 0) ||
                (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 &&
                         magnitude == int.MinValue)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW);
            }
            else
            {
                return -magnitude; // Flip sign.
            }
        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// </summary>
        /// <param name="magnitude">Magnitude of the returned value.</param>
        /// <param name="sign">Sign of the returned value.</param>
        /// <returns>a value with magnitude equal to {@code magnitude} and with the same sign as the {@code sign} argument.</returns>
        public static long CopySign(long magnitude, long sign)
        {
            if ((magnitude >= 0 && sign >= 0) ||
                (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 &&
                         magnitude == long.MinValue)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW);
            }
            else
            {
                return -magnitude; // Flip sign.
            }
        }

        [Obsolete("Deprecatred")]
        public static double Exp(double x)
        {
            // Deprecated
            //return Exp(x, 0.0, null);

            if  (Double.IsNaN(x)) //(x != x)
                return x;
            if (x > EXP_LIMIT_H)
                return Double.PositiveInfinity;
            if (x < EXP_LIMIT_L)
                return 0;

            // Argument reduction.
            double hi;
            double lo;
            int k;
            double t = Math.Abs(x);
            if (t > 0.5 * LN2)
            {
                if (t < 1.5 * LN2)
                {
                    hi = t - LN2_H;
                    lo = LN2_L;
                    k = 1;
                }
                else
                {
                    k = (int)(INV_LN2 * t + 0.5);
                    hi = t - k * LN2_H;
                    lo = k * LN2_L;
                }
                if (x < 0)
                {
                    hi = -hi;
                    lo = -lo;
                    k = -k;
                }
                x = hi - lo;
            }
            else if (t < 1 / TWO_28)
                return 1;
            else
                lo = hi = k = 0;

            // Now x is in primary range.
            t = x * x;
            double c = x - t * (P1 + t * (P2 + t * (P3 + t * (P4 + t * P5))));
            if (k == 0)
                return 1 - (x * c / (c - 2) - x);
            double y = 1 - (lo - x * c / (2 - c) - hi);
            return Scale(y, k);
        }


        /// <summary>
        /// Helper method for scaling a double by a power of 2.
        /// </summary>
        /// <param name="x">the double</param>
        /// <param name="n">the scale; |n| < 2048</param>
        /// <returns>x * 2**n</returns>
        private static double Scale(double x, int n)
        {

            Boolean isDebug = false;

#if DEBUG
            isDebug = true;
#endif


            if (isDebug && Abs(n) >= 2048)
                throw new MathArgumentException(LocalizedResources.Instance().ASSERTION_FAILURE, Abs(n));
            if (x == 0 || x == Double.NegativeInfinity
                || !(x < Double.PositiveInfinity) || n == 0)
                return x;
            long bits = BitConverter.DoubleToInt64Bits(x);
            int exp = (int)(bits >> 52) & 0x7ff;
            if (exp == 0) // Subnormal x.
            {
                x *= TWO_54;
                exp = ((int)(BitConverter.DoubleToInt64Bits(x) >> 52) & 0x7ff) - 54;
            }
            exp += n;
            if (exp > 0x7fe) // Overflow.
                return Double.PositiveInfinity * x;
            if (exp > 0) // Normal.
                return BitConverter.Int64BitsToDouble((long)(((ulong)bits & 0x800fffffffffffffL) | ((ulong)exp << 52)));
            if (exp <= -54)
                return 0 * x; // Underflow.
            exp += 54; // Subnormal result.
            x = BitConverter.Int64BitsToDouble((long)(((ulong)bits & 0x800fffffffffffffL) | ((ulong)exp << 52)));
            return x * (1 / TWO_54);
        }

        /// <summary>
        /// The number of binary digits used to represent the binary number for a decimal precision floating
        /// point value. i.e. there are this many digits used to represent the
        /// actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        const int DecimalWidth = 65;

        /// <summary>
        /// Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
        /// According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// Taking x86 extended precision, 64 bit Significand field and total 80 bit
        /// </summary>
        /// <see cref="https://en.wikipedia.org/wiki/Floating-point_arithmetic"/>
        public static readonly decimal DecimalPrecision = Math2.Pow(2M, -DecimalWidth);

        public static readonly decimal DecimalSqrt2 = 1.4142156862745098039M;

        /// <summary>
        /// The size of a double in bytes.
        /// </summary>
        public const int SizeOfDecimal = sizeof(decimal);

        public static decimal Ceiling(decimal x)
        {
            return System.Decimal.Ceiling(x);
        }

        public static decimal Floor(decimal x)
        {
            return System.Decimal.Floor(x);
        }

        public static decimal Sqrt(decimal x)
        {
            return Sqrt(x, Zero);
        }


        public static decimal Min(decimal a, decimal b)
        {
            if (a > b)
            {
                return b;
            }
            if (a < b)
            {
                return a;
            }
            /* if either arg is NaN, return NaN */
            // Since Decimal has no concept of NaN, this cannot be happened.
            // https://csharpindepth.com/articles/Decimal
            //if (a != b)
            //{
            //    return DecimalNaN;
            //}
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x8000000000000000L == BitConverter.DoubleToInt64Bits(-0.0d) */
            //long zero = BitConverter2.DecimalToInt64Bits(0M);
            //long bits = BitConverter.DoubleToInt64Bits(a);
            long bits = BitConverter2.DecimalToInt64Bits(a);
            if ((ulong)bits == 0x8000000000000000L)
            {
                return a;
            }
            return b;
        }

        public static decimal Max(decimal a, decimal b)
        {
            return System.Math.Max(a, b);
        }

        public static decimal Round(decimal x)
        {
            return System.Decimal.Round(x);
        }

        public static decimal Round(decimal x, int decimals)
        {
            return System.Decimal.Round(x, decimals);
        }

        public static decimal Round(decimal x, MidpointRounding mode)
        {
            return System.Decimal.Round(x, mode);
        }

        public static decimal Round(decimal x, int decimals, MidpointRounding mode)
        {
            return System.Decimal.Round(x, decimals, mode);
        }

        /// <summary>
        /// Analogy of Math.Exp method
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Exp(decimal x)
        {
            var count = 0;

            if (x > One)
            {
                count = decimal.ToInt32(decimal.Truncate(x));
                x -= decimal.Truncate(x);
            }

            if (x < Zero)
            {
                count = decimal.ToInt32(decimal.Truncate(x) - 1);
                x = One + (x - decimal.Truncate(x));
            }

            var iteration = 1;
            var result = One;
            var factorial = One;
            decimal cachedResult;
            do
            {
                cachedResult = result;
                factorial *= x / iteration++;
                result += factorial;
            } while (cachedResult != result);

            if (count == 0)
                return result;
            return result * PowN(DE, count);
        }

        /// <summary>
        /// Analogy of Math.Pow method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static decimal Pow(decimal value, decimal pow)
        {
            if (pow == Zero) return One;
            if (pow == One) return value;
            if (value == One) return One;

            if (value == Zero)
            {
                if (pow > Zero)
                {
                    return Zero;
                }

                throw new Exception(LocalizedResources.Instance().INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER);
            }

            if (pow == -One) return One / value;

            var isPowerInteger = IsInteger(pow);
            if (value < Zero && !isPowerInteger)
            {
                throw new Exception(LocalizedResources.Instance().INVALID_OPERATION_NEGATIVE_BASE_AND_NON_INTEGER_POWER);
            }

            if (isPowerInteger && value > Zero)
            {
                int powerInt = (int)pow;
                return PowN(value, powerInt);
            }

            if (isPowerInteger && value < Zero)
            {
                int powerInt = (int)pow;
                if (powerInt % 2 == 0)
                {
                    return Exp(pow * Log(-value));
                }

                return -Exp(pow * Log(-value));
            }

            return Exp(pow * Log(value));
        }

        private static bool IsInteger(decimal value)
        {
            var longValue = (long)value;
            return Abs(value - longValue) <= DecimalEpsilon;
        }

        /// <summary>
        /// Power to the integer value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static decimal PowN(decimal value, int power)
        {
            while (true)
            {
                if (power == Zero) return One;
                if (power < Zero)
                {
                    value = One / value;
                    power = -power;
                    continue;
                }

                var q = power;
                var prod = One;
                var current = value;
                while (q > 0)
                {
                    if (q % 2 == 1)
                    {
                        // detects the 1s in the binary expression of power
                        prod = current * prod; // picks up the relevant power
                        q--;
                    }

                    current *= current; // value^i -> value^(2*i)
                    q >>= 1;
                }

                return prod;
            }
        }

        /// <summary>
        /// Analogy of Math.Log10
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Log10(decimal x)
        {
            return Log(x) * Log10Inv;
        }

        /// <summary>
        /// Analogy of Math.Log
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Log(decimal x)
        {
            if (x <= Zero)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero, "x"));
            }
            var count = 0;
            while (x >= One)
            {
                x *= Einv;
                count++;
            }
            while (x <= Einv)
            {
                x *= DE;
                count--;
            }
            x--;
            if (x == Zero) return count;
            var result = Zero;
            var iteration = 0;
            var y = One;
            var cacheResult = result - One;
            while (cacheResult != result && iteration < MaxIteration)
            {
                iteration++;
                cacheResult = result;
                y *= -x;
                result += y / iteration;
            }
            return count - result;
        }

        /// <summary>
        /// Analogy of Math.Cos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Cos(decimal x)
        {
            //truncating to  [-2*PI;2*PI]
            TruncateToPeriodicInterval(ref x);

            // now x in (-2pi,2pi)
            if (x >= Pi && x <= PIx2)
            {
                return -Cos(x - Pi);
            }
            if (x >= -PIx2 && x <= -Pi)
            {
                return -Cos(x + Pi);
            }

            x *= x;
            //y=1-x/2!+x^2/4!-x^3/6!...
            var xx = -x * Half;
            var y = One + xx;
            var cachedY = y - One;//init cache  with different value
            for (var i = 1; cachedY != y && i < MaxIteration; i++)
            {
                cachedY = y;
                decimal factor = i * ((i << 1) + 3) + 1; //2i^2+2i+i+1=2i^2+3i+1
                factor = -Half / factor;
                xx *= x * factor;
                y += xx;
            }
            return y;
        }

        /// <summary>
        /// Analogy of Math.Tan
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Tan(decimal x)
        {
            var cos = Cos(x);
            if (cos == Zero) throw new ArgumentException(nameof(x));
            //calculate sin using cos
            var sin = CalculateSinFromCos(x, cos);
            return sin / cos;
        }
        /// <summary>
        /// Helper function for calculating sin(x) from cos(x)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="cos"></param>
        /// <returns></returns>
        private static decimal CalculateSinFromCos(decimal x, decimal cos)
        {
            var moduleOfSin = Sqrt(One - (cos * cos));
            var sineIsPositive = IsSignOfSinePositive(x);
            if (sineIsPositive) return moduleOfSin;
            return -moduleOfSin;
        }
        /// <summary>
        /// Analogy of Math.Sin
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Sin(decimal x)
        {
            var cos = Cos(x);
            return CalculateSinFromCos(x, cos);
        }


        /// <summary>
        /// Truncates to  [-2*PI;2*PI]
        /// </summary>
        /// <param name="x"></param>
        private static void TruncateToPeriodicInterval(ref decimal x)
        {
            while (x >= PIx2)
            {
                var divide = Math.Abs(decimal.ToInt32(x / PIx2));
                x -= divide * PIx2;
            }

            while (x <= -PIx2)
            {
                var divide = Math.Abs(decimal.ToInt32(x / PIx2));
                x += divide * PIx2;
            }
        }


        private static bool IsSignOfSinePositive(decimal x)
        {
            //truncating to  [-2*PI;2*PI]
            TruncateToPeriodicInterval(ref x);

            //now x in [-2*PI;2*PI]
            if (x >= -PIx2 && x <= -Pi) return true;
            if (x >= -Pi && x <= Zero) return false;
            if (x >= Zero && x <= Pi) return true;
            if (x >= Pi && x <= PIx2) return false;

            //will not be reached
            throw new ArgumentException(nameof(x));
        }

        /// <summary>
        /// Analogy of Math.Sqrt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="epsilon">lasts iteration while error less than this epsilon</param>
        /// <returns></returns>
        public static decimal Sqrt(decimal x, decimal epsilon = Zero)
        {
            if (x < Zero) throw new OverflowException(LocalizedResources.Instance().CANNOT_CALCULATE_SQUARE_ROOT_FROM_A_NEGATIVE_NUMBER);
            //initial approximation
            decimal current = (decimal)Math.Sqrt((double)x), previous;
            do
            {
                previous = current;
                if (previous == Zero) return Zero;
                current = (previous + x / previous) * Half;
            } while (Abs(previous - current) > epsilon);
            return current;
        }

        /// <summary>
        /// Analogy of Math.Sinh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Sinh(decimal x)
        {
            var y = Exp(x);
            var yy = One / y;
            return (y - yy) * Half;
        }

        /// <summary>
        /// Analogy of Math.Cosh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Cosh(decimal x)
        {
            var y = Exp(x);
            var yy = One / y;
            return (y + yy) * Half;
        }

        /// <summary>
        /// Analogy of Math.Sign
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Sign(decimal x)
        {
            return x < Zero ? -1 : (x > Zero ? 1 : 0);
        }

        /// <summary>
        /// Analogy of Math.Tanh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Tanh(decimal x)
        {
            var y = Exp(x);
            var yy = One / y;
            return (y - yy) / (y + yy);
        }

        /// <summary>
        /// Analogy of Math.Abs
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Abs(decimal x)
        {
            if (x <= Zero)
            {
                return -x;
            }
            return x;
        }

        /// <summary>
        /// Analogy of Math.Asin
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Asin(decimal x)
        {
            if (x > One || x < -One)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().PARAMETER_MUST_BE_IN, "x", "-1", "1"));
            }
            //known values
            if (x == Zero) return Zero;
            if (x == One) return PIdiv2;
            //asin function is odd function
            if (x < Zero) return -Asin(-x);

            //my optimize trick here

            // used a math formula to speed up :
            // asin(x)=0.5*(pi/2-asin(1-2*x*x)) 
            // if x>=0 is true

            var newX = One - 2 * x * x;

            //for calculating new value near to zero than current
            //because we gain more speed with values near to zero
            if (Abs(x) > Abs(newX))
            {
                var t = Asin(newX);
                return Half * (PIdiv2 - t);
            }
            var y = Zero;
            var result = x;
            decimal cachedResult;
            var i = 1;
            y += result;
            var xx = x * x;
            do
            {
                cachedResult = result;
                result *= xx * (One - Half / (i));
                y += result / ((i << 1) + 1);
                i++;
            } while (cachedResult != result);
            return y;
        }

        /// <summary>
        /// Analogy of Math.Atan
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Atan(decimal x)
        {
            if (x == Zero) return Zero;
            if (x == One) return PIdiv4;
            return Asin(x / Sqrt(One + x * x));
        }
        /// <summary>
        /// Analogy of Math.Acos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Acos(decimal x)
        {
            if (x == Zero) return PIdiv2;
            if (x == One) return Zero;
            if (x < Zero) return Pi - Acos(-x);
            return PIdiv2 - Asin(x);
        }

        /// <summary>
        /// Analogy of Math.Atan2
        /// for more see this
        /// <seealso cref="http://i.imgur.com/TRLjs8R.png"/>
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Atan2(decimal y, decimal x)
        {
            if (x > Zero)
            {
                return Atan(y / x);
            }
            if (x < Zero && y >= Zero)
            {
                return Atan(y / x) + Pi;
            }
            if (x < Zero && y < Zero)
            {
                return Atan(y / x) - Pi;
            }
            if (x == Zero && y > Zero)
            {
                return PIdiv2;
            }
            if (x == Zero && y < Zero)
            {
                return -PIdiv2;
            }
            throw new ArgumentException(String.Format(LocalizedResources.Instance().INVALID_ARGUMENTS, "Atan2"));
        }

        public static double Expm1(double x)
        {
            return Expm1(x, null);
        }

        public static long Factorial(int n)
        {
            long result = (long)System.Math.Round(FactorialDouble(n));
            if (result == long.MaxValue)
            {
                throw new ArithmeticException(LocalizedResources.Instance().RESULT_TOO_LARGE_REPRESENT_IN_A_LONG_INTEGER);
            }
            return result;
        }

        public static double FactorialDouble(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException(LocalizedResources.Instance().MUST_HAVE_N_IS_MORE_THAN_OR_EQUALS_FOR_N_ABSOLUTE);
            }
            return System.Math.Floor(System.Math.Exp(FactorialLog(n)) + 0.5);
        }

        public static double FactorialLog(int n)
        {
            if (n < 0)
            {
                throw new ArithmeticException(LocalizedResources.Instance().MUST_HAVE_N_IS_MORE_THAN_FOR_N_ABSOLUTE);
            }
            double logSum = 0;
            for (int i = 2; i <= n; i++)
            {
                logSum += System.Math.Log((double)i);
            }
            return logSum;
        }

        public static double Floor(double x)
        {
            long y;

            if (Double.IsNaN(x))
            { // NaN
                return x;
            }

            if (x >= TWO_POWER_52 || x <= -TWO_POWER_52)
            {
                return x;
            }

            y = (long)x;
            if (x < 0 && y != x)
            {
                y--;
            }

            if (y == 0)
            {
                return x * y;
            }

            return y;
        }

        public static int FloorDiv(int a, int b)
        {
            if (b == 0)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }

            int m = a % b;
            if ((a ^ b) >= 0 || m == 0)
            {
                // a an b have same sign, or division is exact
                return a / b;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return (a / b) - 1;
            }
        }

        public static long FloorDiv(long a, long b)
        {
            if (b == 0L)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }

            long m = a % b;
            if ((a ^ b) >= 0L || m == 0L)
            {
                // a an b have same sign, or division is exact
                return a / b;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return (a / b) - 1L;
            }
        }

        public static int FloorMod(int a, int b)
        {
            if (b == 0)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }

            int m = a % b;
            if ((a ^ b) >= 0 || m == 0)
            {
                // a an b have same sign, or division is exact
                return m;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return b + m;
            }
        }

        public static long FloorMod(long a, long b)
        {
            if (b == 0L)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }

            long m = a % b;
            if ((a ^ b) >= 0L || m == 0L)
            {
                // a an b have same sign, or division is exact
                return m;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return b + m;
            }
        }

        public static int GetExponent(double d)
        {
            // NaN and Infinite will return 1024 anywho so can use raw bits
            return (int)(LogicalRightShift(BitConverter.DoubleToInt64Bits(d), 52) & 0x7ff) - 1023;
        }

        public static int GetExponent(float f)
        {
            // NaN and Infinite will return the same exponent anywho so can use raw bits
            return ((int)(UInt64.Parse(BitConverter2.FloatToRawIntBits(f).ToString()) >> 23) & 0xff) - 127;
        }

        /// <summary>
        /// 
        /// </summary>Returns an int hash code representing the given double array.
        /// <param name="value">the value to be hashed (may be null)</param>
        /// <returns>the hash code</returns>
        public static int Hash(double[] value)
        {
            return value.GetHashCode();
        }

        ///<summary>
        /// Returns the hypotenuse of a triangle with sides { @code x }
        ///   and {@code y}
        /// - sqrt(<i>x</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)<br/>
        /// avoiding intermediate overflow or underflow.
        ///
        /// <ul>
        /// <li> If either argument is infinite, then the result is positive infinity.</li>
        /// <li> else, if either argument is NaN then the result is NaN.</li>
        /// </ul>
        /// </summary>
        /// <param name="x">a value</param>
        /// <param name="y">a value</param>
        /// <returns>sqrt(<i>x</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)</returns>
        public static double Hypot(double x, double y)
        {
            if (Double.IsInfinity(x) || Double.IsInfinity(y))
            {
                return Double.PositiveInfinity;
            }
            else if (Double.IsNaN(x) || Double.IsNaN(y))
            {
                return Double.NaN;
            }
            else
            {
                #region "Old code from Java" 
                //int expX = GetExponent(x);
                //int expY = GetExponent(y);
                //if (expX > expY + 27)
                //{
                //    // y is neglectible with respect to x
                //    return Abs(x);
                //}
                //else if (expY > expX + 27)
                //{
                //    // x is neglectible with respect to y
                //    return Abs(y);
                //}
                //else
                //{
                //    // find an intermediate scale to avoid both overflow and underflow
                //    int middleExp = (expX + expY) / 2;

                //    // scale parameters without losing precision
                //    double scaledX = Scalb(x, -middleExp);
                //    double scaledY = Scalb(y, -middleExp);

                //    // compute scaled hypotenuse
                //    double scaledH = SquareRoot(scaledX * scaledX + scaledY * scaledY);

                //    // remove scaling
                //    return Scalb(scaledH, middleExp);
                //}
                #endregion
                return System.Math.Sqrt(x * x + y * y);
            }
        }

        public static double IEEEremainder(double dividend, double divisor)
        {
            return System.Math.IEEERemainder(dividend, divisor); // TODO provide our own implementation
        }

        public static double Log(double x)
        {
            return Log(x, null);
        }

        public static double Log(double x, double y)
        {
            return Log(y) / Log(x);
        }

        public static double Log10(double x)
        {
            var hiPrec = new double[2];

            double lores = Log(x, hiPrec);
            if (Double.IsInfinity(lores))
            { // don't allow this to be converted to NaN
                return lores;
            }

            double tmp = hiPrec[0] * HEX_40000000;
            double lna = hiPrec[0] + tmp - tmp;
            double lnb = hiPrec[0] - lna + hiPrec[1];

            double rln10a = 0.4342944622039795;
            double rln10b = 1.9699272335463627E-8;

            return rln10b * lnb + rln10b * lna + rln10a * lnb + rln10a * lna;
        }

        public static double Log1p(double x)
        {
            if (x == -1)
            {
                return Double.NegativeInfinity;
            }

            if (x == Double.PositiveInfinity)
            {
                return Double.PositiveInfinity;
            }

            if (x > 1e-6 ||
                x < -1e-6)
            {
                double xpa = 1 + x;
                double xpb = -(xpa - 1 - x);

                double[] hiPrec = new double[2];
                double lores = Log(xpa, hiPrec);
                if (Double.IsInfinity(lores))
                { // Don't allow this to be converted to NaN
                    return lores;
                }

                // Do a taylor series expansion around xpa:
                //   f(x+y) = f(x) + f'(x) y + f''(x)/2 y^2
                double fx1 = xpb / xpa;
                double epsilon = 0.5 * fx1 + 1;
                return epsilon * fx1 + hiPrec[1] + hiPrec[0];
            }
            else
            {
                // Value is small |x| < 1e6, do a Taylor series centered on 1.
                double y = (x * F_1_3 - F_1_2) * x + 1;
                return y * x;
            }
        }

        public static ulong LogicalRightShift(int x, int y)
        {
            return (UInt64.Parse(x.ToString()) >> y);
        }

        public static ulong LogicalRightShift(long x, int y)
        {
            return (UInt64.Parse(x.ToString()) >> y);
        }

        public static int Max(int a, int b)
        {
            return (a <= b) ? b : a;
        }

        public static long Max(long a, long b)
        {
            return (a <= b) ? b : a;
        }

        public static float Max(float a, float b)
        {
            if (a > b)
            {
                return a;
            }
            if (a < b)
            {
                return b;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return float.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x80000000 == BitConverter2.FloatToRawIntBits(-0.0d) */
            int bits = BitConverter2.FloatToRawIntBits(a);
            if ((uint)bits == 0x80000000)
            {
                return b;
            }
            return a;
        }

        public static double Max(double a, double b)
        {
            if (a > b)
            {
                return a;
            }
            if (a < b)
            {
                return b;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return Double.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x8000000000000000L == BitConverter.DoubleToInt64Bits(-0.0d) */
            long bits = BitConverter.DoubleToInt64Bits(a);
            if ((ulong)bits == 0x8000000000000000L)
            {
                return b;
            }
            return a;
        }

        public static int Min(int a, int b)
        {
            return (a <= b) ? a : b;
        }

        public static long Min(long a, long b)
        {
            return (a <= b) ? a : b;
        }

        public static float Min(float a, float b)
        {
            if (a > b)
            {
                return b;
            }
            if (a < b)
            {
                return a;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return float.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x80000000 == BitConverter2.FloatToRawIntBits(-0.0d) */
            int bits = BitConverter2.FloatToRawIntBits(a);
            if ((uint)bits == 0x80000000)
            {
                return a;
            }
            return b;
        }

        public static double Min(double a, double b)
        {
            if (a > b)
            {
                return b;
            }
            if (a < b)
            {
                return a;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return Double.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x8000000000000000L == BitConverter.DoubleToInt64Bits(-0.0d) */
            long bits = BitConverter.DoubleToInt64Bits(a);
            if ((ulong)bits == 0x8000000000000000L)
            {
                return a;
            }
            return b;
        }

        public static double NextAfter(double d, double direction)
        {
            // handling of some important special cases
            if (Double.IsNaN(d) || Double.IsNaN(direction))
            {
                return Double.NaN;
            }
            else if (d == direction)
            {
                return direction;
            }
            else if (Double.IsInfinity(d))
            {
                return (d < 0) ? -Double.MaxValue : Double.MaxValue;
            }
            else if (d == 0)
            {
                return (direction < 0) ? -Double.MinValue : Double.MinValue;
            }
            // special cases MAX_VALUE to infinity and  MIN_VALUE to 0
            // are handled just as normal numbers
            // can use raw bits since already dealt with infinity and NaN
            long bits = BitConverter.DoubleToInt64Bits(d);
            ulong sign = (ulong)bits & 0x8000000000000000L;
            if ((direction < d) ^ (sign == 0L))
            {
                return BitConverter.Int64BitsToDouble((long)(sign | (ulong)((bits & 0x7fffffffffffffffL) + 1)));
            }
            else
            {
                return BitConverter.Int64BitsToDouble((long)(sign | (ulong)((bits & 0x7fffffffffffffffL) - 1)));
            }
        }

        public static float NextAfter(float f, double direction)
        {
            // handling of some important special cases
            if (Double.IsNaN(f) || Double.IsNaN(direction))
            {
                return float.NaN;
            }
            else if (f == direction)
            {
                return (float)direction;
            }
            else if (float.IsInfinity(f))
            {
                return (f < 0f) ? -float.MaxValue : float.MaxValue;
            }
            else if (f == 0f)
            {
                return (direction < 0) ? -float.MinValue : float.MinValue;
            }
            // special cases MAX_VALUE to infinity and  MIN_VALUE to 0
            // are handled just as normal numbers

            int bits = BitConverter2.FloatToRawIntBits(f);
            uint sign = (uint)bits & 0x80000000;
            if ((direction < f) ^ (sign == 0))
            {
                return BitConverter2.IntBitsToFloat((int)(sign | (uint)((bits & 0x7fffffff) + 1)));
            }
            else
            {
                return BitConverter2.IntBitsToFloat((int)(sign | (uint)((bits & 0x7fffffff) - 1)));
            }
        }

        public static double NextDown(double a)
        {
            return NextAfter(a, Double.NegativeInfinity);
        }

        public static float NextDown(float a)
        {
            return NextAfter(a, float.NegativeInfinity);
        }

        public static double NextUp(double a)
        {
            return NextAfter(a, Double.PositiveInfinity);
        }

        public static float NextUp(float a)
        {
            return NextAfter(a, float.PositiveInfinity);
        }

        /// <summary>
        /// Normalize an angle in a 2&pi; wide interval around a center value.
        /// <p>This method has three main uses:</p>
        /// <ul>
        ///   <li>normalize an angle between 0 and 2&pi;:<br/>
        ///       {@code a = MathUtils.normalizeAngle(a, System.Math.PI);}</li>
        ///   <li>normalize an angle between -&pi; and +&pi;<br/>
        ///       {@code a = MathUtils.normalizeAngle(a, 0.0);}</li>
        ///   <li>compute the angle between two defining angular positions:<br>
        ///       {@code angle = MathUtils.normalizeAngle(end, start) - start;}</li>
        /// </ul>
        /// <p>Note that due to numerical accuracy and since &pi; cannot be represented
        /// exactly, the result interval is <em>closed</em>, it cannot be half-closed
        /// as would be more satisfactory in a purely mathematical view.</p>
        /// </summary>
        /// <param name="a">angle to normalize</param>
        /// <param name="center">center of the desired 2&pi; interval for the result</param>
        /// <returns>a-2k&pi; with int k and center-&pi; &lt;= a-2k&pi; &lt;= center+&pi;</returns>
        public static double NormalizeAngle(double a, double center)
        {
            return a - TWO_PI * System.Math.Floor((a + System.Math.PI - center) / TWO_PI);
        }

        public static double Pow(double x, double y)
        {
            if (y == 0)
            {
                // y = -0 or y = +0
                return 1.0;
            }
            else
            {
                long yBits = BitConverter.DoubleToInt64Bits(y);
                int yRawExp = (int)((yBits & MASK_DOUBLE_EXPONENT) >> 52);
                long yRawMantissa = yBits & MASK_DOUBLE_MANTISSA;
                long xBits = BitConverter.DoubleToInt64Bits(x);
                int xRawExp = (int)((xBits & MASK_DOUBLE_EXPONENT) >> 52);
                long xRawMantissa = xBits & MASK_DOUBLE_MANTISSA;

                if (yRawExp > 1085)
                {
                    // y is either a very large integral value that does not fit in a long or it is a special number

                    if ((yRawExp == 2047 && yRawMantissa != 0) ||
                        (xRawExp == 2047 && xRawMantissa != 0))
                    {
                        // NaN
                        return Double.NaN;
                    }
                    else if (xRawExp == 1023 && xRawMantissa == 0)
                    {
                        // x = -1.0 or x = +1.0
                        if (yRawExp == 2047)
                        {
                            // y is infinite
                            return Double.NaN;
                        }
                        else
                        {
                            // y is a large even int
                            return 1.0;
                        }
                    }
                    else
                    {
                        // the absolute value of x is either greater or smaller than 1.0

                        // if yRawExp == 2047 and mantissa is 0, y = -infinity or y = +infinity
                        // if 1085 < yRawExp < 2047, y is simply a large number, however, due to limited
                        // accuracy, at this magnitude it behaves just like infinity with regards to x
                        if ((y > 0) ^ (xRawExp < 1023))
                        {
                            // either y = +infinity (or large engouh) and abs(x) > 1.0
                            // or     y = -infinity (or large engouh) and abs(x) < 1.0
                            return Double.PositiveInfinity;
                        }
                        else
                        {
                            // either y = +infinity (or large engouh) and abs(x) < 1.0
                            // or     y = -infinity (or large engouh) and abs(x) > 1.0
                            return +0.0;
                        }
                    }
                }
                else
                {
                    // y is a regular non-zero number

                    if (yRawExp >= 1023)
                    {
                        // y may be an integral value, which should be handled specifically
                        long yFullMantissa = IMPLICIT_HIGH_BIT | yRawMantissa;
                        if (yRawExp < 1075)
                        {
                            // normal number with negative shift that may have a fractional part
                            long integralMask = (-1L) << (1075 - yRawExp);
                            if ((yFullMantissa & integralMask) == yFullMantissa)
                            {
                                // all fractional bits are 0, the number is really integral
                                long l = yFullMantissa >> (1075 - yRawExp);
                                return System.Math.Pow(x, (y < 0) ? -l : l);
                            }
                        }
                        else
                        {
                            // normal number with positive shift, always an integral value
                            // we know it fits in a primitive long because yRawExp > 1085 has been handled above
                            long l = yFullMantissa << (yRawExp - 1075);
                            return System.Math.Pow(x, (y < 0) ? -l : l);
                        }
                    }

                    // y is a non-integral value

                    if (x == 0)
                    {
                        // x = -0 or x = +0
                        // the int powers have already been handled above
                        return y < 0 ? Double.PositiveInfinity : +0.0;
                    }
                    else if (xRawExp == 2047)
                    {
                        if (xRawMantissa == 0)
                        {
                            // x = -infinity or x = +infinity
                            return (y < 0) ? +0.0 : Double.PositiveInfinity;
                        }
                        else
                        {
                            // NaN
                            return Double.NaN;
                        }
                    }
                    else if (x < 0)
                    {
                        // the int powers have already been handled above
                        return Double.NaN;
                    }
                    else
                    {
                        // this is the general case, for regular fractional numbers x and y

                        // Split y into ya and yb such that y = ya+yb
                        double tmp = y * HEX_40000000;
                        double ya = (y + tmp) - tmp;
                        double yb = y - ya;

                        /* Compute ln(x) */
                        double[] lns = new double[2];
                        double lores = Log(x, lns);
                        if (Double.IsInfinity(lores))
                        { // don't allow this to be converted to NaN
                            return lores;
                        }

                        double lna = lns[0];
                        double lnb = lns[1];

                        /* resplit lns */
                        double tmp1 = lna * HEX_40000000;
                        double tmp2 = (lna + tmp1) - tmp1;
                        lnb += lna - tmp2;
                        lna = tmp2;

                        // y*ln(x) = (aa+ab)
                        double aa = lna * ya;
                        double ab = lna * yb + lnb * ya + lnb * yb;

                        lna = aa + ab;
                        lnb = -(lna - aa - ab);

                        double z = 1.0 / 120.0;
                        z = z * lnb + (1.0 / 24.0);
                        z = z * lnb + (1.0 / 6.0);
                        z = z * lnb + 0.5;
                        z = z * lnb + 1.0;
                        z *= lnb;

                        double result = Exp(lna, z, null);
                        //result = result + result * z;
                        return result;
                    }
                }
            }
        }

        public static double Pow(double d, int e)
        {
            return Pow(d, (long)e);
        }

        public static double Pow(double d, long e)
        {
            if (e == 0)
            {
                return 1.0;
            }
            else if (e > 0)
            {
                return new Split(d).Pow(e).Full;
            }
            else
            {
                return new Split(d).Reciprocal().Pow(-e).Full;
            }
        }

        public static double Random()
        {
            //Random rnd = new Random();
            lock (rnd)
            {
                return rnd.NextDouble();
            }
        }

        /// <summary>
        /// <p>Reduce {@code |a - offset|} to the primary interval {@code [0, |period|)}.</p>
        /// <p>Specifically, the value returned is <br/>
        /// {@code a - |period| * floor((a - offset) / |period|) - offset}.</p>
        /// <p>If any of the parameters are {@code NaN} or infinite, the result is {@code NaN}.</p>
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="a">Value to reduce.</param>
        /// <param name="period">Period.</param>
        /// <param name="offset">Value that will be mapped to {@code 0}.</param>
        /// <returns>the value, within the interval {@code [0 |period|)}, that corresponds to {@code a}.</returns>
        public static double Reduce(double a, double period, double offset)
        {
            double p = System.Math.Abs(period);
            return a - p * System.Math.Floor((a - offset) / p) - offset;
        }

        public static double Rint(double x)
        {
            double y = Floor(x);
            double d = x - y;

            if (d > 0.5)
            {
                if (y == -1.0)
                {
                    return -0.0; // Preserve sign of operand
                }
                return y + 1.0;
            }
            if (d < 0.5)
            {
                return y;
            }

            /* half way, round to even */
            long z = (long)y;
            return (z & 1) == 0 ? y : y + 1.0;
        }

        public static long Round(double x)
        {
            return (long)Floor(x + 0.5);
        }

        public static int Round(float x)
        {
            return (int)Floor(x + 0.5f);
        }

        public static double Scalb(double d, int n)
        {
            // first simple and fast handling when 2^n can be represented using normal numbers
            if ((n > -1023) && (n < 1024))
            {
                return d * BitConverter.Int64BitsToDouble(((long)(n + 1023)) << 52);
            }

            // handle special cases
            if (Double.IsNaN(d) || Double.IsInfinity(d) || (d == 0))
            {
                return d;
            }
            if (n < -2098)
            {
                return (d > 0) ? 0.0 : -0.0;
            }
            if (n > 2097)
            {
                return (d > 0) ? Double.PositiveInfinity : Double.NegativeInfinity;
            }

            // decompose d
            long bits = BitConverter.DoubleToInt64Bits(d);
            long sign = (long)((ulong)bits & 0x8000000000000000L);
            int exponent = ((int)(LogicalRightShift(bits, 52))) & 0x7ff;
            long mantissa = bits & 0x000fffffffffffffL;

            // compute scaled exponent
            int scaledExponent = exponent + n;

            if (n < 0)
            {
                // we are really in the case n <= -1023
                if (scaledExponent > 0)
                {
                    // both the input and the result are normal numbers, we only adjust the exponent
                    return BitConverter.Int64BitsToDouble(sign | (((long)scaledExponent) << 52) | mantissa);
                }
                else if (scaledExponent > -53)
                {
                    // the input is a normal number and the result is a subnormal number

                    // recover the hidden mantissa bit
                    mantissa |= 1L << 52;

                    // scales down complete mantissa, hence losing least significant bits
                    long mostSignificantLostBit = mantissa & (1L << (-scaledExponent));
                    mantissa = (long)LogicalRightShift(mantissa, (1 - scaledExponent));  //mantissa >>>= 1 - scaledExponent;
                    if (mostSignificantLostBit != 0)
                    {
                        // we need to add 1 bit to round up the result
                        mantissa++;
                    }
                    return BitConverter.Int64BitsToDouble(sign | mantissa);
                }
                else
                {
                    // no need to compute the mantissa, the number scales down to 0
                    return (sign == 0L) ? 0.0 : -0.0;
                }
            }
            else
            {
                // we are really in the case n >= 1024
                if (exponent == 0)
                {
                    // the input number is subnormal, normalize it
                    while ((long)LogicalRightShift(mantissa, 52) != 1)
                    {
                        mantissa <<= 1;
                        --scaledExponent;
                    }
                    ++scaledExponent;
                    mantissa &= 0x000fffffffffffffL;

                    if (scaledExponent < 2047)
                    {
                        return BitConverter.Int64BitsToDouble(sign | (((long)scaledExponent) << 52) | mantissa);
                    }
                    else
                    {
                        return (sign == 0L) ? Double.PositiveInfinity : Double.NegativeInfinity;
                    }
                }
                else if (scaledExponent < 2047)
                {
                    return BitConverter.Int64BitsToDouble(sign | (((long)scaledExponent) << 52) | mantissa);
                }
                else
                {
                    return (sign == 0L) ? Double.PositiveInfinity : Double.NegativeInfinity;
                }
            }
        }

        public static float Scalb(float f, int n)
        {
            // first simple and fast handling when 2^n can be represented using normal numbers
            if ((n > -127) && (n < 128))
            {
                return f * BitConverter2.IntBitsToFloat((n + 127) << 23);
            }

            // handle special cases
            if (float.IsNaN(f) || float.IsInfinity(f) || (f == 0f))
            {
                return f;
            }
            if (n < -277)
            {
                return (f > 0) ? 0.0f : -0.0f;
            }
            if (n > 276)
            {
                return (f > 0) ? float.PositiveInfinity : float.NegativeInfinity;
            }

            // decompose f
            int bits = BitConverter2.FloatToRawIntBits(f);
            uint sign = (uint)bits & 0x80000000;
            int exponent = (int)(UInt64.Parse(bits.ToString()) >> 23) & 0xff;
            int mantissa = bits & 0x007fffff;

            // compute scaled exponent
            int scaledExponent = exponent + n;

            if (n < 0)
            {
                // we are really in the case n <= -127
                if (scaledExponent > 0)
                {
                    // both the input and the result are normal numbers, we only adjust the exponent
                    return BitConverter2.IntBitsToFloat((int)(sign | (uint)(scaledExponent << 23) | (uint)mantissa));
                }
                else if (scaledExponent > -24)
                {
                    // the input is a normal number and the result is a subnormal number

                    // recover the hidden mantissa bit
                    mantissa |= 1 << 23;

                    // scales down complete mantissa, hence losing least significant bits
                    int mostSignificantLostBit = mantissa & (1 << (-scaledExponent));
                    mantissa = (int)LogicalRightShift(mantissa, (1 - scaledExponent));  //mantissa >>>= 1 - scaledExponent;
                    if (mostSignificantLostBit != 0)
                    {
                        // we need to add 1 bit to round up the result
                        mantissa++;
                    }
                    return BitConverter2.IntBitsToFloat((int)(sign | (uint)mantissa));
                }
                else
                {
                    // no need to compute the mantissa, the number scales down to 0
                    return (sign == 0) ? 0.0f : -0.0f;
                }
            }
            else
            {
                // we are really in the case n >= 128
                if (exponent == 0)
                {
                    // the input number is subnormal, normalize it
                    while ((long)LogicalRightShift(mantissa, 23) != 1)
                    {
                        mantissa <<= 1;
                        --scaledExponent;
                    }
                    ++scaledExponent;
                    mantissa &= 0x007fffff;

                    if (scaledExponent < 255)
                    {
                        return BitConverter2.IntBitsToFloat((int)(sign | (uint)(scaledExponent << 23) | (uint)mantissa));
                    }
                    else
                    {
                        return (sign == 0) ? float.PositiveInfinity : float.NegativeInfinity;
                    }
                }
                else if (scaledExponent < 255)
                {
                    return BitConverter2.IntBitsToFloat((int)(sign | (uint)(scaledExponent << 23) | (uint)mantissa));
                }
                else
                {
                    return (sign == 0) ? float.PositiveInfinity : float.NegativeInfinity;
                }
            }
        }

        public static double Signum(double a)
        {
            return (a < 0.0) ? -1.0 : ((a > 0.0) ? 1.0 : a); // return +0.0/-0.0/NaN depending on a
        }

        public static float Signum(float a)
        {
            return (a < 0.0f) ? -1.0f : ((a > 0.0f) ? 1.0f : a); // return +0.0/-0.0/NaN depending on a
        }

        public static double SquareRoot(double a)
        {
            return System.Math.Sqrt(a);
        }

        public static double Square(double value)
        {
            return value * value;
        }

        public static double ToDegrees(double x)
        {
            if (Double.IsInfinity(x) || x == 0.0)
            { // Matches +/- 0.0; return correct sign
                return x;
            }

            // These are 180/PI split into high and low order bits
            double facta = 57.2957763671875;
            double factb = 3.145894820876798E-6;

            double xa = DoubleHighPart(x);
            double xb = x - xa;

            return xb * factb + xb * facta + xa * factb + xa * facta;
        }

        public static int ToIntExact(long n)
        {
            if (n < int.MinValue || n > int.MaxValue)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW);
            }
            return (int)n;
        }

        public static double ToRadians(double x)
        {
            if (Double.IsInfinity(x) || x == 0.0)
            { // Matches +/- 0.0; return correct sign
                return x;
            }

            // These are PI/180 split into high and low order bits
            double facta = 0.01745329052209854;
            double factb = 1.997844754509471E-9;

            double xa = DoubleHighPart(x);
            double xb = x - xa;

            double result = xb * factb + xb * facta + xa * factb + xa * facta;
            if (result == 0)
            {
                result *= x; // ensure correct sign if calculation underflows
            }
            return result;
        }

        //public static double Ulp(double x)
        //{
        //    if (Double.IsInfinity(x))
        //    {
        //        return Double.PositiveInfinity;
        //    }
        //    return Abs(x - BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) ^ 1));
        //}

        public static float Ulp(float x)
        {
            if (float.IsInfinity(x))
            {
                return float.PositiveInfinity;
            }
            return Abs(x - BitConverter2.IntBitsToFloat(BitConverter2.FloatToRawIntBits(x) ^ 1));
        }

        public static double Ulp(Double value)
        {
            //
            //long bits = BitConverter.DoubleToInt64Bits(value);
            //double nextValue = BitConverter.Int64BitsToDouble(bits + 1);
            //double result = nextValue - value;
            //return result;

            // Im order to fix the calculate with a negative value, convert to positive if needed.
            long bits = BitConverter.DoubleToInt64Bits(value);
            if ((bits & 0x7FF0000000000000L) == 0x7FF0000000000000L)
            { // if x is not finite
                if (bits == 0x000FFFFFFFFFFFFFL)
                { // if x is a NaN
                    return value;  // I did not force the sign bit here with NaNs.
                }
                return BitConverter.Int64BitsToDouble(0x7FF0000000000000L); // Positive Infinity;
            }
            bits &= 0x7FFFFFFFFFFFFFFfL; // make positive
            if (bits == 0x7FEFFFFFFFFFFFFL)
            { // if x == max_double (notice the _E_)
                return BitConverter.Int64BitsToDouble(bits) - BitConverter.Int64BitsToDouble(bits - 1);
            }
            double nextValue = BitConverter.Int64BitsToDouble(bits + 1);
            return nextValue - value;
        }

        private static double DoubleHighPart(double d)
        {
            if (d > -Precision2.SAFE_MIN && d < Precision2.SAFE_MIN)
            {
                return d; // These are un-normalised - don't try to convert
            }
            long xl = BitConverter.DoubleToInt64Bits(d); // can take raw bits because just gonna convert it back
            xl &= MASK_30BITS; // Drop low order bits
            return BitConverter.Int64BitsToDouble(xl);
        }

        private static double Exp(double x, double extra, double[] hiPrec)
        {
            double intPartA;
            double intPartB;
            int intVal = (int)x;

            /**
             * Lookup exp(floor(x)).
             * intPartA will have the upper 22 bits, intPartB will have the lower
             * 52 bits.
             */
            if (x < 0.0)
            {
                // We don't check against intVal here as conversion of large negative double values
                // may be affected by a JIT bugd Subsequent comparisons can safely use intVal
                if (x < -746d)
                {
                    if (hiPrec != null)
                    {
                        hiPrec[0] = 0.0;
                        hiPrec[1] = 0.0;
                    }
                    return 0.0;
                }

                if (intVal < -709)
                {
                    /* This will produce a subnormal output */
                    double result1 = Exp(x + 40.19140625, extra, hiPrec) / 285040095144011776.0;
                    if (hiPrec != null)
                    {
                        hiPrec[0] /= 285040095144011776.0;
                        hiPrec[1] /= 285040095144011776.0;
                    }
                    return result1;
                }

                if (intVal == -709)
                {
                    /* exp(1.494140625) is nearly a machine number..d */
                    double result2 = Exp(x + 1.494140625, extra, hiPrec) / 4.455505956692756620;
                    if (hiPrec != null)
                    {
                        hiPrec[0] /= 4.455505956692756620;
                        hiPrec[1] /= 4.455505956692756620;
                    }
                    return result2;
                }

                intVal--;
            }
            else
            {
                if (intVal > 709)
                {
                    if (hiPrec != null)
                    {
                        hiPrec[0] = Double.PositiveInfinity;
                        hiPrec[1] = 0.0;
                    }
                    return Double.PositiveInfinity;
                }
            }

            intPartA = ExpIntTable.GetInstance().EXP_INT_TABLE_A[EXP_INT_TABLE_MAX_INDEX + intVal];
            intPartB = ExpIntTable.GetInstance().EXP_INT_TABLE_B[EXP_INT_TABLE_MAX_INDEX + intVal];

            /* Get the fractional part of x, find the greatest multiple of 2^-10 less than
             * x and look up the exp function of it.
             * fracPartA will have the upper 22 bits, fracPartB the lower 52 bits.
             */
            int intFrac = (int)((x - intVal) * 1024.0);
            double fracPartA = ExpFracTable.GetInstance().EXP_FRAC_TABLE_A[intFrac];
            double fracPartB = ExpFracTable.GetInstance().EXP_FRAC_TABLE_B[intFrac];

            /**
             * epsilon is the difference in x from the nearest multiple of 2^-10d  It
             * has a value in the range 0 <= epsilon < 2^-10.
             * Do the subtraction from x as the last step to avoid possible loss of precision.
             */
            double epsilon = x - (intVal + intFrac / 1024.0);

            /**
             * Compute z = exp(epsilon) - 1.0 via a minimax polynomiald  z has
           full double precision (52 bits)d  Since z < 2^-10, we will have
           62 bits of precision when combined with the constant 1d  This will be
           used in the last addition below to get proper roundingd
            */

            /**
             * Remez generated polynomiald  Converges on the interval [0, 2^-10], error
           is less than 0.5 ULP
            */
            double z = 0.04168701738764507;
            z = z * epsilon + 0.1666666505023083;
            z = z * epsilon + 0.5000000000042687;
            z = z * epsilon + 1.0;
            z = z * epsilon + -3.940510424527919E-20;

            /** 
             * Compute (intPartA+intPartB) * (fracPartA+fracPartB) by binomial
           expansion.
           tempA is exact since intPartA and intPartB only have 22 bits each.
           tempB will have 52 bits of precision.
             */
            double tempA = intPartA * fracPartA;
            double tempB = intPartA * fracPartB + intPartB * fracPartA + intPartB * fracPartB;

            /**
             * Compute the resultd  (1+z)(tempA+tempB)d  Order of operations is
           importantd  For accuracy add by increasing sized  tempA is exact and
           much larger than the othersd  If there are extra bits specified from the
           pow() function, use themd
            */
            double tempC = tempB + tempA;

            // If tempC is positive infinite, the evaluation below could result in NaN,
            // because z could be negative at the same time.
            if (tempC == Double.PositiveInfinity)
            {
                return Double.PositiveInfinity;
            }

            double result;
            if (extra != 0.0)
            {
                result = tempC * extra * z + tempC * extra + tempC * z + tempB + tempA;
            }
            else
            {
                result = tempC * z + tempB + tempA;
            }

            if (hiPrec != null)
            {
                // If requesting high precision
                hiPrec[0] = tempA;
                hiPrec[1] = tempC * extra * z + tempC * extra + tempC * z + tempB;
            }

            return result;
        }

        private static double Expint(int p, ref double[] result)
        {
            //double x = M_E;
            double[] xs = new double[2];
            double[] aa = new double[2];
            double[] ys = new double[2];
            //split(x, xs);
            //xs[1] = (double)(2.7182818284590452353602874713526625L - xs[0]);
            //xs[0] = 2.71827697753906250000;
            //xs[1] = 4.85091998273542816811e-06;
            //xs[0] = Double.longBitsToDouble(0x4005bf0800000000L);
            //xs[1] = Double.longBitsToDouble(0x3ed458a2bb4a9b00L);

            /* E */
            xs[0] = 2.718281828459045;
            xs[1] = 1.4456468917292502E-16;

            ComputeSplit(1.0, ys);

            while (p > 0)
            {
                if ((p & 1) != 0)
                {
                    QuadMult(ys, xs, ref aa);
                    ys[0] = aa[0]; ys[1] = aa[1];
                }

                QuadMult(xs, xs, ref aa);
                xs[0] = aa[0]; xs[1] = aa[1];

                p >>= 1;
            }

            if (result != null)
            {
                result[0] = ys[0];
                result[1] = ys[1];

                Resplit(ref result);
            }

            return ys[0] + ys[1];
        }

        private static double Expm1(double x, double[] hiPrecOut)
        {
            if (Double.IsNaN(x) || x == 0.0)
            { // NaN or zero
                return x;
            }

            if (x <= -1.0 || x >= 1.0)
            {
                // If not between +/- 1.0
                //return exp(x) - 1.0;
                var hiPrec = new double[2];
                Exp(x, 0.0, hiPrec);
                if (x > 0.0)
                {
                    return -1.0 + hiPrec[0] + hiPrec[1];
                }
                else
                {
                    double ra = -1.0 + hiPrec[0];
                    double rb = -(ra + 1.0 - hiPrec[0]);
                    rb += hiPrec[1];
                    return ra + rb;
                }
            }

            double baseA;
            double baseB;
            double epsilon;
            Boolean negative = false;

            if (x < 0.0)
            {
                x = -x;
                negative = true;
            }

            {
                int intFrac = (int)(x * 1024.0);
                double tempA = ExpFracTable.GetInstance().EXP_FRAC_TABLE_A[intFrac] - 1.0;
                double tempB = ExpFracTable.GetInstance().EXP_FRAC_TABLE_B[intFrac];

                double temp1 = tempA + tempB;
                tempB = -(temp1 - tempA - tempB);
                tempA = temp1;

                temp1 = tempA * HEX_40000000;
                baseA = tempA + temp1 - temp1;
                baseB = tempB + (tempA - baseA);

                epsilon = x - intFrac / 1024.0;
            }

            /* Compute expm1(epsilon) */
            double zb = 0.008336750013465571;
            zb = zb * epsilon + 0.041666663879186654;
            zb = zb * epsilon + 0.16666666666745392;
            zb = zb * epsilon + 0.49999999999999994;
            zb *= epsilon;
            zb *= epsilon;

            double za = epsilon;
            double temp2 = za + zb;
            zb = -(temp2 - za - zb);
            za = temp2;

            temp2 = za * HEX_40000000;
            temp2 = za + temp2 - temp2;
            zb += za - temp2;
            za = temp2;

            /* Combine the partsd   expm1(a+b) = expm1(a) + expm1(b) + expm1(a)*expm1(b) */
            double ya = za * baseA;
            //double yb = za*baseB + zb*baseA + zb*baseB;
            temp2 = ya + za * baseB;
            double yb = -(temp2 - ya - za * baseB);
            ya = temp2;

            temp2 = ya + zb * baseA;
            yb += -(temp2 - ya - zb * baseA);
            ya = temp2;

            temp2 = ya + zb * baseB;
            yb += -(temp2 - ya - zb * baseB);
            ya = temp2;

            //ya = ya + za + baseA;
            //yb = yb + zb + baseB;
            temp2 = ya + baseA;
            yb += -(temp2 - baseA - ya);
            ya = temp2;

            temp2 = ya + za;
            //yb += (ya > za) ? -(temp - ya - za) : -(temp - za - ya);
            yb += -(temp2 - ya - za);
            ya = temp2;

            temp2 = ya + baseB;
            //yb += (ya > baseB) ? -(temp - ya - baseB) : -(temp - baseB - ya);
            yb += -(temp2 - ya - baseB);
            ya = temp2;

            temp2 = ya + zb;
            //yb += (ya > zb) ? -(temp - ya - zb) : -(temp - zb - ya);
            yb += -(temp2 - ya - zb);
            ya = temp2;

            if (negative)
            {
                /* Compute expm1(-x) = -expm1(x) / (expm1(x) + 1) */
                double denom = 1.0 + ya;
                double denomr = 1.0 / denom;
                double denomb = -(denom - 1.0 - ya) + yb;
                double ratio = ya * denomr;
                temp2 = ratio * HEX_40000000;
                double ra = ratio + temp2 - temp2;
                double rb = ratio - ra;

                temp2 = denom * HEX_40000000;
                za = denom + temp2 - temp2;
                zb = denom - za;

                rb += (ya - za * ra - za * rb - zb * ra - zb * rb) * denomr;

                // f(x) = x/1+x
                // Compute f'(x)
                // Product rule:  d(uv) = du*v + u*dv
                // Chain rule:  d(f(g(x)) = f'(g(x))*f(g'(x))
                // d(1/x) = -1/(x*x)
                // d(1/1+x) = -1/( (1+x)^2) *  1 =  -1/((1+x)*(1+x))
                // d(x/1+x) = -x/((1+x)(1+x)) + 1/1+x = 1 / ((1+x)(1+x))

                // Adjust for yb
                rb += yb * denomr;                      // numerator
                rb += -ya * denomb * denomr * denomr;   // denominator

                // negate
                ya = -ra;
                yb = -rb;
            }

            if (hiPrecOut != null)
            {
                hiPrecOut[0] = ya;
                hiPrecOut[1] = yb;
            }

            return ya + yb;
        }

        private static double Log(double x, double[] hiPrec)
        {
            if (x == 0)
            { // Handle special case of +0/-0
                return Double.NegativeInfinity;
            }
            long bits = BitConverter.DoubleToInt64Bits(x);

            /* Handle special cases of negative input, and NaN */
            if ((((ulong)bits & 0x8000000000000000L) != 0 || Double.IsNaN(x)) && x != 0.0)
            {
                if (hiPrec != null)
                {
                    hiPrec[0] = Double.NaN;
                }

                return Double.NaN;
            }

            /* Handle special cases of Positive infinityd */
            if (x == Double.PositiveInfinity)
            {
                if (hiPrec != null)
                {
                    hiPrec[0] = Double.PositiveInfinity;
                }

                return Double.PositiveInfinity;
            }

            /* Extract the exponent */
            int exp = (int)(bits >> 52) - 1023;

            if ((bits & 0x7ff0000000000000L) == 0)
            {
                // Subnormal!
                if (x == 0)
                {
                    // Zero
                    if (hiPrec != null)
                    {
                        hiPrec[0] = Double.NegativeInfinity;
                    }

                    return Double.NegativeInfinity;
                }

                /* Normalize the subnormal numberd */
                bits <<= 1;
                while ((bits & 0x0010000000000000L) == 0)
                {
                    --exp;
                    bits <<= 1;
                }
            }

            if ((exp == -1 || exp == 0) && x < 1.01 && x > 0.99 && hiPrec == null)
            {
                /* The normal method doesn't work well in the range [0.99, 1.01], so call do a straight
               polynomial expansion in higer precisiond */

                /* Compute x - 1.0 and split it */
                double xa = x - 1.0;
                double xb = xa - x + 1.0;
                double tmp = xa * HEX_40000000;
                double aa = xa + tmp - tmp;
                double ab = xa - aa;
                xa = aa;
                xb = ab;

                double[] lnCoef_last = LN_QUICK_COEF[LN_QUICK_COEF.Length - 1];
                double ya = lnCoef_last[0];
                double yb = lnCoef_last[1];

                for (int i = LN_QUICK_COEF.Length - 2; i >= 0; i--)
                {
                    /* Multiply a = y * x */
                    aa = ya * xa;
                    ab = ya * xb + yb * xa + yb * xb;
                    /* split, so now y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;

                    /* Add  a = y + lnQuickCoef */
                    double[] lnCoef_i = LN_QUICK_COEF[i];
                    aa = ya + lnCoef_i[0];
                    ab = yb + lnCoef_i[1];
                    /* Split y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;
                }

                /* Multiply a = y * x */
                aa = ya * xa;
                ab = ya * xb + yb * xa + yb * xb;
                /* split, so now y = a */
                tmp = aa * HEX_40000000;
                ya = aa + tmp - tmp;
                yb = aa - ya + ab;

                return ya + yb;
            }

            // lnm is a log of a number in the range of 1.0 - 2.0, so 0 <= lnm < ln(2)
            double[] lnm = lnMant.GetInstance().LN_MANT[(int)((bits & 0x000ffc0000000000L) >> 42)];

            /*
        double epsilon = x / BitConverter.Int64BitsToDouble(bits & 0xfffffc0000000000L);

        epsilon -= 1.0;
             */

            // y is the most significant 10 bits of the mantissa
            //double y = BitConverter.Int64BitsToDouble(bits & 0xfffffc0000000000L);
            //double epsilon = (x - y) / y;
            double epsilon = (bits & 0x3ffffffffffL) / (TWO_POWER_52 + (bits & 0x000ffc0000000000L));

            double lnza = 0.0;
            double lnzb = 0.0;

            if (hiPrec != null)
            {
                /* split epsilon -> x */
                double tmp = epsilon * HEX_40000000;
                double aa = epsilon + tmp - tmp;
                double ab = epsilon - aa;
                double xa = aa;
                double xb = ab;

                /* Need a more accurate epsilon, so adjust the divisiond */
                double numer = bits & 0x3ffffffffffL;
                double denom = TWO_POWER_52 + (bits & 0x000ffc0000000000L);
                aa = numer - xa * denom - xb * denom;
                xb += aa / denom;

                /* Remez polynomial evaluation */
                double[] lnCoef_last = LN_HI_PREC_COEF[LN_HI_PREC_COEF.Length - 1];
                double ya = lnCoef_last[0];
                double yb = lnCoef_last[1];

                for (int i = LN_HI_PREC_COEF.Length - 2; i >= 0; i--)
                {
                    /* Multiply a = y * x */
                    aa = ya * xa;
                    ab = ya * xb + yb * xa + yb * xb;
                    /* split, so now y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;

                    /* Add  a = y + lnHiPrecCoef */
                    double[] lnCoef_i = LN_HI_PREC_COEF[i];
                    aa = ya + lnCoef_i[0];
                    ab = yb + lnCoef_i[1];
                    /* Split y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;
                }

                /* Multiply a = y * x */
                aa = ya * xa;
                ab = ya * xb + yb * xa + yb * xb;

                /* split, so now lnz = a */
                /*
          tmp = aa * 1073741824.0;
          lnza = aa + tmp - tmp;
          lnzb = aa - lnza + ab;
                 */
                lnza = aa + ab;
                lnzb = -(lnza - aa - ab);
            }
            else
            {
                /* High precision not requiredd  Eval Remez polynomial
             using standard double precision */
                lnza = -0.16624882440418567;
                lnza = lnza * epsilon + 0.19999954120254515;
                lnza = lnza * epsilon + -0.2499999997677497;
                lnza = lnza * epsilon + 0.3333333333332802;
                lnza = lnza * epsilon + -0.5;
                lnza = lnza * epsilon + 1.0;
                lnza *= epsilon;
            }

            /* Relative sizes:
             * lnzb     [0, 2.33E-10]
             * lnm[1]   [0, 1.17E-7]
             * ln2B*exp [0, 1.12E-4]
             * lnza      [0, 9.7E-4]
             * lnm[0]   [0, 0.692]
             * ln2A*exp [0, 709]
             */

            /* Compute the following sum:
             * lnzb + lnm[1] + ln2B*exp + lnza + lnm[0] + ln2A*exp;
             */

            //return lnzb + lnm[1] + ln2B*exp + lnza + lnm[0] + ln2A*exp;
            double a = LN_2_A * exp;
            double b = 0.0;
            double c = a + lnm[0];
            double d = -(c - a - lnm[0]);
            a = c;
            b += d;

            c = a + lnza;
            d = -(c - a - lnza);
            a = c;
            b += d;

            c = a + LN_2_B * exp;
            d = -(c - a - LN_2_B * exp);
            a = c;
            b += d;

            c = a + lnm[1];
            d = -(c - a - lnm[1]);
            a = c;
            b += d;

            c = a + lnzb;
            d = -(c - a - lnzb);
            a = c;
            b += d;

            if (hiPrec != null)
            {
                hiPrec[0] = a;
                hiPrec[1] = b;
            }

            return a + b;
        }

        private static double PolyCosine(double x)
        {
            double x2 = x * x;

            double p = 2.479773539153719E-5;
            p = p * x2 + -0.0013888888689039883;
            p = p * x2 + 0.041666666666621166;
            p = p * x2 + -0.49999999999999994;
            p *= x2;

            return p;
        }

        private static double PolySine(double x)
        {
            double x2 = x * x;

            double p = 2.7553817452272217E-6;
            p = p * x2 + -1.9841269659586505E-4;
            p = p * x2 + 0.008333333333329196;
            p = p * x2 + -0.16666666666666666;
            //p *= x2;
            //p *= x;
            p = p * x2 * x;

            return p;
        }

        private static void QuadMult(double[] a, double[] b, ref double[] result)
        {
            double[] xs = new double[2];
            double[] ys = new double[2];
            double[] zs = new double[2];

            /* a[0] * b[0] */
            ComputeSplit(a[0], xs);
            ComputeSplit(b[0], ys);
            SplitMult(xs, ys, ref zs);

            result[0] = zs[0];
            result[1] = zs[1];

            /* a[0] * b[1] */
            ComputeSplit(b[1], ys);
            SplitMult(xs, ys, ref zs);

            double tmp = result[0] + zs[0];
            result[1] -= tmp - result[0] - zs[0];
            result[0] = tmp;
            tmp = result[0] + zs[1];
            result[1] -= tmp - result[0] - zs[1];
            result[0] = tmp;

            /* a[1] * b[0] */
            ComputeSplit(a[1], xs);
            ComputeSplit(b[0], ys);
            SplitMult(xs, ys, ref zs);

            tmp = result[0] + zs[0];
            result[1] -= tmp - result[0] - zs[0];
            result[0] = tmp;
            tmp = result[0] + zs[1];
            result[1] -= tmp - result[0] - zs[1];
            result[0] = tmp;

            /* a[1] * b[0] */
            ComputeSplit(a[1], xs);
            ComputeSplit(b[1], ys);
            SplitMult(xs, ys, ref zs);

            tmp = result[0] + zs[0];
            result[1] -= tmp - result[0] - zs[0];
            result[0] = tmp;
            tmp = result[0] + zs[1];
            result[1] -= tmp - result[0] - zs[1];
            result[0] = tmp;
        }

        private static void ReducePayneHanek(double x, out double[] result)
        {
            result = new double[3];

            /* Convert input double to bits */
            long inbits = BitConverter.DoubleToInt64Bits(x);
            int exponent = (int)((inbits >> 52) & 0x7ff) - 1023;

            /* Convert to fixed point representation */
            inbits &= 0x000fffffffffffffL;
            inbits |= 0x0010000000000000L;

            /* Normalize input to be between 0.5 and 1.0 */
            exponent++;
            inbits <<= 11;

            /* Based on the exponent, get a shifted copy of recip2pi */
            long shpi0;
            long shpiA;
            long shpiB;
            int idx = exponent >> 6;
            int shift = exponent - (idx << 6);

            if (shift != 0)
            {
                shpi0 = (idx == 0) ? 0 : (RECIP_2PI[idx - 1] << shift);
                shpi0 |= (long)LogicalRightShift(RECIP_2PI[idx], (64 - shift)); // RECIP_2PI[idx] >>> (64 - shift);
                shpiA = (RECIP_2PI[idx] << shift) | (long)LogicalRightShift(RECIP_2PI[idx + 1], (64 - shift)); // (RECIP_2PI[idx + 1] >>> (64 - shift));
                shpiB = (RECIP_2PI[idx + 1] << shift) | (long)LogicalRightShift(RECIP_2PI[idx + 2], (64 - shift)); // (RECIP_2PI[idx + 2] >>> (64 - shift));
            }
            else
            {
                shpi0 = (idx == 0) ? 0 : RECIP_2PI[idx - 1];
                shpiA = RECIP_2PI[idx];
                shpiB = RECIP_2PI[idx + 1];
            }

            /* Multiply input by shpiA */
            long a = (long)LogicalRightShift(inbits, 32); // inbits >>> 32;
            long b = inbits & 0xffffffffL;

            long c = (long)LogicalRightShift(shpiA, 32); // shpiA >>> 32;
            long d = shpiA & 0xffffffffL;

            long ac = a * c;
            long bd = b * d;
            long bc = b * c;
            long ad = a * d;

            long prodB = bd + (ad << 32);
            long prodA = ac + (long)(LogicalRightShift(ad, 32)); // (ad >>> 32);

            Boolean bita = ((ulong)bd & 0x8000000000000000L) != 0;
            Boolean bitb = (ad & 0x80000000L) != 0;
            Boolean bitsum = ((ulong)prodB & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prodA++;
            }

            bita = ((ulong)prodB & 0x8000000000000000L) != 0;
            bitb = (bc & 0x80000000L) != 0;

            prodB += bc << 32;
            prodA += (long)(LogicalRightShift(bc, 32)); //bc >>> 32;

            bitsum = ((ulong)prodB & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prodA++;
            }

            /* Multiply input by shpiB */
            c = (long)(LogicalRightShift(shpiB, 32)); //shpiB >>> 32;
            d = shpiB & 0xffffffffL;
            ac = a * c;
            bc = b * c;
            ad = a * d;

            /* Collect terms */
            ac += (long)(LogicalRightShift((bc + ad), 32)); //(bc + ad) >>> 32;

            bita = ((ulong)prodB & 0x8000000000000000L) != 0;
            bitb = ((ulong)ac & 0x8000000000000000L) != 0;
            prodB += ac;
            bitsum = ((ulong)prodB & 0x8000000000000000L) != 0;
            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prodA++;
            }

            /* Multiply by shpi0 */
            c = (long)(LogicalRightShift(shpi0, 32)); //shpi0 >>> 32;
            d = shpi0 & 0xffffffffL;

            bd = b * d;
            bc = b * c;
            ad = a * d;

            prodA += bd + ((bc + ad) << 32);

            /*
             * prodA, prodB now contain the remainder as a fraction of PId  We want this as a fraction of
             * PI/2, so use the following steps:
             * 1d) multiply by 4.
             * 2d) do a fixed point muliply by PI/4.
             * 3d) Convert to floating point.
             * 4d) Multiply by 2
             */

            /* This identifies the quadrant */
            int intPart = (int)(LogicalRightShift(prodA, 62)); //(prodA >>> 62);

            /* Multiply by 4 */
            prodA <<= 2;
            prodA |= (long)(LogicalRightShift(prodB, 62)); //prodB >>> 62;
            prodB <<= 2;

            /* Multiply by PI/4 */
            a = (int)(LogicalRightShift(prodA, 32)); //prodA >>> 32;
            b = prodA & 0xffffffffL;

            c = (int)(LogicalRightShift(PI_O_4_BITS[0], 32)); //PI_O_4_BITS[0] >>> 32;
            d = PI_O_4_BITS[0] & 0xffffffffL;

            ac = a * c;
            bd = b * d;
            bc = b * c;
            ad = a * d;

            long prod2B = bd + (ad << 32);
            long prod2A = ac + (int)(LogicalRightShift(prodA, 62)); //(ad >>> 32);

            bita = ((ulong)bd & 0x8000000000000000L) != 0;
            bitb = (ad & 0x80000000L) != 0;
            bitsum = ((ulong)prod2B & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            bita = ((ulong)prod2B & 0x8000000000000000L) != 0;
            bitb = (bc & 0x80000000L) != 0;

            prod2B += bc << 32;
            prod2A += (long)LogicalRightShift(bc, 32); //bc >>> 32;

            bitsum = ((ulong)prod2B & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            /* Multiply input by pio4bits[1] */
            c = (long)LogicalRightShift(PI_O_4_BITS[1], 32); //PI_O_4_BITS[1] >>> 32;
            d = PI_O_4_BITS[1] & 0xffffffffL;
            ac = a * c;
            bc = b * c;
            ad = a * d;

            /* Collect terms */
            ac += (long)LogicalRightShift((bc + ad), 32);// (bc + ad) >>> 32;

            bita = ((ulong)prod2B & 0x8000000000000000L) != 0;
            bitb = ((ulong)ac & 0x8000000000000000L) != 0;
            prod2B += ac;
            bitsum = ((ulong)prod2B & 0x8000000000000000L) != 0;
            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            /* Multiply inputB by pio4bits[0] */
            a = (long)LogicalRightShift(prodB, 32); // prodB >>> 32;
            b = prodB & 0xffffffffL;
            c = (long)LogicalRightShift(PI_O_4_BITS[0], 32);  // PI_O_4_BITS[0] >>> 32;
            d = PI_O_4_BITS[0] & 0xffffffffL;
            ac = a * c;
            bc = b * c;
            ad = a * d;

            /* Collect terms */
            ac += (long)LogicalRightShift((bc + ad), 32);// (bc + ad) >>> 32;

            bita = ((ulong)prod2B & 0x8000000000000000L) != 0;
            bitb = ((ulong)ac & 0x8000000000000000L) != 0;
            prod2B += ac;
            bitsum = ((ulong)prod2B & 0x8000000000000000L) != 0;
            /* Carry */
            if ((bita && bitb) ||
                    ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            /* Convert to double */
            double tmpA = ((long)LogicalRightShift(prod2A, 12)) / TWO_POWER_52;  // prod2A >>> 12, High order 52 bits
            double tmpB = (((prod2A & 0xfffL) << 40) + ((long)LogicalRightShift(prod2B, 24))) / TWO_POWER_52 / TWO_POWER_52; // prod2B >>> 24, Low bits

            double sumA = tmpA + tmpB;
            double sumB = -(sumA - tmpA - tmpB);

            /* Multiply by PI/2 and return */
            result[0] = intPart;
            result[1] = sumA * 2.0;
            result[2] = sumB * 2.0;
        }

        private static void Resplit(ref double[] a)
        {
            double c = a[0] + a[1];
            double d = -(c - a[0] - a[1]);

            if (c < 8e298 && c > -8e298)
            { // MAGIC NUMBER
                double z = c * HEX_40000000;
                a[0] = (c + z) - z;
                a[1] = c - a[0] + d;
            }
            else
            {
                double z = c * 9.31322574615478515625E-10;
                a[0] = (c + z - c) * HEX_40000000;
                a[1] = c - a[0] + d;
            }
        }

        private static double Slowexp(double x, ref double[] result)
        {
            double[] xs = new double[2];
            double[] ys = new double[2];
            double[] facts = new double[2];
            double[] aa = new double[2];
            ComputeSplit(x, xs);
            ys[0] = ys[1] = 0.0;

            for (int i = FACT.Length - 1; i >= 0; i--)
            {
                SplitMult(xs, ys, ref aa);
                ys[0] = aa[0];
                ys[1] = aa[1];

                ComputeSplit(FACT[i], aa);
                SplitReciprocal(aa, facts);

                SplitAdd(ys, facts, ref aa);
                ys[0] = aa[0];
                ys[1] = aa[1];
            }

            if (result != null)
            {
                result[0] = ys[0];
                result[1] = ys[1];
            }

            return ys[0] + ys[1];
        }

        private static double[] SlowLog(double xi)
        {
            double[] x = new double[2];
            double[] x2 = new double[2];
            double[] y = new double[2];
            double[] a = new double[2];

            ComputeSplit(xi, x);

            /* Set X = (x-1)/(x+1) */
            x[0] += 1.0;
            Resplit(ref x);
            SplitReciprocal(x, a);
            x[0] -= 2.0;
            Resplit(ref x);
            SplitMult(x, a, ref y);
            x[0] = y[0];
            x[1] = y[1];

            /* Square X -> X2*/
            SplitMult(x, x, ref x2);

            //x[0] -= 1.0;
            //resplit(x);

            y[0] = LN_SPLIT_COEF[LN_SPLIT_COEF.GetLength(1) - 1][0];
            y[1] = LN_SPLIT_COEF[LN_SPLIT_COEF.GetLength(1) - 1][1];

            for (int i = LN_SPLIT_COEF.GetLength(1) - 2; i >= 0; i--)
            {
                SplitMult(y, x2, ref a);
                y[0] = a[0];
                y[1] = a[1];
                SplitAdd(y, LN_SPLIT_COEF[i], ref a);
                y[0] = a[0];
                y[1] = a[1];
            }

            SplitMult(y, x, ref a);
            y[0] = a[0];
            y[1] = a[1];

            return y;
        }

        private static void ComputeSplit(double d, double[] split)
        {
            if (d < 8e298 && d > -8e298)
            {
                double a = d * HEX_40000000;
                split[0] = (d + a) - a;
                split[1] = d - split[0];
            }
            else
            {
                double a = d * 9.31322574615478515625E-10;
                split[0] = (d + a - d) * HEX_40000000;
                split[1] = d - split[0];
            }
        }

        private static void SplitAdd(double[] a, double[] b, ref double[] ans)
        {
            ans[0] = a[0] + b[0];
            ans[1] = a[1] + b[1];

            Resplit(ref ans);
        }

        private static void SplitMult(double[] a, double[] b, ref double[] ans)
        {
            ans[0] = a[0] * b[0];
            ans[1] = a[0] * b[1] + a[1] * b[0] + a[1] * b[1];

            /* Resplit */
            Resplit(ref ans);
        }

        private static void SplitReciprocal(double[] value, double[] result)
        {
            double b = 1.0 / 4194304.0;
            double a = 1.0 - b;

            if (value[0] == 0.0)
            {
                value[0] = value[1];
                value[1] = 0.0;
            }

            result[0] = a / value[0];
            result[1] = (b * value[0] - a * value[1]) / (value[0] * value[0] + value[0] * value[1]);

            if (result[1] != result[1])
            { // can happen if result[1] is NAN
                result[1] = 0.0;
            }

            /* Resplit */
            Resplit(ref result);

            for (int i = 0; i < 2; i++)
            {
                /* this may be overkill, probably once is enough */
                double err = 1.0 - result[0] * value[0] - result[0] * value[1] -
                result[1] * value[0] - result[1] * value[1];
                /*err = 1.0 - err; */
                err *= result[0] + result[1];
                /*printf("err = %16e\n", err); */
                result[1] += err;
            }
        }

        /// <summary>
        /// Returns the floor modulus.
        /// <p>
        /// This returns {@code 0} for {@code floorMod(0, 4)}.<br />
        /// This returns {@code 3} for {@code floorMod(-1, 4)}.<br />
        /// This returns {@code 2} for {@code floorMod(-2, 4)}.<br />
        /// This returns {@code 1} for {@code floorMod(-3, 4)}.<br />
        /// This returns {@code 0} for {@code floorMod(-4, 4)}.<br />
        /// This returns {@code 3} for {@code floorMod(-5, 4)}.<br />
        /// </summary>
        /// <param name="a">the dividend</param>
        /// <param name="b">the divisor</param>
        /// <returns>the floor modulus (positive)</returns>
        public static int FloorMod(long a, int b)
        {
            return (int)(((a % b) + b) % b);
        }
        #endregion

        #region Inner use classes

        /// <summary>Class operator on double numbers split into one 26 bits number and one 27 bits numberd */

        public class CodyWaite
        {
            /// <summary>k */
            private int _k;
            /// <summary>remA */
            private double _remA;
            /// <summary>remB */
            private double _remB;

            /// <summary>
            /// <summary>
            /// <param name="xa">Argument.</param>

            public CodyWaite(double xa)
            {
                // Estimate k.
                //k = (int)(xa / 1.5707963267948966);
                int k = (int)(xa * 0.6366197723675814);

                // Compute remainder.
                double remA;
                double remB;
                while (true)
                {
                    double a = -k * 1.570796251296997;
                    remA = xa + a;
                    remB = -(remA - xa - a);

                    a = -k * 7.549789948768648E-8;
                    double b = remA;
                    remA = a + b;
                    remB += -(remA - b - a);

                    a = -k * 6.123233995736766E-17;
                    b = remA;
                    remA = a + b;
                    remB += -(remA - b - a);

                    if (remA > 0)
                    {
                        break;
                    }

                    // Remainder is negative, so decrement k and try again.
                    // This should only happen if the input is very close
                    // to an even multiple of pi/2.
                    --k;
                }

                this._k = k;
                this._remA = remA;
                this._remB = remB;
            }

            /// <summary>
            /// <summary>
            /// <returns>k</returns>

            public int K
            {
                get { return K; }
            }

            /// <summary>
            /// <summary>
            /// <returns>remA</returns>

            public double RemA
            {
                get { return _remA; }
            }

            /// <summary>
            /// <summary>
            /// <returns>remB</returns>

            public double RemB
            {
                get { return _remB; }
            }
        }

        public class ExpFracTable
        {
            /// <summary>Exponential over the range of 0 - 1 in increments of 2^-10
            /// exp(x/1024) =  expFracTableA[x] + expFracTableB[x].
            /// 1024 = 2^10
            /// <summary>
            public double[] EXP_FRAC_TABLE_A = new double[Math2.EXP_FRAC_TABLE_LEN];
            /// <summary>Exponential over the range of 0 - 1 in increments of 2^-10
            /// exp(x/1024) =  expFracTableA[x] + expFracTableB[x].
            /// <summary>
            public double[] EXP_FRAC_TABLE_B = new double[Math2.EXP_FRAC_TABLE_LEN];

            private static ExpFracTable _instance;

            private ExpFracTable()
            {
                double[] tmp = new double[2];

                // Populate expFracTable
                double factor = 1d / (EXP_FRAC_TABLE_LEN - 1);
                for (int i = 0; i < EXP_FRAC_TABLE_A.Length; i++)
                {
                    Slowexp(i * factor, ref tmp);
                    EXP_FRAC_TABLE_A[i] = tmp[0];
                    EXP_FRAC_TABLE_B[i] = tmp[1];
                }
            }

            public static ExpFracTable GetInstance()
            {
                if (_instance == null)
                    _instance = new ExpFracTable();

                return _instance;
            }
        }

        //  This class will be a Singlton Class
        public class ExpIntTable
        {
            /// <summary>Exponential evaluated at int values,
            /// exp(x) =  expIntTableA[x + EXP_INT_TABLE_MAX_INDEX] + expIntTableB[x+EXP_INT_TABLE_MAX_INDEX].
            /// <summary>
            public double[] EXP_INT_TABLE_A = new double[Math2.EXP_INT_TABLE_LEN];
            /// <summary>Exponential evaluated at int values,
            /// exp(x) =  expIntTableA[x + EXP_INT_TABLE_MAX_INDEX] + expIntTableB[x+EXP_INT_TABLE_MAX_INDEX]
            /// <summary>
            public double[] EXP_INT_TABLE_B = new double[Math2.EXP_INT_TABLE_LEN];

            private static ExpIntTable _instance;

            private ExpIntTable()
            {
                double[] tmp = new double[2];
                double[] recip = new double[2];

                // Populate expIntTable
                for (int i = 0; i < Math2.EXP_INT_TABLE_MAX_INDEX; i++)
                {
                    Expint(i, ref tmp);
                    EXP_INT_TABLE_A[i + Math2.EXP_INT_TABLE_MAX_INDEX] = tmp[0];
                    EXP_INT_TABLE_B[i + Math2.EXP_INT_TABLE_MAX_INDEX] = tmp[1];

                    if (i != 0)
                    {
                        // Negative int powers
                        SplitReciprocal(tmp, recip);
                        EXP_INT_TABLE_A[Math2.EXP_INT_TABLE_MAX_INDEX - i] = recip[0];
                        EXP_INT_TABLE_B[Math2.EXP_INT_TABLE_MAX_INDEX - i] = recip[1];
                    }
                }
            }

            public static ExpIntTable GetInstance()
            {
                if (_instance == null)
                    _instance = new ExpIntTable();

                return _instance;
            }
        }

        public class lnMant
        {
            /// <summary>Extended precision logarithm table over the range 1 - 2 in increments of 2^-10d */
            public double[][] LN_MANT = new double[Math2.LN_MANT_LEN][];

            private static lnMant _instance;

            private lnMant()
            {                // Populate lnMant table
                for (int i = 0; i < LN_MANT.Length; i++)
                {
                    double d = BitConverter.Int64BitsToDouble((((long)i) << 42) | 0x3ff0000000000000L);
                    LN_MANT[i] = SlowLog(d);
                }
            }

            public static lnMant GetInstance()
            {
                if (_instance == null)
                    _instance = new lnMant();

                return _instance;
            }
        }

        public class Split
        {
            #region Local Variables

            /// <summary>Split version of NaNd */
            public static Split NAN = new Split(Double.NaN, 0);

            /// <summary>Split version of positive infinityd */
            public static Split NEGATIVE_INFINITY = new Split(Double.NegativeInfinity, 0);
            public static Split POSITIVE_INFINITY = new Split(Double.PositiveInfinity, 0);

            /// <summary>Split version of negative infinityd */
            /// <summary>Full numberd */
            private double _full;

            /// <summary>High order bitsd */
            private double _high;

            /// <summary>Low order bitsd */
            private double _low;

            #endregion Local Variables

            public Split(double x)
            {
                _full = x;
                _high = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) & ((-1L) << 27));
                _low = x - _high;
            }

            /* negative zero */
            public Split(double high, double low) : this(high == 0.0 ? (low == 0.0 && BitConverter.DoubleToInt64Bits(high) == long.MinValue ? -0.0 : low) : high + low, high, low)
            {
            }

            public Split(double full, double high, double low)
            {
                this._full = full;
                this._high = high;
                this._low = low;
            }

            public double Full
            {
                get { return _full; }
                set { _full = value; }
            }

            public double High
            {
                get { return _high; }
                set { _high = value; }
            }

            public double Low
            {
                get { return _low; }
                set { _low = value; }
            }

            /// <summary>Simple constructor.
            /// <summary>
            /// <param name="x">number to split</param>
            /// <summary>Simple constructor.
            /// <summary>
            /// <param name="high">high order bits</param>
            /// <param name="low">low order bits</param>
            /// <summary>Simple constructor.
            /// <summary>
            /// <param name="full">full number</param>
            /// <param name="high">high order bits</param>
            /// <param name="low">low order bits</param>
            /// <summary>Multiply the instance by another one.
            /// <summary>
            /// <param name="b">other instance to multiply by</param>
            /// <returns>product</returns>

            public Split Multiply(Split b)
            {
                // beware the following expressions must NOT be simplified, they rely on floating point arithmetic properties
                Split mulBasic = new Split(_full * b._full);
                double mulError = _low * b._low - (((mulBasic._full - _high * b._high) - _low * b._high) - _high * b._low);
                return new Split(mulBasic._high, mulBasic._low + mulError);
            }

            /// <summary>Compute the reciprocal of the instance.
            /// <summary>
            /// <returns>reciprocal of the instance</returns>

            public Split Pow(long e)
            {
                // prepare result
                Split result = new Split(1);

                // d^(2p)
                Split d2p = new Split(_full, _high, _low);

                for (long p = e; p != 0; p = (long)LogicalRightShift(p, 1))
                {
                    if ((p & 0x1) != 0)
                    {
                        // accurate multiplication result = result * d^(2p) using Veltkamp TwoProduct algorithm
                        result = result.Multiply(d2p);
                    }

                    // accurate squaring d^(2(p+1)) = d^(2p) * d^(2p) using Veltkamp TwoProduct algorithm
                    d2p = d2p.Multiply(d2p);
                }

                if (Double.IsNaN(result._full))
                {
                    if (Double.IsNaN(_full))
                    {
                        return Split.NAN;
                    }
                    else
                    {
                        // some intermediate numbers exceeded capacity,
                        // and the low order bits became NaN (because infinity - infinity = NaN)
                        if (System.Math.Abs(_full) < 1)
                        {
                            return new Split(Math2.CopySign(0.0, _full), 0.0);
                        }
                        else if (_full < 0 && (e & 0x1) == 1)
                        {
                            return Split.NEGATIVE_INFINITY;
                        }
                        else
                        {
                            return Split.POSITIVE_INFINITY;
                        }
                    }
                }
                else
                {
                    return result;
                }
            }

            public Split Reciprocal()
            {
                double approximateInv = 1.0 / _full;
                Split splitInv = new Split(approximateInv);

                // if 1.0/d were computed perfectly, remultiplying it by d should give 1.0
                // we want to estimate the error so we can fix the low order bits of approximateInvLow
                // beware the following expressions must NOT be simplified, they rely on floating point arithmetic properties
                Split product = Multiply(splitInv);
                double error = (product._high - 1) + product._low;

                // better accuracy estimate of reciprocal
                return Double.IsNaN(error) ? splitInv : new Split(splitInv._high, splitInv._low - error / _full);
            }

            /// <summary>Computes this^e.
            /// <summary>
            /// <param name="e">exponent (beware, here it MUST be > 0; the only exclusion is long.MinValue)</param>
            /// <returns>d^e, split in high and low bits</returns>
            /// @since 3.6
        }
        #endregion Inner use classes
    }
}