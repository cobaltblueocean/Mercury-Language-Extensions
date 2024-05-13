using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Mercury.Language.Extensions;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Math.Matrix;
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Language.Log;

namespace MathNet.Numerics.LinearAlgebra
{
    public static class MatrixExtension
    {
        public static void AddToEntry<T>(this Matrix<T> matrix, int row, int column, double increment) where T : struct, IEquatable<T>, IFormattable
        {
            try
            {
                dynamic a = matrix[row, column];
                dynamic b = increment;
                matrix[row, column] = a + b;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new MatrixIndexException(String.Format(LocalizedResources.Instance().NO_SUCH_MATRIX_ENTRY, row, column, matrix.RowCount, matrix.ColumnCount), e);
            }
        }

        public static T[,] AsArrayEx<T>(this Matrix<T> matrix) where T : struct, IEquatable<T>, IFormattable
        {
            T[,] _temp = matrix.AsArray();
            if (_temp == null)
                _temp = matrix.ToArray();

            return _temp;
        }

        public static Vector<T> GetRow<T>(this Matrix<T> matrix, int Index) where T : struct, IEquatable<T>, IFormattable
        {
            T[] data = new T[matrix.ColumnCount];

            for (int i = 0; i < matrix.ColumnCount; i++)
                data[i] = matrix[Index, i];

            return MathNetMatrixUtility.CreateVector(data);
        }

        public static void SetRow<T>(this Matrix<T> matrix, int Index, Vector<T> value) where T : struct, IEquatable<T>, IFormattable
        {
            if (matrix.ColumnCount == value.Count)
            {
                for (int i = 0; i < matrix.ColumnCount; i++)
                {
                    matrix[Index, i] = value[i];
                }
            }
            else
            {
                throw new IndexOutOfRangeException(LocalizedResources.Instance().Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow);
            }
        }

        /// <summary>
        /// Returns the entries in column number<code>column</code>
        /// as a vector.  Column indices start at 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <param name="column">the column to be fetched</param>
        /// <returns>column vector</returns>
        public static Vector<T> GetColumnVector<T>(this Matrix<T> matrix, int column) where T : struct, IEquatable<T>, IFormattable

        {
            return MathNetMatrixUtility.CreateVector<T>(GetColumn(matrix, column));
        }

