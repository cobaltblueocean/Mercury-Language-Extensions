// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by Apache Software Foundation (ASF).
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Mercury.Language;
using Mercury.Language.Exception;
using Mercury.Language.Math.Matrix;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.LinearAlgebra
{
    /// <summary>
    /// Calculates the Cholesky decomposition of a matrix.
    /// <p>The Cholesky decomposition of a real symmetric positive-definite
    /// matrix A consists of a lower triangular matrix L with same size that
    /// satisfy: A = LL<sup>T</sup>Q = I)d In a sense, this is the square root of A.</p>
    /// 
    /// </summary>
    /// <see cref="<a">href="http://mathworld.wolfram.com/CholeskyDecomposition.html">MathWorld</a> </see>
    /// <see cref="<a">href="http://en.wikipedia.org/wiki/Cholesky_decomposition">Wikipedia</a> </see>
    /// @version $Revision: 990655 $ $Date: 2010-08-29 23:49:40 +0200 (dimd 29 août 2010) $
    /// @since 2.0
    [Serializable]
    public sealed class CholeskyDecomposition : ICholeskyDecomposition<Double>
    {
        /// <summary>Default threshold above which off-diagonal elements are considered too different
        /// and matrix not symmetricd /// </summary>
        public static double DEFAULT_RELATIVE_SYMMETRY_THRESHOLD = 1.0e-15;

        /// <summary>Default threshold below which diagonal elements are considered null
        /// and matrix not positive definited /// </summary>
        public static double DEFAULT_ABSOLUTE_POSITIVITY_THRESHOLD = 1.0e-10;

        /// <summary>Row-oriented storage for L<sup>T</sup> matrix datad */
        private double[][] lTData;

        /// <summary>Cached value of Ld */
        private Matrix<Double> cachedL;

        /// <summary>Cached value of LTd */
        private Matrix<Double> cachedLT;

        private Double[,] L;
        private Double[] D;
        private int n;

        private bool positiveDefinite;
        private bool destroyed;
        private bool robust;
        private bool undefined;

        // cache for lazy evaluation
        private Double[,] leftTriangularFactor;
        private Double[,] upperTriangularFactor;
        private Double[,] diagonalMatrix;
        private Double? determinant;
        private double? lndeterminant;
        private bool? nonsingular;

        /// <summary>
        /// Calculates the Cholesky decomposition of the given matrix.
        /// <p>
        /// Calling this constructor is equivalent to call {@link
        /// #CholeskyDecompositionImpl(Matrix<Double>, double, double)} with the
        /// thresholds set to the default values {@link
        /// #DEFAULT_RELATIVE_SYMMETRY_THRESHOLD} and {@link
        /// #DEFAULT_ABSOLUTE_POSITIVITY_THRESHOLD}
        /// </p>
        /// </summary>
        /// <param Name="matrix">the matrix to decompose</param>
        /// <exception cref="NonSquareMatrixException">if matrix is not square </exception>
        /// <exception cref="NotSymmetricMatrixException">if matrix is not symmetric </exception>
        /// <exception cref="NotPositiveDefiniteMatrixException">if the matrix is not </exception>
        /// strictly positive definite
        /// <see cref="#CholeskyDecompositionImpl(Matrix<Double>,">double, double) </see>
        /// <see cref="#DEFAULT_RELATIVE_SYMMETRY_THRESHOLD"></see>
        /// <see cref="#DEFAULT_ABSOLUTE_POSITIVITY_THRESHOLD"></see>
        public CholeskyDecomposition(Matrix<Double> matrix) : this(matrix, DEFAULT_RELATIVE_SYMMETRY_THRESHOLD,
             DEFAULT_ABSOLUTE_POSITIVITY_THRESHOLD)
        {

        }

        /// <summary>
        /// Calculates the Cholesky decomposition of the given matrix.
        /// </summary>
        /// <param Name="matrix">the matrix to decompose</param>
        /// <param Name="relativeSymmetryThreshold">threshold above which off-diagonal</param>
        /// elements are considered too different and matrix not symmetric
        /// <param Name="absolutePositivityThreshold">threshold below which diagonal</param>
        /// elements are considered null and matrix not positive definite
        /// <exception cref="NonSquareMatrixException">if matrix is not square </exception>
        /// <exception cref="NotSymmetricMatrixException">if matrix is not symmetric </exception>
        /// <exception cref="NotPositiveDefiniteMatrixException">if the matrix is not </exception>
        /// strictly positive definite
        /// <see cref="#CholeskyDecompositionImpl(Matrix<Double>)"></see>
        /// <see cref="#DEFAULT_RELATIVE_SYMMETRY_THRESHOLD"></see>
        /// <see cref="#DEFAULT_ABSOLUTE_POSITIVITY_THRESHOLD"></see>
        public CholeskyDecomposition(Matrix<Double> matrix, double relativeSymmetryThreshold, double absolutePositivityThreshold)
        {

            if (!matrix.IsSquare())
            {
                throw new NonSquareMatrixException(matrix.RowCount, matrix.ColumnCount);
            }

            int order = matrix.RowCount;
            Double[,] value = matrix.AsArrayEx();
            lTData = matrix.AsArrayEx().ToJagged();
            cachedL = null;
            cachedLT = null;

            // check the matrix before transformation
            for (int i = 0; i < order; ++i)
            {

                double[] lI = lTData[i];

                // check off-diagonal elements (and reset them to 0)
                for (int j = i + 1; j < order; ++j)
                {
                    double[] lJ = lTData[j];
                    double lIJ = lI[j];
                    double lJI = lJ[i];
                    double maxDelta =
                        relativeSymmetryThreshold * System.Math.Max(Math2.Abs(lIJ), Math2.Abs(lJI));
                    if (Math2.Abs(lIJ - lJI) > maxDelta)
                    {
                        throw new NonSymmetricMatrixException(String.Format(LocalizedResources.Instance().NON_SYMMETRIC_MATRIX, i, j, Math2.Abs(lIJ - lJI) - maxDelta));
                    }
                    lJ[i] = 0;
                }
            }

            // transform the matrix
            for (int i = 0; i < order; ++i)
            {
                double[] ltI = lTData[i];

                // check diagonal element
                if (ltI[i] < absolutePositivityThreshold)
                {
                    throw new NonPositiveDefiniteMatrixException();
                }

                lTData[i][i] = Math2.SquareRoot(lTData[i][i]);
                double inverse = 1.0 / ltI[i];

                for (int q = order - 1; q > i; --q)
                {
                    ltI[q] *= inverse;
                    double[] ltQ = lTData[q];
                    for (int p = q; p < order; ++p)
                    {
                        ltQ[p] -= ltI[q] * ltI[p];
                        lTData[q][p] = ltQ[p];
                    }
                }
            }

            this.n = value.Rows();
            this.L = value.ToUpperTriangular(MatrixType.UpperTriangular, result: value);
        }

        /// <summary>{@inheritDoc} */
        public Matrix<Double> GetL()
        {
            if (cachedL == null)
            {
                cachedL = GetLT().Transpose();
            }
            return cachedL;
        }

        /// <summary>{@inheritDoc} */
        public Matrix<Double> GetLT()
        {

            if (cachedLT == null)
            {
                cachedLT = Matrix.MatrixUtility.CreateMatrix(lTData);
            }

            // return the cached matrix
            return cachedLT;

        }

        /// <summary>{@inheritDoc} */
        public double Determinant
        {
            get
            {
                double determinant = 1.0;
                for (int i = 0; i < lTData.Length; ++i)
                {
                    double lTii = lTData[i][i];
                    determinant *= lTii * lTii;
                }
                return determinant;
            }
        }

        /// <summary>
        ///   Gets whether the decomposed matrix was positive definite.
        /// </summary>
        ///
        public bool IsPositiveDefinite
        {
            get { return this.positiveDefinite; }
        }

        /// <summary>
        ///   Gets a value indicating whether the LDLt factorization
        ///   has been computed successfully or if it is undefined.
        /// </summary>
        /// 
        /// <value>
        ///     <c>true</c> if the factorization is not defined; otherwise, <c>false</c>.
        /// </value>
        /// 
        public bool IsUndefined
        {
            get { return this.undefined; }
        }

        /// <summary>
        ///   Gets the left (lower) triangular factor
        ///   <c>L</c> so that <c>A = L * D * L'</c>.
        /// </summary>
        /// 
        public Double[,] LeftTriangularFactor
        {
            get
            {
                if (leftTriangularFactor == null)
                {
                    if (destroyed)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

                    if (undefined)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

                    leftTriangularFactor = L.GetLowerTriangle();
                }

                return leftTriangularFactor;
            }
        }

        /// <summary>
        ///   Gets the upper triangular factor
        /// </summary>
        /// 
        public Double[,] UpperTriangularFactor
        {
            get
            {
                if (upperTriangularFactor == null)
                {
                    if (destroyed)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

                    if (undefined)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

                    upperTriangularFactor = L.GetUpperTriangle();
                }

                return upperTriangularFactor;
            }
        }

        /// <summary>
        ///   Gets the block diagonal matrix of diagonal elements in a LDLt decomposition.
        /// </summary>        
        ///   
        public Double[,] DiagonalMatrix
        {
            get
            {
                if (diagonalMatrix == null)
                {
                    if (destroyed)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

                    diagonalMatrix = Mercury.Language.Math.Matrix.MatrixUtility.Diagonal<Double>(D);
                }

                return diagonalMatrix;
            }
        }

        /// <summary>
        ///   Gets the one-dimensional array of diagonal elements in a LDLt decomposition.
        /// </summary>        
        /// 
        public Double[] Diagonal
        {
            get { return D; }
        }

        /// <summary>
        ///   If the matrix is positive-definite, gets the
        ///   log-determinant of the decomposed matrix.
        /// </summary>
        /// 
        public double LogDeterminant
        {
            get
            {
                if (!lndeterminant.HasValue)
                {
                    if (destroyed)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

                    if (undefined)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

                    double detL = 0, detD = 0;
                    for (int i = 0; i < n; i++)
                        detL += System.Math.Log((double)L[i, i]);

                    if (D != null)
                    {
                        for (int i = 0; i < D.Length; i++)
                            detD += System.Math.Log((double)D[i]);
                    }

                    lndeterminant = detL + detL + detD;
                }

                return lndeterminant.Value;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the decomposed
        ///   matrix is non-singular (i.e. invertible).
        /// </summary>
        /// 
        public bool Nonsingular
        {
            get
            {
                if (!nonsingular.HasValue)
                {
                    if (destroyed)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

                    if (undefined)
                        throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

                    bool nonSingular = true;
                    for (int i = 0; i < n && nonSingular; i++)
                        if (L[i, i] == 0 || D[i] == 0) nonSingular = false;

                    nonsingular = nonSingular;
                }

                return nonsingular.Value;
            }
        }

        // Original
        // private unsafe void LLt()
        private void LLt()
        {
            D = Ones<Double>(n);

            this.positiveDefinite = true;

            for (int j = 0; j < n; j++)
            {
                Double s = 0;
                for (int k = 0; k < j; k++)
                {
                    Double t = L[k, j];
                    for (int i = 0; i < k; i++)
                        t -= L[j, i] * L[k, i];
                    t = t / L[k, k];

                    L[j, k] = t;
                    s += t * t;
                }

                s = L[j, j] - s;

                // Use a tolerance for positive-definiteness
                this.positiveDefinite &= (s > (Double)1e-14 * System.Math.Abs(L[j, j]));

                L[j, j] = (Double)System.Math.Sqrt((double)s);
            }
        }


        // Original 
        // private unsafe void LDLt()
        private void LDLt()
        {
            D = new Double[n];

            this.positiveDefinite = true;

            Double[] v = new Double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                    v[j] = L[i, j] * D[j];

                Double d = 0;
                for (int k = 0; k < i; k++)
                    d += L[i, k] * v[k];

                d = D[i] = v[i] = L[i, i] - d;

                // Use a tolerance for positive-definiteness
                this.positiveDefinite &= (v[i] > (Double)1e-14 * System.Math.Abs(L[i, i]));

                // If one of the diagonal elements is zero, the 
                // decomposition (without pivoting) is undefined.
                if (v[i] == 0) { undefined = true; return; }

                Parallel.For(i + 1, n, k =>
                {
                    Double s = 0;
                    for (int j = 0; j < i; j++)
                        s += L[k, j] * v[j];

                    L[k, i] = (L[i, k] - s) / d;
                });
            }

            for (int i = 0; i < n; i++)
                L[i, i] = 1;
        }

        public double[,] Solve(double[,] value)
        {
            return GetSolver().Solve(value);
        }

        public double[] Solve(double[] value)
        {
            return GetSolver().Solve(value);
        }

        public double[,] Inverse()
        {
            return Solve(Mercury.Language.Math.Matrix.MatrixUtility.Identity<Double>(n));
        }

        /// <summary>
        ///   Computes the diagonal of the inverse of the decomposed matrix.
        /// </summary>
        /// 
        public Double[] InverseDiagonal(bool destroy = false)
        {
            return InverseDiagonal(new Double[n], destroy);
        }

        /// <summary>
        ///   Computes the diagonal of the inverse of the decomposed matrix.
        /// </summary>
        ///
        /// <param name="destroy">True to conserve memory by reusing the
        ///    same space used to hold the decomposition, thus destroying
        ///    it in the process. Pass false otherwise.</param>
        /// <param name="result">The array to hold the result of the 
        ///    computation. Should be of same length as the the diagonal
        ///    of the original matrix.</param>
        /// 
        public Double[] InverseDiagonal(Double[] result, bool destroy = false)
        {
            if (!robust && !positiveDefinite)
                throw new NonPositiveDefiniteMatrixException(LocalizedResources.Instance().MATRIX_IS_NOT_POSITIVE_DEFINITE);

            if (undefined)
                throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

            if (destroyed)
                throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

            Double[,] S;

            if (destroy)
            {
                S = L; destroyed = true;
            }
            else
            {
                S = Zeros<Double>(n, n);
            }

            // References:
            // http://books.google.com/books?id=myzIPBwyBbcC&pg=PA119

            // Compute the inverse S of the lower triangular matrix L
            // and store in place of the upper triangular part of S.

            for (int j = n - 1; j >= 0; j--)
            {
                S[j, j] = 1 / L[j, j];
                for (int i = j - 1; i >= 0; i--)
                {
                    Double sum = 0;
                    for (int k = i + 1; k <= j; k++)
                        sum += L[k, i] * S[k, j];
                    S[i, j] = -sum / L[i, i];
                }
            }

            // Compute the 2-norm squared of the rows
            // of the upper (right) triangular matrix S.
            if (robust)
            {
                for (int i = 0; i < n; i++)
                {
                    Double sum = 0;
                    for (int j = i; j < n; j++)
                        sum += S[i, j] * S[i, j] / D[j];
                    result[i] = sum;
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    Double sum = 0;
                    for (int j = i; j < n; j++)
                        sum += S[i, j] * S[i, j];
                    result[i] = sum;
                }
            }

            return result;
        }

        /// <summary>
        ///   Computes the trace of the inverse of the decomposed matrix.
        /// </summary>
        ///
        /// <param name="destroy">True to conserve memory by reusing the
        ///    same space used to hold the decomposition, thus destroying
        ///    it in the process. Pass false otherwise.</param>
        /// 
        public Double InverseTrace(bool destroy = false)
        {
            if (!robust && !positiveDefinite)
                throw new NonPositiveDefiniteMatrixException(LocalizedResources.Instance().MATRIX_IS_NOT_POSITIVE_DEFINITE);

            if (undefined)
                throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

            if (destroyed)
                throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

            Double[,] S;

            if (destroy)
            {
                S = L; destroyed = true;
            }
            else
            {
                S = Zeros<Double>(n, n);
            }

            // References:
            // http://books.google.com/books?id=myzIPBwyBbcC&pg=PA119

            // Compute the inverse S of the lower triangular matrix L
            // and store in place of the upper triangular part of S.

            for (int j = n - 1; j >= 0; j--)
            {
                S[j, j] = 1 / L[j, j];
                for (int i = j - 1; i >= 0; i--)
                {
                    Double sum = 0;
                    for (int k = i + 1; k <= j; k++)
                        sum += L[k, i] * S[k, j];
                    S[i, j] = -sum / L[i, i];
                }
            }

            // Compute the 2-norm squared of the rows
            // of the upper (right) triangular matrix S.
            Double trace = 0;

            if (robust)
            {
                for (int i = 0; i < n; i++)
                    for (int j = i; j < n; j++)
                        trace += S[i, j] * S[i, j] / D[j];
            }
            else
            {
                for (int i = 0; i < n; i++)
                    for (int j = i; j < n; j++)
                        trace += S[i, j] * S[i, j];
            }

            return trace;
        }

        public double[,] GetInformationMatrix()
        {
            var X = Reverse();
            return X.TransposeAndDot(X).Inverse();
        }

        public double[,] Reverse()
        {
            if (destroyed)
                throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_DESTROYED);

            if (undefined)
                throw new InvalidOperationException(LocalizedResources.Instance().DECOMPOSITION_UNDEFINED);

            if (robust)
                return LeftTriangularFactor.Dot(DiagonalMatrix).DotWithTransposed(LeftTriangularFactor);
            return LeftTriangularFactor.DotWithTransposed(LeftTriangularFactor);
        }

        /// <summary>
        ///   Creates a new Cholesky decomposition directly from
        ///   an already computed left triangular matrix <c>L</c>.
        /// </summary>
        /// <param name="leftTriangular">The left triangular matrix from a Cholesky decomposition.</param>
        /// 
        public static CholeskyDecomposition FromLeftTriangularMatrix(Double[,] leftTriangular)
        {
            var chol = new CholeskyDecomposition();
            chol.n = leftTriangular.Rows();
            chol.L = leftTriangular;
            chol.positiveDefinite = true;
            chol.robust = false;
            chol.D = Ones<Double>(chol.n);
            return chol;
        }

        public static T[,] Zeros<T>(int rows, int columns)
        {
            return new T[rows, columns];
        }

        public static T[] Ones<T>(int size) where T : struct
        {
            return Create(size, 1.To<T>());
        }

        public static T[] Create<T>(int size, T value)
        {
            var v = new T[size];
            for (int i = 0; i < v.Length; i++)
                v[i] = value;
            return v;
        }

        /// <summary>{@inheritDoc} */
        public IDecompositionSolver GetSolver()
        {
            return new Solver(lTData);
        }

        #region ICloneable Members

        private CholeskyDecomposition()
        {
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        /// 
        public object Clone()
        {
            return new CholeskyDecomposition(Matrix.MatrixUtility.CreateMatrix(lTData));
        }
        #endregion

        /// <summary>
        /// Specialized solver.
        /// </summary>
        private class Solver : IDecompositionSolver
        {

            /// <summary>Row-oriented storage for L<sup>T</sup> matrix datad */
            private double[][] lTData;

            /// <summary>
            /// Build a solver from decomposed matrix.
            /// </summary>
            /// <param Name="lTData">row-oriented storage for L<sup>T</sup> matrix data</param>
            public Solver(double[][] _lTData)
            {
                this.lTData = _lTData;
            }

            /// <summary>{@inheritDoc} */
            public Boolean IsNonSingular
            {
                // if we get this far, the matrix was positive definite, hence non-singular
                get { return true; }
            }

            /// <summary>{@inheritDoc} */
            public double[] Solve(double[] b)
            {

                int m = lTData.Length;
                if (b.Length != m)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().VECTOR_LENGTH_MISMATCH, b.Length, m));
                }

                double[] x = b.CloneExact();

                // Solve LY = b
                for (int j = 0; j < m; j++)
                {
                    double[] lJ = lTData[j];
                    x[j] /= lJ[j];
                    double xJ = x[j];
                    for (int i = j + 1; i < m; i++)
                    {
                        x[i] -= xJ * lJ[i];
                    }
                }

                // Solve LTX = Y
                for (int j = m - 1; j >= 0; j--)
                {
                    x[j] /= lTData[j][j];
                    double xJ = x[j];
                    for (int i = 0; i < j; i++)
                    {
                        x[i] -= xJ * lTData[i][j];
                    }
                }

                return x;

            }

            /// <summary>{@inheritDoc} */
            public Vector<Double> Solve(Vector<Double> b)
            {
                try
                {
                    return Solve((DenseVector)b);
                }
                catch (InvalidCastException cce)
                {

                    int m = lTData.Length;
                    if (b.Count != m)
                    {
                        throw new ArgumentException(String.Format(LocalizedResources.Instance().VECTOR_LENGTH_MISMATCH, b.Count, m));
                    }

                    double[] x = b.AsArrayEx();

                    // Solve LY = b
                    for (int j = 0; j < m; j++)
                    {
                        double[] lJ = lTData[j];
                        x[j] /= lJ[j];
                        double xJ = x[j];
                        for (int i = j + 1; i < m; i++)
                        {
                            x[i] -= xJ * lJ[i];
                        }
                    }

                    // Solve LTX = Y
                    for (int j = m - 1; j >= 0; j--)
                    {
                        x[j] /= lTData[j][j];
                        double xJ = x[j];
                        for (int i = 0; i < j; i++)
                        {
                            x[i] -= xJ * lTData[i][j];
                        }
                    }

                    return MatrixUtility.CreateRealVector(x);

                }
            }

            /// <summary>Solve the linear equation A &times; X = B.
            /// <p>The A matrix is implicit hered It is </p>
            /// </summary>
            /// <param Name="b">right-hand side of the equation A &times; X = B</param>
            /// <returns>a vector X such that A &times; X = B</returns>
            /// <exception cref="ArgumentException">if matrices dimensions don't match </exception>
            /// <exception cref="InvalidMatrixException">if decomposed matrix is singular </exception>
            public DenseVector Solve(DenseVector b)
            {
                return (DenseVector)MatrixUtility.CreateRealVector(Solve(b.AsArrayEx()));
            }

            /// <summary>{@inheritDoc} */
            public Matrix<Double> Solve(Matrix<Double> value)
            {

                return MatrixUtility.CreateMatrix<Double>(Solve(value.AsArrayEx()));

            }

            public double[][] Solve(double[][] value)
            {
                int m = lTData.Length;
                if (value.Length != m)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_2x2, value.Length, value.GetMaxColumnLength(), m, "n"));
                }

                int nColB = value.GetMaxColumnLength();
                double[][] x = value.Copy();

                // Solve LY = b
                for (int j = 0; j < m; j++)
                {
                    double[] lJ = lTData[j];
                    double lJJ = lJ[j];
                    double[] xJ = x[j];
                    for (int k = 0; k < nColB; ++k)
                    {
                        xJ[k] /= lJJ;
                    }
                    for (int i = j + 1; i < m; i++)
                    {
                        double[] xI = x[i];
                        double lJI = lJ[i];
                        for (int k = 0; k < nColB; ++k)
                        {
                            xI[k] -= xJ[k] * lJI;
                        }
                    }
                }

                // Solve LTX = Y
                for (int j = m - 1; j >= 0; j--)
                {
                    double lJJ = lTData[j][j];
                    double[] xJ = x[j];
                    for (int k = 0; k < nColB; ++k)
                    {
                        xJ[k] /= lJJ;
                    }
                    for (int i = 0; i < j; i++)
                    {
                        double[] xI = x[i];
                        double lIJ = lTData[i][j];
                        for (int k = 0; k < nColB; ++k)
                        {
                            xI[k] -= xJ[k] * lIJ;
                        }
                    }
                }

                return x;
            }

            public double[,] Solve(double[,] value)
            {
                return Solve(value.ToJagged()).ToMultidimensional();
            }

            /// <summary>{@inheritDoc} */
            public Matrix<Double> GetInverse()
            {
                return Solve(MatrixUtility.CreateRealIdentityMatrix(lTData.Length));
            }
        }
    }
}

