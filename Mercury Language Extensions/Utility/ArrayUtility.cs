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
        #region Public Static Fields
        public static Boolean[] EMPTY_BOOLEAN_ARRAY = new Boolean[0];
        public static Boolean?[] EMPTY_BOOLEAN_OBJECT_ARRAY = new Boolean?[0];
        public static byte[] EMPTY_BYTE_ARRAY = new byte[0];
        public static Byte?[] EMPTY_BYTE_OBJECT_ARRAY = new Byte?[0];
        public static char[] EMPTY_CHAR_ARRAY = new char[0];
        public static Char?[] EMPTY_CHARACTER_OBJECT_ARRAY = new Char?[0];
        //public static T[] EMPTY_CLASS_ARRAY<T> = new T[0]();
        public static double[] EMPTY_DOUBLE_ARRAY = new double[0];
        public static Double?[] EMPTY_DOUBLE_OBJECT_ARRAY = new Double?[0];
        //public static Field[] EMPTY_FIELD_ARRAY = new Field[0];
        public static float[] EMPTY_FLOAT_ARRAY = new float[0];
        public static float?[] EMPTY_FLOAT_OBJECT_ARRAY = new float?[0];
        public static int[] EMPTY_INT_ARRAY = new int[0];
        public static int?[] EMPTY_INTEGER_OBJECT_ARRAY = new int?[0];
        public static long[] EMPTY_LONG_ARRAY = new long[0];
        public static long?[] EMPTY_LONG_OBJECT_ARRAY = new long?[0];
        //public static Method[] EMPTY_METHOD_ARRAY = new Method[0];
        public static Object[] EMPTY_OBJECT_ARRAY = new Object[0];
        public static short[] EMPTY_SHORT_ARRAY = new short[0];
        public static short?[] EMPTY_SHORT_OBJECT_ARRAY = new short?[0];
        public static String[] EMPTY_STRING_ARRAY = new String[0];
        //public static Throwable[] EMPTY_THROWABLE_ARRAY = new Throwable[0];
        public static Type[] EMPTY_TYPE_ARRAY = new Type[0];
        public static int INDEX_NOT_FOUND = -1;
        #endregion

        #region "Public Static Methods"
        /// <summary>
        /// Return Empty Class array
        /// </summary>
        /// <typeparam name="T">Type of the Array would like to get</typeparam>
        /// <returns>Class array</returns>
        public static T[] GetEmptyClassArray<T>()
        {
            return new T[0];
        }

        /// <summary>
        /// Sort the content of keys and values simultaneously so that
        /// both match the correct ordering. Alters the arrays in place
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values">The values</param>
        public static void ParallelBinarySort<T>(Nullable<T>[] keys, Nullable<T>[] values) where T : struct
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
        public static void ParallelBinarySort<T>(Nullable<T>[] keys, Nullable<T>[] values, int start, int end) where T : struct
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
        public static void ParallelBinarySort<T>(Nullable<T>[] keys, Nullable<T>[] values1, Nullable<T>[] values2) where T : struct
        {
            // ArgumentChecker.NotNull(keys, "x data");
            // ArgumentChecker.NotNull(values1, "y data 1");
            // ArgumentChecker.NotNull(values2, "y data 2");
            // ArgumentChecker.IsTrue(keys.Length == values1.Length, "keys.Length == values1.Length");
            // ArgumentChecker.IsTrue(keys.Length == values2.Length, "keys.Length == values2.Length");
            int n = keys.Length;
            TripleArrayQuickSort(keys, values1, values2, 0, n - 1);
        }

        public static T[] ConvertToArray<T>(T value)
        {
            return new T[] { value };
        }

        public static Nullable<T>[] ConvertToNullableArray<T>(T value) where T : struct
        {
            return new Nullable<T>[] { value };
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
        private static void TripleArrayQuickSort<T>(Nullable<T>[] keys, Nullable<T>[] values1, Nullable<T>[] values2, int left, int right) where T : struct
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
        private static int Partition<T>(Nullable<T>[] keys, Nullable<T>[] values1, Nullable<T>[] values2, int left, int right, int pivot) where T : struct
        {
            Nullable<T> pivotValue = keys[pivot];
            Swap(keys, values1, values2, pivot, right);
            int storeIndex = left;
            for (int i = left; i < right; i++)
            {
                if (keys[i].LessThanOrEqualTo(pivotValue))
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
        private static void Swap<T>(Nullable<T>[] keys, Nullable<T>[] values1, Nullable<T>[] values2, int first, int second) where T : struct
        {
            Nullable<T> t = keys[first];
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
        public static void ParallelBinarySort<T>(T[] keys, T[] values) where T : struct
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
        public static void ParallelBinarySort<T>(T[] keys, T[] values, int start, int end) where T : struct
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
        public static void ParallelBinarySort<T>(T[] keys, T[] values1, T[] values2) where T : struct
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
        private static void TripleArrayQuickSort<T>(T[] keys, T[] values1, T[] values2, int left, int right) where T : struct
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
        private static int Partition<T>(T[] keys, T[] values1, T[] values2, int left, int right, int pivot) where T : struct
        {
            T pivotValue = keys[pivot];
            Swap(keys, values1, values2, pivot, right);
            int storeIndex = left;
            for (int i = left; i < right; i++)
            {
                if (keys[i].LessThanOrEqualTo(pivotValue))
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
        private static void Swap<T>(T[] keys, T[] values1, T[] values2, int first, int second) where T : struct
        {
            T t = keys[first];
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
