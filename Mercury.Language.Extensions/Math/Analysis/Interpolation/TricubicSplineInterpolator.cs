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
using Mercury.Language.Exception;
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis.Function;


namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// TricubicSplineInterpolator Description
    /// </summary>
    public class TricubicSplineInterpolator : ITrivariateRealGridInterpolator
    {
        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public ITrivariateRealFunction Interpolate(double[] xval, double[] yval, double[] zval, double[][][] fval)

        {
            if (xval.Length == 0 || yval.Length == 0 || zval.Length == 0 || fval.Length == 0)
            {
                throw new DataNotFoundException();
            }
            if (xval.Length != fval.Length)
            {
                throw new DimensionMismatchException(xval.Length, fval.Length);
            }

            Math2.CheckOrder(xval);
            Math2.CheckOrder(yval);
            Math2.CheckOrder(zval);

            int xLen = xval.Length;
            int yLen = yval.Length;
            int zLen = zval.Length;

            // Samples, re-ordered as (z, x, y) and (y, z, x) tuplets
            // fvalXY[k][i][j] = f(xval[i], yval[j], zval[k])
            // fvalZX[j][k][i] = f(xval[i], yval[j], zval[k])
            double[]
            []
            []
            fvalXY = ArrayUtility.CreateJaggedArray<double>(zLen, xLen, yLen);
            double[][][] fvalZX = ArrayUtility.CreateJaggedArray<double>(yLen, zLen, xLen);
            AutoParallel.AutoParallelFor(0, xLen, (i) =>
            {
                if (fval[i].Length != yLen)
                {
                    throw new DimensionMismatchException(fval[i].Length, yLen);
                }

                AutoParallel.AutoParallelFor(0, yLen, (j) =>
                {
                    if (fval[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(fval[i][j].Length, zLen);
                    }

                    AutoParallel.AutoParallelFor(0, zLen, (k) =>
                    {
                        double v = fval[i][j][k];
                        fvalXY[k][i][j] = v;
                        fvalZX[j][k][i] = v;
                    });
                });
            });

            BicubicSplineInterpolator bsi = new BicubicSplineInterpolator();

            // For each line x[i] (0 <= i < xLen), construct a 2D spline in y and z
            BicubicSplineInterpolatingFunction[] xSplineYZ
                        = new BicubicSplineInterpolatingFunction[xLen];
            AutoParallel.AutoParallelFor(0, xLen, (i) =>
            {
                xSplineYZ[i] = (BicubicSplineInterpolatingFunction)bsi.Interpolate(yval, zval, fval[i]);
            });

            // For each line y[j] (0 <= j < yLen), construct a 2D spline in z and x
            BicubicSplineInterpolatingFunction[] ySplineZX
                        = new BicubicSplineInterpolatingFunction[yLen];
            AutoParallel.AutoParallelFor(0, yLen, (j) =>
            {
                ySplineZX[j] = (BicubicSplineInterpolatingFunction)bsi.Interpolate(zval, xval, fvalZX[j]);
            });

            // For each line z[k] (0 <= k < zLen), construct a 2D spline in x and y
            BicubicSplineInterpolatingFunction[] zSplineXY
                        = new BicubicSplineInterpolatingFunction[zLen];
            AutoParallel.AutoParallelFor(0, zLen, (k) =>
            {
                zSplineXY[k] = (BicubicSplineInterpolatingFunction)bsi.Interpolate(xval, yval, fvalXY[k]);
            });

            // Partial derivatives wrt x and wrt y
            double[][][] dFdX = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            double[][][] dFdY = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            double[][][] d2FdXdY = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            AutoParallel.AutoParallelFor(0, zLen, (k) =>
            {
                BicubicSplineInterpolatingFunction f = zSplineXY[k];
                AutoParallel.AutoParallelFor(0, xLen, (i) =>
                {
                    double x = xval[i];
                    AutoParallel.AutoParallelFor(0, yLen, (j) =>
                    {
                        double y = yval[j];
                        dFdX[i][j][k] = f.partialDerivativeX(x, y);
                        dFdY[i][j][k] = f.partialDerivativeY(x, y);
                        d2FdXdY[i][j][k] = f.partialDerivativeXY(x, y);
                    });
                });
            });

            // Partial derivatives wrt y and wrt z
            double[][][] dFdZ = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            double[][][] d2FdYdZ = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            AutoParallel.AutoParallelFor(0, xLen, (i) =>
            {
                BicubicSplineInterpolatingFunction f = xSplineYZ[i];
                AutoParallel.AutoParallelFor(0, yLen, (j) =>
                {
                    double y = yval[j];
                    AutoParallel.AutoParallelFor(0, zLen, (k) =>
                    {
                        double z = zval[k];
                        dFdZ[i][j][k] = f.partialDerivativeY(y, z);
                        d2FdYdZ[i][j][k] = f.partialDerivativeXY(y, z);
                    });
                });
            });

            // Partial derivatives wrt x and wrt z
            double[][][] d2FdZdX = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            AutoParallel.AutoParallelFor(0, yLen, (j) =>
            {
                BicubicSplineInterpolatingFunction f = ySplineZX[j];
                AutoParallel.AutoParallelFor(0, zLen, (k) =>
                {
                    double z = zval[k];
                    AutoParallel.AutoParallelFor(0, xLen, (i) =>
                    {
                        double x = xval[i];
                        d2FdZdX[i][j][k] = f.partialDerivativeXY(z, x);
                    });
                });
            });

            // Third partial cross-derivatives
            double[][][] d3FdXdYdZ = ArrayUtility.CreateJaggedArray<double>(xLen, yLen, zLen);
            AutoParallel.AutoParallelFor(0, xLen, (i) =>
            {
                int nI = nextIndex(i, xLen);
                int pI = previousIndex(i);
                AutoParallel.AutoParallelFor(0, yLen, (j) =>
                {
                    int nJ = nextIndex(j, yLen);
                    int pJ = previousIndex(j);
                    AutoParallel.AutoParallelFor(0, zLen, (k) =>
                    {
                        int nK = nextIndex(k, zLen);
                        int pK = previousIndex(k);

                        // XXX Not sure about this formula
                        d3FdXdYdZ[i][j][k] = (fval[nI][nJ][nK] - fval[nI][pJ][nK] -
                                                          fval[pI][nJ][nK] + fval[pI][pJ][nK] -
                                                          fval[nI][nJ][pK] + fval[nI][pJ][pK] +
                                                          fval[pI][nJ][pK] - fval[pI][pJ][pK]) /
                                        ((xval[nI] - xval[pI]) * (yval[nJ] - yval[pJ]) * (zval[nK] - zval[pK]));
                    });
                });
            });

            // Create the interpolating splines
            return new TricubicSplineInterpolatingFunction(xval, yval, zval, fval,
                                                           dFdX, dFdY, dFdZ,
                                                           d2FdXdY, d2FdZdX, d2FdYdZ,
                                                           d3FdXdYdZ);
        }

        /// <summary>
        /// Compute the next index of an array, clipping if necessary.
        /// It is assumed (but not checked) that {@code i} is larger than or equal to 0}.
        /// 
        /// </summary>
        /// <param Name="i">Index</param>
        /// <param Name="max">Upper limit of the array</param>
        /// <returns>the next index</returns>
        private int nextIndex(int i, int max)
        {
            int index = i + 1;
            return index < max ? index : index - 1;
        }
        /// <summary>
        /// Compute the previous index of an array, clipping if necessary.
        /// It is assumed (but not checked) that {@code i} is smaller than the size of the array.
        /// 
        /// </summary>
        /// <param Name="i">Index</param>
        /// <returns>the previous index</returns>
        private int previousIndex(int i)
        {
            int index = i - 1;
            return index >= 0 ? index : 0;
        }
    }
}
