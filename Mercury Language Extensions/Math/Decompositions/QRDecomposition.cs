// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Decompositions Inc.
//
// Copyright (C) 2012 - present by System.Math.Decompositions Inc. and the System.Math.Decompositions group of companies
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

// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
// Original work copyright © Lutz Roeder, 2000
//  Adapted from Mapack for .NET, September 2000
//  Adapted from Mapack for COM and Jama routines
//  http://www.aisto.com/roeder/dotnet
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Decompositions
{
    /// <summary>
    ///   QR decomposition for a rectangular matrix.
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    ///   For an m-by-n matrix <c>A</c> with <c>m >= n</c>, the QR decomposition
    ///   is an m-by-n orthogonal matrix <c>Q</c> and an n-by-n upper triangular
    ///   matrix <c>R</c> so that <c>A = Q * R</c>.</para>
    /// <para>
    ///   The QR decomposition always exists, even if the matrix does not have
    ///   full rank, so the constructor will never fail. The primary use of the
    ///   QR decomposition is in the least squares solution of nonsquare systems
    ///   of simultaneous linear equations.
    ///   This will fail if <see cref="FullRank"/> returns <see langword="false"/>.</para>  
    /// </remarks>
    public sealed class QRDecomposition : ICloneable, ISolverMatrixDecomposition<Double>
    {
        private int n;
        private int m;
        private int p;
        private bool economy;

        private Double[,] qr;
        private Double[] Rdiag;

        // cache for lazy evaluation
        private bool? fullRank;
        private Double[,] orthogonalFactor;
        private Double[,] upperTriangularFactor;

        /// <summary>Constructs a QR decomposition.</summary>    
        /// <param name="value">The matrix A to be decomposed.</param>
        /// <param name="transpose">True if the decomposition should be performed on
        /// the transpose of A rather than A itself, false otherwise. Default is false.</param>
        /// <param name="inPlace">True if the decomposition should be done in place,
        /// overriding the given matrix <paramref name="value"/>. Default is false.</param>
        /// <param name="economy">True to perform the economy decomposition, where only
        ///.the information needed to solve linear systems is computed. If set to false,
        /// the full QR decomposition will be computed.</param>
        public QRDecomposition(Double[,] value, bool transpose = false, bool economy = true, bool inPlace = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", String.Format(LocalizedResources.Instance().MATRIX_CANNOT_BE_NULL, "value"));
            }

            if ((!transpose && value.Rows() < value.Columns()) ||
                 (transpose && value.Columns() < value.Rows()))
            {
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_HAS_MORE_COLUMN_TNAN_ROWS, "value");
            }

            // https://www.inf.ethz.ch/personal/gander/papers/qrneu.pdf

            if (transpose)
            {
                this.p = value.Rows();

                if (economy)
                {
                    // Compute the faster, economy-sized QR decomposition 
                    this.qr = value.Transpose(inPlace: inPlace);
                }
                else
                {
                    // Create room to store the full decomposition
                    this.qr = MatrixUtility.Create(value.Columns(), value.Columns(), value, transpose: true);
                }
            }
            else
            {
                this.p = value.Columns();

                if (economy)
                {
                    // Compute the faster, economy-sized QR decomposition 
                    this.qr = inPlace ? value : value.Copy();
                }
                else
                {
                    // Create room to store the full decomposition
                    this.qr = MatrixUtility.Create(value.Rows(), value.Rows(), value, transpose: false);
                }
            }

            this.economy = economy;
            this.n = qr.Rows();
            this.m = qr.Columns();
            this.Rdiag = new Double[m];

            for (int k = 0; k < m; k++)
            {
                // Compute 2-norm of k-th column without under/overflow.
                Double nrm = 0;
                for (int i = k; i < n; i++)
                    nrm = FunctionUtility.Hypotenuse(nrm, qr[i, k]);

                if (nrm != 0)
                {
                    // Form k-th Householder vector.
                    if (qr[k, k] < 0)
                        nrm = -nrm;

                    for (int i = k; i < n; i++)
                        qr[i, k] /= nrm;

                    qr[k, k] += 1;

                    // Apply transformation to remaining columns.
                    for (int j = k + 1; j < m; j++)
                    {
                        Double s = 0;
                        for (int i = k; i < n; i++)
                            s += qr[i, k] * qr[i, j];

                        s = -s / qr[k, k];
                        for (int i = k; i < n; i++)
                            qr[i, j] += s * qr[i, k];
                    }
                }

                this.Rdiag[k] = -nrm;
            }
        }

        /// <summary>Least squares solution of <c>A * X = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Double[,] Solve(Double[,] value)
        {
            if (value == null)
                throw new ArgumentNullException("value", String.Format(LocalizedResources.Instance().MATRIX_CANNOT_BE_NULL, "value"));

            if (value.Rows() != n)
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);

            if (!this.FullRank)
                throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

            // Copy right hand side
            int count = value.Columns();
            var X = value.Copy();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < p; k++)
            {
                for (int j = 0; j < count; j++)
                {
                    Double s = 0;
                    for (int i = k; i < n; i++)
                        s += qr[i, k] * X[i, j];

                    s = -s / qr[k, k];
                    for (int i = k; i < n; i++)
                        X[i, j] += s * qr[i, k];
                }
            }

            // Solve R*X = Y;
            for (int k = p - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                    X[k, j] /= Rdiag[k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < count; j++)
                        X[i, j] -= X[k, j] * qr[i, k];
            }

            return MatrixUtility.Create(p, count, X, transpose: false);
        }

        /// <summary>Least squares solution of <c>X * A = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many columns as <c>A</c> and any number of rows.</param>
        /// <returns>A matrix that minimized the two norm of <c>X * Q * R - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix column dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Double[,] SolveTranspose(Double[,] value)
        {
            if (value == null)
                throw new ArgumentNullException("value", String.Format(LocalizedResources.Instance().MATRIX_CANNOT_BE_NULL, "value"));

            if (value.Columns() != qr.Rows())
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);

            if (!this.FullRank)
                throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

            // Copy right hand side
            int count = value.Rows();
            var X = value.Transpose();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < p; k++)
            {
                for (int j = 0; j < count; j++)
                {
                    Double s = 0;
                    for (int i = k; i < n; i++)
                        s += qr[i, k] * X[i, j];

                    s = -s / qr[k, k];
                    for (int i = k; i < n; i++)
                        X[i, j] += s * qr[i, k];
                }
            }

            // Solve R*X = Y;
            for (int k = m - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                    X[k, j] /= Rdiag[k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < count; j++)
                        X[i, j] -= X[k, j] * qr[i, k];
            }

            return MatrixUtility.Create(count, p, X, transpose: true);
        }

        /// <summary>Least squares solution of <c>A * X = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Double[] Solve(Double[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length != qr.Rows())
                throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);

            if (!this.FullRank)
                throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

            // Copy right hand side
            var X = value.Copy();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < p; k++)
            {
                Double s = 0;
                for (int i = k; i < n; i++)
                    s += qr[i, k] * X[i];

                s = -s / qr[k, k];
                for (int i = k; i < n; i++)
                    X[i] += s * qr[i, k];
            }

            // Solve R*X = Y;
            for (int k = p - 1; k >= 0; k--)
            {
                X[k] /= Rdiag[k];

                for (int i = 0; i < k; i++)
                    X[i] -= X[k] * qr[i, k];
            }

            return X.First(p);
        }

        /// <summary>Shows if the matrix <c>A</c> is of full rank.</summary>
        /// <value>The value is <see langword="true"/> if <c>R</c>, and hence <c>A</c>, has full rank.</value>
        public bool FullRank
        {
            get
            {
                if (this.fullRank.HasValue)
                    return this.fullRank.Value;

                for (int i = 0; i < p; i++)
                    if (this.Rdiag[i] == 0)
                        return (bool)(this.fullRank = false);

                return (bool)(this.fullRank = true);
            }
        }

        /// <summary>Returns the upper triangular factor <c>R</c>.</summary>
        public Double[,] UpperTriangularFactor
        {
            get
            {
                if (this.upperTriangularFactor != null)
                    return this.upperTriangularFactor;

                int rows = economy ? m : n;
                var x = new Double[rows, p];
                x.Fill<Double>(0);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < p; j++)
                    {
                        if (i < j)
                            x[i, j] = qr[i, j];
                        else if (i == j)
                            x[i, j] = Rdiag[i];
                    }
                }

                return this.upperTriangularFactor = x;
            }
        }

        /// <summary>
        ///   Returns the (economy-size) orthogonal factor <c>Q</c>.
        /// </summary>
        public Double[,] OrthogonalFactor
        {
            get
            {
                if (this.orthogonalFactor != null)
                    return this.orthogonalFactor;

                int cols = economy ? m : n;
                var x = new Double[n, cols];
                x.Fill<Double>(0);

                for (int k = cols - 1; k >= 0; k--)
                {
                    for (int i = 0; i < n; i++)
                        x[i, k] = 0;

                    x[k, k] = 1;
                    for (int j = k; j < cols; j++)
                    {
                        if (qr[k, k] != 0)
                        {
                            Double s = 0;
                            for (int i = k; i < n; i++)
                                s += qr[i, k] * x[i, j];

                            s = -s / qr[k, k];
                            for (int i = k; i < n; i++)
                                x[i, j] += s * qr[i, k];
                        }
                    }
                }

                return this.orthogonalFactor = x;
            }
        }


        public Double[,] GetR()
        {
            // R is supposed to be m x n
            int n = qr.GetLength(0);
            int m = qr.GetLength(1);
            var _tmp = new Double[m, n];

            // copy the diagonal from rDiag and the upper triangle of qr
            for (int row = System.Math.Min(m, n) - 1; row >= 0; row--)
            {
                _tmp[row, row] = Rdiag[row];
                for (int col = row + 1; col < n; col++)
                {
                    _tmp[row, col] = qr[col, row];
                }
            }

            // return the cached matrix
            return _tmp;

        }

        public Double[,] GetQ()
        {
            return GetQT().Transpose();
        }

        public Double[,] GetQT()
        {
            // QT is supposed to be m x m
            int n = qr.GetLength(0);
            int m = qr.GetLength(1);
            var _tmp = new Double[m, m];

            /*
             * Q = Q1 Q2 ... Q_m, so Q is formed by first constructing Q_m and then
             * applying the Householder transformations Q_(m-1),Q_(m-2),...,Q1 in
             * succession to the result
             */
            for (int minor = m - 1; minor >= System.Math.Min(m, n); minor--)
            {
                _tmp[minor, minor] = 1.0;
            }

            for (int minor = System.Math.Min(m, n) - 1; minor >= 0; minor--)
            {
                double[] qrtMinor = qr.GetRow(minor);
                _tmp[minor, minor] = 1.0;
                if (qrtMinor[minor] != 0.0)
                {
                    for (int col = minor; col < m; col++)
                    {
                        double alpha = 0;
                        for (int row = minor; row < m; row++)
                        {
                            alpha -= _tmp[col, row] * qrtMinor[row];
                        }
                        alpha /= Rdiag[minor] * qrtMinor[minor];

                        for (int row = minor; row < m; row++)
                        {
                            _tmp[col, row] = -alpha * qrtMinor[row];
                        }
                    }
                }
            }

            // return the cached matrix
            return _tmp;

        }

        public Double[,] GetH()
        {
            int n = qr.GetLength(0);
            int m = qr.GetLength(1);
            var _tmp = new Double[m, n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < System.Math.Min(i + 1, n); ++j)
                {
                    _tmp[i, j] = qr[j, i] / -Rdiag[j];
                }
            }

            // return the cached matrix
            return _tmp;

        }

        /// <summary>
        /// Return raw data
        /// </summary>
        public Double[,] Data
        {
            get { return qr; }
        }

        /// <summary>Returns the diagonal of <c>R</c>.</summary>
        public Double[] Diagonal
        {
            get { return Rdiag; }
        }

        /// <summary>Least squares solution of <c>A * X = I</c></summary>
        public Double[,] Inverse()
        {
            if (!this.FullRank)
                throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

            return Solve(MatrixUtility.Diagonal(n, n, (Double)1));
        }

        /// <summary>
        ///   Reverses the decomposition, reconstructing the original matrix <c>X</c>.
        /// </summary>
        /// 
        public Double[,] Reverse()
        {
            return OrthogonalFactor.Dot(UpperTriangularFactor);
        }

        /// <summary>
        ///   Computes <c>(Xt * X)^1</c> (the inverse of the covariance matrix). This
        ///   matrix can be used to determine standard errors for the coefficients when
        ///   solving a linear set of equations through any of the <see cref="Solve(Double[,])"/>
        ///   methods.
        /// </summary>
        /// 
        public Double[,] GetInformationMatrix()
        {
            var X = Reverse();
            return X.TransposeAndDot(X).Inverse();
        }

        #region ICloneable Members

        private QRDecomposition()
        {
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            var clone = new QRDecomposition();
            clone.qr = qr.Copy();
            clone.Rdiag = Rdiag.Copy();
            clone.n = n;
            clone.p = p;
            clone.m = m;
            clone.economy = economy;
            return clone;
        }

        #endregion
    }
}
