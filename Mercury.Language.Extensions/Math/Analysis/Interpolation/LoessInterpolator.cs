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
using Mercury.Language.Math.Analysis.Polynomial;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Implements the <a href="http://en.wikipedia.org/wiki/Local_regression">
    /// Local Regression Algorithm</a> (also Loess, Lowess) for interpolation of
    /// real univariate functions.
    /// <p/>
    /// For reference, see
    /// <a href="http://www.System.Math.tau.ac.il/~yekutiel/MA seminar/Cleveland 1979dpdf">
    /// William Sd Cleveland - Robust Locally Weighted Regression and Smoothing
    /// Scatterplots</a>
    /// <p/>
    /// This class  :  both the loess method and serves as an interpolation
    /// adapter to it, allowing to build a spline on the obtained loess fit.
    /// 
    /// @version $Revision: 990655 $ $Date: 2010-08-29 23:49:40 +0200 (dimd 29 août 2010) $
    /// @since 2.0
    /// </summary>
    [Serializable]
    public class LoessInterpolator : IUnivariateRealInterpolator
    {
        /// <summary>Default value of the bandwidth parameterd */
        public static double DEFAULT_BANDWIDTH = 0.3;

        /// <summary>Default value of the number of robustness iterationsd */
        public static int DEFAULT_ROBUSTNESS_ITERS = 2;

        /// <summary>
        /// Default value for accuracy.
        /// @since 2.1
        /// </summary>
        public static double DEFAULT_ACCURACY = 1e-12;

        /// <summary>
        /// The bandwidth parameter: when computing the loess fit at
        /// a particular point, this fraction of source points closest
        /// to the current point is taken into account for computing
        /// a least-squares regression.
        /// <p/>
        /// A sensible value is usually 0.25 to 0.5d
        /// </summary>
        private double bandwidth;

        /// <summary>
        /// The number of robustness iterations parameter: this many
        /// robustness iterations are done.
        /// <p/>
        /// A sensible value is usually 0 (just the initial fit without any
        /// robustness iterations) to 4d
        /// </summary>
        private int robustnessIters;

        /// <summary>
        /// If the median residual at a certain robustness iteration
        /// is less than this amount, no more iterations are done.
        /// </summary>
        private double accuracy;

        /// <summary>
        /// Constructs a new {@link LoessInterpolator}
        /// with a bandwidth of {@link #DEFAULT_BANDWIDTH},
        /// {@link #DEFAULT_ROBUSTNESS_ITERS} robustness iterations
        /// and an accuracy of {#link #DEFAULT_ACCURACY}.
        /// See {@link #LoessInterpolator(double, int, double)} for an explanation of
        /// the parameters.
        /// </summary>
        public LoessInterpolator()
        {
            this.bandwidth = DEFAULT_BANDWIDTH;
            this.robustnessIters = DEFAULT_ROBUSTNESS_ITERS;
            this.accuracy = DEFAULT_ACCURACY;
        }

        /// <summary>
        /// Constructs a new {@link LoessInterpolator}
        /// with given bandwidth and number of robustness iterations.
        /// <p>
        /// Calling this constructor is equivalent to calling {link {@link
        /// #LoessInterpolator(double, int, double) LoessInterpolator(bandwidth,
        /// robustnessIters, LoessInterpolator.DEFAULT_ACCURACY)}
        /// </p>
        /// 
        /// </summary>
        /// <param Name="bandwidth"> when computing the loess fit at</param>
        /// a particular point, this fraction of source points closest
        /// to the current point is taken into account for computing
        /// a least-squares regression.</br>
        /// A sensible value is usually 0.25 to 0.5, the default value is
        /// {@link #DEFAULT_BANDWIDTH}.
        /// <param Name="robustnessIters">This many robustness iterations are done.</br></param>
        /// A sensible value is usually 0 (just the initial fit without any
        /// robustness iterations) to 4, the default value is
        /// {@link #DEFAULT_ROBUSTNESS_ITERS}.
        /// <exception cref="MathArithmeticException">if bandwidth does not lie in the interval [0,1] </exception>
        /// or if robustnessIters is negative.
        /// <see cref="#LoessInterpolator(double,">int, double) </see>
        public LoessInterpolator(double bandwidth, int robustnessIters) : this(bandwidth, robustnessIters, DEFAULT_ACCURACY)
        {

        }

        /// <summary>
        /// Constructs a new {@link LoessInterpolator}
        /// with given bandwidth, number of robustness iterations and accuracy.
        /// 
        /// </summary>
        /// <param Name="bandwidth"> when computing the loess fit at</param>
        /// a particular point, this fraction of source points closest
        /// to the current point is taken into account for computing
        /// a least-squares regression.</br>
        /// A sensible value is usually 0.25 to 0.5, the default value is
        /// {@link #DEFAULT_BANDWIDTH}.
        /// <param Name="robustnessIters">This many robustness iterations are done.</br></param>
        /// A sensible value is usually 0 (just the initial fit without any
        /// robustness iterations) to 4, the default value is
        /// {@link #DEFAULT_ROBUSTNESS_ITERS}.
        /// <param Name="accuracy">If the median residual at a certain robustness iteration</param>
        /// is less than this amount, no more iterations are done.
        /// <exception cref="MathArithmeticException">if bandwidth does not lie in the interval [0,1] </exception>
        /// or if robustnessIters is negative.
        /// <see cref="#LoessInterpolator(double,">int) </see>
        /// @since 2.1
        public LoessInterpolator(double bandwidth, int robustnessIters, double accuracy)
        {
            if (bandwidth < 0 || bandwidth > 1)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().BANDWIDTH_OUT_OF_INTERVAL, bandwidth));
            }
            this.bandwidth = bandwidth;
            if (robustnessIters < 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().NEGATIVE_ROBUSTNESS_ITERATIONS, robustnessIters));
            }
            this.robustnessIters = robustnessIters;
            this.accuracy = accuracy;
        }

        /// <summary>
        /// Compute an interpolating function by performing a loess fit
        /// on the data at the original abscissae and then building a cubic spline
        /// with a
        /// {@link Mercury.Language.Math.Analysis.Interpolation.SplineInterpolator}
        /// on the resulting fit.
        /// 
        /// </summary>
        /// <param Name="xval">the arguments for the interpolation points</param>
        /// <param Name="yval">the values for the interpolation points</param>
        /// <returns>A cubic spline built upon a loess fit to the data at the original abscissae</returns>
        /// <exception cref="MathArithmeticException"> if some of the following conditions are false: </exception>
        /// <ul>
        /// <li> Arguments and values are of the same size that is greater than zero</li>
        /// <li> The arguments are in a strictly increasing order</li>
        /// <li> All arguments and values are finite real numbers</li>
        /// </ul>
        public IUnivariateRealFunction Interpolate(
                double[] xval, double[] yval)
        {
            return new SplineInterpolator().Interpolate(xval, smooth(xval, yval));
        }

        /// <summary>
        /// Compute a weighted loess fit on the data at the original abscissae.
        /// 
        /// </summary>
        /// <param Name="xval">the arguments for the interpolation points</param>
        /// <param Name="yval">the values for the interpolation points</param>
        /// <param Name="weights">point weights: coefficients by which the robustness weight of a point is multiplied</param>
        /// <returns>values of the loess fit at corresponding original abscissae</returns>
        /// <exception cref="MathArithmeticException">if some of the following conditions are false: </exception>
        /// <ul>
        /// <li> Arguments and values are of the same size that is greater than zero</li>
        /// <li> The arguments are in a strictly increasing order</li>
        /// <li> All arguments and values are finite real numbers</li>
        /// </ul>
        /// @since 2.1
        public double[] smooth(double[] xval, double[] yval, double[] weights)

        {
            if (xval.Length != yval.Length)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().MISMATCHED_LOESS_ABSCISSA_ORDINATE_ARRAYS, xval.Length, yval.Length));
            }

            int n = xval.Length;

            if (n == 0)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().LOESS_EXPECTS_AT_LEAST_ONE_POINT);
            }

            checkAllFiniteReal(xval, LocalizedResources.Instance().NON_REAL_FINITE_ABSCISSA);
            checkAllFiniteReal(yval, LocalizedResources.Instance().NON_REAL_FINITE_ORDINATE);
            checkAllFiniteReal(weights, LocalizedResources.Instance().NON_REAL_FINITE_WEIGHT);

            checkStrictlyIncreasing(xval);

            if (n == 1)
            {
                return new double[] { yval[0] };
            }

            if (n == 2)
            {
                return new double[] { yval[0], yval[1] };
            }

            int bandwidthInPoints = (int)(bandwidth * n);

            if (bandwidthInPoints < 2)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().TOO_SMALL_BANDWIDTH, n, 2.0 / n, bandwidth));
            }

            double[]
            res = new double[n];

            double[] residuals = new double[n];
            double[] sortedResiduals = new double[n];

            double[] robustnessWeights = new double[n];

            // Do an initial fit and 'robustnessIters' robustness iterations.
            // This is equivalent to doing 'robustnessIters+1' robustness iterations
            // starting with all robustness weights set to 1d
            robustnessWeights.Fill(1);

            for (int iter = 0; iter <= robustnessIters; ++iter)
            {
                int[] bandwidthInterval = { 0, bandwidthInPoints - 1 };
                // At each x, compute a local weighted linear regression
                for (int i = 0; i < n; ++i)
                {
                    double x = xval[i];

                    // Find out the interval of source points on which
                    // a regression is to be made.
                    if (i > 0)
                    {
                        updateBandwidthInterval(xval, weights, i, bandwidthInterval);
                    }

                    int ileft = bandwidthInterval[0];
                    int iright = bandwidthInterval[1];

                    // Compute the point of the bandwidth interval that is
                    // farthest from x
                    int edge;
                    if (xval[i] - xval[ileft] > xval[iright] - xval[i])
                    {
                        edge = ileft;
                    }
                    else
                    {
                        edge = iright;
                    }

                    // Compute a least-squares linear fit weighted by
                    // the product of robustness weights and the tricube
                    // weight function.
                    // See http://en.wikipedia.org/wiki/Linear_regression
                    // (section "Univariate linear case")
                    // and http://en.wikipedia.org/wiki/Weighted_least_squares
                    // (section "Weighted least squares")
                    double sumWeights = 0;
                    double sumX = 0;
                    double sumXSquared = 0;
                    double sumY = 0;
                    double sumXY = 0;
                    double denom = System.Math.Abs(1.0 / (xval[edge] - x));
                    for (int k = ileft; k <= iright; ++k)
                    {
                        double xk = xval[k];
                        double yk = yval[k];
                        double dist = (k < i) ? x - xk : xk - x;
                        double w = tricube(dist * denom) * robustnessWeights[k] * weights[k];
                        double xkw = xk * w;
                        sumWeights += w;
                        sumX += xkw;
                        sumXSquared += xk * xkw;
                        sumY += yk * w;
                        sumXY += yk * xkw;
                    }

                    double meanX = sumX / sumWeights;
                    double meanY = sumY / sumWeights;
                    double meanXY = sumXY / sumWeights;
                    double meanXSquared = sumXSquared / sumWeights;

                    double beta;
                    if (System.Math.Sqrt(System.Math.Abs(meanXSquared - meanX * meanX)) < accuracy)
                    {
                        beta = 0;
                    }
                    else
                    {
                        beta = (meanXY - meanX * meanY) / (meanXSquared - meanX * meanX);
                    }

                    double alpha = meanY - beta * meanX;

                    res[i] = beta * x + alpha;
                    residuals[i] = System.Math.Abs(yval[i] - res[i]);
                }

                // No need to recompute the robustness weights at the last
                // iteration, they won't be needed anymore
                if (iter == robustnessIters)
                {
                    break;
                }

                // Recompute the robustness weights.

                // Find the median residual.
                // An arraycopy and a sort are completely tractable here,
                // because the preceding loop is a lot more expensive
                Array.Copy(residuals, 0, sortedResiduals, 0, n);
                Array.Sort(sortedResiduals);
                double medianResidual = sortedResiduals[n / 2];

                if (System.Math.Abs(medianResidual) < accuracy)
                {
                    break;
                }

                for (int i = 0; i < n; ++i)
                {
                    double arg = residuals[i] / (6 * medianResidual);
                    if (arg >= 1)
                    {
                        robustnessWeights[i] = 0;
                    }
                    else
                    {
                        double w = 1 - arg * arg;
                        robustnessWeights[i] = w * w;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Compute a loess fit on the data at the original abscissae.
        /// 
        /// </summary>
        /// <param Name="xval">the arguments for the interpolation points</param>
        /// <param Name="yval">the values for the interpolation points</param>
        /// <returns>values of the loess fit at corresponding original abscissae</returns>
        /// <exception cref="MathArithmeticException">if some of the following conditions are false: </exception>
        /// <ul>
        /// <li> Arguments and values are of the same size that is greater than zero</li>
        /// <li> The arguments are in a strictly increasing order</li>
        /// <li> All arguments and values are finite real numbers</li>
        /// </ul>
        public double[] smooth(double[] xval, double[] yval)

        {
            if (xval.Length != yval.Length)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().MISMATCHED_LOESS_ABSCISSA_ORDINATE_ARRAYS, xval.Length, yval.Length));
            }

            double[]
            unitWeights = new double[xval.Length];
            unitWeights.Fill(1.0);

            return smooth(xval, yval, unitWeights);
        }

        /// <summary>
        /// Given an index interval into xval that embraces a certain number of
        /// points closest to xval[i-1], update the interval so that it embraces
        /// the same number of points closest to xval[i], ignoring zero weights.
        /// 
        /// </summary>
        /// <param Name="xval">arguments array</param>
        /// <param Name="weights">weights array</param>
        /// <param Name="i">the index around which the new interval should be computed</param>
        /// <param Name="bandwidthInterval">a two-element array {left, right} such that: <p/></param>
        /// <i>(left==0 or xval[i] - xval[left-1] > xval[right] - xval[i])</i>
        /// <p/> and also <p/>
        /// <i>(right==xval.Length-1 or xval[right+1] - xval[i] > xval[i] - xval[left])</i>.
        /// The array will be updated.
        private static void updateBandwidthInterval(double[] xval, double[] weights,
                                                    int i,
                                                    int[] bandwidthInterval)
        {
            int left = bandwidthInterval[0];
            int right = bandwidthInterval[1];

            // The right edge should be adjusted if the next point to the right
            // is closer to xval[i] than the leftmost point of the current interval
            int nextRight = nextNonzero(weights, right);
            if (nextRight < xval.Length && xval[nextRight] - xval[i] < xval[i] - xval[left])
            {
                int nextLeft = nextNonzero(weights, bandwidthInterval[0]);
                bandwidthInterval[0] = nextLeft;
                bandwidthInterval[1] = nextRight;
            }
        }

        /// <summary>
        /// Returns the smallest index j such that j > i && (j==weights.Length || weights[j] != 0)
        /// </summary>
        /// <param Name="weights">weights array</param>
        /// <param Name="i">the index from which to start search; must be < weights.Length</param>
        /// <returns>the smallest index j such that j > i && (j==weights.Length || weights[j] != 0)</returns>
        private static int nextNonzero(double[] weights, int i)
        {
            int j = i + 1;
            while (j < weights.Length && weights[j] == 0)
            {
                j++;
            }
            return j;
        }

        /// <summary>
        /// Compute the
        /// <a href="http://en.wikipedia.org/wiki/Local_regression#Weight_function">tricube</a>
        /// weight function
        /// 
        /// </summary>
        /// <param Name="x">the argument</param>
        /// <returns>(1-|x|^3)^3</returns>
        private static double tricube(double x)
        {
            double tmp = 1 - x * x * x;
            return tmp * tmp * tmp;
        }

        /// <summary>
        /// Check that all elements of an array are finite real numbers.
        /// 
        /// </summary>
        /// <param Name="values">the values array</param>
        /// <param Name="pattern">pattern of the error message</param>
        /// <exception cref="MathArithmeticException">if one of the values is not a finite real number </exception>
        private static void checkAllFiniteReal(double[] values, String pattern)

        {
            AutoParallel.AutoParallelFor(0, values.Length, (i) =>
            {
                double x = values[i];
                if (x.IsInfinite() || Double.IsNaN(x))
                {
                    throw new MathArithmeticException(String.Format(pattern, i, x));
                }
            });
        }

        /// <summary>
        /// Check that elements of the abscissae array are in a strictly
        /// increasing order.
        /// 
        /// </summary>
        /// <param Name="xval">the abscissae array</param>
        /// <exception cref="MathArithmeticException">if the abscissae array </exception>
        /// is not in a strictly increasing order
        private static void checkStrictlyIncreasing(double[] xval)

        {
            AutoParallel.AutoParallelFor(0, xval.Length, (i) =>
            {
                if (i >= 1 && xval[i - 1] >= xval[i])
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OUT_OF_ORDER_ABSCISSA_ARRAY, i - 1, xval[i - 1], i, xval[i]));
                }
            });
        }
    }
}
