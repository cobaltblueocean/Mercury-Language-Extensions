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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Mercury.Language.Math.Matrix;
using Mercury.Language.Math.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Extensions.Test.Decomposition
{
    /// <summary>
    /// CholeskyDecompositionTest Description
    /// </summary>
    public class CholeskyDecompositionTest
    {
        [Test]
        public void Test()
        {
            Double[][] A = new double[][] { new double[] { 10.0, 2.0, -1.0 }, new double[] { 2.0, 5.0, -2.0 }, new double[] { -1.0, -2.0, 15.0 } };
            var expMatrix = new double[][]{new double[]{ 3.1622776601683795, 0.6324555320336759, -0.31622776601683794},new double[]{ 0.0, 2.1447610589527217, -0.8392543274162825},new double[]{ 0.0, 0.0, 3.7677117954951176}};

            var matrix = MatrixUtility.CreateMatrix(A);
            var cholesky = new CholeskyDecomposition(matrix);

            var chMatrix = cholesky.GetLT().AsArrayEx();

            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(chMatrix[i,j ], expMatrix[i][j]);
                }
            }
            Assert.Pass();
        }
    }
}
