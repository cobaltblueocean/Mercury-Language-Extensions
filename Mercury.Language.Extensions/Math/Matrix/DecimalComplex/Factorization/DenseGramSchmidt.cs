﻿// <copyright file="DenseGramSchmidt.cs" company="QuickMath.NET">
// QuickMath.NET Numerics, part of the QuickMath.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2013 QuickMath.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Providers.LinearAlgebra;
using Mercury.Language;

namespace MathNet.Numerics.LinearAlgebra.DecimalComplex.Factorization
{
    using DecimalComplex = System.Numerics.DecimalComplex;

    /// <summary>
    /// <para>A class which encapsulates the functionality of the QR decomposition Modified Gram-Schmidt Orthogonalization.</para>
    /// <para>Any complex square matrix A may be decomposed as A = QR where Q is an unitary mxn matrix and R is an nxn upper triangular matrix.</para>
    /// </summary>
    /// <remarks>
    /// The computation of the QR decomposition is done at construction time by modified Gram-Schmidt Orthogonalization.
    /// </remarks>
    internal sealed class DenseGramSchmidt : GramSchmidt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DenseGramSchmidt"/> class. This object creates an unitary matrix
        /// using the modified Gram-Schmidt method.
        /// </summary>
        /// <param name="matrix">The matrix to factor.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="matrix"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="matrix"/> row count is less then column count</exception>
        /// <exception cref="ArgumentException">If <paramref name="matrix"/> is rank deficient</exception>
        public static DenseGramSchmidt Create(Matrix<DecimalComplex> matrix)
        {
            if (matrix.RowCount < matrix.ColumnCount)
            {
                throw MatrixExceptionFactory.DimensionsDontMatch<ArgumentException>(matrix.RowCount, matrix.ColumnCount);
            }

            var q = (DenseMatrix)matrix.Clone();
            var r = new DenseMatrix(matrix.ColumnCount, matrix.ColumnCount);
            Factorize(q.Values, q.RowCount, q.ColumnCount, r.Values);

            return new DenseGramSchmidt(q, r);
        }

        DenseGramSchmidt(Matrix<DecimalComplex> q, Matrix<DecimalComplex> rFull)
            : base(q, rFull)
        {
        }

        /// <summary>
        /// Factorize matrix using the modified Gram-Schmidt method.
        /// </summary>
        /// <param name="q">Initial matrix. On exit is replaced by <see cref="Matrix{T}"/> Q.</param>
        /// <param name="rowsQ">Number of rows in <see cref="Matrix{T}"/> Q.</param>
        /// <param name="columnsQ">Number of columns in <see cref="Matrix{T}"/> Q.</param>
        /// <param name="r">On exit is filled by <see cref="Matrix{T}"/> R.</param>
        static void Factorize(DecimalComplex[] q, int rowsQ, int columnsQ, DecimalComplex[] r)
        {
            for (var k = 0; k < columnsQ; k++)
            {
                var norm = 0.0M;
                for (var i = 0; i < rowsQ; i++)
                {
                    norm += q[(k * rowsQ) + i].Magnitude * q[(k * rowsQ) + i].Magnitude;
                }

                norm = QuickMath.Sqrt(norm);
                if (norm == 0.0M)
                {
                    throw new ArgumentException(LocalizedResources.Instance().MATRIX_MUST_NOT_BE_RANK_DEFICIENT);
                }

                r[(k * columnsQ) + k] = norm;
                for (var i = 0; i < rowsQ; i++)
                {
                    q[(k * rowsQ) + i] /= norm;
                }

                for (var j = k + 1; j < columnsQ; j++)
                {
                    var k1 = k;
                    var j1 = j;

                    var dot = DecimalComplex.Zero;
                    for (var index = 0; index < rowsQ; index++)
                    {
                        dot += q[(k1 * rowsQ) + index].Conjugate() * q[(j1 * rowsQ) + index];
                    }

                    r[(j * columnsQ) + k] = dot;
                    for (var i = 0; i < rowsQ; i++)
                    {
                        var value = q[(j * rowsQ) + i] - (q[(k * rowsQ) + i] * dot);
                        q[(j * rowsQ) + i] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Solves a system of linear equations, <b>AX = B</b>, with A QR factorized.
        /// </summary>
        /// <param name="input">The right hand side <see cref="Matrix{T}"/>, <b>B</b>.</param>
        /// <param name="result">The left hand side <see cref="Matrix{T}"/>, <b>X</b>.</param>
        public override void Solve(Matrix<DecimalComplex> input, Matrix<DecimalComplex> result)
        {
            // The solution X should have the same number of columns as B
            if (input.ColumnCount != result.ColumnCount)
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_COLUMN_DIMENSIONS_MUST_AGREE);
            }

            // The dimension compatibility conditions for X = A\B require the two matrices A and B to have the same number of rows
            if (Q.RowCount != input.RowCount)
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);
            }

            // The solution X row dimension is equal to the column dimension of A
            if (Q.ColumnCount != result.RowCount)
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_COLUMN_DIMENSIONS_MUST_AGREE);
            }

            if (input is DenseMatrix dinput && result is DenseMatrix dresult)
            {
                LinearAlgebraControl2.Provider.QRSolveFactored(((DenseMatrix) Q).Values, ((DenseMatrix) FullR).Values, Q.RowCount, FullR.ColumnCount, null, dinput.Values, input.ColumnCount, dresult.Values, QRMethod.Thin);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().CAN_ONLY_DO_FACTORIZATION_FOR_DENSE_MATRICES_AT_THE_MOMENT, "GramSchmidt"));
            }
        }

        /// <summary>
        /// Solves a system of linear equations, <b>Ax = b</b>, with A QR factorized.
        /// </summary>
        /// <param name="input">The right hand side vector, <b>b</b>.</param>
        /// <param name="result">The left hand side <see cref="Matrix{T}"/>, <b>x</b>.</param>
        public override void Solve(Vector<DecimalComplex> input, Vector<DecimalComplex> result)
        {
            // Ax=b where A is an m x n matrix
            // Check that b is a column vector with m entries
            if (Q.RowCount != input.Count)
            {
                throw new ArgumentException(LocalizedResources.Instance().ALL_VECTORS_MUST_HAVE_THE_SAME_DIMENSIONALITY);
            }

            // Check that x is a column vector with n entries
            if (Q.ColumnCount != result.Count)
            {
                throw MatrixExceptionFactory.DimensionsDontMatch<ArgumentException>(Q.ColumnCount, result.Count);
            }

            if (input is DenseVector dinput && result is DenseVector dresult)
            {
                LinearAlgebraControl2.Provider.QRSolveFactored(((DenseMatrix) Q).Values, ((DenseMatrix) FullR).Values, Q.RowCount, FullR.ColumnCount, null, dinput.Values, 1, dresult.Values, QRMethod.Thin);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().CAN_ONLY_DO_FACTORIZATION_FOR_DENSE_VECTORS_AT_THE_MOMENT, "GramSchmidt"));
            }
        }
    }
}
