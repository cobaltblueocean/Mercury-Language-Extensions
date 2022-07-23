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
    /// Interface AutoParallel.AutoParallelForEach(univariate real) optimization algorithms.
    /// 
    /// @version $Revision: 1073658 $ $Date: 2011-02-23 10:45:42 +0100 (merd 23 févrd 2011) $
    /// @since 2.0
    /// </summary>

    public interface IUnivariateRealOptimizer
    {
        /// <summary>
        /// Get/set the upper limit for the number of iterations.
        /// <p>
        /// Usually a high iteration count indicates convergence problems. However,
        /// the "reasonable value" varies widely for different algorithms. Users are
        /// advised to use the default value supplied by the algorithm.</p>
        /// <p>
        /// A {@link ConvergenceException} will be thrown if this number
        /// is exceeded.</p>
        /// </summary>
        /// <returns>the actual upper limit</returns>
        int MaximalIterationCount { get; set; }

        /// <summary>
        /// Get/set the actual absolute accuracy.
        /// <p>
        /// The default is usually chosen so that results in the interval
        /// -10..-0.1 and +0.1..+10 can be found with a reasonable accuracy. If the
        /// expected absolute value of your results is of much smaller magnitude, set
        /// this to a smaller value.</p>
        /// <p>
        /// Algorithms are advised to do a plausibility check with the relative
        /// accuracy, but clients should not rely on this.</p>
        /// </summary>
        /// <returns>the accuracy</returns>
        double AbsoluteAccuracy { get; set; }

        /// <summary>
        /// Get/set the actual relative accuracy.
        /// <p>
        /// This is used to stop iterations if the absolute accuracy can't be
        /// achieved due to large values or short mantissa length.</p>
        /// <p>
        /// If this should be the primary criterion for convergence rather then a
        /// safety measure, set the absolute accuracy to a ridiculously small value,
        /// like {@link org.apache.commons.math.util.MathUtils#SAFE_MIN MathUtils.SAFE_MIN}.</p>
        /// </summary>
        /// <returns>the accuracy</returns>
        double RelativeAccuracy { get; set; }

        /// <summary>
        /// Get the number of iterations in the last run of the algorithm.
        /// <p>
        /// This is mainly meant for testing purposes. It may occasionally
        /// help track down performance problems: if the iteration count
        /// is notoriously high, check whether the problem is evaluated
        /// properly, and whether another algorithm is more amenable to the
        /// problem.</p>
        /// 
        /// </summary>
        /// <returns>the last iteration count.</returns>
        /// <exception cref="IllegalStateException">if there is no result available, either </exception>
        /// because no result was yet computed or the last attempt failed.
        int IterationCount { get; }

        /// <summary>Get/set the maximal number of functions evaluations.
        /// </summary>
        /// <returns>the maximal number of functions evaluations.</returns>
        int MaxEvaluations { get; set; }

        /// <summary>Get the number of evaluations of the objective function.
        /// <p>
        /// The number of evaluations corresponds to the last call to the
        /// {@link #Optimize(UnivariateRealFunction, GoalType, double, double) optimize}
        /// methodd It is 0 if the method has not been called yet.
        /// </p>
        /// </summary>
        /// <returns>the number of evaluations of the objective function.</returns>
        int Evaluations { get; }

        /// <summary>
        /// Get the result of the last run of the optimizer.
        /// 
        /// </summary>
        /// <returns>the optimum.</returns>
        /// <exception cref="InvalidOperationException">if there is no result available, either </exception>
        /// because no result was yet computed or the last attempt failed.
        double Result { get; }

        /// <summary>
        /// Get the result of the last run of the optimizer.
        /// 
        /// </summary>
        /// <returns>the value of the function at the optimum.</returns>
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the functiond </exception>
        /// <exception cref="InvalidOperationException">if there is no result available, either </exception>
        /// because no result was yet computed or the last attempt failed.
        double FunctionValue { get; }

        /// <summary>
        /// Reset the upper limit for the number of iterations to the default.
        /// <p>
        /// The default value is supplied by the algorithm implementation.</p>
        /// 
        /// </summary>
        /// <see cref="#setMaximalIterationCount(int)"></see>
        void ResetMaximalIterationCount();

        /// <summary>
        /// Reset the relative accuracy to the default.
        /// The default value is provided by the algorithm implementation.
        /// </summary>
        void ResetRelativeAccuracy();

        /// <summary>
        /// Reset the absolute accuracy to the default.
        /// <p>
        /// The default value is provided by the algorithm implementation.</p>
        /// </summary>
        void ResetAbsoluteAccuracy();

        /// <summary>
        /// Reset the iterations counter to 0.
        /// </summary>
        void ResetIterationsCounter();

        /// <summary>
        /// Find an optimum in the given interval.
        /// <p>
        /// An optimizer may require that the interval brackets a single optimum.
        /// </p>
        /// </summary>
        /// <param Name="f">the function to optimize.</param>
        /// <param Name="goalType">type of optimization goal: either {@link GoalType#MAXIMIZE}</param>
        /// or {@link GoalType#MINIMIZE}.
        /// <param Name="min">the lower bound for the interval.</param>
        /// <param Name="max">the upper bound for the interval.</param>
        /// <returns>a value where the function is optimum.</returns>
        /// <exception cref="NonConvergenceException">if the maximum iteration count is exceeded </exception>
        /// or the optimizer detects convergence problems otherwise.
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the functiond </exception>
        /// <exception cref="ArgumentException">if min > max or the endpoints do not </exception>
        /// satisfy the requirements specified by the optimizer.
        double Optimize(IUnivariateRealFunction f, GoalType goalType, double min, double max);

        /// <summary>
        /// Find an optimum in the given interval, start at startValue.
        /// <p>
        /// An optimizer may require that the interval brackets a single optimum.
        /// </p>
        /// </summary>
        /// <param Name="f">the function to optimize.</param>
        /// <param Name="goalType">type of optimization goal: either {@link GoalType#MAXIMIZE}</param>
        /// or {@link GoalType#MINIMIZE}.
        /// <param Name="min">the lower bound for the interval.</param>
        /// <param Name="max">the upper bound for the interval.</param>
        /// <param Name="startValue">the start value to use.</param>
        /// <returns>a value where the function is optimum.</returns>
        /// <exception cref="NonConvergenceException">if the maximum iteration count is exceeded </exception>
        /// or the optimizer detects convergence problems otherwise.
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the functiond </exception>
        /// <exception cref="ArgumentException">if min > max or the arguments do not </exception>
        /// satisfy the requirements specified by the optimizer.
        /// <exception cref="InvalidOperationException">if there are no datad </exception>
        double Optimize(IUnivariateRealFunction f, GoalType goalType, double min, double max, double startValue);
    }
}
