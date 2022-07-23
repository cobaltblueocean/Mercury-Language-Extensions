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
using Mercury.Language.Math.Optimization.Univariate;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Optimization;

namespace Mercury.Language.Extensions.Test.Optimization
{
    /// <summary>
    /// BrentOptimizerTest Description
    /// </summary>
    public class BrentOptimizerTest
    {
        private static double EPS = 1e-5;

        [Test]
        public void Test()
        {
            double startPosition = 0.0;
            double upperBound = -10d;
            double lowerBound = 10d;

            var OPTIMIZER = new BrentOptimizer();
            GoalType MINIMIZE = GoalType.MINIMIZE;

            IUnivariateRealFunction commonsFunction = new UnivariateRealFunction() {
                function = new Func<double?, double?>((x) => {
                    return x * x + 7 * x + 12;
                })
            };

            var result = OPTIMIZER.Optimize(commonsFunction, MINIMIZE, lowerBound, upperBound, startPosition);
            Assert.AreEqual(result, -3.5, EPS);
        }
    }
}
