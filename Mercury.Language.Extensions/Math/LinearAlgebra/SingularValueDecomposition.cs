// Copyright (c) 2017 - presented by Kei Nakai
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
//
using System;
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis.Solver;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Mercury.Language.Math.Matrix;
using SysMath = System.Math;

namespace Mercury.Language.Math.LinearAlgebra
{
    /// <summary>
    /// Calculates the compact Singular Value Decomposition of a matrix.
    /// <p>
    /// The Singular Value Decomposition of matrix A is a set of three matrices: U,
    /// &Sigma; and V such that A = U &times; &Sigma; &times; V<sup>T</sup>d Let A be
    /// a m &times; n matrix, then U is a m &times; p orthogonal matrix, &Sigma; is a
    /// p &times; p diagonal matrix with positive or null elements, V is a p &times;
    /// n orthogonal matrix (hence V<sup>T</sup> is also orthogonal) where
    /// p=min(m,n).
    /// </p>
    /// <p>This class is similar to the class with similar Name from the
    /// <a href="http://math.nist.gov/javanumerics/jama/">JAMA</a> library, with the
    /// following changes:</p>
    /// <ul>
    ///   <li>the {@code norm2} method which has been renamed as {@link #Norm
    ///   getNorm},</li>
    ///   <li>the {@code cond} method which has been renamed as {@link
    ///   #getConditionNumber() getConditionNumber},</li>
    ///   <li>the {@code rank} method which has been renamed as {@link #getRank()
    ///   getRank},</li>
    ///   <li>a {@link #getUT() getUT} method has been added,</li>
    ///   <li>a {@link #getVT() getVT} method has been added,</li>
    ///   <li>a {@link #Solver getSolver} method has been added,</li>
    ///   <li>a {@link #getCovariance(double) getCovariance} method has been added.</li>
    /// </ul>
    /// </summary>
    /// <see cref="<a">href="http://mathworld.wolfram.com/SingularValueDecomposition.html">MathWorld</a> </see>
    /// <see cref="<a">href="http://en.wikipedia.org/wiki/Singular_value_decomposition">Wikipedia</a> </see>
    /// @since 2.0 (changed to concrete class in 3.0)
    public class SingularValueDecomposition : ISingularValueDecomposition
    {
        /// <summary>
        /// Relative threshold for small singular valuesd
        /// </summary>
        private static double EPS = 2.22045E-16;    //0x1.0p-52
        /// <summary>
        /// Absolute threshold for small singular valuesd
        /// </summary>
        private static double TINY = 1.6033E-291;  //0x1.0p-966;
        /// <summary>
        /// Computed singular valuesd
        /// </summary>
        private double[] singularValues;
        /// <summary>
        /// max(row dimension, column dimension)d
        /// </summary>
        private int m;
        /// <summary>
        /// min(row dimension, column dimension)d
        /// </summary>
        private int n;
        /// <summary>
        /// Indicator for transposed matrixd
        /// </summary>
        private Boolean transposed;
        /// <summary>
        /// Cached value of U matrixd
        /// </summary>
        private Matrix<Double> cachedU;
        /// <summary>
        /// Cached value of transposed U matrixd
        /// </summary>
        private Matrix<Double> cachedUt;
        /// <summary>
        /// Cached value of S (diagonal) matrixd
        /// </summary>
        private Matrix<Double> cachedS;
        /// <summary>
        /// Cached value of V matrixd
        /// </summary>
        private Matrix<Double> cachedV;
        /// <summary>
        /// Cached value of transposed V matrixd
        /// </summary>
        private Matrix<Double> cachedVt;
        /// <summary>
        /// Tolerance value for small singular values, calculated once we have
        /// populated "singularValues".
        /// </summary>
        private double tol;

