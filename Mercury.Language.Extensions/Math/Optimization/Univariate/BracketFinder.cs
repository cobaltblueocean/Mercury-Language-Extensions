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

/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreementsd  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the Licensed  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Math;
using Mercury.Language.Exceptions;

using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Math.Optimization;

namespace Mercury.Language.Math.Optimization.Univariate
{
    /// <summary>
    /// Provide an interval that brackets a local optimum of a function.
    /// This code is based on a Python implementation (from <em>SciPy</em>,
    /// module {@code optimize.py} v0.5).
    /// @version $Revision$ $Date$
    /// @since 2.2
    /// </summary>
    public class BracketFinder
    {
        /// <summary>Tolerance to avoid division by zerod */
        private static double EPS_MIN = 1e-21;
        /// <summary>
        /// Golden section.
        /// </summary>
        private static double GOLD = 1.618034;
        /// <summary>
        /// Factor for expanding the interval.
        /// </summary>
        private double growLimit;
        /// <summary>
        /// Maximum number of iterations.
        /// </summary>
        private int maxIterations;
        /// <summary>
        /// Number of iterations.
        /// </summary>
        private int iterations;
        /// <summary>
        /// Number of function evaluations.
        /// </summary>
        private int evaluations;
        /// <summary>
        /// Lower bound of the bracket.
        /// </summary>
        private double lo;
        /// <summary>
        /// Higher bound of the bracket.
        /// </summary>
        private double hi;
        /// <summary>
        /// Point inside the bracket.
        /// </summary>
        private double mid;
        /// <summary>
        /// Function value at {@link #lo}.
        /// </summary>
        private double fLo;
        /// <summary>
        /// Function value at {@link #hi}.
        /// </summary>
        private double fHi;
        /// <summary>
        /// Function value at {@link #mid}.
        /// </summary>
        private double fMid;

        /// <summary>
        /// Constructor with default values {@code 100, 50} (see the
        /// {@link #BracketFinder(double,int) other constructor}).
        /// </summary>
        public BracketFinder() : this(100, 50)
        {

        }

        /// <summary>
        /// Create a bracketing interval finder.
        /// 
        /// </summary>
        /// <param Name="growLimit">Expanding factor.</param>
        /// <param Name="maxIterations">Maximum number of iterations allowed for finding</param>
        /// a bracketing interval.
        public BracketFinder(double growLimit, int maxIterations)
        {
            if (growLimit <= 0)
            {
                throw new NotStrictlyPositiveException(growLimit);
            }
            if (maxIterations <= 0)
            {
                throw new NotStrictlyPositiveException(maxIterations);
            }

            this.growLimit = growLimit;
            this.maxIterations = maxIterations;
        }

