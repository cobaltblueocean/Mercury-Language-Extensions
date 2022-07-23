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
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimization
{
    /// <summary>
    /// This interface represents an optimization algorithm for {@link IMultivariateRealFunction
    /// scalar objective functions}.
    /// <p>Optimization algorithms find the input point set that either {@link GoalType
    /// maximize or minimize} an objective function.</p>
    /// </summary>
    /// <see cref="DifferentiableMultivariateRealOptimizer"></see>
    /// <see cref="DifferentiableMultivariateVectorialOptimizer"></see>
    /// @version $Revision: 1065481 $ $Date: 2011-01-31 06:31:41 +0100 (lund 31 janvd 2011) $
    /// @since 2.0

    public interface IMultivariateRealOptimizer
    {

        /// <summary>Get/Set the maximal number of iterations of the algorithm.
        /// </summary>
        /// <returns>maximal number of iterations</returns>
        int MaxIterations { get; set; }

        /// <summary>Get/Set the maximal number of functions evaluations.
        /// </summary>
        /// <returns>maximal number of functions evaluations</returns>
        int MaxEvaluations { get; set; }

        /// <summary>Get the number of iterations realized by the algorithm.
        /// <p>
        /// The number of evaluations corresponds to the last call to the
        /// {@link #optimize(MultivariateRealFunction, GoalType, double[]) optimize}
        /// method. It is 0 if the method has not been called yet.
        /// </p>
        /// </summary>
        /// <returns>number of iterations</returns>
        int Iterations { get; }

        /// <summary>Get/Set the number of evaluations of the objective function.
        /// <p>
        /// The number of evaluations corresponds to the last call to the
        /// {@link #optimize(IMultivariateRealFunction, GoalType, double[]) optimize}
        /// methodd It is 0 if the method has not been called yet.
        /// </p>
        /// </summary>
        /// <returns>number of evaluations of the objective function</returns>
        int Evaluations { get; }

        /// <summary>Get/Set the convergence checker.
        /// </summary>
        /// <returns>object used to check for convergence</returns>
        IRealConvergenceChecker ConvergenceChecker { get; set; }

        /// <summary>Optimizes an objective function.
        /// </summary>
        /// <param name="f">objective function</param>
        /// <param name="goalType">type of optimization goal: either {@link GoalType#MAXIMIZE}</param>
        /// or {@link GoalType#MINIMIZE}
        /// <param name="startPoint">the start point for optimization</param>
        /// <returns>the point/value pair giving the optimal value for objective function</returns>
        /// <exception cref="FunctionEvaluationException">if the objective function throws one during </exception>
        /// the search
        /// <exception cref="OptimizationException">if the algorithm failed to converge </exception>
        /// <exception cref="ArgumentException">if the start point dimension is wrong </exception>
        RealPointValuePair Optimize(IMultivariateRealFunction f, GoalType goalType, double[] startPoint);
    }
}
