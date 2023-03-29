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
using Mercury.Language.Exceptions;

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
    public class MullerSolver : AbstractPolynomialSolver<Double>
    {
        /// <summary>
        /// Construct a solver for the given function.
        /// </summary>
        /// <param Name="f">function to solve.</param>
        public MullerSolver(IUnivariateRealFunction f) : base(f, DEFAULT_MAXIMAL_ITERATION_COUNT, DEFAULT_ABSOLUTE_ACCURACY, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }


        /// <summary>
        /// Construct a solver.
        /// </summary>
        public MullerSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {

        }

        public MullerSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public MullerSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public MullerSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
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
            // [x0, x2] is the bracketing interval in each iteration
            // x1 is the last approximation and an interpolation point in (x0, x2)
            // x is the new root approximation and new x1 for next round
            // d01, d12, d012 are divided differences
            base.function = f;

            double x0 = min;
            double y0 = f.Value(x0);
            double x2 = max;
            double y2 = f.Value(x2);
            double x1 = 0.5 * (x0 + x2);
            double y1 = f.Value(x1);

            // check for zeros before verifying bracketing
            if (y0 == 0.0)
            {
                return min;
            }
            if (y2 == 0.0)
            {
                return max;
            }
            VerifyBracketing(f, min, max);

            double oldx = Double.PositiveInfinity;
            for (int i = 1; i <= MaximalIterationCount; ++i)
            {
                // Muller's method employs quadratic interpolation through
                // x0, x1, x2 and x is the zero of the interpolating parabola.
                // Due to bracketing condition, this parabola must have two
                // real roots and we choose one in [x0, x2] to be x.
                double d01 = (y1 - y0) / (x1 - x0);
                double d12 = (y2 - y1) / (x2 - x1);
                double d012 = (d12 - d01) / (x2 - x0);
                double c1 = d01 + (x1 - x0) * d012;
                double delta = c1 * c1 - 4 * y1 * d012;
                double xplus = x1 + (-2.0 * y1) / (c1 + System.Math.Sqrt(delta));
                double xminus = x1 + (-2.0 * y1) / (c1 - System.Math.Sqrt(delta));
                // xplus and xminus are two roots of parabola and at least
                // one of them should lie in (x0, x2)
                double x = IsSequence(x0, xplus, x2) ? xplus : xminus;
                double y = f.Value(x);

                // check for convergence
                double tolerance = System.Math.Max(relativeAccuracy * System.Math.Abs(x), absoluteAccuracy);
                if (System.Math.Abs(x - oldx) <= tolerance)
                {
                    SetResult(x, i);
                    return result;
                }
                if (System.Math.Abs(y) <= functionValueAccuracy)
                {
                    SetResult(x, i);
                    return result;
                }

                // Bisect if convergence is too slowd Bisection would waste
                // our calculation of x, hopefully it won't happen often.
                // the real number equality test x == x1 is intentional and
                // completes the proximity tests above it
                Boolean bisect = (x < x1 && (x1 - x0) > 0.95 * (x2 - x0)) ||
                                 (x > x1 && (x2 - x1) > 0.95 * (x2 - x0)) ||
                                 (x == x1);
                // prepare the new bracketing interval for next iteration
                if (!bisect)
                {
                    x0 = x < x1 ? x0 : x1;
                    y0 = x < x1 ? y0 : y1;
                    x2 = x > x1 ? x2 : x1;
                    y2 = x > x1 ? y2 : y1;
                    x1 = x; y1 = y;
                    oldx = x;
                }
                else
                {
                    double xm = 0.5 * (x0 + x2);
                    double ym = f.Value(xm);
                    if (System.Math.Sign(y0) + System.Math.Sign(ym) == 0.0)
                    {
                        x2 = xm; y2 = ym;
                    }
                    else
                    {
                        x0 = xm; y0 = ym;
                    }
                    x1 = 0.5 * (x0 + x2);
                    y1 = f.Value(x1);
                    oldx = Double.PositiveInfinity;
                }
            }
            throw new TooManyEvaluationsException(MaximalIterationCount);
        }


        protected override double DoSolve()
        {
            return Solve(function, Min, Max);
        }
    }
}

