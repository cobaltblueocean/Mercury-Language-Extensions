// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Integration
{
    /// <summary>
    /// Interface for univariate real integration algorithms.
    /// </summary>
    public interface IUnivariateRealIntegrator : IConvergingAlgorithm
    {
        /// <summary>
        /// Get/Set the lower limit for the number of iterations.
        /// <p>
        /// Minimal iteration is needed to avoid false early convergence, e.g.
        /// the sample points happen to be zeroes of the function. Users can
        /// use the default value or choose one that they see as appropriate.</p>
        /// <p>
        /// A <code>ConvergenceException</code> will be thrown if this number
        /// is not met.</p>
        /// 
        /// </summary>
        /// <param name="count">minimum number of iterations</param>

        int MinimalIterationCount { get; set; }


        /// <summary>
        /// Reset the lower limit for the number of iterations to the default.
        /// <p>
        /// The default value is supplied by the implementation.</p>
        /// 
        /// </summary>
        /// <see cref="#setMinimalIterationCount(int)"></see>
        void ResetMinimalIterationCount();


        /// <summary>
        /// Integrate the function in the given interval.
        /// 
        /// </summary>
        /// <param name="f">the integrand function</param>
        /// <param name="min">the lower bound for the interval</param>
        /// <param name="max">the upper bound for the interval</param>
        /// <returns>the value of integral</returns>
        /// <exception cref="ConvergenceException">if the maximum iteration count is exceeded </exception>
        /// or the integrator detects convergence problems otherwise
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the function </exception>
        /// <exception cref="IllegalArgumentException">if min > max or the endpoints do not </exception>
        /// satisfy the requirements specified by the integrator
        double Integrate(IUnivariateRealFunction f, double min, double max);

        /// <summary>
        /// Get the result of the last run of the integrator.
        /// 
        /// </summary>
        /// <returns>the last result</returns>
        /// <exception cref="IllegalStateException">if there is no result available, either </exception>
        /// because no result was yet computed or the last attempt failed
        double Result();
    }
}
