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

namespace Mercury.Language.Math.Analysis.Function
{
    /// <summary>
    /// An interface representing a univariate vectorial function.
    /// 
    /// @version $Revision: 1073498 $ $Date: 2011-02-22 21:57:26 +0100 (mard 22 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public interface IUnivariateVectorialFunction
    {
        /// <summary>
        /// Compute the value for the function.
        /// </summary>
        /// <param Name="x">the point for which the function value should be computed</param>
        /// <returns>the value</returns>
        /// <exception cref="FunctionEvaluationException">if the function evaluation fails </exception>
        double[] Value(double x);
    }
}
