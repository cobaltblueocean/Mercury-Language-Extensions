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
    public static class MatrixUtils
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
            return Matrix<Double>.Build.Dense(rows, columns);
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
            return Matrix<Double>.Build.Dense(storage);
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
            return Matrix<Double>.Build.Dense(storage);
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
        public static Vector<Double> CreateRealVector(double[] data)
        {
            return Vector<Double>.Build.Dense(data);
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
            if (column < 0 || column >= m.ColumnCount) {
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
            if (selectedRows.Length * selectedColumns.Length == 0) {
                if (selectedRows.Length == 0)
                {
                    throw new MatrixIndexException(LocalizedResources.Instance().EMPTY_SELECTED_ROW_INDEX_ARRAY);
                }
                throw new MatrixIndexException(LocalizedResources.Instance().EMPTY_SELECTED_COLUMN_INDEX_ARRAY);
            }

            AutoParallel.AutoParallelForEach(selectedRows, (row) => {
                CheckRowIndex(m, row);
            });
            AutoParallel.AutoParallelForEach(selectedColumns, (column) => {
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
                (left.ColumnCount != right.ColumnCount)) {
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
                (left.ColumnCount != right.ColumnCount)) {
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
            if (left.ColumnCount != right.RowCount) {
                throw MathRuntimeException.CreateArithmeticException(String.Format(LocalizedResources.Instance().NOT_MULTIPLICATION_COMPATIBLE_MATRICES, left.RowCount, left.ColumnCount, right.RowCount, right.ColumnCount));
            }
        }
    }
}
