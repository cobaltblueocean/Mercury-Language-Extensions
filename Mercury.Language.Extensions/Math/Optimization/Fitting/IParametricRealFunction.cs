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

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>
    /// An interface representing a real function that depends on one independent
    /// variable plus some extra parameters.
    /// 
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    /// </summary>

    public interface IParametricRealFunction
    {

        /// <summary>
        /// Compute the value of the function.
        /// </summary>
        /// <param Name="x">the point for which the function value should be computed</param>
        /// <param Name="parameters">function parameters</param>
        /// <returns>the value</returns>
        /// <exception cref="FunctionEvaluationException">if the function evaluation fails </exception>
        double Value(double x, double[] parameters);

        /// <summary>
        /// Compute the gradient of the function with respect to its parameters.
        /// </summary>
        /// <param Name="x">the point for which the function value should be computed</param>
        /// <param Name="parameters">function parameters</param>
        /// <returns>the value</returns>
        /// <exception cref="FunctionEvaluationException">if the function evaluation fails </exception>
        double[] Gradient(double x, double[] parameters);
    }
}
