﻿// Copyright (c) 2017 - presented by Kei Nakai
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
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Storage;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Threading;

namespace MathNet.Numerics.LinearAlgebra.Decimal.Factorization
{
/// <summary>
    /// <para>A class which encapsulates the functionality of a Cholesky factorization for user matrices.</para>
    /// <para>For a symmetric, positive definite matrix A, the Cholesky factorization
    /// is an lower triangular matrix L so that A = L*L'.</para>
    /// </summary>
    /// <remarks>
    /// The computation of the Cholesky factorization is done at construction time. If the matrix is not symmetric
    /// or positive definite, the constructor will throw an exception.
    /// </remarks>
    public sealed class UserCholesky : Cholesky
    {
        /// <summary>
        /// Computes the Cholesky factorization in-place.
        /// </summary>
        /// <param name="factor">On entry, the matrix to factor. On exit, the Cholesky factor matrix</param>
        /// <exception cref="ArgumentNullException">If <paramref name="factor"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="factor"/> is not a square matrix.</exception>
        /// <exception cref="ArgumentException">If <paramref name="factor"/> is not positive definite.</exception>
        static void DoCholesky(Matrix<decimal> factor)
        {
            if (factor.RowCount != factor.ColumnCount)
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_BE_SQUARE);
            }

            var tmpColumn = new decimal[factor.RowCount];

            // Main loop - along the diagonal
            for (var ij = 0; ij < factor.RowCount; ij++)
            {
                // "Pivot" element
                var tmpVal = factor.At(ij, ij);

                if (tmpVal > 0.0M)
                {
                    tmpVal = QuickMath.Sqrt(tmpVal);
                    factor.At(ij, ij, tmpVal);
                    tmpColumn[ij] = tmpVal;

                    // Calculate multipliers and copy to local column
                    // Current column, below the diagonal
                    for (var i = ij + 1; i < factor.RowCount; i++)
                    {
                        factor.At(i, ij, factor.At(i, ij) / tmpVal);
                        tmpColumn[i] = factor.At(i, ij);
                    }

                    // Remaining columns, below the diagonal
                    DoCholeskyStep(factor, factor.RowCount, ij + 1, factor.RowCount, tmpColumn, Control.MaxDegreeOfParallelism);
                }
                else
                {
                    throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_BE_POSITIVE_DEFINITE);
                }

                for (var i = ij + 1; i < factor.RowCount; i++)
                {
                    factor.At(ij, i, 0.0M);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCholesky"/> class. This object will compute the
        /// Cholesky factorization when the constructor is called and cache it's factorization.
        /// </summary>
        /// <param name="matrix">The matrix to factor.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="matrix"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="matrix"/> is not a square matrix.</exception>
        /// <exception cref="ArgumentException">If <paramref name="matrix"/> is not positive definite.</exception>
        public static UserCholesky Create(Matrix<decimal> matrix)
        {
            // Create a new matrix for the Cholesky factor, then perform factorization (while overwriting).
            var factor = matrix.Clone();
            DoCholesky(factor);
            return new UserCholesky(factor);
        }

        /// <summary>
        /// Calculates the Cholesky factorization of the input matrix.
        /// </summary>
        /// <param name="matrix">The matrix to be factorized<see cref="Matrix{T}"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="matrix"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="matrix"/> is not a square matrix.</exception>
        /// <exception cref="ArgumentException">If <paramref name="matrix"/> is not positive definite.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="matrix"/> does not have the same dimensions as the existing factor.</exception>
        public override void Factorize(Matrix<decimal> matrix)
        {
            if (matrix.RowCount != Factor.RowCount || matrix.ColumnCount != Factor.ColumnCount)
            {
                throw MatrixExceptionFactory.DimensionsDontMatch<ArgumentException>(matrix, Factor);
            }

            matrix.CopyTo(Factor);
            DoCholesky(Factor);
        }

        UserCholesky(Matrix<decimal> factor)
            : base(factor)
        {
        }

        /// <summary>
        /// Calculate Cholesky step
        /// </summary>
        /// <param name="data">Factor matrix</param>
        /// <param name="rowDim">Number of rows</param>
        /// <param name="firstCol">Column start</param>
        /// <param name="colLimit">Total columns</param>
        /// <param name="multipliers">Multipliers calculated previously</param>
        /// <param name="availableCores">Number of available processors</param>
        static void DoCholeskyStep(Matrix<decimal> data, int rowDim, int firstCol, int colLimit, decimal[] multipliers, int availableCores)
        {
            var tmpColCount = colLimit - firstCol;

            if ((availableCores > 1) && (tmpColCount > 200))
            {
                var tmpSplit = firstCol + (tmpColCount / 3);
                var tmpCores = availableCores / 2;

                CommonParallel.Invoke(
                    () => DoCholeskyStep(data, rowDim, firstCol, tmpSplit, multipliers, tmpCores),
                    () => DoCholeskyStep(data, rowDim, tmpSplit, colLimit, multipliers, tmpCores));
            }
            else
            {
                for (var j = firstCol; j < colLimit; j++)
                {
                    var tmpVal = multipliers[j];
                    for (var i = j; i < rowDim; i++)
                    {
                        data.At(i, j, data.At(i, j) - (multipliers[i] * tmpVal));
                    }
                }
            }
        }

        /// <summary>
        /// Solves a system of linear equations, <b>AX = B</b>, with A Cholesky factorized.
        /// </summary>
        /// <param name="input">The right hand side <see cref="Matrix{T}"/>, <b>B</b>.</param>
        /// <param name="result">The left hand side <see cref="Matrix{T}"/>, <b>X</b>.</param>
        public override void Solve(Matrix<decimal> input, Matrix<decimal> result)
        {
            if (result.RowCount != input.RowCount)
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);
            }

