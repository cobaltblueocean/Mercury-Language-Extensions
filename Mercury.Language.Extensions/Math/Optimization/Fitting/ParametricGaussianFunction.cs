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

/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreementsd  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the Licensed  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

using Mercury.Language.Math.Optimization.Fitting;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>
    /// A Gaussian functiond  Specifically:
    /// <p>
    /// <i>f(x) = a + b*Exp(-((x - c)^2 / (2*d^2)))</i>
    /// <p>
    /// The parameters have the following meaning:
    /// <ul>
    /// <li><i>a</i> is a constant offset that shifts <i>f(x)</i> up or down
    /// <li><i>b</i> is the height of the peak
    /// <li><i>c</i> is the position of the center of the peak
    /// <li><i>d</i> is related to the FWHM by <i>FWHM = 2*sqrt(2*ln(2))*d</i>
    /// </ul>
    /// Notation key:
    /// <ul>
    /// <li><i>x^n</i>: <i>x</i> raised to the power of <i>n</i>
    /// <li><i>Exp(x)</i>: <i>e</i><i>^x</i>
    /// <li><i>sqrt(x)</i>: the square root of <i>x</i>
    /// <li><i>ln(x)</i>: the natural logarithm of <i>x</i>
    /// </ul>
    /// References:
    /// <ul>
    /// <li><a href="http://en.wikipedia.org/wiki/Gaussian_function">Wikipedia:
    ///   Gaussian function</a>
    /// </ul>
    /// 
    /// @since 2.2
    /// @version $Revision: 1037327 $ $Date: 2010-11-20 21:57:37 +0100 (samd 20 novd 2010) $
    /// </summary>
    [Serializable]
    public class ParametricGaussianFunction : IParametricRealFunction
    {

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        public ParametricGaussianFunction()
        {
        }

        /// <summary>
        /// Computes value of function <i>f(x)</i> for the specified <i>x</i> and
        /// parameters <i>a</i>, <i>b</i>, <i>c</i>, and <i>d</i>.
        /// 
        /// </summary>
        /// <param Name="x"><i>x</i> value</param>
        /// <param Name="parameters">values of <i>a</i>, <i>b</i>, <i>c</i>, and</param>
        ///        <i>d</i>
        /// 
        /// <returns>value of <i>f(x)</i> evaluated at <i>x</i> with the specified</returns>
        ///         parameters
        /// 
        /// <exception cref="ArgumentException">if <code>parameters</code> is invalid as </exception>
        ///         determined by {@link #validateParameters(double[])}
        /// <exception cref="ZeroOperationException">if <code>parameters</code> values are </exception>
        ///         invalid as determined by {@link #validateParameters(double[])}
        public double Value(double x, double[] parameters)
        {
            validateParameters(parameters);
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            double d = parameters[3];
            double xMc = x - c;
            return a + b * System.Math.Exp(-xMc * xMc / (2.0 * (d * d)));
        }

        /// <summary>
        /// Computes the gradient vector for a four variable version of the function
        /// where the parameters, <i>a</i>, <i>b</i>, <i>c</i>, and <i>d</i>,
        /// are considered the variables, not <i>x</i>d  That is, instead of
        /// computing the gradient vector for the function <i>f(x)</i> (which would
        /// just be the derivative of <i>f(x)</i> with respect to <i>x</i> since
        /// it's a one-dimensional function), computes the gradient vector for the
        /// function <i>f(a, b, c, d) = a + b*Exp(-((x - c)^2 / (2*d^2)))</i>
        /// treating the specified <i>x</i> as a constant.
        /// <p>
        /// The components of the computed gradient vector are the partial
        /// derivatives of <i>f(a, b, c, d)</i> with respect to each variable.
        /// That is, the partial derivative of <i>f(a, b, c, d)</i> with respect to
        /// <i>a</i>, the partial derivative of <i>f(a, b, c, d)</i> with respect
        /// to <i>b</i>, the partial derivative of <i>f(a, b, c, d)</i> with
        /// respect to <i>c</i>, and the partial derivative of <i>f(a, b, c,
        /// d)</i> with respect to <i>d</i>.
        /// 
        /// </summary>
        /// <param Name="x"><i>x</i> value to be used as constant in <i>f(a, b, c,</param>
        ///        d)</i>
        /// <param Name="parameters">values of <i>a</i>, <i>b</i>, <i>c</i>, and</param>
        ///        <i>d</i> for computation of gradient vector of <i>f(a, b, c,
        ///        d)</i>
        /// 
        /// <returns>gradient vector of <i>f(a, b, c, d)</i></returns>
        /// 
        /// <exception cref="ArgumentException">if <code>parameters</code> is invalid as </exception>
        ///         determined by {@link #validateParameters(double[])}
        /// <exception cref="ZeroOperationException">if <code>parameters</code> values are </exception>
        ///         invalid as determined by {@link #validateParameters(double[])}
        public double[] Gradient(double x, double[] parameters)
        {

            validateParameters(parameters);
            double b = parameters[1];
            double c = parameters[2];
            double d = parameters[3];

            double xMc = x - c;
            double d2 = d * d;
            double exp = System.Math.Exp(-xMc * xMc / (2 * d2));
            double f = b * exp * xMc / d2;

            return new double[] { 1.0, exp, f, f * xMc / d };
        }

        /// <summary>
        /// Validates parameters to ensure they are appropriate for the evaluation of
        /// the <code>value</code> and <code>gradient</code> methods.
        /// 
        /// </summary>
        /// <param Name="parameters">values of <i>a</i>, <i>b</i>, <i>c</i>, and</param>
        ///        <i>d</i>
        /// 
        /// <exception cref="ArgumentException">if <code>parameters</code> is </exception>
        ///         <code>null</code> or if <code>parameters</code> does not have
        ///         Length == 4
        /// <exception cref="ZeroOperationException">if <code>parameters[3]</code> </exception>
        ///         (<i>d</i>) is 0
        private void validateParameters(double[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().INPUT_ARRAY);
            }
            if (parameters.Length != 4)
            {
                throw new DimensionMismatchException(4, parameters.Length);
            }
            if (parameters[3] == 0.0)
            {
                throw new ZeroOperationException();
            }
        }
    }
}
