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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Integrator
{
    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/SimpsonsRule.html">
    /// Simpson's Rule</a> for Integration of real univariate functionsd For
    /// reference, see <b>Introduction to Numerical Analysis</b>, ISBN 038795452X,
    /// chapter 3.
    /// <p>
    /// This implementation employs basic trapezoid rule as building blocks to
    /// calculate the Simpson's rule of alternating 2/3 and 4/3.</p>
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 1.2
    /// </summary>
    public class SimpsonIntegrator : UnivariateRealIntegrator
    {

        /// <summary>
        /// Construct an integrator.
        /// </summary>
        public SimpsonIntegrator() : base(64)
        {

        }

        public override double Integrate(IUnivariateRealFunction f, double min, double max)
        {

            ClearResult();
            VerifyInterval(min, max);
            VerifyIterationCount();

            TrapezoidIntegrator qtrap = new TrapezoidIntegrator();
            if (minimalIterationCount == 1)
            {
                double s = (4 * qtrap.Stage(f, min, max, 1) - qtrap.Stage(f, min, max, 0)) / 3.0;
                SetResult(s, 1);
                return result;
            }
            // Simpson's rule requires at least two trapezoid stages.
            double olds = 0;
            double oldt = qtrap.Stage(f, min, max, 0);
            for (int i = 1; i <= maximalIterationCount; ++i)
            {
                double t = qtrap.Stage(f, min, max, i);
                double s = (4 * t - oldt) / 3.0;
                if (i >= minimalIterationCount)
                {
                    double delta = System.Math.Abs(s - olds);
                    double rLimit =
                        relativeAccuracy * (System.Math.Abs(olds) + System.Math.Abs(s)) * 0.5;
                    if ((delta <= rLimit) || (delta <= absoluteAccuracy))
                    {
                        SetResult(s, i);
                        return result;
                    }
                }
                olds = s;
                oldt = t;
            }
            throw new IndexOutOfRangeException(maximalIterationCount.ToString());
        }

        /// <summary>{@inheritDoc} */
        //@Override
        protected override void VerifyIterationCount()
        {
            base.VerifyIterationCount();
            // at most 64 bisection refinements
            if (maximalIterationCount > 64)
            {
                throw new MathArgumentException(LocalizedResources.Instance().INVALID_ITERATIONS_LIMITS, 0, 64);
            }
        }
    }
}
