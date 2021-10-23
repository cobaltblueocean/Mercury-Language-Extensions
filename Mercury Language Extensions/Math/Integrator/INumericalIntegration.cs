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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math.Integrator
{
    /// <summary>
    ///   Common interface for numeric integration methods.
    /// </summary>
    /// 
    public interface INumericalIntegration : ICloneable
    {

        /// <summary>
        ///   Gets the numerically computed result of the 
        ///   definite integral for the specified function.
        /// </summary>
        /// 
        double Area { get; }

        /// <summary>
        ///   Computes the area of the function under the selected 
        ///   range. The computed value will be available at this 
        ///   class's <see cref="Area"/> property.
        /// </summary>
        /// 
        /// <returns>True if the integration method succeeds, false otherwise.</returns>
        /// 
        bool Compute();

    }

    /// <summary>
    ///   Common interface for numeric integration methods.
    /// </summary>
    /// 
    public interface INumericalIntegration<TCode> : INumericalIntegration
        where TCode : struct
    {
        /// <summary>
        ///   Get the exit code returned in the last call to the
        ///   <see cref="INumericalIntegration.Compute()"/> method.
        /// </summary>
        /// 
        TCode Status { get; }
    }
}
