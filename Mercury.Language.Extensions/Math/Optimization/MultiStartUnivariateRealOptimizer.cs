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
using Mercury.Language.Math;


using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exception;
using Mercury.Language.Math.Random;

namespace Mercury.Language.Math.Optimization
{

    /// <summary>
    /// Special implementation of the {@link UnivariateRealOptimizer} interface adding
    /// multi-start features to an existing optimizer.
    /// <p>
    /// This class wraps a classical optimizer to use it several times in
    /// turn with different starting points in order to avoid being trapped
    /// into a local extremum when looking for a global one.
    /// </p>
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public class MultiStartUnivariateRealOptimizer : IUnivariateRealOptimizer
    {

        /// <summary>IUnderlying classical optimizerd */
        private IUnivariateRealOptimizer _optimizer;

        /// <summary>Maximal number of iterations allowedd */
        private int _maxIterations;

        /// <summary>Maximal number of evaluations allowedd */
        private int _maxEvaluations;

        /// <summary>Number of iterations already performed for all startsd */
        private int _totalIterations;

        /// <summary>Number of evaluations already performed for all startsd */
        private int _totalEvaluations;

        /// <summary>Number of starts to god */
        private int _starts;

        /// <summary>Random generator for multi-startd */
        private RandomGenerator _generator;

        /// <summary>Found optimad */
        private double[] _optima;

        /// <summary>Found function values at optimad */
        private double[] _optimaValues;

        /// <summary>
        /// Create a multi-start optimizer from a single-start optimizer
        /// </summary>
        /// <param Name="optimizer">single-start optimizer to wrap</param>
        /// <param Name="starts">number of starts to perform (including the</param>
        /// first one), multi-start is disabled if value is less than or
        /// equal to 1
        /// <param Name="generator">random generator to use for restarts</param>
        public MultiStartUnivariateRealOptimizer(IUnivariateRealOptimizer optimizer, int starts, RandomGenerator generator)
        {
            this._optimizer = optimizer;
            this._totalIterations = 0;
            this._starts = starts;
            this._generator = generator;
            this._optima = null;
            MaximalIterationCount = (Int32.MaxValue);
            MaxEvaluations = (Int32.MaxValue);
        }

        /// <summary>{@inheritDoc} */
        public double FunctionValue
        {
            get { return _optimaValues[0]; }
        }

        /// <summary>{@inheritDoc} */
        public double Result
        {
            get { return _optima[0]; }
        }

        /// <summary>{@inheritDoc} */
        public double AbsoluteAccuracy
        {
            get { return _optimizer.AbsoluteAccuracy; }
            set { _optimizer.AbsoluteAccuracy = value; }
        }

        /// <summary>{@inheritDoc} */
        public int IterationCount
        {
            get { return _totalIterations; }
        }

        /// <summary>{@inheritDoc} */
        public int MaximalIterationCount
        {
            get { return _maxIterations; }
            set { this._maxIterations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int MaxEvaluations
        {
            get { return _maxEvaluations; }
            set { _maxEvaluations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Evaluations
        {
            get { return _totalEvaluations; }
        }

        /// <summary>{@inheritDoc} */
        public double RelativeAccuracy
        {
            get { return _optimizer.RelativeAccuracy; }
            set { _optimizer.RelativeAccuracy = value; }
        }

        /// <summary>{@inheritDoc} */
        public void ResetAbsoluteAccuracy()
        {
            _optimizer.ResetAbsoluteAccuracy();
        }

        /// <summary>{@inheritDoc} */
        public void ResetMaximalIterationCount()
        {
            _optimizer.ResetMaximalIterationCount();
        }

        /// <summary>{@inheritDoc} */
        public void ResetRelativeAccuracy()
        {
            _optimizer.ResetRelativeAccuracy();
        }

        public void ResetIterationsCounter()
        {
            _totalIterations = 0;
        }

        /// <summary>Get all the optima found during the last call to {@link
        /// #Optimize(IUnivariateRealFunction, GoalType, double, double) optimize}.
        /// <p>The optimizer stores all the optima found during a set of
        /// restartsd The {@link #Optimize(IUnivariateRealFunction, GoalType,
        /// double, double) optimize} method returns the best point onlyd This
        /// method returns all the points found at the end of each starts,
        /// including the best one already returned by the {@link
        /// #Optimize(IUnivariateRealFunction, GoalType, double, double) optimize}
        /// method.
        /// </p>
        /// <p>
        /// The returned array as one element for each start as specified
        /// in the constructord It is ordered with the results from the
        /// runs that did converge first, sorted from best to worst
        /// objective value (i.e in ascending order if minimizing and in
        /// descending order if maximizing), followed by Double.NaN elements
        /// corresponding to the runs that did not converged This means all
        /// elements will be NaN if the {@link #Optimize(IUnivariateRealFunction,
        /// GoalType, double, double) optimize} method did throw a {@link
        /// NonConvergenceException NonConvergenceException})d This also means that
        /// if the first element is not NaN, it is the best point found across
        /// all starts.</p>
        /// </summary>
        /// <returns>array containing the optima</returns>
        /// <exception cref="InvalidOperationException">if {@link #Optimize(IUnivariateRealFunction, </exception>
        /// GoalType, double, double) optimize} has not been called
        /// <see cref="#getOptimaValues()"></see>
        public double[] GetOptima()
        {
            if (_optima == null)
            {
                throw new InvalidOperationException(LocalizedResources.Instance().NO_OPTIMUM_COMPUTED_YET);
            }
            return _optima.CloneExact();
        }

        /// <summary>Get all the function values at optima found during the last call to {@link
        /// #Optimize(IUnivariateRealFunction, GoalType, double, double) optimize}.
        /// <p>
        /// The returned array as one element for each start as specified
        /// in the constructord It is ordered with the results from the
        /// runs that did converge first, sorted from best to worst
        /// objective value (i.e in ascending order if minimizing and in
        /// descending order if maximizing), followed by Double.NaN elements
        /// corresponding to the runs that did not converged This means all
        /// elements will be NaN if the {@link #Optimize(IUnivariateRealFunction,
        /// GoalType, double, double) optimize} method did throw a {@link
        /// NonConvergenceException NonConvergenceException})d This also means that
        /// if the first element is not NaN, it is the best point found across
        /// all starts.</p>
        /// </summary>
        /// <returns>array containing the optima</returns>
        /// <exception cref="InvalidOperationException">if {@link #Optimize(IUnivariateRealFunction, </exception>
        /// GoalType, double, double) optimize} has not been called
        /// <see cref="#GetOptima()"></see>
        public double[] OptimaValues
        {
            get
            {
                if (_optimaValues == null)
                {
                    throw new InvalidOperationException(LocalizedResources.Instance().NO_OPTIMUM_COMPUTED_YET);
                }
                return _optimaValues.CloneExact();
            }
        }

        /// <summary>{@inheritDoc} */
        public double Optimize(IUnivariateRealFunction f, GoalType goalType,
                               double min, double max)
        {

            _optima = new double[_starts];
            _optimaValues = new double[_starts];
            _totalIterations = 0;
            _totalEvaluations = 0;

            // multi-start loop
            for (int i = 0; i < _starts; ++i)
            {

                try
                {
                    _optimizer.MaximalIterationCount = (_maxIterations - _totalIterations);
                    _optimizer.MaxEvaluations = (_maxEvaluations - _totalEvaluations);
                    double bound1 = (i == 0) ? min : min + _generator.NextDouble() * (max - min);
                    double bound2 = (i == 0) ? max : min + _generator.NextDouble() * (max - min);
                    _optima[i] = _optimizer.Optimize(f, goalType,
                                                         System.Math.Min(bound1, bound2),
                                                                     System.Math.Max(bound1, bound2));
                    _optimaValues[i] = _optimizer.FunctionValue;
                }
                catch (FunctionEvaluationException fee)
                {
                    _optima[i] = Double.NaN;
                    _optimaValues[i] = Double.NaN;
                }
                catch (ConvergenceException ce)
                {
                    _optima[i] = Double.NaN;
                    _optimaValues[i] = Double.NaN;
                }

                _totalIterations += _optimizer.IterationCount;
                _totalEvaluations += _optimizer.Evaluations;

            }

            // sort the optima from best to worst, followed by NaN elements
            int lastNaN = _optima.Length;
            for (int i = 0; i < lastNaN; ++i)
            {
                if (Double.IsNaN(_optima[i]))
                {
                    _optima[i] = _optima[--lastNaN];
                    _optima[lastNaN + 1] = Double.NaN;
                    _optimaValues[i] = _optimaValues[--lastNaN];
                    _optimaValues[lastNaN + 1] = Double.NaN;
                }
            }

            double currX = _optima[0];
            double currY = _optimaValues[0];
            for (int j = 1; j < lastNaN; ++j)
            {
                double prevY = currY;
                currX = _optima[j];
                currY = _optimaValues[j];
                if ((goalType == GoalType.MAXIMIZE) ^ (currY < prevY))
                {
                    // the current element should be inserted closer to the beginning
                    int i = j - 1;
                    double mIX = _optima[i];
                    double mIY = _optimaValues[i];
                    while ((i >= 0) && ((goalType == GoalType.MAXIMIZE) ^ (currY < mIY)))
                    {
                        _optima[i + 1] = mIX;
                        _optimaValues[i + 1] = mIY;
                        if (i-- != 0)
                        {
                            mIX = _optima[i];
                            mIY = _optimaValues[i];
                        }
                        else
                        {
                            mIX = Double.NaN;
                            mIY = Double.NaN;
                        }
                    }
                    _optima[i + 1] = currX;
                    _optimaValues[i + 1] = currY;
                    currX = _optima[j];
                    currY = _optimaValues[j];
                }
            }

            if (Double.IsNaN(_optima[0]))
            {
                throw new OptimizationException(LocalizedResources.Instance().NO_CONVERGENCE_WITH_ANY_START_POINT, _starts);
            }

            // return the found point given the best objective function value
            return _optima[0];

        }

        /// <summary>{@inheritDoc} */
        public double Optimize(IUnivariateRealFunction f, GoalType goalType, double min, double max, double startValue)
        {
            return Optimize(f, goalType, min, max);
        }
    }
}
