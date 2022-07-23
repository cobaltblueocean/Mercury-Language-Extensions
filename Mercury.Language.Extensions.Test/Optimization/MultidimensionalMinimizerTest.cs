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
using Mercury.Language.Log;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Optimization;
using Mercury.Language.Math.Optimization.Direct;
using Mercury.Language.Math.Matrix;

namespace Mercury.Language.Extensions.Test.Optimization
{
    /// <summary>
    /// MultidimensionalMinimizerTest Description
    /// </summary>
    public class MultidimensionalMinimizerTest
    {
        private static int HOTSPOT_WARMUP_CYCLES = 0;
        private static int BENCHMARK_CYCLES = 1;

        private static double EPS = 1e-8;
        private static GoalType MINIMIZER = GoalType.MINIMIZE;
        IMultivariateRealOptimizer optimizer = new NelderMead();


        [Test]
        public void Test()
        {
            var start = MatrixUtility.CreateRealVector(new double[] { -1.0, 1.0 });
            var Name = "Simplex - rosenbrock ";

            IMultivariateRealFunction commonsFunction = new MultivariateRealFunction()
            {
                function = new Func<Double[], Double>((x) =>
                {
                    return System.Math2.Square(1 - x[0]) + 100 * System.Math2.Square(x[1] - System.Math2.Square(x[0]));
                })
            };

            for (int i = 0; i < HOTSPOT_WARMUP_CYCLES; i++)
            {
                optimizer.Optimize(commonsFunction, MINIMIZER, start.AsArrayEx());
            }
            if (BENCHMARK_CYCLES > 0)
            {
                TimerCallback timer_delegate = state =>
                {
                    Logger.Information("processing {0} cycles on" + state);
                };

                Timer timer = new Timer(timer_delegate, Name, BENCHMARK_CYCLES, BENCHMARK_CYCLES);
                for (int i = 0; i < BENCHMARK_CYCLES; i++)
                {
                    optimizer.Optimize(commonsFunction, MINIMIZER, start.AsArrayEx());
                }
                timer.Dispose();
            }
        }
    }
}
