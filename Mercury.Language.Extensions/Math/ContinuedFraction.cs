// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math Inc.
//
// Copyright (C) 2012 - present by System.Math Inc. and the System.Math group of companies
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
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
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
using Mercury.Language.Extensions;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Math
{
    /// <summary>
    /// Provides a generic means to evaluate continued fractions.  Subclasses simply
    /// provided the a and b coefficients to evaluate the continued fraction.
    ///
    /// <p>
    /// References:
    /// <ul>
    /// <li><a href="http://mathworld.wolfram.com/ContinuedFraction.html">
    /// Continued Fraction</a></li>
    /// </ul>
    /// </p>    /// </summary>
    public abstract class ContinuedFraction
    {
        private static double DEFAULT_EPSILON = 10e-9;

        protected abstract double GetA(int n, double x);

        protected abstract double GetB(int n, double x);

        public double Evaluate(double x)
        {
            return Evaluate(x, DEFAULT_EPSILON, int.MaxValue);
        }

        public double Evaluate(double x, double epsilon)
        {
            return Evaluate(x, epsilon, int.MaxValue);
        }

        public double Evaluate(double x, int maxIterations)
        {
            return Evaluate(x, DEFAULT_EPSILON, maxIterations);
        }

        public double Evaluate(double x, double epsilon, int maxIterations)

        {
            double p0 = 1.0;
            double p1 = GetA(0, x);
            double q0 = 0.0;
            double q1 = 1.0;
            double c = p1 / q1;
            int n = 0;
            double relativeError = System.Double.MaxValue;
            while (n < maxIterations && relativeError > epsilon)
            {
                ++n;
                double a = GetA(n, x);
                double b = GetB(n, x);
                double p2 = a * p1 + b * p0;
                double q2 = a * q1 + b * q0;
                Boolean infinite = false;
                if (System.Double.IsInfinity(p2) || System.Double.IsInfinity(q2))
                {
                    /*
                     * Need to scale. Try successive powers of the larger of a or b
                     * up to 5th power. Throw NonConvergenceException if one or both
                     * of p2, q2 still overflow.
                     */
                    double scaleFactor = 1d;
                    double lastScaleFactor = 1d;
                    int maxPower = 5;
                    double scale = System.Math.Max(a, b);
                    if (scale <= 0)
                    {  // Can't scale
                        throw new NonConvergenceException(
                                String.Format(LocalizedResources.Instance().CONTINUED_FRACTION_INFINITY_DIVERGENCE, x));
                    }
                    infinite = true;
                    for (int i = 0; i < maxPower; i++)
                    {
                        lastScaleFactor = scaleFactor;
                        scaleFactor *= scale;
                        if (a != 0.0 && a > b)
                        {
                            p2 = p1 / lastScaleFactor + (b / scaleFactor * p0);
                            q2 = q1 / lastScaleFactor + (b / scaleFactor * q0);
                        }
                        else if (b != 0)
                        {
                            p2 = (a / scaleFactor * p1) + p0 / lastScaleFactor;
                            q2 = (a / scaleFactor * q1) + q0 / lastScaleFactor;
                        }
                        infinite = System.Double.IsInfinity(p2) || System.Double.IsInfinity(q2);
                        if (!infinite)
                        {
                            break;
                        }
                    }
                }

                if (infinite)
                {
                    // Scaling failed
                    throw new NonConvergenceException(
                      String.Format(LocalizedResources.Instance().CONTINUED_FRACTION_INFINITY_DIVERGENCE, x));
                }

                double r = p2 / q2;

                if (System.Double.IsNaN(r))
                {
                    throw new NonConvergenceException(
                      String.Format(LocalizedResources.Instance().CONTINUED_FRACTION_NAN_DIVERGENCE, x));
                }
                relativeError = System.Math.Abs(r / c - 1.0);

                // prepare for next iteration
                c = p2 / q2;
                p0 = p1;
                p1 = p2;
                q0 = q1;
                q1 = q2;
            }

            if (n >= maxIterations)
            {
                throw new IndexOutOfRangeException(
                    String.Format(LocalizedResources.Instance().NON_CONVERGENT_CONTINUED_FRACTION, maxIterations, x));
            }
            return c;
        }
    }
}
