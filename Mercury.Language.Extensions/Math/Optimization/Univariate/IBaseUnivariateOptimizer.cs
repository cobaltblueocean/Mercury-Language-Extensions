// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization.Univariate Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization.Univariate Inc. and the System.Math.Optimization.Univariate group of companies
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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimization.Univariate
{
    /// <summary>
    /// This interface is mainly intended to enforce the internal coherence of
    /// Commons-Math. Users of the API are advised to base their code on
    /// the following interfaces: <see cref="IUnivariateOptimizer"/>
    /// </summary>
    /// <typeparam name="FUNC">Type of the objective function to be optimized.</typeparam>
    public interface IBaseUnivariateOptimizer<FUNC> : IBaseOptimizer<UnivariatePointValuePair> where FUNC : IUnivariateRealFunction
    {
        /// <summary>
        /// Find an optimum in the given interval.
        /// An optimizer may require that the interval brackets a single optimum.
        /// </summary>
        /// <param name="maxEval">Maximum number of function evaluations.</param>
        /// <param name="f">Function to optimize.</param>
        /// <param name="goalType">Type of optimization goal: either <see cref="GoalType.MAXIMIZE"/> or <see cref="GoalType.MINIMIZE"/>.</param>
        /// <param name="min">Lower bound for the interval.</param>
        /// <param name="max">Upper bound for the interval.</param>
        /// <returns>a (point, value) pair where the function is optimum.</returns>
        UnivariatePointValuePair Optimize(int maxEval, FUNC f, GoalType goalType, double min, double max);

        /// <summary>
        /// Find an optimum in the given interval, start at startValue.
        /// An optimizer may require that the interval brackets a single optimum.
        /// </summary>
        /// <param name="maxEval">Maximum number of function evaluations.</param>
        /// <param name="f">Function to optimize.</param>
        /// <param name="goalType">Type of optimization goal: either <see cref="GoalType.MAXIMIZE"/> or <see cref="GoalType.MINIMIZE"/>.</param>
        /// <param name="min">Lower bound for the interval.</param>
        /// <param name="max">Upper bound for the interval.</param>
        /// <param name="startValue"></param>
        /// <returns>Start value to use.</returns>
        UnivariatePointValuePair Optimize(int maxEval, FUNC f, GoalType goalType, double min, double max, double startValue);
    }
}
