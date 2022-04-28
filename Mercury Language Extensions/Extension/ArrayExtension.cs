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
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math;
using Mercury.Language.Math.Ranges;
using Mercury.Language.Properties;
using Mercury.Language;

namespace System
{
    public static class ArrayExtension
    {
        /// <summary>
        ///   Directions for the General Comparer.
        /// </summary>
        /// 
        public enum ComparerDirection
        {
            /// <summary>
            ///   Sorting will be performed in ascending order.
            /// </summary>
            /// 
            Ascending = +1,

            /// <summary>
            ///   Sorting will be performed in descending order.
            /// </summary>
            /// 
            Descending = -1
        };

        /// <summary>
        ///   Creates a range vector (like NumPy's arange function).
        /// </summary>
        /// 
        /// <param name="n">The exclusive upper bound of the range.</param>
        ///
        /// <remarks>
        /// <para>
        ///   The Range methods should be equivalent to NumPy's np.arange method, with one
        ///   single difference: when the intervals are inverted (i.e. a > b) and the step
        ///   size is negative, the framework still iterates over the range backwards, as 
        ///   if the step was negative.</para>
        /// <para>
        ///   This function never includes the upper bound of the range. For methods
        ///   that include it, please see the <see cref="Interval(int, int)"/> methods.</para>  
        /// </remarks>
        ///
        /// <seealso cref="Interval(int, int)"/>
        ///
        private static int[] Range(int n)
        {
            int[] r = new int[(int)n];
            for (int i = 0; i < r.Length; i++)
                r[i] = (int)i;
            return r;
        }


        /// <summary>
        ///   Gets the indices that sort a vector.
        /// </summary>
        /// 
        public static int[] ArgSort<T>(this T[] values)
            where T : IComparable<T>
        {
            int[] idx;
            values.Copy().Sort(out idx);
            return idx;
        }

        public static T[] Add<T>(this T[] array, int index, T value)
        {
            T[] target = new T[array.Length + 1];
            int j = 0;

            for (int i = 0; i < array.Length + 1; i++)
            {
                if (i == index)
                {
                    target[j] = value;
                    j++;
                }
                target[j] = array[i];
                j++;
            }

            return target;
        }


        public static T[] Add<T>(this T[] array, T value)
        {
            T[] target = new T[array.Length + 1];
            array.CopyTo(target, 0);
            target[target.Length - 1] = value;

            return target;
        }

        /// <summary>
        ///   Sorts the elements of an entire one-dimensional array using the given comparison.
        /// </summary>
        /// 
        public static void Sort<T>(this T[] values, out int[] order, bool stable = false, ComparerDirection direction = ComparerDirection.Ascending)
            where T : IComparable<T>
        {
            if (!stable)
            {
                order = Range(values.Length);
                Array.Sort(values, order);

                if (direction == ComparerDirection.Descending)
                {
                    Array.Reverse(values);
                    Array.Reverse(order);
                }
            }
            else
            {
                var keys = new KeyValuePair<int, T>[values.Length];
                for (var i = 0; i < values.Length; i++)
                    keys[i] = new KeyValuePair<int, T>(i, values[i]);

                if (direction == ComparerDirection.Ascending)
                    Array.Sort(keys, values, new StableComparer<T>((a, b) => a.CompareTo(b)));
                else
                    Array.Sort(keys, values, new StableComparer<T>((a, b) => -a.CompareTo(b)));

                order = new int[values.Length];
                for (int i = 0; i < keys.Length; i++)
                    order[i] = keys[i].Key;
            }
        }

        public static T[,] ArrayOf<T>(this T[,,] originalArray, int Index)
        {
            if (Index >= originalArray.GetLength(0))
            {
                throw new IndexOutOfRangeException();
            }

            var newArray = new T[originalArray.GetLength(1), originalArray.GetLength(2)];

            for (int i = 0; i < originalArray.GetLength(1); i++)
            {
                for (int j = 0; j < originalArray.GetLength(2); j++)
                {
                    newArray[i, j] = originalArray[Index, i, j];
                }
            }

            return newArray;
        }


        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <param name="abort">Whether to throw an exception if the check fails.</param>
        /// <returns>true if the arrays have the same Length.</returns>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static Boolean CheckEqualLength(double[] a, double[] b, Boolean abort)
        {
            if (a.Length == b.Length)
            {
                return true;
            }
            else
            {
                if (abort)
                {
                    throw new DimensionMismatchException(a.Length, b.Length);
                }
                return false;
            }
        }

        public static bool VerifyValues<T>(this T[] values, int begin, int Length)
        {
            return VerifyValues(values, begin, Length, false);
        }

        public static bool VerifyValues<T>(this T[] values, int begin, int Length, bool allowEmpty)
        {
            if (values == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().INPUT_ARRAY);
            }

            if (begin < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().START_POSITION, begin);
            }

