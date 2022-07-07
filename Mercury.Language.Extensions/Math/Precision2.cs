// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
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
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language.Math
{
    /// <summary>
    /// <p>
    /// Largest double-precision floating-point number such that
    /// {@code 1 + EPSILON} is numerically equal to 1d This value is an upper
    /// bound on the relative error due to rounding real numbers to double
    /// precision floating-point numbers.
    /// </p>
    /// <p>
    /// In IEEE 754 arithmetic, this is 2<sup>-53</sup>.
    /// </p>
    ///</summary>
    ///<a href="http://en.wikipedia.org/wiki/Machine_epsilon">Machine epsilon</a>
    public static class Precision2
    {
        /** Exponent offset in IEEE754 representationd */
        private static long EXPONENT_OFFSET = 1023L;

        /** Offset to order signed double numbers lexicographicallyd */
        private static long SGN_MASK = unchecked((long)0x8000000000000000L); //0x8000000000000000L;
        /** Offset to order signed double numbers lexicographicallyd */
        private static int SGN_MASK_FLOAT = unchecked((int)0x80000000);    //0x80000000;
        /** Positive zerod */
        private static double POSITIVE_ZERO = 0d;
        /** Positive zero bitsd */
        private static long POSITIVE_ZERO_DOUBLE_BITS = BitConverter.DoubleToInt64Bits(+0.0);
        /** Negative zero bitsd */
        private static long NEGATIVE_ZERO_DOUBLE_BITS = BitConverter.DoubleToInt64Bits(-0.0);
        /** Positive zero bitsd */
        private static int POSITIVE_ZERO_FLOAT_BITS = BitConverter2.FloatToRawIntBits(+0.0f);
        /** Negative zero bitsd */
        private static int NEGATIVE_ZERO_FLOAT_BITS = BitConverter2.FloatToRawIntBits(-0.0f);

        /// <summary>
        /// This was previously expressed as = 0x1.0p-53;
        /// However, OpenJDK (Sparc Solaris) cannot handle such small
        /// constants: MATH-721
        /// </summary>
        public static double EPSILON = BitConverter.Int64BitsToDouble((EXPONENT_OFFSET - 53L) << 52);

        /// <summary>
        /// Safe minimum, such that {@code 1 / SAFE_MIN} does not overflow.
        /// In IEEE 754 arithmetic, this is also the smallest normalized
        /// number 2<sup>-1022</sup>.
        /// 
        /// This was previously expressed as = 0x1.0p-1022;
        /// However, OpenJDK (Sparc Solaris) cannot handle such small
        /// constants: MATH-721
        /// </summary>
        public static double SAFE_MIN = BitConverter.Int64BitsToDouble((EXPONENT_OFFSET - 1022L) << 52);

        /// <summary>
        /// Value representing 10 * 2^(-53) = 1.11022302462516E-15
        /// </summary>
        public static readonly double DefaultDoubleAccuracy = MathNet.Numerics.Precision.DoublePrecision * 10;


        /// <summary>
        /// Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <param name="value">The value used to determine the minimum distance.</param>
        /// <returns>
        /// Relative Epsilon (positive double or NaN).
        /// </returns>
        /// <remarks>Evaluates the <b>negative</b> epsilon. The more common positive epsilon is equal to two times this negative epsilon.</remarks>
        /// <seealso cref="PositiveEpsilonOf(decimal)"/>
        public static decimal EpsilonOf(this decimal value)
        {
            //if (double.IsInfinity(value) || double.IsNaN(value))
            //{
            //    return double.NaN;
            //}

            long signed64 = BitConverter2.DecimalToInt64Bits(value);
            if (signed64 == 0)
            {
                signed64++;
                return BitConverter2.Int64BitsToDecimal(signed64) - value;
            }
            if (signed64-- < 0)
            {
                return BitConverter2.Int64BitsToDecimal(signed64) - value;
            }
            return value - BitConverter2.Int64BitsToDecimal(signed64);
        }


        /// <summary>
        /// Check whether two floating point values match with a given precision.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <param name="delta">Precision</param>
        /// <returns><code>true</code> if the difference of <i>a</i> and <b>b</b> is smaller or equal than <i>delta</i>, otherwise <code>false</code></returns>
        public static Boolean AlmostEqual(double a, double b, double delta)
        {
            return System.Math.Abs(a - b) <= delta;
        }

        /// <summary>
        /// Check whether two floating point values match with default precision.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns><code>true</code> if the difference of <i>a</i> and <b>b</b> is smaller or equal than <see cref="EPSILON"/>, otherwise <code>false</code></returns>
        public static Boolean AlmostEqual(double a, double b)
        {
            return AlmostEqual(a, b, EPSILON);
        }

        /// <summary>
        /// Compares two numbers given some amount of allowed error.
        /// The returned value is
        /// <ul>
        ///  <li>
        ///   0 if <see cref="Equals(double, double, double)">Equals(x, y, eps)</see>,
        ///  </li>
        ///  <li>
        ///   negative if !<see cref="Equals(double, double, double)">Equals(x, y, eps)</see> and x < y,
        ///  </li>
        ///  <li>
        ///   positive if !<see cref="Equals(double, double, double)">Equals(x, y, eps)</see> and x > y or
        ///   either argument is NaN.
        ///  </li>
        /// </ul>
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <param name="eps">Allowed error when checking for equality.</param>
        /// <returns>0 if the value are considered equal, -1 if the first is smaller than the second, 1 is the first is larger than the second.</returns>
        public static int CompareTo(double x, double y, double eps)
        {
            if (Equals(x, y, eps))
            {
                return 0;
            }
            else if (x < y)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// Compares two numbers given some amount of allowed error.
        /// Two float numbers are considered equal if there are {@code (maxUlps - 1)}
        /// (or fewer) floating point numbers between them, i.ed two adjacent floating
        /// point numbers are considered equal.
        /// Adapted from
        /// <a href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">
        /// Bruce Dawson</a>d Returns {@code false} if either of the arguments is NaN.
        /// The returned value is
        /// <ul>
        ///  <li>
        ///   zero if {@link #Equals(double,double,int) Equals(x, y, maxUlps)},
        ///  </li>
        ///  <li>
        ///   negative if !{@link #Equals(double,double,int) Equals(x, y, maxUlps)} and {@code x < y},
        ///  </li>
        ///  <li>
        ///   positive if !{@link #Equals(double,double,int) Equals(x, y, maxUlps)} and {@code x > y}
        ///       or either argument is {@code NaN}.
        ///  </li>
        /// </ul>
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <param name="maxUlps">(maxUlps - 1) is the number of floating point values between x and y.</param>
        /// <returns>0 if the value are considered equal, -1 if the first is smaller than the second, 1 is the first is larger than the second.</returns>
        public static int CompareTo(double x, double y, int maxUlps)
        {
            if (Equals(x, y, maxUlps))
            {
                return 0;
            }
            else if (x < y)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// Returns true iff they are equal as defined by
        /// <see cref="Equals(float, float, int)">Equals(x, y, 1)</see>.
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <returns>{@code true} if the values are equal.</returns>
        public static Boolean Equals(float x, float y)
        {
            return Equals(x, y, 1);
        }


        /// <summary>
        /// Returns true if both arguments are NaN or they are
        /// equal as defined by <see cref="Equals(float, float, int)">Equals(x, y, 1)</see>.
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <returns>{@code true} if the values are equal or both are NaN.</returns>
        public static Boolean EqualsIncludingNaN(float x, float y)
        {
            return (float.IsNaN(x) || float.IsNaN(y)) ? !(float.IsNaN(x) ^ float.IsNaN(y)) : Equals(x, y, 1);
        }

        /// <summary>
        /// Returns true if the arguments are equal or within the range of allowed
        /// error (inclusive)d  Returns {@code false} if either of the arguments
        /// is NaN.
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="eps">the amount of absolute error to allow.</param>
        /// <returns>{@code true} if the values are equal or within range of each other.</returns>
        public static Boolean Equals(float x, float y, float eps)
        {
            return Equals(x, y, 1) || System.Math.Abs(y - x) <= eps;
        }

        /// <summary>
        /// Returns true if the arguments are both NaN, are equal, or are within the range
        /// of allowed error (inclusive).
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="eps">the amount of absolute error to allow.</param>
        /// <returns>{@code true} if the values are equal or within range of each other, or both are NaN.</returns>
        public static Boolean EqualsIncludingNaN(float x, float y, float eps)
        {
            return EqualsIncludingNaN(x, y) || (System.Math.Abs(y - x) <= eps);
        }

        /// <summary>
        /// Returns true if the arguments are equal or within the range of allowed
        /// error (inclusive).
        /// Two float numbers are considered equal if there are {@code (maxUlps - 1)}
        /// (or fewer) floating point numbers between them, i.ed two adjacent floating
        /// point numbers are considered equal.
        /// Adapted from <a
        /// href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">
        /// Bruce Dawson</a>d  Returns {@code false} if either of the arguments is NaN.
        /// 
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="maxUlps">{@code (maxUlps - 1)} is the number of floating point values between {@code x} and {@code y}.</param>
        /// <returns>{@code true} if there are fewer than {@code maxUlps} floating point values between {@code x} and {@code y}.</returns>
        public static Boolean Equals(float x, float y, int maxUlps)
        {

            int xInt = BitConverter2.FloatToRawIntBits(x);
            int yInt = BitConverter2.FloatToRawIntBits(y);

            Boolean isEqual;
            if (((xInt ^ yInt) & SGN_MASK_FLOAT) == 0)
            {
                // number have same sign, there is no risk of overflow
                isEqual = System.Math.Abs(xInt - yInt) <= maxUlps;
            }
            else
            {
                // number have opposite signs, take care of overflow
                int deltaPlus;
                int deltaMinus;
                if (xInt < yInt)
                {
                    deltaPlus = yInt - POSITIVE_ZERO_FLOAT_BITS;
                    deltaMinus = xInt - NEGATIVE_ZERO_FLOAT_BITS;
                }
                else
                {
                    deltaPlus = xInt - POSITIVE_ZERO_FLOAT_BITS;
                    deltaMinus = yInt - NEGATIVE_ZERO_FLOAT_BITS;
                }

                if (deltaPlus > maxUlps)
                {
                    isEqual = false;
                }
                else
                {
                    isEqual = deltaMinus <= (maxUlps - deltaPlus);
                }
            }
            return isEqual && !float.IsNaN(x) && !float.IsNaN(y);
        }

        /// <summary>
        /// Returns true if the arguments are both NaN or if they are equal as defined
        /// by <see cref="Equals(float, float, int)">Equals(x, y, maxUlps)</see>.
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="maxUlps">{@code (maxUlps - 1)} is the number of floating point values between {@code x} and {@code y}.</param>
        /// <returns>{@code true} if both arguments are NaN or if there are less than {@code maxUlps} floating point values between {@code x} and {@code y}.</returns>
        public static Boolean EqualsIncludingNaN(float x, float y, int maxUlps)
        {
            return (float.IsNaN(x) || float.IsNaN(y)) ? !(float.IsNaN(x) ^ float.IsNaN(y)) : Equals(x, y, maxUlps);
        }

        /// <summary>
        /// Returns true iff they are equal as defined by <see cref="Equals(double, double, int)">Equals(x, y, 1)</see>.
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <returns>{@code true} if the values are equal.</returns>
        public static Boolean Equals(double x, double y)
        {
            return Equals(x, y, 1);
        }

        /// <summary>
        /// Returns true if the arguments are both NaN or they are
        /// equal as defined by <see cref="Equals(double,double)">Equals(x, y, 1)</see>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Boolean EqualsIncludingNaN(double x, double y)
        {
            return (double.IsNaN(x) || double.IsNaN(y)) ? !(double.IsNaN(x) ^ double.IsNaN(y)) : Equals(x, y, 1);
        }

        /// <summary>
        /// Returns {@code true} if there is no double value strictly between the
        /// arguments or the difference between them is within the range of allowed
        /// error (inclusive)d Returns {@code false} if either of the arguments
        /// is NaN.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <param name="eps">Amount of allowed absolute error.</param>
        /// <returns>{@code true} if the values are two adjacent floating point numbers or they are within range of each other.</returns>
        public static Boolean Equals(double x, double y, double eps)
        {
            return Equals(x, y, 1) || System.Math.Abs(y - x) <= eps;
        }

        /// <summary>
        /// Returns {@code true} if there is no double value strictly between the
        /// arguments or the relative difference between them is less than or equal
        /// to the given toleranced Returns {@code false} if either of the arguments
        /// is NaN.
        /// 
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <param name="eps">Amount of allowed relative error.</param>
        /// <returns>{@code true} if the values are two adjacent floating point numbers or they are within range of each other.</returns>
        public static Boolean EqualsWithRelativeTolerance(double x, double y, double eps)
        {
            if (Equals(x, y, 1))
            {
                return true;
            }

            double absoluteMax = System.Math.Max(System.Math.Abs(x), System.Math.Abs(y));
            double relativeDifference = System.Math.Abs((x - y) / absoluteMax);

            return relativeDifference <= eps;
        }

        /// <summary>
        /// Returns true if the arguments are both NaN, are equal or are within the range
        /// of allowed error (inclusive).
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="eps">the amount of absolute error to allow.</param>
        /// <returns>{@code true} if the values are equal or within range of each other, or both are NaN.</returns>
        public static Boolean EqualsIncludingNaN(double x, double y, double eps)
        {
            return EqualsIncludingNaN(x, y) || (System.Math.Abs(y - x) <= eps);
        }

        /// <summary>
        /// Returns true if the arguments are equal or within the range of allowed
        /// error (inclusive).
        /// <p>
        /// Two float numbers are considered equal if there are {@code (maxUlps - 1)}
        /// (or fewer) floating point numbers between them, i.ed two adjacent
        /// floating point numbers are considered equal.
        /// </p>
        /// <p>
        /// Adapted from <a
        /// href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">
        /// Bruce Dawson</a>d Returns {@code false} if either of the arguments is NaN.
        /// </p>
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="maxUlps">{@code (maxUlps - 1)} is the number of floating point values between {@code x} and {@code y}.</param>
        /// <returns>{@code true} if there are fewer than {@code maxUlps} floating point values between {@code x} and {@code y}.</returns>
        public static Boolean Equals(double x, double y, int maxUlps)
        {
            long xInt = BitConverter.DoubleToInt64Bits(x);
            long yInt = BitConverter.DoubleToInt64Bits(y);

            Boolean isEqual;
            if (((xInt ^ yInt) & SGN_MASK) == 0L)
            {
                // number have same sign, there is no risk of overflow
                isEqual = System.Math.Abs(xInt - yInt) <= maxUlps;
            }
            else
            {
                // number have opposite signs, take care of overflow
                long deltaPlus;
                long deltaMinus;
                if (xInt < yInt)
                {
                    deltaPlus = yInt - POSITIVE_ZERO_DOUBLE_BITS;
                    deltaMinus = xInt - NEGATIVE_ZERO_DOUBLE_BITS;
                }
                else
                {
                    deltaPlus = xInt - POSITIVE_ZERO_DOUBLE_BITS;
                    deltaMinus = yInt - NEGATIVE_ZERO_DOUBLE_BITS;
                }

                if (deltaPlus > maxUlps)
                {
                    isEqual = false;
                }
                else
                {
                    isEqual = deltaMinus <= (maxUlps - deltaPlus);
                }
            }
            return isEqual && !Double.IsNaN(x) && !Double.IsNaN(y);
        }

        /// <summary>
        /// Returns true if both arguments are NaN or if they are equal as defined
        /// by <see cref="Equals(double,double,int)">Equals(x, y, maxUlps)</see>.
        /// </summary>
        /// <param name="x">first value</param>
        /// <param name="y">second value</param>
        /// <param name="maxUlps">{@code (maxUlps - 1)} is the number of floating point values between {@code x} and {@code y}.</param>
        /// <returns>{@code true} if both arguments are NaN or if there are less than {@code maxUlps} floating point values between {@code x} and {@code y}.</returns>
        public static Boolean EqualsIncludingNaN(double x, double y, int maxUlps)
        {
            return (double.IsNaN(x) || double.IsNaN(y)) ? !(double.IsNaN(x) ^ double.IsNaN(y)) : Equals(x, y, maxUlps);
        }

        /// <summary>
        /// Rounds the given value to the specified number of decimal places.
        /// The value is rounded using the <see cref="BigDecimal.RoundMode.HalfUp"/> method.
        /// </summary>
        /// <param name="x">Value to round.</param>
        /// <param name="scale">Number of digits to the right of the decimal point.</param>
        /// <returns>the rounded value.</returns>
        public static double Round(double x, int scale)
        {
            return Round(x, scale, BigDecimal.RoundMode.HalfUp);
        }

        /// <summary>
        /// Rounds the given value to the specified number of decimal places.
        /// The value is rounded using the given method which is any method defined
        /// in <see cref="BigDecimal"/>.
        /// If {@code x} is infinite or {@code NaN}, then the value of {@code x} is
        /// returned unchanged, regardless of the other parameters.
        /// </summary>
        /// <param name="x">Value to round.</param>
        /// <param name="scale">Number of digits to the right of the decimal point.</param>
        /// <param name="roundingMethod">Rounding method as defined in <see cref="BigDecimal"/>.</param>
        /// <returns>the rounded value.</returns>
        /// <exception cref="ArithmeticException">if {@code roundingMethod == <see cref="BigDecimal.RoundMode.Unnecessary"/>} and the specified scaling operation would require rounding.</exception>
        /// <exception cref="ArgumentException">if {@code roundingMethod} does not represent a valid rounding mode.</exception>
        public static double Round(double x, int scale, BigDecimal.RoundMode roundingMethod)
        {
            try
            {

                double rounded = (new BigDecimal(new BigInteger(x), scale, roundingMethod)).ToDoubleValue();
                // MATH-1089: negative values rounded to zero should result in negative zero
                return rounded == POSITIVE_ZERO ? POSITIVE_ZERO * x : rounded;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);

                if (Double.IsInfinity(x))
                {
                    return x;
                }
                else
                {
                    return Double.NaN;
                }
            }
        }

        /// <summary>
        /// Computes a number {@code delta} close to {@code originalDelta} with
        /// the property that <pre><code>
        ///   x + delta - x
        /// </code></pre>
        /// is exactly machine-representable.
        /// This is useful when computing numerical derivatives, in order to reduce
        /// roundoff errors.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="originalDelta">Offset value.</param>
        /// <returns>a number {@code delta} so that {@code x + delta} and {@code x} differ by a representable floating number.</returns>
        public static double representableDelta(double x, double originalDelta)
        {
            return x + originalDelta - x;
        }
    }
}
