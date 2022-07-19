// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by Apache Software Foundation (ASF).
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreementsd  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the Licensed  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
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
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Matrix
{
    /// <summary>
    /// A collection of static methods that operate on or return matrices.
    /// </summary>
    public static class MatrixUtility
    {

        /// <summary>
        /// Returns a {@link Matrix<Double>} with specified dimensions.
        /// <p>The type of matrix returned depends on the dimensiond Below
        /// 2<sup>12</sup> elements (i.ed 4096 elements or 64&times;64 for a
        /// square matrix) which can be stored in a 32kB array, a {@link
        /// Matrix<Double>} instance is builtd Above this threshold a {@link
        /// BlockMatrix<Double>} instance is built.</p>
        /// <p>The matrix elements are all set to 0.0.</p>
        /// </summary>
        /// <param Name="rows">number of rows of the matrix</param>
        /// <param Name="columns">number of columns of the matrix</param>
        /// <returns> Matrix<Double> with specified dimensions</returns>
        /// <see cref="CreateMatrix(double[][])"></see>
        public static Matrix<Double> CreateMatrix(int rows, int columns)
        {
            return Matrix<Double>.Build.Dense(rows, columns).Clone();
        }

        /// <summary>
        /// Returns a {@link Matrix<Double>} with specified dimensions.
        /// <p>The type of matrix returned depends on the dimensiond Below
        /// 2<sup>12</sup> elements (i.ed 4096 elements or 64&times;64 for a
        /// square matrix) which can be stored in a 32kB array, a {@link
        /// Matrix<Double>} instance is builtd Above this threshold a {@link
        /// BlockMatrix<Double>} instance is built.</p>
        /// <p>The matrix elements are all set to 0.0.</p>
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param Name="rows">number of rows of the matrix</param>
        /// <param Name="columns">number of columns of the matrix</param>
        /// <returns> Matrix<Double> with specified dimensions</returns>
        /// <see cref="CreateMatrix(double[][])"></see>
        /// <returns></returns>
        public static Matrix<T> CreateMatrix<T>(int rows, int columns) where T : struct, IEquatable<T>, IFormattable
        {
            return Matrix<T>.Build.Dense(rows, columns).Clone();
        }


        /// <summary>
        /// Returns a {@link Matrix<Double>} whose entries are the the values in the
        /// the input array.
        /// <p>The type of matrix returned depends on the dimensiond Below
        /// 2<sup>12</sup> elements (i.ed 4096 elements or 64&times;64 for a
        /// square matrix) which can be stored in a 32kB array, a {@link
        /// Matrix<Double>} instance is builtd Above this threshold a {@link
        /// BlockMatrix<Double>} instance is built.</p>
        /// <p>The input array is copied, not referenced.</p>
        /// 
        /// </summary>
        /// <param Name="data">input array</param>
        /// <returns> Matrix<Double> containing the values of the array</returns>
        /// <exception cref="ArgumentException">if <code>data</code> is not rectangular </exception>
        ///  (not all rows have the same Length) or empty
        /// <exception cref="NullReferenceException">if either <code>data</code> or </exception>
        /// <code>data[0]</code> is null
        /// <see cref="#CreateMatrix(int,">int) </see>
        public static Matrix<Double> CreateMatrix(double[][] data)
        {
            DenseColumnMajorMatrixStorage<double> storage = DenseColumnMajorMatrixStorage<double>.OfArray(data.ToMultidimensional());
            return Matrix<Double>.Build.Dense(storage).Clone();
        }

        /// <summary>
        /// Returns a {@link Matrix<Double>} whose entries are the the values in the
        /// the input array.
        /// <p>The type of matrix returned depends on the dimensiond Below
        /// 2<sup>12</sup> elements (i.ed 4096 elements or 64&times;64 for a
        /// square matrix) which can be stored in a 32kB array, a {@link
        /// Matrix<Double>} instance is builtd Above this threshold a {@link
        /// BlockMatrix<Double>} instance is built.</p>
        /// <p>The input array is copied, not referenced.</p>
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param Name="data">input array</param>
        /// <returns> Matrix<Double> containing the values of the array</returns>
        /// <exception cref="ArgumentException">if <code>data</code> is not rectangular </exception>
        ///  (not all rows have the same Length) or empty
        /// <exception cref="NullReferenceException">if either <code>data</code> or </exception>
        /// <code>data[0]</code> is null
        /// <see cref="#CreateMatrix(int,">int) </see>
        public static Matrix<T> CreateMatrix<T>(T[][] data) where T : struct, IEquatable<T>, IFormattable
        {
            DenseColumnMajorMatrixStorage<T> storage = DenseColumnMajorMatrixStorage<T>.OfArray(data.ToMultidimensional());
            return Matrix<T>.Build.Dense(storage).Clone();
        }


        /// <summary>
        /// Returns a {@link Matrix<Double>} whose entries are the the values in the
        /// the input array.
        /// <p>The type of matrix returned depends on the dimensiond Below
        /// 2<sup>12</sup> elements (i.ed 4096 elements or 64&times;64 for a
        /// square matrix) which can be stored in a 32kB array, a {@link
        /// Matrix<Double>} instance is builtd Above this threshold a {@link
        /// BlockMatrix<Double>} instance is built.</p>
        /// <p>The input array is copied, not referenced.</p>
        /// 
        /// </summary>
        /// <param Name="data">input array</param>
        /// <returns> Matrix<Double> containing the values of the array</returns>
        /// <exception cref="ArgumentException">if <code>data</code> is not rectangular </exception>
        ///  (not all rows have the same Length) or empty
        /// <exception cref="NullReferenceException">if either <code>data</code> or </exception>
        /// <code>data[0]</code> is null
        /// <see cref="#createMatrix(int,">int) </see>
        public static Matrix<Double> CreateMatrix(double[,] data)
        {
            DenseColumnMajorMatrixStorage<double> storage = DenseColumnMajorMatrixStorage<double>.OfArray(data);
            return Matrix<Double>.Build.Dense(storage).Clone();
        }

        /// <summary>
        /// Returns a {@link Matrix<Double>} whose entries are the the values in the
        /// the input array.
        /// <p>The type of matrix returned depends on the dimensiond Below
        /// 2<sup>12</sup> elements (i.ed 4096 elements or 64&times;64 for a
        /// square matrix) which can be stored in a 32kB array, a {@link
        /// Matrix<Double>} instance is builtd Above this threshold a {@link
        /// BlockMatrix<Double>} instance is built.</p>
        /// <p>The input array is copied, not referenced.</p>
        /// 
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param Name="data">input array</param>
        /// <returns> Matrix<Double> containing the values of the array</returns>
        /// <exception cref="ArgumentException">if <code>data</code> is not rectangular </exception>
        ///  (not all rows have the same Length) or empty
        /// <exception cref="NullReferenceException">if either <code>data</code> or </exception>
        /// <code>data[0]</code> is null
        /// <see cref="#createMatrix(int,">int) </see>
        public static Matrix<T> CreateMatrix<T>(T[,] data) where T : struct, IEquatable<T>, IFormattable
        {
            DenseColumnMajorMatrixStorage<T> storage = DenseColumnMajorMatrixStorage<T>.OfArray(data);
            return Matrix<T>.Build.Dense(storage).Clone();
        }

        /// <summary>
        /// Returns <code>dimension x dimension</code> identity matrix.
        /// 
        /// </summary>
        /// <param Name="dimension">dimension of identity matrix to generate</param>
        /// <returns>identity matrix</returns>
        /// <exception cref="ArgumentException">if dimension is not positive </exception>
        /// @since 1.1
        public static Matrix<Double> CreateRealIdentityMatrix(int dimension)
        {
            Matrix<Double> m = CreateMatrix(dimension, dimension);
            for (int i = 0; i < dimension; ++i)
            {
                m[i, i] = 1.0;
            }
            return m;
        }

        /// <summary>
        /// Returns a diagonal matrix with specified elements.
        /// 
        /// </summary>
        /// <param Name="diagonal">diagonal elements of the matrix (the array elements</param>
        /// will be copied)
        /// <returns>diagonal matrix</returns>
        /// @since 2.0
        public static Matrix<Double> CreateRealDiagonalMatrix(double[] diagonal)
        {
            Matrix<Double> m = CreateMatrix(diagonal.Length, diagonal.Length);
            for (int i = 0; i < diagonal.Length; ++i)
            {
                m[i, i] = diagonal[i];
            }
            return m;
        }


        /// <summary>
        /// Creates a {@link RealVector} using the data from the input array.
        /// 
        /// </summary>
        /// <param Name="data">the input data</param>
        /// <returns>a data.Length RealVector</returns>
        /// <exception cref="ArgumentException">if <code>data</code> is empty </exception>
        /// <exception cref="NullReferenceException">if <code>data</code>is null </exception>
        public static Vector<T> CreateVector<T>(T[] data) where T : struct, IEquatable<T>, IFormattable
        {
            return Vector<T>.Build.Dense(data).Clone();
        }

        /// <summary>
        /// Creates a {@link RealVector} using the data from the input array.
        /// 
        /// </summary>
        /// <param Name="data">the input data</param>
        /// <returns>a data.Length RealVector</returns>
        /// <exception cref="ArgumentException">if <code>data</code> is empty </exception>
        /// <exception cref="NullReferenceException">if <code>data</code>is null </exception>
        public static Vector<Double> CreateRealVector(double[] data)
        {
            return Vector<Double>.Build.Dense(data).Clone();
        }

        /// <summary>
        /// Creates a row {@link Matrix<Double>} using the data from the input
        /// array.
        /// 
        /// </summary>
        /// <param Name="rowData">the input row data</param>
        /// <returns>a 1 x rowData.Length Matrix<Double></returns>
        /// <exception cref="ArgumentException">if <code>rowData</code> is empty </exception>
        /// <exception cref="NullReferenceException">if <code>rowData</code>is null </exception>
        public static Matrix<Double> CreateRowMatrix(double[] rowData)
        {
            int nCols = rowData.Length;
            Matrix<Double> m = CreateMatrix(1, nCols);
            AutoParallel.AutoParallelFor(0, nCols, (i) =>
            {
                m[0, i] = rowData[i];
            });
            return m;
        }

        /// <summary>
        /// Creates a column {@link Matrix<Double>} using the data from the input
        /// array.
        /// 
        /// </summary>
        /// <param Name="columnData"> the input column data</param>
        /// <returns>a columnData x 1 Matrix<Double></returns>
        /// <exception cref="ArgumentException">if <code>columnData</code> is empty </exception>
        /// <exception cref="NullReferenceException">if <code>columnData</code>is null </exception>
        public static Matrix<Double> CreateColumnMatrix(double[] columnData)
        {
            int nRows = columnData.Length;
            Matrix<Double> m = CreateMatrix(nRows, 1);
            AutoParallel.AutoParallelFor(0, nRows, (i) =>
            {
                m[i, 0] = columnData[i];
            });
            return m;
        }

        /// <summary>
        /// Check if a row index is valid.
        /// </summary>
        /// <param Name="m">matrix containing the submatrix</param>
        /// <param Name="row">row index to check</param>
        /// <exception cref="MatrixIndexException">if index is not valid </exception>
        public static void CheckRowIndex<T>(Matrix<T> m, int row) where T : struct, IEquatable<T>, IFormattable
        {
            if (row < 0 || row >= m.RowCount)
            {
                throw new MatrixIndexException(String.Format(LocalizedResources.Instance().ROW_INDEX_OUT_OF_RANGE, row, 0, m.RowCount - 1));
            }
        }

        /// <summary>
        /// Check if a column index is valid.
        /// </summary>
        /// <param Name="m">matrix containing the submatrix</param>
        /// <param Name="column">column index to check</param>
        /// <exception cref="MatrixIndexException">if index is not valid </exception>
        public static void CheckColumnIndex<T>(Matrix<T> m, int column) where T : struct, IEquatable<T>, IFormattable
        {
            if (column < 0 || column >= m.ColumnCount)
            {
                throw new MatrixIndexException(String.Format(LocalizedResources.Instance().COLUMN_INDEX_OUT_OF_RANGE, column, 0, m.ColumnCount - 1));
            }
        }

        /// <summary>
        /// Check if submatrix ranges indices are valid.
        /// Rows and columns are indicated counting from 0 to n-1.
        /// 
        /// </summary>
        /// <param Name="m">matrix containing the submatrix</param>
        /// <param Name="startRow">Initial row index</param>
        /// <param Name="endRow">Final row index</param>
        /// <param Name="startColumn">Initial column index</param>
        /// <param Name="endColumn">Final column index</param>
        /// <exception cref="MatrixIndexException"> if the indices are not valid </exception>
        public static void CheckSubMatrixIndex<T>(Matrix<T> m, int startRow, int endRow, int startColumn, int endColumn) where T : struct, IEquatable<T>, IFormattable
        {
            CheckRowIndex(m, startRow);
            CheckRowIndex(m, endRow);
            if (startRow > endRow)
            {
                throw new MatrixIndexException(String.Format(LocalizedResources.Instance().INITIAL_ROW_AFTER_FINAL_ROW, startRow, endRow));
            }

            CheckColumnIndex(m, startColumn);
            CheckColumnIndex(m, endColumn);
            if (startColumn > endColumn)
            {
                throw new MatrixIndexException(String.Format(LocalizedResources.Instance().INITIAL_COLUMN_AFTER_FINAL_COLUMN, startColumn, endColumn));
            }
        }

        /// <summary>
        /// Check if submatrix ranges indices are valid.
        /// Rows and columns are indicated counting from 0 to n-1.
        /// 
        /// </summary>
        /// <param Name="m">matrix containing the submatrix</param>
        /// <param Name="selectedRows">Array of row indices.</param>
        /// <param Name="selectedColumns">Array of column indices.</param>
        /// <exception cref="MatrixIndexException">if row or column selections are not valid </exception>
        public static void CheckSubMatrixIndex<T>(Matrix<T> m, int[] selectedRows, int[] selectedColumns) where T : struct, IEquatable<T>, IFormattable
        {
            if (selectedRows.Length * selectedColumns.Length == 0)
            {
                if (selectedRows.Length == 0)
                {
                    throw new MatrixIndexException(LocalizedResources.Instance().EMPTY_SELECTED_ROW_INDEX_ARRAY);
                }
                throw new MatrixIndexException(LocalizedResources.Instance().EMPTY_SELECTED_COLUMN_INDEX_ARRAY);
            }

            AutoParallel.AutoParallelForEach(selectedRows, (row) =>
            {
                CheckRowIndex(m, row);
            });
            AutoParallel.AutoParallelForEach(selectedColumns, (column) =>
            {
                CheckColumnIndex(m, column);
            });
        }

        /// <summary>
        /// Check if matrices are addition compatible
        /// </summary>
        /// <param Name="left">left hand side matrix</param>
        /// <param Name="right">right hand side matrix</param>
        /// <exception cref="ArgumentException">if matrices are not addition compatible </exception>
        public static void CheckAdditionCompatible<T>(Matrix<T> left, Matrix<T> right) where T : struct, IEquatable<T>, IFormattable
        {
            if ((left.RowCount != right.RowCount) ||
                (left.ColumnCount != right.ColumnCount))
            {
                throw MathRuntimeException.CreateArithmeticException(String.Format(LocalizedResources.Instance().NOT_ADDITION_COMPATIBLE_MATRICES, left.RowCount, left.ColumnCount, right.RowCount, right.ColumnCount));
            }
        }

        /// <summary>
        /// Check if matrices are subtraction compatible
        /// </summary>
        /// <param Name="left">left hand side matrix</param>
        /// <param Name="right">right hand side matrix</param>
        /// <exception cref="ArgumentException">if matrices are not subtraction compatible </exception>
        public static void CheckSubtractionCompatible<T>(Matrix<T> left, Matrix<T> right) where T : struct, IEquatable<T>, IFormattable
        {
            if ((left.RowCount != right.RowCount) ||
                (left.ColumnCount != right.ColumnCount))
            {
                throw MathRuntimeException.CreateArithmeticException(String.Format(LocalizedResources.Instance().NOT_SUBTRACTION_COMPATIBLE_MATRICES, left.RowCount, left.ColumnCount, right.RowCount, right.ColumnCount));
            }
        }

        /// <summary>
        /// Check if matrices are multiplication compatible
        /// </summary>
        /// <param Name="left">left hand side matrix</param>
        /// <param Name="right">right hand side matrix</param>
        /// <exception cref="ArgumentException">if matrices are not multiplication compatible </exception>
        public static void CheckMultiplicationCompatible<T>(Matrix<T> left, Matrix<T> right) where T : struct, IEquatable<T>, IFormattable
        {
            if (left.ColumnCount != right.RowCount)
            {
                throw MathRuntimeException.CreateArithmeticException(String.Format(LocalizedResources.Instance().NOT_MULTIPLICATION_COMPATIBLE_MATRICES, left.RowCount, left.ColumnCount, right.RowCount, right.ColumnCount));
            }
        }

        public static DenseMatrix CreateDenseMatrix(double[][] x)
        {
            return CreateDenseMatrix(x.ToMultidimensional());
        }

        public static DenseMatrix CreateDenseMatrix(double[,] x)
        {
            DenseColumnMajorMatrixStorage<double> storage = DenseColumnMajorMatrixStorage<double>.OfArray(x);
            return new DenseMatrix(storage);
        }

        public static DenseMatrix CreateDenseDiagonalMatrix(double[] x)
        {
            int rowCount = x.Length;
            int colCount = x.Length;

            double[,] data = new double[rowCount, colCount];

            data.Fill(0);

            AutoParallel.AutoParallelFor(0, rowCount, (i) =>
            {
                data[i, i] = x[i];
            });

            return CreateDenseMatrix(data);
        }

        /// <summary>
        ///   Returns a copy of an array in reversed order.
        /// </summary>
        /// 
        public static T[] First<T>(this T[] values, int count)
        {
            var r = new T[count];
            for (int i = 0; i < r.Length; i++)
                r[i] = values[i];
            return r;
        }


        /// <summary>
        ///   Returns a subvector extracted from the current vector.
        /// </summary>
        /// 
        /// <param name="source">The vector to return the subvector from.</param>
        /// <param name="indexes">Array of indices.</param>
        /// <param name="inPlace">True to return the results in place, changing the
        ///   original <paramref name="source"/> vector; false otherwise.</param>
        /// 
        public static T[] Get<T>(this T[] source, int[] indexes, bool inPlace = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (indexes == null)
                throw new ArgumentNullException("indexes");

            if (inPlace && source.Length != indexes.Length)
                throw new ArgumentNullException(LocalizedResources.Instance().SOURCE_AND_INDEXES_ARRAYS_MUST_HAVE_THE_SAME_DIMENSION);

            var destination = new T[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                int j = indexes[i];
                if (j >= 0)
                    destination[i] = source[j];
                else
                    destination[i] = source[source.Length + j];
            }

            if (inPlace)
            {
                for (int i = 0; i < destination.Length; i++)
                    source[i] = destination[i];
            }

            return destination;
        }
        /// <summary>
        ///   Returns a sub matrix extracted from the current matrix.
        /// </summary>
        /// 
        /// <param name="source">The matrix to return the submatrix from.</param>
        /// <param name="rowIndexes">Array of row indices. Pass null to select all indices.</param>
        /// <param name="columnIndexes">Array of column indices. Pass null to select all indices.</param>
        /// <param name="result">An optional matrix where the results should be stored.</param>
        /// 
        public static T[,] Get<T>(this T[,] source, int[] rowIndexes, int[] columnIndexes, T[,] result = null)
        {
            return Get(source, result, rowIndexes, columnIndexes);
        }

        /// <summary>
        ///   Extracts a selected area from a matrix.
        /// </summary>
        /// 
        /// <remarks>
        ///   Routine adapted from Lutz Roeder's Mapack for .NET, September 2000.
        /// </remarks>
        /// 
        private static T[,] Get<T>(this T[,] source, T[,] destination, int[] rowIndexes, int[] columnIndexes)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int rows = source.Rows();
            int cols = source.GetMaxColumnLength();

            int newRows = rows;
            int newCols = cols;

            if (rowIndexes == null && columnIndexes == null)
            {
                return source;
            }

            if (rowIndexes != null)
            {
                newRows = rowIndexes.Length;
                for (int i = 0; i < rowIndexes.Length; i++)
                    if ((rowIndexes[i] < 0) || (rowIndexes[i] >= rows))
                        throw new ArgumentException(LocalizedResources.Instance().ARGUMENT_OUT_OF_RANGE);
            }
            if (columnIndexes != null)
            {
                newCols = columnIndexes.Length;
                for (int i = 0; i < columnIndexes.Length; i++)
                    if ((columnIndexes[i] < 0) || (columnIndexes[i] >= cols))
                        throw new ArgumentException(LocalizedResources.Instance().ARGUMENT_OUT_OF_RANGE);
            }


            if (destination != null)
            {
                if (destination.Rows() < newRows || destination.GetMaxColumnLength() < newCols)
                    throw new ArgumentException("destination", LocalizedResources.Instance().THE_DESTINATION_MATRIX_MUST_BE_BIG_ENOUGH);
            }
            else
            {
                destination = new T[newRows, newCols];
            }

            if (columnIndexes == null)
            {
                for (int i = 0; i < rowIndexes.Length; i++)
                    for (int j = 0; j < cols; j++)
                        destination[i, j] = source[rowIndexes[i], j];
            }
            else if (rowIndexes == null)
            {
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columnIndexes.Length; j++)
                        destination[i, j] = source[i, columnIndexes[j]];
            }
            else
            {
                for (int i = 0; i < rowIndexes.Length; i++)
                    for (int j = 0; j < columnIndexes.Length; j++)
                        destination[i, j] = source[rowIndexes[i], columnIndexes[j]];
            }

            return destination;
        }

        /// <summary>
        ///   Creates a square matrix with ones across its diagonal.
        /// </summary>
        /// 
        public static double[,] Identity(int size)
        {
            return Diagonal(size, 1.0);
        }

        /// <summary>
        ///   Creates a square matrix with ones across its diagonal.
        /// </summary>
        /// 
        public static T[,] Identity<T>(int size) where T : struct
        {
            return Diagonal(size, 1.To<T>());
        }

        public static T[] GetColumnValue<T>(this T[,] source, int col)
        {
            var ret = new T[source.Rows()];

            for (int i = 0; i < source.Rows(); i++)
            {
                ret[i] = source[i, col];
            }

            return ret;
        }

        public static T[,] CreateAs<T>(T[,] matrix)
        {
            return new T[matrix.Rows(), matrix.Rows()];
        }

        public static T[,] CreateAs<T>(T[][] matrix)
        {
            return new T[matrix.Length, matrix[0].Length];
        }

        /// <summary>
        ///   Copies the content of an array to another array.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements to be copied.</typeparam>
        /// 
        /// <param name="matrix">The source matrix to be copied.</param>
        /// <param name="destination">The matrix where the elements should be copied to.</param>
        /// <param name="transpose">Whether to transpose the matrix when copying or not. Default is false.</param>
        /// 
        public static void CopyTo<T>(this T[,] matrix, T[,] destination, bool transpose = false)
        {
            if (matrix == destination)
            {
                if (transpose)
                    matrix.Transpose(true);
            }
            else
            {
                if (transpose)
                {
                    int rows = System.Math.Min(matrix.Rows(), destination.Columns());
                    int cols = System.Math.Min(matrix.Columns(), destination.Rows());
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            destination[j, i] = matrix[i, j];
                }
                else
                {
                    if (matrix.Length == destination.Length)
                    {
                        Array.Copy(matrix, 0, destination, 0, matrix.Length);
                    }
                    else
                    {
                        int rows = System.Math.Min(matrix.Rows(), destination.Rows());
                        int cols = System.Math.Min(matrix.Columns(), destination.Columns());
                        for (int i = 0; i < rows; i++)
                            for (int j = 0; j < cols; j++)
                                destination[i, j] = matrix[i, j];
                    }
                }
            }
        }

        /// <summary>
        ///   Copies the content of an array to another array.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements to be copied.</typeparam>
        /// 
        /// <param name="matrix">The source matrix to be copied.</param>
        /// <param name="destination">The matrix where the elements should be copied to.</param>
        /// <param name="transpose">Whether to transpose the matrix when copying or not. Default is false.</param>
        /// 
        public static void CopyTo<T>(this T[,] matrix, T[][] destination, bool transpose = false)
        {
            if (transpose)
            {
                int rows = System.Math.Min(matrix.Rows(), destination.Columns());
                int cols = System.Math.Min(matrix.Columns(), destination.Rows());
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        destination[j][i] = matrix[i, j];
            }
            else
            {
                if (matrix.Length == destination.Length)
                {
                    Array.Copy(matrix, 0, destination, 0, matrix.Length);
                }
                else
                {
                    int rows = System.Math.Min(matrix.Rows(), destination.Rows());
                    int cols = System.Math.Min(matrix.Columns(), destination.Columns());
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            destination[i][j] = matrix[i, j];
                }
            }
        }


        /// <summary>
        ///   Copies the content of an array to another array.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the elements to be copied.</typeparam>
        /// 
        /// <param name="matrix">The source matrix to be copied.</param>
        /// <param name="destination">The matrix where the elements should be copied to.</param>
        /// 
        public static void CopyTo<T>(this T[,] matrix, T[][] destination)
        {
            for (int i = 0; i < destination.Length; i++)
                for (int j = 0; j < destination[i].Length; j++)
                    destination[i][j] = matrix[i, j];
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
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int size, T value)
        {
            return Diagonal(size, value, new T[size, size]);
        }

        /// <summary>
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int size, T value, T[,] result)
        {
            for (int i = 0; i < size; i++)
                result[i, i] = value;
            return result;
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int rows, int cols, T value)
        {
            return Diagonal(rows, cols, value, new T[rows, cols]);
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(int rows, int cols, T value, T[,] result)
        {
            int min = System.Math.Min(rows, cols);
            for (int i = 0; i < min; i++)
                result[i, i] = value;
            return result;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(T[] values)
        {
            return Diagonal(values, new T[values.Length, values.Length]);
        }


        /// <summary>
        ///   Gets the diagonal vector from a matrix.
        /// </summary>
        /// 
        /// <param name="matrix">A matrix.</param>
        /// 
        /// <returns>The diagonal vector from the given matrix.</returns>
        /// 
        public static T[] Diagonal<T>(this T[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            var r = new T[matrix.Rows()];
            for (int i = 0; i < r.Length; i++)
                r[i] = matrix[i, i];

            return r;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
        public static T[,] Diagonal<T>(T[] values, T[,] result)
        {
            for (int i = 0; i < values.Length; i++)
                result[i, i] = values[i];
            return result;
        }

        public static T[] Create<T>(int size, T value)
        {
            var v = new T[size];
            for (int i = 0; i < v.Length; i++)
                v[i] = value;
            return v;
        }

        public static T[][] Create<T>(int rows, int columns, params T[] values)
        {
            if (values.Length == 0)
            {
                T[][] matrix = new T[rows][];
                for (int i = 0; i < matrix.Length; i++)
                    matrix[i] = new T[columns];
                return matrix;
            }
            return values.Reshape(rows, columns).ToJagged();
        }

        /// <summary>
        ///   Creates a matrix with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// <param name="values">The initial values for the matrix.</param>
        /// <param name="transpose">Whether to transpose the matrix when copying or not. Default is false.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        public static T[,] Create<T>(int rows, int columns, T[,] values, bool transpose = false)
        {
            var result = new T[rows, columns];

            MatrixUtility.CopyTo(values, destination: result, transpose: transpose);
            return result;
        }


        /// <summary>
        ///   Creates a tensor with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="shape">The number of dimensions that the matrix should have.</param>
        /// <param name="value">The initial values for the vector.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        public static Array Create<T>(int[] shape, T value)
        {
            return Create(typeof(T), shape, value);
        }

        /// <summary>
        ///   Creates a tensor with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="elementType">The type of the elements to be contained in the matrix.</param>
        /// <param name="shape">The number of dimensions that the matrix should have.</param>
        /// <param name="value">The initial values for the vector.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        public static Array Create(Type elementType, int[] shape, object value)
        {
            Array arr = Array.CreateInstance(elementType, shape);
            foreach (int[] idx in arr.GetIndices())
                arr.SetValue(value, idx);
            return arr;
        }

        /// <summary>
        ///   Computes the product <c>R = A*B</c> of two matrices <c>A</c>
        ///   and <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static int[][] Dot(this int[][] a, int[][] b, int[][] result)
        {
            int N = result.Length;
            int K = a.Columns();
            int M = result.Columns();

            var t = new int[K];

            for (int j = 0; j < M; j++)
            {
                for (int k = 0; k < b.Length; k++)
                    t[k] = b[k][j];

                for (int i = 0; i < a.Length; i++)
                {
                    int s = (int)0;
                    for (int k = 0; k < t.Length; k++)
                        s += (int)((int)a[i][k] * (int)t[k]);
                    result[i][j] = (int)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Multiplies a matrix <c>A</c> and a column vector <c>v</c>,
        ///   giving the product <c>A*v</c>
        /// </summary>
        /// 
        /// <param name="matrix">The matrix <c>A</c>.</param>
        /// <param name="columnVector">The column vector <c>v</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static byte[] Dot(this byte[][] matrix, byte[] columnVector, byte[] result)
        {

            for (int i = 0; i < matrix.Length; i++)
            {
                byte s = (byte)0;
                for (int j = 0; j < columnVector.Length; j++)
                    s += (byte)((byte)matrix[i][j] * (byte)columnVector[j]);
                result[i] = (byte)s;
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A*B</c> of two matrices <c>A</c> and <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A*B</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[,] Dot(this double[,] a, double[,] b)
        {
            return Dot(a, b, new double[a.Rows(), b.Columns()]);
        }

        /// <summary>
        ///   Computes the product <c>R = A*B</c> of two matrices <c>A</c>
        ///   and <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static int[,] Dot(this int[,] a, int[,] b, int[,] result)
        {
            int N = result.Length;
            int K = a.Columns();
            int M = result.Columns();

            var t = new int[K];

            for (int j = 0; j < M; j++)
            {
                for (int k = 0; k < b.Length; k++)
                    t[k] = b[k, j];

                for (int i = 0; i < a.Length; i++)
                {
                    int s = (int)0;
                    for (int k = 0; k < t.Length; k++)
                        s += (int)((int)a[i, k] * (int)t[k]);
                    result[i, j] = (int)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>R = A*B</c> of two matrices <c>A</c>
        ///   and <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product.</param>
        /// 
        public static Double[,] Dot(this Double[,] a, Double[,] b, Double[,] result)
        {
            int N = result.Length;
            int K = a.Columns();
            int M = result.Columns();

            var t = new Double[K];

            for (int j = 0; j < M; j++)
            {
                for (int k = 0; k < b.Length; k++)
                    t[k] = b[k, j];

                for (int i = 0; i < a.Length; i++)
                {
                    Double s = (Double)0;
                    for (int k = 0; k < t.Length; k++)
                        s += (int)((Double)a[i, k] * (Double)t[k]);
                    result[i, j] = (Double)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and transpose of <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A*B'</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[,] DotWithTransposed(this double[,] a, double[,] b)
        {
            return DotWithTransposed(a, b, new double[a.Rows(), b.Rows()]);
        }


        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and transpose of <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A*B'</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[][] DotWithTransposed(this double[][] a, double[][] b)
        {
            return DotWithTransposed(a, b, Create<double>(a.Length, b.Length));
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and
        ///   transpose of <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A*B'</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        public static double[,] DotWithTransposed(this double[,] a, double[,] b, double[,] result)
        {
            for (int i = 0; i < a.Length; i++)
            {
                double[] arow = a.GetRow(i);
                for (int j = 0; j < b.Length; j++)
                {
                    double sum = 0;
                    double[] brow = b.GetRow(j);
                    for (int k = 0; k < arow.Length; k++)
                        sum += (double)((double)arow[k] * (double)brow[k]);
                    result[i, j] = (double)sum;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A*B'</c> of matrix <c>A</c> and
        ///   transpose of <c>B</c>, storing the result in matrix <c>R</c>.
        /// </summary>
        /// 
        /// <param name="a">The left matrix <c>A</c>.</param>
        /// <param name="b">The transposed right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A*B'</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        ///    
        public static double[][] DotWithTransposed(this double[][] a, double[][] b, double[][] result)
        {
            for (int i = 0; i < a.Length; i++)
            {
                double[] arow = a[i];
                for (int j = 0; j < b.Length; j++)
                {
                    double sum = 0;
                    double[] brow = b[j];
                    for (int k = 0; k < arow.Length; k++)
                        sum += (double)((double)arow[k] * (double)brow[k]);
                    result[i][j] = (double)sum;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A'*B</c> of transposed of matrix <c>A</c> and <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A'*B</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[,] TransposeAndDot(this double[,] a, double[,] b)
        {
            var result = new Double[a.Columns(), b.Columns()];
            result.Fill<Double>(0d);
            return TransposeAndDot(a, b, result);
        }

        /// <summary>
        ///   Computes the product <c>A'*B</c> of matrix <c>A</c> transposed and matrix <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A'*B</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        /// 
        public static double[,] TransposeAndDot(this double[,] a, double[,] b, double[,] result)
        {
            int n = a.Length;
            int m = a.Columns();
            int p = b.Columns();

            var Bcolj = new double[n];
            for (int i = 0; i < p; i++)
            {
                for (int k = 0; k < b.Length; k++)
                    Bcolj[k] = b[k, i];

                for (int j = 0; j < m; j++)
                {
                    double s = (double)0;
                    for (int k = 0; k < Bcolj.Length; k++)
                        s += (double)((double)a[k, j] * (double)Bcolj[k]);
                    result[j, i] = (double)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the product <c>A'*B</c> of transposed of matrix <c>A</c> and <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        ///
        /// <returns>The product <c>A'*B</c> of the given matrices <c>A</c> and <c>B</c>.</returns>
        /// 
        public static double[][] TransposeAndDot(this double[][] a, double[][] b)
        {
            return TransposeAndDot(a, b, Create<double>(a.Columns(), b.Columns()));
        }


        /// <summary>
        ///   Computes the product <c>A'*B</c> of matrix <c>A</c> transposed and matrix <c>B</c>.
        /// </summary>
        /// 
        /// <param name="a">The transposed left matrix <c>A</c>.</param>
        /// <param name="b">The right matrix <c>B</c>.</param>
        /// <param name="result">The matrix <c>R</c> to store the product <c>R = A'*B</c>
        ///   of the given matrices <c>A</c> and <c>B</c>.</param>
        /// 
        public static double[][] TransposeAndDot(this double[][] a, double[][] b, double[][] result)
        {
            int n = a.Length;
            int m = a.Columns();
            int p = b.Columns();

            var Bcolj = new double[n];
            for (int i = 0; i < p; i++)
            {
                for (int k = 0; k < b.Length; k++)
                    Bcolj[k] = b[k][i];

                for (int j = 0; j < m; j++)
                {
                    double s = (double)0;
                    for (int k = 0; k < Bcolj.Length; k++)
                        s += (double)((double)a[k][j] * (double)Bcolj[k]);
                    result[j][i] = (double)s;
                }
            }
            return result;
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
		/// 
		/// <example>
		///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
		/// </example>
        /// 
        public static Double[][] Inverse(this Double[][] matrix)
        {
            return Inverse(matrix, false);
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
		/// <example>
		///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
		/// </example>
        /// 
        public static Double[][] Inverse(this Double[][] matrix, bool inPlace)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;

            if (rows != cols)
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_BE_SQUARE, "matrix");

            if (rows == 3)
            {
                // Special case for 3x3 matrices
                Double a = matrix[0][0], b = matrix[0][1], c = matrix[0][2];
                Double d = matrix[1][0], e = matrix[1][1], f = matrix[1][2];
                Double g = matrix[2][0], h = matrix[2][1], i = matrix[2][2];

                Double den = a * (e * i - f * h) -
                             b * (d * i - f * g) +
                             c * (d * h - e * g);

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[3][];
                    for (int j = 0; j < inv.Length; j++)
                        inv[j] = new Double[3];
                }

                inv[0][0] = m * (e * i - f * h);
                inv[0][1] = m * (c * h - b * i);
                inv[0][2] = m * (b * f - c * e);
                inv[1][0] = m * (f * g - d * i);
                inv[1][1] = m * (a * i - c * g);
                inv[1][2] = m * (c * d - a * f);
                inv[2][0] = m * (d * h - e * g);
                inv[2][1] = m * (b * g - a * h);
                inv[2][2] = m * (a * e - b * d);

                return inv;
            }

            if (rows == 2)
            {
                // Special case for 2x2 matrices
                Double a = matrix[0][0], b = matrix[0][1];
                Double c = matrix[1][0], d = matrix[1][1];

                Double den = a * d - b * c;

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[2][];
                    for (int j = 0; j < inv.Length; j++)
                        inv[j] = new Double[2];
                }

                inv[0][0] = +m * d;
                inv[0][1] = -m * b;
                inv[1][0] = -m * c;
                inv[1][1] = +m * a;

                return inv;
            }

            #region Calculate Jagged Lu Decomposition Inverse
            //return new JaggedLuDecomposition(matrix, false, inPlace).Inverse();

            var lu = matrix.MemberwiseClone();
            var pivotSign = 1;

            var pivotVector = new int[rows];
            for (int i = 0; i < rows; i++)
                pivotVector[i] = i;

            Double[] LUcolj = new Double[rows];


            // Outer loop.
            for (int j = 0; j < cols; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < rows; i++)
                    LUcolj[i] = lu[i][j];

                // Apply previous transformations.
                for (int i = 0; i < rows; i++)
                {
                    Double s = 0;

                    // Most of the time is spent in
                    // the following dot product:
                    int kmax = System.Math.Min(i, j);
                    for (int k = 0; k < kmax; k++)
                        s += lu[i][k] * LUcolj[k];

                    lu[i][j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        Double t = lu[p][k];
                        lu[p][k] = lu[j][k];
                        lu[j][k] = t;
                    }

                    int v = pivotVector[p];
                    pivotVector[p] = pivotVector[j];
                    pivotVector[j] = v;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.
                if (j < rows && lu[j][j] != 0)
                {
                    for (int i = j + 1; i < rows; i++)
                        lu[i][j] /= lu[j][j];
                }
            }

            // Copy right hand side with pivoting
            var X = new Double[rows][];
            for (int i = 0; i < rows; i++)
            {
                X[i] = new Double[rows];
                int k = pivotVector[i];
                X[i][k] = 1;
            }

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < rows; k++)
                for (int i = k + 1; i < rows; i++)
                    for (int j = 0; j < rows; j++)
                        X[i][j] -= X[k][j] * lu[i][k];

            // Solve U*X = I;
            for (int k = rows - 1; k >= 0; k--)
            {
                for (int j = 0; j < rows; j++)
                    X[k][j] /= lu[k][k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < rows; j++)
                        X[i][j] -= X[k][j] * lu[i][k];
            }

            #endregion

            return X;
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
        /// 
        /// <example>
        ///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
        /// </example>
        /// 
        public static Double[,] Inverse(this Double[,] matrix)
        {
            return Inverse(matrix, false);
        }

        /// <summary>
        ///   Computes the inverse of a matrix.
        /// </summary>
        /// 
		/// <example>
		///   <code source="Unit Tests\Accord.Tests.Math\Matrix\MatrixTest.cs" region="doc_inverse" />
		/// </example>
        /// 
        public static Double[,] Inverse(this Double[,] matrix, bool inPlace)
        {
            int rows = matrix.Rows();
            int cols = matrix.GetMaxColumnLength();

            if (rows != cols)
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_BE_SQUARE, "matrix");

            if (rows == 3)
            {
                // Special case for 3x3 matrices
                Double a = matrix[0, 0], b = matrix[0, 1], c = matrix[0, 2];
                Double d = matrix[1, 0], e = matrix[1, 1], f = matrix[1, 2];
                Double g = matrix[2, 0], h = matrix[2, 1], i = matrix[2, 2];

                Double den = a * (e * i - f * h) -
                             b * (d * i - f * g) +
                             c * (d * h - e * g);

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[3, 3];
                    for (int j = 0; j < inv.Length; j++)
                        inv.Fill(j, 0d);
                }

                inv[0, 0] = m * (e * i - f * h);
                inv[0, 1] = m * (c * h - b * i);
                inv[0, 2] = m * (b * f - c * e);
                inv[1, 0] = m * (f * g - d * i);
                inv[1, 1] = m * (a * i - c * g);
                inv[1, 2] = m * (c * d - a * f);
                inv[2, 0] = m * (d * h - e * g);
                inv[2, 1] = m * (b * g - a * h);
                inv[2, 2] = m * (a * e - b * d);

                return inv;
            }

            if (rows == 2)
            {
                // Special case for 2x2 matrices
                Double a = matrix[0, 0], b = matrix[0, 1];
                Double c = matrix[1, 0], d = matrix[1, 1];

                Double den = a * d - b * c;

                if (den == 0)
                    throw new SingularMatrixException();

                Double m = 1 / den;

                var inv = matrix;
                if (!inPlace)
                {
                    inv = new Double[2, 2];
                    for (int j = 0; j < inv.Length; j++)
                        inv.Fill(j, 2d);
                }

                inv[0, 0] = +m * d;
                inv[0, 1] = -m * b;
                inv[1, 0] = -m * c;
                inv[1, 1] = +m * a;

                return inv;
            }

            #region Calculate Jagged Lu Decomposition Inverse
            //return new JaggedLuDecomposition(matrix, false, inPlace).Inverse();

            var lu = matrix.MemberwiseClone();
            var pivotSign = 1;

            var pivotVector = new int[rows];
            for (int i = 0; i < rows; i++)
                pivotVector[i] = i;

            Double[] LUcolj = new Double[rows];


            // Outer loop.
            for (int j = 0; j < cols; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < rows; i++)
                    LUcolj[i] = lu[i, j];

                // Apply previous transformations.
                for (int i = 0; i < rows; i++)
                {
                    Double s = 0;

                    // Most of the time is spent in
                    // the following dot product:
                    int kmax = System.Math.Min(i, j);
                    for (int k = 0; k < kmax; k++)
                        s += lu[i, k] * LUcolj[k];

                    lu[i, j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        Double t = lu[p, k];
                        lu[p, k] = lu[j, k];
                        lu[j, k] = t;
                    }

                    int v = pivotVector[p];
                    pivotVector[p] = pivotVector[j];
                    pivotVector[j] = v;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.
                if (j < rows && lu[j, j] != 0)
                {
                    for (int i = j + 1; i < rows; i++)
                        lu[i, j] /= lu[j, j];
                }
            }

            // Copy right hand side with pivoting
            var X = new Double[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                X.Fill(i, 0d);
                int k = pivotVector[i];
                X[i, k] = 1;
            }

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < rows; k++)
                for (int i = k + 1; i < rows; i++)
                    for (int j = 0; j < rows; j++)
                        X[i, j] -= X[k, j] * lu[i, k];

            // Solve U*X = I;
            for (int k = rows - 1; k >= 0; k--)
            {
                for (int j = 0; j < rows; j++)
                    X[k, j] /= lu[k, k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < rows; j++)
                        X[i, j] -= X[k, j] * lu[i, k];
            }

            #endregion

            return X;
        }

    }

    public enum MatrixOrder
    {
        /// <summary>
        ///   Row-major order (C, C++, C#, SAS, Pascal, NumPy default).
        /// </summary>
        CRowMajor = 1,

        /// <summary>
        ///   Column-major oder (Fotran, MATLAB, R).
        /// </summary>
        /// 
        FortranColumnMajor = 0,

        /// <summary>
        ///   Default (Row-Major, C/C++/C# order).
        /// </summary>
        /// 
        Default = CRowMajor
    }
}

