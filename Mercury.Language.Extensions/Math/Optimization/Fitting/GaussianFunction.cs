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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>
    /// A Gaussian functiond  Specifically:
    /// <p>
    /// <i>f(x) = a + b*Exp(-((x - c)^2 / (2*d^2)))</i>
    /// <p>
    /// Notation key:
    /// <ul>
    /// <li><i>x^n</i>: <i>x</i> raised to the power of <i>n</i>
    /// <li><i>Exp(x)</i>: <i>e</i><i>^x</i>
    /// </ul>
    /// References:
    /// <ul>
    /// <li><a href="http://en.wikipedia.org/wiki/Gaussian_function">Wikipedia:
    ///   Gaussian function</a>
    /// </ul>
    /// 
    /// </summary>
    /// <see cref="GaussianDerivativeFunction"></see>
    /// <see cref="ParametricGaussianFunction"></see>
    /// @since 2.2
    /// @version $Revision: 1037327 $ $Date: 2010-11-20 21:57:37 +0100 (samd 20 novd 2010) $
    [Serializable]
    public class GaussianFunction : IDifferentiableUnivariateRealFunction
    {

        /// <summary>
        /// Parameter a of this functiond
        /// </summary>
        private double _a;

        /// <summary>
        /// Parameter b of this functiond
        /// </summary>
        private double _b;

        /// <summary>
        /// Parameter c of this functiond
        /// </summary>
        private double _c;

        /// <summary>
        /// Parameter d of this functiond
        /// </summary>
        private double _d;

        /// <summary>Parameter values/
        /// </summary>
        private double _paramValue;


        /// <summary>
        /// Gets <i>a</i> parameter value.
        /// 
        /// </summary>
        /// <returns><i>a</i> parameter value</returns>
        public double A
        {
            get { return _a; }
        }

        /// <summary>
        /// Gets <i>b</i> parameter value.
        /// 
        /// </summary>
        /// <returns><i>b</i> parameter value</returns>
        public double B
        {
            get { return _b; }
        }

        /// <summary>
        /// Gets <i>c</i> parameter value.
        /// 
        /// </summary>
        /// <returns><i>c</i> parameter value</returns>
        public double C
        {
            get { return _c; }
        }

        /// <summary>
        /// Gets <i>d</i> parameter value.
        /// 
        /// </summary>
        /// <returns><i>d</i> parameter value</returns>
        public double D
        {
            get { return _d; }
        }

        public double ParamValue
        {
            get { return _paramValue; }
        }

        /// <summary>
        /// Constructs an instance with the specified parameters.
        /// 
        /// </summary>
        /// <param Name="a"><i>a</i> parameter value</param>
        /// <param Name="b"><i>b</i> parameter value</param>
        /// <param Name="c"><i>c</i> parameter value</param>
        /// <param Name="d"><i>d</i> parameter value</param>
        /// 
        /// <exception cref="ArgumentException">if <code>d</code> is 0 </exception>
        public GaussianFunction(double a, double b, double c, double d)
        {
            if (d == 0.0)
            {
                throw new ZeroOperationException();
            }
            this._a = a;
            this._b = b;
            this._c = c;
            this._d = d;
        }

        /// <summary>
        /// Constructs an instance with the specified parameters.
        /// 
        /// </summary>
        /// <param Name="parameters"><i>a</i>, <i>b</i>, <i>c</i>, and <i>d</i></param>
        ///        parameter values
        /// 
        /// <exception cref="ArgumentException">if <code>parameters</code> is null, </exception>
        ///         <code>parameters</code> Length is not 4, or if
        ///         <code>parameters[3]</code> is 0
        public GaussianFunction(double[] parameters)
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
            this._a = parameters[0];
            this._b = parameters[1];
            this._c = parameters[2];
            this._d = parameters[3];
        }

        /// <summary>{@inheritDoc} */
        public IUnivariateRealFunction Derivative()
        {
            return new GaussianDerivativeFunction(_b, _c, _d);
        }

        /// <summary>{@inheritDoc} */
        public double Value(double x)
        {
            _paramValue = x;

            double xMc = x - _c;
            return _a + _b * System.Math.Exp(-xMc * xMc / (2.0 * (_d * _d)));
        }
    }
}