        public static void SetColumnVector<T>(this Matrix<T> matrix, int column, Vector<T> vector) where T : struct, IEquatable<T>, IFormattable
        {
            MathNetMatrixUtility.CheckColumnIndex(matrix, column);
            int nRows = matrix.RowCount;
            if (vector.Count != nRows)
            {
                throw new InvalidMatrixException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_2x2, vector.Count, 1, nRows, 1));
            }
            for (int i = 0; i < nRows; ++i)
            {
                matrix[i, column] = vector[i];
            }
        }

        /// <summary>
        /// Returns the entries in column number<code>col</code> as an array.
        /// <p>
        /// Column indices start at 0.  A<code> MatrixIndexException</code> is thrown
        /// unless <code>0 <= column<columnDimension.</code></p>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">the matrix to be fetched</param>
        /// <param name="column">the column to be fetched</param>
        /// <returns>array of entries in the column</returns>
        public static T[] GetColumn<T>(this Matrix<T> matrix, int column) where T : struct, IEquatable<T>, IFormattable
        {

            MathNetMatrixUtility.CheckColumnIndex(matrix, column);
            int nRows = matrix.RowCount;
            T[] _out = new T[nRows];
            for (int i = 0; i < nRows; ++i)
            {
                _out[i] = matrix[i, column];
            }

            return _out;
        }

        public static T[] Operate<T>(this Matrix<T> matrix, T[] v) where T : struct, IEquatable<T>, IFormattable
        {

            int nRows = matrix.RowCount;
            int nCols = matrix.ColumnCount;
            if (v.Length != nCols)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().VECTOR_LENGTH_MISMATCH, v.Length, nCols));
            }

            T[] _out = new T[nRows];
            for (int row = 0; row < nRows; ++row)
            {
                double sum = 0;
                for (int i = 0; i < nCols; ++i)
                {
                    sum += matrix[row, i].CastType<double>() * v[i].CastType<double>();
                }
                _out[row] = sum.CastType<T>();
            }

            return _out;

        }

        public static Vector<T> Operate<T>(this Matrix<T> matrix, Vector<T> v) where T : struct, IEquatable<T>, IFormattable
        {
            try
            {
                return MathNetMatrixUtility.CreateVector<T>(matrix.Operate(((Vector<T>)v).AsArrayEx()));
            }
            catch (InvalidCastException cce)
            {
                // Log what happened.
                Logger.Information(cce.Message);

                int nRows = matrix.RowCount;
                int nCols = matrix.ColumnCount;
                if (v.Count != nCols)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().VECTOR_LENGTH_MISMATCH, v.Count, nCols));
                }

                T[] _out = new T[nRows];
                for (int row = 0; row < nRows; ++row)
                {
                    double sum = 0;
                    for (int i = 0; i < nCols; ++i)
                    {
                        sum += matrix[row, i].CastType<double>() * v[i].CastType<double>();
                    }
                    _out[row] = sum.CastType<T>();
                }

                return MathNetMatrixUtility.CreateVector<T>(_out);
            }
        }

        public static T Multiply<T>(this T[] array, T d) where T : struct, IEquatable<T>, IFormattable
        {
            double sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i].CastType<double>() * d.CastType<double>();
            }

            return sum.CastType<T>();
        }

        public static Vector<T> MapMultiply<T>(this Matrix<T> matrix, T d) where T : struct, IEquatable<T>, IFormattable
        {
            T[] _out = new T[matrix.RowCount];
            T[][] data = matrix.AsArrayEx().ToJagged();

            for (int i = 0; i < matrix.RowCount; i++)
            {
                _out[i] = data[i].Multiply(d);
            }
            return MathNetMatrixUtility.CreateVector<T>(_out);
        }

        public static Matrix<T> GetSubMatrix<T>(this Matrix<T> matrix, int startRow, int endRow, int startColumn, int endColumn) where T : struct, IEquatable<T>, IFormattable
        {

            MathNetMatrixUtility.CheckSubMatrixIndex(matrix, startRow, endRow, startColumn, endColumn);

            Matrix<T> subMatrix = MathNetMatrixUtility.CreateMatrix<T>(endRow - startRow + 1, endColumn - startColumn + 1);
            for (int i = startRow; i <= endRow; ++i)
            {
                for (int j = startColumn; j <= endColumn; ++j)
                {
                    subMatrix[i - startRow, j - startColumn] = matrix[i, j];
                }
            }
            return subMatrix;
        }

        public static Matrix<T> GetSubMatrix<T>(this Matrix<T> matrix, int[] selectedRows, int[] selectedColumns) where T : struct, IEquatable<T>, IFormattable
        {

            // safety checks
            MathNetMatrixUtility.CheckSubMatrixIndex(matrix, selectedRows, selectedColumns);

            // copy entries
            Matrix<T> subMatrix = MathNetMatrixUtility.CreateMatrix<T>(selectedRows.Length, selectedColumns.Length);
            subMatrix.WalkInOptimizedOrder(new DefaultRealMatrixPreservingVisitor()
            {
                visitfunc = new Func<int, int, double, double>((row, column, value) =>
                {
                    return matrix[selectedRows[row], selectedColumns[column]].CastType<double>();
                })
            });

            return subMatrix;

        }

        /** {@inheritDoc} */
        public static void CopySubMatrix<T>(this Matrix<T> matrix, int startRow, int endRow, int startColumn, int endColumn, double[][] destination) where T : struct, IEquatable<T>, IFormattable
        {

            // safety checks
            MathNetMatrixUtility.CheckSubMatrixIndex(matrix, startRow, endRow, startColumn, endColumn);
            int rowsCount = endRow + 1 - startRow;
            int columnsCount = endColumn + 1 - startColumn;
            if ((destination.Length < rowsCount) || (destination[0].Length < columnsCount))
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_2x2, destination.Length, destination[0].Length, rowsCount, columnsCount));
            }

            // copy entries
            WalkInOptimizedOrder(matrix, new DefaultRealMatrixPreservingVisitor2(destination), startRow, endRow, startColumn, endColumn);

        }

        /** {@inheritDoc} */
        public static void CopySubMatrix<T>(this Matrix<T> matrix, int[] selectedRows, int[] selectedColumns, double[][] destination) where T : struct, IEquatable<T>, IFormattable
        {

            // safety checks
            MathNetMatrixUtility.CheckSubMatrixIndex(matrix, selectedRows, selectedColumns);
            if ((destination.Length < selectedRows.Length) ||
                (destination[0].Length < selectedColumns.Length))
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_2x2, destination.Length, destination[0].Length, selectedRows.Length, selectedColumns.Length));
            }

            // copy entries
            for (int i = 0; i < selectedRows.Length; i++)
            {
                double[] destinationI = destination[i];
                for (int j = 0; j < selectedColumns.Length; j++)
                {
                    destinationI[j] = matrix[selectedRows[i], selectedColumns[j]].CastType<double>();
                }
            }
        }

        /** {@inheritDoc} */
        public static void SetSubMatrix<T>(this Matrix<T> matrix, double[][] subMatrix, int row, int column) where T : struct, IEquatable<T>, IFormattable
        {

            int nRows = subMatrix.Length;
            if (nRows == 0)
            {
                throw new ArgumentException(LocalizedResources.Instance().AT_LEAST_ONE_ROW);
            }

            int nCols = subMatrix[0].Length;
            if (nCols == 0)
            {
                throw new ArgumentException(LocalizedResources.Instance().AT_LEAST_ONE_COLUMN);
            }

            for (int r = 1; r < nRows; ++r)
            {
                if (subMatrix[r].Length != nCols)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().DIFFERENT_ROWS_LENGTHS, nCols, subMatrix[r].Length));
                }
            }

            MathNetMatrixUtility.CheckRowIndex(matrix, row);
            MathNetMatrixUtility.CheckColumnIndex(matrix, column);
            MathNetMatrixUtility.CheckRowIndex(matrix, nRows + row - 1);
            MathNetMatrixUtility.CheckColumnIndex(matrix, nCols + column - 1);

            for (int i = 0; i < nRows; ++i)
            {
                for (int j = 0; j < nCols; ++j)
                {
                    matrix[row + i, column + j] = subMatrix[i][j].CastType<T>();
                }
            }
            //lu = null;
        }


        /** {@inheritDoc} */
        public static double WalkInOptimizedOrder<T>(this Matrix<T> matrix, IRealMatrixPreservingVisitor visitor) where T : struct, IEquatable<T>, IFormattable

        {
            return WalkInRowOrder(matrix, visitor);
        }


        /** {@inheritDoc} */
        public static double WalkInOptimizedOrder<T>(this Matrix<T> matrix, IRealMatrixPreservingVisitor visitor,
                                           int startRow, int endRow,
                                           int startColumn, int endColumn) where T : struct, IEquatable<T>, IFormattable
        {
            return WalkInRowOrder(matrix, visitor, startRow, endRow, startColumn, endColumn);
        }


        /** {@inheritDoc} */
        public static double WalkInRowOrder<T>(this Matrix<T> matrix, IRealMatrixPreservingVisitor visitor) where T : struct, IEquatable<T>, IFormattable

        {
            int rows = matrix.RowCount;
            int columns = matrix.ColumnCount;
            visitor.Start(rows, columns, 0, rows - 1, 0, columns - 1);
            for (int row = 0; row < rows; ++row)
            {
                for (int column = 0; column < columns; ++column)
                {
                    visitor.Visit(row, column, matrix[row, column].CastType<double>());
                }
            }
            return visitor.End();
        }


        /** {@inheritDoc} */
        public static double WalkInRowOrder<T>(this Matrix<T> matrix, IRealMatrixPreservingVisitor visitor, int startRow, int endRow, int startColumn, int endColumn) where T : struct, IEquatable<T>, IFormattable
        {
            MathNetMatrixUtility.CheckSubMatrixIndex(matrix, startRow, endRow, startColumn, endColumn);
            visitor.Start(matrix.RowCount, matrix.ColumnCount,
                          startRow, endRow, startColumn, endColumn);
            for (int row = startRow; row <= endRow; ++row)
            {
                for (int column = startColumn; column <= endColumn; ++column)
                {
                    visitor.Visit(row, column, matrix[row, column].CastType<double>());
                }
            }
            return visitor.End();
        }

        private class DefaultRealMatrixPreservingVisitor2 : IRealMatrixPreservingVisitor
        {
            /** Initial row index. */
            private int startRow;

            /** Initial column index. */
            private int startColumn;

            private double[][] _destination;

            public DefaultRealMatrixPreservingVisitor2(double[][] destination)
            {
                _destination = destination;
            }

            public double End()
            {
                return 0;
            }

            /** {@inheritDoc} */
            //@Override
            public double Start(int rows, int columns,
                               int startRow, int endRow,
                               int startColumn, int endColumn)
            {
                this.startRow = startRow;
                this.startColumn = startColumn;

                return 0;
            }

            /** {@inheritDoc} */
            //@Override
            public double Visit(int row, int column, double value)
            {
                _destination[row - startRow][column - startColumn] = value;

                return 0;
            }
        }
    }
}
