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
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Performs sortingd This is not "parallel" in the sense of threads, but parallel in the sense that
    /// two arrays are sorted in parallel.
    /// </summary>
    public static class ArrayUtility
    {
        #region "Public Static Methods"
        /// <summary>
        /// Sort the content of keys and values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values">The values</param>
        public static void ParallelBinarySort(Double?[] keys, Double?[] values)
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values, "y data");
            // ArgumentChecker.IsTrue(keys.Length == values.Length);
            int n = keys.Length;
            TripleArrayQuickSort(keys, values, null, 0, n - 1);
        }

        /// <summary>
        /// Sort the content of keys and values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place.
        /// Allow control over range of ordering for subarray ordering.
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values">The values</param>
        /// <param name="start">Starting point (0-based)</param>
        /// <param name="end">Final point (0-based, inclusive)</param>
        public static void ParallelBinarySort(Double?[] keys, Double?[] values, int start, int end)
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values, "y data");
            // ArgumentChecker.IsTrue(keys.Length == values.Length);
            // ArgumentChecker.IsTrue(start >= 0);
            // ArgumentChecker.IsTrue(end < keys.Length);
            TripleArrayQuickSort(keys, values, null, start, end);
        }

        /// <summary>
        /// Sort the content of keys and two sets of values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place.
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values1">The first set of values</param>
        /// <param name="values2">The second set of values</param>
        public static void ParallelBinarySort(Double?[] keys, Double?[] values1, Double?[] values2)
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values1, "y data 1");
            // ArgumentChecker.NotNull(values2, "y data 2");
            // ArgumentChecker.IsTrue(keys.Length == values1.Length, "keys.Length == values1.Length");
            // ArgumentChecker.IsTrue(keys.Length == values2.Length, "keys.Length == values2.Length");
            int n = keys.Length;
            TripleArrayQuickSort(keys, values1, values2, 0, n - 1);
        }

        public static double[] ConvertToArray(double value)
        {
            return new double[] { value };
        }

        public static double?[] ConvertToNullableArray(double value)
        {
            return new double?[] { value };
        }

        public static int[] ConvertToArray(int value)
        {
            return new int[] { value };
        }

        public static int?[] ConvertToNullableArray(int value)
        {
            return new int?[] { value };
        }

        public static float[] ConvertToArray(float value)
        {
            return new float[] { value };
        }

        public static float?[] ConvertToNullableArray(float value)
        {
            return new float?[] { value };
        }

        public static long[] ConvertToArray(long value)
        {
            return new long[] { value };
        }

        public static long?[] ConvertToNullableArray(long value)
        {
            return new long?[] { value };
        }

        public static decimal[] ConvertToArray(decimal value)
        {
            return new decimal[] { value };
        }

        public static decimal?[] ConvertToNullableArray(decimal value)
        {
            return new decimal?[] { value };
        }

        #endregion

        #region "Private Static Methods"

        /// <summary>
        /// quick sorts
        /// hard coded types
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values1"></param>
        /// <param name="values2"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void TripleArrayQuickSort(Double?[] keys, Double?[] values1, Double?[] values2, int left, int right)
        {
            if (right > left)
            {
                int pivot = (left + right) >> 1;
                int pivotNewIndex = Partition(keys, values1, values2, left, right, pivot);
                TripleArrayQuickSort(keys, values1, values2, left, pivotNewIndex - 1);
                TripleArrayQuickSort(keys, values1, values2, pivotNewIndex + 1, right);
            }
        }

        #region "Partitioins"
        private static int Partition(Double?[] keys, Double?[] values1, Double?[] values2, int left, int right, int pivot)
        {
            Double? pivotValue = keys[pivot];
            Swap(keys, values1, values2, pivot, right);
            int storeIndex = left;
            for (int i = left; i < right; i++)
            {
                if (keys[i] <= pivotValue)
                {
                    Swap(keys, values1, values2, i, storeIndex);
                    storeIndex++;
                }
            }
            Swap(keys, values1, values2, storeIndex, right);
            return storeIndex;
        }
        #endregion

        #region "Swaps"
        private static void Swap(Double?[] keys, Double?[] values1, Double?[] values2, int first, int second)
        {
            Double? t = keys[first];
            keys[first] = keys[second];
            keys[second] = t;
            if (values1 != null)
            {
                t = values1[first];
                values1[first] = values1[second];
                values1[second] = t;
            }
            if (values2 != null)
            {
                t = values2[first];
                values2[first] = values2[second];
                values2[second] = t;
            }
        }

        #endregion
        #endregion

        #region "Public Static Methods"
        /// <summary>
        /// Sort the content of keys and values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values">The values</param>
        public static void ParallelBinarySort(double[] keys, double[] values)
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values, "y data");
            // ArgumentChecker.IsTrue(keys.Length == values.Length);
            int n = keys.Length;
            TripleArrayQuickSort(keys, values, null, 0, n - 1);
        }

        /// <summary>
        /// Sort the content of keys and values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place.
        /// Allow control over range of ordering for subarray ordering.
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values">The values</param>
        /// <param name="start">Starting point (0-based)</param>
        /// <param name="end">Final point (0-based, inclusive)</param>
        public static void ParallelBinarySort(double[] keys, double[] values, int start, int end)
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values, "y data");
            // ArgumentChecker.IsTrue(keys.Length == values.Length);
            // ArgumentChecker.IsTrue(start >= 0);
            // ArgumentChecker.IsTrue(end < keys.Length);
            TripleArrayQuickSort(keys, values, null, start, end);
        }

        /// <summary>
        /// Sort the content of keys and two sets of values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place.
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values1">The first set of values</param>
        /// <param name="values2">The second set of values</param>
        public static void ParallelBinarySort(double[] keys, double[] values1, double[] values2)
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values1, "y data 1");
            // ArgumentChecker.NotNull(values2, "y data 2");
            // ArgumentChecker.IsTrue(keys.Length == values1.Length, "keys.Length == values1.Length");
            // ArgumentChecker.IsTrue(keys.Length == values2.Length, "keys.Length == values2.Length");
            int n = keys.Length;
            TripleArrayQuickSort(keys, values1, values2, 0, n - 1);
        }

        /// <summary>
        /// Create a 2 dimension jagged array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <returns></returns>
        public static T[][] CreateJaggedArray<T>(int len1, int? len2)
        {
            T[][] retArray = new T[len1][];

            if (len2.HasValue)
            {
                for (int i = 0; i < len1; i++)
                {
                    retArray[i] = new T[len2.Value];
                }
            }

            return retArray;
        }

        /// <summary>
        /// Create a 3 dimension jagged array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <param name="len3"></param>
        /// <returns></returns>
        public static T[][][] CreateJaggedArray<T>(int len1, int len2, int? len3)
        {
            T[][][] retArray = new T[len1][][];

            for (int i = 0; i < len1; i++)
            {
                retArray[i] = new T[len2][];

                if (len3.HasValue)
                {
                    for (int j = 0; j < len2; j++)
                    {
                        retArray[i][j] = new T[len3.Value];
                    }
                }
            }

            return retArray;
        }

        #endregion

        #region "Private Static Methods"
        /// <summary>
        /// quick sorts
        /// hard coded types
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values1"></param>
        /// <param name="values2"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void TripleArrayQuickSort(double[] keys, double[] values1, double[] values2, int left, int right)
        {
            if (right > left)
            {
                int pivot = (left + right) >> 1;
                int pivotNewIndex = Partition(keys, values1, values2, left, right, pivot);
                TripleArrayQuickSort(keys, values1, values2, left, pivotNewIndex - 1);
                TripleArrayQuickSort(keys, values1, values2, pivotNewIndex + 1, right);
            }
        }

        #region "Partitioins"
        private static int Partition(double[] keys, double[] values1, double[] values2, int left, int right, int pivot)
        {
            double pivotValue = keys[pivot];
            Swap(keys, values1, values2, pivot, right);
            int storeIndex = left;
            for (int i = left; i < right; i++)
            {
                if (keys[i] <= pivotValue)
                {
                    Swap(keys, values1, values2, i, storeIndex);
                    storeIndex++;
                }
            }
            Swap(keys, values1, values2, storeIndex, right);
            return storeIndex;
        }
        #endregion

        #region "Swaps"
        private static void Swap(double[] keys, double[] values1, double[] values2, int first, int second)
        {
            double t = keys[first];
            keys[first] = keys[second];
            keys[second] = t;
            if (values1 != null)
            {
                t = values1[first];
                values1[first] = values1[second];
                values1[second] = t;
            }
            if (values2 != null)
            {
                t = values2[first];
                values2[first] = values2[second];
                values2[second] = t;
            }
        }

        #endregion
        #endregion

    }
}
