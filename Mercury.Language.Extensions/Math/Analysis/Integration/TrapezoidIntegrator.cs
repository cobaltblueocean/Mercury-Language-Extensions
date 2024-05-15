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
using Mercury.Language;
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Integration
{
    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/TrapezoidalRule.html">
    /// Trapezoidal Rule</a> for integration of real univariate functions. For
    /// reference, see <b>Introduction to Numerical Analysis</b>, ISBN 038795452X,
    /// chapter 3.
    /// <p>
    /// The function should be integrable.</p>    /// </summary>
    public class TrapezoidIntegrator : UnivariateRealIntegrator
    {
        /** Intermediate result. */
        private double s;

        public TrapezoidIntegrator(IUnivariateRealFunction f) : base(f, 64)
        {

        }

        public TrapezoidIntegrator() : base(64)
        {

        }

        public double Stage(IUnivariateRealFunction f, double min, double max, int n)
        {

            if (n == 0)
            {
                s = 0.5 * (max - min) * (f.Value(min) + f.Value(max));
                return s;
            }
            else
            {
                long np = 1L << (n - 1);           // number of new points in this stage
                double sum = 0;
                double spacing = (max - min) / np; // spacing between adjacent new points
                double x = min + 0.5 * spacing;    // the first new point
                for (long i = 0; i < np; i++)
                {
                    sum += f.Value(x);
                    x += spacing;
                }
                // add the new sum to previously calculated result
                s = 0.5 * (s + sum * spacing);
                return s;
            }
        }

        public override double Integrate(double min, double max)
        {
            return Integrate(f, min, max);
        }

        /** {@inheritDoc} */
        public override double Integrate(IUnivariateRealFunction f, double min, double max)
        {

            ClearResult();
            VerifyInterval(min, max);
            VerifyIterationCount();

            double oldt = Stage(f, min, max, 0);
            for (int i = 1; i <= maximalIterationCount; ++i)
            {
                double t = Stage(f, min, max, i);
                if (i >= minimalIterationCount)
                {
                    double delta = System.Math.Abs(t - oldt);
                    double rLimit =
                        relativeAccuracy * (System.Math.Abs(oldt) + System.Math.Abs(t)) * 0.5;
                    if ((delta <= rLimit) || (delta <= absoluteAccuracy))
                    {
                        SetResult(t, i);
                        return result;
                    }
                }
                oldt = t;
            }
            throw new IndexOutOfRangeException(MaximalIterationCount.ToString());
        }

        protected override void VerifyIterationCount()
        {
            base.VerifyIterationCount();
            // at most 64 bisection refinements
            if (MaximalIterationCount > 64)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().INVALID_ITERATIONS_LIMITS, 0, 64));
            }
        }
    }
}
