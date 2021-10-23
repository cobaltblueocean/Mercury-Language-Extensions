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

namespace Mercury.Language.Math.Optimization
{
    /// <summary>
    /// This interface is mainly intended to enforce the internal coherence of Commons-Math. 
    /// Users of the API are advised to base their code on the following interfaces:
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public interface IBaseOptimizer<P>
    {
        /// <summary>
        /// Get the maximal number of function evaluations.
        /// </summary>
        /// <returns>the maximal number of function evaluations.</returns>
       int MaxEvaluations { get; set; }

        /// <summary>
        /// Get the number of evaluations of the objective function.
        /// The number of evaluations corresponds to the last call to the
        /// optimize method. It is 0 if the method has not been called yet.
        /// </summary>
        /// <returns>the number of evaluations of the objective function.</returns>
        int Evaluations { get; }

        /// <summary>
        /// Get the convergence checker.
        /// </summary>
        /// <returns>the object used to check for convergence.</returns>
        IConvergenceChecker<P> ConvergenceChecker { get; }
    }
}
