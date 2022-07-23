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
using Mercury.Language.Math.Optimization;


namespace Mercury.Language.Math.Optimization.Linear
{

    /// <summary>
    /// This interface represents an optimization algorithm for linear problems.
    /// <p>Optimization algorithms find the input point set that either {@link GoalType
    /// maximize or minimize} an objective functiond In the linear case the form of
    /// the function is restricted to
    /// <pre>
    /// c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> = v
    /// </pre>
    /// and there may be linear constraints too, of one of the forms:
    /// <ul>
    ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> = v</li>
    ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> &lt;= v</li>
    ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> >= v</li>
    ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> =
    ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
    ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> &lt;=
    ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
    ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> >=
    ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
    /// </ul>
    /// where the c<sub>i</sub>, l<sub>i</sub> or r<sub>i</sub> are the coefficients of
    /// the constraints, the x<sub>i</sub> are the coordinates of the current point and
    /// v is the value of the constraint.
    /// </p>
    /// @version $Revision: 811685 $ $Date: 2009-09-05 19:36:48 +0200 (samd 05 septd 2009) $
    /// @since 2.0
    /// </summary>

    public interface ILinearOptimizer
    {

        /// <summary>Get/set the maximal number of iterations of the algorithm.
        /// </summary>
        /// <returns>maximal number of iterations</returns>
        int MaxIterations { get; set; }

        /// <summary>Get the number of iterations realized by the algorithm.
        /// <p>
        /// The number of evaluations corresponds to the last call to the
        /// {@link #Optimize(LinearObjectiveFunction, Collection, GoalType, Boolean) optimize}
        /// methodd It is 0 if the method has not been called yet.
        /// </p>
        /// </summary>
        /// <returns>number of iterations</returns>
        int Iterations { get; }

        /// <summary>Optimizes an objective function.
        /// </summary>
        /// <param Name="f">linear objective function</param>
        /// <param Name="constraints">linear constraints</param>
        /// <param Name="goalType">type of optimization goal: either {@link GoalType#MAXIMIZE}</param>
        /// or {@link GoalType#MINIMIZE}
        /// <param Name="restrictToNonNegative">whether to restrict the variables to non-negative values</param>
        /// <returns>point/value pair giving the optimal value for objective function</returns>
        /// <exception cref="OptimizationException">if no solution fulfilling the constraints </exception>
        /// can be found in the allowed number of iterations
        RealPointValuePair Optimize(LinearObjectiveFunction f, ICollection<LinearConstraint> constraints, GoalType goalType, Boolean restrictToNonNegative);
    }
}
