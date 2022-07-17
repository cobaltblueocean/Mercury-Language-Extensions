using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Mercury.Language.Exception;
using Mercury.Language;

namespace MathNet.Numerics.LinearAlgebra
{
    public static class MatrixExtension
    {
        public static DenseVector Operate (this DenseMatrix matrix, DenseVector v)
        {
            int nRows = matrix.RowCount;
            int nCols = matrix.ColumnCount;
            if (v.Count != nCols)
            {
                throw new DimensionMismatchException(v.Count, nCols);
            }
            double[] result = new double[nRows];
            var data = matrix.ToArray();

            AutoParallel.AutoParallelFor(0, nRows, (row) =>
            {
                double[] dataRow = data.GetRow(row);
                double sum = 0;
                AutoParallel.AutoParallelFor(0, nCols, (i) =>
                {
                    sum += dataRow[i] * v[i];
                });
                result[row] = sum;
            });

            return new DenseVector(result);
        }

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
    }
}
