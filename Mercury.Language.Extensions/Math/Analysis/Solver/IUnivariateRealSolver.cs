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

/// <summary>
/// Licensed to the Apache Software Foundation (ASF) under one or more
/// contributor license agreements.  See the NOTICE file distributed with
/// this work for additional information regarding copyright ownership.
/// The ASF licenses this file to You under the Apache License, Version 2.0
/// (the "License"); you may not use this file except in compliance with
/// the License.  You may obtain a copy of the License at
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

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// Interface for (univariate real) rootfinding algorithms.
    /// <p>
    /// Implementations will search for only one zero in the given interval.</p>
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mar. 15 févr. 2011) $
    /// </summary>

    public interface IUnivariateRealSolver<T> where T: struct, IEquatable<T>, IFormattable
    {

        /// <summary>
        /// Get the absolute accuracy of the solver.  Solutions returned by the
        /// solver should be accurate to this tolerance, i.e., if &epsilon; is the
        /// absolute accuracy of the solver and <see cref="V"/> is a value returned by
        /// one of the <see cref="Solve"/> methods, then a root of the function should
        /// exist somewhere in the interval (<see cref="V"/> - &epsilon;, <see cref="V"/> + &epsilon;).
        /// </summary>
        /// <returns>the absolute accuracy.</returns>
        double AbsoluteAccuracy { get; }

        /// <summary>
        /// Get the relative accuracy of the solver.  The contract for relative
        /// accuracy is the same as <see cref="GetAbsoluteAccuracy"/>, but using
        /// relative, rather than absolute error.  If &rho; is the relative accuracy
        /// configured for a solver and <see cref="V"/> is a value returned, then a root
        /// of the function should exist somewhere in the interval
        /// (<see cref="V"/> - &rho; <see cref="V"/>, <see cref="V"/> + &rho; <see cref="V"/>).
        /// </summary>
        /// <returns></returns>
        double RelativeAccuracy { get; }

        /// <summary>
        /// Get/set the actual function value accuracy.
        /// <p>
        /// This is used to determine when an evaluated function value or some other
        /// value which is used as divisor is zero.</p>
        /// <p>
        /// This is a safety guard and it shouldn't be necessary to change this in
        /// general.</p>
        /// </summary>
        /// <returns>the accuracy</returns>
        double FunctionValueAccuracy { get; set; }

        /// <summary>
        /// Reset the actual function accuracy to the default.
        /// The default value is provided by the solver implementation.
        /// </summary>
        void ResetFunctionValueAccuracy();


        /// <summary>
        /// Get the result of the last run of the solver.
        /// 
        /// </summary>
        /// <returns>the last result.</returns>
        /// <exception cref="IllegalStateException">if there is no result available, either </exception>
        /// because no result was yet computed or the last attempt failed.
        double Result { get; }

        /// <summary>
        /// Get the result of the last run of the solver.
        /// 
        /// </summary>
        /// <returns>the value of the function at the last result.</returns>
        /// <exception cref="IllegalStateException">if there is no result available, either </exception>
        /// because no result was yet computed or the last attempt failed.
        double FunctionValue { get; }

        /// <summary>
        /// Solve for a zero root in the given interval.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="f">Function to solve.</param>
        /// <param name="min">Lower bound for the interval.</param>
        /// <param name="max">Upper bound for the interval.</param>
        /// <returns>a value where the function is zero.</returns>
        T Solve(IUnivariateRealFunction f, double min, double max);

        /// <summary>
        /// Solve for a zero in the given interval, start at <see cref="startValue"/>.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="f">f Function to solve.</param>
        /// <param name="min">Lower bound for the interval.</param>
        /// <param name="max">Upper bound for the interval.</param>
        /// <param name="startValue">Start value to use.</param>
        /// <returns>a value where the function is zero.</returns>
        T Solve(IUnivariateRealFunction f, double min, double max, double startValue);

        /// <summary>
        /// Solve for a zero in the vicinity of <see cref="startValue"/>
        /// </summary>
        /// <param name="f">f Function to solve.</param>
        /// <param name="startValue">Start value to use.</param>
        /// <returns>a value where the function is zero.</returns>
        T Solve(IUnivariateRealFunction f, double startValue);

    }
}
