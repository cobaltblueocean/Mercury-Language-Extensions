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
using Mercury.Language.Exceptions;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Mercury.Language.Math.Analysis.Solver;
using Mercury.Language.Math.Matrix;
using Mercury.Language;

namespace Mercury.Language.Math.LinearAlgebra
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
    public sealed class QRDecomposition : ICloneable, IQRDecomposition
    {
        private int n;
        private int m;
        private int p;
        private bool economy;

        private Double[,] qrt;
        private Double[] rDiag;

        // cache for lazy evaluation
        private bool? fullRank;
        private Double[,] orthogonalFactor;
        private Double[,] upperTriangularFactor;


        /** Cached value of Q. */
        private Matrix<double> cachedQ;

        /** Cached value of QT. */
        private Matrix<double> cachedQT;

        /** Cached value of R. */
        private Matrix<double> cachedR;

        /** Cached value of H. */
        private Matrix<double> cachedH;


        /// <summary>Constructs a QR decomposition.</summary>    
        /// <param name="value">The matrix A to be decomposed.</param>
        /// <param name="transpose">True if the decomposition should be performed on
        /// the transpose of A rather than A itself, false otherwise. Default is false.</param>
        /// <param name="inPlace">True if the decomposition should be done in place,
        /// overriding the given matrix <paramref name="value"/>. Default is false.</param>
        /// <param name="economy">True to perform the economy decomposition, where only
        ///.the information needed to solve linear systems is computed. If set to false,
        /// the full QR decomposition will be computed.</param>
        public QRDecomposition(Double[,] value) : this(Matrix.MathNetMatrixUtility.CreateMatrix(value))
        {

        }

        public QRDecomposition(Matrix<double> matrix)
        {

            m = matrix.RowCount;
            n = matrix.ColumnCount;
            qrt = matrix.Transpose().ToArray();
            rDiag = new double[System.Math.Min(m, n)];
            p = rDiag.Length;
            cachedQ = null;
            cachedQT = null;
            cachedR = null;
            cachedH = null;

            /*
             * The QR decomposition of a matrix A is calculated using Householder
             * reflectors by repeating the following operations to each minor
             * A(minor,minor) of A:
             */
            for (int minor = 0; minor < System.Math.Min(m, n); minor++)
            {

                double[] qrtMinor = qrt.GetRow(minor);

                /*
                 * Let x be the first column of the minor, and a^2 = |x|^2.
                 * x will be in the positions qr[minor][minor] through qr[m][minor].
                 * The first column of the transformed minor will be (a,0,0,..)'
                 * The sign of a is chosen to be opposite to the sign of the first
                 * component of x. Let's find a:
                 */
                double xNormSqr = 0;
                for (int row = minor; row < m; row++)
                {
                    double c = qrtMinor[row];
                    xNormSqr += c * c;
                }
                double a = (qrtMinor[minor] > 0) ? -System.Math.Sqrt(xNormSqr) : System.Math.Sqrt(xNormSqr);
                rDiag[minor] = a;

                if (a != 0.0)
                {

                    /*
                     * Calculate the normalized reflection vector v and transform
                     * the first column. We know the norm of v beforehand: v = x-ae
                     * so |v|^2 = <x-ae,x-ae> = <x,x>-2a<x,e>+a^2<e,e> =
                     * a^2+a^2-2a<x,e> = 2a*(a - <x,e>).
                     * Here <x, e> is now qr[minor][minor].
                     * v = x-ae is stored in the column at qr:
                     */
                    qrtMinor[minor] -= a; // now |v|^2 = -2a*(qr[minor][minor])
                    qrt[minor, minor] -= a;

                    /*
                     * Transform the rest of the columns of the minor:
                     * They will be transformed by the matrix H = I-2vv'/|v|^2.
                     * If x is a column vector of the minor, then
                     * Hx = (I-2vv'/|v|^2)x = x-2vv'x/|v|^2 = x - 2<x,v>/|v|^2 v.
                     * Therefore the transformation is easily calculated by
                     * subtracting the column vector (2<x,v>/|v|^2)v from x.
                     *
                     * Let 2<x,v>/|v|^2 = alpha. From above we have
                     * |v|^2 = -2a*(qr[minor][minor]), so
                     * alpha = -<x,v>/(a*qr[minor][minor])
                     */
                    for (int col = minor + 1; col < n; col++)
                    {
                        double[] qrtCol = qrt.GetRow(col);
                        double alpha = 0;
                        for (int row = minor; row < m; row++)
                        {
                            alpha -= qrtCol[row] * qrtMinor[row];
                        }
                        alpha /= a * qrtMinor[minor];

                        // Subtract the column vector alpha*v from x.
                        for (int row = minor; row < m; row++)
                        {
                            qrtCol[row] -= alpha * qrtMinor[row];
                            qrt[col, row] = qrtCol[row];
                        }
                    }
                }
            }
        }


        /// <summary>Least squares solution of <c>A * X = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Double[,] Solve(Double[,] value)
        {
            return GetSolver().Solve(value);
        }

        /// <summary>Least squares solution of <c>X * A = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many columns as <c>A</c> and any number of rows.</param>
        /// <returns>A matrix that minimized the two norm of <c>X * Q * R - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix column dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Double[,] SolveTranspose(Double[,] value)
        {
            return ((Solver)GetSolver()).SolveTranspose(value);
        }

        /// <summary>Least squares solution of <c>A * X = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Double[] Solve(Double[] value)
        {
            #region Old Code
            //if (value == null)
            //    throw new ArgumentNullException("value");

            //if (value.Length != qrt.Rows())
            //    throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);

            //if (!this.FullRank)
            //    throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

            //// Copy right hand side
            //var X = value.Copy();

            //// Compute Y = transpose(Q)*B
            //for (int k = 0; k < p; k++)
            //{
            //    Double s = 0;
            //    for (int i = k; i < n; i++)
            //        s += qrt[i, k] * X[i];

            //    s = -s / qrt[k, k];
            //    for (int i = k; i < n; i++)
            //        X[i] += s * qrt[i, k];
            //}

            //// Solve R*X = Y;
            //for (int k = p - 1; k >= 0; k--)
            //{
            //    X[k] /= rDiag[k];

            //    for (int i = 0; i < k; i++)
            //        X[i] -= X[k] * qrt[i, k];
            //}

            //return X.First(p);
            #endregion

            return GetSolver().Solve(value);
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
                    if (this.rDiag[i] == 0)
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
                            x[i, j] = qrt[i, j];
                        else if (i == j)
                            x[i, j] = rDiag[i];
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
                        if (qrt[k, k] != 0)
                        {
                            Double s = 0;
                            for (int i = k; i < n; i++)
                                s += qrt[i, k] * x[i, j];

                            s = -s / qrt[k, k];
                            for (int i = k; i < n; i++)
                                x[i, j] += s * qrt[i, k];
                        }
                    }
                }

                return this.orthogonalFactor = x;
            }
        }


        public Matrix<Double> GetR()
        {
            if (cachedR == null)
            {
                // R is supposed to be m x n
                int n = qrt.GetLength(0);
                int m = qrt.GetLength(1);
                cachedR = Matrix.MathNetMatrixUtility.CreateMatrix(m, n);

                // copy the diagonal from rDiag and the upper triangle of qr
                for (int row = System.Math.Min(m, n) - 1; row >= 0; row--)
                {
                    cachedR[row, row] = rDiag[row];
                    for (int col = row + 1; col < n; col++)
                    {
                        cachedR[row, col] = qrt[col, row];
                    }
                }
            }

            // return the cached matrix
            return cachedR;

        }

        public Matrix<Double> GetQ()
        {
            if (cachedQ == null)
            {
                cachedQ = Matrix.MathNetMatrixUtility.CreateMatrix(GetQT().AsArrayEx().Transpose());
            }
            return cachedQ;
        }

        public Matrix<Double> GetQT()
        {
            if (cachedQT == null)
            {

                // QT is supposed to be m x m
                int n = qrt.GetLength(0);
                int m = qrt.GetLength(1);
                //var _tmp = new Double[m, m];

                cachedQT = Matrix.MathNetMatrixUtility.CreateMatrix(m, m);

                /*
                 * Q = Q1 Q2 ... Q_m, so Q is formed by first constructing Q_m and then
                 * applying the Householder transformations Q_(m-1),Q_(m-2),...,Q1 in
                 * succession to the result
                 */
                for (int minor = m - 1; minor >= System.Math.Min(m, n); minor--)
                {
                    cachedQT[minor, minor] = 1.0;
                }

                for (int minor = System.Math.Min(m, n) - 1; minor >= 0; minor--)
                {
                    double[] qrtMinor = qrt.GetRow(minor);
                    cachedQT[minor, minor] = 1.0;
                    if (qrtMinor[minor] != 0.0)
                    {
                        for (int col = minor; col < m; col++)
                        {
                            double alpha = 0;
                            for (int row = minor; row < m; row++)
                            {
                                alpha -= cachedQT[col, row] * qrtMinor[row];
                            }
                            alpha /= rDiag[minor] * qrtMinor[minor];

                            for (int row = minor; row < m; row++)
                            {
                                cachedQT.AddToEntry(col, row, -alpha * qrtMinor[row]);
                            }
                        }
                    }
                }
            }

            // return the cached matrix
            return cachedQT;
        }

        public Matrix<Double> GetH()
        {
            if (cachedH == null)
            {

                int n = qrt.GetLength(0);
                int m = qrt.GetLength(1);
                cachedH = Matrix.MathNetMatrixUtility.CreateMatrix(m, n);
                for (int i = 0; i < m; ++i)
                {
                    for (int j = 0; j < System.Math.Min(i + 1, n); ++j)
                    {
                        cachedH[i, j] = qrt[j, i] / -rDiag[j];
                    }
                }
            }

            // return the cached matrix
            return cachedH;

        }

        /// <summary>
        /// Return raw data
        /// </summary>
        public Double[,] Data
        {
            get { return qrt; }
        }

        /// <summary>Returns the diagonal of <c>R</c>.</summary>
        public Double[] Diagonal
        {
            get { return rDiag; }
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

        public IDecompositionSolver GetSolver()
        {
            return new Solver(qrt, rDiag, m, n, FullRank);
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
            clone.qrt = qrt.Copy();
            clone.rDiag = rDiag.Copy();
            clone.n = n;
            clone.p = p;
            clone.m = m;
            clone.economy = economy;
            return clone;
        }

        #endregion


        /// <summary>
        /// Specialized solver.
        /// </summary>
        private class Solver : IDecompositionSolver
        {
            private int n;
            private int m;
            private int p;

            /**
             * A packed TRANSPOSED representation of the QR decomposition.
             * <p>The elements BELOW the diagonal are the elements of the UPPER triangular
             * matrix R, and the rows ABOVE the diagonal are the Householder reflector vectors
             * from which an explicit form of Q can be recomputed if desired.</p>
             */
            private double[,] qrt;

            /** The diagonal elements of R. */
            private double[] rDiag;

            private bool _fullRank;

            /**
             * Build a solver from decomposed matrix.
             * @param qrt packed TRANSPOSED representation of the QR decomposition
             * @param rDiag diagonal elements of R
             */
            public Solver(double[,] qrt, double[] rDiag, int rows, int cols, bool fullRank)
            {
                m = rows;
                n = cols;

                this.qrt = qrt;
                this.rDiag = rDiag;
                p = rDiag.Length;
                _fullRank = fullRank;
            }

            public bool IsNonSingular
            {
                get
                {
                    foreach (double diag in rDiag)
                    {
                        if (diag == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            public Matrix<double> GetInverse()
            {
                return Solve(MathNetMatrixUtility.CreateRealIdentityMatrix(rDiag.Length));
            }

            /// <summary>Least squares solution of <c>A * X = I</c></summary>
            public Double[,] Inverse()
            {
                if (!this._fullRank)
                    throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

                return Solve(MatrixUtility.Diagonal(n, n, (Double)1));
            }

            public double[] Solve(double[] b)
            {
                int n = qrt.Rows();
                int m = qrt.GetMaxColumnLength();
                if (b.Length != m)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().VECTOR_LENGTH_MISMATCH, b.Length, m));
                }
                if (!IsNonSingular)
                {
                    throw new SingularMatrixException();
                }

                double[] x = new double[n];
                double[] y = b.Copy();

                // apply Householder transforms to solve Q.y = b
                for (int minor = 0; minor < System.Math.Min(m, n); minor++)
                {

                    double[] qrtMinor = qrt.GetRow(minor);
                    double dotProduct = 0;
                    for (int row = minor; row < m; row++)
                    {
                        dotProduct += y[row] * qrtMinor[row];
                    }
                    dotProduct /= rDiag[minor] * qrtMinor[minor];

                    for (int row = minor; row < m; row++)
                    {
                        y[row] += dotProduct * qrtMinor[row];
                    }

                }

                // solve triangular system R.x = y
                for (int row = rDiag.Length - 1; row >= 0; --row)
                {
                    y[row] /= rDiag[row];
                    double yRow = y[row];
                    double[] qrtRow = qrt.GetRow(row);
                    x[row] = yRow;
                    for (int i = 0; i < row; i++)
                    {
                        y[i] -= yRow * qrtRow[i];
                    }
                }

                return x;
            }

            public Vector<double> Solve(Vector<double> b)
            {
                return MathNetMatrixUtility.CreateRealVector(Solve(b.AsArrayEx()));
            }

            public Matrix<double> Solve(Matrix<double> value)
            {
                return MathNetMatrixUtility.CreateMatrix<Double>(Solve(value.AsArrayEx()));
            }

            public double[][] Solve(double[][] value)
            {
                int n = qrt.Rows();
                int m = qrt.GetMaxColumnLength();
                int p = rDiag.Length;

                if (value == null)
                    throw new ArgumentNullException("value", String.Format(LocalizedResources.Instance().MATRIX_CANNOT_BE_NULL, "value"));

                if (value.Length != n)
                    throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);

                if (!this._fullRank)
                    throw new InvalidOperationException(LocalizedResources.Instance().MATRIX_IS_RANK_DEFICIENT);

                // Copy right hand side
                int count = value.GetMaxColumnLength();
                var X = value.Copy();

                // Compute Y = transpose(Q)*B
                for (int k = 0; k < p; k++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        Double s = 0;
                        for (int i = k; i < n; i++)
                            s += qrt[i, k] * X[i][j];

                        s = -s / qrt[k, k];
                        for (int i = k; i < n; i++)
                            X[i][j] += s * qrt[i, k];
                    }
                }

                // Solve R*X = Y;
                for (int k = p - 1; k >= 0; k--)
                {
                    for (int j = 0; j < count; j++)
                        X[k][j] /= rDiag[k];

                    for (int i = 0; i < k; i++)
                        for (int j = 0; j < count; j++)
                            X[i][j] -= X[k][j] * qrt[i, k];
                }

                return X;
            }

            public double[,] Solve(double[,] value)
            {
                return Solve(value.ToJagged()).ToMultidimensional();
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

                if (value.Columns() != qrt.Rows())
                    throw new ArgumentException(LocalizedResources.Instance().MATRIX_ROW_DIMENSIONS_MUST_AGREE);

                if (!this._fullRank)
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
                            s += qrt[i, k] * X[i, j];

                        s = -s / qrt[k, k];
                        for (int i = k; i < n; i++)
                            X[i, j] += s * qrt[i, k];
                    }
                }

                // Solve R*X = Y;
                for (int k = m - 1; k >= 0; k--)
                {
                    for (int j = 0; j < count; j++)
                        X[k, j] /= rDiag[k];

                    for (int i = 0; i < k; i++)
                        for (int j = 0; j < count; j++)
                            X[i, j] -= X[k, j] * qrt[i, k];
                }

                return MatrixUtility.Create(count, p, X, transpose: true);
            }
        }
    }
}
