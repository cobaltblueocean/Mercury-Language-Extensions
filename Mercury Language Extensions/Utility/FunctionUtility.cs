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
using Mercury.Language.Exception;

namespace System
{
    /// <summary>
    /// A collection of basic useful maths functions
    /// </summary>
    public sealed class FunctionUtility
    {
        /// <summary>
        /// Returns the square of a number
        /// </summary>
        /// <param name="x">some number</param>
        /// <returns>x*x</returns>
        public static double Square(double x)
        {
            return x * x;
        }

        /// <summary>
        /// Returns the cube of a number
        /// </summary>
        /// <param name="x">some number</param>
        /// <returns>x*x*x</returns>
        public static double Cube(double x)
        {
            return x * x * x;
        }

        public static int ToTensorIndex(int[] indices, int[] dimensions)
        {
            // ArgumentChecker.NotNull(indices, "indices");
            // ArgumentChecker.NotNull(dimensions, "dimensions");
            int dim = indices.Length;
            // ArgumentChecker.IsTrue(dim == dimensions.Length);
            int sum = 0;
            int product = 1;
            for (int i = 0; i < dim; i++)
            {
                // ArgumentChecker.IsTrue(indices[i] < dimensions[i], "index out of bounds");
                sum += indices[i] * product;
                product *= dimensions[i];
            }
            return sum;
        }

        public static int[] FromTensorIndex(int index, int[] dimensions)
        {
            // ArgumentChecker.NotNull(dimensions, "dimensions");
            int dim = dimensions.Length;
            int[] res = new int[dim];

            int product = 1;
            int[] products = new int[dim - 1];
            for (int i = 0; i < dim - 1; i++)
            {
                product *= dimensions[i];
                products[i] = product;
            }

            int a = index;
            for (int i = dim - 1; i > 0; i--)
            {
                res[i] = a / products[i - 1];
                a -= res[i] * products[i - 1];
            }
            res[0] = a;

            return res;
        }

        /// <summary>
        /// Same behaviour as mathlab unique
        /// </summary>
        /// <param name="v">input array</param>
        /// <returns>a sorted array with no duplicates values</returns>
        public static double[] Unique(double[] v)
        {
            Array.Sort(v);
            int n = v.Length;
            double[] temp = new double[n];
            temp[0] = v[0];
            int count = 1;
            for (int i = 1; i < n; i++)
            {
                if (v[i].CompareTo(v[i - 1]) != 0)
                {
                    temp[count++] = v[i];
                }
            }
            if (count == n)
            {
                return temp;
            }
            return temp.CopyOf(count);
        }

        /// <summary>
        /// Same behaviour as mathlab unique
        /// </summary>
        /// <param name="v">input array</param>
        /// <returns>a sorted array with no duplicates values</returns>
        public static int[] Unique(int[] v)
        {
            Array.Sort(v);
            int n = v.Length;
            int[] temp = new int[n];
            temp[0] = v[0];
            int count = 1;
            for (int i = 1; i < n; i++)
            {
                if (v[i] != v[i - 1])
                {
                    temp[count++] = v[i];
                }
            }
            if (count == n)
            {
                return temp;
            }
            return v.CopyOf(count);
        }

        /// <summary>
        /// Find the index of a <b>sorted</b> set that is less than or equal to a given value. If the given value is lower than the lowest member (i.e. the first)
        /// of the set, zero is returned.  This uses Array.BinarySearch
        /// </summary>
        /// <param name="set">a <b>sorted</b> array of numbers.</param>
        /// <param name="value">The value to search for</param>
        /// <returns>index in the array</returns>
        public static int GetLowerBoundIndex(double[] set, double value)
        {
            int n = set.Length;
            if (value < set[0])
            {
                return 0;
            }
            if (value > set[n - 1])
            {
                return n - 1;
            }
            int index = Array.BinarySearch(set, value);
            if (index >= 0)
            {
                // Fast break out if it's an exact match.
                return index;
            }
            if (index < 0)
            {
                index = -(index + 1);
                index--;
            }
            if (value == -0 && index < n - 1 && set[index + 1] == 0)
            {
                ++index;
            }
            return index;
        }

        /// <summary>
        ///   Hypotenuse calculus without overflow/underflow
        /// </summary>
        /// 
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// 
        /// <returns>The hypotenuse Sqrt(a^2 + b^2)</returns>
        /// 
        public static double Hypotenuse(double a, double b)
        {
            double r = 0.0;
            double absA = System.Math.Abs(a);
            double absB = System.Math.Abs(b);

            if (absA > absB)
            {
                r = b / a;
                r = absA * System.Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                r = absB * System.Math.Sqrt(1 + r * r);
            }

            return r;
        }

        /// <summary>
        ///   Hypotenuse calculus without overflow/underflow
        /// </summary>
        /// 
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// 
        /// <returns>The hypotenuse Sqrt(a^2 + b^2)</returns>
        /// 
        public static decimal Hypotenuse(decimal a, decimal b)
        {
            decimal r = 0;
            decimal absA = System.Math.Abs(a);
            decimal absB = System.Math.Abs(b);

            if (absA > absB)
            {
                r = b / a;
                r = absA * (decimal)System.Math.Sqrt((double)(1 + r * r));
            }
            else if (b != 0)
            {
                r = a / b;
                r = absB * (decimal)System.Math.Sqrt((double)(1 + r * r));
            }

            return r;
        }

        /// <summary>
        ///   Hypotenuse calculus without overflow/underflow
        /// </summary>
        /// 
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// 
        /// <returns>The hypotenuse Sqrt(a^2 + b^2)</returns>
        /// 
        public static float Hypotenuse(float a, float b)
        {
            double r = 0;
            float absA = System.Math.Abs(a);
            float absB = System.Math.Abs(b);

            if (absA > absB)
            {
                r = b / a;
                r = absA * System.Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                r = absB * System.Math.Sqrt(1 + r * r);
            }

            return (float)r;
        }
    }
}
