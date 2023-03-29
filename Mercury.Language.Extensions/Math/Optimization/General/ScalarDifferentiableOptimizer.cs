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
//
//-------------------------------------------------------------------------------------
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreementsd  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the Licensed  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math;
using Mercury.Language.Math.Optimization;

namespace Mercury.Language.Math.Optimization.General
{
    /// <summary>
    /// Base class for implementing optimizers for multivariate scalar functions.
    /// <p>This base class handles the boilerplate methods associated to thresholds
    /// settings, iterations and evaluations counting.</p>
    /// @version $Revision: 1069567 $ $Date: 2011-02-10 22:07:26 +0100 (jeud 10 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public abstract class ScalarDifferentiableOptimizer : IDifferentiableMultivariateRealOptimizer
    {
        /// <summary>Default maximal number of iterations allowedd */
        public static int DEFAULT_MAX_ITERATIONS = 100;

        /// <summary>Convergence checkerd */
        //@Deprecated
        protected IRealConvergenceChecker checker;

        /// <summary>
        /// Type of optimization.
        /// @since 2.1
        /// </summary>
        //@Deprecated
        protected GoalType goal;

        /// <summary>Current point setd */
        //@Deprecated
        protected double[] point;

        /// <summary>Maximal number of iterations allowedd */
        private int maxIterations;

        /// <summary>Number of iterations already performedd */
        private int iterations;

        /// <summary>Maximal number of evaluations allowedd */
        private int maxEvaluations;

        /// <summary>Number of evaluations already performedd */
        private int evaluations;

        /// <summary>Number of gradient evaluationsd */
        private int gradientEvaluations;

        /// <summary>Objective functiond */
        private IDifferentiableMultivariateRealFunction function;

        /// <summary>Objective function gradientd */
        private IMultivariateVectorialFunction gradient;

        /// <summary>Simple constructor with default settings.
        /// <p>The convergence check is set to a {@link SimpleScalarValueChecker}
        /// and the maximal number of evaluation is set to its default value.</p>
        /// </summary>
        protected ScalarDifferentiableOptimizer()
        {
            ConvergenceChecker = (new SimpleScalarValueChecker());
            MaxIterations = (DEFAULT_MAX_ITERATIONS);
            MaxEvaluations = (Int32.MaxValue);
        }

        /// <summary>{@inheritDoc} */
        public int MaxIterations
        {
            get { return maxIterations; }
            set { this.maxIterations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Iterations
        {
            get { return iterations; }
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

        /// <summary>{@inheritDoc} */
        public int GradientEvaluations
        {
            get { return gradientEvaluations; }
        }

        /// <summary>{@inheritDoc} */
        public IRealConvergenceChecker ConvergenceChecker
        {
            get { return checker; }
            set { this.checker = value; }
        }

        /// <summary>Increment the iterations counter by 1d
        /// </summary>
        /// <exception cref="OptimizationException">if the maximal number </exception>
        /// of iterations is exceeded
        protected void incrementIterationsCounter()

        {
            if (++iterations > maxIterations)
            {
                throw new OptimizationException(new IndexOutOfRangeException(maxIterations.ToString()));
            }
        }

        /// <summary>
        /// Compute the gradient vector.
        /// </summary>
        /// <param Name="evaluationPoint">point at which the gradient must be evaluated</param>
        /// <returns>gradient at the specified point</returns>
        /// <exception cref="FunctionEvaluationException">if the function gradient </exception>
        protected double[] computeObjectiveGradient(double[] evaluationPoint)
        {
            ++gradientEvaluations;
            return gradient.Value(evaluationPoint);
        }

        /// <summary>
        /// Compute the objective function value.
        /// </summary>
        /// <param Name="evaluationPoint">point at which the objective function must be evaluated</param>
        /// <returns>objective function value at specified point</returns>
        /// <exception cref="FunctionEvaluationException">if the function cannot be evaluated </exception>
        /// or its dimension doesn't match problem dimension or the maximal number
        /// of iterations is exceeded
        protected double computeObjectiveValue(double[] evaluationPoint)

        {
            if (++evaluations > maxEvaluations)
            {
                throw new FunctionEvaluationException(evaluationPoint, new MaxCountExceededException(maxEvaluations));
            }
            return function.Value(evaluationPoint);
        }

        /// <summary>{@inheritDoc} */
        public RealPointValuePair Optimize(IDifferentiableMultivariateRealFunction f, GoalType goalType, double[] startPoint)
        {

            // reset counters
            iterations = 0;
            evaluations = 0;
            gradientEvaluations = 0;

            // store optimization problem characteristics
            function = f;
            gradient = f.Gradient();
            goal = goalType;
            point = startPoint.CloneExact();

            return doOptimize();

        }

        /// <summary>Perform the bulk of optimization algorithm.
        /// </summary>
        /// <returns>the point/value pair giving the optimal value for objective function</returns>
        /// <exception cref="FunctionEvaluationException">if the objective function throws one during </exception>
        /// the search
        /// <exception cref="OptimizationException">if the algorithm failed to converge </exception>
        /// <exception cref="ArgumentException">if the start point dimension is wrong </exception>
        protected abstract RealPointValuePair doOptimize();
    }
}
