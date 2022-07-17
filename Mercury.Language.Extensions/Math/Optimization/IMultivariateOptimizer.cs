// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization Inc. and the System.Math.Optimization group of companies
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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimization
{
    /// <summary>
    /// This interface represents an optimization algorithm for <see cref="IMultivariateRealFunction">scalar objective functions</see>.
    /// <p>Optimization algorithms find the input point set that either <see cref="GoalType.MAXIMIZE"/> or <see cref="GoalType.MINIMIZE"/> an objective function.</p>
    /// </summary>
    public interface IMultivariateOptimizer: IBaseMultivariateOptimizer<IMultivariateRealFunction>
    {
    }
}
