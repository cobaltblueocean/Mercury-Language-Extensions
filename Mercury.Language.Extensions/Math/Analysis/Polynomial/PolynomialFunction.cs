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
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Differentiation;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Polynomial
{
    /// <summary>
    /// PolynomialFunction Description
    /// </summary>
    public class PolynomialFunction: IDifferentiableUnivariateRealFunction, IUnivariateDifferentiableFunction
    {
        private double _paramValue;

        public double ParamValue
        {
            get { return _paramValue; }
        }

        /// <summary>
        /// The coefficients of the polynomial, ordered by degree -- i.ed,
        /// coefficients[0] is the constant term and coefficients[n] is the
        /// coefficient of x^n where n is the degree of the polynomial.
        /// </summary>
        private double[] _coefficients;

        /// <summary>
        /// Construct a polynomial with the given coefficientsd  The first element
        /// of the coefficients array is the constant termd  Higher degree
        /// coefficients follow in sequenced  The degree of the resulting polynomial
        /// is the index of the last non-null element of the array, or 0 if all elements
        /// are null.
        /// <p>
        /// The constructor makes a copy of the input array and assigns the copy to
        /// the coefficients property.</p>
        /// </summary>
        /// <param name="c">Polynomial coefficients.</param>
        /// <exception cref="ArgumentNullException">if {@code c} is {@code null}.</exception>
        /// <exception cref="DataNotFoundException">if {@code c} is empty.</exception>
        public PolynomialFunction(double[] c) : base()
        {
            Math2.CheckNotNull(c);
            int n = c.Length;
            if (n == 0)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY);
            }
            while ((n > 1) && (c[n - 1] == 0))
            {
                --n;
            }
            this._coefficients = new double[n];
            Array.Copy(c, 0, this._coefficients, 0, n);
        }

        /// <summary>
        /// Compute the value of the function for the given argument.
        /// <p>
        ///  The value returned is </p><p>
        ///  {@code coefficients[n] * x^n + ..d + coefficients[1] * x  + coefficients[0]}
        /// </p>
        /// </summary>
        /// <param name="x">Argument for which the function value should be computed.</param>
        /// <returns>the value of the polynomial at the given point.</returns>
        /// <see cref="Analysis.UnivariateRealFunction.Value(double)"/>
        public double Value(double x)
        {
            _paramValue = x;
            return Evaluate(_coefficients, x);
        }

        /// <summary>
        /// Returns the degree of the polynomial.
        /// </summary>
        public int Degree
        {
            get { return _coefficients.Length - 1; }
        }

        /// <summary>
        /// Returns the coefficients array.
        /// </summary>
        public double[] Coefficients
        {
            get { return _coefficients; }
        }

        /// <summary>
        /// Uses Horner's Method to evaluate the polynomial with the given coefficients at
        /// the argument.
        /// </summary>
        /// <param name="coefficients">Coefficients of the polynomial to evaluate.</param>
        /// <param name="argument">Input value.</param>
        /// <returns>the value of the polynomial.</returns>
        /// <exception cref="DataNotFoundException">if {@code coefficients} is empty.</exception>
        /// <exception cref="ArgumentNullException">if {@code coefficients} is {@code null}.</exception>
        protected static double Evaluate(double[] coefficients, double argument)
        {
            Math2.CheckNotNull(coefficients);
            int n = coefficients.Length;
            if (n == 0)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY);
            }
            double result = coefficients[n - 1];
            for (int j = n - 2; j >= 0; j--)
            {
                result = argument * result + coefficients[j];
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="DataNotFoundException">if {@code coefficients} is empty.</exception>
        /// <exception cref="ArgumentNullException">if {@code coefficients} is {@code null}.</exception>
        public DerivativeStructure Value(DerivativeStructure t)
        {
            Math2.CheckNotNull(_coefficients);
            int n = _coefficients.Length;
            if (n == 0)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY);
            }
            DerivativeStructure result =
                    new DerivativeStructure(t.FreeParameters, t.Order, _coefficients[n - 1]);
            for (int j = n - 2; j >= 0; j--)
            {
                result = result.Multiply(t).Add(_coefficients[j]);
            }
            return result;
        }

        /// <summary>
        /// Add a polynomial to the instance.
        /// </summary>
        /// <param name="p">Polynomial to add.</param>
        /// <returns>a new polynomial which is the sum of the instance and {@code p}.</returns>
        public PolynomialFunction Add(PolynomialFunction p)
        {
            // identify the lowest degree polynomial
            int lowLength = System.Math.Min(_coefficients.Length, p._coefficients.Length);
            int highLength = System.Math.Max(_coefficients.Length, p._coefficients.Length);

            // build the coefficients array
            double[] newCoefficients = new double[highLength];
            for (int i = 0; i < lowLength; ++i)
            {
                newCoefficients[i] = _coefficients[i] + p._coefficients[i];
            }
            Array.Copy((_coefficients.Length < p._coefficients.Length) ?
                             p._coefficients : _coefficients,
                             lowLength,
                             newCoefficients, lowLength,
                             highLength - lowLength);

            return new PolynomialFunction(newCoefficients);
        }

        /// <summary>
        /// Subtract a polynomial from the instance.
        /// </summary>
        /// <param name="p">Polynomial to subtract.</param>
        /// <returns>a new polynomial which is the instance minus {@code p}.</returns>
        public PolynomialFunction Subtract(PolynomialFunction p)
        {
            // identify the lowest degree polynomial
            int lowLength = System.Math.Min(_coefficients.Length, p._coefficients.Length);
            int highLength = System.Math.Max(_coefficients.Length, p._coefficients.Length);

            // build the coefficients array
            double[] newCoefficients = new double[highLength];
            for (int i = 0; i < lowLength; ++i)
            {
                newCoefficients[i] = _coefficients[i] - p._coefficients[i];
            }
            if (_coefficients.Length < p._coefficients.Length)
            {
                for (int i = lowLength; i < highLength; ++i)
                {
                    newCoefficients[i] = -p._coefficients[i];
                }
            }
            else
            {
                Array.Copy(_coefficients, lowLength, newCoefficients, lowLength,
                                 highLength - lowLength);
            }

            return new PolynomialFunction(newCoefficients);
        }

        /// <summary>
        /// Negate the instance.
        /// </summary>
        /// <returns>a new polynomial with all coefficients negated</returns>
        public PolynomialFunction Negate()
        {
            double[] newCoefficients = new double[_coefficients.Length];
            for (int i = 0; i < _coefficients.Length; ++i)
            {
                newCoefficients[i] = -_coefficients[i];
            }
            return new PolynomialFunction(newCoefficients);
        }

        /// <summary>
        /// Multiply the instance by a polynomial.
        /// </summary>
        /// <param name="p">Polynomial to multiply by.</param>
        /// <returns>a new polynomial equal to this times {@code p}</returns>
        public PolynomialFunction Multiply(PolynomialFunction p)
        {
            double[] newCoefficients = new double[_coefficients.Length + p._coefficients.Length - 1];

            for (int i = 0; i < newCoefficients.Length; ++i)
            {
                newCoefficients[i] = 0.0;
                for (int j = System.Math.Max(0, i + 1 - p._coefficients.Length);
                     j < System.Math.Min(_coefficients.Length, i + 1);
                     ++j)
                {
                    newCoefficients[i] += _coefficients[j] * p._coefficients[i - j];
                }
            }

            return new PolynomialFunction(newCoefficients);
        }

        /// <summary>
        /// Returns the coefficients of the derivative of the polynomial with the given coefficients.
        /// </summary>
        /// <param name="coefficients">Coefficients of the polynomial to differentiate.</param>
        /// <returns>the coefficients of the derivative or {@code null} if coefficients has Length 1.</returns>
        /// <exception cref="DataNotFoundException">if {@code coefficients} is empty.</exception>
        /// <exception cref="ArgumentNullException">if {@code coefficients} is {@code null}.</exception>
        protected static double[] Differentiate(double[] coefficients)
        {
            Math2.CheckNotNull(coefficients);
            int n = coefficients.Length;
            if (n == 0)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY);
            }
            if (n == 1)
            {
                return new double[] { 0 };
            }
            double[] result = new double[n - 1];
            for (int i = n - 1; i > 0; i--)
            {
                result[i - 1] = i * coefficients[i];
            }
            return result;
        }

        /// <summary>
        /// Returns the derivative as a <see cref="PolynomialFunction"/>.
        /// </summary>
        /// <returns>the derivative polynomial.</returns>
        public PolynomialFunction PolynomialDerivative()
        {
            return new PolynomialFunction(Differentiate(_coefficients));
        }

        /// <summary>
        /// Returns the derivative as a <see cref="Analysis.UnivariateRealFunction"/>.
        /// </summary>
        /// <returns>the derivative function.</returns>
        public IUnivariateRealFunction Derivative()
        {
            return PolynomialDerivative();
        }

        /// <summary>
        /// Creates a string representing a coefficient, removing ".0" endings.
        /// </summary>
        /// <param name="coeff">Coefficient.</param>
        /// <returns>a string representation of {@code coeff}.</returns>
        private static String ToString(double coeff)
        {
            String c = coeff.ToString();
            if (c.EndsWith(".0"))
            {
                return c.Substring(0, c.Length - 2);
            }
            else
            {
                return c;
            }
        }

        IUnivariateRealFunction IDifferentiableUnivariateRealFunction.Derivative()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dedicated parametric polynomial class.
        /// </summary>
        public class Parametric : IParametricUnivariateRealFunction
        {
            /** {@inheritDoc} */
            public double[] Gradient(double x, params double[] parameters)
            {
                double[] gradient = new double[parameters.Length];
                double xn = 1.0;
                for (int i = 0; i < parameters.Length; ++i)
                {
                    gradient[i] = xn;
                    xn *= x;
                }
                return gradient;
            }

            /** {@inheritDoc} */
            public double Value(double x, params double[] parameters)

            {
                return PolynomialFunction.Evaluate(parameters, x);
            }
        }
    }
}
