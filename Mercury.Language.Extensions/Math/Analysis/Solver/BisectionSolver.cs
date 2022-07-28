// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
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
using Mercury.Language.Math;

using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;


namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/Bisection.html">
    /// bisection algorithm</a> for finding zeros of univariate real functions.
    /// <p>
    /// The function should be continuous but not necessarily smooth.</p>
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// </summary>
    public class BisectionSolver : AbstractPolynomialSolver<Double>
    {
        /// <summary>
        /// Construct a solver for the given function.
        /// </summary>
        /// <param name="f">function to solve.</param>
        public BisectionSolver(IUnivariateRealFunction f):base(f, DEFAULT_MAXIMAL_ITERATION_COUNT, DEFAULT_ABSOLUTE_ACCURACY, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }


        /// <summary>
        /// Construct a solver.
        /// </summary>
        public BisectionSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {

        }

        public BisectionSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public BisectionSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public BisectionSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public double Solve(double min, double max, double initial)
        {
            return Solve(function, min, max, initial);
        }

        public double Solve(double min, double max)
        {
            return Solve(function, min, max);
        }

        public override double Solve(IUnivariateRealFunction f, double min, double max, double initial)
        {
            return Solve(f, min, max);
        }

        public override double Solve(IUnivariateRealFunction f, double min, double max)
        {
            base.function = f;

            ClearResult();
            VerifyInterval(min, max);
            double m;
            double fm;
            double fmin;

            int i = 0;
            while (true)
            {
                m = Midpoint(min, max);
                fmin = f.Value(min);
                fm = f.Value(m);

                if (fm * fmin > 0.0)
                {
                    // max and m bracket the root.
                    min = m;
                }
                else
                {
                    // min and m bracket the root.
                    max = m;
                }

                if (System.Math.Abs(max - min) <= absoluteAccuracy)
                {
                    m = Midpoint(min, max);
                    SetResult(m, i);
                    return m;
                }
                ++i;
                IncrementEvaluationCount();
            }
        }


        protected override double DoSolve()
        {
            return Solve(function, Min, Max);
        }
    }
}
