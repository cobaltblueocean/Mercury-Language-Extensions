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
using Mercury.Language.Math;
using Mercury.Language.Math.Optimization.General;
using Mercury.Language.Math.Optimization.Fitting;
using Mercury.Language.Math.Analysis.Polynomial;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// SmoothingPolynomialBicubicSplineInterpolator Description
    /// </summary>
    public class SmoothingPolynomialBicubicSplineInterpolator : BicubicSplineInterpolator
    {

        /// <summary>Fitter for xd */
        private PolynomialFitter xFitter;

        /// <summary>Fitter for yd */
        private PolynomialFitter yFitter;

        /// <summary>
        /// Default constructord The degree of the fitting polynomials is set to 3d
        /// </summary>
        public SmoothingPolynomialBicubicSplineInterpolator() : this(3)
        {

        }

        /// <summary>
        /// </summary>
        /// <param Name="degree">Degree of the polynomial fitting functions.</param>
        public SmoothingPolynomialBicubicSplineInterpolator(int degree) : this(degree, degree)
        {

        }

        /// <summary>
        /// </summary>
        /// <param Name="xDegree">Degree of the polynomial fitting functions along the</param>
        /// x-dimension.
        /// <param Name="yDegree">Degree of the polynomial fitting functions along the</param>
        /// y-dimension.
        public SmoothingPolynomialBicubicSplineInterpolator(int xDegree,
                                                            int yDegree)
        {
            xFitter = new PolynomialFitter(xDegree, new GaussNewtonOptimizer(false));
            yFitter = new PolynomialFitter(yDegree, new GaussNewtonOptimizer(false));
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        //@Override
        public override IBivariateRealFunction Interpolate(double[] xval,
                                                              double[] yval,
                                                              double[][] fval)

        {
            if (xval.Length == 0 || yval.Length == 0 || fval.Length == 0)
            {
                throw new DataNotFoundException();
            }
            if (xval.Length != fval.Length)
            {
                throw new DimensionMismatchException(xval.Length, fval.Length);
            }

            int xLen = xval.Length;
            int yLen = yval.Length;

            for (int i = 0; i < xLen; i++)
            {
                if (fval[i].Length != yLen)
                {
                    throw new DimensionMismatchException(fval[i].Length, yLen);
                }
            }

            Math2.CheckOrder(xval);
            Math2.CheckOrder(yval);

            // For each line y[j] (0 <= j < yLen), construct a polynomial, with
            // respect to variable x, fitting array fval[][j]
            PolynomialFunction[]
            yPolyX = new PolynomialFunction[yLen];
            AutoParallel.AutoParallelFor(0, yLen, (j) =>
            {
                xFitter.ClearObservations();
                AutoParallel.AutoParallelFor(0, xLen, (i) =>
                {
                    xFitter.AddObservedPoint(1, xval[i], fval[i][j]);
                });

                yPolyX[j] = xFitter.Fit();
            });

            // For every knot (xval[i], yval[j]) of the grid, calculate corrected
            // values fval_1
            double[][] fval_1 = ArrayUtility.CreateJaggedArray<double>(xLen, yLen);
            AutoParallel.AutoParallelFor(0, yLen, (j) =>
            {
                PolynomialFunction f = yPolyX[j];
                AutoParallel.AutoParallelFor(0, xLen, (i) =>
                {
                    fval_1[i][j] = f.Value(xval[i]);
                });
            });

            // For each line x[i] (0 <= i < xLen), construct a polynomial, with
            // respect to variable y, fitting array fval_1[i][]
            PolynomialFunction[] xPolyY = new PolynomialFunction[xLen];
            AutoParallel.AutoParallelFor(0, xLen, (i) =>
            {
                yFitter.ClearObservations();
                AutoParallel.AutoParallelFor(0, yLen, (j) =>
                {
                    yFitter.AddObservedPoint(1, yval[j], fval_1[i][j]);
                });

                xPolyY[i] = yFitter.Fit();
            });

            // For every knot (xval[i], yval[j]) of the grid, calculate corrected
            // values fval_2
            double[][] fval_2 = ArrayUtility.CreateJaggedArray<double>(xLen, yLen);
            AutoParallel.AutoParallelFor(0, xLen, (i) =>
            {
                PolynomialFunction f = xPolyY[i];
                AutoParallel.AutoParallelFor(0, yLen, (j) =>
                {
                    fval_2[i][j] = f.Value(yval[j]);
                });
            });

            return base.Interpolate(xval, yval, fval_2);
        }
    }
}
