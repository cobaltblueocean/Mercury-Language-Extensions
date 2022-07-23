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
using Mercury.Language.Math;
using Mercury.Language.Math.Polynomial;
using Mercury.Language.Math.Optimization;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>This class  :  a Curve fitting specialized for polynomials.
    /// <p>Polynomial fitting is a very simple case of Curve fittingd The
    /// estimated coefficients are the polynomial coefficientsd They are
    /// searched by a least square estimator.</p>
    /// @version $Revision: 1073270 $ $Date: 2011-02-22 10:19:27 +0100 (mard 22 févrd 2011) $
    /// @since 2.0
    /// </summary>

    public class PolynomialFitter
    {

        /// <summary>Fitter for the coefficientsd */
        private CurveFitter fitter;

        /// <summary>Polynomial degreed */
        private int degree;

        /// <summary>Simple constructor.
        /// <p>The polynomial fitter built this way are complete polynomials,
        /// ied a n-degree polynomial has n+1 coefficients.</p>
        /// </summary>
        /// <param Name="degree">maximal degree of the polynomial</param>
        /// <param Name="optimizer">optimizer to use for the fitting</param>
        public PolynomialFitter(int degree, IDifferentiableMultivariateVectorialOptimizer optimizer)
        {
            this.fitter = new CurveFitter(optimizer);
            this.degree = degree;
        }

        /// <summary>Add an observed weighted (x,y) point to the sample.
        /// </summary>
        /// <param Name="weight">weight of the observed point in the fit</param>
        /// <param Name="x">abscissa of the point</param>
        /// <param Name="y">observed value of the point at x, after fitting we should</param>
        /// have P(x) as close as possible to this value
        public void AddObservedPoint(double weight, double x, double y)
        {
            fitter.AddObservedPoint(weight, x, y);
        }

        /// <summary>
        /// Remove all observations.
        /// @since 2.2
        /// </summary>
        public void ClearObservations()
        {
            fitter.ClearObservations();
        }

        /// <summary>Get the polynomial fitting the weighted (x, y) points.
        /// </summary>
        /// <returns>polynomial function best fitting the observed points</returns>
        /// <exception cref="OptimizationException">if the algorithm failed to converge </exception>
        public PolynomialFunction Fit()
        {
            try
            {
                return new PolynomialFunction(fitter.Fit(new ParametricPolynomial(), new double[degree + 1]));
            }
            catch (FunctionEvaluationException fee)
            {
                // should never happen
                throw new System.Exception(LocalizedResources.Instance().FUNCTION, fee);
            }
        }

        /// <summary>Dedicated parametric polynomial classd */
        private class ParametricPolynomial : IParametricRealFunction
        {

            /// <summary>{@inheritDoc} */
            public double[] Gradient(double x, double[] parameters)
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

            /// <summary>{@inheritDoc} */
            public double Value(double x, double[] parameters)
            {
                double y = 0;
                for (int i = parameters.Length - 1; i >= 0; --i)
                {
                    y = y * x + parameters[i];
                }
                return y;
            }
        }
    }
}
