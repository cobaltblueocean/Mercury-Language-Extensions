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
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Language.Math.Matrix;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Mercury.Test.Utility;

namespace Mercury.Language.Extensions.Test.Decomposition
{
    /// <summary>
    /// SingleValueDecompositionTest Description
    /// </summary>
    public class SinglarValueDecompositionTest
    {

        private double eps = 1e-13;

        [Test]
        public void Test()
        {
            Matrix<Double> A = MatrixUtility.CreateMatrix(new double[][] { new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 } });

            double[] x = new double[3];
            //x[0] = Math2.Random();
            //x[1] = Math2.Random();
            //x[2] = Math2.Random();
            x[0] = 0.40313338447982294;
            x[1] = 0.5215001209732417;
            x[2] = 0.11364736745315263;

            double[] u = new double[2];
            double[] v = new double[3];

            var rWtWRt = MatrixUtility.CreateMatrix<Double>(new double[,] { { 62081983946.74736, 60654260990.086517, 60617845313.867073, 59879654204.350334, 61922746873.988861 },
                                         { 60654260990.086548,59260098981.620117,59225251954.187042,58504717536.27533,60501517727.089394},
                                         { 60617845313.867073,59225251954.187042,59191161616.653465,58471744289.66951,60468043285.484619},
                                         { 59879654204.350327,58504717536.275314,58471744289.669518,57761740902.766129,59734394974.261719},
                                         { 61922746873.988869,60501517727.089371,60468043285.484604,59734394974.261719,61774947493.495926} });

            var rWtWR = MatrixUtility.CreateRealVector(new double[] { -305156491329.04016, -298145847189.25842, -297974046459.86169, -294352251907.323, -304401650354.3205 });

            SingularValueDecomposition svdA = new SingularValueDecomposition(rWtWRt);

            var expectedU = new double[,] { {-0.4548481394860997,-0.6466829262860327,0.4104165317385213,-0.0,-0.4543925982362497,},
                                            {-0.4443984680775086,-0.3081722653748162,-0.1835131542642888,0.39850186331834014,0.7176761411443879},
                                            {-0.44414237030182135,0.01768047803122978,-0.7841987874815886,-0.3224998312097539,-0.28887899920443144},
                                            {-0.4387439181852749,0.3281623627113635,0.4012972502348073,-0.6533073755413294,0.3346090673792501},
                                            {-0.4537229338864302,0.6154899772540325,0.14789851707871798,0.5571172200397252,-0.2881909418188661}};

            var expectedVT = new double[,] { {-0.4548481394860998,-0.4443984680775087,-0.44414237030182113,-0.438743918185275,-0.45372293388643015},
                                            {-0.6466828252907099,-0.3081723122148135,0.017680340878960355,0.3281623449314336,0.6154900733348241},
                                            {-0.6123026159343583,0.6555966730967037,0.31125746166573987,-0.020668044113381252,-0.31300181030531293},
                                            {0.0,-0.5256369482701144,0.7764652982915754,0.09278751376016112,-0.3349595743555688},
                                            {0.0,-0.03792925509884924,-0.3203831979918099,0.8311296883563183,-0.4529231934720459}};

            var expectedSingularValues = new double[] { 3.0006299180604175E11, 6941134.97138654, 0.0, 0.0, 0.0 };

            var expectedQ = new double[] { -1.012786479666829, -0.9916140623390675, -0.9931525383144617, -0.9830938777886331, -1.0184462759643793 };

            var q = svdA.GetSolver().Solve(rWtWR);

            Assert2.ArrayEquals(svdA.GetU().AsArrayEx(), expectedU, eps);
            Assert2.ArrayEquals(svdA.GetVT().AsArrayEx(), expectedVT, eps);
            Assert2.ArrayEquals(svdA.SingularValues, expectedSingularValues, eps);
            Assert2.ArrayEquals(q.AsArrayEx(), expectedQ, eps);
        }

        [Test]
        public void Test2()
        {
            var expectedSingularValues = new double[] { 23.567495561769917, 18.785576308145938, 3.126363155561169, 0.6455649745229703 };

            var expectedQ = new double[] { -1.012786479666829, -0.9916140623390675, -0.9931525383144617, -0.9830938777886331, -1.0184462759643793 };
            var expectedU = new double[,] { {-0.3441671808204185,0.05862710366802659,-0.7313629750979653,0.5858498212148545},
                                            {-0.2384743525420915,0.16755831888783956,-0.5183545733654906,-0.8039668707242804},
                                            {-0.8308810600307643,-0.4568499903712133,0.31357691475735916,-0.050933966183992894},
                                            {-0.366490926092188,0.8716508238880994,0.31318724983336876,0.08844766163726105}};
            var expectedV = new double[,] { {-0.3441671808204185,0.05862710366802659,-0.7313629750979653,0.5858498212148545},
                                            {-0.2384743525420915,0.16755831888783956,-0.5183545733654906,-0.8039668707242804},
                                            {-0.8308810600307643,-0.4568499903712133,0.31357691475735916,-0.050933966183992894},
                                            {-0.366490926092188,0.8716508238880994,0.31318724983336876,0.08844766163726105}};

            var rWtWRt = MatrixUtility.CreateMatrix<Double>(new double[,]{{4.75,3,5.5,3.25},{3,3.125,2.75,4.25},{5.5,2.75,20.5,0},{3.25,4.25,0,17.75}});

            SingularValueDecomposition svdA = new SingularValueDecomposition(rWtWRt);

            var rWtWR = MatrixUtility.CreateRealVector(new double[] { -26.165000000000003, -18.58, -63.815000000000005, -18 });

            var q = svdA.GetSolver().Solve(rWtWR);

            Assert2.ArrayEquals(svdA.GetU().AsArrayEx(), expectedU, eps);
            Assert2.ArrayEquals(svdA.GetV().AsArrayEx(), expectedV, eps);
            Assert2.ArrayEquals(svdA.SingularValues, expectedSingularValues, eps);
            Assert2.ArrayEquals(q.AsArrayEx(), expectedQ, eps);
        }
    }
}
