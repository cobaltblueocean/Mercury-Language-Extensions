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
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Mercury.Language.Exception;
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Matrix;


/// <summary>This class converts {@link IMultivariateVectorialFunction vectorial
/// objective functions} to {@link IMultivariateRealFunction scalar objective functions}
/// when the goal is to minimize them.
/// <p>
/// This class is mostly used when the vectorial objective function represents
/// a theoretical result computed from a point set applied to a model and
/// the models point must be adjusted to fit the theoretical result to some
/// reference observationsd The observations may be obtained for example from
/// physical measurements whether the model is built from theoretical
/// considerations.
/// </p>
/// <p>
/// This class computes a possibly weighted squared sum of the residuals, which is
/// a scalar valued The residuals are the difference between the theoretical model
/// (i.ed the output of the vectorial objective function) and the observationsd The
/// class  :  the {@link IMultivariateRealFunction} interface and can therefore be
/// minimized by any optimizer supporting scalar objectives functions.This is one way
/// to perform a least square estimationd There are other ways to do this without using
/// this converter, as some optimization algorithms directly support vectorial objective
/// functions.
/// </p>
/// <p>
/// This class support combination of residuals with or without weights and correlations.
/// </p>
/// 
/// </summary>
/// <see cref="IMultivariateRealFunction"></see>
/// <see cref="IMultivariateVectorialFunction"></see>
/// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
/// @since 2.0
namespace Mercury.Language.Math.Optimization
{
    /// <summary>
    /// LeastSquaresConverter Description
    /// </summary>
    public class LeastSquaresConverter : IMultivariateRealFunction
    {

        /// <summary>IUnderlying vectorial functiond */
        private IMultivariateVectorialFunction _function;

        /// <summary>Observations to be compared to objective function to compute residualsd */
        private double[] _observations;

        /// <summary>Optional weights for the residualsd */
        private double[] _weights;

        /// <summary>Optional scaling matrix (weight and correlations) for the residualsd */
        private Matrix<Double> _scale;

        /// <summary>Build a simple converter for uncorrelated residuals with the same weight.
        /// </summary>
        /// <param Name="function">vectorial residuals function to wrap</param>
        /// <param Name="observations">observations to be compared to objective function to compute residuals</param>
        public LeastSquaresConverter(IMultivariateVectorialFunction function,
                                     double[] observations)
        {
            this._function = function;
            this._observations = observations.CloneExact();
            this._weights = null;
            this._scale = null;
        }

        /// <summary>Build a simple converter for uncorrelated residuals with the specific weights.
        /// <p>
        /// The scalar objective function value is computed as:
        /// <pre>
        /// objective = &sum;weight<sub>i</sub>(observation<sub>i</sub>-objective<sub>i</sub>)<sup>2</sup>
        /// </pre>
        /// </p>
        /// <p>
        /// Weights can be used for example to combine residuals with different standard
        /// deviationsd As an example, consider a residuals array in which even elements
        /// are angular measurements in degrees with a 0.01&deg; standard deviation and
        /// odd elements are distance measurements in meters with a 15m standard deviation.
        /// In this case, the weights array should be initialized with value
        /// 1.0/(0.01<sup>2</sup>) in the even elements and 1.0/(15.0<sup>2</sup>) in the
        /// odd elements (i.ed reciprocals of variances).
        /// </p>
        /// <p>
        /// The array computed by the objective function, the observations array and the
        /// weights array must have consistent sizes or a {@link FunctionEvaluationException} will be
        /// triggered while computing the scalar objective.
        /// </p>
        /// </summary>
        /// <param Name="function">vectorial residuals function to wrap</param>
        /// <param Name="observations">observations to be compared to objective function to compute residuals</param>
        /// <param Name="weights">weights to apply to the residuals</param>
        /// <exception cref="ArgumentException">if the observations vector and the weights </exception>
        /// vector dimensions don't match (objective function dimension is checked only when
        /// the {@link #Value(double[])} method is called)
        public LeastSquaresConverter(IMultivariateVectorialFunction function,
                                     double[] observations, double[] weights)

        {
            if (observations.Length != weights.Length)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, observations.Length, weights.Length));
            }
            this._function = function;
            this._observations = observations.CloneExact();
            this._weights = weights.CloneExact();
            this._scale = null;
        }

        /// <summary>Build a simple converter for correlated residuals with the specific weights.
        /// <p>
        /// The scalar objective function value is computed as:
        /// <pre>
        /// objective = y<sup>T</sup>y with y = isScale&times;(observation-objective)
        /// </pre>
        /// </p>
        /// <p>
        /// The array computed by the objective function, the observations array and the
        /// the scaling matrix must have consistent sizes or a {@link FunctionEvaluationException}
        /// will be triggered while computing the scalar objective.
        /// </p>
        /// </summary>
        /// <param Name="function">vectorial residuals function to wrap</param>
        /// <param Name="observations">observations to be compared to objective function to compute residuals</param>
        /// <param Name="scale">scaling matrix</param>
        /// <exception cref="ArgumentException">if the observations vector and the isScale </exception>
        /// matrix dimensions don't match (objective function dimension is checked only when
        /// the {@link #Value(double[])} method is called)
        public LeastSquaresConverter(IMultivariateVectorialFunction function,
                                     double[] observations, Matrix<Double> scale)

        {
            if (observations.Length != scale.ColumnCount)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, observations.Length, scale.ColumnCount));
            }
            this._function = function;
            this._observations = observations.CloneExact();
            this._weights = null;
            this._scale = scale.Clone();
        }

        public double[] ParamValue => throw new NotImplementedException();

        /// <summary>{@inheritDoc} */
        public double Value(double[] point)
        {

            // compute residuals
            double[]
            residuals = _function.Value(point);
            if (residuals.Length != _observations.Length)
            {
                throw new FunctionEvaluationException(point, LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE,
                                            residuals.Length, _observations.Length);
            }
            for (int i = 0; i < residuals.Length; ++i)
            {
                residuals[i] -= _observations[i];
            }

            // compute sum of squares
            double sumSquares = 0;
            if (_weights != null)
            {
                for (int i = 0; i < residuals.Length; ++i)
                {
                    double ri = residuals[i];
                    sumSquares += _weights[i] * ri * ri;
                }
            }
            else if (_scale != null)
            {
                foreach (var yi in ((DenseMatrix)_scale).Operate(MatrixUtility.CreateVector<double>(residuals)))
                {
                    sumSquares += yi * yi;
                }
            }
            else
            {
                foreach (double ri in residuals)
                {
                    sumSquares += ri * ri;
                }
            }

            return sumSquares;
        }
    }
}