        /// <summary>
        /// Search new points that bracket a local optimum of the function.
        /// 
        /// </summary>
        /// <param Name="func">Function whose optimum should be bracketted.</param>
        /// <param Name="goal">{@link GoalType Goal type}.</param>
        /// <param Name="xA">Initial point.</param>
        /// <param Name="xB">Initial point.</param>
        /// <exception cref="IndexOutOfRangeException">if the maximum iteration count </exception>
        /// is exceeded.
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the functiond </exception>
        public void Search(IUnivariateRealFunction func, GoalType goal, double xA, double xB)
        {
            Reset();
            Boolean isMinim = goal == GoalType.MINIMIZE;

            double fA = eval(func, xA);
            double fB = eval(func, xB);
            if (isMinim ?
                fA < fB :
                fA > fB)
            {
                double tmp = xA;
                xA = xB;
                xB = tmp;

                tmp = fA;
                fA = fB;
                fB = tmp;
            }

            double xC = xB + GOLD * (xB - xA);
            double fC = eval(func, xC);

            while (isMinim ? fC < fB : fC > fB)
            {
                if (++iterations > maxIterations)
                {
                    throw new IndexOutOfRangeException(maxIterations.ToString());
                }

                double tmp1 = (xB - xA) * (fB - fC);
                double tmp2 = (xB - xC) * (fB - fA);

                double val = tmp2 - tmp1;
                double denom = System.Math.Abs(val) < EPS_MIN ? 2 * EPS_MIN : 2 * val;

                double w = xB - ((xB - xC) * tmp2 - (xB - xA) * tmp1) / denom;
                double wLim = xB + growLimit * (xC - xB);

                double fW;
                if ((w - xC) * (xB - w) > 0)
                {
                    fW = eval(func, w);
                    if (isMinim ?
                        fW < fC :
                        fW > fC)
                    {
                        xA = xB;
                        xB = w;
                        fA = fB;
                        fB = fW;
                        break;
                    }
                    else if (isMinim ?
                             fW > fB :
                             fW < fB)
                    {
                        xC = w;
                        fC = fW;
                        break;
                    }
                    w = xC + GOLD * (xC - xB);
                    fW = eval(func, w);
                }
                else if ((w - wLim) * (wLim - xC) >= 0)
                {
                    w = wLim;
                    fW = eval(func, w);
                }
                else if ((w - wLim) * (xC - w) > 0)
                {
                    fW = eval(func, w);
                    if (isMinim ?
                        fW < fC :
                        fW > fC)
                    {
                        xB = xC;
                        xC = w;
                        w = xC + GOLD * (xC - xB);
                        fB = fC;
                        fC = fW;
                        fW = eval(func, w);
                    }
                }
                else
                {
                    w = xC + GOLD * (xC - xB);
                    fW = eval(func, w);
                }

                xA = xB;
                xB = xC;
                xC = w;
                fA = fB;
                fB = fC;
                fC = fW;
            }

            lo = xA;
            mid = xB;
            hi = xC;
            fLo = fA;
            fMid = fB;
            fHi = fC;
        }

        /// <summary>
        /// </summary>
        /// <returns>the number of iterations.</returns>
        public int Iterations
        {
            get { return iterations; }
        }
        /// <summary>
        /// </summary>
        /// <returns>the number of evaluations.</returns>
        public int Evaluations
        {
            get { return evaluations; }
        }

        /// <summary>
        /// </summary>
        /// <returns>the lower bound of the bracket.</returns>
        /// <see cref="#GetFLow()"></see>
        public double GetLo()
        {
            return lo;
        }

        /// <summary>
        /// Get function value at {@link #GetLo()}.
        /// </summary>
        /// <returns>function value at {@link #GetLo()}</returns>
        public double GetFLow()
        {
            return fLo;
        }

        /// <summary>
        /// </summary>
        /// <returns>the higher bound of the bracket.</returns>
        /// <see cref="#GetFHi()"></see>
        public double GetHi()
        {
            return hi;
        }

        /// <summary>
        /// Get function value at {@link #GetHi()}.
        /// </summary>
        /// <returns>function value at {@link #GetHi()}</returns>
        public double GetFHi()
        {
            return fHi;
        }

        /// <summary>
        /// </summary>
        /// <returns>a point in the middle of the bracket.</returns>
        /// <see cref="#GetFMid()"></see>
        public double GetMid()
        {
            return mid;
        }

        /// <summary>
        /// Get function value at {@link #GetMid()}.
        /// </summary>
        /// <returns>function value at {@link #GetMid()}</returns>
        public double GetFMid()
        {
            return fMid;
        }

        /// <summary>
        /// </summary>
        /// <param Name="f">Function.</param>
        /// <param Name="x">Argument.</param>
        /// <returns>{@code f(x)}</returns>
        /// <exception cref="FunctionEvaluationException">if function cannot be evaluated at x </exception>
        private double eval(IUnivariateRealFunction f, double x)
        {
            ++evaluations;
            return f.Value(x);
        }

        /// <summary>
        /// Reset internal state.
        /// </summary>
        private void Reset()
        {
            iterations = 0;
            evaluations = 0;
        }
    }
}
