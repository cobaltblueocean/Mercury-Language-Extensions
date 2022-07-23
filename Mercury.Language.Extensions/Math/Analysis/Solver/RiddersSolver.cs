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
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// RiddersSolver Description
    /// </summary>
    public class RiddersSolver : AbstractUnivariateSolver<Double>
    {
        /// <summary>
        /// Default absolute accuracyd
        /// </summary>
        private static double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver with default accuracy (1e-6).
        /// </summary>
        public RiddersSolver():this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        public RiddersSolver(double absoluteAccuracy):base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy">Absolute accuracy.Relative accuracy.</param>
        /// <param name="absoluteAccuracy"></param>
        public RiddersSolver(double relativeAccuracy, double absoluteAccuracy):base(relativeAccuracy, absoluteAccuracy)
        {
        }

        protected override double DoSolve()
        {
            double min = Min;
            double max = Max;
            // [x1, x2] is the bracketing interval in each iteration
            // x3 is the midpoint of [x1, x2]
            // x is the new root approximation and an endpoint of the new interval
            double x1 = min;
            double y1 = ComputeObjectiveValue(x1);
            double x2 = max;
            double y2 = ComputeObjectiveValue(x2);

            // check for zeros before verifying bracketing
            if (y1 == 0)
            {
                return min;
            }
            if (y2 == 0)
            {
                return max;
            }
            VerifyBracketing(min, max);

            double absoluteAccuracy = AbsoluteAccuracy;
            double functionValueAccuracy = FunctionValueAccuracy;
            double relativeAccuracy = RelativeAccuracy;

            double oldx = Double.PositiveInfinity;
            while (true)
            {
                // calculate the new root approximation
                double x3 = 0.5 * (x1 + x2);
                double y3 = ComputeObjectiveValue(x3);
                if (System.Math.Abs(y3) <= functionValueAccuracy)
                {
                    return x3;
                }
                double delta = 1 - (y1 * y2) / (y3 * y3);  // delta > 1 due to bracketing
                double correction = (System.Math.Sign(y2) * System.Math.Sign(y3)) *
                                          (x3 - x1) / System.Math.Sqrt(delta);
                double x = x3 - correction;                // correction != 0
                double y = ComputeObjectiveValue(x);

                // check for convergence
                double tolerance = System.Math.Max(relativeAccuracy * System.Math.Abs(x), absoluteAccuracy);
                if (System.Math.Abs(x - oldx) <= tolerance)
                {
                    return x;
                }
                if (System.Math.Abs(y) <= functionValueAccuracy)
                {
                    return x;
                }

                // prepare the new interval for next iteration
                // Ridders' method guarantees x1 < x < x2
                if (correction > 0.0)
                {             // x1 < x < x3
                    if (System.Math.Sign(y1) + System.Math.Sign(y) == 0.0)
                    {
                        x2 = x;
                        y2 = y;
                    }
                    else
                    {
                        x1 = x;
                        x2 = x3;
                        y1 = y;
                        y2 = y3;
                    }
                }
                else
                {                            // x3 < x < x2
                    if (System.Math.Sign(y2) + System.Math.Sign(y) == 0.0)
                    {
                        x1 = x;
                        y1 = y;
                    }
                    else
                    {
                        x1 = x3;
                        x2 = x;
                        y1 = y3;
                        y2 = y;
                    }
                }
                oldx = x;
            }
        }
    }
}
