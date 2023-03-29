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

/// <summary>
/// Licensed to the Apache Software Foundation (ASF) under one or more
/// contributor license agreementsd  See the NOTICE file distributed with
/// this work for additional information regarding copyright ownership.
/// The ASF licenses this file to You under the Apache License, Version 2.0
/// (the "License"); you may not use this file except in compliance with
/// the Licensed  You may obtain a copy of the License at
/// 
///      http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;

namespace Mercury.Language.Math.Optimization.Univariate
{
    /// <summary>
    /// Provide a default implementation for several functions useful to generic
    /// optimizers.
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public abstract class UnivariateRealOptimizer : IUnivariateRealOptimizer
    {
        /// <summary>Indicates where a root has been computedd */
        protected Boolean resultComputed;
        /// <summary>The last computed rootd */
        protected double result;
        /// <summary>Value of the function at the last computed resultd */
        protected double functionValue;
        /// <summary>Maximal number of evaluations allowedd */
        private int maxEvaluations;
        /// <summary>Number of evaluations already performedd */
        private int evaluations;
        /// <summary>Optimization type */
        private GoalType optimizationGoal;
        /// <summary>Lower end of search intervald */
        private double searchMin;
        /// <summary>Higher end of search intervald */
        private double searchMax;
        /// <summary>Initial guess d */
        private double searchStart;
        /// <summary>Function to optimized */
        private IUnivariateRealFunction function;
        private int maximalIterationCount;
        private double absoluteAccuracy;
        private double relativeAccuracy;
        private int iterationCount;
        private const double defaultAbsoluteAccuracy = 1.0e-14;
        private const double defaultRelativeAccuracy = 1e-9;

        /// <summary>
        /// Default constructor.
        /// To be removed once the single non-default one has been removed.
        /// </summary>
        protected UnivariateRealOptimizer() { }


        /// <summary>{@inheritDoc} */
        public double Result
        {
            get
            {
                if (!resultComputed)
                {
                    throw new DataNotFoundException(LocalizedResources.Instance().NO_DATA);
                }
                return result;
            }
        }

        /// <summary>{@inheritDoc} */
        public double FunctionValue
        {
            get
            {
                if (Double.IsNaN(functionValue))
                {
                    double opt = Result;
                    functionValue = function.Value(opt);
                }
                return functionValue;
            }
            set { functionValue = value; }
        }

        /// <summary>{@inheritDoc} */
        public int MaxEvaluations
        {
            get { return maxEvaluations; }
            set { this.maxEvaluations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Evaluations
        {
            get { return evaluations; }
        }

        /// <summary>
        /// </summary>
        /// <returns>the optimization type.</returns>
        public GoalType GoalType
        {
            get { return optimizationGoal; }
        }
        /// <summary>
        /// </summary>
        /// <returns>the lower of the search interval.</returns>
        public double Min
        {
            get { return searchMin; }
        }
        /// <summary>
        /// </summary>
        /// <returns>the higher of the search interval.</returns>
        public double Max
        {
            get { return searchMax; }
        }
        /// <summary>
        /// </summary>
        /// <returns>the initial guess.</returns>
        public double StartValue
        {
            get { return searchStart; }
        }

        public int MaximalIterationCount
        {
            get { return maximalIterationCount; }
            set { maximalIterationCount = value; }
        }
        public double AbsoluteAccuracy
        {
            get { return absoluteAccuracy; }
            set { absoluteAccuracy = value; }
        }
        public double RelativeAccuracy
        {
            get { return relativeAccuracy; }
            set { relativeAccuracy = value; }
        }

        public int IterationCount
        {
            get { return iterationCount; }
        }

        /// <summary>
        /// Compute the objective function value.
        /// 
        /// </summary>
        /// <param Name="point">Point at which the objective function must be evaluated.</param>
        /// <returns>the objective function value at specified point.</returns>
        /// <exception cref="FunctionEvaluationException">if the function cannot be evaluated </exception>
        /// or the maximal number of iterations is exceeded.
        protected double computeObjectiveValue(double point)

        {
            if (++evaluations > maxEvaluations)
            {
                resultComputed = false;
                throw new FunctionEvaluationException(point, new MaxCountExceededException(maxEvaluations));
            }
            return function.Value(point);
        }

        /// <summary>{@inheritDoc} */
        public double Optimize(IUnivariateRealFunction f, GoalType goal, double min, double max, double startValue)
        {
            // Initialize.
            this.searchMin = min;
            this.searchMax = max;
            this.searchStart = startValue;
            this.optimizationGoal = goal;
            this.function = f;

            // Reset.
            functionValue = Double.NaN;
            evaluations = 0;
            ResetIterationsCounter();

            // Perform computation.
            result = doOptimize();
            resultComputed = true;

            return result;
        }

        /// <summary>
        /// Reset the iterations counter to 0.
        /// </summary>
        public void ResetIterationsCounter()
        {
            iterationCount = 0;
        }


        /// <summary>{@inheritDoc} */
        public double Optimize(IUnivariateRealFunction f, GoalType goal, double min, double max)
        {
            return Optimize(f, goal, min, max, min + 0.5 * (max - min));
        }


        /// <summary>
        /// Method for implementing actual optimization algorithms in derived
        /// classes.
        /// 
        /// From version 3.0 onwards, this method will be abstract - i.e.
        /// concrete implementations will have to implement itd  If this method
        /// is not implemented, subclasses must override
        /// {@link #Optimize(IUnivariateRealFunction, GoalType, double, double)}.
        /// 
        /// </summary>
        /// <returns>the optimum.</returns>
        /// <exception cref="IndexOutOfRangeException">if the maximum iteration count </exception>
        /// is exceeded.
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating </exception>
        /// the function.
        protected abstract double doOptimize();

        public void ResetMaximalIterationCount()
        {
            maximalIterationCount = Int32.MaxValue;
        }

        public void ResetRelativeAccuracy()
        {
            relativeAccuracy = defaultRelativeAccuracy;
        }

        public void ResetAbsoluteAccuracy()
        {
            absoluteAccuracy = defaultAbsoluteAccuracy;
        }

        /// <summary>
        /// Increment the iterations counter by 1.
        /// 
        /// </summary>
        /// <exception cref="MaxIterationsExceededException">if the maximal number </exception>
        /// of iterations is exceeded.
        /// @since 2.2
        protected void incrementIterationsCounter()

        {
            if (++iterationCount > maximalIterationCount)
            {
                throw new MaxCountExceededException(maximalIterationCount);
            }
        }
    }
}
