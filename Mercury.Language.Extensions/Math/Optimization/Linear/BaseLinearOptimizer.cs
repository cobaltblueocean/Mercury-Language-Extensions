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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Math;
using Mercury.Language.Math.Optimization;
using Mercury.Language.Exception;


namespace Mercury.Language.Math.Optimization.Linear
{
    /// <summary>
    /// Base class for implementing linear optimizers.
    /// <p>This base class handles the boilerplate methods associated to thresholds
    /// settings and iterations counters.</p>
    /// @version $Revision: 925812 $ $Date: 2010-03-21 16:49:31 +0100 (dimd 21 mars 2010) $
    /// @since 2.0
    /// 
    /// </summary>
    public abstract class BaseLinearOptimizer : ILinearOptimizer
    {

        /// <summary>Default maximal number of iterations allowedd */
        public static int DEFAULT_MAX_ITERATIONS = 100;

        /// <summary>
        /// Linear objective function.
        /// @since 2.1
        /// </summary>
        protected LinearObjectiveFunction function;

        /// <summary>
        /// Linear constraints.
        /// @since 2.1
        /// </summary>
        protected ICollection<LinearConstraint> linearConstraints;

        /// <summary>
        /// Type of optimization goal: either {@link GoalType#MAXIMIZE} or {@link GoalType#MINIMIZE}.
        /// @since 2.1
        /// </summary>
        protected GoalType goal;

        /// <summary>
        /// Whether to restrict the variables to non-negative values.
        /// @since 2.1
        /// </summary>
        protected Boolean nonNegative;

        /// <summary>Maximal number of iterations allowedd */
        private int maxIterations;

        /// <summary>Number of iterations already performedd */
        private int iterations;

        /// <summary>Simple constructor with default settings.
        /// <p>The maximal number of evaluation is set to its default value.</p>
        /// </summary>
        protected BaseLinearOptimizer()
        {
            MaxIterations = (DEFAULT_MAX_ITERATIONS);
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

        /// <summary>{@inheritDoc} */
        public RealPointValuePair Optimize(LinearObjectiveFunction f, ICollection<LinearConstraint> constraints, GoalType goalType, Boolean restrictToNonNegative)

        {

            // store linear problem characteristics
            this.function = f;
            this.linearConstraints = constraints;
            this.goal = goalType;
            this.nonNegative = restrictToNonNegative;

            iterations = 0;

            // solve the problem
            return doOptimize();

        }

        /// <summary>Perform the bulk of optimization algorithm.
        /// </summary>
        /// <returns>the point/value pair giving the optimal value for objective function</returns>
        /// <exception cref="OptimizationException">if no solution fulfilling the constraints </exception>
        /// can be found in the allowed number of iterations
        protected abstract RealPointValuePair doOptimize();
    }
}
