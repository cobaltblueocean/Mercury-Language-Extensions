using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Mercury.Language.Exception;

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
    }
}
