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

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// Interface handling decomposition algorithms that can solve A &times; X = B.
    /// <p>Decomposition algorithms decompose an A matrix has a product of several specific
    /// matrices from which they can solve A &times; X = B in least squares sense: they find X
    /// such that ||A &times; X - B|| is minimal.</p>
    /// <p>Some solvers like {@link LUDecomposition} can only find the solution for
    /// square matrices and when the solution is an exact linear solution, i.e. when
    /// ||A &times; X - B|| is exactly 0. Other solvers can also find solutions
    /// with non-square matrix A and with non-null minimal norm. If an exact linear
    /// solution exists it is also the minimal norm solution.</p>
    /// 
    /// @version $Revision: 811685 $ $Date: 2009-09-05 19:36:48 +0200 (sam. 05 sept. 2009) $
    /// @since 2.0
    /// </summary>
    public interface IDecompositionSolver
    {
        /// <summary>Solve the linear equation A &times; X = B for matrices A.
        /// <p>The A matrix is implicit, it is provided by the underlying
        /// decomposition algorithm.</p>
        /// </summary>
        /// <param name="b">right-hand side of the equation A &times; X = B</param>
        /// <returns>a vector X that minimizes the two norm of A &times; X - B</returns>
        double[] Solve(double[] b);

        /// <summary>Solve the linear equation A &times; X = B for matrices A.
        /// <p>The A matrix is implicit, it is provided by the underlying
        /// decomposition algorithm.</p>
        /// </summary>
        /// <param name="b">right-hand side of the equation A &times; X = B</param>
        /// <returns>a vector X that minimizes the two norm of A &times; X - B</returns>
        Vector<double> Solve(Vector<double> b);

        /// <summary>Solve the linear equation A &times; X = B for matrices A.
        /// <p>The A matrix is implicit, it is provided by the underlying
        /// decomposition algorithm.</p>
        /// </summary>
        /// <param name="b">right-hand side of the equation A &times; X = B</param>
        /// <returns>a matrix X that minimizes the two norm of A &times; X - B</returns>
        Matrix<Double> Solve(Matrix<Double> b);

        /// <summary>
        ///   Solves a set of equation systems of type <c>A * X = B</c>.
        /// </summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * U * X = B</c>.</returns>
        Double[][] Solve(Double[][] value);

        /// <summary>
        ///   Solves a set of equation systems of type <c>A * X = B</c>.
        /// </summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * U * X = B</c>.</returns>
        Double[,] Solve(Double[,] value);


        /// <summary>
        /// Check if the decomposed matrix is non-singular.
        /// </summary>
        /// <returns>true if the decomposed matrix is non-singular</returns>
        Boolean IsNonSingular { get; }

        /// <summary>Get the inverse (or pseudo-inverse) of the decomposed matrix.
        /// </summary>
        /// <returns>inverse matrix</returns>
        /// <exception cref="InvalidMatrixException">if decomposed matrix is singular </exception>
        Matrix<Double> GetInverse();
    }
}
