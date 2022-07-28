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
    public class NewtonSolver : AbstractPolynomialSolver<Double>
    {
        /// <summary>
        /// Construct a solver for the given function.
        /// </summary>
        /// <param Name="f">function to solve.</param>
        public NewtonSolver(IUnivariateRealFunction f) : base(f, DEFAULT_MAXIMAL_ITERATION_COUNT, DEFAULT_ABSOLUTE_ACCURACY, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }


        /// <summary>
        /// Construct a solver.
        /// </summary>
        public NewtonSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {

        }

        public NewtonSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public NewtonSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
            base.MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;
        }

        public NewtonSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
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
            base.function = f;

            IUnivariateRealFunction derivative = ((IDifferentiableUnivariateRealFunction)f).Derivative();
            ClearResult();
            VerifySequence(min, initial, max);

            double x0 = StartValue;
            double x1;

            int i = 0;
            while (true)
            {

                x1 = x0 - (f.Value(x0) / derivative.Value(x0));
                if (System.Math.Abs(x1 - x0) <= absoluteAccuracy)
                {
                    SetResult(x1, i);
                    return x1;
                }

                x0 = x1;
                ++i;
                IncrementEvaluationCount();
            }
        }

        public override double Solve(IUnivariateRealFunction f, double min, double max)
        {
            return Solve(f, min, max, StartValue);
        }


        protected override double DoSolve()
        {
            throw new NotImplementedException();
        }
    }
}

