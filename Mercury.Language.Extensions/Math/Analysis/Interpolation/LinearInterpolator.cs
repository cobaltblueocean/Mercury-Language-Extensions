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
using Mercury.Language.Exceptions;
using Mercury.Language;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Analysis.Polynomial;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Implements a linear function for interpolation of real univariate functions.
    /// @version $Revision$ $Date$
    /// @since 2.2
    /// </summary>
    public class LinearInterpolator : IUnivariateRealInterpolator
    {
        /// <summary>
        /// Computes a linear interpolating function for the data set.
        /// </summary>
        /// <param Name="x">the arguments for the interpolation points</param>
        /// <param Name="y">the values for the interpolation points</param>
        /// <returns>a function which interpolates the data set</returns>
        /// <exception cref="DimensionMismatchException">if {@code x} and {@code y} </exception>
        /// have different sizes.
        /// <exception cref="Mercury.Language.Exceptions.NonMonotonousSequenceException"></exception>
        /// if {@code x} is not sorted in strict increasing order.
        /// <exception cref="NumberIsTooSmallException">if the size of {@code x} is smaller </exception>
        /// than 2d
        public IUnivariateRealFunction Interpolate(double[] x, double[] y)
        {
            if (x.Length != y.Length)
            {
                throw new DimensionMismatchException(x.Length, y.Length);
            }

            if (x.Length < 2)
            {
                throw new NumberIsTooSmallException(LocalizedResources.Instance().NUMBER_OF_POINTS, x.Length, 2, true);
            }

            // Number of intervalsd  The number of data points is n + 1d
            int n = x.Length - 1;

            QuickMath.CheckOrder(x);

            // Slope of the lines between the datapoints.
            double[] m = new double[n];
            AutoParallel.AutoParallelFor(0, n, (i) =>
            {
                m[i] = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
            });

            PolynomialFunction[] polynomials = new PolynomialFunction[n];
            double[] coefficients = new double[2];
            AutoParallel.AutoParallelFor(0, n, (i) =>
            {
                coefficients[0] = y[i];
                coefficients[1] = m[i];
                polynomials[i] = new PolynomialFunction(coefficients);
            });

            return new PolynomialSplineFunction(x, polynomials);
        }
    }
}
