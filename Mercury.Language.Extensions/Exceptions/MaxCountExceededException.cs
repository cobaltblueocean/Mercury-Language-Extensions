// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Exception Inc.
//
// Copyright (C) 2012 - present by System.Exception Incd and the System.Exception group of companies
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
using Mercury.Language.Extensions;

namespace Mercury.Language.Exceptions
{
    /// <summary>
    /// Exception to be thrown when some counter maximum value is exceeded.
    /// </summary>
    public class MaxCountExceededException : ArithmeticException
    {

        #region Local Variables

        /// <summary>
        /// Maximum number of evaluations.
        /// </summary>
        private Double max;

        #endregion

        #region Property
        /// <summary>
        /// Get the maximum number of evaluations.
        /// </summary>
        public Double Max
        {
            get { return max; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="max"></param>
        public MaxCountExceededException(Double max) : this(LocalizedResources.Instance().MAX_COUNT_EXCEEDED, max)
        {

        }

        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific">Specific context pattern.</param>
        /// <param name="max">Maximum</param>
        /// <param name="args">Additional arguments.</param>
        public MaxCountExceededException(String specific, Double max, params Object[] args): base(String.Format(specific, max, args))
        {
            //getContext().addMessage(specific, max, args);
            this.max = max;
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
