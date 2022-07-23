// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization.Univariate Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization.Univariate Incd and the System.Math.Optimization.Univariate group of companies
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

namespace Mercury.Language.Math.Optimizer.Univariate
{
    /// <summary>
    /// This class holds a point and the value of an objective function at this point.
    /// This is a simple immutable container.
    /// </summary>
    [Serializable]
    public class UnivariatePointValuePair
    {

        #region Local Variables
        /// <summary>
        /// Point.
        /// </summary>
        private double _point;

        /// <summary>
        /// Value of the objective function at the point.
        /// </summary>
        private double _value;
        #endregion

        #region Property
        /// <summary>
        /// Get the point.
        /// </summary>
        public double Point
        {
            get { return _point; }
        }

        /// <summary>
        /// Get the value of the objective function.
        /// </summary>
        public double Value
        {
            get { return _value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Build a point/objective function value pair.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="value">Value of an objective function at the point</param>
        public UnivariatePointValuePair(double point, double value)
        {
            _point = point;
            _value = value;
        }
        #endregion

        #region Implement Methods

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods

        #endregion

    }
}
