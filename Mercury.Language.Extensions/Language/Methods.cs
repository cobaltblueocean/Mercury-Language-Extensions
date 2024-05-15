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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language;
using Mercury.Language;

namespace System
{
    /// <summary>
    /// A set of utility methods that provide additional functionality for working
    /// with dates and times.
    /// <p>
    /// The contents of this class replace functionality available in JDK 8.
    ///
    /// <h3>Specification for implementors</h3>
    /// This is a thread-safe utility class.
    /// All returned classes are immutable and thread-safe.
    /// </summary>
    static class Methods
    {
        /// <summary>
        /// Ensures that the argument is non-null.
        /// </summary>
        /// <typeparam name="T">the value type</typeparam>
        /// <param name="value">the value to check</param>
        /// <returns>the checked non-null value</returns>
        public static T RequireNonNull<T>(T value)
        {
            if (value == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().METHOD_VALUE_MUST_NOT_BE_NULL);
            }
            return value;
        }

        /// <summary>
        /// Ensures that the argument is non-null.
        /// </summary>
        /// <typeparam name="T">the value type</typeparam>
        /// <param name="value">the value to check</param>
        /// <param name="parameterName">the name of the parameter</param>
        /// <returns>the checked non-null value</returns>
        public static T RequireNonNull<T>(T value, String parameterName)
        {
            if (value == null)
            {
                throw new NullReferenceException(String.Format(LocalizedResources.Instance().METHOD_PARAMETER_MUST_NOT_BE_NULL, parameterName));
            }
            return value;
        }

        /// <summary>
        /// Compares two ints.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
        public static int CompareInts(int a, int b)
        {
            if (a < b)
            {
                return -1;
            }
            if (a > b)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compares two longs.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
        public static int CompareLongs(long a, long b)
        {
            if (a < b)
            {
                return -1;
            }
            if (a > b)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Safely adds two int values.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
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
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
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
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
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
        /// Safely multiply one long by another.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>the result</returns>
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
        /// <returns>the result</returns>
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

        /// <summary>
        /// Safely convert a long to an int.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>the int value</returns>
        public static int SafeToInt(long value)
        {
            if (value > int.MaxValue || value < int.MinValue)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().METHOD_CALCULACTION_OVERFLOWS_INT, value));
            }
            return (int)value;
        }

        /// <summary>
        /// Returns the floor division.
        /// 
        /// This returns 0 for FloorDiv(0, 4).
        /// This returns -1 for FloorDiv(-1, 4).
        /// This returns -1 for FloorDiv(-2, 4).
        /// This returns -1 for FloorDiv(-3, 4).
        /// This returns -1 for FloorDiv(-4, 4).
        /// This returns -2 for FloorDiv(-5, 4).
        /// </summary>
        /// <param name="a">the dividend</param>
        /// <param name="b">the divisor</param>
        /// <returns>the floor division</returns>
        public static long FloorDiv(long a, long b)
        {
            return (a >= 0 ? a / b : ((a + 1) / b) - 1);
        }

        /// <summary>
        /// Returns the floor modulus.
        ///
        /// This returns 0 for FloorMod(0, 4).
        /// This returns 1 for FloorMod(-1, 4).
        /// This returns 2 for FloorMod(-2, 4).
        /// This returns 3 for FloorMod(-3, 4).
        /// This returns 0 for FloorMod(-4, 4).
        /// </summary>
        /// <param name="a">the dividend</param>
        /// <param name="b">the divisor</param>
        /// <returns>the floor modulus (positive)</returns>
        public static long FloorMod(long a, long b)
        {
            return ((a % b) + b) % b;
        }

        /// <summary>
        /// Returns the floor modulus.
        ///
        /// This returns 0 for FloorMod(0, 4).
        /// This returns 3 for FloorMod(-1, 4).
        /// This returns 2 for FloorMod(-2, 4).
        /// This returns 1 for FloorMod(-3, 4).
        /// This returns 0 for FloorMod(-4, 4).
        /// This returns 3 for FloorMod(-5, 4).
        /// </summary>
        /// <param name="a">the dividend</param>
        /// <param name="b">the divisor</param>
        /// <returns>the floor modulus (positive)</returns>
        public static int FloorMod(long a, int b)
        {
            return (int)(((a % b) + b) % b);
        }

        /// <summary>
        /// Returns the floor division.
        ///
        /// This returns 1 for FloorDiv(3, 3).
        /// This returns 0 for FloorDiv(2, 3).
        /// This returns 0 for FloorDiv(1, 3).
        /// This returns 0 for FloorDiv(0, 3).
        /// This returns -1 for FloorDiv(-1, 3).
        /// This returns -1 for FloorDiv(-2, 3).
        /// This returns -1 for FloorDiv(-3, 3).
        /// This returns -2 for FloorDiv(-4, 3).
        /// </summary>
        /// <param name="a">the dividend</param>
        /// <param name="b">the divisor</param>
        /// <returns>the floor division</returns>
        public static int FloorDiv(int a, int b)
        {
            return (a >= 0 ? a / b : ((a + 1) / b) - 1);
        }

        /// <summary>
        /// Returns the floor modulus.
        ///
        /// This returns 0 for FloorDiv(3, 3).
        /// This returns 2 for FloorDiv(2, 3).
        /// This returns 1 for FloorDiv(1, 3).
        /// This returns 0 for FloorDiv(0, 3).
        /// This returns 2 for FloorDiv(-1, 3).
        /// This returns 1 for FloorDiv(-2, 3).
        /// This returns 0 for FloorDiv(-3, 3).
        /// This returns 2 for FloorDiv(-4, 3).
        /// </summary>
        /// <param name="a">the dividend</param>
        /// <param name="b">the divisor</param>
        /// <returns>the floor modulus (positive)</returns>
        public static int FloorMod(int a, int b)
        {
            return ((a % b) + b) % b;
        }
    }
}