            if (Length < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().LENGTH, Length);
            }

            if (begin + Length > values.Length)
            {
                throw new NumberIsTooLargeException(LocalizedResources.Instance().SUBARRAY_ENDS_AFTER_ARRAY_END,
                        begin + Length, values.Length, true);
            }

            if (Length == 0 && !allowEmpty)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static void CheckEqualLength(double[] a, double[] b)
        {
            CheckEqualLength(a, b, true);
        }


        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <param name="abort">Whether to throw an exception if the check fails.</param>
        /// <returns>true if the arrays have the same Length.</returns>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static Boolean CheckEqualLength(int[] a, int[] b, Boolean abort)
        {
            if (a.Length == b.Length)
            {
                return true;
            }
            else
            {
                if (abort)
                {
                    throw new DimensionMismatchException(a.Length, b.Length);
                }
                return false;
            }
        }

        /// <summary>
        /// Check that both arrays have the same Length.
        /// </summary>
        /// <param name="a">Array</param>
        /// <param name="b">Array</param>
        /// <exception cref="DimensionMismatchException">if the Lengths differ.</exception>
        public static void CheckEqualLength(int[] a, int[] b)
        {
            CheckEqualLength(a, b, true);
        }

        //public static T[] CloneExact<T>(this T[] originalArray)
        //{
        //    return (T[])originalArray.Clone();
        //}

        /// <summary>
        /// Specification of ordering direction.
        /// <summary>
        public enum OrderDirection
        {
            /// <summary>Constant for increasing direction. */
            INCREASING,
            /// <summary>Constant for decreasing direction. */
            DECREASING
        }

        /// <summary>
        /// Check that the given array is sorted.
        /// 
        /// <summary>
        /// <param name="val">Values.</param>
        /// <param name="dir">Ordering direction.</param>
        /// <param name="strict">Whether the order should be strict.</param>
        /// <param name="abort">Whether to throw an exception if the check fails.</param>
        /// <returns>{@code true} if the array is sorted.</returns>
        /// <exception cref="NonMonotonicSequenceException">if the array is not sorted </exception>
        /// and {@code abort} is {@code true}.
        public static Boolean CheckOrder(this double[] val, OrderDirection dir,                                         Boolean strict, Boolean abort)
        {
            double previous = val[0];
            int max = val.Length;

            int index;
            //Boolean Ordered = false;

            for (index = 1; index < max; index++)
            {
                switch (dir)
                {
                    case OrderDirection.INCREASING:
                        if (strict)
                        {
                            if (val[index] <= previous)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (val[index] < previous)
                            {
                                break;
                            }
                        }
                        break;
                    case OrderDirection.DECREASING:
                        if (strict)
                        {
                            if (val[index] >= previous)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (val[index] > previous)
                            {
                                break;
                            }
                        }
                        break;
                    default:
                        // Should never happen.
                        throw new MathArithmeticException(Mercury.Language.LocalizedResources.Instance().ARRAY_INVALID_ORDER_DIRECTION);
                }

                previous = val[index];
            }

            if (index == max)
            {
                // Loop completed.
                return true;
            }

            // Loop early exit means wrong ordering.
            if (abort)
            {
                throw new MathArgumentException(Mercury.Language.LocalizedResources.Instance().ARRAY_THE_ARRAY_IS_NON_MONOTONIC_SEQUENCE, new Object[]{ val[index], previous, index, dir, strict });
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check that the given array is sorted.
        /// 
        /// <summary>
        /// <param name="val">Values.</param>
        /// <param name="dir">Ordering direction.</param>
        /// <param name="strict">Whether the order should be strict.</param>
        /// <exception cref="NonMonotonicSequenceException">if the array is not sorted. </exception>
        /// @since 2.2
        public static void CheckOrder(this double[] val, OrderDirection dir,                                      Boolean strict)
        {
            CheckOrder(val, dir, strict, true);
        }

        /// <summary>
        /// Check that the given array is sorted in strictly increasing order.
        /// 
        /// <summary>
        /// <param name="val">Values.</param>
        /// <exception cref="NonMonotonicSequenceException">if the array is not sorted. </exception>
        /// @since 2.2
        public static void CheckOrder(this double[] val)
        {
            CheckOrder(val, OrderDirection.INCREASING, true);
        }

        public static T[] Copy<T>(this T[] originalArray)
        {
            var newArray = new T[originalArray.Length];
            for (int i = 0; i < originalArray.Length; i++)
                newArray[i] = originalArray[i];

            return newArray;
        }

        /// <summary>
        ///   Creates a memberwise copy of a matrix. Matrix elements
        ///   themselves are copied only in a shallow manner (i.e. not cloned).
        /// </summary>
        /// 
        public static T[,] Copy<T>(this T[,] a)
        {
            return (T[,])a.Clone();
        }

        public static T[] CopyOf<T>(this T[] originalArray, int length)
        {
            var newArray = new T[length];
            Array.Copy(originalArray, newArray, length);
            return newArray;
        }

        public static T[] CopyOf<T>(this T[] originalArray, int index, int length)
        {
            var newArray = new T[length];
            Array.Copy(originalArray, index, newArray, 0, length);
            return newArray;
        }

        public static T[] CopyOfRange<T>(this T[] originalArray, int from, int to)
        {
            int newLength = to - from;
            if (newLength < 0)
                throw new IndexOutOfRangeException(from + " > " + to);
            T[] copy = new T[newLength];
            Array.Copy(originalArray, from, copy, 0, Math.Min(originalArray.Length - from, newLength));
            return copy;
        }

        public static void CopyRow<T>(this T[,] sourceArray, int srcRow, int srcPos, ref T[,] dest, int destRow, int destPos, int length)
        {
            if (sourceArray.GetLength(1) < srcPos)
                throw new IndexOutOfRangeException();

            if (sourceArray.GetLength(1) < (srcPos + length))
                throw new IndexOutOfRangeException();

            if (dest.GetLength(1) < destPos)
                throw new IndexOutOfRangeException();

            if (dest.GetLength(1) < (destPos + length))
                throw new IndexOutOfRangeException();

            for (int i = 0; i < length; i++)
            {
                dest[destRow, i + destPos] = sourceArray[srcRow, i + srcPos];
            }
        }

        public static Complex[] ConvertToComplex(this double[] real)
        {
            Complex[] c = new Complex[real.Length];
            for (int i = 0; i < real.Length; i++)
            {
                c[i] = new Complex(real[i], 0);
            }

            return c;
        }

        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }

        public static void Fill<T>(this T[] originalArray, int idx, int to, T with)
        {
            if ((idx > originalArray.Length) || (to > originalArray.Length))
                throw new IndexOutOfRangeException();

            for (int i = idx; i < to; i++)
            {
                originalArray[i] = with;
            }
        }

        public static void Fill<T>(this T[,] originalArray, int Index, T with)
        {
            for (int i = 0; i < originalArray.GetLength(1); i++)
            {
                originalArray[Index, i] = with;
            }
        }

        public static void Fill<T>(this T[,] originalArray, T with)
        {
            for (int i = 0; i < originalArray.GetLength(0); i++)
            {
                for (int j = 0; j < originalArray.GetLength(1); j++)
                {
                    originalArray[i, j] = with;
                }
            }
        }

        public static void LoadRow<T>(this T[,] originalArray, int Index, T[] data)
        {
            if (originalArray.GetLength(1) != data.Length)
                throw new IndexOutOfRangeException();

            for (int i = 0; i < originalArray.GetLength(1); i++)
                originalArray[Index, i] = data[i];
        }

        public static T[] GetRow<T>(this T[,] originalArray, int Index)
        {
            T[] data = new T[originalArray.GetLength(1)];

            for (int i = 0; i < originalArray.GetLength(1); i++)
                data[i] = originalArray[Index, i];

            return data;
        }

        public static void SetRow<T>(this T[,] originalArray, int Index, T[] value)
        {
            if (originalArray.GetLength(1) == value.Length)
            {
                for (int i = 0; i < originalArray.GetLength(1); i++)
                {
                    originalArray[Index, i] = value[i];
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }

        public static T[,] GetRow<T>(this T[,,] originalArray, int Index)
        {
            T[,] data = new T[originalArray.GetLength(1), originalArray.GetLength(2)];

            for (int i = 0; i < originalArray.GetLength(1); i++)
            {
                for (int j = 0; j < originalArray.GetLength(1); j++)
                {
                    data[i, j] = originalArray[Index, i, j];
                }
            }

            return data;
        }

        public static void SetRow<T>(this T[,,] originalArray, int Index, T[,] value)
        {
            if (originalArray.GetLength(2) == value.GetLength(1) && originalArray.GetLength(3) == value.GetLength(2))
            {
                for (int i = 0; i < originalArray.GetLength(2); i++)
                {
                    for (int j = 0; j < originalArray.GetLength(3); j++)
                    {
                        originalArray[Index, i, j] = value[i, j];
                    }
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }


        public static void SetRow<T>(this T[,,] originalArray, int Index1,  int Index2, T[] value)
        {
            if (originalArray.GetLength(3) == value.Length)
            {
                for (int i = 0; i < originalArray.GetLength(3); i++)
                {
                    originalArray[Index1, Index2, i] = value[i];
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }


        /// <summary>
        ///   Gets the number of rows in a multidimensional matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of rows must be computed.</param>
        /// 
        /// <returns>The number of rows in the matrix.</returns>
        /// 
        public static int Rows<T>(this T[,] matrix)
        {
            return matrix.GetLength(0);
        }

        /// <summary>
        ///   Gets the number of columns in a multidimensional matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of columns must be computed.</param>
        /// 
        /// <returns>The number of columns in the matrix.</returns>
        /// 
        public static int Columns<T>(this T[,] matrix)
        {
            return matrix.GetLength(1);
        }

        /// <summary>
        ///   Gets the number of rows in a jagged matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of rows must be computed.</param>
        /// 
        /// <returns>The number of rows in the matrix.</returns>
        /// 
        public static int Rows<T>(this T[][] matrix)
        {
            return matrix.Length;
        }

        /// <summary>
        ///   Gets the number of columns in a jagged matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements in the matrix.</typeparam>
        /// <param name="matrix">The matrix whose number of columns must be computed.</param>
        /// 
        /// <returns>The number of columns in the matrix.</returns>
        /// 
        public static int Columns<T>(this T[][] matrix)
        {
            if (matrix.Length == 0)
                return 0;
            return matrix[0].Length;
        }

        /// <summary>
        ///   Converts a matrix to upper triangular form, if possible.
        /// </summary>
        /// 
        public static T[,] ToUpperTriangular<T>(this T[,] matrix, MatrixType from, T[,] result = null)
        {
            if (result == null)
                result = MatrixUtility.CreateAs(matrix);
            matrix.CopyTo(result);

            switch (from)
            {
                case MatrixType.UpperTriangular:
                case MatrixType.Diagonal:
                    break;

                case MatrixType.LowerTriangular:
                    Transpose(result, inPlace: true);
                    break;

                default:
                    throw new ArgumentException(LocalizedResources.Instance().Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime, "matrixType");
            }

            return result;
        }

        public static IEnumerator<T> ToEnumerator<T>(this T[] baseArray)
        {
            return baseArray.AsEnumerable().GetEnumerator();
        }

        public static String[] ToStringArray<T>(this T[] baseArray)
        {
            return baseArray.OfType<object>().Select(o => o.ToString()).ToArray();
        }

        ///// <summary>
        /////   Converts a matrix to lower triangular form, if possible.
        ///// </summary>
        ///// 
        //public static T[][] ToLowerTriangular<T>(this T[][] matrix, MatrixType from, T[][] result = null)
        //{
        //    if (result == null)
        //        result = Jagged.CreateAs(matrix);
        //    matrix.CopyTo(result);

        //    switch (from)
        //    {
        //        case MatrixType.LowerTriangular:
        //        case MatrixType.Diagonal:
        //            break;

        //        case MatrixType.UpperTriangular:
        //            Transpose(result, inPlace: true);
        //            break;

        //        default:
        //            throw new ArgumentException(LocalizedResources.Instance().ARRAY_ONLY_SUPPORTED_THIS_TIME, "matrixType");
        //    }

        //    return result;
        //}

        ///// <summary>
        /////   Converts a matrix to upper triangular form, if possible.
        ///// </summary>
        ///// 
        //public static T[][] ToUpperTriangular<T>(this T[][] matrix, MatrixType from, T[][] result = null)
        //{
        //    if (result == null)
        //        result = Jagged.CreateAs(matrix);
        //    matrix.CopyTo(result);

        //    switch (from)
        //    {
        //        case MatrixType.UpperTriangular:
        //        case MatrixType.Diagonal:
        //            break;

        //        case MatrixType.LowerTriangular:
        //            Transpose(result, inPlace: true);
        //            break;

        //        default:
        //            throw new ArgumentException(LocalizedResources.Instance().ARRAY_ONLY_SUPPORTED_THIS_TIME, "matrixType");
        //    }

        //    return result;
        //}

        /// <summary>
        ///   Gets the lower triangular part of a matrix.
        /// </summary>
        /// 
        public static T[,] GetLowerTriangle<T>(this T[,] matrix, bool includeDiagonal = true)
        {
            int s = includeDiagonal ? 1 : 0;
            var r = MatrixUtility.CreateAs(matrix);
            for (int i = 0; i < matrix.Rows(); i++)
                for (int j = 0; j < i + s; j++)
                    r[i, j] = matrix[i, j]; ;
            return r;
        }

        /// <summary>
        ///   Gets the upper triangular part of a matrix.
        /// </summary>
        /// 
        public static T[,] GetUpperTriangle<T>(this T[,] matrix, bool includeDiagonal = false)
        {
            int s = includeDiagonal ? 0 : 1;
            var r = MatrixUtility.CreateAs(matrix);
            for (int i = 0; i < matrix.Rows(); i++)
                for (int j = i + s; j < matrix.Columns(); j++)
                    r[i, j] = matrix[i, j]; ;
            return r;
        }

        /// <summary>
        ///   Transforms a triangular matrix in a symmetric matrix by copying
        ///   its elements to the other, unfilled part of the matrix.
        /// </summary>
        /// 
        public static T[,] GetSymmetric<T>(this T[,] matrix, MatrixType type, T[,] result = null)
        {
            if (result == null)
                result = MatrixUtility.CreateAs(matrix);

            switch (type)
            {
                case MatrixType.LowerTriangular:
                    for (int i = 0; i < matrix.Rows(); i++)
                        for (int j = 0; j <= i; j++)
                            result[i, j] = result[j, i] = matrix[i, j];
                    break;
                case MatrixType.UpperTriangular:
                    for (int i = 0; i < matrix.Rows(); i++)
                        for (int j = i; j <= matrix.Columns(); j++)
                            result[i, j] = result[j, i] = matrix[i, j];
                    break;
                default:
                    throw new System.Exception(LocalizedResources.Instance().Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace);
            }

            return result;
        }

        /// <summary>
        ///   Creates a memberwise copy of a multidimensional matrix. Matrix elements
        ///   themselves are copied only in a shallowed manner (i.e. not cloned).
        /// </summary>
        /// 
        public static T[,] MemberwiseClone<T>(this T[,] a)
        {
            // TODO: Rename to Copy and implement shallow and deep copies
            return (T[,])a.Clone();
        }

        /// <summary>
        ///   Creates a memberwise copy of a vector matrix. Vector elements
        ///   themselves are copied only in a shallow manner (i.e. not cloned).
        /// </summary>
        /// 
        public static T[] MemberwiseClone<T>(this T[] a)
        {
            // TODO: Rename to Copy and implement shallow and deep copies
            return (T[])a.Clone();
        }

        public static int[] Natural(this int[] a, int n)
        {
            return Sequence(a, n, 0, 1);
        }

        public static int[] Sequence(this int[] a, int size, int start, int stride)
        {
            a = new int[size];
            for (int i = 0; i < size; i++)
            {
                a[i] = start + i * stride;
            }
            return a;
        }

        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        /// 
        public static T[,] Reshape<T>(this T[] array, int rows, int cols, MatrixOrder order = MatrixOrder.Default)
        {
            return Reshape(array, rows, cols, new T[rows, cols], order);
        }

        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        /// 
        public static T[,] Reshape<T>(this T[] array, int rows, int cols, T[,] result, MatrixOrder order = MatrixOrder.Default)
        {
            if (order == MatrixOrder.CRowMajor)
            {
                int k = 0;
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        result[i, j] = array[k++];
            }
            else
            {
                int k = 0;
                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        result[i, j] = array[k++];
            }

            return result;
        }

        /// <summary>
        ///   Combines a vector and a element horizontally.
        /// </summary>
        /// 
        public static T[] Concatenate<T>(this T element, T[] vector)
        {
            T[] r = new T[vector.Length + 1];

            r[0] = element;

            for (int i = 0; i < vector.Length; i++)
                r[i + 1] = vector[i];

            return r;
        }

        /// <summary>
        ///   Determines whether an array is a jagged array 
        ///   (containing inner arrays as its elements).
        /// </summary>
        /// 
        public static bool IsJagged(this Array array)
        {
            if (array.Length == 0)
                return array.Rank == 1;
            return array.GetType().GetElementType().IsArray;
        }

        /// <summary>
        ///   Gets the length of each dimension of an array.
        /// </summary>
        /// 
        /// <param name="array">The array.</param>
        /// <param name="deep">Pass true to retrieve all dimensions of the array,
        ///   even if it contains nested arrays (as in jagged matrices)</param>
        /// <param name="max">Gets the maximum length possible for each dimension (in case
        ///   the jagged matrices has different lengths).</param>
        /// 
        public static int[] GetLength(this Array array, bool deep = true, bool max = false)
        {
            if (array == null)
                return new int[] { -1 };
            if (array.Rank == 0)
                return new int[0];

            if (deep && IsJagged(array))
            {
                if (array.Length == 0)
                    return new int[0];

                int[] rest;
                if (!max)
                {
                    rest = GetLength(array.GetValue(0) as Array, deep);
                }
                else
                {
                    // find the max
                    rest = GetLength(array.GetValue(0) as Array, deep);
                    for (int i = 1; i < array.Length; i++)
                    {
                        int[] r = GetLength(array.GetValue(i) as Array, deep);

                        for (int j = 0; j < r.Length; j++)
                        {
                            if (r[j] > rest[j])
                                rest[j] = r[j];
                        }
                    }
                }

                return array.Length.Concatenate(rest);
            }

            int[] vector = new int[array.Rank];
            for (int i = 0; i < vector.Length; i++)
                vector[i] = array.GetUpperBound(i) + 1;
            return vector;
        }

        /// <summary>
        ///   Creates a vector containing every index that can be used to
        ///   address a given <paramref name="array"/>, in order.
        /// </summary>
        /// 
        /// <param name="array">The array whose indices will be returned.</param>
        /// <param name="deep">Pass true to retrieve all dimensions of the array,
        ///   even if it contains nested arrays (as in jagged matrices).</param>
        /// <param name="max">Bases computations on the maximum length possible for 
        ///   each dimension (in case the jagged matrices has different lengths).</param>
        /// <param name="order">The direction to access the matrix. Pass 1 to read the 
        ///   matrix in row-major order. Pass 0 to read in column-major order. Default is 
        ///   1 (row-major, c-style order).</param>
        /// 
        /// <returns>
        ///   An enumerable object that can be used to iterate over all
        ///   positions of the given <paramref name="array">System.Array</paramref>.
        /// </returns>
        /// 
        /// <example>
        /// <code>
        ///   double[,] a = 
        ///   { 
        ///      { 5.3, 2.3 },
        ///      { 4.2, 9.2 }
        ///   };
        ///   
        ///   foreach (int[] idx in a.GetIndices())
        ///   {
        ///      // Get the current element
        ///      double e = (double)a.GetValue(idx);
        ///   }
        /// </code>
        /// </example>
        /// 
        /// <seealso cref="Accord.Math.Vector.GetIndices{T}(T[])"/>
        /// 
        public static IEnumerable<int[]> GetIndices(this Array array, bool deep = false, bool max = false, MatrixOrder order = MatrixOrder.Default)
        {
            return Combinatorics.Sequences(array.GetLength(deep, max), firstColumnChangesFaster: order == MatrixOrder.FortranColumnMajor);
        }

        public static Boolean IsSquare(this double[,] A, int n, int m)
        {
            return (n == m);
        }

        public static Boolean IsSquare(this double[,] A)
        {
            return (A.GetLength(0) == A.GetLength(1));
        }

        public static Boolean IsSquare(this double[][] A)
        {
            return (A.Length == A[0].Length);
        }

        public static Boolean IsSquare(this double[][] A, int n, int m)
        {
            return (n == m);
        }

        /// <summary>
        /// Determines if int array is sorted from 0 -> Max
        /// </summary>
        public static bool IsSorted(this int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1] > arr[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if int array is sorted from 0 -> Max
        /// </summary>
        public static bool IsSorted(this Double[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1] > arr[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if string array is sorted from A -> Z
        /// </summary>
        public static bool IsSorted(this string[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1].CompareTo(arr[i]) > 0) // If previous is bigger, return false
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if int array is sorted from Max -> 0
        /// </summary>
        public static bool IsSortedDescending(this int[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i] < arr[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if int array is sorted from Max -> 0
        /// </summary>
        public static bool IsSortedDescending(this double[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i] < arr[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if string array is sorted from Z -> A
        /// </summary>
        public static bool IsSortedDescending(this string[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i].CompareTo(arr[i + 1]) < 0) // If previous is smaller, return false
                {
                    return false;
                }
            }
            return true;
        }

        public static Boolean IsDistinct<T>(this T[] A)
        {
            return A.Distinct().Count() == A.Count();
        }

        public static Boolean IsDistinct<T>(this T[,] A)
        {
            var query = from T item in A
                        select item;

            return query.Distinct().Count() == query.Count();
        }

        public static Boolean IsDistinct<T>(this T[][] A)
        {
            var query = from T item in A
                        select item;

            return query.Distinct().Count() == query.Count();
        }

        public static Boolean IsDiagonal(this double[,] A)
        {
            return IsDiagonal(A, A.GetLength(0), A.GetLength(1));
        }

        public static Boolean IsDiagonal(this double[,] A, int n, int m)
        {
            Boolean flag = IsSquare(A, n, m);
            if (flag)
            {
                for (int i = 0; flag && i < n - 1; i++)
                {
                    for (int j = i + 1; flag && j < n; j++)
                    {
                        flag = (A[i, j].Equals(0) && A[j, i].Equals(0));
                    }
                }
            }
            return flag;
        }

        public static Boolean IsDiagonal(this double[][] A)
        {
            return IsDiagonal(A, A.Length, A[0].Length);
        }

        public static Boolean IsDiagonal(this double[][] A, int n, int m)
        {
            Boolean flag = IsSquare(A, n, m);
            if (flag)
            {
                for (int i = 0; flag && i < n - 1; i++)
                {
                    for (int j = i + 1; flag && j < n; j++)
                    {
                        flag = (A[i][j].Equals(0) && A[j][i].Equals(0));
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// This method computes the sum of the products
        /// <code>a<sub>i</sub> b<sub>i</sub></code> to high accuracy.
        /// It does so by using specific multiplication and addition algorithms to
        /// preserve accuracy and reduce cancellation effects.
        /// <br/>
        /// It is based on the 2005 paper
        /// <a href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita, Siegfried Md Rump,
        /// and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// 
        /// <summary>
        /// <param name="a">Factors.</param>
        /// <param name="b">Factors.</param>
        /// <returns><code>&Sigma;<sub>i</sub> a<sub>i</sub> b<sub>i</sub></code>.</returns>
        /// <exception cref="DimensionMismatchException">if arrays dimensions don't match </exception>
        public static double LinearCombination(this double[] a, double[] b)
        {
            CheckEqualLength(a, b);
            int len = a.Length;

            if (len == 1)
            {
                // Revert to scalar multiplication.
                return a[0] * b[0];
            }

            double[] prodHigh = new double[len];
            double prodLowSum = 0;

            for (int i = 0; i < len; i++)
            {
                double ai = a[i];
                double aHigh = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(ai) & ((-1L) << 27));
                double aLow = ai - aHigh;

                double bi = b[i];
                double bHigh = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(bi) & ((-1L) << 27));
                double bLow = bi - bHigh;
                prodHigh[i] = ai * bi;
                double prodLow = aLow * bLow - (((prodHigh[i] -
                                                        aHigh * bHigh) -
                                                       aLow * bHigh) -
                                                      aHigh * bLow);
                prodLowSum += prodLow;
            }


            double prodHighCur = prodHigh[0];
            double prodHighNext = prodHigh[1];
            double sHighPrev = prodHighCur + prodHighNext;
            double sPrime = sHighPrev - prodHighNext;
            double sLowSum = (prodHighNext - (sHighPrev - sPrime)) + (prodHighCur - sPrime);

            int lenMinusOne = len - 1;
            for (int i = 1; i < lenMinusOne; i++)
            {
                prodHighNext = prodHigh[i + 1];
                double sHighCur = sHighPrev + prodHighNext;
                sPrime = sHighCur - prodHighNext;
                sLowSum += (prodHighNext - (sHighCur - sPrime)) + (sHighPrev - sPrime);
                sHighPrev = sHighCur;
            }

            double result = sHighPrev + (prodLowSum + sLowSum);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = 0;
                for (int i = 0; i < len; ++i)
                {
                    result += a[i] * b[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// <p>
        /// This method computes a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> to high accuracyd It does
        /// so by using specific multiplication and addition algorithms to
        /// preserve accuracy and reduce cancellation effectsd It is based
        /// on the 2005 paper <a
        /// href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita,
        /// Siegfried Md Rump, and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// </p>
        /// <summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <returns>a<sub>1</sub>&times;b<sub>1</sub> +</returns>
        /// a<sub>2</sub>&times;b<sub>2</sub>
        /// <see cref="#linearCombination(double,">double, double, double, double, double) </see>
        /// <see cref="#linearCombination(double,">double, double, double, double, double, double, double) </see>
        public static double LinearCombination(this double a1, double b1,
                                               double a2, double b2)
        {

            // the code below is split in many additions/subtractions that may
            // appear redundantd However, they should NOT be simplified, as they
            // use IEEE754 floating point arithmetic rounding properties.
            // The variable naming conventions are that xyzHigh contains the most significant
            // bits of xyz and xyzLow contains its least significant bitsd So theoretically
            // xyz is the sum xyzHigh + xyzLow, but in many cases below, this sum cannot
            // be represented in only one double precision number so we preserve two numbers
            // to hold it as long as we can, combining the high and low order bits together
            // only at the end, after cancellation may have occurred on high order bits

            // split a1 and b1 as one 26 bits number and one 27 bits number

            double a1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a1) & ((-1L) << 27));
            double a1Low = a1 - a1High;
            double b1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b1) & ((-1L) << 27));
            double b1Low = b1 - b1High;

            // accurate multiplication a1 * b1
            double prod1High = a1 * b1;
            double prod1Low = a1Low * b1Low - (((prod1High - a1High * b1High) - a1Low * b1High) - a1High * b1Low);

            // split a2 and b2 as one 26 bits number and one 27 bits number
            double a2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a2) & ((-1L) << 27));
            double a2Low = a2 - a2High;
            double b2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b2) & ((-1L) << 27));
            double b2Low = b2 - b2High;

            // accurate multiplication a2 * b2
            double prod2High = a2 * b2;
            double prod2Low = a2Low * b2Low - (((prod2High - a2High * b2High) - a2Low * b2High) - a2High * b2Low);

            // accurate addition a1 * b1 + a2 * b2
            double s12High = prod1High + prod2High;
            double s12Prime = s12High - prod2High;
            double s12Low = (prod2High - (s12High - s12Prime)) + (prod1High - s12Prime);

            // rounding, s12 may have suffered many cancellations, we try
            // to recover some bits from the extra words we have saved up to now
            double result = s12High + (prod1Low + prod2Low + s12Low);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = a1 * b1 + a2 * b2;
            }

            return result;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// <p>
        /// This method computes a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub>
        /// to high accuracyd It does so by using specific multiplication and
        /// addition algorithms to preserve accuracy and reduce cancellation effects.
        /// It is based on the 2005 paper <a
        /// href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita,
        /// Siegfried Md Rump, and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// </p>
        /// <summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <returns>a<sub>1</sub>&times;b<sub>1</sub> +</returns>
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub>
        /// <see cref="#linearCombination(double,">double, double, double) </see>
        /// <see cref="#linearCombination(double,">double, double, double, double, double, double, double) </see>
        public static double LinearCombination(this double a1, double b1,
                                               double a2, double b2,
                                               double a3, double b3)
        {

            // the code below is split in many additions/subtractions that may
            // appear redundantd However, they should NOT be simplified, as they
            // do use IEEE754 floating point arithmetic rounding properties.
            // The variables naming conventions are that xyzHigh contains the most significant
            // bits of xyz and xyzLow contains its least significant bitsd So theoretically
            // xyz is the sum xyzHigh + xyzLow, but in many cases below, this sum cannot
            // be represented in only one double precision number so we preserve two numbers
            // to hold it as long as we can, combining the high and low order bits together
            // only at the end, after cancellation may have occurred on high order bits

            // split a1 and b1 as one 26 bits number and one 27 bits number
            double a1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a1) & ((-1L) << 27));
            double a1Low = a1 - a1High;
            double b1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b1) & ((-1L) << 27));
            double b1Low = b1 - b1High;

            // accurate multiplication a1 * b1
            double prod1High = a1 * b1;
            double prod1Low = a1Low * b1Low - (((prod1High - a1High * b1High) - a1Low * b1High) - a1High * b1Low);

            // split a2 and b2 as one 26 bits number and one 27 bits number
            double a2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a2) & ((-1L) << 27));
            double a2Low = a2 - a2High;
            double b2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b2) & ((-1L) << 27));
            double b2Low = b2 - b2High;

            // accurate multiplication a2 * b2
            double prod2High = a2 * b2;
            double prod2Low = a2Low * b2Low - (((prod2High - a2High * b2High) - a2Low * b2High) - a2High * b2Low);

            // split a3 and b3 as one 26 bits number and one 27 bits number
            double a3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a3) & ((-1L) << 27));
            double a3Low = a3 - a3High;
            double b3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b3) & ((-1L) << 27));
            double b3Low = b3 - b3High;

            // accurate multiplication a3 * b3
            double prod3High = a3 * b3;
            double prod3Low = a3Low * b3Low - (((prod3High - a3High * b3High) - a3Low * b3High) - a3High * b3Low);

            // accurate addition a1 * b1 + a2 * b2
            double s12High = prod1High + prod2High;
            double s12Prime = s12High - prod2High;
            double s12Low = (prod2High - (s12High - s12Prime)) + (prod1High - s12Prime);

            // accurate addition a1 * b1 + a2 * b2 + a3 * b3
            double s123High = s12High + prod3High;
            double s123Prime = s123High - prod3High;
            double s123Low = (prod3High - (s123High - s123Prime)) + (s12High - s123Prime);

            // rounding, s123 may have suffered many cancellations, we try
            // to recover some bits from the extra words we have saved up to now
            double result = s123High + (prod1Low + prod2Low + prod3Low + s12Low + s123Low);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = a1 * b1 + a2 * b2 + a3 * b3;
            }

            return result;
        }

        /// <summary>
        /// Compute a linear combination accurately.
        /// <p>
        /// This method computes a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub> +
        /// a<sub>4</sub>&times;b<sub>4</sub>
        /// to high accuracyd It does so by using specific multiplication and
        /// addition algorithms to preserve accuracy and reduce cancellation effects.
        /// It is based on the 2005 paper <a
        /// href="http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.2.1547">
        /// Accurate Sum and Dot Product</a> by Takeshi Ogita,
        /// Siegfried Md Rump, and Shin'ichi Oishi published in SIAM Jd Scid Comput.
        /// </p>
        /// <summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <param name="a4">first factor of the third term</param>
        /// <param name="b4">second factor of the third term</param>
        /// <returns>a<sub>1</sub>&times;b<sub>1</sub> +</returns>
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub> +
        /// a<sub>4</sub>&times;b<sub>4</sub>
        /// <see cref="#linearCombination(double,">double, double, double) </see>
        /// <see cref="#linearCombination(double,">double, double, double, double, double) </see>
        public static double LinearCombination(this double a1, double b1,
                                               double a2, double b2,
                                               double a3, double b3,
                                               double a4, double b4)
        {

            // the code below is split in many additions/subtractions that may
            // appear redundantd However, they should NOT be simplified, as they
            // do use IEEE754 floating point arithmetic rounding properties.
            // The variables naming conventions are that xyzHigh contains the most significant
            // bits of xyz and xyzLow contains its least significant bitsd So theoretically
            // xyz is the sum xyzHigh + xyzLow, but in many cases below, this sum cannot
            // be represented in only one double precision number so we preserve two numbers
            // to hold it as long as we can, combining the high and low order bits together
            // only at the end, after cancellation may have occurred on high order bits

            // split a1 and b1 as one 26 bits number and one 27 bits number
            double a1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a1) & ((-1L) << 27));
            double a1Low = a1 - a1High;
            double b1High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b1) & ((-1L) << 27));
            double b1Low = b1 - b1High;

            // accurate multiplication a1 * b1
            double prod1High = a1 * b1;
            double prod1Low = a1Low * b1Low - (((prod1High - a1High * b1High) - a1Low * b1High) - a1High * b1Low);

            // split a2 and b2 as one 26 bits number and one 27 bits number
            double a2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a2) & ((-1L) << 27));
            double a2Low = a2 - a2High;
            double b2High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b2) & ((-1L) << 27));
            double b2Low = b2 - b2High;

            // accurate multiplication a2 * b2
            double prod2High = a2 * b2;
            double prod2Low = a2Low * b2Low - (((prod2High - a2High * b2High) - a2Low * b2High) - a2High * b2Low);

            // split a3 and b3 as one 26 bits number and one 27 bits number
            double a3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a3) & ((-1L) << 27));
            double a3Low = a3 - a3High;
            double b3High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b3) & ((-1L) << 27));
            double b3Low = b3 - b3High;

            // accurate multiplication a3 * b3
            double prod3High = a3 * b3;
            double prod3Low = a3Low * b3Low - (((prod3High - a3High * b3High) - a3Low * b3High) - a3High * b3Low);

            // split a4 and b4 as one 26 bits number and one 27 bits number
            double a4High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(a4) & ((-1L) << 27));
            double a4Low = a4 - a4High;
            double b4High = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(b4) & ((-1L) << 27));
            double b4Low = b4 - b4High;

            // accurate multiplication a4 * b4
            double prod4High = a4 * b4;
            double prod4Low = a4Low * b4Low - (((prod4High - a4High * b4High) - a4Low * b4High) - a4High * b4Low);

            // accurate addition a1 * b1 + a2 * b2
            double s12High = prod1High + prod2High;
            double s12Prime = s12High - prod2High;
            double s12Low = (prod2High - (s12High - s12Prime)) + (prod1High - s12Prime);

            // accurate addition a1 * b1 + a2 * b2 + a3 * b3
            double s123High = s12High + prod3High;
            double s123Prime = s123High - prod3High;
            double s123Low = (prod3High - (s123High - s123Prime)) + (s12High - s123Prime);

            // accurate addition a1 * b1 + a2 * b2 + a3 * b3 + a4 * b4
            double s1234High = s123High + prod4High;
            double s1234Prime = s1234High - prod4High;
            double s1234Low = (prod4High - (s1234High - s1234Prime)) + (s123High - s1234Prime);

            // rounding, s1234 may have suffered many cancellations, we try
            // to recover some bits from the extra words we have saved up to now
            double result = s1234High + (prod1Low + prod2Low + prod3Low + prod4Low + s12Low + s123Low + s1234Low);

            if (Double.IsNaN(result))
            {
                // either we have split infinite numbers or some coefficients were NaNs,
                // just rely on the naive implementation and let IEEE754 handle this
                result = a1 * b1 + a2 * b2 + a3 * b3 + a4 * b4;
            }

            return result;
        }


        #region Transpose

        /// <summary>
        ///   Gets the transpose of a matrix.
        /// </summary>
        /// 
        /// <param name="matrix">A matrix.</param>
        /// 
        /// <returns>The transpose of the given matrix.</returns>
        /// 
        public static T[,] Transpose<T>(this T[,] matrix)
        {
            return Transpose(matrix, false);
        }

        /// <summary>
        ///   Gets the transpose of a matrix.
        /// </summary>
        /// 
        /// <param name="matrix">A matrix.</param>
        /// 
        /// <param name="inPlace">True to store the transpose over the same input
        ///   <paramref name="matrix"/>, false otherwise. Default is false.</param>
        ///   
        /// <returns>The transpose of the given matrix.</returns>
        /// 
        public static T[,] Transpose<T>(this T[,] matrix, bool inPlace)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (inPlace)
            {
                if (rows != cols)
                    throw new ArgumentException(LocalizedResources.Instance().Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace, "matrix");

                for (int i = 0; i < rows; i++)
                {
                    for (int j = i; j < cols; j++)
                    {
                        T element = matrix[j, i];
                        matrix[j, i] = matrix[i, j];
                        matrix[i, j] = element;
                    }
                }

                return matrix;
            }
            else
            {
                T[,] result = new T[cols, rows];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        result[j, i] = matrix[i, j];

                return result;
            }
        }



        /// <summary>
        ///   Gets the transpose of a row vector.
        /// </summary>
        /// 
        /// <param name="rowVector">A row vector.</param>
        /// 
        /// <returns>The transpose of the given vector.</returns>
        /// 
        public static T[,] Transpose<T>(this T[] rowVector)
        {
            var result = new T[rowVector.Length, 1];
            for (int i = 0; i < rowVector.Length; i++)
                result[i, 0] = rowVector[i];
            return result;
        }

        /// <summary>
        ///   Gets the transpose of a row vector.
        /// </summary>
        /// 
        /// <param name="rowVector">A row vector.</param>
        /// <param name="result">The matrix where to store the transpose.</param>
        /// 
        /// <returns>The transpose of the given vector.</returns>
        /// 
        public static T[,] Transpose<T>(this T[] rowVector, T[,] result)
        {
            for (int i = 0; i < rowVector.Length; i++)
                result[i, 0] = rowVector[i];
            return result;
        }


        /// <summary>
        ///   Gets the generalized transpose of a tensor.
        /// </summary>
        /// 
        /// <param name="array">A tensor.</param>
        /// 
        /// <returns>The transpose of the given tensor.</returns>
        /// 
        //public static Array Transpose(this Array array)
        //{
        //    return Transpose(array, Accord.Math.Vector.Range(array.Rank - 1, -1));
        //}

        /// <summary>
        ///   Gets the generalized transpose of a tensor.
        /// </summary>
        /// 
        /// <param name="array">A tensor.</param>
        /// <param name="order">The new order for the tensor's dimensions.</param>
        /// 
        /// <returns>The transpose of the given tensor.</returns>
        /// 
        public static Array Transpose(this Array array, int[] order)
        {
            if (order.Length != array.Rank)
                throw new ArgumentException(LocalizedResources.Instance().ARRAY_ORDER);

            if (array.Length == 1 || array.Length == 0)
                return array;

            // Get the number of samples at each dimension
            int[] size = new int[array.Rank];
            for (int i = 0; i < size.Length; i++)
                size[i] = array.GetLength(i);

            Array r = Array.CreateInstance(array.GetType().GetElementType(), size.Get(order));

            // Generate all indices for accessing the matrix 
            foreach (int[] pos in Combinatorics.Sequences(size, inPlace: true))
            {
                int[] newPos = pos.Get(order);
                object value = array.GetValue(pos);
                r.SetValue(value, newPos);
            }

            return r;
        }

        /// <summary>
        ///   Gets the generalized transpose of a tensor.
        /// </summary>
        /// 
        /// <param name="array">A tensor.</param>
        /// <param name="order">The new order for the tensor's dimensions.</param>
        /// 
        /// <returns>The transpose of the given tensor.</returns>
        /// 
        public static T Transpose<T>(this T array, int[] order)
            where T : class, IList
        {
            Array arr = array as Array;

            if (arr == null)
                throw new ArgumentException(LocalizedResources.Instance().Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray, "array");

            return Transpose(arr, order) as T;
        }

        #endregion
    }
}
