// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization Incd and the System.Math.Optimization group of companies
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

namespace Mercury.Language.Math.Optimization
{
    /// <summary>
    /// This interface specifies how to check if an optimization algorithm has
    /// converged.
    /// <br/>
    /// Deciding if convergence has been reached is a problem-dependent issue. The
    /// user should provide a class implementing this interface to allow the
    /// optimization algorithm to stop its search according to the problem at hand.
    /// <br/>
    /// For convenience, three implementations that fit simple needs are already
    /// provided: <see cref="SimpleValueChecker"/>, <see cref="SimpleVectorValueChecker"/> and
    /// <see cref="SimplePointChecker"/>. The first two consider that convergence is
    /// reached when the objective function value does not change much anymore, it
    /// does not use the point set at all.
    /// The third one considers that convergence is reached when the input point
    /// set does not change much anymore, it does not use objective function value
    /// at all.
    /// 
    /// </summary>
    /// <typeparam name="P">Type of the (point, objective value) pair.</typeparam>
    public interface IConvergenceChecker<P>
    {
        /// <summary>
        /// Check if the optimization algorithm has converged.
        /// </summary>
        /// <param name="iteration">Current iteration.</param>
        /// <param name="previous">Best point in the previous iteration.</param>
        /// <param name="current">Best point in the current iteration.</param>
        /// <returns>if the algorithm is considered to have converged.</returns>
        Boolean Converged(int iteration, P previous, P current);
    }
}