            if (result.ColumnCount != input.ColumnCount)
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_COLUMN_DIMENSIONS_MUST_AGREE);
            }

            if (input.RowCount != Factor.RowCount)
            {
                throw MatrixExceptionFactory.DimensionsDontMatch<ArgumentException>(input, Factor);
            }

            input.CopyTo(result);
            var order = Factor.RowCount;

            for (var c = 0; c < result.ColumnCount; c++)
            {
                // Solve L*Y = B;
                decimal sum;
                for (var i = 0; i < order; i++)
                {
                    sum = result.At(i, c);
                    for (var k = i - 1; k >= 0; k--)
                    {
                        sum -= Factor.At(i, k) * result.At(k, c);
                    }

                    result.At(i, c, sum / Factor.At(i, i));
                }

                // Solve L'*X = Y;
                for (var i = order - 1; i >= 0; i--)
                {
                    sum = result.At(i, c);
                    for (var k = i + 1; k < order; k++)
                    {
                        sum -= Factor.At(k, i) * result.At(k, c);
                    }

                    result.At(i, c, sum / Factor.At(i, i));
                }
            }
        }

        /// <summary>
        /// Solves a system of linear equations, <b>Ax = b</b>, with A Cholesky factorized.
        /// </summary>
        /// <param name="input">The right hand side vector, <b>b</b>.</param>
        /// <param name="result">The left hand side <see cref="Matrix{T}"/>, <b>x</b>.</param>
        public override void Solve(Vector<decimal> input, Vector<decimal> result)
        {
            if (input.Count != result.Count)
            {
                throw new ArgumentException(LocalizedResources.Instance().ALL_VECTORS_MUST_HAVE_THE_SAME_DIMENSIONALITY);
            }

            if (input.Count != Factor.RowCount)
            {
                throw MatrixExceptionFactory.DimensionsDontMatch<ArgumentException>(input, Factor);
            }

            input.CopyTo(result);
            var order = Factor.RowCount;

            // Solve L*Y = B;
            decimal sum;
            for (var i = 0; i < order; i++)
            {
                sum = result[i];
                for (var k = i - 1; k >= 0; k--)
                {
                    sum -= Factor.At(i, k) * result[k];
                }

                result[i] = sum / Factor.At(i, i);
            }

            // Solve L'*X = Y;
            for (var i = order - 1; i >= 0; i--)
            {
                sum = result[i];
                for (var k = i + 1; k < order; k++)
                {
                    sum -= Factor.At(k, i) * result[k];
                }

                result[i] = sum / Factor.At(i, i);
            }
        }

    }
}
