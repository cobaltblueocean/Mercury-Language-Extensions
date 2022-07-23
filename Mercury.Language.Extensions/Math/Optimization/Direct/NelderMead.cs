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
//-------------------------------------------------------------------------------------
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreementsd  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the Licensed  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Math;
using Mercury.Language.Math.Optimization;


namespace Mercury.Language.Math.Optimization.Direct
{
    /// <summary>
    /// This class  :  the Nelder-Mead direct search method.
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// </summary>
    /// <see cref="MultiDirectional"></see>
    /// @since 1.2
    public class NelderMead : DirectSearchOptimizer
    {

        /// <summary>Reflection coefficientd */
        private double rho;

        /// <summary>Expansion coefficientd */
        private double khi;

        /// <summary>Contraction coefficientd */
        private double gamma;

        /// <summary>Shrinkage coefficientd */
        private double sigma;

        /// <summary>Build a Nelder-Mead optimizer with default coefficients.
        /// <p>The default coefficients are 1.0 for rho, 2.0 for khi and 0.5
        /// for both gamma and sigma.</p>
        /// </summary>
        public NelderMead()
        {
            this.rho = 1.0;
            this.khi = 2.0;
            this.gamma = 0.5;
            this.sigma = 0.5;
        }

        /// <summary>Build a Nelder-Mead optimizer with specified coefficients.
        /// </summary>
        /// <param Name="rho">reflection coefficient</param>
        /// <param Name="khi">expansion coefficient</param>
        /// <param Name="gamma">contraction coefficient</param>
        /// <param Name="sigma">shrinkage coefficient</param>
        public NelderMead(double rho, double khi, double gamma, double sigma)
        {
            this.rho = rho;
            this.khi = khi;
            this.gamma = gamma;
            this.sigma = sigma;
        }

        /// <summary>{@inheritDoc} */
        //@Override
        protected override void iterateSimplex(IComparer<RealPointValuePair> _comparator)
        {
            incrementIterationsCounter();

            // the _simplex has n+1 point if dimension is n
            int n = _simplex.Length - 1;

            // interesting values
            RealPointValuePair best = _simplex[0];
            RealPointValuePair secondBest = _simplex[n - 1];
            RealPointValuePair worst = _simplex[n];
            double[] xWorst = worst.PointRefrence;

            // compute the centroid of the best vertices
            // (dismissing the worst point at index n)
            double[] centroid = new double[n];
            for (int i = 0; i < n; ++i)
            {
                double[] x = _simplex[i].PointRefrence;
                for (int j = 0; j < n; ++j)
                {
                    centroid[j] += x[j];
                }
            }
            double scaling = 1.0 / n;
            for (int j = 0; j < n; ++j)
            {
                centroid[j] *= scaling;
            }

            // compute the reflection point
            double[] xR = new double[n];
            for (int j = 0; j < n; ++j)
            {
                xR[j] = centroid[j] + rho * (centroid[j] - xWorst[j]);
            }
            RealPointValuePair reflected = new RealPointValuePair(xR, Evaluate(xR), false);

            if ((_comparator.Compare(best, reflected) <= 0) &&
                (_comparator.Compare(reflected, secondBest) < 0))
            {
                // accept the reflected point
                ReplaceWorstPoint(reflected, _comparator);
            }
            else if (_comparator.Compare(reflected, best) < 0)
            {
                // compute the expansion point
                double[] xE = new double[n];
                for (int j = 0; j < n; ++j)
                {
                    xE[j] = centroid[j] + khi * (xR[j] - centroid[j]);
                }
                RealPointValuePair expanded = new RealPointValuePair(xE, Evaluate(xE), false);

                if (_comparator.Compare(expanded, reflected) < 0)
                {
                    // accept the expansion point
                    ReplaceWorstPoint(expanded, _comparator);
                }
                else
                {
                    // accept the reflected point
                    ReplaceWorstPoint(reflected, _comparator);
                }
            }
            else
            {
                if (_comparator.Compare(reflected, worst) < 0)
                {
                    // perform an outside contraction
                    double[] xC = new double[n];
                    for (int j = 0; j < n; ++j)
                    {
                        xC[j] = centroid[j] + gamma * (xR[j] - centroid[j]);
                    }
                    RealPointValuePair outContracted = new RealPointValuePair(xC, Evaluate(xC), false);

                    if (_comparator.Compare(outContracted, reflected) <= 0)
                    {
                        // accept the contraction point
                        ReplaceWorstPoint(outContracted, _comparator);
                        return;
                    }
                }
                else
                {

                    // perform an inside contraction
                    double[] xC = new double[n];
                    for (int j = 0; j < n; ++j)
                    {
                        xC[j] = centroid[j] - gamma * (centroid[j] - xWorst[j]);
                    }
                    RealPointValuePair inContracted = new RealPointValuePair(xC, Evaluate(xC), false);

                    if (_comparator.Compare(inContracted, worst) < 0)
                    {
                        // accept the contraction point
                        ReplaceWorstPoint(inContracted, _comparator);
                        return;
                    }
                }

                // perform a shrink
                double[] xSmallest = _simplex[0].PointRefrence;
                for (int i = 1; i < _simplex.Length; ++i)
                {
                    double[] x = _simplex[i].Point;
                    for (int j = 0; j < n; ++j)
                    {
                        x[j] = xSmallest[j] + sigma * (x[j] - xSmallest[j]);
                    }
                    _simplex[i] = new RealPointValuePair(x, Double.NaN, false);
                }
                evaluateSimplex(_comparator);
            }
        }
    }
}
