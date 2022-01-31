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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.Decompositions
{
    public interface ICholeskyDecomposition<T> : ICloneable, ISolverMatrixDecomposition<T> where T : struct
    {
        /// <summary>
        /// Returns the matrix L of the decomposition.
        /// <p>L is an lower-triangular matrix</p>
        /// </summary>
        /// <returns>the L matrix</returns>
        Matrix<Double> GetL();

        /// <summary>
        /// Returns the transpose of the matrix L of the decomposition.
        /// <p>L<sup>T</sup> is an upper-triangular matrix</p>
        /// </summary>
        /// <returns>the transpose of the matrix L of the decomposition</returns>
        Matrix<Double> GetLT();

        /// <summary>
        /// Return the determinant of the matrix
        /// </summary>
        /// <returns>determinant of the matrix</returns>
        double Determinant { get; }

        /// <summary>
        /// Get a solver for finding the A &times; X = B solution in least square sense.
        /// </summary>
        /// <returns>a solver</returns>
        IDecompositionSolver GetSolver();
    }
}
