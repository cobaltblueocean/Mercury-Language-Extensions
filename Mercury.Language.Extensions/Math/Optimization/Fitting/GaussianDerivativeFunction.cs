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

/*
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
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>
    /// The derivative of {@link GaussianFunction}d  Specifically:
    /// <p>
    /// <i>f'(x) = (-b / (d^2)) * (x - c) * Exp(-((x - c)^2) / (2*(d^2)))</i>
    /// <p>
    /// Notation key:
    /// <ul>
    /// <li><i>x^n</i>: <i>x</i> raised to the power of <i>n</i>
    /// <li><i>Exp(x)</i>: <i>e</i><i>^x</i>
    /// </ul>
    /// 
    /// @since 2.2
    /// @version $Revision: 1037327 $ $Date: 2010-11-20 21:57:37 +0100 (samd 20 novd 2010) $
    /// </summary>
    [Serializable]
    public class GaussianDerivativeFunction : IUnivariateRealFunction
    {

        /// <summary>Parameter b of this functiond */
        private double b;

        /// <summary>Parameter c of this functiond */
        private double c;

        /// <summary>Square of the parameter d of this functiond */
        private double d2;

        public double ParamValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructs an instance with the specified parameters.
        /// 
        /// </summary>
        /// <param Name="b"><i>b</i> parameter value</param>
        /// <param Name="c"><i>c</i> parameter value</param>
        /// <param Name="d"><i>d</i> parameter value</param>
        /// 
        /// <exception cref="ArgumentException">if <code>d</code> is 0 </exception>
        public GaussianDerivativeFunction(double b, double c, double d)
        {
            if (d == 0.0)
            {
                throw new ZeroOperationException();
            }
            this.b = b;
            this.c = c;
            this.d2 = d * d;
        }

        /// <summary>
        /// Constructs an instance with the specified parameters.
        /// 
        /// </summary>
        /// <param Name="parameters"><i>b</i>, <i>c</i>, and <i>d</i> parameter values</param>
        /// 
        /// <exception cref="ArgumentException">if <code>parameters</code> is null, </exception>
        ///         <code>parameters</code> Length is not 3, or if
        ///         <code>parameters[2]</code> is 0
        public GaussianDerivativeFunction(double[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().INPUT_ARRAY);
            }
            if (parameters.Length != 3)
            {
                throw new DimensionMismatchException(3, parameters.Length);
            }
            if (parameters[2] == 0.0)
            {
                throw new ZeroOperationException();
            }
            this.b = parameters[0];
            this.c = parameters[1];
            this.d2 = parameters[2] * parameters[2];
        }

        /// <summary>
        /// {@inheritDoc} 
        /// </summary>
        public double Value(double x)
        {
            ParamValue = x;

            double xMc = x - c;
            return (-b / d2) * xMc * System.Math.Exp(-(xMc * xMc) / (2.0 * d2));
        }
    }
}
