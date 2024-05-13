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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Function that  :  the
    /// <a href="http://en.wikipedia.org/wiki/Bicubic_interpolation">
    /// bicubic spline interpolation</a>.
    /// 
    /// @version $Revision$ $Date$
    /// @since 2.1
    /// </summary>
    public class BicubicSplineInterpolatingFunction : IBivariateRealFunction
    {
        /// <summary>
        /// Matrix to compute the spline coefficients from the function values
        /// and function derivatives values
        /// </summary>
        private static double[][] AINV = new double[][] {
        new double[] { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        new double[]{ 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
        new double[]{ -3,3,0,0,-2,-1,0,0,0,0,0,0,0,0,0,0 },
        new double[]{ 2,-2,0,0,1,1,0,0,0,0,0,0,0,0,0,0 },
        new double[]{ 0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
        new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0 },
        new double[]{ 0,0,0,0,0,0,0,0,-3,3,0,0,-2,-1,0,0 },
        new double[]{ 0,0,0,0,0,0,0,0,2,-2,0,0,1,1,0,0 },
        new double[]{ -3,0,3,0,0,0,0,0,-2,0,-1,0,0,0,0,0 },
        new double[]{ 0,0,0,0,-3,0,3,0,0,0,0,0,-2,0,-1,0 },
        new double[]{ 9,-9,-9,9,6,3,-6,-3,6,-6,3,-3,4,2,2,1 },
        new double[]{ -6,6,6,-6,-3,-3,3,3,-4,4,-2,2,-2,-2,-1,-1 },
        new double[]{ 2,0,-2,0,0,0,0,0,1,0,1,0,0,0,0,0 },
        new double[]{ 0,0,0,0,2,0,-2,0,0,0,0,0,1,0,1,0 },
        new double[]{ -6,6,6,-6,-4,-2,4,2,-3,3,-3,3,-2,-1,-2,-1 },
        new double[]{ 4,-4,-4,4,2,2,-2,-2,2,-2,2,-2,1,1,1,1 }
    };

        /// <summary>Samples x-coordinates */
        private double[] xval;
        /// <summary>Samples y-coordinates */
        private double[] yval;
        /// <summary>Set of cubic splines patching the whole data grid */
        private BicubicSplineFunction[][] splines;
        /// <summary>
        /// Partial derivatives
        /// The value of the first index determines the kind of derivatives:
        /// 0 = first partial derivatives wrt x
        /// 1 = first partial derivatives wrt y
        /// 2 = second partial derivatives wrt x
        /// 3 = second partial derivatives wrt y
        /// 4 = cross partial derivatives
        /// </summary>
        private IBivariateRealFunction[][][] partialDerivatives = null;

        /// <summary>
        /// </summary>
        /// <param name="x">Sample values of the x-coordinate, in increasing order.</param>
        /// <param name="y">Sample values of the y-coordinate, in increasing order.</param>
        /// <param name="f">Values of the function on every grid point.</param>
        /// <param name="dFdX">Values of the partial derivative of function with respect</param>
        /// to x on every grid point.
        /// <param name="dFdY">Values of the partial derivative of function with respect</param>
        /// to y on every grid point.
        /// <param name="d2FdXdY">Values of the cross partial derivative of function on</param>
        /// every grid point.
        /// <exception cref="DimensionMismatchException">if the various arrays do not contain </exception>
        /// the expected number of elements.
        /// <exception cref="Mercury.Language.Exceptions.NonMonotonousSequenceException"></exception>
        /// if {@code x} or {@code y} are not strictly increasing.
        /// <exception cref="DataNotFoundException">if any of the arrays has zero lengthd </exception>
        public BicubicSplineInterpolatingFunction(double[] x,
                                                  double[] y,
                                                  double[][] f,
                                                  double[][] dFdX,
                                                  double[][] dFdY,
                                                  double[][] d2FdXdY)

        {
            int xLen = x.Length;
            int yLen = y.Length;

            if (xLen == 0 || yLen == 0 || f.Length == 0 || f.GetLength(1) == 0)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().NO_DATA);
            }
            if (xLen != f.Length)
            {
                throw new DimensionMismatchException(xLen, f.Length);
            }
            if (xLen != dFdX.Length)
            {
                throw new DimensionMismatchException(xLen, dFdX.Length);
            }
            if (xLen != dFdY.Length)
            {
                throw new DimensionMismatchException(xLen, dFdY.Length);
            }
            if (xLen != d2FdXdY.Length)
            {
                throw new DimensionMismatchException(xLen, d2FdXdY.Length);
            }

            QuickMath.CheckOrder(x);
            QuickMath.CheckOrder(y);

            xval = x.CloneExact();
            yval = y.CloneExact();

            int lastI = xLen - 1;
            int lastJ = yLen - 1;
            splines = ArrayUtility.CreateJaggedArray<BicubicSplineFunction>(lastI, lastJ);

            for (int i = 0; i < lastI; i++)
            {
                if (f[i].Length != yLen)
                {
                    throw new DimensionMismatchException(f[i].Length, yLen);
                }
                if (dFdX[i].Length != yLen)
                {
                    throw new DimensionMismatchException(dFdX[i].Length, yLen);
                }
                if (dFdY[i].Length != yLen)
                {
                    throw new DimensionMismatchException(dFdY[i].Length, yLen);
                }
                if (d2FdXdY[i].Length != yLen)
                {
                    throw new DimensionMismatchException(d2FdXdY[i].Length, yLen);
                }
                int ip1 = i + 1;
                for (int j = 0; j < lastJ; j++)
                {
                    int jp1 = j + 1;
                    double[] beta = new double[] {
                    f[i][j], f[ip1][j], f[i][jp1], f[ip1][jp1],
                    dFdX[i][j], dFdX[ip1][j], dFdX[i][jp1], dFdX[ip1][jp1],
                    dFdY[i][j], dFdY[ip1][j], dFdY[i][jp1], dFdY[ip1][jp1],
                    d2FdXdY[i][j], d2FdXdY[ip1][j], d2FdXdY[i][jp1], d2FdXdY[ip1][jp1]
                };

                    splines[i][j] = new BicubicSplineFunction(computeSplineCoefficients(beta));
                }
            }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public double Value(double x, double y)
        {
            int i = searchIndex(x, xval);
            if (i == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, xval[0], xval[xval.Length - 1]));
            }
            int j = searchIndex(y, yval);
            if (j == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, y, yval[0], yval[yval.Length - 1]));
            }

            double xN = (x - xval[i]) / (xval[i + 1] - xval[i]);
            double yN = (y - yval[j]) / (yval[j + 1] - yval[j]);

            return splines[i][j].Value(xN, yN);
        }

        /// <summary>
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        /// <returns>the value at point (x, y) of the first partial derivative with</returns>
        /// respect to x.
        /// @since 2.2
        public double partialDerivativeX(double x, double y)
        {
            return PartialDerivative(0, x, y);
        }
        /// <summary>
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        /// <returns>the value at point (x, y) of the first partial derivative with</returns>
        /// respect to y.
        /// @since 2.2
        public double partialDerivativeY(double x, double y)
        {
            return PartialDerivative(1, x, y);
        }
        /// <summary>
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        /// <returns>the value at point (x, y) of the second partial derivative with</returns>
        /// respect to x.
        /// @since 2.2
        public double partialDerivativeXX(double x, double y)
        {
            return PartialDerivative(2, x, y);
        }
        /// <summary>
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        /// <returns>the value at point (x, y) of the second partial derivative with</returns>
        /// respect to y.
        /// @since 2.2
        public double partialDerivativeYY(double x, double y)
        {
            return PartialDerivative(3, x, y);
        }
        /// <summary>
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        /// <returns>the value at point (x, y) of the second partial cross-derivative.</returns>
        /// @since 2.2
        public double partialDerivativeXY(double x, double y)
        {
            return PartialDerivative(4, x, y);
        }

        /// <summary>
        /// </summary>
        /// <param name="which">First index in {@link #partialDerivatives}.</param>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        /// <returns>the value at point (x, y) of the selected partial derivative.</returns>
        /// <exception cref="FunctionEvaluationException"></exception>
        private double PartialDerivative(int which, double x, double y)
        {
            if (partialDerivatives == null)
            {
                computePartialDerivatives();
            }

            int i = searchIndex(x, xval);
            if (i == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, xval[0], xval[xval.Length - 1]));
            }
            int j = searchIndex(y, yval);
            if (j == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, y, yval[0], yval[yval.Length - 1]));
            }

            double xN = (x - xval[i]) / (xval[i + 1] - xval[i]);
            double yN = (y - yval[j]) / (yval[j + 1] - yval[j]);

            try
            {
                return partialDerivatives[which][i][j].Value(xN, yN);
            }
            catch (FunctionEvaluationException fee)
            {
                // this should never happen
                throw new System.Exception(LocalizedResources.Instance().FUNCTION, fee);
            }
        }

        /// <summary>
        /// Compute all partial derivatives.
        /// </summary>
        private void computePartialDerivatives()
        {
            int lastI = xval.Length - 1;
            int lastJ = yval.Length - 1;
            partialDerivatives = ArrayUtility.CreateJaggedArray<IBivariateRealFunction>(5, lastI, lastJ);

            for (int i = 0; i < lastI; i++)
            {
                for (int j = 0; j < lastJ; j++)
                {
                    BicubicSplineFunction f = splines[i][j];
                    partialDerivatives[0][i][j] = f.PartialDerivativeX;
                    partialDerivatives[1][i][j] = f.PartialDerivativeY;
                    partialDerivatives[2][i][j] = f.PartialDerivativeXX;
                    partialDerivatives[3][i][j] = f.PartialDerivativeYY;
                    partialDerivatives[4][i][j] = f.PartialDerivativeXY;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="c">Coordinate.</param>
        /// <param name="val">Coordinate samples.</param>
        /// <returns>the index in {@code val} corresponding to the interval</returns>
        /// containing {@code c}, or {@code -1} if {@code c} is out of the
        /// range defined by the end values of {@code val}.
        private int searchIndex(double c, double[] val)
        {
            if (c < val[0])
            {
                return -1;
            }

            int max = val.Length;
            for (int i = 1; i < max; i++)
            {
                if (c <= val[i])
                {
                    return i - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Compute the spline coefficients from the list of function values and
        /// function partial derivatives values at the four corners of a grid
        /// elementd They must be specified in the following order:
        /// <ul>
        ///  <li>f(0,0)</li>
        ///  <li>f(1,0)</li>
        ///  <li>f(0,1)</li>
        ///  <li>f(1,1)</li>
        ///  <li>f<sub>x</sub>(0,0)</li>
        ///  <li>f<sub>x</sub>(1,0)</li>
        ///  <li>f<sub>x</sub>(0,1)</li>
        ///  <li>f<sub>x</sub>(1,1)</li>
        ///  <li>f<sub>y</sub>(0,0)</li>
        ///  <li>f<sub>y</sub>(1,0)</li>
        ///  <li>f<sub>y</sub>(0,1)</li>
        ///  <li>f<sub>y</sub>(1,1)</li>
        ///  <li>f<sub>xy</sub>(0,0)</li>
        ///  <li>f<sub>xy</sub>(1,0)</li>
        ///  <li>f<sub>xy</sub>(0,1)</li>
        ///  <li>f<sub>xy</sub>(1,1)</li>
        /// </ul>
        /// where the subscripts indicate the partial derivative with respect to
        /// the corresponding variable(s).
        /// 
        /// </summary>
        /// <param name="beta">List of function values and function partial derivatives</param>
        /// values.
        /// <returns>the spline coefficients.</returns>
        private double[] computeSplineCoefficients(double[] beta)
        {
            double[] a = new double[16];

            for (int i = 0; i < 16; i++)
            {
                double result = 0;
                double[] row = AINV[i];
                for (int j = 0; j < 16; j++)
                {
                    result += row[j] * beta[j];
                }
                a[i] = result;
            }

            return a;
        }
    }

    /// <summary>
    /// 2D-spline function.
    /// 
    /// @version $Revision$ $Date$
    /// </summary>
    class BicubicSplineFunction
         : IBivariateRealFunction
    {

        /// <summary>Number of pointsd */
        private static short N = 4;

        /// <summary>Coefficients */
        private double[][] a;

        /// <summary>First partial derivative along xd */
        private IBivariateRealFunction partialDerivativeX;

        /// <summary>First partial derivative along yd */
        private IBivariateRealFunction partialDerivativeY;

        /// <summary>Second partial derivative along xd */
        private IBivariateRealFunction partialDerivativeXX;

        /// <summary>Second partial derivative along yd */
        private IBivariateRealFunction partialDerivativeYY;

        /// <summary>Second crossed partial derivatived */
        private IBivariateRealFunction partialDerivativeXY;

        /// <summary>
        /// Simple constructor.
        /// </summary>
        /// <param name="a">Spline coefficients</param>
        public BicubicSplineFunction(double[] a)
        {
            this.a = ArrayUtility.CreateJaggedArray<double>(N, N);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    this.a[i][j] = a[i + N * j];
                }
            }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public double Value(double x, double y)
        {
            if (x < 0 || x > 1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, 0, 1));
            }
            if (y < 0 || y > 1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, y, 0, 1));
            }

            double x2 = x * x;
            double x3 = x2 * x;
            double[] pX = { 1, x, x2, x3 };

            double y2 = y * y;
            double y3 = y2 * y;
            double[] pY = { 1, y, y2, y3 };

            return apply(pX, pY, a);
        }

        /// <summary>
        /// Compute the value of the bicubic polynomial.
        /// 
        /// </summary>
        /// <param name="pX">Powers of the x-coordinate.</param>
        /// <param name="pY">Powers of the y-coordinate.</param>
        /// <param name="coeff">Spline coefficients.</param>
        /// <returns>the interpolated value.</returns>
        private static double apply(double[] pX, double[] pY, double[][] coeff)
        {
            double result = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result += coeff[i][j] * pX[i] * pY[j];
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>the partial derivative wrt {@code x}.</returns>
        public IBivariateRealFunction PartialDerivativeX
        {
            get
            {
                if (partialDerivativeX == null)
                {
                    computePartialDerivatives();
                }

                return partialDerivativeX;
            }
        }
        /// <summary>
        /// </summary>
        /// <returns>the partial derivative wrt {@code y}.</returns>
        public IBivariateRealFunction PartialDerivativeY
        {
            get
            {
                if (partialDerivativeY == null)
                {
                    computePartialDerivatives();
                }

                return partialDerivativeY;
            }
        }
        /// <summary>
        /// </summary>
        /// <returns>the second partial derivative wrt {@code x}.</returns>
        public IBivariateRealFunction PartialDerivativeXX
        {
            get
            {
                if (partialDerivativeXX == null)
                {
                    computePartialDerivatives();
                }

                return partialDerivativeXX;
            }
        }
        /// <summary>
        /// </summary>
        /// <returns>the second partial derivative wrt {@code y}.</returns>
        public IBivariateRealFunction PartialDerivativeYY
        {
            get
            {
                if (partialDerivativeYY == null)
                {
                    computePartialDerivatives();
                }

                return partialDerivativeYY;
            }
        }
        /// <summary>
        /// </summary>
        /// <returns>the second partial cross-derivative.</returns>
        public IBivariateRealFunction PartialDerivativeXY
        {
            get
            {
                if (partialDerivativeXY == null)
                {
                    computePartialDerivatives();
                }

                return partialDerivativeXY;
            }
        }

        /// <summary>
        /// Compute all partial derivatives functions.
        /// </summary>
        private void computePartialDerivatives()
        {
            double[][] aX = ArrayUtility.CreateJaggedArray<double>(N, N);
            double[][] aY = ArrayUtility.CreateJaggedArray<double>(N, N);
            double[][] aXX = ArrayUtility.CreateJaggedArray<double>(N, N);
            double[][] aYY = ArrayUtility.CreateJaggedArray<double>(N, N);
            double[][] aXY = ArrayUtility.CreateJaggedArray<double>(N, N);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    double c = a[i][j];
                    aX[i][j] = i * c;
                    aY[i][j] = j * c;
                    aXX[i][j] = (i - 1) * aX[i][j];
                    aYY[i][j] = (j - 1) * aY[i][j];
                    aXY[i][j] = j * aX[i][j];
                }
            }

            partialDerivativeX = new PartialDerivativeXBivariateRealFunction(aX);
            partialDerivativeY = new PartialDerivativeYBivariateRealFunction(aY);
            partialDerivativeXX = new PartialDerivativeXXBivariateRealFunction(aXX);
            partialDerivativeYY = new PartialDerivativeYYBivariateRealFunction(aYY);
            partialDerivativeXY = new PartialDerivativeXYBivariateRealFunction(aXY);
        }

        private class PartialDerivativeXBivariateRealFunction : IBivariateRealFunction
        {
            double[][] _aX;

            public PartialDerivativeXBivariateRealFunction(double[][] aX)
            {
                _aX = aX;
            }

            public double Value(double x, double y)
            {
                double x2 = x * x;
                double[] pX = { 0, 1, x, x2 };

                double y2 = y * y;
                double y3 = y2 * y;
                double[] pY = { 1, y, y2, y3 };

                return apply(pX, pY, _aX);
            }
        }

        private class PartialDerivativeYBivariateRealFunction : IBivariateRealFunction
        {
            double[][] _aY;

            public PartialDerivativeYBivariateRealFunction(double[][] aY)
            {
                _aY = aY;
            }

            public double Value(double x, double y)
            {
                double x2 = x * x;
                double x3 = x2 * x;
                double[] pX = { 1, x, x2, x3 };

                double y2 = y * y;
                double[] pY = { 0, 1, y, y2 };

                return apply(pX, pY, _aY);
            }

        }

        private class PartialDerivativeXXBivariateRealFunction : IBivariateRealFunction
        {
            double[][] _aXX;

            public PartialDerivativeXXBivariateRealFunction(double[][] aXX)
            {
                _aXX = aXX;
            }
            public double Value(double x, double y)
            {
                double[] pX = { 0, 0, 1, x };

                double y2 = y * y;
                double y3 = y2 * y;
                double[] pY = { 1, y, y2, y3 };

                return apply(pX, pY, _aXX);
            }

        }

        private class PartialDerivativeYYBivariateRealFunction : IBivariateRealFunction
        {
            double[][] _aYY;

            public PartialDerivativeYYBivariateRealFunction(double[][] aYY)
            {
                _aYY = aYY;
            }
            public double Value(double x, double y)
            {
                double x2 = x * x;
                double x3 = x2 * x;
                double[] pX = { 1, x, x2, x3 };

                double[] pY = { 0, 0, 1, y };

                return apply(pX, pY, _aYY);
            }

        }

        private class PartialDerivativeXYBivariateRealFunction : IBivariateRealFunction
        {
            double[][] _aXY;

            public PartialDerivativeXYBivariateRealFunction(double[][] aXY)
            {
                _aXY = aXY;
            }
            public double Value(double x, double y)
            {
                double x2 = x * x;
                double[] pX = { 0, 1, x, x2 };

                double y2 = y * y;
                double[] pY = { 0, 1, y, y2 };

                return apply(pX, pY, _aXY);
            }
        }
    }
}
