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
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Analysis.Polynomial;
using Mercury.Language.Exceptions;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Implements the <a href="
    /// "http://mathworld.wolfram.com/NewtonsDividedDifferenceInterpolationFormula.html">
    /// Divided Difference Algorithm</a> for interpolation of real univariate
    /// functionsd For reference, see <b>Introduction to Numerical Analysis</b>,
    /// ISBN 038795452X, chapter 2d
    /// <p>
    /// The actual code of Neville's evaluation is in PolynomialFunctionLagrangeForm,
    /// this class provides an easy-to-use interface to it.</p>
    /// 
    /// @version $Revision: 825919 $ $Date: 2009-10-16 16:51:55 +0200 (vend 16 octd 2009) $
    /// @since 1.2
    /// </summary>
    [Serializable]
    public class DividedDifferenceInterpolator : IUnivariateRealInterpolator
    {

        /// <summary>
        /// Computes an interpolating function for the data set.
        /// 
        /// </summary>
        /// <param Name="x">the interpolating points array</param>
        /// <param Name="y">the interpolating values array</param>
        /// <returns>a function which interpolates the data set</returns>
        /// <exception cref="DuplicateSampleAbscissaException">if arguments are invalid </exception>
        public IUnivariateRealFunction Interpolate(double[] x, double[] y)
        {

            /// <summary>
            /// a[] and c[] are defined in the general formula of Newton form:
            /// p(x) = a[0] + a[1](x-c[0]) + a[2](x-c[0])(x-c[1]) + [] +
            ///        a[n](x-c[0])(x-c[1])...(x-c[n-1])
            /// </summary>
            PolynomialFunctionLagrangeForm.VerifyInterpolationArray(x, y);

            /// <summary>
            /// When used for interpolation, the Newton form formula becomes
            /// p(x) = f[x0] + f[x0,x1](x-x0) + f[x0,x1,x2](x-x0)(x-x1) + [] +
            ///        f[x0,x1,[],x[n-1]](x-x0)(x-x1)...(x-x[n-2])
            /// Therefore, a[k] = f[x0,x1,[],xk], c[k] = x[k].
            /// <p>
            /// Note x[], y[], a[] have the same Length but c[]'s size is one less.</p>
            /// </summary>
            double[] c = new double[x.Length - 1];
            Array.Copy(x, 0, c, 0, c.Length);

            double[] a = computeDividedDifference(x, y);
            return new PolynomialFunctionNewtonForm(a, c);

        }

        /// <summary>
        /// Returns a copy of the divided difference array.
        /// <p>
        /// The divided difference array is defined recursively by <pre>
        /// f[x0] = f(x0)
        /// f[x0,x1,[],xk] = (f(x1,[],xk) - f(x0,[],x[k-1])) / (xk - x0)
        /// </pre></p>
        /// <p>
        /// The computational complexity is O(N^2).</p>
        /// 
        /// </summary>
        /// <param Name="x">the interpolating points array</param>
        /// <param Name="y">the interpolating values array</param>
        /// <returns>a fresh copy of the divided difference array</returns>
        /// <exception cref="DuplicateSampleAbscissaException">if any abscissas coincide </exception>
        protected static double[] computeDividedDifference(double[] x, double[] y)
        {

            PolynomialFunctionLagrangeForm.VerifyInterpolationArray(x, y);

            double[]
            divdiff = y.CloneExact(); // initialization

            int n = x.Length;
            double[]
            a = new double[n];
            a[0] = divdiff[0];
            AutoParallel.AutoParallelFor(1, n, (i) =>
            {
                AutoParallel.AutoParallelFor(0, n - i, (j) =>
                {
                    double denominator = x[j + i] - x[j];
                    if (denominator == 0.0)
                    {
                        // This happens only when two abscissas are identical.
                        throw new DuplicateSampleAbscissaException(x[j], j, j + i);
                    }
                    divdiff[j] = (divdiff[j + 1] - divdiff[j]) / denominator;
                });
                a[i] = divdiff[0];
            });

            return a;
        }
    }
}
