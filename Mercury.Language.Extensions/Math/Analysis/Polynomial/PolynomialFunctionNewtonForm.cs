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
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Analysis.Polynomial
{
    /// <summary>
    /// Implements the representation of a real polynomial function in
    /// Newton Formd For reference, see <b>Elementary Numerical Analysis</b>,
    /// ISBN 0070124477, chapter 2d
    /// <p>
    /// The formula of polynomial in Newton form is
    ///     p(x) = a[0] + a[1](x-c[0]) + a[2](x-c[0])(x-c[1]) + [] +
    ///            a[n](x-c[0])(x-c[1])...(x-c[n-1])
    /// Note that the Length of a[] is one more than the Length of c[]</p>
    //
    /// @version $Revision: 1073498 $ $Date: 2011-02-22 21:57:26 +0100 (mard 22 févrd 2011) $
    /// @since 1.2
    /// </summary>
    public class PolynomialFunctionNewtonForm : IUnivariateRealFunction
    {

        /// <summary>
        /// The coefficients of the polynomial, ordered by degree -- i.e.
        /// coefficients[0] is the constant term and coefficients[n] is the
        /// coefficient of x^n where n is the degree of the polynomial.
        /// </summary>
        private double[] coefficients;

        /// <summary>
        /// Centers of the Newton polynomial.
        /// </summary>
        private double[] c;

        /// <summary>
        /// When all c[i] = 0, a[] becomes normal polynomial coefficients,
        /// i.ed a[i] = coefficients[i].
        /// </summary>
        private double[] a;

        /// <summary>
        /// Whether the polynomial coefficients are available.
        /// </summary>
        private Boolean coefficientsComputed;

        private double _paramValue;

        /// <summary>
        /// Construct a Newton polynomial with the given a[] and c[]d The order of
        /// centers are important in that if c[] shuffle, then values of a[] would
        /// completely change, not just a permutation of old a[].
        /// <p>
        /// The constructor makes copy of the input arrays and assigns them.</p>
        /// </summary>
        /// <param name="a">the coefficients in Newton form formula</param>
        /// <param name="c">the centers</param>
        public PolynomialFunctionNewtonForm(double[] a, double[] c)
        {

            verifyInputArray(a, c);
            this.a = new double[a.Length];
            this.c = new double[c.Length];
            Array.Copy(a, 0, this.a, 0, a.Length);
            Array.Copy(c, 0, this.c, 0, c.Length);
            coefficientsComputed = false;
        }

        /// <summary>
        /// Calculate the function value at the given point.
        /// 
        /// </summary>
        /// <param Name="z">the point at which the function value is to be computed</param>
        /// <returns>the function value</returns>
        /// <exception cref="FunctionEvaluationException">if a runtime error occurs </exception>
        /// <see cref="IUnivariateRealFunction#Value(double)"></see>
        public double Value(double z)
        {
            _paramValue = z;
            return Evaluate(a, c, z);
        }

        /// <summary>
        /// Returns the degree of the polynomial.
        /// 
        /// </summary>
        /// <returns>the degree of the polynomial</returns>
        public int Degree
        {
            get { return c.Length; }
        }

        /// <summary>
        /// Returns a copy of coefficients in Newton form formula.
        /// <p>
        /// Changes made to the returned copy will not affect the polynomial.</p>
        /// 
        /// </summary>
        /// <returns>a fresh copy of coefficients in Newton form formula</returns>
        public double[] NewtonCoefficients
        {
            get
            {
                double[] _out = new double[a.Length];
                Array.Copy(a, 0, _out, 0, a.Length);
                return _out;
            }
        }

        /// <summary>
        /// Returns a copy of the centers array.
        /// <p>
        /// Changes made to the returned copy will not affect the polynomial.</p>
        /// 
        /// </summary>
        /// <returns>a fresh copy of the centers array</returns>
        public double[] Centers
        {
            get
            {
                double[] _out = new double[c.Length];
                Array.Copy(c, 0, _out, 0, c.Length);
                return _out;
            }
        }

        /// <summary>
        /// Returns a copy of the coefficients array.
        /// <p>
        /// Changes made to the returned copy will not affect the polynomial.</p>
        /// 
        /// </summary>
        /// <returns>a fresh copy of the coefficients array</returns>
        public double[] Coefficients
        {
            get
            {
                if (!coefficientsComputed)
                {
                    computeCoefficients();
                }
                double[] _out = new double[coefficients.Length];
                Array.Copy(coefficients, 0, _out, 0, coefficients.Length);
                return _out;
            }
        }

        public double ParamValue => _paramValue;

        /// <summary>
        /// Evaluate the Newton polynomial using nested multiplicationd It is
        /// also called <a href="http://mathworld.wolfram.com/HornersRule.html">
        /// Horner's Rule</a> and takes O(N) time.
        /// 
        /// </summary>
        /// <param Name="a">the coefficients in Newton form formula</param>
        /// <param Name="c">the centers</param>
        /// <param Name="z">the point at which the function value is to be computed</param>
        /// <returns>the function value</returns>
        /// <exception cref="FunctionEvaluationException">if a runtime error occurs </exception>
        /// <exception cref="ArgumentException">if inputs are not valid </exception>
        public static double Evaluate(double[] a, double[] c, double z)
        {

            verifyInputArray(a, c);

            int n = c.Length;
            double value = a[n];
            //for(int i = n - 1; i >= 0; i--) {
            AutoParallel.AutoParallelFor(n - 1, 0, (i) =>
            {
                value = a[i] + (z - c[i]) * value;
            }, true);
            //}

            return value;
        }

        /// <summary>
        /// Calculate the normal polynomial coefficients given the Newton form.
        /// It also uses nested multiplication but takes O(N^2) time.
        /// </summary>
        protected void computeCoefficients()
        {
            int n = Degree;

            coefficients = new double[n + 1];
            //for(int i = 0; i <= n; i++)
            AutoParallel.AutoParallelFor(0, n, (i) =>
            {
                coefficients[i] = 0.0;
            }, true);

            coefficients[0] = a[n];
            //for(int i = n - 1; i >= 0; i--)
            //{
            AutoParallel.AutoParallelFor(n - 1, 0, (i) =>
            {
                //    for(int j = n - i; j > 0; j--)
                //    {
                AutoParallel.AutoParallelFor(n - i, 0, (j) =>
                {
                    coefficients[j] = coefficients[j - 1] - c[i] * coefficients[j];
                });
                coefficients[0] = a[i] - c[i] * coefficients[0];
            }, true);

            coefficientsComputed = true;
        }

        /// <summary>
        /// Verifies that the input arrays are valid.
        /// <p>
        /// The centers must be distinct for interpolation purposes, but not
        /// for general used Thus it is not verified here.</p>
        /// 
        /// </summary>
        /// <param Name="a">the coefficients in Newton form formula</param>
        /// <param Name="c">the centers</param>
        /// <exception cref="ArgumentException">if not valid </exception>
        /// <see cref="Mercury.Language.Math.Analysis.Interpolation.DividedDifferenceInterpolator#computeDividedDifference(double[],double[])"></see>
        protected static void verifyInputArray(double[] a, double[] c)
        {

            if (a.Length < 1 || c.Length < 1)
            {
                throw new ArgumentException(LocalizedResources.Instance().EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY);
            }
            if (a.Length != c.Length + 1)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1, a.Length, c.Length));
            }
        }
    }
}
