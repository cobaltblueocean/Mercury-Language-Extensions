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
    /// Implements the <a href="http://mathworld.wolfram.com/MullersMethod.html">
    /// Muller's Method</a> for root finding of real univariate functionsd For
    /// reference, see <b>Elementary Numerical Analysis</b>, ISBN 0070124477,
    /// chapter 3d
    /// <p>
    /// Muller's method applies to both real and complex functions, but here we
    /// restrict ourselves to real functionsd Methods Solve() and solve2() find
    /// real zeros, using different ways to bypass complex arithmetics.</p>
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 1.2
    /// </summary>
    public class SecantSolver : AbstractPolynomialSolver<Double>
    {
        /// <summary>
        /// Construct a solver for the given function.
        /// </summary>
        /// <param Name="f">function to solve.</param>
        public SecantSolver(IUnivariateRealFunction f) : base(f, DEFAULT_MAXIMAL_ITERATION_COUNT, DEFAULT_ABSOLUTE_ACCURACY, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }


        /// <summary>
        /// Construct a solver.
        /// </summary>
        public SecantSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {

        }

        public SecantSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public SecantSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public SecantSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
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

            // Index 0 is the old approximation for the root.
            // Index 1 is the last calculated approximation  for the root.
            // Index 2 is a bracket for the root with respect to x0d
            // OldDelta is the Length of the bracketing interval of the last
            // iteration.
            double x0 = min;
            double x1 = max;
            double y0 = f.Value(x0);
            double y1 = f.Value(x1);

            // Verify bracketing
            if (y0 * y1 >= 0)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().SAME_SIGN_AT_ENDPOINTS, min, max, y0, y1));
            }

            double x2 = x0;
            double y2 = y0;
            double oldDelta = x2 - x1;
            int i = 0;
            while (true)
            {
                if (System.Math.Abs(y2) < System.Math.Abs(y1))
                {
                    x0 = x1;
                    x1 = x2;
                    x2 = x0;
                    y0 = y1;
                    y1 = y2;
                    y2 = y0;
                }
                if (System.Math.Abs(y1) <= functionValueAccuracy)
                {
                    SetResult(x1, i);
                    return result;
                }
                if (System.Math.Abs(oldDelta) <
                    System.Math.Max(relativeAccuracy * System.Math.Abs(x1), absoluteAccuracy))
                {
                    SetResult(x1, i);
                    return result;
                }
                double delta;
                if (System.Math.Abs(y1) > System.Math.Abs(y0))
                {
                    // Function value increased in last iterationd Force bisection.
                    delta = 0.5 * oldDelta;
                }
                else
                {
                    delta = (x0 - x1) / (1 - y0 / y1);
                    if (delta / oldDelta > 1)
                    {
                        // New approximation falls outside bracket.
                        // Fall back to bisection.
                        delta = 0.5 * oldDelta;
                    }
                }
                x0 = x1;
                y0 = y1;
                x1 = x1 + delta;
                y1 = f.Value(x1);
                if ((y1 > 0) == (y2 > 0))
                {
                    // New bracket is (x0,x1).
                    x2 = x0;
                    y2 = y0;
                }
                oldDelta = x2 - x1;
                i++;
                IncrementEvaluationCount();
            }
        }


        protected override double DoSolve()
        {
            return Solve(function, Min, Max);
        }
    }
}