        /// <summary>
        /// Calculates the compact Singular Value Decomposition of the given matrix.
        /// 
        /// </summary>
        /// <param Name="matrix">Matrix to decompose.</param>
        public SingularValueDecomposition(Matrix<Double> matrix)
        {
            double[][] A;

            // "m" is always the largest dimension.
            if (matrix.RowCount < matrix.ColumnCount)
            {
                transposed = true;
                A = matrix.Transpose().ToArray().ToJagged();
                m = matrix.ColumnCount;
                n = matrix.RowCount;
            }
            else
            {
                transposed = false;
                A = matrix.ToArray().ToJagged();
                m = matrix.RowCount;
                n = matrix.ColumnCount;
            }

            singularValues = new double[n];
            double[][] U = ArrayUtility.CreateJaggedArray<double>(m, n);
            double[][] V = ArrayUtility.CreateJaggedArray<double>(n, n);
            double[] e = new double[n];
            double[] work = new double[m];
            // Reduce A to bidiagonal form, storing the diagonal elements
            // in s and the base-diagonal elements in e.
            int nct = SysMath.Min(m - 1, n);
            int nrt = SysMath.Max(0, n - 2);
            for (int k = 0; k < SysMath.Max(nct, nrt); k++)
            {
                if (k < nct)
                {
                    // Compute the transformation for the k-th column and
                    // place the k-th diagonal in s[k].
                    // Compute 2-norm of k-th column without under/overflow.
                    singularValues[k] = 0;
                    for (int i = k; i < m; i++)
                    {
                        singularValues[k] = Math2.Hypot(singularValues[k], A[i][k]);
                    }
                    if (singularValues[k] != 0)
                    {
                        if (A[k][k] < 0)
                        {
                            singularValues[k] = -singularValues[k];
                        }
                        for (int i = k; i < m; i++)
                        {
                            A[i][k] /= singularValues[k];
                        }
                        A[k][k] += 1;
                    }
                    singularValues[k] = -singularValues[k];
                }
                for (int j = k + 1; j < n; j++)
                {
                    if (k < nct &&
                        singularValues[k] != 0)
                    {
                        // Apply the transformation.
                        double t = 0;
                        for (int i = k; i < m; i++)
                        {
                            t += A[i][k] * A[i][j];
                        }
                        t = -t / A[k][k];
                        for (int i = k; i < m; i++)
                        {
                            A[i][j] += t * A[i][k];
                        }
                    }
                    // Place the k-th row of A into e for the
                    // subsequent calculation of the row transformation.
                    e[j] = A[k][j];
                }
                if (k < nct)
                {
                    // Place the transformation in U for subsequent back
                    // multiplication.
                    for (int i = k; i < m; i++)
                    {
                        U[i][k] = A[i][k];
                    }
                }
                if (k < nrt)
                {
                    // Compute the k-th row transformation and place the
                    // k-th base-diagonal in e[k].
                    // Compute 2-norm without under/overflow.
                    e[k] = 0;
                    for (int i = k + 1; i < n; i++)
                    {
                        e[k] = Math2.Hypot(e[k], e[i]);
                    }
                    if (e[k] != 0)
                    {
                        if (e[k + 1] < 0)
                        {
                            e[k] = -e[k];
                        }
                        for (int i = k + 1; i < n; i++)
                        {
                            e[i] /= e[k];
                        }
                        e[k + 1] += 1;
                    }
                    e[k] = -e[k];
                    if (k + 1 < m &&
                        e[k] != 0)
                    {
                        // Apply the transformation.
                        for (int i = k + 1; i < m; i++)
                        {
                            work[i] = 0;
                        }
                        for (int j = k + 1; j < n; j++)
                        {
                            for (int i = k + 1; i < m; i++)
                            {
                                work[i] += e[j] * A[i][j];
                            }
                        }
                        for (int j = k + 1; j < n; j++)
                        {
                            double t = -e[j] / e[k + 1];
                            for (int i = k + 1; i < m; i++)
                            {
                                A[i][j] += t * work[i];
                            }
                        }
                    }

                    // Place the transformation in V for subsequent
                    // back multiplication.
                    for (int i = k + 1; i < n; i++)
                    {
                        V[i][k] = e[i];
                    }
                }
            }
            // Set up the bidiagonal matrix or order p.
            int p = n;
            if (nct < n)
            {
                singularValues[nct] = A[nct][nct];
            }
            if (m < p)
            {
                singularValues[p - 1] = 0;
            }
            if (nrt + 1 < p)
            {
                e[nrt] = A[nrt][p - 1];
            }
            e[p - 1] = 0;

            // Generate U.
            for (int j = nct; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    U[i][j] = 0;
                }
                U[j][j] = 1;
            }
            for (int k = nct - 1; k >= 0; k--)
            {
                if (singularValues[k] != 0)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        double t = 0;
                        for (int i = k; i < m; i++)
                        {
                            t += U[i][k] * U[i][j];
                        }
                        t = -t / U[k][k];
                        for (int i = k; i < m; i++)
                        {
                            U[i][j] += t * U[i][k];
                        }
                    }
                    for (int i = k; i < m; i++)
                    {
                        U[i][k] = -U[i][k];
                    }
                    U[k][k] = 1 + U[k][k];
                    for (int i = 0; i < k - 1; i++)
                    {
                        U[i][k] = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < m; i++)
                    {
                        U[i][k] = 0;
                    }
                    U[k][k] = 1;
                }
            }

            // Generate V.
            for (int k = n - 1; k >= 0; k--)
            {
                if (k < nrt &&
                    e[k] != 0)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        double t = 0;
                        for (int i = k + 1; i < n; i++)
                        {
                            t += V[i][k] * V[i][j];
                        }
                        t = -t / V[k + 1][k];
                        for (int i = k + 1; i < n; i++)
                        {
                            V[i][j] += t * V[i][k];
                        }
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    V[i][k] = 0;
                }
                V[k][k] = 1;
            }

            // Main iteration loop for the singular values.
            int pp = p - 1;
            while (p > 0)
            {
                int k;
                int kase;
                // Here is where a test for too many iterations would go.
                // This section of the program inspects for
                // negligible elements in the s and e arraysd  On
                // completion the variables kase and k are set as follows.
                // kase = 1     if s(p) and e[k-1] are negligible and k<p
                // kase = 2     if s(k) is negligible and k<p
                // kase = 3     if e[k-1] is negligible, k<p, and
                //              s(k), [], s(p) are not negligible (qr step).
                // kase = 4     if e(p-1) is negligible (convergence).
                for (k = p - 2; k >= 0; k--)
                {
                    double threshold = TINY + EPS * (SysMath.Abs(singularValues[k]) + SysMath.Abs(singularValues[k + 1]));

                    // the following condition is written this way in order
                    // to break out of the loop when NaN occurs, writing it
                    // as "if (SysMath.Abs(e[k]) <= threshold)" would loop
                    // indefinitely in case of NaNs because comparison on NaNs
                    // always return false, regardless of what is checked
                    // see issue MATH-947
                    if (!(SysMath.Abs(e[k]) > threshold))
                    {
                        e[k] = 0;
                        break;
                    }
                }

                if (k == p - 2)
                {
                    kase = 4;
                }
                else
                {
                    int ks;
                    for (ks = p - 1; ks >= k; ks--)
                    {
                        if (ks == k)
                        {
                            break;
                        }
                        double t = (ks != p ? SysMath.Abs(e[ks]) : 0) +
                            (ks != k + 1 ? SysMath.Abs(e[ks - 1]) : 0);
                        if (SysMath.Abs(singularValues[ks]) <= TINY + EPS * t)
                        {
                            singularValues[ks] = 0;
                            break;
                        }
                    }
                    if (ks == k)
                    {
                        kase = 3;
                    }
                    else if (ks == p - 1)
                    {
                        kase = 1;
                    }
                    else
                    {
                        kase = 2;
                        k = ks;
                    }
                }
                k++;
                // Perform the task indicated by kase.
                switch (kase)
                {
                    // Deflate negligible s(p).
                    case 1:
                        {
                            double f = e[p - 2];
                            e[p - 2] = 0;
                            for (int j = p - 2; j >= k; j--)
                            {
                                double t = Math2.Hypot(singularValues[j], f);
                                double cs = singularValues[j] / t;
                                double sn = f / t;
                                singularValues[j] = t;
                                if (j != k)
                                {
                                    f = -sn * e[j - 1];
                                    e[j - 1] = cs * e[j - 1];
                                }

                                for (int i = 0; i < n; i++)
                                {
                                    t = cs * V[i][j] + sn * V[i][p - 1];
                                    V[i][p - 1] = -sn * V[i][j] + cs * V[i][p - 1];
                                    V[i][j] = t;
                                }
                            }
                        }
                        break;
                    // Split at negligible s(k).
                    case 2:
                        {
                            double f = e[k - 1];
                            e[k - 1] = 0;
                            for (int j = k; j < p; j++)
                            {
                                double t = Math2.Hypot(singularValues[j], f);
                                double cs = singularValues[j] / t;
                                double sn = f / t;
                                singularValues[j] = t;
                                f = -sn * e[j];
                                e[j] = cs * e[j];

                                for (int i = 0; i < m; i++)
                                {
                                    t = cs * U[i][j] + sn * U[i][k - 1];
                                    U[i][k - 1] = -sn * U[i][j] + cs * U[i][k - 1];
                                    U[i][j] = t;
                                }
                            }
                        }
                        break;
                    // Perform one qr step.
                    case 3:
                        {
                            // Calculate the shift.
                            double maxPm1Pm2 = SysMath.Max(SysMath.Abs(singularValues[p - 1]), SysMath.Abs(singularValues[p - 2]));
                            double isScale = SysMath.Max(SysMath.Max(SysMath.Max(maxPm1Pm2, SysMath.Abs(e[p - 2])), SysMath.Abs(singularValues[k])), SysMath.Abs(e[k]));
                            double sp = singularValues[p - 1] / isScale;
                            double spm1 = singularValues[p - 2] / isScale;
                            double epm1 = e[p - 2] / isScale;
                            double sk = singularValues[k] / isScale;
                            double ek = e[k] / isScale;
                            double b = ((spm1 + sp) * (spm1 - sp) + epm1 * epm1) / 2.0;
                            double c = (sp * epm1) * (sp * epm1);
                            double shift = 0;
                            if (b != 0 ||
                                c != 0)
                            {
                                shift = SysMath.Sqrt(b * b + c);
                                if (b < 0)
                                {
                                    shift = -shift;
                                }
                                shift = c / (b + shift);
                            }
                            double f = (sk + sp) * (sk - sp) + shift;
                            double g = sk * ek;
                            // Chase zeros.
                            for (int j = k; j < p - 1; j++)
                            {
                                double t = Math2.Hypot(f, g);
                                double cs = f / t;
                                double sn = g / t;
                                if (j != k)
                                {
                                    e[j - 1] = t;
                                }
                                f = cs * singularValues[j] + sn * e[j];
                                e[j] = cs * e[j] - sn * singularValues[j];
                                g = sn * singularValues[j + 1];
                                singularValues[j + 1] = cs * singularValues[j + 1];

                                for (int i = 0; i < n; i++)
                                {
                                    t = cs * V[i][j] + sn * V[i][j + 1];
                                    V[i][j + 1] = -sn * V[i][j] + cs * V[i][j + 1];
                                    V[i][j] = t;
                                }
                                t = Math2.Hypot(f, g);
                                cs = f / t;
                                sn = g / t;
                                singularValues[j] = t;
                                f = cs * e[j] + sn * singularValues[j + 1];
                                singularValues[j + 1] = -sn * e[j] + cs * singularValues[j + 1];
                                g = sn * e[j + 1];
                                e[j + 1] = cs * e[j + 1];
                                if (j < m - 1)
                                {
                                    for (int i = 0; i < m; i++)
                                    {
                                        t = cs * U[i][j] + sn * U[i][j + 1];
                                        U[i][j + 1] = -sn * U[i][j] + cs * U[i][j + 1];
                                        U[i][j] = t;
                                    }
                                }
                            }
                            e[p - 2] = f;
                        }
                        break;
                    // Convergence.
                    default:
                        {
                            // Make the singular values positive.
                            if (singularValues[k] <= 0)
                            {
                                singularValues[k] = singularValues[k] < 0 ? -singularValues[k] : 0;

                                for (int i = 0; i <= pp; i++)
                                {
                                    V[i][k] = -V[i][k];
                                }
                            }
                            // Order the singular values.
                            while (k < pp)
                            {
                                if (singularValues[k] >= singularValues[k + 1])
                                {
                                    break;
                                }
                                double t = singularValues[k];
                                singularValues[k] = singularValues[k + 1];
                                singularValues[k + 1] = t;
                                if (k < n - 1)
                                {
                                    for (int i = 0; i < n; i++)
                                    {
                                        t = V[i][k + 1];
                                        V[i][k + 1] = V[i][k];
                                        V[i][k] = t;
                                    }
                                }
                                if (k < m - 1)
                                {
                                    for (int i = 0; i < m; i++)
                                    {
                                        t = U[i][k + 1];
                                        U[i][k + 1] = U[i][k];
                                        U[i][k] = t;
                                    }
                                }
                                k++;
                            }
                            p--;
                        }
                        break;
                }
            }

            // Set the small value tolerance used to calculate rank and pseudo-inverse
            tol = SysMath.Max(m * singularValues[0] * EPS, SysMath.Sqrt(Precision2.SAFE_MIN));

            if (!transposed)
            {
                cachedU = MatrixUtility.CreateMatrix<Double>(U);
                cachedV = MatrixUtility.CreateMatrix<Double>(V);
            }
            else
            {
                cachedU = MatrixUtility.CreateMatrix<Double>(V);
                cachedV = MatrixUtility.CreateMatrix<Double>(U);
            }
        }


        /// <summary>
        /// Returns the matrix U of the decomposition.
        /// <p>U is an orthogonal matrix, i.ed its transpose is also its inverse.</p>
        /// </summary>
        /// <returns>the U matrix</returns>
        /// <see cref="#getUT()"></see>
        public Matrix<Double> GetU()
        {
            // return the cached matrix
            return cachedU;
        }

        /// <summary>
        /// Returns the transpose of the matrix U of the decomposition.
        /// <p>U is an orthogonal matrix, i.ed its transpose is also its inverse.</p>
        /// </summary>
        /// <returns>the U matrix (or null if decomposed matrix is singular)</returns>
        /// <see cref="#U"></see>
        public Matrix<Double> GetUT()
        {
            if (cachedUt == null)
            {
                cachedUt = (Matrix<Double>)GetU().Transpose();
            }
            // return the cached matrix
            return cachedUt;
        }

        /// <summary>
        /// Returns the diagonal matrix &Sigma; of the decomposition.
        /// <p>&Sigma; is a diagonal matrixd The singular values are provided in
        /// non-increasing order, for compatibility with Jama.</p>
        /// </summary>
        /// <returns>the &Sigma; matrix</returns>
        public Matrix<Double> GetSigma()
        {
            if (cachedS == null)
            {
                // cache the matrix for subsequent calls
                cachedS = MatrixUtility.CreateDenseDiagonalMatrix(singularValues);
            }
            return cachedS;
        }

        /// <summary>
        /// Returns the diagonal elements of the matrix &Sigma; of the decomposition.
        /// <p>The singular values are provided in non-increasing order, for
        /// compatibility with Jama.</p>
        /// </summary>
        /// <returns>the diagonal elements of the &Sigma; matrix</returns>
        public double[] SingularValues
        {
            get { return singularValues.CloneExact(); }
        }

        /// <summary>
        /// Returns the matrix V of the decomposition.
        /// <p>V is an orthogonal matrix, i.ed its transpose is also its inverse.</p>
        /// </summary>
        /// <returns>the V matrix (or null if decomposed matrix is singular)</returns>
        /// <see cref="#getVT()"></see>
        public Matrix<Double> GetV()
        {
            // return the cached matrix
            return cachedV;
        }

        /// <summary>
        /// Returns the transpose of the matrix V of the decomposition.
        /// <p>V is an orthogonal matrix, i.ed its transpose is also its inverse.</p>
        /// </summary>
        /// <returns>the V matrix (or null if decomposed matrix is singular)</returns>
        /// <see cref="#getV()"></see>
        public Matrix<Double> GetVT()
        {
            if (cachedVt == null)
            {
                cachedVt = (Matrix<Double>)GetV().Transpose();
            }
            // return the cached matrix
            return cachedVt;
        }

        /// <summary>
        /// Returns the n &times; n covariance matrix.
        /// <p>The covariance matrix is V &times; J &times; V<sup>T</sup>
        /// where J is the diagonal matrix of the inverse of the squares of
        /// the singular values.</p>
        /// </summary>
        /// <param Name="minSingularValue">value below which singular values are ignored</param>
        /// (a 0 or negative value implies all singular value will be used)
        /// <returns>covariance matrix</returns>
        /// <exception cref="ArgumentException">if minSingularValue is larger than </exception>
        /// the largest singular value, meaning all singular values are ignored
        public Matrix<Double> GetCovariance(double minSingularValue)
        {
            // get the number of singular values to consider
            int p = singularValues.Length;
            int dimension = 0;
            while (dimension < p &&
                   singularValues[dimension] >= minSingularValue)
            {
                ++dimension;
            }

            if (dimension == 0)
            {
                throw new NumberIsTooLargeException(LocalizedResources.Instance().TOO_LARGE_CUTOFF_SINGULAR_VALUE, minSingularValue, singularValues[0], true);
            }

            double[][] data = ArrayUtility.CreateJaggedArray<double>(dimension, p);

            var vtData = GetVT().ToArray();

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    data[i][j] = vtData[i, j] / singularValues[i];
                }
            }

            Matrix<Double> jv = MatrixUtility.CreateMatrix<Double>(data);
            return (Matrix<Double>)jv.Transpose().Multiply(jv);
        }

        /// <summary>
        /// Returns the L<sub>2</sub> norm of the matrix.
        /// <p>The L<sub>2</sub> norm is max(|A &times; u|<sub>2</sub> /
        /// |u|<sub>2</sub>), where |.|<sub>2</sub> denotes the vectorial 2-norm
        /// (i.ed the traditional euclidian norm).</p>
        /// </summary>
        /// <returns>norm</returns>
        public double Norm
        {
            get { return singularValues[0]; }
        }

        /// <summary>
        /// Return the condition number of the matrix.
        /// </summary>
        /// <returns>condition number of the matrix</returns>
        public double ConditionNumber
        {
            get { return singularValues[0] / singularValues[n - 1]; }
        }

        /// <summary>
        /// Computes the inverse of the condition number.
        /// In cases of rank deficiency, the {@link #getConditionNumber() condition
        /// number} will become undefined.
        /// 
        /// </summary>
        /// <returns>the inverse of the condition number.</returns>
        public double InverseConditionNumber
        {
            get { return singularValues[n - 1] / singularValues[0]; }
        }

        /// <summary>
        /// Return the effective numerical matrix rank.
        /// <p>The effective numerical rank is the number of non-negligible
        /// singular valuesd The threshold used to identify non-negligible
        /// terms is max(m,n) &times; ulp(s<sub>1</sub>) where ulp(s<sub>1</sub>)
        /// is the least significant bit of the largest singular value.</p>
        /// </summary>
        /// <returns>effective numerical matrix rank</returns>
        public int GetRank()
        {
            int r = 0;
            for (int i = 0; i < singularValues.Length; i++)
            {
                if (singularValues[i] > tol)
                {
                    r++;
                }
            }
            return r;
        }

        /// <summary>
        /// Get a solver for finding the A &times; X = B solution in least square sense.
        /// </summary>
        /// <returns>a solver</returns>
        public IDecompositionSolver GetSolver()
        {
            return new Solver(singularValues, GetUT(), GetV(), GetRank() == m, tol);
        }

        /// <summary>Specialized solverd */
        public class Solver : IDecompositionSolver
        {
            /// <summary>Pseudo-inverse of the initial matrixd */
            private Matrix<Double> pseudoInverse;
            /// <summary>Singularity indicatord */
            private Boolean nonSingular;

            /// <summary>
            /// Build a solver from decomposed matrix.
            /// 
            /// </summary>
            /// <param Name="singularValues">Singular values.</param>
            /// <param Name="uT">U<sup>T</sup> matrix of the decomposition.</param>
            /// <param Name="v">V matrix of the decomposition.</param>
            /// <param Name="nonSingular">Singularity indicator.</param>
            /// <param Name="tol">tolerance for singular values</param>
            public Solver(double[] singularValues, Matrix<Double> uT, Matrix<Double> v, Boolean nonSingular, double tol)
            {
                double[][] suT = uT.ToArray().ToJagged();
                for (int i = 0; i < singularValues.Length; i++)
                {
                    double a;
                    if (singularValues[i] > tol)
                    {
                        a = 1 / singularValues[i];
                    }
                    else
                    {
                        a = 0;
                    }
                    double[] suTi = suT[i];
                    for (int j = 0; j < suTi.Length; j++)
                    {
                        suTi[j] *= a;
                    }
                }
                pseudoInverse = (Matrix<Double>)v.Multiply(MatrixUtility.CreateMatrix<Double>(suT));
                this.nonSingular = nonSingular;
            }

            /// <summary>
            /// Solve the linear equation A &times; X = B in least square sense.
            /// <p>
            /// The m&times;n matrix A may not be square, the solution X is such that
            /// ||A &times; X - B|| is minimal.
            /// </p>
            /// </summary>
            /// <param Name="b">Right-hand side of the equation A &times; X = B</param>
            /// <returns>a vector X that minimizes the two norm of A &times; X - B</returns>
            /// <exception cref="org.apache.commons.math3.exception.DimensionMismatchException"></exception>
            /// if the matrices dimensions do not match.
            public Vector<Double> Solve(Vector<Double> b)
            {
                return ((DenseMatrix)pseudoInverse).Operate((DenseVector)b);
            }

            /// <summary>
            /// Solve the linear equation A &times; X = B in least square sense.
            /// <p>
            /// The m&times;n matrix A may not be square, the solution X is such that
            /// ||A &times; X - B|| is minimal.
            /// </p>
            /// 
            /// </summary>
            /// <param Name="b">Right-hand side of the equation A &times; X = B</param>
            /// <returns>a matrix X that minimizes the two norm of A &times; X - B</returns>
            /// <exception cref="org.apache.commons.math3.exception.DimensionMismatchException"></exception>
            /// if the matrices dimensions do not match.
            public Matrix<Double> Solve(Matrix<Double> b)
            {
                return (Matrix<Double>)pseudoInverse.Multiply(b);
            }

            /// <summary>
            /// Check if the decomposed matrix is non-singular.
            /// 
            /// </summary>
            /// <returns>{@code true} if the decomposed matrix is non-singular.</returns>
            public Boolean IsNonSingular
            {
                get { return nonSingular; }
            }

            /// <summary>
            /// Get the pseudo-inverse of the decomposed matrix.
            /// 
            /// </summary>
            /// <returns>the inverse matrix.</returns>
            public Matrix<Double> GetInverse()
            {
                return pseudoInverse;
            }

            public double[] Solve(double[] b)
            {
                return Solve(Vector<Double>.Build.Dense(b)).AsArrayEx();
            }

            public double[][] Solve(double[][] b)
            {
                return Solve(b.ToMultidimensional()).ToJagged();
            }

            public double[,] Solve(double[,] b)
            {
                MathNet.Numerics.LinearAlgebra.Storage.DenseColumnMajorMatrixStorage<Double> storage = MathNet.Numerics.LinearAlgebra.Storage.DenseColumnMajorMatrixStorage<Double>.OfArray(b);
                return pseudoInverse.Multiply(Matrix<Double>.Build.Dense(storage)).AsArrayEx();
            }

            Matrix<double> IDecompositionSolver.GetInverse()
            {
                return (Matrix<double>)pseudoInverse;
            }
        }
    }
}
