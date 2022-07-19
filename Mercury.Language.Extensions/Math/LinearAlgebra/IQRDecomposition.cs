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
using System;
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.LinearAlgebra
{
    public interface IQRDecomposition: ISolverMatrixDecomposition<Double>
    {
        double[,] Data { get; }
        double[] Diagonal { get; }
        bool FullRank { get; }
        double[,] OrthogonalFactor { get; }
        double[,] UpperTriangularFactor { get; }

        object Clone();
        Matrix<double> GetH();
        Matrix<double> GetQ();
        Matrix<double> GetQT();
        Matrix<double> GetR();
        IDecompositionSolver GetSolver();
        double[,] SolveTranspose(double[,] value);
    }
}