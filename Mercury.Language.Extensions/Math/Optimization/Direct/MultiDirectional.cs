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
    /// This class  :  the multi-directional direct search method.
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// </summary>
    /// <see cref="NelderMead"></see>
    /// @since 1.2
    public class MultiDirectional : DirectSearchOptimizer
    {

        /// <summary>Expansion coefficientd */
        private double khi;

        /// <summary>Contraction coefficientd */
        private double gamma;

        /// <summary>Build a multi-directional optimizer with default coefficients.
        /// <p>The default values are 2.0 for khi and 0.5 for gamma.</p>
        /// </summary>
        public MultiDirectional()
        {
            this.khi = 2.0;
            this.gamma = 0.5;
        }

        /// <summary>Build a multi-directional optimizer with specified coefficients.
        /// </summary>
        /// <param Name="khi">expansion coefficient</param>
        /// <param Name="gamma">contraction coefficient</param>
        public MultiDirectional(double khi, double gamma)
        {
            this.khi = khi;
            this.gamma = gamma;
        }

        /// <summary>{@inheritDoc} */
        //@Override
        protected override void iterateSimplex(IComparer<RealPointValuePair> comparator)
        {

            IRealConvergenceChecker checker = ConvergenceChecker;
            while (true)
            {
                incrementIterationsCounter();

                // save the original vertex
                RealPointValuePair[] original = _simplex;
                RealPointValuePair best = original[0];

                // perform a reflection step
                RealPointValuePair reflected = evaluateNewSimplex(original, 1.0, comparator);
                if (comparator.Compare(reflected, best) < 0)
                {

                    // compute the expanded simplex
                    RealPointValuePair[] reflectedSimplex = _simplex;
                    RealPointValuePair expanded = evaluateNewSimplex(original, khi, comparator);
                    if (comparator.Compare(reflected, expanded) <= 0)
                    {
                        // accept the reflected simplex
                        _simplex = reflectedSimplex;
                    }
                    return;
                }

                // compute the contracted simplex
                RealPointValuePair contracted = evaluateNewSimplex(original, gamma, comparator);
                if (comparator.Compare(contracted, best) < 0)
                {
                    // accept the contracted simplex
                    return;
                }

                // check convergence
                int iter = Iterations;
                Boolean converged = true;
                for (int i = 0; i < _simplex.Length; ++i)
                {
                    converged &= checker.Converged(iter, original[i], _simplex[i]);
                }
                if (converged)
                {
                    return;
                }
            }
        }

        /// <summary>Compute and evaluate a new simplex.
        /// </summary>
        /// <param Name="original">original simplex (to be preserved)</param>
        /// <param Name="coeff">linear coefficient</param>
        /// <param Name="comparator">comparator to use to sort simplex vertices from best to poorest</param>
        /// <returns>best point in the transformed simplex</returns>
        /// <exception cref="FunctionEvaluationException">if the function cannot be evaluated at some point </exception>
        /// <exception cref="OptimizationException">if the maximal number of evaluations is exceeded </exception>
        private RealPointValuePair evaluateNewSimplex(RealPointValuePair[] original, double coeff, IComparer<RealPointValuePair> comparator)
        {
            double[] xSmallest = original[0].PointRefrence;
            int n = xSmallest.Length;

            // create the linearly transformed simplex
            _simplex = new RealPointValuePair[n + 1];
            _simplex[0] = original[0];
            for (int i = 1; i <= n; ++i)
            {
                double[] xOriginal = original[i].PointRefrence;
                double[] xTransformed = new double[n];
                for (int j = 0; j < n; ++j)
                {
                    xTransformed[j] = xSmallest[j] + coeff * (xSmallest[j] - xOriginal[j]);
                }
                _simplex[i] = new RealPointValuePair(xTransformed, Double.NaN, false);
            }

            // evaluate it
            evaluateSimplex(comparator);
            return _simplex[0];
        }
    }
}
