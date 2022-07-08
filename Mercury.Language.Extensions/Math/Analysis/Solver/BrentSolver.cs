// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Analysis.Solver Inc.
//
// Copyright (C) 2012 - present by System.Math.Analysis.Solver Incd and the System.Math.Analysis.Solver group of companies
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
    /// BrentSolver Description
    /// </summary>
    public class BrentSolver: AbstractUnivariateSolver
    {

        #region Local Variables
        /** Default absolute accuracyd */
        private static double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;
        #endregion

        #region Property

        #endregion

        #region Constructor
        /// <summary>
        /// Construct a solver with default absolute accuracy (1e-6).
        /// </summary>
        public BrentSolver(): this(DEFAULT_ABSOLUTE_ACCURACY)
        {
            
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        public BrentSolver(double absoluteAccuracy): base(absoluteAccuracy)
        {
            
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy">Relative accuracy.</param>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        public BrentSolver(double relativeAccuracy, double absoluteAccuracy): base(relativeAccuracy, absoluteAccuracy)
        {
            
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy">Relative accuracy.</param>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        /// <param name="functionValueAccuracy">Function value accuracy.</param>
        public BrentSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy): base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {
            
        }
        #endregion

        #region Implement Methods

        public override double Solve(IUnivariateRealFunction f, double[] values, double startValue)
        {
            throw new NotImplementedException();
        }

        public override double Solve(IUnivariateRealFunction f, double[] values, double min, double max)
        {
            throw new NotImplementedException();
        }

        public override Double Solve(IUnivariateRealFunction f, double[] values, double min, double max, double startValue)
        {
            throw new NotImplementedException();
        }


        public Double Solve()
        {
            return DoSolve();
        }

        protected override double DoSolve()
        {
            double initial = StartValue;
            double functionValueAccuracy = FunctionValueAccuracy;

            VerifySequence(Min, initial, Max);

            // Return the initial guess if it is good enough.
            double yInitial = ComputeObjectiveValue(initial);
            if (System.Math.Abs(yInitial) <= functionValueAccuracy)
            {
                return initial;
            }

            // Return the first endpoint if it is good enough.
            double yMin = ComputeObjectiveValue(Min);
            if (System.Math.Abs(yMin) <= functionValueAccuracy)
            {
                return Min;
            }

            // Reduce interval if min and initial bracket the root.
            if (yInitial * yMin < 0)
            {
                return Brent(Min, initial, yMin, yInitial);
            }

            // Return the second endpoint if it is good enough.
            double yMax = ComputeObjectiveValue(Max);
            if (System.Math.Abs(yMax) <= functionValueAccuracy)
            {
                return Max;
            }

            // Reduce interval if initial and max bracket the root.
            if (yInitial * yMax < 0)
            {
                return Brent(initial, Max, yInitial, yMax);
            }

            throw new NoBracketingException(Min, Max, yMin, yMax);
        }

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods

        /// <summary>
        /// Search for a zero inside the provided interval.
        /// This implementation is based on the algorithm described at page 58 of the book
        /// <blockquote>
        ///   <b>Algorithms for Minimization Without Derivatives</b>
        ///   <it>Richard P.Brent</it>
        ///   Dover 0-486-41998-3
        ///  </blockquote>
        /// </summary>
        /// <param name="lo">Lower bound of the search interval.</param>
        /// <param name="hi">Higher bound of the search interval.</param>
        /// <param name="fLo">Function value at the lower bound of the search interval.</param>
        /// <param name="fHi">Function value at the higher bound of the search interval.</param>
        /// <returns>the value where the function is zero.</returns>
        private double Brent(double lo, double hi,
                     double fLo, double fHi)
        {
            double a = lo;
            double fa = fLo;
            double b = hi;
            double fb = fHi;
            double c = a;
            double fc = fa;
            double d = b - a;
            double e = d;

            double t = AbsoluteAccuracy;
            double eps = RelativeAccuracy;

            while (true)
            {
                if (System.Math.Abs(fc) < System.Math.Abs(fb))
                {
                    a = b;
                    b = c;
                    c = a;
                    fa = fb;
                    fb = fc;
                    fc = fa;
                }

                double tol = 2 * eps * System.Math.Abs(b) + t;
                double m = 0.5 * (c - b);

                if (System.Math.Abs(m) <= tol || fb.AlmostEquals(0d, 1d))
                {
                    return b;
                }
                if (System.Math.Abs(e) < tol ||
                    System.Math.Abs(fa) <= System.Math.Abs(fb))
                {
                    // Force bisection.
                    d = m;
                    e = d;
                }
                else
                {
                    double s = fb / fa;
                    double p;
                    double q;
                    // The equality test (a == c) is intentional,
                    // it is part of the original Brent's method and
                    // it should NOT be replaced by proximity test.
                    if (a == c)
                    {
                        // Linear interpolation.
                        p = 2 * m * s;
                        q = 1 - s;
                    }
                    else
                    {
                        // Inverse quadratic interpolation.
                        q = fa / fc;
                        double r = fb / fc;
                        p = s * (2 * m * q * (q - r) - (b - a) * (r - 1));
                        q = (q - 1) * (r - 1) * (s - 1);
                    }
                    if (p > 0)
                    {
                        q = -q;
                    }
                    else
                    {
                        p = -p;
                    }
                    s = e;
                    e = d;
                    if (p >= 1.5 * m * q - System.Math.Abs(tol * q) ||
                        p >= System.Math.Abs(0.5 * s * q))
                    {
                        // Inverse quadratic interpolation gives a value
                        // in the wrong direction, or progress is slow.
                        // Fall back to bisection.
                        d = m;
                        e = d;
                    }
                    else
                    {
                        d = p / q;
                    }
                }
                a = b;
                fa = fb;

                if (System.Math.Abs(d) > tol)
                {
                    b += d;
                }
                else if (m > 0)
                {
                    b += tol;
                }
                else
                {
                    b -= tol;
                }
                fb = ComputeObjectiveValue(b);
                if ((fb > 0 && fc > 0) ||
                    (fb <= 0 && fc <= 0))
                {
                    c = a;
                    fc = fa;
                    d = b - a;
                    e = d;
                }
            }
        }
        #endregion
    }
}
