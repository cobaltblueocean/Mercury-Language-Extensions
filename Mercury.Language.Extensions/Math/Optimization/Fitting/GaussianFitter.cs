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
using Mercury.Language.Math.Optimization;

using Mercury.Language.Math.Optimization.Fitting;


namespace Mercury.Language.Math.Optimization.Fitting
{

    /// <summary>
    /// Fits points to a Gaussian function (that is, a {@link GaussianFunction}).
    /// <p>
    /// Usage example:
    /// <pre>
    ///   GaussianFitter fitter = new GaussianFitter(
    ///     new LevenbergMarquardtOptimizer());
    ///   fitter.AddObservedPoint(4.0254623,  531026.0);
    ///   fitter.AddObservedPoint(4.03128248, 984167.0);
    ///   fitter.AddObservedPoint(4.03839603, 1887233.0);
    ///   fitter.AddObservedPoint(4.04421621, 2687152.0);
    ///   fitter.AddObservedPoint(4.05132976, 3461228.0);
    ///   fitter.AddObservedPoint(4.05326982, 3580526.0);
    ///   fitter.AddObservedPoint(4.05779662, 3439750.0);
    ///   fitter.AddObservedPoint(4.0636168,  2877648.0);
    ///   fitter.AddObservedPoint(4.06943698, 2175960.0);
    ///   fitter.AddObservedPoint(4.07525716, 1447024.0);
    ///   fitter.AddObservedPoint(4.08237071, 717104.0);
    ///   fitter.AddObservedPoint(4.08366408, 620014.0);
    ///  GaussianFunction fitFunction = fitter.Fit();
    /// </pre>
    /// 
    /// </summary>
    /// <see cref="ParametricGaussianFunction"></see>
    /// @since 2.2
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    public class GaussianFitter
    {

        /// <summary>Fitter used for fittingd */
        private CurveFitter fitter;

        /// <summary>
        /// Constructs an instance using the specified optimizer.
        /// 
        /// </summary>
        /// <param Name="optimizer">optimizer to use for the fitting</param>
        public GaussianFitter(IDifferentiableMultivariateVectorialOptimizer optimizer)
        {
            fitter = new CurveFitter(optimizer);
        }

        /// <summary>
        /// Adds point (<code>x</code>, <code>y</code>) to list of observed points
        /// with a weight of 1.0d
        /// 
        /// </summary>
        /// <param Name="x"><i>x</i> point value</param>
        /// <param Name="y"><i>y</i> point value</param>
        public void AddObservedPoint(double x, double y)
        {
            AddObservedPoint(1.0, x, y);
        }

        /// <summary>
        /// Adds point (<code>x</code>, <code>y</code>) to list of observed points
        /// with a weight of <code>weight</code>.
        /// 
        /// </summary>
        /// <param Name="weight">weight assigned to point</param>
        /// <param Name="x"><i>x</i> point value</param>
        /// <param Name="y"><i>y</i> point value</param>
        public void AddObservedPoint(double weight, double x, double y)
        {
            fitter.AddObservedPoint(weight, x, y);
        }

        /// <summary>
        /// Fits Gaussian function to the observed points.
        /// 
        /// </summary>
        /// <returns>Gaussian function best fitting the observed points</returns>
        /// 
        /// <exception cref="FunctionEvaluationException">if <code>CurveFitter.fit</code> throws it </exception>
        /// <exception cref="OptimizationException">if <code>CurveFitter.fit</code> throws it </exception>
        /// <exception cref="ArgumentException">if <code>CurveFitter.fit</code> throws it </exception>
        /// 
        /// <see cref="CurveFitter"></see>
        public GaussianFunction Fit()
        {
            return new GaussianFunction(fitter.Fit(new ParametricGaussianFunction(), createParametersGuesser(fitter.Observations).guess()));
        }

        /// <summary>
        /// Factory method to create a <code>GaussianParametersGuesser</code>
        /// instance initialized with the specified observations.
        /// 
        /// </summary>
        /// <param Name="observations">points used to initialize the created</param>
        ///        <code>GaussianParametersGuesser</code> instance
        /// 
        /// <returns>new <code>GaussianParametersGuesser</code> instance</returns>
        protected GaussianParametersGuesser createParametersGuesser(WeightedObservedPoint[] observations)
        {
            return new GaussianParametersGuesser(observations);
        }
    }
}
