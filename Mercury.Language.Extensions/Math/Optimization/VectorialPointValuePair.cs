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
    /// <summary>
    /// VectorialPointValuePair Description
    /// </summary>
    public class VectorialPointValuePair
    {
        /// <summary>Point coordinatesd */
        private double[] point;

        /// <summary>Value of the objective function at the pointd */
        private double[] _value;

        /// <summary>Build a point/objective function value pair.
        /// </summary>
        /// <param Name="point">point coordinates (the built instance will store</param>
        /// a copy of the array, not the array passed as argument)
        /// <param Name="value">value of an objective function at the point</param>
        public VectorialPointValuePair(double[] point, double[] value)
        {
            this.point = (point == null) ? null : point.CloneExact();
            this._value = (value == null) ? null : value.CloneExact();
        }

        /// <summary>Build a point/objective function value pair.
        /// </summary>
        /// <param Name="point">point coordinates (the built instance will store</param>
        /// a copy of the array, not the array passed as argument)
        /// <param Name="value">value of an objective function at the point</param>
        /// <param Name="copyArray">if true, the input arrays will be copied, otherwise</param>
        /// they will be referenced
        public VectorialPointValuePair(double[] point, double[] value,
                                       Boolean copyArray)
        {
            this.point = copyArray ?
                          ((point == null) ? null : point.CloneExact()) :
                          point;
            this._value = copyArray ? ((value == null) ? null : value.CloneExact()) : value;
        }

        /// <summary>Get the point.
        /// </summary>
        /// <returns>a copy of the stored point</returns>
        public double[] Point
        {
            get { return (point == null) ? null : point.CloneExact(); }
        }

        /// <summary>Get a reference to the point.
        /// <p>This method is provided as a convenience to avoid copying
        /// the array, the elements of the array should <em>not</em> be modified.</p>
        /// </summary>
        /// <returns>a reference to the internal array storing the point</returns>
        public double[] PointReference
        {
            get { return point; }
        }

        /// <summary>Get the value of the objective function.
        /// </summary>
        /// <returns>the stored value of the objective function</returns>
        public double[] Value
        {
            get { return _value; }
            protected set { _value = value; }
        }
    }
}
