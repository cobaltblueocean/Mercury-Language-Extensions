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
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Math.LinearAlgebra
{
    public interface ISingularValueDecomposition
    {
        double ConditionNumber { get; }
        double InverseConditionNumber { get; }
        double Norm { get; }
        double[] SingularValues { get; }

        Matrix<double> GetCovariance(double minSingularValue);
        int GetRank();
        Matrix<double> GetSigma();
        IDecompositionSolver GetSolver();
        Matrix<double> GetU();
        Matrix<double> GetUT();
        Matrix<double> GetV();
        Matrix<double> GetVT();
    }
}