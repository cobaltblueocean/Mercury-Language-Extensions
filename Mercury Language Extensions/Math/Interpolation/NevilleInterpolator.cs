// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Interpolation Inc.
//
// Copyright (C) 2012 - present by System.Math.Interpolation Inc. and the System.Math.Interpolation group of companies
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
using Mercury.Language.Math.Polynomial;

namespace Mercury.Language.Math.Interpolation
{
    /// <summary>
    /// mplements the <a href="http://mathworld.wolfram.com/NevillesAlgorithm.html">
    /// Neville's Algorithm</a> for interpolation of real univariate functions. For
    /// reference, see <b>Introduction to Numerical Analysis</b>, ISBN 038795452X,
    /// chapter 2.
    /// <p>
    /// The actual code of Neville's algorithm is in PolynomialFunctionLagrangeForm,
    /// this class provides an easy-to-use interface to it.</p>
    /// </summary>
    public class NevilleInterpolator
    {

        #region Local Variables

        #endregion

        #region Property

        #endregion

        #region Constructor

        #endregion

        #region Implement Methods

        #endregion

        #region Local Public Methods

        /// <summary>
        /// Computes an interpolating function for the data set.
        /// </summary>
        /// <param name="x">Interpolating points.</param>
        /// <param name="y">Interpolating values.</param>
        /// <returns>a function which interpolates the data set</returns>
        public PolynomialFunctionLagrangeForm Interpolate(double[] x, double[] y)
        {
            return new PolynomialFunctionLagrangeForm(x, y);
        }
        #endregion

        #region Local Private Methods

        #endregion
    }
}
