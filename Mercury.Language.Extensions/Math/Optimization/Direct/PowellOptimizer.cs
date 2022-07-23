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
//
// --------------------------------------------------------------------------------------
//
/// <summary>
/// Licensed to the Apache Software Foundation (ASF) under one or more
/// contributor license agreementsd  See the NOTICE file distributed with
/// this work for additional information regarding copyright ownership.
/// The ASF licenses this file to You under the Apache License, Version 2.0
/// (the "License"); you may not use this file except in compliance with
/// the Licensed  You may obtain a copy of the License at
/// 
///      http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Optimization.General;
using Mercury.Language.Math.Optimization.Univariate;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Optimization.Direct
{
    /// <summary>
    /// Powell algorithm.
    /// This code is translated and adapted from the Python version of this
    /// algorithm (as implemented in module {@code optimize.py} v0.5 of
    /// <em>SciPy</em>).
    /// 
    /// @version $Revision$ $Date$
    /// @since 2.2
    /// </summary>
    public class PowellOptimizer : ScalarDifferentiableOptimizer
    {
        /// <summary>
        /// Default relative tolerance for line search ({@value}).
        /// </summary>
        public static double DEFAULT_LS_RELATIVE_TOLERANCE = 1e-7;
        /// <summary>
        /// Default absolute tolerance for line search ({@value}).
        /// </summary>
        public static double DEFAULT_LS_ABSOLUTE_TOLERANCE = 1e-11;
        /// <summary>
        /// Line search.
        /// </summary>
        private LineSearch line;

        /// <summary>
        /// Constructor with default line search tolerances (see the
        /// {@link #PowellOptimizer(double,double) other constructor}).
        /// </summary>
        public PowellOptimizer() : this(DEFAULT_LS_RELATIVE_TOLERANCE, DEFAULT_LS_ABSOLUTE_TOLERANCE)
        {

        }

        /// <summary>
        /// Constructor with default absolute line search tolerances (see
        /// the {@link #PowellOptimizer(double,double) other constructor}).
        /// 
        /// </summary>
        /// <param Name="lsRelativeTolerance">Relative error tolerance for</param>
        /// the line search algorithm ({@link BrentOptimizer}).
        public PowellOptimizer(double lsRelativeTolerance) : this(lsRelativeTolerance, DEFAULT_LS_ABSOLUTE_TOLERANCE)
        {

        }

        /// <summary>
        /// </summary>
        /// <param Name="lsRelativeTolerance">Relative error tolerance for</param>
        /// the line search algorithm ({@link BrentOptimizer}).
        /// <param Name="lsAbsoluteTolerance">Relative error tolerance for</param>
        /// the line search algorithm ({@link BrentOptimizer}).
        public PowellOptimizer(double lsRelativeTolerance, double lsAbsoluteTolerance)
        {
            line = new LineSearch(this, lsRelativeTolerance, lsAbsoluteTolerance);
        }

        /// <summary>{@inheritDoc} */
        //@Override
        protected override RealPointValuePair doOptimize()
        {
            double[] guess = point.CloneExact();
            int n = guess.Length;

            double[][] direc = ArrayUtility.CreateJaggedArray<double>(n, n);
            for (int i = 0; i < n; i++)
            {
                direc[i][i] = 1;
            }

            double[] x = guess;
            double fVal = computeObjectiveValue(x);
            double[] x1 = x.CloneExact();
            while (true)
            {
                incrementIterationsCounter();

                double fX = fVal;
                double fX2 = 0;
                double delta = 0;
                int bigInd = 0;
                double alphaMin = 0;

                for (int i = 0; i < n; i++)
                {
                    double[] d1 = direc[i].CopyOf(n); // Java 1.5 does not support Array.CopyOf()

                    fX2 = fVal;

                    line.Search(x, d1);
                    fVal = line.getValueAtOptimum();
                    alphaMin = line.getOptimum();
                    double[][] result = newPointAndDirection(x, d1, alphaMin);
                    x = result[0];

                    if ((fX2 - fVal) > delta)
                    {
                        delta = fX2 - fVal;
                        bigInd = i;
                    }
                }

                RealPointValuePair previous = new RealPointValuePair(x1, fX);
                RealPointValuePair current = new RealPointValuePair(x, fVal);
                if (ConvergenceChecker.Converged(Iterations, previous, current))
                {
                    if (goal == GoalType.MINIMIZE)
                    {
                        return (fVal < fX) ? current : previous;
                    }
                    else
                    {
                        return (fVal > fX) ? current : previous;
                    }
                }

                double[] d = new double[n];
                double[] x2 = new double[n];
                for (int i = 0; i < n; i++)
                {
                    d[i] = x[i] - x1[i];
                    x2[i] = 2 * x[i] - x1[i];
                }


                x1 = x.CloneExact();
                fX2 = computeObjectiveValue(x2);

                if (fX > fX2)
                {
                    double t = 2 * (fX + fX2 - 2 * fVal);
                    double temp = fX - fVal - delta;
                    t *= temp * temp;
                    temp = fX - fX2;
                    t -= delta * temp * temp;

                    if (t < 0.0)
                    {
                        line.Search(x, d);
                        fVal = line.getValueAtOptimum();
                        alphaMin = line.getOptimum();
                        double[][] result = newPointAndDirection(x, d, alphaMin);
                        x = result[0];

                        int lastInd = n - 1;
                        direc[bigInd] = direc[lastInd];
                        direc[lastInd] = result[1];
                    }
                }
            }
        }

        /// <summary>
        /// Compute a new point (in the original space) and a new direction
        /// vector, resulting from the line search.
        /// The parameters {@code p} and {@code d} will be changed in-place.
        /// 
        /// </summary>
        /// <param Name="p">Point used in the line search.</param>
        /// <param Name="d">Direction used in the line search.</param>
        /// <param Name="optimum">Optimum found by the line search.</param>
        /// <returns>a 2-element array containing the new point (at index 0) and</returns>
        /// the new direction (at index 1).
        private double[][] newPointAndDirection(double[] p,                                                double[] d,                                                double optimum)
        {
            int n = p.Length;
            double[][] result = ArrayUtility.CreateJaggedArray<double>(2, n);
            double[] nP = result[0];
            double[] nD = result[1];
            for (int i = 0; i < n; i++)
            {
                nD[i] = d[i] * optimum;
                nP[i] = p[i] + nD[i];
            }
            return result;
        }

        /// <summary>
        /// Class for finding the minimum of the objective function along a given
        /// direction.
        /// </summary>
        private class LineSearch
        {
            private PowellOptimizer _base;

            /// <summary>
            /// Optimizer.
            /// </summary>
            private IUnivariateRealOptimizer optim = new BrentOptimizer();
            /// <summary>
            /// Automatic bracketing.
            /// </summary>
            private BracketFinder bracket = new BracketFinder();
            /// <summary>
            /// Value of the optimum.
            /// </summary>
            private double optimum = Double.NaN;
            /// <summary>
            /// Value of the objective function at the optimum.
            /// </summary>
            private double valueAtOptimum = Double.NaN;

            /// <summary>
            /// </summary>
            /// <param Name="relativeTolerance">Relative tolerance.</param>
            /// <param Name="absoluteTolerance">Absolute tolerance.</param>
            public LineSearch(PowellOptimizer powellOptimizer, double relativeTolerance, double absoluteTolerance)
            {
                _base = powellOptimizer;
                optim.RelativeAccuracy = relativeTolerance;
                optim.AbsoluteAccuracy = absoluteTolerance;
            }

            /// <summary>
            /// Find the minimum of the function {@code f(p + alpha * d)}.
            /// 
            /// </summary>
            /// <param Name="p">Starting point.</param>
            /// <param Name="d">Search direction.</param>
            /// <exception cref="FunctionEvaluationException">if function cannot be evaluated at some test point </exception>
            /// <exception cref="OptimizationException">if algorithm fails to converge </exception>
            public void Search(double[] p, double[] d)
            {

                // Reset.
                optimum = Double.NaN;
                valueAtOptimum = Double.NaN;

                try
                {
                    int n = p.Length;
                    IUnivariateRealFunction f = new UnivariateRealFunction()
                    {
                        function = new Func<double?, double?>((alpha) =>
                        {
                            double[] x = new double[n];
                            for (int i = 0; i < n; i++)
                            {
                                x[i] = p[i] + alpha.Value * d[i];
                            }

                            double obj;
                            obj = _base.computeObjectiveValue(x);
                            return obj;
                        })
                    };

                    bracket.Search(f, _base.goal, 0, 1);
                    optimum = optim.Optimize(f, _base.goal, bracket.GetLo(), bracket.GetHi(), bracket.GetMid());
                    valueAtOptimum = optim.FunctionValue;
                }
                catch (IndexOutOfRangeException e)
                {
                    throw new OptimizationException(e);
                }
            }

            /// <summary>
            /// </summary>
            /// <returns>the optimum.</returns>
            public double getOptimum()
            {
                return optimum;
            }
            /// <summary>
            /// </summary>
            /// <returns>the value of the function at the optimum.</returns>
            public double getValueAtOptimum()
            {
                return valueAtOptimum;
            }
        }

        /// <summary>
        /// Java 1.5 does not support Array.CopyOf()
        /// 
        /// </summary>
        /// <param Name="source">the array to be copied</param>
        /// <param Name="newLen">the Length of the copy to be returned</param>
        /// <returns>the copied array, truncated or padded as necessary.</returns>
        private double[] CopyOf(double[] source, int newLen)
        {
            double[] output = new double[newLen];
            Array.Copy(source, 0, output, 0, System.Math.Min(source.Length, newLen));
            return output;
        }
    }
}
