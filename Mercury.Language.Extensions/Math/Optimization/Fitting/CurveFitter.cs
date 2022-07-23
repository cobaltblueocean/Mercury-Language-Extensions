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
using Mercury.Language.Math.Analysis.Solver;
using Mercury.Language.Math;
using Mercury.Language.Math.Optimization;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>Fitter for parametric univariate real functions y = f(x).
    /// <p>When a univariate real function y = f(x) does depend on some
    /// unknown parameters p<sub>0</sub>, p<sub>1</sub> [] p<sub>n-1</sub>,
    /// this class can be used to find these parametersd It does this
    /// by <em>fitting</em> the Curve so it remains very close to a set of
    /// observed points (x<sub>0</sub>, y<sub>0</sub>), (x<sub>1</sub>,
    /// y<sub>1</sub>) [] (x<sub>k-1</sub>, y<sub>k-1</sub>)d This fitting
    /// is done by finding the parameters values that minimizes the objective
    /// function &sum;(y<sub>i</sub>-f(x<sub>i</sub>))<sup>2</sup>d This is
    /// really a least squares problem.</p>
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public class CurveFitter
    {

        /// <summary>Optimizer to use for the fittingd */
        private IDifferentiableMultivariateVectorialOptimizer optimizer;

        /// <summary>Observed pointsd */
        private List<WeightedObservedPoint> observations;

        /// <summary>Simple constructor.
        /// </summary>
        /// <param Name="optimizer">optimizer to use for the fitting</param>
        public CurveFitter(IDifferentiableMultivariateVectorialOptimizer optimizer)
        {
            this.optimizer = optimizer;
            observations = new List<WeightedObservedPoint>();
        }

        /// <summary>Add an observed (x,y) point to the sample with unit weight.
        /// <p>Calling this method is equivalent to call
        /// <code>addObservedPoint(1.0, x, y)</code>.</p>
        /// </summary>
        /// <param Name="x">abscissa of the point</param>
        /// <param Name="y">observed value of the point at x, after fitting we should</param>
        /// have f(x) as close as possible to this value
        /// <see cref="#addObservedPoint(double,">double, double) </see>
        /// <see cref="#addObservedPoint(WeightedObservedPoint)"></see>
        /// <see cref="#Observations"></see>
        public void AddObservedPoint(double x, double y)
        {
            AddObservedPoint(1.0, x, y);
        }

        /// <summary>Add an observed weighted (x,y) point to the sample.
        /// </summary>
        /// <param Name="weight">weight of the observed point in the fit</param>
        /// <param Name="x">abscissa of the point</param>
        /// <param Name="y">observed value of the point at x, after fitting we should</param>
        /// have f(x) as close as possible to this value
        /// <see cref="#addObservedPoint(double,">double) </see>
        /// <see cref="#addObservedPoint(WeightedObservedPoint)"></see>
        /// <see cref="#Observations"></see>
        public void AddObservedPoint(double weight, double x, double y)
        {
            observations.Add(new WeightedObservedPoint(weight, x, y));
        }

        /// <summary>Add an observed weighted (x,y) point to the sample.
        /// </summary>
        /// <param Name="observed">observed point to add</param>
        /// <see cref="#addObservedPoint(double,">double) </see>
        /// <see cref="#addObservedPoint(double,">double, double) </see>
        /// <see cref="#Observations"></see>
        public void AddObservedPoint(WeightedObservedPoint observed)
        {
            observations.Add(observed);
        }

        /// <summary>Get the observed points.
        /// </summary>
        /// <returns>observed points</returns>
        /// <see cref="#addObservedPoint(double,">double) </see>
        /// <see cref="#addObservedPoint(double,">double, double) </see>
        /// <see cref="#addObservedPoint(WeightedObservedPoint)"></see>
        public WeightedObservedPoint[] Observations
        {
            get { return observations.ToArray(); }
        }

        /// <summary>
        /// Remove all observations.
        /// </summary>
        public void ClearObservations()
        {
            observations.Clear();
        }

        /// <summary>Fit a Curve.
        /// <p>This method compute the coefficients of the Curve that best
        /// fit the sample of observed points previously given through calls
        /// to the {@link #addObservedPoint(WeightedObservedPoint)
        /// addObservedPoint} method.</p>
        /// </summary>
        /// <param Name="f">parametric function to fit</param>
        /// <param Name="initialGuess">first guess of the function parameters</param>
        /// <returns>fitted parameters</returns>
        /// <exception cref="FunctionEvaluationException">if the objective function throws one during the search </exception>
        /// <exception cref="OptimizationException">if the algorithm failed to converge </exception>
        /// <exception cref="ArgumentException">if the start point dimension is wrong </exception>
        public double[] Fit(IParametricRealFunction f, double[] initialGuess)
        {

            // prepare least squares problem
            double[] target = new double[observations.Count];
            double[] weights = new double[observations.Count];
            int i = 0;
            AutoParallel.AutoParallelForEach(observations, (point) =>
            {
                target[i] = point.Y;
                weights[i] = point.Weight;
                ++i;
            });

            // perform the fit
            VectorialPointValuePair optimum =
                optimizer.Optimize(new TheoreticalValuesFunction(observations, f), target, weights, initialGuess);

            // extract the coefficients
            return optimum.PointReference;
        }

        /// <summary>Vectorial function computing function theoretical valuesd */
        private class TheoreticalValuesFunction : IDifferentiableMultivariateVectorialFunction
        {

            /// <summary>Function to fitd */
            private IParametricRealFunction _f;
            private List<WeightedObservedPoint> _observations;

            /// <summary>Simple constructor.
            /// </summary>
            /// <param Name="f">function to fit.</param>
            public TheoreticalValuesFunction(List<WeightedObservedPoint> observations, IParametricRealFunction f)
            {
                this._f = f;
                this._observations = observations;
            }

            /// <summary>{@inheritDoc} */
            public IMultivariateMatrixFunction Jacobian()
            {
                return new JcobianMultivariateMatrixFunction(_observations, _f);
            }

            /// <summary>{@inheritDoc} */
            public double[] Value(double[] point)
            {

                // compute the residuals
                double[] values = new double[_observations.Count];
                int i = 0;
                foreach (WeightedObservedPoint observed in _observations)
                {
                    values[i++] = _f.Value(observed.X, point);
                }

                return values;
            }
        }

        private class JcobianMultivariateMatrixFunction : IMultivariateMatrixFunction
        {
            List<WeightedObservedPoint> _observations;
            IParametricRealFunction _f;

            public JcobianMultivariateMatrixFunction(List<WeightedObservedPoint> observations, IParametricRealFunction f)
            {
                _observations = observations;
                _f = f;
            }

            public double[][] Value(double[] point)
            {

                double[][] jacobian = new double[_observations.Count][];

                int i = 0;
                foreach (WeightedObservedPoint observed in _observations)
                {
                    jacobian[i++] = _f.Gradient(observed.X, point);
                }

                return jacobian;

            }
        }
    }
}
