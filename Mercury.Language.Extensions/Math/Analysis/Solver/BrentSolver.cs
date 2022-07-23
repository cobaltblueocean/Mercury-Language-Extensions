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
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// BrentSolver Description
    /// </summary>
    public class BrentSolver : AbstractUnivariateSolver<Double>
    {

        #region Local Variables
        /// <summary>
        /// Default absolute accuracyd
        /// </summary>
        private static double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Default maximum number of iterations
        /// </summary>
        public static int DEFAULT_MAXIMUM_ITERATIONS = 100;


        #endregion

        #region Property

        #endregion

        #region Constructor
        /// <summary>
        /// Construct a solver with default absolute accuracy (1e-6).
        /// </summary>
        public BrentSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {

        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        public BrentSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {

        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy">Relative accuracy.</param>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        public BrentSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {

        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy">Relative accuracy.</param>
        /// <param name="absoluteAccuracy">Absolute accuracy.</param>
        /// <param name="functionValueAccuracy">Function value accuracy.</param>
        public BrentSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {

        }
        #endregion

        #region Implement Methods
        /// <summary>
        /// Find a zero in the given interval with an initial guess.
        /// <p>Throws <code>IllegalArgumentException</code> if the values of the
        /// function at the three points have the same sign (note that it is
        /// allowed to have endpoints with the same sign if the initial point has
        /// opposite sign function-wise).</p>
        /// 
        /// </summary>
        /// <param name="f">function to solve.</param>
        /// <param name="min">the lower bound for the interval.</param>
        /// <param name="max">the upper bound for the interval.</param>
        /// <param name="initial">the start value to use (must be set to min if no</param>
        /// initial point is known).
        /// <returns>the value where the function is zero</returns>
        /// <exception cref="MaxIterationsExceededException">the maximum iteration count is exceeded </exception>
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating  the function </exception>
        /// <exception cref="IllegalArgumentException">if initial is not between min and max </exception>
        /// (even if it <em>is</em> a root)
        public override double Solve(IUnivariateRealFunction f, double min, double max, double initial)
        {
            return Solve(DEFAULT_MAXIMUM_ITERATIONS, f, min, max, initial);
        }

        /// <summary>
        /// Find a zero in the given interval with an initial guess.
        /// <p>Throws <code>IllegalArgumentException</code> if the values of the
        /// function at the three points have the same sign (note that it is
        /// allowed to have endpoints with the same sign if the initial point has
        /// opposite sign function-wise).</p>
        /// 
        /// </summary>
        /// <param name="f">function to solve.</param>
        /// <param name="min">the lower bound for the interval.</param>
        /// <param name="max">the upper bound for the interval.</param>
        /// <param name="initial">the start value to use (must be set to min if no</param>
        /// initial point is known).
        /// <param name="maxEval">Maximum number of evaluations.</param>
        /// <returns>the value where the function is zero</returns>
        /// <exception cref="MaxIterationsExceededException">the maximum iteration count is exceeded </exception>
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating  the function </exception>
        /// <exception cref="IllegalArgumentException">if initial is not between min and max </exception>
        /// (even if it <em>is</em> a root)
        public double Solve(int maxEval, IUnivariateRealFunction f, double min, double max, double initial)
        {
            maximalIterationCount = maxEval;

            ClearResult();
            VerifyInterval(min, max);

            double ret = Double.NaN;

            double yMin = f.Value(min);
            double yMax = f.Value(max);

            // Verify bracketing
            double sign = yMin * yMax;
            if (sign > 0)
            {
                // check if either value is close to a zero
                if (System.Math.Abs(yMin) <= functionValueAccuracy)
                {
                    SetResult(min, 0);
                    ret = min;
                }
                else if (System.Math.Abs(yMax) <= functionValueAccuracy)
                {
                    SetResult(max, 0);
                    ret = max;
                }
                else
                {
                    // neither value is close to zero and min and max do not bracket root.
                    throw new MathArgumentException(LocalizedResources.Instance().SAME_SIGN_AT_ENDPOINTS, min, max, yMin, yMax);
                }
            }
            else if (sign < 0)
            {
                // solve using only the first endpoint as initial guess
                ret = solve(f, min, yMin, max, yMax, min, yMin);
            }
            else
            {
                // either min or max is a root
                if (yMin == 0.0)
                {
                    ret = min;
                }
                else
                {
                    ret = max;
                }
            }

            return ret;
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
        private double Brent(double lo, double hi, double fLo, double fHi)
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

        /// <summary>
        /// Find a zero starting search according to the three provided points.
        /// </summary>
        /// <param Name="f">the function to solve</param>
        /// <param Name="x0">old approximation for the root</param>
        /// <param Name="y0">function value at the approximation for the root</param>
        /// <param Name="x1">last calculated approximation for the root</param>
        /// <param Name="y1">function value at the last calculated approximation</param>
        /// for the root
        /// <param Name="x2">bracket point (must be set to x0 if no bracket point is</param>
        /// known, this will force starting with linear interpolation)
        /// <param Name="y2">function value at the bracket point.</param>
        /// <returns>the value where the function is zero</returns>
        /// <exception cref="IndexOutOfRangeException">if the maximum iteration count is exceeded </exception>
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the function </exception>
        private double solve(IUnivariateRealFunction f, double x0, double y0, double x1, double y1, double x2, double y2)
        {

            double delta = x1 - x0;
            double oldDelta = delta;

            int i = 0;
            while (i < DEFAULT_MAXIMUM_ITERATIONS)
            {
                if (System.Math.Abs(y2) < System.Math.Abs(y1))
                {
                    // use the bracket point if is better than last approximation
                    x0 = x1;
                    x1 = x2;
                    x2 = x0;
                    y0 = y1;
                    y1 = y2;
                    y2 = y0;
                }
                if (System.Math.Abs(y1) <= DEFAULT_FUNCTION_VALUE_ACCURACY)
                {
                    // Avoid division by very small valuesd Assume
                    // the iteration has converged (the problem may
                    // still be ill conditioned)
                    SetResult(x1, i);
                    return result;
                }
                double dx = x2 - x1;
                double tolerance =
                    System.Math.Max(relativeAccuracy * System.Math.Abs(x1), absoluteAccuracy);
                if (System.Math.Abs(dx) <= tolerance)
                {
                    SetResult(x1, i);
                    return result;
                }
                if ((System.Math.Abs(oldDelta) < tolerance) ||
                        (System.Math.Abs(y0) <= System.Math.Abs(y1)))
                {
                    // Force bisection.
                    delta = 0.5 * dx;
                    oldDelta = delta;
                }
                else
                {
                    double r3 = y1 / y0;
                    double p;
                    double p1;
                    // the equality test (x0 == x2) is intentional,
                    // it is part of the original Brent's method,
                    // it should NOT be replaced by proximity test
                    if (x0 == x2)
                    {
                        // Linear interpolation.
                        p = dx * r3;
                        p1 = 1.0 - r3;
                    }
                    else
                    {
                        // Inverse quadratic interpolation.
                        double r1 = y0 / y2;
                        double r2 = y1 / y2;
                        p = r3 * (dx * r1 * (r1 - r2) - (x1 - x0) * (r2 - 1.0));
                        p1 = (r1 - 1.0) * (r2 - 1.0) * (r3 - 1.0);
                    }
                    if (p > 0.0)
                    {
                        p1 = -p1;
                    }
                    else
                    {
                        p = -p;
                    }
                    if (2.0 * p >= 1.5 * dx * p1 - System.Math.Abs(tolerance * p1) ||
                            p >= System.Math.Abs(0.5 * oldDelta * p1))
                    {
                        // Inverse quadratic interpolation gives a value
                        // in the wrong direction, or progress is slow.
                        // Fall back to bisection.
                        delta = 0.5 * dx;
                        oldDelta = delta;
                    }
                    else
                    {
                        oldDelta = delta;
                        delta = p / p1;
                    }
                }
                // Save old X1, Y1
                x0 = x1;
                y0 = y1;
                // Compute new X1, Y1
                if (System.Math.Abs(delta) > tolerance)
                {
                    x1 = x1 + delta;
                }
                else if (dx > 0.0)
                {
                    x1 = x1 + 0.5 * tolerance;
                }
                else if (dx <= 0.0)
                {
                    x1 = x1 - 0.5 * tolerance;
                }
                y1 = f.Value(x1);
                if ((y1 > 0) == (y2 > 0))
                {
                    x2 = x0;
                    y2 = y0;
                    delta = x1 - x0;
                    oldDelta = delta;
                }
                i++;
            }
            throw new IndexOutOfRangeException(DEFAULT_MAXIMUM_ITERATIONS.ToString());
        }
        #endregion
    }
}
