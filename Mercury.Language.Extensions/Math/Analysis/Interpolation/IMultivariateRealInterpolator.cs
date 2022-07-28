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
    /// @since 2.1
    /// @version $Revision: 924794 $ $Date: 2010-03-18 15:15:50 +0100 (jeud 18 mars 2010) $
    /// </summary>
    public interface IMultivariateRealInterpolator
    {

        /// <summary>
        /// Computes an interpolating function for the data set.
        /// 
        /// </summary>
        /// <param Name="xval">the arguments for the interpolation points.</param>
        /// {@code xval[i][0]} is the first component of interpolation point
        /// {@code i}, {@code xval[i][1]} is the second component, and so on
        /// until {@code xval[i][d-1]}, the last component of that interpolation
        /// point (where {@code d} is thus the dimension of the space).
        /// <param Name="yval">the values for the interpolation points</param>
        /// <returns>a function which interpolates the data set</returns>
        /// <exception cref="MathArithmeticException">if arguments violate assumptions made by the </exception>
        ///         interpolation algorithm or some dimension mismatch occurs
        /// <exception cref="ArgumentException">if there are no data (xval null or zero Length) </exception>
        IMultivariateRealFunction Interpolate(double[][] xval, double[] yval);
    }
}
