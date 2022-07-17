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
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math.Polynomial;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Solver
{

    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/LaguerresMethod.html">
    /// Laguerre's Method</a> for root finding of real coefficient polynomials.
    /// For reference, see
    /// <blockquote>
    ///  <b>A First Course in Numerical Analysis</b>,
    ///  ISBN 048641454X, chapter 8.
    /// </blockquote>
    /// Laguerre's method is global in the sense that it can start with any initial
    /// approximation and be able to solve all roots from that point.
    /// The algorithm requires a bracketing condition.
    /// </summary>
    public class LaguerreSolver : AbstractPolynomialSolver
    {
        /** Default absolute accuracyd */
        private static double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;
        /** Complex solverd */
        private ComplexSolver complexSolver = new ComplexSolver();

        public LaguerreSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {

        }

        public LaguerreSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {

        }

        public LaguerreSolver(double relativeAccuracy,
                          double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {

        }

        public LaguerreSolver(double relativeAccuracy,
                          double absoluteAccuracy,
                          double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {

        }

        #region Inherited Methods
        public override double Solve(IUnivariateRealFunction f, double[] values, double startValue)
        {
            throw new NotImplementedException();
        }

        public override double Solve(IUnivariateRealFunction f, double[] values, double min, double max)
        {
            throw new NotImplementedException();
        }

        public override double Solve(IUnivariateRealFunction f, double[] values, double min, double max, double startValue)
        {
            throw new NotImplementedException();
        }
        #endregion

        protected override double DoSolve()
        {
            double min = Min;
            double max = Max;
            double initial = StartValue;
            double functionValueAccuracy = FunctionValueAccuracy;

            VerifySequence(min, initial, max);

            // Return the initial guess if it is good enough.
            double yInitial = ComputeObjectiveValue(initial);
            if (System.Math.Abs(yInitial) <= functionValueAccuracy)
            {
                return initial;
            }

            // Return the first endpoint if it is good enough.
            double yMin = ComputeObjectiveValue(min);
            if (System.Math.Abs(yMin) <= functionValueAccuracy)
            {
                return min;
            }

            // Reduce interval if min and initial bracket the root.
            if (yInitial * yMin < 0)
            {
                return Laguerre(min, initial, yMin, yInitial);
            }

            // Return the second endpoint if it is good enough.
            double yMax = ComputeObjectiveValue(max);
            if (System.Math.Abs(yMax) <= functionValueAccuracy)
            {
                return max;
            }

            // Reduce interval if initial and max bracket the root.
            if (yInitial * yMax < 0)
            {
                return Laguerre(initial, max, yInitial, yMax);
            }

            throw new NoBracketingException(min, max, yMin, yMax);
        }

        public double Laguerre(double lo, double hi, double fLo, double fHi)
        {
            Complex[] c = Coefficients.ConvertToComplex();

            Complex initial = new Complex(0.5 * (lo + hi), 0);
            Complex z = complexSolver.Solve(c, initial);
            if (complexSolver.IsRoot(lo, hi, z))
            {
                return z.Real;
            }
            else
            {
                double r = Double.NaN;
                // Solve all roots and select the one we are seeking.
                Complex[] root = complexSolver.SolveAll(c, initial);

                if (root.Any(x => complexSolver.IsRoot(lo, hi, x)))
                    r = root.Where(x => complexSolver.IsRoot(lo, hi, x)).First().Real;

                return r;
            }
        }

        public Complex[] SolveAllComplex(double[] coefficients, double initial)
        {
            return SolveAllComplex(coefficients, initial, int.MaxValue);
        }

        public Complex[] SolveAllComplex(double[] coefficients, double initial, int maxEval)
        {
            Setup(maxEval,
                  new PolynomialFunction(coefficients),
                  coefficients,
                  Double.NegativeInfinity,
                  Double.PositiveInfinity,
                  initial);
            return complexSolver.SolveAll(coefficients.ConvertToComplex(),
                                          new Complex(initial, 0d));
        }

        public Complex SolveComplex(double[] coefficients, double initial)
        {
            return SolveComplex(coefficients, initial, int.MaxValue);
        }

        public Complex SolveComplex(double[] coefficients, double initial, int maxEval)
        {
            Setup(maxEval,
                  new PolynomialFunction(coefficients),
                  coefficients,
                  Double.NegativeInfinity,
                  Double.PositiveInfinity,
                  initial);
            return complexSolver.Solve(coefficients.ConvertToComplex(),
                                       new Complex(initial, 0d));
        }

        /// <summary>
        /// Class for searching all (complex) roots.
        /// </summary>
        private class ComplexSolver : AbstractPolynomialSolver
        {
            /** Default absolute accuracyd */
            private static double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

            public ComplexSolver() : base(DEFAULT_ABSOLUTE_ACCURACY)
            {

            }

            /// <summary>
            /// Check whether the given complex root is actually a real zero
            /// in the given interval, within the solver tolerance level.
            /// </summary>
            /// <param name="min">Lower bound for the interval.</param>
            /// <param name="max">Upper bound for the interval.</param>
            /// <param name="z">Complex root.</param>
            /// <returns>{@code true} if z is a real zero.</returns>
            public Boolean IsRoot(double min, double max, Complex z)
            {
                if (IsSequence(min, z.Real, max))
                {
                    double tolerance = System.Math.Max(RelativeAccuracy * Complex.Abs(z), AbsoluteAccuracy);
                    return (System.Math.Abs(z.Imaginary) <= tolerance) ||
                         (Complex.Abs(z) <= FunctionValueAccuracy);
                }
                return false;
            }

            /// <summary>
            /// Find all complex roots for the polynomial with the given
            /// coefficients, starting from the given initial value.
            /// </summary>
            /// <param name="coefficients">Polynomial coefficients.</param>
            /// <param name="initial">Start value.</param>
            /// <returns>the point at which the function value is zero.</returns>
            /// <exception cref="TooManyEvaluationsException">if the maximum number of evaluations is exceeded.</exception>
            /// <exception cref="ArgumentNullException">if the {@code coefficients} is {@code null}.</exception>
            /// <exception cref="DataNotFoundException">if the {@code coefficients} array is empty.</exception>
            public Complex[] SolveAll(Complex[] coefficients, Complex initial)
            {
                if (coefficients == null)
                {
                    throw new ArgumentNullException();
                }
                int n = coefficients.Length - 1;
                if (n == 0)
                {
                    throw new DataNotFoundException(LocalizedResources.Instance().POLYNOMIAL);
                }
                // Coefficients for deflated polynomial.
                Complex[] c = new Complex[n + 1];
                AutoParallel.AutoParallelFor(0, n + 1, (i) =>
                {
                    c[i] = coefficients[i];
                });

                // Solve individual roots successively.
                Complex[] root = new Complex[n];
                AutoParallel.AutoParallelFor(0, n, (i) =>
                {
                    Complex[] subarray = new Complex[n - i + 1];
                    Array.Copy(c, 0, subarray, 0, subarray.Length);
                    root[i] = Solve(subarray, initial);
                    // Polynomial deflation using synthetic division.
                    Complex newc = c[n - i];
                    Complex oldc = 0;
                    AutoParallel.AutoParallelFor(0, n - i - 1, (t) =>
                    {
                        int j = (n - i - 1) - t;
                        oldc = c[j];
                        c[j] = newc;
                        newc = Complex.Add(oldc, Complex.Multiply(newc, root[i]));
                    });
                });

                return root;
            }

            /// <summary>
            /// Find a complex root for the polynomial with the given coefficients,
            /// starting from the given initial value.
            /// </summary>
            /// <param name="coefficients">Polynomial coefficients.</param>
            /// <param name="initial">Start value.</param>
            /// <returns>the point at which the function value is zero.</returns>
            /// <exception cref="TooManyEvaluationsException">if the maximum number of evaluations is exceeded.</exception>
            /// <exception cref="ArgumentNullException">if the {@code coefficients} is {@code null}.</exception>
            /// <exception cref="DataNotFoundException">if the {@code coefficients} array is empty.</exception>
            public Complex Solve(Complex[] coefficients, Complex initial)
            {
                if (coefficients == null)
                {
                    throw new ArgumentNullException();
                }

                int n = coefficients.Length - 1;
                if (n == 0)
                {
                    throw new DataNotFoundException(LocalizedResources.Instance().POLYNOMIAL);
                }

                double absoluteAccuracy = AbsoluteAccuracy;
                double relativeAccuracy = RelativeAccuracy;
                double functionValueAccuracy = FunctionValueAccuracy;

                Complex nC = new Complex(n, 0);
                Complex n1C = new Complex(n - 1, 0);

                Complex z = initial;
                Complex oldz = new Complex(Double.PositiveInfinity,
                                           Double.PositiveInfinity);
                while (true)
                {
                    // Compute pv (polynomial value), dv (derivative value), and
                    // d2v (second derivative value) simultaneously.
                    Complex pv = coefficients[n];
                    Complex dv = Complex.Zero;
                    Complex d2v = Complex.Zero;
                    AutoParallel.AutoParallelFor(0, n - 1, (i) =>
                   {
                       int j = (n - 1) - i;
                       d2v = Complex.Add(dv, Complex.Multiply(z, d2v));
                       dv = Complex.Add(pv, Complex.Multiply(z, dv));
                       pv = Complex.Add(coefficients[j], Complex.Multiply(z, pv));
                   });
                    d2v = Complex.Multiply(d2v, new Complex(2.0, 0.0));

                    // Check for convergence.
                    double tolerance = System.Math.Max(relativeAccuracy * Complex.Abs(z),
                                                          absoluteAccuracy);
                    if (Complex.Abs((Complex.Subtract(z, oldz))) <= tolerance)
                    {
                        return z;
                    }
                    if (Complex.Abs(pv) <= functionValueAccuracy)
                    {
                        return z;
                    }

                    // Now pv != 0, calculate the new approximation.
                    Complex G = Complex.Divide(dv, pv);
                    Complex G2 = Complex.Multiply(G, G);
                    Complex H = Complex.Subtract(G2, Complex.Divide(d2v, pv));
                    Complex delta = Complex.Multiply(n1C, Complex.Subtract((Complex.Multiply(nC, H)), G2));
                    // Choose a denominator larger in magnitude.
                    Complex deltaSqrt = Complex.Sqrt(delta);
                    Complex dplus = Complex.Add(G, deltaSqrt);
                    Complex dminus = Complex.Subtract(G, deltaSqrt);
                    Complex denominator = Complex.Abs(dplus) > Complex.Abs(dminus) ? dplus : dminus;
                    // Perturb z if denominator is zero, for instance,
                    // p(x) = x^3 + 1, z = 0.
                    if (denominator.Equals(new Complex(0.0, 0.0)))
                    {
                        z = Complex.Add(z, new Complex(absoluteAccuracy, absoluteAccuracy));
                        oldz = new Complex(Double.PositiveInfinity,
                                           Double.PositiveInfinity);
                    }
                    else
                    {
                        oldz = z;
                        z = Complex.Subtract(z, Complex.Divide(nC, denominator));
                    }

                    IncrementEvaluationCount();
                }
            }

            protected override double DoSolve()
            {
                throw new NotImplementedException();
            }
        }
    }
}

