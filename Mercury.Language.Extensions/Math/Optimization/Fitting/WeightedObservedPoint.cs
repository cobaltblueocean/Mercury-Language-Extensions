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

/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreementsd  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the Licensed  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math.Optimization.Fitting
{
    /// <summary>This class is a simple container for weighted observed point in
    /// {@link CurveFitter Curve fitting}.
    /// <p>Instances of this class are guaranteed to be immutable.</p>
    /// @version $Revision: 786479 $ $Date: 2009-06-19 14:36:16 +0200 (vend 19 juin 2009) $
    /// @since 2.0
    /// </summary>
    [Serializable]
    public class WeightedObservedPoint
    {

        /// <summary>Weight of the measurement in the fitting processd */
        private double weight;

        /// <summary>Abscissa of the pointd */
        private double x;

        /// <summary>Observed value of the function at xd */
        private double y;

        /// <summary>Simple constructor.
        /// </summary>
        /// <param Name="weight">weight of the measurement in the fitting process</param>
        /// <param Name="x">abscissa of the measurement</param>
        /// <param Name="y">ordinate of the measurement</param>
        public WeightedObservedPoint(double weight, double x, double y)
        {
            this.weight = weight;
            this.x = x;
            this.y = y;
        }

        /// <summary>Get the weight of the measurement in the fitting process.
        /// </summary>
        /// <returns>weight of the measurement in the fitting process</returns>
        public double Weight
        {
            get { return weight; }
        }

        /// <summary>Get the abscissa of the point.
        /// </summary>
        /// <returns>abscissa of the point</returns>
        public double X
        {
            get { return x; }
        }

        /// <summary>Get the observed value of the function at x.
        /// </summary>
        /// <returns>observed value of the function at x</returns>
        public double Y
        {
            get { return y; }
        }
    }
}
