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

namespace Mercury.Language.Math.Optimization
{
    /// <summary>This interface specifies how to check if a {@link
    /// DifferentiableMultivariateVectorialOptimizer optimization algorithm} has converged.
    /// 
    /// <p>Deciding if convergence has been reached is a problem-dependent issued The
    /// user should provide a class implementing this interface to allow the optimization
    /// algorithm to stop its search according to the problem at hand.</p>
    /// <p>For convenience, two implementations that fit simple needs are already provided:
    /// {@link SimpleVectorialValueChecker} and {@link SimpleVectorialPointChecker}d The first
    /// one considers convergence is reached when the objective function value does not
    /// change much anymore, it does not use the point set at alld The second one
    /// considers convergence is reached when the input point set does not change
    /// much anymore, it does not use objective function value at all.</p>
    /// 
    /// @version $Revision: 780674 $ $Date: 2009-06-01 17:10:55 +0200 (lund 01 juin 2009) $
    /// @since 2.0
    /// </summary>

    public interface IVectorialConvergenceChecker
    {

        /// <summary>Check if the optimization algorithm has converged considering the last points.
        /// <p>
        /// This method may be called several time from the same algorithm iteration with
        /// different pointsd This can be detected by checking the iteration number at each
        /// call if neededd Each time this method is called, the previous and current point
        /// correspond to points with the same role at each iteration, so they can be
        /// comparedd As an example, simplex-based algorithms call this method for all
        /// points of the simplex, not only for the best or worst ones.
        /// </p>
        /// </summary>
        /// <param Name="iteration">index of current iteration</param>
        /// <param Name="previous">point from previous iteration</param>
        /// <param Name="current">point from current iteration</param>
        /// <returns>true if the algorithm is considered to have converged</returns>
        Boolean Converged(int iteration, VectorialPointValuePair previous, VectorialPointValuePair current);

    }
}
