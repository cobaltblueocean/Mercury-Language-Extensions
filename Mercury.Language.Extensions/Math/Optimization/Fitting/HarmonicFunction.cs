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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>Harmonic function of the form <code>f (t) = a cos (&omega; t + &phi;)</code>.
    /// @version $Revision: 990655 $ $Date: 2010-08-29 23:49:40 +0200 (dimd 29 août 2010) $
    /// @since 2.0
    /// </summary>
    public class HarmonicFunction : IDifferentiableUnivariateRealFunction
    {

        /// <summary>Amplitude ad */
        private double a;

        /// <summary>Pulsation &omega;d */
        private double omega;

        /// <summary>Phase &phi;d */
        private double phi;

        private double _paramValue;

        /// <summary>Get the amplitude a.
        /// </summary>
        /// <returns>amplitude a;</returns>
        public double Amplitude
        {
            get { return a; }
        }

        /// <summary>Get the pulsation &omega;.
        /// </summary>
        /// <returns>pulsation &omega;</returns>
        public double Pulsation
        {
            get { return omega; }
        }

        /// <summary>Get the phase &phi;.
        /// </summary>
        /// <returns>phase &phi;</returns>
        public double Phase
        {
            get { return phi; }
        }

        public double ParamValue
        {
            get { return _paramValue; }
        }

        /// <summary>Simple constructor.
        /// </summary>
        /// <param Name="a">amplitude</param>
        /// <param Name="omega">pulsation</param>
        /// <param Name="phi">phase</param>
        public HarmonicFunction(double a, double omega, double phi)
        {
            this.a = a;
            this.omega = omega;
            this.phi = phi;
        }

        /// <summary>
        /// {@inheritDoc} 
        /// </summary>
        public double Value(double x)
        {
            _paramValue = x;
            return a * System.Math.Cos(omega * x + phi);
        }

        /// <summary>
        /// {@inheritDoc} 
        /// </summary>
        public IUnivariateRealFunction Derivative()
        {
            return new HarmonicFunction(a * omega, omega, phi + System.Math.PI / 2);
        }
    }
}
