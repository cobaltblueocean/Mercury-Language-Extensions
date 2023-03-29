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
using Mercury.Language.Exceptions;
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
using Mercury.Language.Log;
using Mercury.Language.Extensions;

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
    /// <see href="https://github.com/Azure/Accord-NET/blob/master/Sources/Accord.Math/Decompositions/SingularValueDecomposition.cs"/>
    /// @since 2.0 (changed to concrete class in 3.0)
    public class SingularValueDecomposition : ISingularValueDecomposition
    {
        /// <summary>
        /// Absolute threshold for small singular valuesd
        /// </summary>
        private static double TINY = 1.6033E-291;  //0x1.0p-966;


        /// <summary>
        /// Eigen decomposition of the tridiagonal matrix.
        /// </summary>
        private EigenDecomposition eigenDecomposition;

        /// <summary>
        /// Computed singular valuesd
        /// </summary>
        private double[] singularValues;
        /// <summary>
        /// sorting order
        /// </summary>
        private int[] si;
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
        /// Calculates the compact Singular Value Decomposition of the given matrix.
        /// 
        /// </summary>
        /// <param Name="matrix">Matrix to decompose.</param>
        public SingularValueDecomposition(Matrix<Double> matrix)// : this(matrix.AsArrayEx())
        {
            #region Old Code (Unused)
            //double[][] A;

            //// "m" is always the largest dimension.
            //if (matrix.RowCount < matrix.ColumnCount)
            //{
            //    transposed = true;
            //    A = matrix.Transpose().ToArray().ToJagged();
            //    m = matrix.ColumnCount;
            //    n = matrix.RowCount;
            //}
            //else
            //{
            //    transposed = false;
            //    A = matrix.ToArray().ToJagged();
            //    m = matrix.RowCount;
            //    n = matrix.ColumnCount;
            //}

            //singularValues = new double[n];
            //double[][] U = ArrayUtility.CreateJaggedArray<double>(m, n);
            //double[][] V = ArrayUtility.CreateJaggedArray<double>(n, n);
            //double[] e = new double[n];
            //double[] work = new double[m];
            //// Reduce A to bidiagonal form, storing the diagonal elements
            //// in s and the base-diagonal elements in e.
            //int nct = SysMath.Min(m - 1, n);
            //int nrt = SysMath.Max(0, n - 2);
            //for (int k = 0; k < SysMath.Max(nct, nrt); k++)
            //{
            //    if (k < nct)
            //    {
            //        // Compute the transformation for the k-th column and
            //        // place the k-th diagonal in s[k].
            //        // Compute 2-norm of k-th column without under/overflow.
            //        singularValues[k] = 0;
            //        for (int i = k; i < m; i++)
            //        {
            //            singularValues[k] = Math2.Hypot(singularValues[k], A[i][k]);
            //        }
            //        if (singularValues[k] != 0)
            //        {
            //            if (A[k][k] < 0)
            //            {
            //                singularValues[k] = -singularValues[k];
            //            }
            //            for (int i = k; i < m; i++)
            //            {
            //                A[i][k] /= singularValues[k];
            //            }
            //            A[k][k] += 1;
            //        }
            //        singularValues[k] = -singularValues[k];
            //    }
            //    for (int j = k + 1; j < n; j++)
            //    {
            //        if (k < nct &&
            //            singularValues[k] != 0)
            //        {
            //            // Apply the transformation.
            //            double t = 0;
            //            for (int i = k; i < m; i++)
            //            {
            //                t += A[i][k] * A[i][j];
            //            }
            //            t = -t / A[k][k];
            //            for (int i = k; i < m; i++)
            //            {
            //                A[i][j] += t * A[i][k];
            //            }
            //        }
            //        // Place the k-th row of A into e for the
            //        // subsequent calculation of the row transformation.
            //        e[j] = A[k][j];
            //    }
            //    if (k < nct)
            //    {
            //        // Place the transformation in U for subsequent back
            //        // multiplication.
            //        for (int i = k; i < m; i++)
            //        {
            //            U[i][k] = A[i][k];
            //        }
            //    }
            //    if (k < nrt)
            //    {
            //        // Compute the k-th row transformation and place the
            //        // k-th base-diagonal in e[k].
            //        // Compute 2-norm without under/overflow.
            //        e[k] = 0;
            //        for (int i = k + 1; i < n; i++)
            //        {
            //            e[k] = Math2.Hypot(e[k], e[i]);
            //        }
            //        if (e[k] != 0)
            //        {
            //            if (e[k + 1] < 0)
            //            {
            //                e[k] = -e[k];
            //            }
            //            for (int i = k + 1; i < n; i++)
            //            {
            //                e[i] /= e[k];
            //            }
            //            e[k + 1] += 1;
            //        }
            //        e[k] = -e[k];
            //        if (k + 1 < m &&
            //            e[k] != 0)
            //        {
            //            // Apply the transformation.
            //            for (int i = k + 1; i < m; i++)
            //            {
            //                work[i] = 0;
            //            }
            //            for (int j = k + 1; j < n; j++)
            //            {
            //                for (int i = k + 1; i < m; i++)
            //                {
            //                    work[i] += e[j] * A[i][j];
            //                }
            //            }
            //            for (int j = k + 1; j < n; j++)
            //            {
            //                double t = -e[j] / e[k + 1];
            //                for (int i = k + 1; i < m; i++)
            //                {
            //                    A[i][j] += t * work[i];
            //                }
            //            }
            //        }

            //        // Place the transformation in V for subsequent
            //        // back multiplication.
            //        for (int i = k + 1; i < n; i++)
            //        {
            //            V[i][k] = e[i];
            //        }
            //    }
            //}
            //// Set up the bidiagonal matrix or order p.
            //int p = n;
            //if (nct < n)
            //{
            //    singularValues[nct] = A[nct][nct];
            //}
            //if (m < p)
            //{
            //    singularValues[p - 1] = 0;
            //}
            //if (nrt + 1 < p)
            //{
            //    e[nrt] = A[nrt][p - 1];
            //}
            //e[p - 1] = 0;

            //// Generate U.
            //for (int j = nct; j < n; j++)
            //{
            //    for (int i = 0; i < m; i++)
            //    {
            //        U[i][j] = 0;
            //    }
            //    U[j][j] = 1;
            //}
            //for (int k = nct - 1; k >= 0; k--)
            //{
            //    if (singularValues[k] != 0)
            //    {
            //        for (int j = k + 1; j < n; j++)
            //        {
            //            double t = 0;
            //            for (int i = k; i < m; i++)
            //            {
            //                t += U[i][k] * U[i][j];
            //            }
            //            t = -t / U[k][k];
            //            for (int i = k; i < m; i++)
            //            {
            //                U[i][j] += t * U[i][k];
            //            }
            //        }
            //        for (int i = k; i < m; i++)
            //        {
            //            U[i][k] = -U[i][k];
            //        }
            //        U[k][k] = 1 + U[k][k];
            //        for (int i = 0; i < k - 1; i++)
            //        {
            //            U[i][k] = 0;
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < m; i++)
            //        {
            //            U[i][k] = 0;
            //        }
            //        U[k][k] = 1;
            //    }
            //}

            //// Generate V.
            //for (int k = n - 1; k >= 0; k--)
            //{
            //    if (k < nrt &&
            //        e[k] != 0)
            //    {
            //        for (int j = k + 1; j < n; j++)
            //        {
            //            double t = 0;
            //            for (int i = k + 1; i < n; i++)
            //            {
            //                t += V[i][k] * V[i][j];
            //            }
            //            t = -t / V[k + 1][k];
            //            for (int i = k + 1; i < n; i++)
            //            {
            //                V[i][j] += t * V[i][k];
            //            }
            //        }
            //    }
            //    for (int i = 0; i < n; i++)
            //    {
            //        V[i][k] = 0;
            //    }
            //    V[k][k] = 1;
            //}

            //// Main iteration loop for the singular values.
            //int pp = p - 1;
            //while (p > 0)
            //{
            //    int k;
            //    int kase;
            //    // Here is where a test for too many iterations would go.
            //    // This section of the program inspects for
            //    // negligible elements in the s and e arraysd  On
            //    // completion the variables kase and k are set as follows.
            //    // kase = 1     if s(p) and e[k-1] are negligible and k<p
            //    // kase = 2     if s(k) is negligible and k<p
            //    // kase = 3     if e[k-1] is negligible, k<p, and
            //    //              s(k), [], s(p) are not negligible (qr step).
            //    // kase = 4     if e(p-1) is negligible (convergence).
            //    for (k = p - 2; k >= 0; k--)
            //    {
            //        double threshold = TINY + EPS * (SysMath.Abs(singularValues[k]) + SysMath.Abs(singularValues[k + 1]));

            //        // the following condition is written this way in order
            //        // to break out of the loop when NaN occurs, writing it
            //        // as "if (SysMath.Abs(e[k]) <= threshold)" would loop
            //        // indefinitely in case of NaNs because comparison on NaNs
            //        // always return false, regardless of what is checked
            //        // see issue MATH-947
            //        if (!(SysMath.Abs(e[k]) > threshold))
            //        {
            //            e[k] = 0;
            //            break;
            //        }
            //    }

            //    if (k == p - 2)
            //    {
            //        kase = 4;
            //    }
            //    else
            //    {
            //        int ks;
            //        for (ks = p - 1; ks >= k; ks--)
            //        {
            //            if (ks == k)
            //            {
            //                break;
            //            }
            //            double t = (ks != p ? SysMath.Abs(e[ks]) : 0) +
            //                (ks != k + 1 ? SysMath.Abs(e[ks - 1]) : 0);
            //            if (SysMath.Abs(singularValues[ks]) <= TINY + EPS * t)
            //            {
            //                singularValues[ks] = 0;
            //                break;
            //            }
            //        }
            //        if (ks == k)
            //        {
            //            kase = 3;
            //        }
            //        else if (ks == p - 1)
            //        {
            //            kase = 1;
            //        }
            //        else
            //        {
            //            kase = 2;
            //            k = ks;
            //        }
            //    }
            //    k++;
            //    // Perform the task indicated by kase.
            //    switch (kase)
            //    {
            //        // Deflate negligible s(p).
            //        case 1:
            //            {
            //                double f = e[p - 2];
            //                e[p - 2] = 0;
            //                for (int j = p - 2; j >= k; j--)
            //                {
            //                    double t = Math2.Hypot(singularValues[j], f);
            //                    double cs = singularValues[j] / t;
            //                    double sn = f / t;
            //                    singularValues[j] = t;
            //                    if (j != k)
            //                    {
            //                        f = -sn * e[j - 1];
            //                        e[j - 1] = cs * e[j - 1];
            //                    }

            //                    for (int i = 0; i < n; i++)
            //                    {
            //                        t = cs * V[i][j] + sn * V[i][p - 1];
            //                        V[i][p - 1] = -sn * V[i][j] + cs * V[i][p - 1];
            //                        V[i][j] = t;
            //                    }
            //                }
            //            }
            //            break;
            //        // Split at negligible s(k).
            //        case 2:
            //            {
            //                double f = e[k - 1];
            //                e[k - 1] = 0;
            //                for (int j = k; j < p; j++)
            //                {
            //                    double t = Math2.Hypot(singularValues[j], f);
            //                    double cs = singularValues[j] / t;
            //                    double sn = f / t;
            //                    singularValues[j] = t;
            //                    f = -sn * e[j];
            //                    e[j] = cs * e[j];

            //                    for (int i = 0; i < m; i++)
            //                    {
            //                        t = cs * U[i][j] + sn * U[i][k - 1];
            //                        U[i][k - 1] = -sn * U[i][j] + cs * U[i][k - 1];
            //                        U[i][j] = t;
            //                    }
            //                }
            //            }
            //            break;
            //        // Perform one qr step.
            //        case 3:
            //            {
            //                // Calculate the shift.
            //                double maxPm1Pm2 = SysMath.Max(SysMath.Abs(singularValues[p - 1]), SysMath.Abs(singularValues[p - 2]));
            //                double isScale = SysMath.Max(SysMath.Max(SysMath.Max(maxPm1Pm2, SysMath.Abs(e[p - 2])), SysMath.Abs(singularValues[k])), SysMath.Abs(e[k]));
            //                double sp = singularValues[p - 1] / isScale;
            //                double spm1 = singularValues[p - 2] / isScale;
            //                double epm1 = e[p - 2] / isScale;
            //                double sk = singularValues[k] / isScale;
            //                double ek = e[k] / isScale;
            //                double b = ((spm1 + sp) * (spm1 - sp) + epm1 * epm1) / 2.0;
            //                double c = (sp * epm1) * (sp * epm1);
            //                double shift = 0;
            //                if (b != 0 ||
            //                    c != 0)
            //                {
            //                    shift = SysMath.Sqrt(b * b + c);
            //                    if (b < 0)
            //                    {
            //                        shift = -shift;
            //                    }
            //                    shift = c / (b + shift);
            //                }
            //                double f = (sk + sp) * (sk - sp) + shift;
            //                double g = sk * ek;
            //                // Chase zeros.
            //                for (int j = k; j < p - 1; j++)
            //                {
            //                    double t = Math2.Hypot(f, g);
            //                    double cs = f / t;
            //                    double sn = g / t;
            //                    if (j != k)
            //                    {
            //                        e[j - 1] = t;
            //                    }
            //                    f = cs * singularValues[j] + sn * e[j];
            //                    e[j] = cs * e[j] - sn * singularValues[j];
            //                    g = sn * singularValues[j + 1];
            //                    singularValues[j + 1] = cs * singularValues[j + 1];

            //                    for (int i = 0; i < n; i++)
            //                    {
            //                        t = cs * V[i][j] + sn * V[i][j + 1];
            //                        V[i][j + 1] = -sn * V[i][j] + cs * V[i][j + 1];
            //                        V[i][j] = t;
            //                    }
            //                    t = Math2.Hypot(f, g);
            //                    cs = f / t;
            //                    sn = g / t;
            //                    singularValues[j] = t;
            //                    f = cs * e[j] + sn * singularValues[j + 1];
            //                    singularValues[j + 1] = -sn * e[j] + cs * singularValues[j + 1];
            //                    g = sn * e[j + 1];
            //                    e[j + 1] = cs * e[j + 1];
            //                    if (j < m - 1)
            //                    {
            //                        for (int i = 0; i < m; i++)
            //                        {
            //                            t = cs * U[i][j] + sn * U[i][j + 1];
            //                            U[i][j + 1] = -sn * U[i][j] + cs * U[i][j + 1];
            //                            U[i][j] = t;
            //                        }
            //                    }
            //                }
            //                e[p - 2] = f;
            //            }
            //            break;
            //        // Convergence.
            //        default:
            //            {
            //                // Make the singular values positive.
            //                if (singularValues[k] <= 0)
            //                {
            //                    singularValues[k] = singularValues[k] < 0 ? -singularValues[k] : 0;

            //                    for (int i = 0; i <= pp; i++)
            //                    {
            //                        V[i][k] = -V[i][k];
            //                    }
            //                }
            //                // Order the singular values.
            //                while (k < pp)
            //                {
            //                    if (singularValues[k] >= singularValues[k + 1])
            //                    {
            //                        break;
            //                    }
            //                    double t = singularValues[k];
            //                    singularValues[k] = singularValues[k + 1];
            //                    singularValues[k + 1] = t;
            //                    if (k < n - 1)
            //                    {
            //                        for (int i = 0; i < n; i++)
            //                        {
            //                            t = V[i][k + 1];
            //                            V[i][k + 1] = V[i][k];
            //                            V[i][k] = t;
            //                        }
            //                    }
            //                    if (k < m - 1)
            //                    {
            //                        for (int i = 0; i < m; i++)
            //                        {
            //                            t = U[i][k + 1];
            //                            U[i][k + 1] = U[i][k];
            //                            U[i][k] = t;
            //                        }
            //                    }
            //                    k++;
            //                }
            //                p--;
            //            }
            //            break;
            //    }
            //}

            //// Set the small value tolerance used to calculate rank and pseudo-inverse
            //tol = SysMath.Max(m * singularValues[0] * EPS, SysMath.Sqrt(Precision2.SAFE_MIN));

            //if (!transposed)
            //{
            //    cachedU = MatrixUtility.CreateMatrix<Double>(U);
            //    cachedV = MatrixUtility.CreateMatrix<Double>(V);
            //}
            //else
            //{
            //    cachedU = MatrixUtility.CreateMatrix<Double>(V);
            //    cachedV = MatrixUtility.CreateMatrix<Double>(U);
            //}
            #endregion

            #region code from Java, not working
            m = matrix.RowCount;
            n = matrix.ColumnCount;

            cachedU = null;
            cachedS = null;
            cachedV = null;
            cachedVt = null;

            double[][] localcopy = matrix.AsArrayEx().ToJagged();
            double[][] matATA = ArrayUtility.CreateJaggedArray<double>(n, n);
            //
            // create A^T*A
            //
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    matATA[i][j] = 0.0;
                    for (int k = 0; k < m; k++)
                    {
                        matATA[i][j] += localcopy[k][i] * localcopy[k][j];
                    }
                    matATA[j][i] = matATA[i][j];
                }
            }

            double[][] matAAT = ArrayUtility.CreateJaggedArray<double>(m, m);
            //
            // create A*A^T
            //
            for (int i = 0; i < m; i++)
            {
                for (int j = i; j < m; j++)
                {
                    matAAT[i][j] = 0.0;
                    for (int k = 0; k < n; k++)
                    {
                        matAAT[i][j] += localcopy[i][k] * localcopy[j][k];
                    }
                    matAAT[j][i] = matAAT[i][j];
                }
            }
            int p;
            if (m >= n)
            {
                p = n;
                // compute eigen decomposition of A^T*A
                eigenDecomposition = new EigenDecomposition(MatrixUtility.CreateMatrix(matATA), 1.0);
                singularValues = eigenDecomposition.GetRealEigenvalues();
                cachedV = eigenDecomposition.GetV();
                // compute eigen decomposition of A*A^T
                eigenDecomposition = new EigenDecomposition(MatrixUtility.CreateMatrix(matAAT), 1.0);
                cachedU = eigenDecomposition.GetV().GetSubMatrix(0, m - 1, 0, p - 1);
            }
            else
            {
                p = m;
                // compute eigen decomposition of A*A^T
                eigenDecomposition = new EigenDecomposition(MatrixUtility.CreateMatrix(matAAT), 1.0);
                singularValues = eigenDecomposition.GetRealEigenvalues();
                cachedU = eigenDecomposition.GetV();

                // compute eigen decomposition of A^T*A
                eigenDecomposition = new EigenDecomposition(MatrixUtility.CreateMatrix(matATA), 1.0);
                cachedV = eigenDecomposition.GetV().GetSubMatrix(0, n - 1, 0, p - 1);
            }
            for (int i = 0; i < p; i++)
            {
                singularValues[i] = System.Math.Sqrt(System.Math.Abs(singularValues[i]));
            }
            // Up to this point, U and V are computed independently of each other.
            // There still a sign indetermination of each column of, say, U.
            // The sign is set such that A.V_i=sigma_i.U_i (i<=p)
            // The right sign corresponds to a positive dot product of A.V_i and U_i
            for (int i = 0; i < p; i++)
            {
                Vector<Double> tmp = cachedU.GetColumnVector(i);
                double product = matrix.Operate(cachedV.GetColumnVector(i)).DotProduct(tmp);
                if (product < 0)
                {
                    //cachedU.SetColumnVector(i, tmp.mapMultiply(-1.0));
                    cachedU.SetColumnVector(i, tmp.Multiply(-1.0));
                }
            }
            #endregion
        }

        /// <summary>
        ///   Constructs a new singular value decomposition.
        /// </summary>
        ///
        /// <param name="value">
        ///   The matrix to be decomposed.</param>
        ///
        public SingularValueDecomposition(Double[,] value)
            : this(value, true, true)
        {
        }


        /// <summary>
        ///     Constructs a new singular value decomposition.
        /// </summary>
        /// 
        /// <param name="value">
        ///   The matrix to be decomposed.</param>
        /// <param name="computeLeftSingularVectors">
        ///   Pass <see langword="true"/> if the left singular vector matrix U 
        ///   should be computed. Pass <see langword="false"/> otherwise. Default
        ///   is <see langword="true"/>.</param>
        /// <param name="computeRightSingularVectors">
        ///   Pass <see langword="true"/> if the right singular vector matrix V
        ///   should be computed. Pass <see langword="false"/> otherwise. Default
        ///   is <see langword="true"/>.</param>
        /// 
        public SingularValueDecomposition(Double[,] value,
            bool computeLeftSingularVectors, bool computeRightSingularVectors)
            : this(value, computeLeftSingularVectors, computeRightSingularVectors, false)
        {
        }

        /// <summary>
        ///   Constructs a new singular value decomposition.
        /// </summary>
        /// 
        /// <param name="value">
        ///   The matrix to be decomposed.</param>
        /// <param name="computeLeftSingularVectors">
        ///   Pass <see langword="true"/> if the left singular vector matrix U 
        ///   should be computed. Pass <see langword="false"/> otherwise. Default
        ///   is <see langword="true"/>.</param>
        /// <param name="computeRightSingularVectors">
        ///   Pass <see langword="true"/> if the right singular vector matrix V 
        ///   should be computed. Pass <see langword="false"/> otherwise. Default
        ///   is <see langword="true"/>.</param>
        /// <param name="autoTranspose">
        ///   Pass <see langword="true"/> to automatically transpose the value matrix in
        ///   case JAMA's assumptions about the dimensionality of the matrix are violated.
        ///   Pass <see langword="false"/> otherwise. Default is <see langword="false"/>.</param>
        /// 
        public SingularValueDecomposition(Double[,] value,
            bool computeLeftSingularVectors, bool computeRightSingularVectors, bool autoTranspose)
            : this(value, computeLeftSingularVectors, computeRightSingularVectors, autoTranspose, false)
        {
        }

        /// <summary>
        ///   Constructs a new singular value decomposition.
        /// </summary>
        /// 
        /// <param name="value">
        ///   The matrix to be decomposed.</param>
        /// <param name="computeLeftSingularVectors">
        ///   Pass <see langword="true"/> if the left singular vector matrix U 
        ///   should be computed. Pass <see langword="false"/> otherwise. Default
        ///   is <see langword="true"/>.</param>
        /// <param name="computeRightSingularVectors">
        ///   Pass <see langword="true"/> if the right singular vector matrix V 
        ///   should be computed. Pass <see langword="false"/> otherwise. Default
        ///   is <see langword="true"/>.</param>
        /// <param name="autoTranspose">
        ///   Pass <see langword="true"/> to automatically transpose the value matrix in
        ///   case JAMA's assumptions about the dimensionality of the matrix are violated.
        ///   Pass <see langword="false"/> otherwise. Default is <see langword="false"/>.</param>
        /// <param name="inPlace">
        ///   Pass <see langword="true"/> to perform the decomposition in place. The matrix
        ///   <paramref name="value"/> will be destroyed in the process, resulting in less
        ///   memory comsumption.</param>
        /// 
        public SingularValueDecomposition(Double[,] value,
           bool computeLeftSingularVectors, bool computeRightSingularVectors, bool autoTranspose, bool inPlace)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Matrix cannot be null.");

            Double[,] a;
            m = value.Rows();    // rows

            if (m == 0)
                throw new ArgumentException("Matrix does not have any rows.", "value");

            n = value.Columns(); // cols

            if (n == 0)
                throw new ArgumentException("Matrix does not have any columns.", "value");

            if (m < n) // Check if we are violating JAMA's assumption
            {
                if (!autoTranspose) // Yes, check if we should correct it
                {
                    // Warning! This routine is not guaranteed to work when A has less rows
                    //  than columns. If this is the case, you should compute SVD on the
                    //  transpose of A and then swap the left and right eigenvectors.

                    // However, as the solution found can still be useful, the exception below
                    // will not be thrown, and only a warning will be output in the trace.

                    // throw new ArgumentException("Matrix should have more rows than columns.");


                    Logger.Warning("Computing SVD on a matrix with more columns than rows.");

                    // Proceed anyway
                    a = inPlace ? value : value.Copy();
                }
                else
                {
                    // Transposing and swapping
                    a = value.Transpose(inPlace && m == n);
                    n = value.Rows();    // rows
                    m = value.Columns(); // cols
                    transposed = true;

                    bool aux = computeLeftSingularVectors;
                    computeLeftSingularVectors = computeRightSingularVectors;
                    computeRightSingularVectors = aux;
                }
            }
            else
            {
                // Input matrix is ok
                a = inPlace ? value : value.Copy();
            }


            int nu = System.Math.Min(m, n);
            int ni = System.Math.Min(m + 1, n);
            singularValues = new Double[ni];
            cachedU = MatrixUtility.CreateMatrix<Double>(m, nu);
            cachedV = MatrixUtility.CreateMatrix<Double>(n, n);

            Double[] e = new Double[n];
            Double[] work = new Double[m];
            bool wantu = computeLeftSingularVectors;
            bool wantv = computeRightSingularVectors;

            // Will store ordered sequence of indices after sorting.
            si = new int[ni]; for (int i = 0; i < ni; i++) si[i] = i;


            // Reduce A to bidiagonal form, storing the diagonal elements in s and the super-diagonal elements in e.
            int nct = System.Math.Min(m - 1, n);
            int nrt = System.Math.Max(0, System.Math.Min(n - 2, m));
            int mrc = System.Math.Max(nct, nrt);

            for (int k = 0; k < mrc; k++)
            {
                if (k < nct)
                {
                    // Compute the transformation for the k-th column and place the k-th diagonal in s[k].
                    // Compute 2-norm of k-th column without under/overflow.
                    singularValues[k] = 0;
                    for (int i = k; i < a.Rows(); i++)
                        singularValues[k] = Math2.Hypot(singularValues[k], a[i, k]);

                    if (singularValues[k] != 0)
                    {
                        if (a[k, k] < 0)
                            singularValues[k] = -singularValues[k];

                        for (int i = k; i < a.Rows(); i++)
                            a[i, k] /= singularValues[k];

                        a[k, k] += 1;
                    }

                    singularValues[k] = -singularValues[k];
                }

                for (int j = k + 1; j < n; j++)
                {
                    if ((k < nct) & (singularValues[k] != 0))
                    {
                        // Apply the transformation.
                        Double t = 0;
                        for (int i = k; i < a.Rows(); i++)
                            t += a[i, k] * a[i, j];

                        t = -t / a[k, k];

                        for (int i = k; i < a.Rows(); i++)
                            a[i, j] += t * a[i, k];
                    }

                    // Place the k-th row of A into e for the
                    // subsequent calculation of the row transformation.

                    e[j] = a[k, j];
                }

                if (wantu & (k < nct))
                {
                    // Place the transformation in U for subsequent back
                    // multiplication.

                    for (int i = k; i < a.Rows(); i++)
                        cachedU[i, k] = a[i, k];
                }

                if (k < nrt)
                {
                    // Compute the k-th row transformation and place the
                    // k-th super-diagonal in e[k].
                    // Compute 2-norm without under/overflow.
                    e[k] = 0;
                    for (int i = k + 1; i < e.Length; i++)
                        e[k] = Math2.Hypot(e[k], e[i]);

                    if (e[k] != 0)
                    {
                        if (e[k + 1] < 0)
                            e[k] = -e[k];

                        for (int i = k + 1; i < e.Length; i++)
                            e[i] /= e[k];

                        e[k + 1] += 1;
                    }

                    e[k] = -e[k];
                    if ((k + 1 < m) & (e[k] != 0))
                    {
                        // Apply the transformation.
                        for (int i = k + 1; i < work.Length; i++)
                            work[i] = 0;

                        for (int i = k + 1; i < a.Rows(); i++)
                            for (int j = k + 1; j < a.Columns(); j++)
                                work[i] += e[j] * a[i, j];

                        for (int j = k + 1; j < n; j++)
                        {
                            Double t = -e[j] / e[k + 1];
                            for (int i = k + 1; i < work.Length; i++)
                                a[i, j] += t * work[i];
                        }
                    }

                    if (wantv)
                    {
                        // Place the transformation in V for subsequent
                        // back multiplication.

                        for (int i = k + 1; i < cachedV.RowCount; i++)
                            cachedV[i, k] = e[i];
                    }
                }
            }

            // Set up the final bidiagonal matrix or order p.
            int p = System.Math.Min(n, m + 1);
            if (nct < n)
                singularValues[nct] = a[nct, nct];
            if (m < p)
                singularValues[p - 1] = 0;
            if (nrt + 1 < p)
                e[nrt] = a[nrt, p - 1];
            e[p - 1] = 0;

            // If required, generate U.
            if (wantu)
            {
                for (int j = nct; j < nu; j++)
                {
                    for (int i = 0; i < cachedU.RowCount; i++)
                        cachedU[i, j] = 0;

                    cachedU[j, j] = 1;
                }

                for (int k = nct - 1; k >= 0; k--)
                {
                    if (singularValues[k] != 0)
                    {
                        for (int j = k + 1; j < nu; j++)
                        {
                            Double t = 0;
                            for (int i = k; i < cachedU.RowCount; i++)
                                t += cachedU[i, k] * cachedU[i, j];

                            t = -t / cachedU[k, k];

                            for (int i = k; i < cachedU.RowCount; i++)
                                cachedU[i, j] += t * cachedU[i, k];
                        }

                        for (int i = k; i < cachedU.RowCount; i++)
                            cachedU[i, k] = -cachedU[i, k];

                        cachedU[k, k] = 1 + cachedU[k, k];
                        for (int i = 0; i < k - 1; i++)
                            cachedU[i, k] = 0;
                    }
                    else
                    {
                        for (int i = 0; i < cachedU.RowCount; i++)
                            cachedU[i, k] = 0;
                        cachedU[k, k] = 1;
                    }
                }
            }


            // If required, generate V.
            if (wantv)
            {
                for (int k = n - 1; k >= 0; k--)
                {
                    if ((k < nrt) & (e[k] != 0))
                    {
                        // TODO: The following is a pseudo correction to make SVD
                        //  work on matrices with n > m (less rows than columns).

                        // For the proper correction, compute the decomposition of the
                        //  transpose of A and swap the left and right eigenvectors

                        // Original line:
                        //   for (int j = k + 1; j < nu; j++)
                        // Pseudo correction:
                        //   for (int j = k + 1; j < n; j++)

                        for (int j = k + 1; j < n; j++) // pseudo-correction
                        {
                            Double t = 0;
                            for (int i = k + 1; i < cachedV.RowCount; i++)
                                t += cachedV[i, k] * cachedV[i, j];

                            t = -t / cachedV[k + 1, k];
                            for (int i = k + 1; i < cachedV.RowCount; i++)
                                cachedV[i, j] += t * cachedV[i, k];
                        }
                    }

                    for (int i = 0; i < cachedV.RowCount; i++)
                        cachedV[i, k] = 0;
                    cachedV[k, k] = 1;
                }
            }

            // Main iteration loop for the singular values.

            int pp = p - 1;
            int iter = 0;
            Double eps = Math2.DoubleEpsilon;
            while (p > 0)
            {
                int k, kase;

                // Here is where a test for too many iterations would go.

                // This section of the program inspects for
                // negligible elements in the s and e arrays.  On
                // completion the variables kase and k are set as follows.

                // kase = 1     if s(p) and e[k-1] are negligible and k<p
                // kase = 2     if s(k) is negligible and k<p
                // kase = 3     if e[k-1] is negligible, k<p, and
                //              s(k), ..., s(p) are not negligible (qr step).
                // kase = 4     if e(p-1) is negligible (convergence).

                for (k = p - 2; k >= -1; k--)
                {
                    if (k == -1)
                        break;

                    var alpha = TINY + eps * (System.Math.Abs(singularValues[k]) + System.Math.Abs(singularValues[k + 1]));
                    if (System.Math.Abs(e[k]) <= alpha || Double.IsNaN(e[k]))
                    {
                        e[k] = 0;
                        break;
                    }
                }

                if (k == p - 2)
                    kase = 4;

                else
                {
                    int ks;
                    for (ks = p - 1; ks >= k; ks--)
                    {
                        if (ks == k)
                            break;

                        Double t = (ks != p ? System.Math.Abs(e[ks]) : (Double)0) +
                                   (ks != k + 1 ? System.Math.Abs(e[ks - 1]) : (Double)0);

                        if (System.Math.Abs(singularValues[ks]) <= eps * t)
                        {
                            singularValues[ks] = 0;
                            break;
                        }
                    }

                    if (ks == k)
                        kase = 3;

                    else if (ks == p - 1)
                        kase = 1;

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
                            Double f = e[p - 2];
                            e[p - 2] = 0;
                            for (int j = p - 2; j >= k; j--)
                            {
                                Double t = Math2.Hypot(singularValues[j], f);
                                Double cs = singularValues[j] / t;
                                Double sn = f / t;
                                singularValues[j] = t;
                                if (j != k)
                                {
                                    f = -sn * e[j - 1];
                                    e[j - 1] = cs * e[j - 1];
                                }
                                if (wantv)
                                {
                                    for (int i = 0; i < cachedV.RowCount; i++)
                                    {
                                        t = cs * cachedV[i, j] + sn * cachedV[i, p - 1];
                                        cachedV[i, p - 1] = -sn * cachedV[i, j] + cs * cachedV[i, p - 1];
                                        cachedV[i, j] = t;
                                    }
                                }
                            }
                        }
                        break;

                    // Split at negligible s(k).

                    case 2:
                        {
                            Double f = e[k - 1];
                            e[k - 1] = 0;
                            for (int j = k; j < p; j++)
                            {
                                Double t = Math2.Hypot(singularValues[j], f);
                                Double cs = singularValues[j] / t;
                                Double sn = f / t;
                                singularValues[j] = t;
                                f = -sn * e[j];
                                e[j] = cs * e[j];
                                if (wantu)
                                {
                                    for (int i = 0; i < cachedU.RowCount; i++)
                                    {
                                        t = cs * cachedU[i, j] + sn * cachedU[i, k - 1];
                                        cachedU[i, k - 1] = -sn * cachedU[i, j] + cs * cachedU[i, k - 1];
                                        cachedU[i, j] = t;
                                    }
                                }
                            }
                        }
                        break;

                    // Perform one qr step.
                    case 3:
                        {
                            // Calculate the shift.
                            Double scale = System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Max(
                                    System.Math.Abs(singularValues[p - 1]), System.Math.Abs(singularValues[p - 2])), System.Math.Abs(e[p - 2])),
                                    System.Math.Abs(singularValues[k])), System.Math.Abs(e[k]));
                            Double sp = singularValues[p - 1] / scale;
                            Double spm1 = singularValues[p - 2] / scale;
                            Double epm1 = e[p - 2] / scale;
                            Double sk = singularValues[k] / scale;
                            Double ek = e[k] / scale;
                            Double b = ((spm1 + sp) * (spm1 - sp) + epm1 * epm1) / 2;
                            Double c = (sp * epm1) * (sp * epm1);
                            Double shift = 0;
                            if ((b != 0) || (c != 0))
                            {
                                if (b < 0)
                                    shift = -(Double)System.Math.Sqrt(b * b + c);
                                else
                                    shift = (Double)System.Math.Sqrt(b * b + c);
                                shift = c / (b + shift);
                            }

                            Double f = (sk + sp) * (sk - sp) + (Double)shift;
                            Double g = sk * ek;

                            // Chase zeros.
                            for (int j = k; j < p - 1; j++)
                            {
                                Double t = Math2.Hypot(f, g);
                                Double cs = f / t;
                                Double sn = g / t;

                                if (j != k)
                                    e[j - 1] = t;

                                f = cs * singularValues[j] + sn * e[j];
                                e[j] = cs * e[j] - sn * singularValues[j];
                                g = sn * singularValues[j + 1];
                                singularValues[j + 1] = cs * singularValues[j + 1];

                                if (wantv)
                                {
                                    for (int i = 0; i < cachedV.RowCount; i++)
                                    {
                                        t = cs * cachedV[i, j] + sn * cachedV[i, j + 1];
                                        cachedV[i, j + 1] = -sn * cachedV[i, j] + cs * cachedV[i, j + 1];
                                        cachedV[i, j] = t;
                                    }
                                }

                                t = Math2.Hypot(f, g);
                                cs = f / t;
                                sn = g / t;
                                singularValues[j] = t;
                                f = cs * e[j] + sn * singularValues[j + 1];
                                singularValues[j + 1] = -sn * e[j] + cs * singularValues[j + 1];
                                g = sn * e[j + 1];
                                e[j + 1] = cs * e[j + 1];

                                if (wantu && (j < m - 1))
                                {
                                    for (int i = 0; i < cachedU.RowCount; i++)
                                    {
                                        t = cs * cachedU[i, j] + sn * cachedU[i, j + 1];
                                        cachedU[i, j + 1] = -sn * cachedU[i, j] + cs * cachedU[i, j + 1];
                                        cachedU[i, j] = t;
                                    }
                                }
                            }

                            e[p - 2] = f;
                            iter = iter + 1;
                        }
                        break;

                    // Convergence.
                    case 4:
                        {
                            // Make the singular values positive.
                            if (singularValues[k] <= 0)
                            {
                                singularValues[k] = (singularValues[k] < 0 ? -singularValues[k] : (Double)0);

                                if (wantv)
                                {
                                    for (int i = 0; i <= pp; i++)
                                        cachedV[i, k] = -cachedV[i, k];
                                }
                            }

                            // Order the singular values.
                            while (k < pp)
                            {
                                if (singularValues[k] >= singularValues[k + 1])
                                    break;

                                Double t = singularValues[k];
                                singularValues[k] = singularValues[k + 1];
                                singularValues[k + 1] = t;
                                if (wantv && (k < n - 1))
                                {
                                    for (int i = 0; i < n; i++)
                                    {
                                        t = cachedV[i, k + 1];
                                        cachedV[i, k + 1] = cachedV[i, k];
                                        cachedV[i, k] = t;
                                    }
                                }

                                if (wantu && (k < m - 1))
                                {
                                    for (int i = 0; i < cachedU.RowCount; i++)
                                    {
                                        t = cachedU[i, k + 1];
                                        cachedU[i, k + 1] = cachedU[i, k];
                                        cachedU[i, k] = t;
                                    }
                                }

                                k++;
                            }

                            iter = 0;
                            p--;
                        }
                        break;
                }
            }

            // Remove unnecessary (last) singularValues if the matrix row is smaller than p.
            if (m < System.Math.Min(n, m + 1))
            {
                double[] temp = new double[m];
                Array.Copy(singularValues, temp, m);
                singularValues = temp;
            }

            // If we are violating JAMA's assumption about 
            // the input dimension, we need to swap u and v.
            if (transposed)
            {
                var temp = this.cachedU;
                this.cachedU = this.cachedV;
                this.cachedV = temp;
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
            double threshold = System.Math.Max(m, n) * System.Math2.Ulp(singularValues[0]);

            for (int i = singularValues.Length - 1; i >= 0; --i)
            {
                if (singularValues[i] > threshold)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Get a solver for finding the A &times; X = B solution in least square sense.
        /// </summary>
        /// <returns>a solver</returns>
        public IDecompositionSolver GetSolver()
        {
            return new Solver(singularValues, GetUT(), GetV(), GetRank() == System.Math.Max(m, n));
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
            public Solver(double[] singularValues, Matrix<Double> uT, Matrix<Double> v, Boolean nonSingular)
            {
                double[][] suT = uT.ToArray().ToJagged();
                for (int i = 0; i < singularValues.Length; i++)
                {
                    double a;
                    if (singularValues[i] > 0)
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
