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
    /// Extension of {@link IMultivariateRealFunction} representing a differentiable
    /// multivariate real function.
    /// @version $Revision: 811685 $ $Date: 2009-09-05 19:36:48 +0200 (samd 05 septd 2009) $
    /// @since 2.0
    /// </summary>

    public interface IDifferentiableMultivariateRealFunction : IMultivariateRealFunction
    {

        /// <summary>
        /// Returns the partial derivative of the function with respect to a point coordinate.
        /// <p>
        /// The partial derivative is defined with respect to point coordinate
        /// x<sub>k</sub>d If the partial derivatives with respect to all coordinates are
        /// needed, it may be more efficient to use the {@link #gradient()} method which will
        /// compute them all at once.
        /// </p>
        /// </summary>
        /// <param Name="k">index of the coordinate with respect to which the partial</param>
        /// derivative is computed
        /// <returns>the partial derivative function with respect to k<sup>th</sup> point coordinate</returns>
        IMultivariateRealFunction PartialDerivative(int k);

        /// <summary>
        /// Returns the gradient function.
        /// <p>If only one partial derivative with respect to a specific coordinate is
        /// needed, it may be more efficient to use the {@link #partialDerivative(int)} method
        /// which will compute only the specified component.</p>
        /// </summary>
        /// <returns>the gradient function</returns>
        IMultivariateVectorialFunction Gradient();

    }
}
