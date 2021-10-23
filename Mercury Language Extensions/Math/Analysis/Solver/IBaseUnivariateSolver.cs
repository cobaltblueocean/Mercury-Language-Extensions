// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Analysis.Solver Inc.
//
// Copyright (C) 2012 - present by System.Math.Analysis.Solver Inc. and the System.Math.Analysis.Solver group of companies
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

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// Interface for (univariate real) rootfinding algorithms.
    /// Implementations will search for only one zero in the given interval.
    /// 
    /// This class is not intended for use outside of the Apache Commons Math
    /// library, regular user should rely on more specific interfaces like
    /// <see cref="UnivariateSolver"/>, <see cref="PolynomialSolver"/> or <see cref="DifferentiableUnivariateSolver"/>.
    /// </summary>
    public interface IBaseUnivariateSolver
    {
        ///// <summary>
        ///// Get the maximum number of function evaluations.
        ///// </summary>
        ///// <returns>the maximum number of function evaluations.</returns>
        //int GetMaxEvaluations();

        ///// <summary>
        ///// Get the number of evaluations of the objective function.
        ///// The number of evaluations corresponds to the last call to the <see cref="Optimize"/> method.
        ///// It is 0 if the method has not been called yet.
        ///// </summary>
        ///// <returns>the number of evaluations of the objective function.</returns>
        //int GetEvaluations();

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
        /// Get the function value accuracy of the solver.  If {@code v} is
        /// a value returned by the solver for a function <see cref="f"/>,
        /// then by contract, <see cref="f(v)"/> should be less than or equal to
        /// the function value accuracy configured for the solver.
        /// </summary>
        /// <returns>the function value accuracy.</returns>
        double FunctionValueAccuracy { get; }

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
        double Solve(UnivariateFunction f, Double[] values, double min, double max);

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
        double Solve(UnivariateFunction f, Double[] values, double min, double max, double startValue);

        /// <summary>
        /// Solve for a zero in the vicinity of <see cref="startValue"/>
        /// </summary>
        /// <param name="f">f Function to solve.</param>
        /// <param name="startValue">Start value to use.</param>
        /// <returns>a value where the function is zero.</returns>
        double Solve(UnivariateFunction f, Double[] values, double startValue);
    }
}
