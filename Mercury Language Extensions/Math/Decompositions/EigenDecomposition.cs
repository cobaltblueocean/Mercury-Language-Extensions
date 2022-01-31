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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.Decompositions
{
    /// <summary>
    /// /// Calculates the eigen decomposition of a real <strong>symmetric</strong>
    /// matrix.
    /// <p>The eigen decomposition of matrix A is a set of two matrices:
    /// V and D such that A = V &times; D &times; V<sup>T</sup>.
    /// A, V and D are all m &times; m matrices.</p>
    /// This implementation is based on the paper by A. Drubrulle, R.S. Martin and
    /// J.H. Wilkinson "The Implicit QL Algorithm" in Wilksinson and Reinsch (1971)
    /// Handbook for automatic computation, vol. 2, Linear algebra, Springer-Verlag,
    /// New-York
    /// </p>
    /// @see <a href="http://mathworld.wolfram.com/EigenDecomposition.html">MathWorld</a>
    /// @see <a href="http://en.wikipedia.org/wiki/Eigendecomposition_of_a_matrix">Wikipedia</a>
    /// </summary>
    public class EigenDecomposition: IEigenDecomposition<Double>
    {
        /// <summary>
        /// Maximum number of iterations accepted in the implicit QL transformation
        /// </summary>
        private byte maxIter = 30;

        /// <summary>
        /// Main diagonal of the tridiagonal matrix.
        /// </summary>
        private double[] main;

        /// <summary>
        /// Secondary diagonal of the tridiagonal matrix.
        /// </summary>
        private double[] secondary;

        /// <summary>
        /// Transformer to tridiagonal (may be null if matrix is already tridiagonal).
        /// </summary>
        private TriDiagonalTransformer transformer;

        /// <summary>
        /// Real part of the realEigenvalues.
        /// </summary>
        private double[] realEigenvalues;

        /// <summary>
        /// Imaginary part of the realEigenvalues.
        /// </summary>
        private double[] imagEigenvalues;

        /// <summary>
        /// Eigenvectors.
        /// </summary>
        private MathNet.Numerics.LinearAlgebra.Vector<Double>[] eigenvectors;

        /// <summary>
        /// Cached value of V.
        /// </summary>
        private Matrix<Double> cachedV;

        /// <summary>
        /// Cached value of D.
        /// </summary>
        private Matrix<Double> cachedD;

        /// <summary>
        /// Cached value of Vt.
        /// </summary>
        private Matrix<Double> cachedVt;

        /// <summary>
        /// Is symmetric the Matrix?
        /// </summary>
        private Boolean isSymmetric;

        //private static uint SGN_MASK_FLOAT = 0x80000000;

        //public EigenDecomposition(Matrix<Double> matrix)
        //{
        //    double symTol = 10 * matrix.RowCount * matrix.ColumnCount * System.Double.Epsilon;

        //    isSymmetric = matrix.IsSymmetric(symTol);
        //    if (isSymmetric) {
        //        transformToTridiagonal(matrix);
        //        findEigenVectors(transformer.GetQ().AsArray());
        //    } else {
        //        SchurTransformer t = transformToSchur(matrix);
        //        findEigenVectorsFromSchur(t);
        //    }
        //}

        /// <summary>
        /// Calculates the eigen decomposition of the given symmetric matrix.
        /// </summary>
        /// <param name="matrix">Matrix to decompose. It must be symmetric.</param>
        /// <param name="splitTolerance">Dummy parameter (present for backward compatibility only).</param>
        public EigenDecomposition(Matrix<Double> matrix, double splitTolerance)// : this(matrix)
        {
            if (matrix.IsSymmetric())
            {
                isSymmetric = true;
                TransformToTridiagonal(matrix);
                FindEigenVectors(transformer.GetQ().AsArray());
            }
        }

        /// <summary>
        /// Calculates the eigen decomposition of the symmetric tridiagonal matrix.
        /// The Householder matrix is assumed to be the identity matrix.
        /// </summary>
        /// <param name="main">Main diagonal of the symmetric tridiagonal form.</param>
        /// <param name="secondary">Secondary of the tridiagonal form.</param>
        public EigenDecomposition(double[] main, double[] secondary)
        {
            isSymmetric = true;
            this.main = main;
            this.secondary = secondary;
            transformer = null;
            int size = main.Length;
            double[,] z = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                z[i, i] = 1.0;
            }
            FindEigenVectors(z);
        }

        /// <summary>
        /// Calculates the eigen decomposition of the symmetric tridiagonal matrix.
        /// The Householder matrix is assumed to be the identity matrix.
        /// </summary>
        /// <param name="main">Main diagonal of the symmetric tridiagonal form.</param>
        /// <param name="secondary">Secondary of the tridiagonal form.</param>
        /// <param name="splitTolerance">Dummy parameter (present for backward compatibility only).</param>
        public EigenDecomposition(double[] main, double[] secondary, double splitTolerance) : this(main, secondary)
        {

        }

        /// <summary>
        /// Gets the matrix V of the decomposition.
        /// V is an orthogonal matrix, i.e. its transpose is also its inverse.
        /// The columns of V are the eigenvectors of the original matrix.
        /// No assumption is made about the orientation of the system axes formed
        /// by the columns of V (e.g. in a 3-dimension space, V can form a left-
        /// or right-handed system).
        /// </summary>
        /// <returns>the V matrix.</returns>
        public Matrix<Double> GetV()
        {

            if (cachedV == null)
            {
                int m = eigenvectors.Length;
                DenseMatrix temp = new DenseMatrix(m, m);
                for (int k = 0; k < m; ++k)
                {
                    temp.SetColumnVector(k, (DenseVector)eigenvectors[k]);
                }
                cachedV = temp;
            }
            // return the cached matrix
            return cachedV;
        }

        /// <summary>
        /// Gets the block diagonal matrix D of the decomposition.
        /// D is a block diagonal matrix.
        /// Real eigenvalues are on the diagonal while complex values are on
        /// 2x2 blocks { {real +imaginary}, {-imaginary, real} }.
        /// </summary>
        /// <returns>the D matrix.</returns>
        public Matrix<Double> GetD()
        {

            if (cachedD == null)
            {
                // cache the matrix for subsequent calls
                cachedD = Matrix<Double>.Build.Dense(realEigenvalues.Length, realEigenvalues.Length);

                for (int i = 0; i < imagEigenvalues.Length; i++)
                {
                    if (Precision.CompareTo(imagEigenvalues[i], 0.0, System.Double.Epsilon) > 0)
                    {
                        cachedD.At(i, i + 1, imagEigenvalues[i]);
                    }
                    else if (Precision.CompareTo(imagEigenvalues[i], 0.0, System.Double.Epsilon) < 0)
                    {
                        cachedD.At(i, i - 1, imagEigenvalues[i]);
                    }
                }
            }
            return cachedD;
        }

        /// <summary>
        /// Gets the transpose of the matrix V of the decomposition.
        /// V is an orthogonal matrix, i.e. its transpose is also its inverse.
        /// The columns of V are the eigenvectors of the original matrix.
        /// No assumption is made about the orientation of the system axes formed
        /// by the columns of V (e.g. in a 3-dimension space, V can form a left-
        /// or right-handed system). 
        /// </summary>
        /// <returns>the transpose of the V matrix.</returns>
        public Matrix<Double> GetVT()
        {

            if (cachedVt == null)
            {
                int m = eigenvectors.Length;
                DenseMatrix temp = new DenseMatrix(m, m);
                for (int k = 0; k < m; ++k)
                {
                    temp.SetRowVector(k, (DenseVector)eigenvectors[k]);
                }

                cachedVt = temp;
            }

            // return the cached matrix
            return cachedVt;
        }

        /// <summary>
        /// Check if the Matrix has complex Eigenvalues or not
        /// </summary>
        /// <returns>True if the Matrix contins complex Eigenvalues, otherwise return False</returns>
        public Boolean HasComplexEigenvalues()
        {

            for (int i = 0; i < imagEigenvalues.Length; i++)
            {
                if (!imagEigenvalues[i].AlmostEqual(0.0, System.Double.Epsilon))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a copy of the real parts of the eigenvalues of the original matrix.
        /// </summary>
        public double[] GetRealEigenvalues()
        {
             return realEigenvalues; 
        }

        /// <summary>
        /// Returns the real part of the i<sup>th</sup> eigenvalue of the original matrix.
        /// </summary>
        /// <param name="i">index of the eigenvalue (counting from 0)</param>
        /// <returns>real part of the i<sup>th</sup> eigenvalue of the original matrix.</returns>
        public double GetRealEigenvalue(int i)
        {
            return realEigenvalues[i];
        }

        /// <summary>
        /// Gets a copy of the imaginary parts of the eigenvalues of the original matrix.
        /// </summary>
        public double[] GetImagEigenvalues()
        {
             return imagEigenvalues; 
        }

        /// <summary>
        /// Gets the imaginary part of the i<sup>th</sup> eigenvalue of the original matrix.
        /// </summary>
        /// <param name="i">Index of the eigenvalue (counting from 0).</param>
        /// <returns>the imaginary part of the i<sup>th</sup> eigenvalue of the original matrix.</returns>
        public double GetImagEigenvalue(int i)
        {
            return imagEigenvalues[i];
        }

        /// <summary>
        /// Gets a copy of the i<sup>th</sup> eigenvector of the original matrix.
        /// </summary>
        /// <param name="i">Index of the eigenvector (counting from 0).</param>
        /// <returns>a copy of the i<sup>th</sup> eigenvector of the original matrix.</returns>
        public MathNet.Numerics.LinearAlgebra.Vector<Double> GetEigenvector(int i)
        {
            return eigenvectors[i];
        }

        /// <summary>
        /// Computes the determinant of the matrix.
        /// </summary>
        /// <returns>the determinant of the matrix.</returns>
        public double Determinant
        {
            get
            {
                double determinant = 1;
                foreach (var lambda in realEigenvalues)
                {
                    determinant *= lambda;
                }
                return determinant;
            }
        }

        /// <summary>
        /// Computes the square root of the matrix.
        /// </summary>
        /// <returns>the square root of the matrix.</returns>
        public Matrix<Double> GetSquareRoot()
        {
            if (!isSymmetric)
            {
                throw new NotSupportedException();
            }

            double[] sqrtEigenValues = new double[realEigenvalues.Length];
            for (int i = 0; i < realEigenvalues.Length; i++)
            {
                double eigen = realEigenvalues[i];
                if (eigen <= 0)
                {
                    throw new NotSupportedException();
                }
                sqrtEigenValues[i] = System.Math.Sqrt(eigen);
            }

            Matrix<Double> sqrtEigen = Matrix<Double>.Build.Dense(sqrtEigenValues.Length, sqrtEigenValues.Length);
            for (int i = 0; i < sqrtEigenValues.Length; i++)
            {
                sqrtEigen.At(i, i, sqrtEigenValues[i]);
            }

            Matrix<Double> v = GetV();
            Matrix<Double> vT = GetVT();

            return (Matrix<Double>)v.Multiply(sqrtEigen).Multiply(vT);
        }

        /// <summary>
        /// Gets a solver for finding the A &times; X = B solution in exact linear sense.
        /// </summary>
        /// <returns>a solver.</returns>
        public IDecompositionSolver GetSolver()
        {
            if (HasComplexEigenvalues())
            {
                throw new NotSupportedException();
            }
            return new Solver(realEigenvalues, imagEigenvalues, eigenvectors);
        }

        /// <summary>
        /// Transform the Matrix to Tridiagonal
        /// </summary>
        /// <param name="matrix">A Matrix</param>
        private void TransformToTridiagonal(Matrix<Double> matrix)
        {
            // transform the matrix to tridiagonal
            transformer = new TriDiagonalTransformer((DenseMatrix)matrix);
            main = transformer.MainDiagonal;
            secondary = transformer.SecondaryDiagonal;
        }

        /// <summary>
        /// Find EigenVectors of the Matrix
        /// </summary>
        /// <param name="householderMatrix">A Matrix</param>
        private void FindEigenVectors(double[,] householderMatrix)
        {
            double[,] z = householderMatrix;
            int n = main.Length;
            realEigenvalues = new double[n];
            imagEigenvalues = new double[n];
            double[] e = new double[n];
            for (int i = 0; i < n - 1; i++)
            {
                realEigenvalues[i] = main[i];
                e[i] = secondary[i];
            }
            realEigenvalues[n - 1] = main[n - 1];
            e[n - 1] = 0;

            // Determine the largest main and secondary value in absolute term.
            double maxAbsoluteValue = 0;
            for (int i = 0; i < n; i++)
            {
                if (System.Math.Abs(realEigenvalues[i]) > maxAbsoluteValue)
                {
                    maxAbsoluteValue = System.Math.Abs(realEigenvalues[i]);
                }
                if (System.Math.Abs(e[i]) > maxAbsoluteValue)
                {
                    maxAbsoluteValue = System.Math.Abs(e[i]);
                }
            }
            // Make null any main and secondary value too small to be significant
            if (maxAbsoluteValue != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (System.Math.Abs(realEigenvalues[i]) <= System.Double.Epsilon * maxAbsoluteValue)
                    {
                        realEigenvalues[i] = 0;
                    }
                    if (System.Math.Abs(e[i]) <= System.Double.Epsilon * maxAbsoluteValue)
                    {
                        e[i] = 0;
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                int its = 0;
                int m;
                do
                {
                    for (m = j; m < n - 1; m++)
                    {
                        double delta = System.Math.Abs(realEigenvalues[m]) +
                            System.Math.Abs(realEigenvalues[m + 1]);
                        if (System.Math.Abs(e[m]) + delta == delta)
                        {
                            break;
                        }
                    }
                    if (m != j)
                    {
                        if (its == maxIter)
                        {
                            throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().CONVERGENCE_FAILED, maxIter));
                        }
                        its++;
                        double q = (realEigenvalues[j + 1] - realEigenvalues[j]) / (2 * e[j]);
                        double t = System.Math.Sqrt(1 + q * q);
                        if (q < 0.0)
                        {
                            q = realEigenvalues[m] - realEigenvalues[j] + e[j] / (q - t);
                        }
                        else
                        {
                            q = realEigenvalues[m] - realEigenvalues[j] + e[j] / (q + t);
                        }
                        double u = 0.0;
                        double s = 1.0;
                        double c = 1.0;
                        int i;
                        for (i = m - 1; i >= j; i--)
                        {
                            double p = s * e[i];
                            double h = c * e[i];
                            if (System.Math.Abs(p) >= System.Math.Abs(q))
                            {
                                c = q / p;
                                t = System.Math.Sqrt(c * c + 1.0);
                                e[i + 1] = p * t;
                                s = 1.0 / t;
                                c *= s;
                            }
                            else
                            {
                                s = p / q;
                                t = System.Math.Sqrt(s * s + 1.0);
                                e[i + 1] = q * t;
                                c = 1.0 / t;
                                s *= c;
                            }
                            if (e[i + 1] == 0.0)
                            {
                                realEigenvalues[i + 1] -= u;
                                e[m] = 0.0;
                                break;
                            }
                            q = realEigenvalues[i + 1] - u;
                            t = (realEigenvalues[i] - q) * s + 2.0 * c * h;
                            u = s * t;
                            realEigenvalues[i + 1] = q + u;
                            q = c * t - h;
                            for (int ia = 0; ia < n; ia++)
                            {
                                p = z[ia, i + 1];
                                z[ia, i + 1] = s * z[ia, i] + c * p;
                                z[ia, i] = c * z[ia, i] - s * p;
                            }
                        }
                        if (t == 0.0 && i >= j)
                        {
                            continue;
                        }
                        realEigenvalues[j] -= u;
                        e[j] = q;
                        e[m] = 0.0;
                    }
                } while (m != j);
            }

            //Sort the eigen values (and vectors) in increase order
            for (int i = 0; i < n; i++)
            {
                int k = i;
                double p = realEigenvalues[i];
                for (int j = i + 1; j < n; j++)
                {
                    if (realEigenvalues[j] > p)
                    {
                        k = j;
                        p = realEigenvalues[j];
                    }
                }
                if (k != i)
                {
                    realEigenvalues[k] = realEigenvalues[i];
                    realEigenvalues[i] = p;
                    for (int j = 0; j < n; j++)
                    {
                        p = z[j, i];
                        z[j, i] = z[j, k];
                        z[j, k] = p;
                    }
                }
            }

            // Determine the largest eigen value in absolute term.
            maxAbsoluteValue = 0;
            for (int i = 0; i < n; i++)
            {
                if (System.Math.Abs(realEigenvalues[i]) > maxAbsoluteValue)
                {
                    maxAbsoluteValue = System.Math.Abs(realEigenvalues[i]);
                }
            }
            // Make null any eigen value too small to be significant
            if (maxAbsoluteValue != 0.0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (System.Math.Abs(realEigenvalues[i]) < System.Double.Epsilon * maxAbsoluteValue)
                    {
                        realEigenvalues[i] = 0;
                    }
                }
            }
            eigenvectors = new MathNet.Numerics.LinearAlgebra.Vector<Double>[n];
            double[] tmp = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tmp[j] = z[j, i];
                }
                eigenvectors[i] = MathNet.Numerics.LinearAlgebra.Vector<Double>.Build.Dense(tmp);
            }
        }

        /// <summary>
        /// Divides a complex number by another complex number
        /// </summary>
        /// <param name="xr">a real part of comlex number to be devided</param>
        /// <param name="xi">an imaginary part of comlex number to be devided</param>
        /// <param name="yr">a real part of comlex number to devide</param>
        /// <param name="yi">an imaginary part of comlex number to devide</param>
        /// <returns>devided complex value</returns>
        private System.Numerics.Complex Cdiv(double xr, double xi, double yr, double yi)
        {
            return System.Numerics.Complex.Divide(new System.Numerics.Complex(xr, xi), new System.Numerics.Complex(yr, yi));
        }

        public object Clone()
        {
            return new EigenDecomposition(this.main, this.secondary);
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
            throw new NotImplementedException();
        }

        public double[,] GetInformationMatrix()
        {
            throw new NotImplementedException();
        }

        public double[,] Reverse()
        {
            throw new NotImplementedException();
        }

        #region Unused methods
        //private SchurTransformer transformToSchur(Matrix<Double> matrix)
        //{
        //    SchurTransformer schurTransform = new SchurTransformer(matrix);
        //    double[][] matT = schurTransform.getT().getData();

        //    realEigenvalues = new double[matT.Length];
        //    imagEigenvalues = new double[matT.Length];

        //    for (int i = 0; i < realEigenvalues.Length; i++)
        //    {
        //        if (i == (realEigenvalues.Length - 1) ||
        //            Precision.AlmostEqual(matT[i + 1][i], 0.0, System.Double.Epsilon))
        //        {
        //            realEigenvalues[i] = matT[i][i];
        //        }
        //        else
        //        {
        //            double x = matT[i + 1][i + 1];
        //            double p = 0.5 * (matT[i][i] - x);
        //            double z = System.Math.Sqrt(System.Math.Abs(p * p + matT[i + 1][i] * matT[i][i + 1]));
        //            realEigenvalues[i] = x + p;
        //            imagEigenvalues[i] = z;
        //            realEigenvalues[i + 1] = x + p;
        //            imagEigenvalues[i + 1] = -z;
        //            i++;
        //        }
        //    }
        //    return schurTransform;
        //}


        //private static Boolean PrecisionEquals(float x, float y, float eps)
        //{
        //    return PrecisionEquals(x, y, 1) || System.Math.Abs(y - x) <= eps;
        //}

        //private static Boolean PrecisionEquals(float x, float y, int maxUlps)
        //{
        //    int xInt = System.Single.floatToIntBits(x);
        //    int yInt = System.Single.floatToIntBits(y);

        //    // Make lexicographically ordered as a two's-complement integer.
        //    if (xInt < 0)
        //    {
        //        xInt = SGN_MASK_FLOAT - xInt;
        //    }
        //    if (yInt < 0)
        //    {
        //        yInt = SGN_MASK_FLOAT - yInt;
        //    }

        //    Boolean isEqual = System.Math.Abs(xInt - yInt) <= maxUlps;

        //    return isEqual && !System.Single.IsNaN(x) && !System.Single.IsNaN(y);
        //}

        //private void findEigenVectorsFromSchur(SchurTransformer schur)
        //{
        //    double[][] matrixT = schur.getT().getData();
        //    double[][] matrixP = schur.getP().getData();

        //    int n = matrixT.Length;

        //    // compute matrix norm
        //    double norm = 0.0;
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = System.Math.Max(i - 1, 0); j < n; j++)
        //        {
        //            norm += System.Math.Abs(matrixT[i][j]);
        //        }
        //    }

        //    // we can not handle a matrix with zero norm
        //    if (Precision.AlmostEqual(norm, 0.0, System.Double.Epsilon))
        //    {
        //        throw new MathArithmeticException(LocalizedResources.Instance().ZERO_NORM);
        //    }

        //    // Backsubstitute to find vectors of upper triangular form

        //    double r = 0.0;
        //    double s = 0.0;
        //    double z = 0.0;

        //    for (int idx = n - 1; idx >= 0; idx--)
        //    {
        //        double p = realEigenvalues[idx];
        //        double q = imagEigenvalues[idx];

        //        if (Precision.Equals(q, 0.0))
        //        {
        //            // Real vector
        //            int l = idx;
        //            matrixT[idx][idx] = 1.0;
        //            for (int i = idx - 1; i >= 0; i--)
        //            {
        //                double w = matrixT[i][i] - p;
        //                r = 0.0;
        //                for (int j = l; j <= idx; j++)
        //                {
        //                    r += matrixT[i][j] * matrixT[j][idx];
        //                }
        //                if (Precision.CompareTo(imagEigenvalues[i], 0.0, System.Double.Epsilon) < 0)
        //                {
        //                    z = w;
        //                    s = r;
        //                }
        //                else
        //                {
        //                    l = i;
        //                    if (Precision.Equals(imagEigenvalues[i], 0.0))
        //                    {
        //                        if (w != 0.0)
        //                        {
        //                            matrixT[i][idx] = -r / w;
        //                        }
        //                        else
        //                        {
        //                            matrixT[i][idx] = -r / (System.Double.Epsilon * norm);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Solve real equations
        //                        double x = matrixT[i][i + 1];
        //                        double y = matrixT[i + 1][i];
        //                        q = (realEigenvalues[i] - p) * (realEigenvalues[i] - p) +
        //                                                        imagEigenvalues[i] * imagEigenvalues[i];
        //                        double t = (x * s - z * r) / q;
        //                        matrixT[i][idx] = t;
        //                        if (System.Math.Abs(x) > System.Math.Abs(z))
        //                        {
        //                            matrixT[i + 1][idx] = (-r - w * t) / x;
        //                        }
        //                        else
        //                        {
        //                            matrixT[i + 1][idx] = (-s - y * t) / z;
        //                        }
        //                    }

        //                    // Overflow control
        //                    double t2 = System.Math.Abs(matrixT[i][idx]);
        //                    if ((System.Double.Epsilon * t2) * t2 > 1)
        //                    {
        //                        for (int j = i; j <= idx; j++)
        //                        {
        //                            matrixT[j][idx] /= t2;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (q < 0.0)
        //        {
        //            // Complex vector
        //            int l = idx - 1;

        //            // Last vector component imaginary so matrix is triangular
        //            if (System.Math.Abs(matrixT[idx][idx - 1]) > System.Math.Abs(matrixT[idx - 1][idx]))
        //            {
        //                matrixT[idx - 1][idx - 1] = q / matrixT[idx][idx - 1];
        //                matrixT[idx - 1][idx] = -(matrixT[idx][idx] - p) / matrixT[idx][idx - 1];
        //            }
        //            else
        //            {
        //                System.Numerics.Complex result = cdiv(0.0, -matrixT[idx - 1][idx],
        //                                            matrixT[idx - 1][idx - 1] - p, q);
        //                matrixT[idx - 1][idx - 1] = result.Real;
        //                matrixT[idx - 1][idx] = result.Imaginary;
        //            }

        //            matrixT[idx][idx - 1] = 0.0;
        //            matrixT[idx][idx] = 1.0;

        //            for (int i = idx - 2; i >= 0; i--)
        //            {
        //                double ra = 0.0;
        //                double sa = 0.0;
        //                for (int j = l; j <= idx; j++)
        //                {
        //                    ra += matrixT[i][j] * matrixT[j][idx - 1];
        //                    sa += matrixT[i][j] * matrixT[j][idx];
        //                }
        //                double w = matrixT[i][i] - p;

        //                if (Precision.CompareTo(imagEigenvalues[i], 0.0, System.Double.Epsilon) < 0)
        //                {
        //                    z = w;
        //                    r = ra;
        //                    s = sa;
        //                }
        //                else
        //                {
        //                    l = i;
        //                    if (Precision.Equals(imagEigenvalues[i], 0.0))
        //                    {
        //                        System.Numerics.Complex c = cdiv(-ra, -sa, w, q);
        //                        matrixT[i][idx - 1] = c.Real;
        //                        matrixT[i][idx] = c.Imaginary;
        //                    }
        //                    else
        //                    {
        //                        // Solve complex equations
        //                        double x = matrixT[i][i + 1];
        //                        double y = matrixT[i + 1][i];
        //                        double vr = (realEigenvalues[i] - p) * (realEigenvalues[i] - p) +
        //                                    imagEigenvalues[i] * imagEigenvalues[i] - q * q;
        //                        double vi = (realEigenvalues[i] - p) * 2.0 * q;
        //                        if (Precision.Equals(vr, 0.0) && Precision.Equals(vi, 0.0))
        //                        {
        //                            vr = System.Double.Epsilon * norm *
        //                                 (System.Math.Abs(w) + System.Math.Abs(q) + System.Math.Abs(x) +
        //                                  System.Math.Abs(y) + System.Math.Abs(z));
        //                        }
        //                        System.Numerics.Complex c = cdiv(x * r - z * ra + q * sa,
        //                                                   x * s - z * sa - q * ra, vr, vi);
        //                        matrixT[i][idx - 1] = c.Real;
        //                        matrixT[i][idx] = c.Imaginary;

        //                        if (System.Math.Abs(x) > (System.Math.Abs(z) + System.Math.Abs(q)))
        //                        {
        //                            matrixT[i + 1][idx - 1] = (-ra - w * matrixT[i][idx - 1] +
        //                                                       q * matrixT[i][idx]) / x;
        //                            matrixT[i + 1][idx] = (-sa - w * matrixT[i][idx] -
        //                                                       q * matrixT[i][idx - 1]) / x;
        //                        }
        //                        else
        //                        {
        //                            System.Numerics.Complex c2 = cdiv(-r - y * matrixT[i][idx - 1],
        //                                                           -s - y * matrixT[i][idx], z, q);
        //                            matrixT[i + 1][idx - 1] = c2.Real;
        //                            matrixT[i + 1][idx] = c2.Imaginary;
        //                        }
        //                    }

        //                    // Overflow control
        //                    double t = System.Math.Max(System.Math.Abs(matrixT[i][idx - 1]),
        //                                            System.Math.Abs(matrixT[i][idx]));
        //                    if ((System.Double.Epsilon * t) * t > 1)
        //                    {
        //                        for (int j = i; j <= idx; j++)
        //                        {
        //                            matrixT[j][idx - 1] /= t;
        //                            matrixT[j][idx] /= t;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Back transformation to get eigenvectors of original matrix
        //    for (int j = n - 1; j >= 0; j--)
        //    {
        //        for (int i = 0; i <= n - 1; i++)
        //        {
        //            z = 0.0;
        //            for (int k = 0; k <= System.Math.Min(j, n - 1); k++)
        //            {
        //                z += matrixP[i][k] * matrixT[k][j];
        //            }
        //            matrixP[i][j] = z;
        //        }
        //    }

        //    eigenvectors = new Vector<Double>[n];
        //    double[] tmp = new double[n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            tmp[j] = matrixP[j][i];
        //        }
        //        eigenvectors[i] = new Vector<Double>(tmp);
        //    }
        //}

        #endregion

        /// <summary>
        /// Specialized solver.
        /// </summary>
        public class Solver : IDecompositionSolver
        {
            /// <summary>Real part of the realEigenvalues. */
            private double[] realEigenvalues;
            /// <summary>Imaginary part of the realEigenvalues. */
            private double[] imagEigenvalues;
            /// <summary>Eigenvectors. */
            private MathNet.Numerics.LinearAlgebra.Vector<Double>[] eigenvectors;

            /// <summary>
            /// Builds a solver from decomposed matrix.
            /// 
            /// </summary>
            /// <param name="realEigenvalues">Real parts of the eigenvalues.</param>
            /// <param name="imagEigenvalues">Imaginary parts of the eigenvalues.</param>
            /// <param name="eigenvectors">Eigenvectors.</param>
            public Solver(double[] realEigenvalues, double[] imagEigenvalues, MathNet.Numerics.LinearAlgebra.Vector<Double>[] eigenvectors)
            {
                this.realEigenvalues = realEigenvalues;
                this.imagEigenvalues = imagEigenvalues;
                this.eigenvectors = eigenvectors;
            }

            /// <summary>
            /// Solves the linear equation A &times; X = B for symmetric matrices A.
            /// This method only finds exact linear solutions, i.e. solutions for
            /// which ||A &times; X - B|| is exactly 0.
            /// </summary>
            /// <param name="b">Right-hand side of the equation A &times; X = B.</param>
            /// <returns>a Vector X that minimizes the two norm of A &times; X - B.</returns>
            public MathNet.Numerics.LinearAlgebra.Vector<Double> Solve(MathNet.Numerics.LinearAlgebra.Vector<Double> b)
            {
                if (!IsNonSingular)
                {
                    throw new SingularMatrixException();
                }

                int m = realEigenvalues.Length;
                if (b.Count != m)
                {
                    throw new DimensionMismatchException(b.Count, m);
                }

                double[] bp = new double[m];
                for (int i = 0; i < m; ++i)
                {
                    MathNet.Numerics.LinearAlgebra.Vector<Double> v = eigenvectors[i];
                    double[] vData = v.AsArray();
                    double s = v.DotProduct(b) / realEigenvalues[i];
                    for (int j = 0; j < m; ++j)
                    {
                        bp[j] += s * vData[j];
                    }
                }

                return MathNet.Numerics.LinearAlgebra.Vector<Double>.Build.Dense(bp);
            }

            /// <summary>
            /// Solve the linear equation A &times; X = B for matrices A.
            /// <p>The A matrix is implicit, it is provided by the underlying
            /// decomposition algorithm.</p>
            /// </summary>
            /// <param name="b">right-hand side of the equation A &times; X = B</param>
            /// <returns>a matrix X that minimizes the two norm of A &times; X - B</returns>
            public Matrix<Double> Solve(Matrix<Double> b)
            {
                if (!IsNonSingular)
                {
                    throw new SingularMatrixException();
                }

                int m = realEigenvalues.Length;
                if (b.RowCount != m)
                {
                    throw new DimensionMismatchException(b.RowCount, m);
                }

                int nColB = b.ColumnCount;
                double[,] bp = new double[m, nColB];
                double[] tmpCol = new double[m];
                for (int k = 0; k < nColB; ++k)
                {
                    for (int i = 0; i < m; ++i)
                    {
                        tmpCol[i] = b[i, k];
                        bp[i, k] = 0;
                    }
                    for (int i = 0; i < m; ++i)
                    {
                        MathNet.Numerics.LinearAlgebra.Vector<Double> v = eigenvectors[i];
                        double[] vData = v.AsArray();
                        double s = 0;
                        for (int j = 0; j < m; ++j)
                        {
                            s += v[j] * tmpCol[j];
                        }
                        s /= realEigenvalues[i];
                        for (int j = 0; j < m; ++j)
                        {
                            bp[j, k] += s * vData[j];
                        }
                    }
                }

                var ret = new DenseMatrix(bp.GetLength(0), bp.GetLength(1));
                ret.Load(bp);

                return ret;
            }

            /// <summary>
            /// Checks whether the decomposed matrix is non-singular.
            /// </summary>
            /// <returns>true if the decomposed matrix is non-singular.</returns>
            public Boolean IsNonSingular
            {
                get
                {
                    double largestEigenvalueNorm = 0.0;
                    // Looping over all values (in case they are not sorted in decreasing
                    // order of their norm).
                    for (int i = 0; i < realEigenvalues.Length; ++i)
                    {
                        largestEigenvalueNorm = System.Math.Max(largestEigenvalueNorm, EigenvalueNorm(i));
                    }
                    // Corner case: zero matrix, all exactly 0 eigenvalues
                    if (largestEigenvalueNorm == 0.0)
                    {
                        return false;
                    }
                    for (int i = 0; i < realEigenvalues.Length; ++i)
                    {
                        // Looking for eigenvalues that are 0, where we consider anything much much smaller
                        // than the largest eigenvalue to be effectively 0.
                        if (Precision.AlmostEqual(EigenvalueNorm(i) / largestEigenvalueNorm, 0, System.Double.Epsilon))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            /// <summary>
            /// Get Eigenvalue Norm at the indexed matrix.
            /// </summary>
            /// <param name="i">Index of the eigenvector (counting from 0).</param>
            /// <returns></returns>
            private double EigenvalueNorm(int i)
            {
                double re = realEigenvalues[i];
                double im = imagEigenvalues[i];
                return System.Math.Sqrt(re * re + im * im);
            }

            /// <summary>
            /// Get the inverse of the decomposed matrix.
            /// </summary>
            /// <returns>the inverse matrix.</returns>
            public Matrix<Double> GetInverse()
            {
                if (!IsNonSingular)
                {
                    throw new SingularMatrixException();
                }

                int m = realEigenvalues.Length;
                double[,] invData = new double[m, m];

                for (int i = 0; i < m; ++i)
                {
                    for (int j = 0; j < m; ++j)
                    {
                        double invIJ = 0;
                        for (int k = 0; k < m; ++k)
                        {
                            double[] vK = eigenvectors[k].AsArray();
                            invIJ += vK[i] * vK[j] / realEigenvalues[k];
                        }
                        invData[i, j] = invIJ;
                    }
                }

                var retmtx = new DenseMatrix(invData.GetLength(0), invData.GetLength(1));
                retmtx.Load(invData);

                return retmtx;
            }

            public double[] Solve(double[] b)
            {
                if (!IsNonSingular)
                {
                    throw new SingularMatrixException();
                }

                int m = realEigenvalues.Length;
                if (b.Length != m)
                {
                    throw new DimensionMismatchException(b.Length, m);
                }

                double[] bp = new double[m];
                for (int i = 0; i < m; ++i)
                {
                    MathNet.Numerics.LinearAlgebra.Vector<Double> v = eigenvectors[i];
                    double[] vData = v.AsArray();
                    double s = v.DotProduct(MathNet.Numerics.LinearAlgebra.Vector<Double>.Build.Dense(b)) / realEigenvalues[i];
                    for (int j = 0; j < m; ++j)
                    {
                        bp[j] += s * vData[j];
                    }
                }

                return bp;
            }

            public double[][] Solve(double[][] value)
            {
                return Solve(value.ToMultidimensional()).ToJagged();
            }

            public double[,] Solve(double[,] b)
            {
                if (!IsNonSingular)
                {
                    throw new SingularMatrixException();
                }

                int m = realEigenvalues.Length;
                if (b.Length != m)
                {
                    throw new DimensionMismatchException(b.Length, m);
                }

                int nColB = b.GetLength(1);
                double[,] bp = new double[m, nColB];
                double[] tmpCol = new double[m];
                for (int k = 0; k < nColB; ++k)
                {
                    for (int i = 0; i < m; ++i)
                    {
                        tmpCol[i] = b[i, k];
                        bp[i, k] = 0;
                    }
                    for (int i = 0; i < m; ++i)
                    {
                        MathNet.Numerics.LinearAlgebra.Vector<Double> v = eigenvectors[i];
                        double[] vData = v.AsArray();
                        double s = 0;
                        for (int j = 0; j < m; ++j)
                        {
                            s += v[j] * tmpCol[j];
                        }
                        s /= realEigenvalues[i];
                        for (int j = 0; j < m; ++j)
                        {
                            bp[j, k] += s * vData[j];
                        }
                    }
                }

                return bp;
            }
        }
    }
}
