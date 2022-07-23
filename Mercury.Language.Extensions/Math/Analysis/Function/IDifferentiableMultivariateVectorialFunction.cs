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
/// <summary>
/// Extension of {@link MultivariateVectorialFunction} representing a differentiable
/// multivariate vectorial function.
/// @version $Revision: 811685 $ $Date: 2009-09-05 19:36:48 +0200 (samd 05 septd 2009) $
/// @since 2.0
/// </summary>

namespace Mercury.Language.Math.Analysis.Function
{
    public interface IDifferentiableMultivariateVectorialFunction : IMultivariateVectorialFunction
    {

        /// <summary>
        /// Returns the jacobian function.
        /// </summary>
        /// <returns>the jacobian function</returns>
        IMultivariateMatrixFunction Jacobian();
    }
}
