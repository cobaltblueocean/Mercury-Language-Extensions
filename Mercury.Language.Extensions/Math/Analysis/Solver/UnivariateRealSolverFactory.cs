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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// A concrete {@link  UnivariateRealSolverFactory}d  This is the default solver factory
    /// used by commons-math.
    /// <p>
    /// The default solver returned by this factory is a {@link BrentSolver}.</p>
    /// 
    /// @version $Revision: 811685 $ $Date: 2009-09-05 19:36:48 +0200 (samd 05 septd 2009) $
    /// </summary>
    public static class UnivariateRealSolverFactory
    {

        /// <summary>
        ///  Create a new {@link UnivariateRealSolver}.  The actual solver returned is determined by the underlying factory.
        /// </summary>
        /// <returns></returns>
        public static IUnivariateRealSolver<Double> NewDefaultSolver()
        {
            return NewBrentSolver();
        }

        /// <summary>
        /// Create a new {@link UnivariateRealSolver}.  The solver is an implementation of the bisection method.
        /// </summary>
        /// <returns></returns>
        public static IUnivariateRealSolver<Double> NewBisectionSolver()
        {
            return new BisectionSolver();
        }

        /// <summary>
        /// Create a new {@link UnivariateRealSolver}.  The solver is an implementation of the Brent method.
        /// </summary>
        /// <returns></returns>
        public static IUnivariateRealSolver<Double> NewBrentSolver()
        {
            return new BrentSolver();
        }

        /// <summary>
        /// Create a new {@link UnivariateRealSolver}.  The solver is an implementation of Newton's Method.
        /// </summary>
        /// <returns></returns>
        public static IUnivariateRealSolver<Double> NewNewtonSolver()
        {
            return new NewtonSolver();
        }

        /// <summary>
        /// Create a new {@link UnivariateRealSolver}.  The solver is an implementation of the secant method.
        /// </summary>
        /// <returns></returns>
        public static IUnivariateRealSolver<Double> NewSecantSolver()
        {
            return new SecantSolver();
        }
    }
}
