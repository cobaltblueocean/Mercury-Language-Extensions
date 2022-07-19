// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Math.Matrix;
using Mercury.Language.Math.LinearAlgebra;

namespace Mercury.Language.Extensions.Test.Decomposition
{
    /// <summary>
    /// EigenDecompositionTest Description
    /// </summary>
    public class EigenDecompositionTest
    {
        [Test]
        public void Test()
        {
            Double expDeterminant = -2.9999999999999982;
            Double[,] value = new double[,] { { 1, 2 }, { 2, 1 } };
            var m = MatrixUtility.CreateMatrix(value);
            var eigen = new EigenDecomposition(m, 0.0);

            Assert.AreEqual(expDeterminant, eigen.Determinant);
        }
    }
}
