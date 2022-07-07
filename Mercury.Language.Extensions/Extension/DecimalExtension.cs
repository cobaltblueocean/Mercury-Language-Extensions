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
using System.Numerics;
using System.Runtime;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// Extension Method class for Decimal
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// The number of binary digits used to represent the binary number for a decimal precision floating
        /// point value. i.e. there are this many digits used to represent the
        /// actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        const int DecimalWidth = 65;

        /// <summary>
        /// Standard epsilon, the maximum relative precision of IEEE 754 decimal-precision floating numbers (64 bit).
        /// According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// </summary>
        public static readonly decimal DecimalPrecision = Math2.Pow(2M, -DecimalWidth);

        /// <summary>
        /// Value representing 10 * 2^(-53) = 1.11022302462516E-15
        /// </summary>
        static readonly decimal DefaultDecimalAccuracy = DecimalPrecision * 10;

        /// <summary>
        /// Checks whether two real numbers are almost equal.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>true if the two values differ by no more than 10 * 2^(-52); false otherwise.</returns>
        public static bool AlmostEqual(this decimal a, decimal b)
        {
            return AlmostEqualNorm(a, b, a - b, DefaultDecimalAccuracy);
        }

        /// <summary>
        /// Compares two decimals and determines if they are equal within
        /// the specified maximum error.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="maximumAbsoluteError">The accuracy required for being almost equal.</param>
        public static bool AlmostEqual(this decimal a, decimal b, decimal maximumAbsoluteError)
        {
            return AlmostEqualNorm(a, b, a - b, maximumAbsoluteError);
        }

        /// <summary>
        /// Compares two decimals and determines if they are equal to within the specified number of decimal places or not, using the
        /// number of decimal places as an absolute measure.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The values are equal if the difference between the two numbers is smaller than 0.5e-decimalPlaces. We divide by
        /// two so that we have half the range on each side of the numbers, e.g. if <paramref name="decimalPlaces"/> == 2, then 0.01 will equal between
        /// 0.005 and 0.015, but not 0.02 and not 0.00
        /// </para>
        /// </remarks>
        /// <param name="a">The norm of the first value (can be negative).</param>
        /// <param name="b">The norm of the second value (can be negative).</param>
        /// <param name="diff">The norm of the difference of the two values (can be negative).</param>
        /// <param name="decimalPlaces">The number of decimal places.</param>
        public static bool AlmostEqualNorm(this decimal a, decimal b, decimal diff, decimal maximumAbsoluteError)
        {
            //// If A or B are a NAN, return false. NANs are equal to nothing,
            //// not even themselves.
            //if (decimal.IsNaN(a) || decimal.IsNaN(b))
            //{
            //    return false;
            //}

            //// If A or B are infinity (positive or negative) then
            //// only return true if they are exactly equal to each other -
            //// that is, if they are both infinities of the same sign.
            //if (decimal.IsInfinity(a) || decimal.IsInfinity(b))
            //{
            //    return a == b;
            //}

            return Math2.Abs(diff) < maximumAbsoluteError;
        }

        /// <summary>
        /// Compares two doubles and determines if they are equal to within the specified number of decimal places or not, using the
        /// number of decimal places as an absolute measure.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The values are equal if the difference between the two numbers is smaller than 0.5e-decimalPlaces. We divide by
        /// two so that we have half the range on each side of the numbers, e.g. if <paramref name="decimalPlaces"/> == 2, then 0.01 will equal between
        /// 0.005 and 0.015, but not 0.02 and not 0.00
        /// </para>
        /// </remarks>
        /// <param name="a">The norm of the first value (can be negative).</param>
        /// <param name="b">The norm of the second value (can be negative).</param>
        /// <param name="diff">The norm of the difference of the two values (can be negative).</param>
        /// <param name="decimalPlaces">The number of decimal places.</param>
        public static bool AlmostEqualNorm(this decimal a, decimal b, decimal diff, int decimalPlaces)
        {
            //// If A or B are a NAN, return false. NANs are equal to nothing,
            //// not even themselves.
            //if (double.IsNaN(a) || double.IsNaN(b))
            //{
            //    return false;
            //}

            //// If A or B are infinity (positive or negative) then
            //// only return true if they are exactly equal to each other -
            //// that is, if they are both infinities of the same sign.
            //if (double.IsInfinity(a) || double.IsInfinity(b))
            //{
            //    return a == b;
            //}

            // The values are equal if the difference between the two numbers is smaller than
            // 10^(-numberOfDecimalPlaces). We divide by two so that we have half the range
            // on each side of the numbers, e.g. if decimalPlaces == 2,
            // then 0.01 will equal between 0.005 and 0.015, but not 0.02 and not 0.00
            return Math2.Abs(diff) < Pow10(-decimalPlaces) * 0.5M;
        }

        /// <summary>
        /// Compares two decimals and determines if they are equal within
        /// the specified maximum error.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="maximumError">The accuracy required for being almost equal.</param>
        public static bool AlmostEqualRelative(this decimal a, decimal b, decimal maximumError)
        {
            return AlmostEqualNormRelative(a, b, a - b, maximumError);
        }

        /// <summary>
        /// Checks whether two real numbers are almost equal.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>true if the two values differ by no more than 10 * 2^(-52); false otherwise.</returns>
        public static bool AlmostEqualRelative(this decimal a, decimal b)
        {
            return AlmostEqualNormRelative(a, b, a - b, DefaultDecimalAccuracy);
        }

        /// <summary>
        /// Compares two decimals and determines if they are equal
        /// within the specified maximum error.
        /// </summary>
        /// <param name="a">The norm of the first value (can be negative).</param>
        /// <param name="b">The norm of the second value (can be negative).</param>
        /// <param name="diff">The norm of the difference of the two values (can be negative).</param>
        /// <param name="maximumError">The accuracy required for being almost equal.</param>
        /// <returns>True if both decimals are almost equal up to the specified maximum error, false otherwise.</returns>
        public static bool AlmostEqualNormRelative(this decimal a, decimal b, decimal diff, decimal maximumError)
        {
            //// If A or B are infinity (positive or negative) then
            //// only return true if they are exactly equal to each other -
            //// that is, if they are both infinities of the same sign.
            //if (decimal.IsInfinity(a) || decimal.IsInfinity(b))
            //{
            //    return a == b;
            //}

            //// If A or B are a NAN, return false. NANs are equal to nothing,
            //// not even themselves.
            //if (decimal.IsNaN(a) || decimal.IsNaN(b))
            //{
            //    return false;
            //}

            // If one is almost zero, fall back to absolute equality
            if (Math.Abs(a) < DecimalPrecision || Math.Abs(b) < DecimalPrecision)
            {
                return Math.Abs(diff) < maximumError;
            }

            if ((a == 0 && Math.Abs(b) < maximumError) || (b == 0 && Math.Abs(a) < maximumError))
            {
                return true;
            }

            return Math.Abs(diff) < maximumError * Math.Max(Math.Abs(a), Math.Abs(b));
        }

        /// <summary>
        /// Compares two doubles and determines if they are equal to within the tolerance or not. Equality comparison is based on the binary representation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Determines the 'number' of floating point numbers between two values (i.e. the number of discrete steps
        /// between the two numbers) and then checks if that is within the specified tolerance. So if a tolerance
        /// of 1 is passed then the result will be true only if the two numbers have the same binary representation
        /// OR if they are two adjacent numbers that only differ by one step.
        /// </para>
        /// <para>
        /// The comparison method used is explained in http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm . The article
        /// at http://www.extremeoptimization.com/resources/Articles/FPDotNetConceptsAndFormats.aspx explains how to transform the C code to
        /// .NET enabled code without using pointers and unsafe code.
        /// </para>
        /// </remarks>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="maxNumbersBetween">The maximum number of floating point values between the two values. Must be 1 or larger.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxNumbersBetween"/> is smaller than one.</exception>
        public static bool AlmostEqualNumbersBetween(this decimal a, decimal b, long maxNumbersBetween)
        {
            // Make sure maxNumbersBetween is non-negative and small enough that the
            // default NAN won't compare as equal to anything.
            if (maxNumbersBetween < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxNumbersBetween));
            }

            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            //if (double.IsInfinity(a) || double.IsInfinity(b))
            //{
            //    return a == b;
            //}

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            //if (double.IsNaN(a) || double.IsNaN(b))
            //{
            //    return false;
            //}

            // Get the first double and convert it to an integer value (by using the binary representation)
            long firstUlong = AsDirectionalInt64(a);

            // Get the second double and convert it to an integer value (by using the binary representation)
            long secondUlong = AsDirectionalInt64(b);

            // Now compare the values.
            // Note that this comparison can overflow so we'll approach this differently
            // Do note that we could overflow this way too. We should probably check that we don't.
            return (a > b) ? (secondUlong + maxNumbersBetween >= firstUlong) : (firstUlong + maxNumbersBetween >= secondUlong);
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        static decimal Truncate(decimal value)
        {
            return Math.Truncate(value);
        }

        /// <summary>
        /// Returns the magnitude of the number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The magnitude of the number.</returns>
        public static int Magnitude(this decimal value)
        {
            // Can't do this with zero because the 10-log of zero doesn't exist.
            if (value.Equals(0.0))
            {
                return 0;
            }

            // Note that we need the absolute value of the input because Log10 doesn't
            // work for negative numbers (obviously).
            decimal magnitude = Math2.Log10(Math2.Abs(value));
            var truncated = (int)Truncate(magnitude);

            // To get the right number we need to know if the value is negative or positive
            // truncating a positive number will always give use the correct magnitude
            // truncating a negative number will give us a magnitude that is off by 1 (unless integer)
            return magnitude < 0M && truncated != magnitude
                ? truncated - 1
                : truncated;
        }

        /// <summary>
        /// Returns the number divided by it's magnitude, effectively returning a number between -10 and 10.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of the number.</returns>
        public static decimal ScaleUnitMagnitude(this decimal value)
        {
            if (value.Equals(0.0))
            {
                return value;
            }

            int magnitude = Magnitude(value);
            return value * Math2.Pow(10M, -magnitude);
        }

        /// <summary>
        /// Returns a 'directional' long value. This is a long value which acts the same as a double,
        /// e.g. a negative double value will return a negative double value starting at 0 and going
        /// more negative as the double value gets more negative.
        /// </summary>
        /// <param name="value">The input double value.</param>
        /// <returns>A long value which is roughly the equivalent of the double value.</returns>
        static long AsDirectionalInt64(decimal value)
        {
            // Convert in the normal way.
            long result = BitConverter2.DecimalToInt64Bits(value); //BitConverter.DoubleToInt64Bits(value);

            // Now find out where we're at in the range
            // If the value is larger/equal to zero then we can just return the value
            // if the value is negative we subtract long.MinValue from it.
            return (result >= 0) ? result : (long.MinValue - result);
        }


        /// <summary>
        /// Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <param name="value">The value used to determine the minimum distance.</param>
        /// <returns>
        /// Relative Epsilon (positive decimal).
        /// </returns>
        /// <remarks>Evaluates the <b>negative</b> epsilon. The more common positive epsilon is equal to two times this negative epsilon.</remarks>
        /// <seealso cref="PositiveEpsilonOf(double)"/>
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

        public static float ToSingle(this decimal value)
        {
            return (float)value;
        }

        static readonly decimal[] NegativePowersOf10 = new decimal[]
        {
            1, 0.1M, 0.01M, 1e-3M, 1e-4M, 1e-5M, 1e-6M, 1e-7M, 1e-8M, 1e-9M,
            1e-10M, 1e-11M, 1e-12M, 1e-13M, 1e-14M, 1e-15M, 1e-16M,
            1e-17M, 1e-18M, 1e-19M, 1e-20M, 1e-21M, 1e-22M, 1e-23M, 1e-24M
        };

        static decimal Pow10(int y)
        {
            return -NegativePowersOf10.Length < y && y <= 0
               ? NegativePowersOf10[-y]
               : Math2.Pow(10.0M, y);
        }
    }
}
