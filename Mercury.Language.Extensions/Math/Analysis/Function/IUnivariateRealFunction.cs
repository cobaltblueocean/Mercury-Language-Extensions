// Copyright (c) 2017 - presented by Kei Nakai
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

namespace Mercury.Language.Math.Analysis.Function
{
    /// <summary>
    /// An interface representing a univariate real function.
    /// <br/>
    /// When a <em>user-defined</em> function encounters an error during
    /// evaluation, the {
    ///       @link #value(double) value} method should throw a
    /// < em > user - defined </ em > unchecked exception.
    /// < br />
    /// The following code excerpt shows the recommended way to do that using
    /// a root solver as an example, but the same construct is applicable to
    /// ODE integrators or optimizers.
    /// </summary>
    public interface IUnivariateRealFunction
    {
        /// <summary>
        /// Get the argument value.
        /// Since C# doesn't have a feature to retrieve the argument values from Func<>, need to implement to keep the argument.
        /// </summary>
        double ParamValue { get; }

        /// <summary>
        /// Compute the value of the function.
        /// </summary>
        /// <param name="x">Point at which the function value should be computed.</param>
        /// <returns>the value of the function.</returns>
        double Value(double x);
    }
}
