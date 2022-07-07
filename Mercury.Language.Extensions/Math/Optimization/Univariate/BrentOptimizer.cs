// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization.Univariate Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization.Univariate Incd and the System.Math.Optimization.Univariate group of companies
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

namespace Mercury.Language.Math.Optimization.Univariate
{
    /// <summary>
    /// For a function defined on some interval {@code (lo, hi)}, this class
    /// finds an approximation {@code x} to the point at which the function
    /// attains its minimum.
    /// It implements Richard Brent's algorithm (from his book "Algorithms for
    /// Minimization without Derivatives", p. 79) for finding minima of real
    /// univariate functions.
    /// <br/>
    /// This code is an adaptation, partly based on the Python code from SciPy
    /// (module "optimize.py" v0.5); the original algorithm is also modified
    /// <ul>
    ///  <li>to use an initial guess provided by the user,</li>
    ///  <li>to ensure that the best point encountered is the one returned.</li>
    /// </ul>    /// </summary>
    public class BrentOptimizer : BaseAbstractUnivariateOptimizer
    {

        #region Local Variables
        private static double GOLDEN_SECTION = 0.5 * (3 - System.Math.Sqrt(5));
        private static double MIN_RELATIVE_TOLERANCE = 2 * Math2.Ulp(1d);
        private double relativeThreshold;
        private double absoluteThreshold;
        #endregion

        #region Property

        #endregion

        #region Constructor
        public BrentOptimizer() : this(MIN_RELATIVE_TOLERANCE, 0)
        { }

        public BrentOptimizer(double rel, double abs, IConvergenceChecker<UnivariatePointValuePair> checker) : base(checker)
        {
            if (rel < MIN_RELATIVE_TOLERANCE)
            {
                throw new NumberTooSmallException(rel, MIN_RELATIVE_TOLERANCE, true);
            }
            if (abs <= 0)
            {
                throw new NotStrictlyPositiveException(abs);
            }

            relativeThreshold = rel;
            absoluteThreshold = abs;
        }

        public BrentOptimizer(double rel, double abs) : this(rel, abs, null)
        {

        }
        #endregion

        #region Implement Methods

        protected override UnivariatePointValuePair doOptimize()
        {
            Boolean isMinim = GoalType == GoalType.MINIMIZE;
            double lo = Min;
            double mid = StartValue;
            double hi = Max;

            // Optional additional convergence criteria.
            IConvergenceChecker<UnivariatePointValuePair> checker = ConvergenceChecker;

            double a;
            double b;
            if (lo < hi)
            {
                a = lo;
                b = hi;
            }
            else
            {
                a = hi;
                b = lo;
            }

            double x = mid;
            double v = x;
            double w = x;
            double d = 0;
            double e = 0;
            double fx = computeObjectiveValue(x);
            if (!isMinim)
            {
                fx = -fx;
            }
            double fv = fx;
            double fw = fx;

            UnivariatePointValuePair previous = null;
            UnivariatePointValuePair current
                = new UnivariatePointValuePair(x, isMinim ? fx : -fx);
            // Best point encountered so far (which is the initial guess).
            UnivariatePointValuePair best = current;

            int iter = 0;
            while (true)
            {
                double m = 0.5 * (a + b);
                double tol1 = relativeThreshold * System.Math.Abs(x) + absoluteThreshold;
                double tol2 = 2 * tol1;

                // Default stopping criterion.
                Boolean stop = System.Math.Abs(x - m) <= tol2 - 0.5 * (b - a);
                if (!stop)
                {
                    double p = 0;
                    double q = 0;
                    double r = 0;
                    double u = 0;

                    if (System.Math.Abs(e) > tol1)
                    { // Fit parabola.
                        r = (x - w) * (fx - fv);
                        q = (x - v) * (fx - fw);
                        p = (x - v) * q - (x - w) * r;
                        q = 2 * (q - r);

                        if (q > 0)
                        {
                            p = -p;
                        }
                        else
                        {
                            q = -q;
                        }

                        r = e;
                        e = d;

                        if (p > q * (a - x) &&
                            p < q * (b - x) &&
                            System.Math.Abs(p) < System.Math.Abs(0.5 * q * r))
                        {
                            // Parabolic interpolation step.
                            d = p / q;
                            u = x + d;

                            // f must not be evaluated too close to a or b.
                            if (u - a < tol2 || b - u < tol2)
                            {
                                if (x <= m)
                                {
                                    d = tol1;
                                }
                                else
                                {
                                    d = -tol1;
                                }
                            }
                        }
                        else
                        {
                            // Golden section step.
                            if (x < m)
                            {
                                e = b - x;
                            }
                            else
                            {
                                e = a - x;
                            }
                            d = GOLDEN_SECTION * e;
                        }
                    }
                    else
                    {
                        // Golden section step.
                        if (x < m)
                        {
                            e = b - x;
                        }
                        else
                        {
                            e = a - x;
                        }
                        d = GOLDEN_SECTION * e;
                    }

                    // Update by at least "tol1".
                    if (System.Math.Abs(d) < tol1)
                    {
                        if (d >= 0)
                        {
                            u = x + tol1;
                        }
                        else
                        {
                            u = x - tol1;
                        }
                    }
                    else
                    {
                        u = x + d;
                    }

                    double fu = computeObjectiveValue(u);
                    if (!isMinim)
                    {
                        fu = -fu;
                    }

                    // User-defined convergence checker.
                    previous = current;
                    current = new UnivariatePointValuePair(u, isMinim ? fu : -fu);
                    best = Best(best,
                                Best(previous,
                                     current,
                                     isMinim),
                                isMinim);

                    if (checker != null && checker.Converged(iter, previous, current))
                    {
                        return best;
                    }

                    // Update a, b, v, w and x.
                    if (fu <= fx)
                    {
                        if (u < x)
                        {
                            b = x;
                        }
                        else
                        {
                            a = x;
                        }
                        v = w;
                        fv = fw;
                        w = x;
                        fw = fx;
                        x = u;
                        fx = fu;
                    }
                    else
                    {
                        if (u < x)
                        {
                            a = u;
                        }
                        else
                        {
                            b = u;
                        }
                        if (fu <= fw ||
                            w.AlmostEquals(x, 1d))
                        {
                            v = w;
                            fv = fw;
                            w = u;
                            fw = fu;
                        }
                        else if (fu <= fv ||
                                 v.AlmostEquals(x, 1d) ||
                                 v.AlmostEquals(w, 1d))
                        {
                            v = u;
                            fv = fu;
                        }
                    }
                }
                else
                { // Default termination (Brent's criterion).
                    return Best(best,
                                Best(previous,
                                     current,
                                     isMinim),
                                isMinim);
                }
                ++iter;
            }

        }

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods
        private UnivariatePointValuePair Best(UnivariatePointValuePair a, UnivariatePointValuePair b, Boolean isMinim)
        {
            if (a == null)
            {
                return b;
            }
            if (b == null)
            {
                return a;
            }

            if (isMinim)
            {
                return a.Value <= b.Value ? a : b;
            }
            else
            {
                return a.Value >= b.Value ? a : b;
            }
        }
        #endregion
    }
}
