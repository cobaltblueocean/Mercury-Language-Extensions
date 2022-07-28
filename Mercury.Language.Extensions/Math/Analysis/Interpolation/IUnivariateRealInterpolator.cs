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

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Interface representing a univariate real interpolating function.
    /// 
    /// @version $Revision: 821626 $ $Date: 2009-10-04 23:57:30 +0200 (dimd 04 octd 2009) $
    /// </summary>

    public interface IUnivariateRealInterpolator
    {

        /// <summary>
        /// Computes an interpolating function for the data set.
        /// </summary>
        /// <param Name="xval">the arguments for the interpolation points</param>
        /// <param Name="yval">the values for the interpolation points</param>
        /// <returns>a function which interpolates the data set</returns>
        /// <exception cref="MathArithmeticException">if arguments violate assumptions made by the </exception>
        ///         interpolation algorithm
        IUnivariateRealFunction Interpolate(double[] xval, double[] yval);
    }
}
