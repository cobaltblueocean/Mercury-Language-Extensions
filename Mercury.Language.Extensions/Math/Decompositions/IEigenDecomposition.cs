// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by Apache Software Foundation (ASF).
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.Decompositions
{
    /// <summary>
    /// An interface to classes that implement an algorithm to calculate the
    /// eigen decomposition of a real matrix.
    /// <p>The eigen decomposition of matrix A is a set of two matrices:
    /// V and D such that A = V &times; D &times; V<sup>T</sup>.
    /// A, V and D are all m &times; m matrices.</p>
    /// <p>This interface is similar in spirit to the <code>EigenvalueDecomposition</code>
    /// class from the <a href="http://math.nist.gov/javanumerics/jama/">JAMA</a>
    /// library, with the following changes:</p>
    /// <ul>
    ///   <li>a {@link #getVT() getVt} method has been added,</li>
    ///   <li>two {@link #getRealEigenvalue(int) getRealEigenvalue} and {@link #getImagEigenvalue(int)
    ///   getImagEigenvalue} methods to pick up a single eigenvalue have been added,</li>
    ///   <li>a {@link #getEigenvector(int) getEigenvector} method to pick up a single
    ///   eigenvector has been added,</li>
    ///   <li>a {@link #getDeterminant() getDeterminant} method has been added.</li>
    ///   <li>a {@link #Solver getSolver} method has been added.</li>
    /// </ul>
    /// </summary>
    /// <see cref="<a">href="http://mathworld.wolfram.com/EigenDecomposition.html">MathWorld</a> </see>
    /// <see cref="<a">href="http://en.wikipedia.org/wiki/Eigendecomposition_of_a_matrix">Wikipedia</a> </see>
    /// @version $Revision: 997726 $ $Date: 2010-09-16 14:39:51 +0200 (jeud 16 septd 2010) $
    /// @since 2.0
    public interface IEigenDecomposition<T> : ICloneable, ISolverMatrixDecomposition<T> where T : struct
    {
        /// <summary>
        /// Returns the matrix V of the decomposition.
        /// <p>V is an orthogonal matrix, i.ed its transpose is also its inverse.</p>
        /// <p>The columns of V are the eigenvectors of the original matrix.</p>
        /// <p>No assumption is made about the orientation of the system axes formed
        /// by the columns of V (e.gd in a 3-dimension space, V can form a left-
        /// or right-handed system).</p>
        /// </summary>
        /// <returns>the V matrix</returns>
        Matrix<Double> GetV();

        /// <summary>
        /// Returns the block diagonal matrix D of the decomposition.
        /// <p>D is a block diagonal matrix.</p>
        /// <p>Real eigenvalues are on the diagonal while complex values are on
        /// 2x2 blocks { {real +imaginary}, {-imaginary, real} }.</p>
        /// </summary>
        /// <returns>the D matrix</returns>
        /// <see cref="#getRealEigenvalues()"></see>
        /// <see cref="#getImagEigenvalues()"></see>
        Matrix<Double> GetD();

        /// <summary>
        /// Returns the transpose of the matrix V of the decomposition.
        /// <p>V is an orthogonal matrix, i.ed its transpose is also its inverse.</p>
        /// <p>The columns of V are the eigenvectors of the original matrix.</p>
        /// <p>No assumption is made about the orientation of the system axes formed
        /// by the columns of V (e.gd in a 3-dimension space, V can form a left-
        /// or right-handed system).</p>
        /// </summary>
        /// <returns>the transpose of the V matrix</returns>
        Matrix<Double> GetVT();

        /// <summary>
        /// Returns a copy of the real parts of the eigenvalues of the original matrix.
        /// </summary>
        /// <returns>a copy of the real parts of the eigenvalues of the original matrix</returns>
        /// <see cref="#D"></see>
        /// <see cref="#getRealEigenvalue(int)"></see>
        /// <see cref="#getImagEigenvalues()"></see>
        double[] GetRealEigenvalues();

        /// <summary>
        /// Returns the real part of the i<sup>th</sup> eigenvalue of the original matrix.
        /// </summary>
        /// <param Name="i">index of the eigenvalue (counting from 0)</param>
        /// <returns>real part of the i<sup>th</sup> eigenvalue of the original matrix</returns>
        /// <see cref="#D"></see>
        /// <see cref="#getRealEigenvalues()"></see>
        /// <see cref="#getImagEigenvalue(int)"></see>
        double GetRealEigenvalue(int i);

        /// <summary>
        /// Returns a copy of the imaginary parts of the eigenvalues of the original matrix.
        /// </summary>
        /// <returns>a copy of the imaginary parts of the eigenvalues of the original matrix</returns>
        /// <see cref="#D"></see>
        /// <see cref="#getImagEigenvalue(int)"></see>
        /// <see cref="#getRealEigenvalues()"></see>
        double[] GetImagEigenvalues();

        /// <summary>
        /// Returns the imaginary part of the i<sup>th</sup> eigenvalue of the original matrix.
        /// </summary>
        /// <param Name="i">index of the eigenvalue (counting from 0)</param>
        /// <returns>imaginary part of the i<sup>th</sup> eigenvalue of the original matrix</returns>
        /// <see cref="#D"></see>
        /// <see cref="#getImagEigenvalues()"></see>
        /// <see cref="#getRealEigenvalue(int)"></see>
        double GetImagEigenvalue(int i);

        /// <summary>
        /// Returns a copy of the i<sup>th</sup> eigenvector of the original matrix.
        /// </summary>
        /// <param Name="i">index of the eigenvector (counting from 0)</param>
        /// <returns>copy of the i<sup>th</sup> eigenvector of the original matrix</returns>
        /// <see cref="#D"></see>
        Vector<Double> GetEigenvector(int i);

        /// <summary>
        /// Return the determinant of the matrix
        /// </summary>
        /// <returns>determinant of the matrix</returns>
        double Determinant { get; }

        /// <summary>
        /// Get a solver for finding the A &times; X = B solution in exact linear sense.
        /// </summary>
        /// <returns>a solver</returns>
        IDecompositionSolver GetSolver();
    }
}
