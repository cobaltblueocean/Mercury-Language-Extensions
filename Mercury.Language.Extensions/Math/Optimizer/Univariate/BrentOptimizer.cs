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

namespace Mercury.Language.Math.Optimizer.Univariate
{
    /// <summary>
    /// For a function defined on some interval {@code (lo, hi)}, this class
    /// finds an approximation {@code x} to the point at which the function
    /// attains its minimum.
    /// It  :  Richard Brent's algorithm (from his book "Algorithms for
    /// Minimization without Derivatives", pd 79) for finding minima of real
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
        private static double ABSOLUTE_THREASHOLD = 1e-11;
        private double relativeThreshold;
        private double absoluteThreshold;
        #endregion

        #region Property

        #endregion

        #region Constructor
        public BrentOptimizer() : this(MIN_RELATIVE_TOLERANCE, ABSOLUTE_THREASHOLD)
        { }

        public BrentOptimizer(double rel, double abs, IConvergenceChecker<double> checker) : base(checker)
        {
            if (rel < MIN_RELATIVE_TOLERANCE)
            {
                throw new NumberIsTooSmallException(rel, MIN_RELATIVE_TOLERANCE, true);
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

        protected override double doOptimize()
        {
            return localMin(base.GoalType == GoalType.MINIMIZE, Min, StartValue, Max, relativeThreshold, absoluteThreshold);
        }

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods

        /// <summary>
        /// Find the minimum of the function within the interval {@code (lo, hi)}.
        /// 
        /// If the function is defined on the interval {@code (lo, hi)}, then
        /// this method finds an approximation {@code x} to the point at which
        /// the function attains its minimum.<br/>
        /// {@code t} and {@code eps} define a tolerance {@code tol = eps |x| + t}
        /// and the function is never evaluated at two points closer together than
        /// {@code tol}d {@code eps} should be no smaller than <em>2 macheps</em> and
        /// preferable not much less than <em>Sqrt(macheps)</em>, where
        /// <em>macheps</em> is the relative machine precisiond {@code t} should be
        /// positive.
        /// </summary>
        /// <param Name="isMinim">{@code true} when minimizing the function.</param>
        /// <param Name="lo">Lower bound of the interval.</param>
        /// <param Name="mid">Point inside the interval {@code [lo, hi]}.</param>
        /// <param Name="hi">Higher bound of the interval.</param>
        /// <param Name="eps">Relative accuracy.</param>
        /// <param Name="t">Absolute accuracy.</param>
        /// <returns>the optimum point.</returns>
        /// <exception cref="IndexOutOfRangeException">if the maximum iteration count </exception>
        /// is exceeded.
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the functiond </exception>
        private double localMin(Boolean isMinim, double lo, double mid, double hi, double eps, double t)
        {
            if (eps <= 0)
            {
                throw new NotStrictlyPositiveException(eps);
            }
            if (t <= 0)
            {
                throw new NotStrictlyPositiveException(t);
            }
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

            // Optional additional convergence criteria.
            IConvergenceChecker<double> checker = ConvergenceChecker;

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

            double previous = double.NaN;
            double current = x;
            double best = current;

            while (true)
            {
                double m = 0.5 * (a + b);
                double tol1 = eps * System.Math.Abs(x) + t;
                double tol2 = 2 * tol1;

                // Check stopping criterion.
                if (System.Math.Abs(x - m) > tol2 - 0.5 * (b - a))
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
                        if (fu <= fw || w == x)
                        {
                            v = w;
                            fv = fw;
                            w = u;
                            fw = fu;
                        }
                        else if (fu <= fv || v == x || v == w)
                        {
                            v = u;
                            fv = fu;
                        }
                    }
                }
                else
                { // termination
                    FunctionValue = isMinim ? fx : -fx;
                    return x;
                }
                incrementIterationsCounter();
            }
        }

        private double Best(double a, double b, Boolean isMinim)
        {
            if (Double.IsNaN(a))
            {
                return b;
            }
            if (Double.IsNaN(b))
            {
                return a;
            }

            if (isMinim)
            {
                return a <= b ? a : b;
            }
            else
            {
                return a >= b ? a : b;
            }
        }
        #endregion
    }
}
