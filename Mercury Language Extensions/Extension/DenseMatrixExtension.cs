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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    /// DenseMatrixExtensions Description
    /// </summary>
    public static class DenseMatrixExtension
    {
        public static double[,] AsArray(this DenseMatrix dm)
        {
            var result = new double[dm.RowCount, dm.ColumnCount];

            AutoParallel.AutoParallelFor(0, dm.RowCount, (i) =>
            {
                AutoParallel.AutoParallelFor(0, dm.ColumnCount, (j) =>
                {
                    result[i, j] = dm.At(i, j);
                });
            });

            return result;
        }

        public static void Load(this DenseMatrix dm, System.Double[,] data)
        {
            var row = data.GetLength(0);
            var col = data.GetLength(1);

            if ((dm.RowCount != row) || (dm.ColumnCount != col))
                throw new IndexOutOfRangeException("Row count and/or Column count not match with loading data matrix.");

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    dm.At(i, j, data[i, j]);
                }
            }
        }

        public static Boolean IsSquare<T>(this Matrix<T> mtx) where T : struct, IEquatable<T>, IFormattable
        {
            if (mtx.ColumnCount == mtx.RowCount)
                return true;
            else
                return false;
        }

        public static void SetColumnVector(this DenseMatrix matrix, int column, DenseVector vector)
        {
            var rows = matrix.RowCount;
            if (vector.Count != rows)
                throw new MatrixDimensionMismatchException(vector.Count, 1, rows, 1);

            for (int i = 0; i < rows; ++i)
            {
                matrix.At(i, column, vector[i]);
            }
        }


        public static void SetRowVector(this DenseMatrix matrix, int row, DenseVector vector)
        {
            var cols = matrix.ColumnCount;
            if (vector.Count != cols)
                throw new MatrixDimensionMismatchException(1, vector.Count, 1, cols);

            for (int i = 0; i < cols; ++i)
            {
                matrix.At(row, i, vector[i]);
            }
        }

        public static Boolean IsSymmetric(this DenseMatrix matrix, double eps)
        {
            return IsSymmetricInternal(matrix, eps, false);

        }

        private static Boolean IsSymmetricInternal(this DenseMatrix matrix, double relativeTolerance, Boolean raiseException)
        {
            int rows = matrix.RowCount;
            if (rows != matrix.ColumnCount)
            {
                if (raiseException)
                {
                    throw new NonSquareMatrixException(rows, matrix.ColumnCount);
                }
                else
                {
                    return false;
                }
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = i + 1; j < rows; j++)
                {
                    double mij = matrix.At(i, j);
                    double mji = matrix.At(j, i);
                    if (System.Math.Abs(mij - mji) >
                        System.Math.Max(System.Math.Abs(mij), System.Math.Abs(mji)) * relativeTolerance)
                    {
                        if (raiseException)
                        {
                            throw new NonSymmetricMatrixException(i, j, relativeTolerance);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
