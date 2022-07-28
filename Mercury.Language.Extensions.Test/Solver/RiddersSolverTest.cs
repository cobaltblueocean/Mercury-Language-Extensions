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
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Analysis.Solver;

namespace Mercury.Language.Extensions.Test.Solver
{
    /// <summary>
    /// RiddersSolverTest Description
    /// </summary>
    public class RiddersSolverTest
    {
        private static int MAX_ITER = 10000;
        private RiddersSolver _ridder = new RiddersSolver();
        private static double EPS = 1e-9;

        [Test]
        public void Test()
        {
            IUnivariateRealFunction F = new UnivariateRealFunction()
            {
                function = new Func<Double?, Double?>((x) =>
                {
                    return x * x * x - 4 * x * x + x + 6;
                })
            };

            _ridder.MaximalIterationCount = MAX_ITER;

            Assert.AreEqual(_ridder.Solve (F, 2.5, 3.5),  3, EPS);
            Assert.AreEqual(_ridder.Solve(F, 1.5, 2.5), 2, EPS);
            Assert.AreEqual(_ridder.Solve(F, -1.5, 0.5), -1, EPS);
        }
    }
}
