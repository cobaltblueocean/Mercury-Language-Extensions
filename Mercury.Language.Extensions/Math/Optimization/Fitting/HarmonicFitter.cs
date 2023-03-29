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
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Math.Optimization;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>This class  :  a Curve fitting specialized for sinusoids.
    /// <p>Harmonic fitting is a very simple case of Curve fittingd The
    /// estimated coefficients are the amplitude a, the pulsation &omega; and
    /// the phase &phi;: <code>f (t) = a cos (&omega; t + &phi;)</code>d They are
    /// searched by a least square estimator initialized with a rough guess
    /// based on integrals.</p>
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public class HarmonicFitter
    {

        /// <summary>Fitter for the coefficientsd */
        private CurveFitter fitter;

        /// <summary>Values for amplitude, pulsation &omega; and phase &phi;d */
        private double[] parameters;

        /// <summary>Simple constructor.
        /// </summary>
        /// <param Name="optimizer">optimizer to use for the fitting</param>
        public HarmonicFitter(IDifferentiableMultivariateVectorialOptimizer optimizer)
        {
            this.fitter = new CurveFitter(optimizer);
            parameters = null;
        }

        /// <summary>Simple constructor.
        /// <p>This constructor can be used when a first guess of the
        /// coefficients is already known.</p>
        /// </summary>
        /// <param Name="optimizer">optimizer to use for the fitting</param>
        /// <param Name="initialGuess">guessed values for amplitude (index 0),</param>
        /// pulsation &omega; (index 1) and phase &phi; (index 2)
        public HarmonicFitter(IDifferentiableMultivariateVectorialOptimizer optimizer, double[] initialGuess)
        {
            this.fitter = new CurveFitter(optimizer);
            this.parameters = initialGuess.CloneExact();
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

        /// <summary>Fit an harmonic function to the observed points.
        /// </summary>
        /// <returns>harmonic function best fitting the observed points</returns>
        /// <exception cref="OptimizationException">if the sample is too short or if </exception>
        /// the first guess cannot be computed
        public HarmonicFunction Fit()
        {

            // shall we compute the first guess of the parameters ourselves ?
            if (parameters == null)
            {
                WeightedObservedPoint[] observations = fitter.Observations;
                if (observations.Length < 4)
                {
                    throw new OptimizationException(LocalizedResources.Instance().INSUFFICIENT_OBSERVED_POINTS_IN_SAMPLE, observations.Length, 4);
                }

                HarmonicCoefficientsGuesser guesser = new HarmonicCoefficientsGuesser(observations);
                guesser.guess();
                parameters = new double[]
                {
                    guesser.GuessedAmplitude,
                    guesser.GuessedPulsation,
                    guesser.GuessedPhase
                };
            }

            try
            {
                double[] fitted = fitter.Fit(new ParametricHarmonicFunction(), parameters);
                return new HarmonicFunction(fitted[0], fitted[1], fitted[2]);
            }
            catch (FunctionEvaluationException fee)
            {
                // should never happen
                throw new System.Exception(LocalizedResources.Instance().FUNCTION, fee);
            }

        }

        /// <summary>Parametric harmonic functiond */
        private class ParametricHarmonicFunction : IParametricRealFunction
        {

            /// <summary>{@inheritDoc} */
            public double Value(double x, double[] parameters)
            {
                double a = parameters[0];
                double omega = parameters[1];
                double phi = parameters[2];
                return a * System.Math.Cos(omega * x + phi);
            }

            /// <summary>{@inheritDoc} */
            public double[] Gradient(double x, double[] parameters)
            {
                double a = parameters[0];
                double omega = parameters[1];
                double phi = parameters[2];
                double alpha = omega * x + phi;
                double cosAlpha = System.Math.Cos(alpha);
                double sinAlpha = System.Math.Sin(alpha);
                return new double[] { cosAlpha, -a * x * sinAlpha, -a * sinAlpha };
            }
        }
    }
}
