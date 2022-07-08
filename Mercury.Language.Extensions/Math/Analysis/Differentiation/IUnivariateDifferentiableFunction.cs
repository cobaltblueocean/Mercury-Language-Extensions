// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Analysis.Differentiation
{
    /// <summary>
    /// Interface for univariate functions derivatives.
    /// <p>This interface represents a simple function which computes
    /// both the value and the first derivative of a mathematical function.
    /// The derivative is computed with respect to the input variable.</p>
    /// </summary>
    public interface IUnivariateDifferentiableFunction: IUnivariateRealFunction
    {
        /// <summary>
        /// Simple mathematical function.
        /// <p><see cref="IUnivariateDifferentiableFunction"/> classes compute both the
        /// value and the first derivative of the function.</p>
        /// </summary>
        /// <param name="t">function input value</param>
        /// <returns>function result</returns>
        /// <exception cref="DimensionMismatchException">if t is inconsistent with the</exception>
        DerivativeStructure Value(DerivativeStructure t);
    }
}
