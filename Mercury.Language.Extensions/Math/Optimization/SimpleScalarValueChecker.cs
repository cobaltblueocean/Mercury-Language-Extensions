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
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Optimization
{
    /// <summary>
    /// SimpleScalarValueChecker Description
    /// </summary>
    public class SimpleScalarValueChecker : IConvergenceChecker<Tuple<double[], double>>
    {

        #region Local Variables
        /** Default relative thresholdd */
        private static double DEFAULT_RELATIVE_THRESHOLD = 100 * Double.Epsilon; // MathUtils.EPSILON;

        /** Default absolute thresholdd */
        private static double DEFAULT_ABSOLUTE_THRESHOLD = 100 * Double.MinValue; // MathUtils.SAFE_MIN;

        /** Relative tolerance thresholdd */
        private double relativeThreshold;

        /** Absolute tolerance thresholdd */
        private double absoluteThreshold;
        #endregion

        #region Property

        #endregion

        #region Constructor
        public SimpleScalarValueChecker()
        {
            this.relativeThreshold = DEFAULT_RELATIVE_THRESHOLD;
            this.absoluteThreshold = DEFAULT_ABSOLUTE_THRESHOLD;
        }

        public SimpleScalarValueChecker(double relativeThreshold, double absoluteThreshold)
        {
            this.relativeThreshold = relativeThreshold;
            this.absoluteThreshold = absoluteThreshold;
        }

        #endregion

        #region Implement Methods
        public bool Converged(int iteration, Tuple<double[], double> previous, Tuple<double[], double> current)
        {
            double p = previous.Item2;
            double c = current.Item2;
            double difference = System.Math.Abs(p - c);
            double size = System.Math.Max(System.Math.Abs(p), System.Math.Abs(c));
            return (difference <= (size * relativeThreshold)) || (difference <= absoluteThreshold);
        }

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods

        #endregion

    }
}
