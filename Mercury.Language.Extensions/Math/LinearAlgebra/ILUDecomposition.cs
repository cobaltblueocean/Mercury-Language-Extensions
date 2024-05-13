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
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.LinearAlgebra
{
    public interface ILUDecomposition<T> : ICloneable, ISolverMatrixDecomposition<T> where T : struct
    {
        /// <summary>
        ///   Returns if the matrix is non-singular (i.e. invertible).
        ///   Please see remarks for important information regarding
        ///   numerical stability when using this method.
        /// </summary>
        /// 
        /// <remarks>
        ///   Please keep in mind this is not one of the most reliable methods
        ///   for checking singularity of a matrix. For a more reliable method,
        ///   please use <see cref="Mercury.Language.Math.Matrix.MathNetMatrixUtility.IsSingular"/> or the 
        ///   <see cref="SingularValueDecomposition"/>.
        /// </remarks>
        Boolean Nonsingular { get; }

        double[,] Data { get; }

        /// <summary>
        /// Returns the matrix L of the decomposition.
        /// <p>L is an lower-triangular matrix</p>
        /// </summary>
        /// <returns>the L matrix (or null if decomposed matrix is singular)</returns>
        Matrix<Double> GetL();

        /// <summary>
        /// Returns the matrix U of the decomposition.
        /// <p>U is an upper-triangular matrix</p>
        /// </summary>
        /// <returns>the U matrix (or null if decomposed matrix is singular)</returns>
        Matrix<Double> GetU();

        /// <summary>
        /// Returns the P rows permutation matrix.
        /// <p>P is a sparse matrix with exactly one element set to 1.0 in
        /// each row and each column, all other elements being set to 0.0.</p>
        /// <p>The positions of the 1 elements are given by the {@link #getPivot()
        /// pivot permutation vector}.</p>
        /// </summary>
        /// <returns>the P rows permutation matrix (or null if decomposed matrix is singular)</returns>
        /// <see cref="#getPivot()"></see>
        Matrix<Double> GetP();

        /// <summary>
        /// Returns the pivot permutation vector.
        /// </summary>
        /// <returns>the pivot permutation vector</returns>
        /// <see cref="#getP()"></see>
        int[] PivotPermutationVector { get; }

        /// <summary>
        /// Return the determinant of the matrix
        /// </summary>
        /// <returns>determinant of the matrix</returns>
        double Determinant { get; }

        IDecompositionSolver GetSolver();
    }
}
