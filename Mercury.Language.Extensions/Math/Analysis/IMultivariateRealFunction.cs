// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Interface Inc.
//
// Copyright (C) 2012 - present by System.Math.Interface Inc. and the System.Math.Interface group of companies
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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Analysis
{
    /// <summary>
    /// An interface representing a multivariate real function.
    /// </summary>
    public interface IMultivariateRealFunction
    {

        /// <summary>
        /// Get the argument value.
        /// Since C# doesn't have a feature to retrieve the argument values from Func<>, need to implement to keep the argument.
        /// </summary>
        double[] ParamValue { get; }

        /// <summary>
        /// Compute the value for the function at the given point.
        /// </summary>
        /// <param name="point">point at which the function must be evaluated</param>
        /// <returns>function value for the given point</returns>
        /// <exception cref="FunctionEvaluationException"></exception>
        double Value(double[] point);
    }
}
