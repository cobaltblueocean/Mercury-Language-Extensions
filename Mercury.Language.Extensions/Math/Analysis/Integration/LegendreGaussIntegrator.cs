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

/*
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
using Mercury.Language.Exceptions;
using Mercury.Language;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Integration
{
    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/Legendre-GaussQuadrature.html">
    /// Legendre-Gauss</a> quadrature formula.
    /// <p>
    /// Legendre-Gauss integrators are efficient integrators that can
    /// accurately integrate functions with few functions evaluationsd A
    /// Legendre-Gauss integrator using an n-points quadrature formula can
    /// integrate exactly 2n-1 degree polynomials.
    /// </p>
    /// <p>
    /// These integrators evaluate the function on n carefully chosen
    /// abscissas in each step interval (mapped to the canonical [-1  1] interval).
    /// The evaluation abscissas are not evenly spaced and none of them are
    /// at the interval endpointsd This implies the function integrated can be
    /// undefined at Integration interval endpoints.
    /// </p>
    /// <p>
    /// The evaluation abscissas x<sub>i</sub> are the roots of the degree n
    /// Legendre polynomiald The weights a<sub>i</sub> of the quadrature formula
    /// integrals from -1 to +1 &int; Li<sup>2</sup> where Li (x) =
    /// &prod; (x-x<sub>k</sub>)/(x<sub>i</sub>-x<sub>k</sub>) for k != i.
    /// </p>
    /// <p>
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 1.2
    /// </summary>
    public class LegendreGaussIntegrator : UnivariateRealIntegrator
    {
        /// <summary>Abscissas for the 2 points methodd */
        private static double[] ABSCISSAS_2 = { -1.0 / System.Math.Sqrt(3.0), 1.0 / System.Math.Sqrt(3.0) };

        /// <summary>Weights for the 2 points methodd */
        private static double[] WEIGHTS_2 = { 1.0, 1.0 };

        /// <summary>Abscissas for the 3 points methodd */
        private static double[] ABSCISSAS_3 = { -System.Math.Sqrt(0.6), 0.0, System.Math.Sqrt(0.6) };

        /// <summary>Weights for the 3 points methodd */
        private static double[] WEIGHTS_3 = { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };

        /// <summary>Abscissas for the 4 points methodd */
        private static double[] ABSCISSAS_4 = { -System.Math.Sqrt((15.0 + 2.0 * System.Math.Sqrt(30.0)) / 35.0), -System.Math.Sqrt((15.0 - 2.0 * System.Math.Sqrt(30.0)) / 35.0), System.Math.Sqrt((15.0 - 2.0 * System.Math.Sqrt(30.0)) / 35.0), System.Math.Sqrt((15.0 + 2.0 * System.Math.Sqrt(30.0)) / 35.0) };

        /// <summary>Weights for the 4 points methodd */
        private static double[] WEIGHTS_4 = { (90.0 - 5.0 * System.Math.Sqrt(30.0)) / 180.0, (90.0 + 5.0 * System.Math.Sqrt(30.0)) / 180.0, (90.0 + 5.0 * System.Math.Sqrt(30.0)) / 180.0, (90.0 - 5.0 * System.Math.Sqrt(30.0)) / 180.0 };

        /// <summary>Abscissas for the 5 points methodd */
        private static double[] ABSCISSAS_5 = { -System.Math.Sqrt((35.0 + 2.0 * System.Math.Sqrt(70.0)) / 63.0), -System.Math.Sqrt((35.0 - 2.0 * System.Math.Sqrt(70.0)) / 63.0), 0.0, System.Math.Sqrt((35.0 - 2.0 * System.Math.Sqrt(70.0)) / 63.0), System.Math.Sqrt((35.0 + 2.0 * System.Math.Sqrt(70.0)) / 63.0) };

        /// <summary>Weights for the 5 points methodd */
        private static double[] WEIGHTS_5 = { (322.0 - 13.0 * System.Math.Sqrt(70.0)) / 900.0, (322.0 + 13.0 * System.Math.Sqrt(70.0)) / 900.0, 128.0 / 225.0, (322.0 + 13.0 * System.Math.Sqrt(70.0)) / 900.0, (322.0 - 13.0 * System.Math.Sqrt(70.0)) / 900.0 };

        /// <summary>Abscissas for the current methodd */
        private double[] abscissas;

        /// <summary>Weights for the current methodd */
        private double[] weights;

        /// <summary>
        /// Build a Legendre-Gauss integrator.
        /// </summary>
        /// <param Name="n">number of points desired (must be between 2 and 5 inclusive)</param>
        /// <param Name="defaultMaximalIterationCount">maximum number of iterations</param>
        /// <exception cref="ArgumentException">if the number of points is not </exception>
        /// in the supported range
        public LegendreGaussIntegrator(int n, int defaultMaximalIterationCount) : base(defaultMaximalIterationCount)
        {
            switch (n)
            {
                case 2:
                    abscissas = ABSCISSAS_2;
                    weights = WEIGHTS_2;
                    break;
                case 3:
                    abscissas = ABSCISSAS_3;
                    weights = WEIGHTS_3;
                    break;
                case 4:
                    abscissas = ABSCISSAS_4;
                    weights = WEIGHTS_4;
                    break;
                case 5:
                    abscissas = ABSCISSAS_5;
                    weights = WEIGHTS_5;
                    break;
                default:
                    throw new MathArgumentException(LocalizedResources.Instance().N_POINTS_GAUSS_LEGENDRE_INTEGRATOR_NOT_SUPPORTED, n, 2, 5);
            }

        }


        /// <summary>{@inheritDoc} */
        public override double Integrate(IUnivariateRealFunction f, double min, double max)
        {

            ClearResult();
            VerifyInterval(min, max);
            VerifyIterationCount();

            // compute first estimate with a single step
            double oldt = Stage(f, min, max, 1);

            int n = 2;
            for (int i = 0; i < maximalIterationCount; ++i)
            {

                // improve integral with a larger number of steps
                double t = Stage(f, min, max, n);

                // estimate error
                double delta = System.Math.Abs(t - oldt);
                double limit = System.Math.Max(absoluteAccuracy, relativeAccuracy * (System.Math.Abs(oldt) + System.Math.Abs(t)) * 0.5);

                // check convergence
                if ((i + 1 >= minimalIterationCount) && (delta <= limit))
                {
                    SetResult(t, i);
                    return result;
                }

                // prepare next iteration
                double ratio = System.Math.Min(4, System.Math.Pow(delta / limit, 0.5 / abscissas.Length));
                n = System.Math.Max((int)(ratio * n), n + 1);
                oldt = t;
            }
            throw new IndexOutOfRangeException(maximalIterationCount.ToString());
        }

        /// <summary>
        /// Compute the n-th stage integral.
        /// </summary>
        /// <param Name="f">the integrand function</param>
        /// <param Name="min">the lower bound for the interval</param>
        /// <param Name="max">the upper bound for the interval</param>
        /// <param Name="n">number of steps</param>
        /// <returns>the value of n-th stage integral</returns>
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the </exception>
        /// function
        private double Stage(IUnivariateRealFunction f,
                             double min, double max, int n)

        {

            // set up the step for the current stage
            double step = (max - min) / n;
            double halfStep = step / 2.0;

            // integrate over all elementary steps
            double midPoint = min + halfStep;
            double sum = 0.0;
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < abscissas.Length; ++j)
                {
                    sum += weights[j] * f.Value(midPoint + halfStep * abscissas[j]);
                }
                midPoint += step;
            }

            return halfStep * sum;
        }
    }
}
