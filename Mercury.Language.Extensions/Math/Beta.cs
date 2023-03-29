// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math Inc.
//
// Copyright (C) 2012 - present by System.Math Inc. and the System.Math group of companies
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
using Mercury.Language.Exceptions;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Math
{
    /// <summary>
    /// This is a utility class that provides computation methods related to the Beta family of functions.
    /// </summary>
    public static class Beta
    {
        /// <summary>
        /// Maximum allowed numerical error.
        /// </summary>
        private static double DEFAULT_EPSILON = 10e-15;

        /// <summary>
        /// Returns the <a href="http://mathworld.wolfram.com/RegularizedBetaFunction.html"> regularized beta function</a> I(x, a, b).
        /// </summary>
        /// <param name="x">the value.</param>
        /// <param name="a">the a parameter.</param>
        /// <param name="b">the b parameter.</param>
        /// <returns>the regularized beta function I(x, a, b)</returns>
        public static double RegularizedBeta(double x, double a, double b)
        {
            return RegularizedBeta(x, a, b, DEFAULT_EPSILON, int.MaxValue);
        }

        /// <summary>
        /// Returns the <a href="http://mathworld.wolfram.com/RegularizedBetaFunction.html"> regularized beta function</a> I(x, a, b).
        /// </summary>
        /// <param name="x">the value.</param>
        /// <param name="a">the a parameter.</param>
        /// <param name="b">the b parameter.</param>
        /// <param name="epsilon">When the absolute value of the nth item in the series is less than epsilon the approximation ceases to calculate further elements in the series.</param>
        /// <returns>the regularized beta function I(x, a, b)</returns>
        public static double RegularizedBeta(double x, double a, double b, double epsilon)
        {
            return RegularizedBeta(x, a, b, epsilon, int.MaxValue);
        }

        /// <summary>
        /// Returns the <a href="http://mathworld.wolfram.com/RegularizedBetaFunction.html"> regularized beta function</a> I(x, a, b).
        /// </summary>
        /// <param name="x">the value.</param>
        /// <param name="a">the a parameter.</param>
        /// <param name="b">the b parameter.</param>
        /// <param name="maxIterations">Maximum number of "iterations" to complete.</param>
        /// <returns>the regularized beta function I(x, a, b)</returns>
        public static double RegularizedBeta(double x, double a, double b, int maxIterations)
        {
            return RegularizedBeta(x, a, b, DEFAULT_EPSILON, maxIterations);
        }

        /// <summary>
        /// Returns the <a href="http://mathworld.wolfram.com/RegularizedBetaFunction.html"> regularized beta function</a> I(x, a, b).
        /// </summary>
        /// <param name="x">the value.</param>
        /// <param name="a">the a parameter.</param>
        /// <param name="b">the b parameter.</param>
        /// <param name="epsilon">When the absolute value of the nth item in the series is less than epsilon the approximation ceases to calculate further elements in the series.</param>
        /// <param name="maxIterations">Maximum number of "iterations" to complete.</param>
        /// <returns>the regularized beta function I(x, a, b)</returns>
        public static double RegularizedBeta(double x, double a, double b, double epsilon, int maxIterations)
        {
            double ret;

            if (System.Double.IsNaN(x) || System.Double.IsNaN(a) || System.Double.IsNaN(b) || (x < 0) ||
                (x > 1) || (a <= 0.0) || (b <= 0.0))
            {
                ret = System.Double.NaN;
            }
            else if (x > (a + 1.0) / (a + b + 2.0))
            {
                ret = 1.0 - RegularizedBeta(1.0 - x, b, a, epsilon, maxIterations);
            }
            else
            {
                ContinuedFraction fraction = new BetaContinuedFraction(a, b);
                ret = System.Math.Exp((a * System.Math.Log(x)) + (b * System.Math.Log(1.0 - x)) -
                    System.Math.Log(a) - LogBeta(a, b, epsilon, maxIterations)) *
                    1.0 / fraction.Evaluate(x, epsilon, maxIterations);
            }

            return ret;
        }

        /// <summary>
        /// Returns the natural logarithm of the beta function B(a, b).
        /// </summary>
        /// <param name="a">the a parameter.</param>
        /// <param name="b">the b parameter.</param>
        /// <returns>log(B(a, b))</returns>
        public static double LogBeta(double a, double b)
        {
            return LogBeta(a, b, DEFAULT_EPSILON, int.MaxValue);
        }

        /// <summary>
        /// Returns the natural logarithm of the beta function B(a, b).
        /// </summary>
        /// <param name="a">the a parameter.</param>
        /// <param name="b">the b parameter.</param>
        /// <param name="epsilon">When the absolute value of the nth item in the series is less than epsilon the approximation ceases to calculate further elements in the series.</param>
        /// <param name="maxIterations">Maximum number of "iterations" to complete.</param>
        /// <returns>log(B(a, b))</returns>
        public static double LogBeta(double a, double b, double epsilon,
            int maxIterations)
        {

            double ret;

            if (System.Double.IsNaN(a) || System.Double.IsNaN(b) || (a <= 0.0) || (b <= 0.0))
            {
                ret = System.Double.NaN;
            }
            else
            {
                ret = Gamma.Log(a) + Gamma.Log(b) -
                    Gamma.Log(a + b);
            }

            return ret;
        }

        private class BetaContinuedFraction : ContinuedFraction
        {
            private double _a;
            private double _b;

            public BetaContinuedFraction(double a, double b)
            {
                _a = a;
                _b = b;
            }

            protected override double GetB(int n, double x)
            {
                double ret;
                double m;
                if (n % 2 == 0)
                { // even
                    m = n / 2.0;
                    ret = (m * (_b - m) * x) /
                        ((_a + (2 * m) - 1) * (_a + (2 * m)));
                }
                else
                {
                    m = (n - 1.0) / 2.0;
                    ret = -((_a + m) * (_a + _b + m) * x) /
                            ((_a + (2 * m)) * (_a + (2 * m) + 1.0));
                }
                return ret;
            }


            protected override double GetA(int n, double x)
            {
                return 1.0;
            }
        }
    }
}

