// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Utility Inc.
//
// Copyright (C) 2012 - present by System.Utility Inc. and the System.Utility group of companies
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
using Mercury.Language.Exceptions;

namespace System
{
    /// <summary>
    /// Utility methods to simplify comparisons.
    /// This is a thread-safe static utility class.
    /// </summary>
    public sealed class CompareUtility
    {
        /// <summary>
        /// Compares two objects finding the maximum.
        /// </summary>
        /// <typeparam name="T">the object type</typeparam>
        /// <param name="a">item that CompareTo is called on, may be null</param>
        /// <param name="b">item that is being compared, may be null</param>
        /// <returns>the maximum of the two objects, null if both null</returns>
        public static T Max<T>(T a, T b) where T: IComparable<T>
        {
            if (a != null && b != null)
            {
                return a.CompareTo(b) >= 0 ? a : b;
            }
            if (a == null)
            {
                if (b == null)
                {
                    return default(T);
                }
                else
                {
                    return b;
                }
            }
            else
            {
                return a;
            }
        }

        /// <summary>
        /// Compares two objects finding the minimum.
        /// </summary>
        /// <typeparam name="T">the object type</typeparam>
        /// <param name="a">item that CompareTo is called on, may be null</param>
        /// <param name="b">item that is being compared, may be null</param>
        /// <returns>the minimum of the two objects, null if both null</returns>
        public static T Min<T>(T a, T b) where T : IComparable<T>
        {
            if (a != null && b != null)
            {
                return a.CompareTo(b) <= 0 ? a : b;
            }
            if (a == null)
            {
                if (b == null)
                {
                    return default(T);
                }
                else
                {
                    return b;
                }
            }
            else
            {
                return a;
            }
        }

        /// <summary>
        /// Compares two objects, either of which might be null, sorting nulls low.
        /// </summary>
        /// <typeparam name="E">the type of object being compared</typeparam>
        /// <param name="a">item that CompareTo is called on</param>
        /// <param name="b">item that is being compared</param>
        /// <returns>negative when a less than b, zero when equal, positive when greater</returns>
        public static int CompareWithNullLow<E>(IComparable<E> a, E b)
        {
            if (a == null)
            {
                return b == null ? 0 : -1;
            }
            else if (b == null)
            {
                return 1; // a not null
            }
            else
            {
                return a.CompareTo((E)b);
            }
        }

        /// <summary>
        /// Compares two objects, either of which might be null, sorting nulls high.
        /// </summary>
        /// <typeparam name="E">the type of object being compared</typeparam>
        /// <param name="a">item that CompareTo is called on</param>
        /// <param name="b">item that is being compared</param>
        /// <returns>negative when a less than b, zero when equal, positive when greater</returns>
        public static int CompareWithNullHigh<E>(IComparable<E> a, E b)
        {
            if (a == null)
            {
                return b == null ? 0 : 1;
            }
            else if (b == null)
            {
                return -1; // a not null
            }
            else
            {
                return a.CompareTo((E)b);
            }
        }

        /// <summary>
        /// Compare two doubles to see if they're 'closely' equal.
        /// 
        /// This handles rounding errors which can mean the results of double precision computations
        /// lead to small differences in results.
        /// The definition 'close' is that the difference is less than 10^-15 (1E-15).
        /// If a different maximum allowed difference is required, use the other version of this method.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <returns>true, if a and b are equal to within 10^-15, false otherwise</returns>
        public static Boolean CloseEquals(double a, double b)
        {
            if (Double.IsInfinity(a))
            {
                return (a == b);
            }
            return (System.Math.Abs(a - b) < 1E-15);
        }

        /// <summary>
        /// Compare two doubles to see if they're 'closely' equal.
        /// 
        /// This handles rounding errors which can mean the results of double precision computations
        /// lead to small differences in results.
        /// The definition 'close' is that the absolute difference is less than the specified difference.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <param name="maxDifference">the maximum difference to allow</param>
        /// <returns>true, if a and b are equal to within the tolerance</returns>
        public static Boolean CloseEquals(double a, double b, double maxDifference)
        {
            if (Double.IsInfinity(a))
            {
                return (a == b);
            }
            return (System.Math.Abs(a - b) < maxDifference);
        }

        /// <summary>
        /// Compare two doubles to see if they're 'closely' equal.
        /// 
        /// This handles rounding errors which can mean the results of double precision computations
        /// lead to small differences in results.
        /// This method returns the difference to indicate how the first differs from the second.
        /// </summary>
        /// <param name="a">the first value</param>
        /// <param name="b">the second value</param>
        /// <param name="maxDifference">the maximum difference to allow while still considering the values equal</param>
        /// <returns>the value 0 if a and b are equal to within the tolerance; a value less than 0 if a is numerically less than b; and a value greater than 0 if a is numerically greater than b.</returns>
        public static int CompareWithTolerance(double a, double b, double maxDifference)
        {
            if (a == Double.PositiveInfinity)
            {
                return (a == b ? 0 : 1);
            }
            else if (a == Double.NegativeInfinity)
            {
                return (a == b ? 0 : -1);
            }
            else if (b == Double.PositiveInfinity)
            {
                return (a == b ? 0 : -1);
            }
            else if (b == Double.NegativeInfinity)
            {
                return (a == b ? 0 : 1);
            }
            if (System.Math.Abs(a - b) < maxDifference)
            {
                return 0;
            }
            return (a < b) ? -1 : 1;
        }

        /// <summary>
        /// Compare two items, with the ordering determined by a list of those items.
        /// 
        /// Nulls are permitted and sort low, and if a or b are not in the list, then the result of comparing the toString() output is used instead.
        /// </summary>
        /// <typeparam name="T">the list type</typeparam>
        /// <param name="list">the list, not null</param>
        /// <param name="a">the first object, may be null</param>
        /// <param name="b">the second object, may be null</param>
        /// <returns>0, if equal, -1 if a < b, +1 if a > b</returns>
        public static int CompareByList<T>(List<T> list, T a, T b)
        {
            if (a == null)
            {
                if (b == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (b == null)
                {
                    return 1;
                }
                else
                {
                    if (list.Contains(a) && list.Contains(b))
                    {
                        return list.IndexOf(a) - list.IndexOf(b);
                    }
                    else
                    {
                        return CompareWithNullLow(a.ToString(), b.ToString());
                    }
                }
            }
        }
    }
}
