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

using System;
using Mercury.Language.Exception;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math
{
    /// <summary>
    /// Interface for algorithms handling convergence settings.
    /// This interface only deals with convergence parameters setting, not execution of the algorithms per se.
    /// </summary>
    public interface IConvergingAlgorithm
    {
        /// <summary>
        /// Get or set the upper limit for the number of iterations.
        /// <p>
        /// Usually a high iteration count indicates convergence problems. However, the "reasonable value" varies widely for different algorithms. Users are advised to use the default value supplied by the algorithm.
        /// </p>
        /// A <see cref="ConvergenceException"/> will be thrown if this number is exceeded.</p>
        /// </summary>
        /// <param name="count">maximum number of iterations</param>
        int MaximalIterationCount { get; set; }

        /// <summary>
        /// Reset the upper limit for the number of iterations to the default.
        /// <p>
        /// The default value is supplied by the algorithm implementation.</p>
        /// </summary>
        void ResetMaximalIterationCount();

        /// <summary>
        /// Get/set the absolute accuracy.
        /// <p>
        /// The default is usually chosen so that results in the interval
        /// -10..-0.1 and +0.1..+10 can be found with a reasonable accuracy. If the
        /// expected absolute value of your results is of much smaller magnitude, set
        /// this to a smaller value.</p>
        /// <p>
        /// Algorithms are advised to do a plausibility check with the relative
        /// accuracy, but clients should not rely on this.</p>
        /// </summary>
        /// <param name="accuracy">the accuracy.</param>
        /// <exception cref="ArgumentException">if the accuracy can't be achieved by the solver or is otherwise deemed unreasonable.</exception>
        double AbsoluteAccuracy { get; set; }

        /// <summary>
        /// Reset the absolute accuracy to the default.
        /// <p>
        /// The default value is provided by the algorithm implementation.</p>
        /// </summary>
        void ResetAbsoluteAccuracy();

        /// <summary>
        /// Get/set the relative accuracy.
        /// <p>
        /// This is used to stop iterations if the absolute accuracy can't be
        /// achieved due to large values or short mantissa length.</p>
        /// <p>
        /// If this should be the primary criterion for convergence rather then a
        /// safety measure, set the absolute accuracy to a ridiculously small value.</p>
        /// </summary>
        /// <param name="accuracy">the relative accuracy.</param>
        /// <exception cref="ArgumentException">if the accuracy can't be achieved by the algorithm or is otherwise deemed unreasonable.</exception>
        double RelativeAccuracy { get; set; }

        /// <summary>
        /// Reset the relative accuracy to the default.
        /// The default value is provided by the algorithm implementation.
        /// </summary>
        void ResetRelativeAccuracy();

        /// <summary>
        /// Get the number of iterations in the last run of the algorithm.
        /// <p>
        /// This is mainly meant for testing purposes. It may occasionally
        /// help track down performance problems: if the iteration count
        /// is notoriously high, check whether the problem is evaluated
        /// properly, and whether another algorithm is more amenable to the
        /// problem.</p>
        /// </summary>
        /// <returns>the last iteration count.</returns>
        /// <exception cref="ArgumentException">if there is no result available, either because no result was yet computed or the last attempt failed.</exception>
        int IterationCount { get; }
    }
}
