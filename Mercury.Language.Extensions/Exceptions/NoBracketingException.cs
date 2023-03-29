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
    /// NoBracketingException Description
    /// </summary>
    public class NoBracketingException: MathArithmeticException
    {

        #region Local Variables
        /** Lower end of the intervald */
        private double lo;
        /** Higher end of the intervald */
        private double hi;
        /** Value at lower end of the intervald */
        private double fLo;
        /** Value at higher end of the intervald */
        private double fHi;
        #endregion

        #region Property
        /// <summary>
        /// Get the lower end of the interval.
        /// </summary>
        public double Lo
        {
            get
            {
                return lo;
            }
        }

        /// <summary>
        /// Get the higher end of the interval.
        /// </summary>
        public double Hi
        {
            get
            {
                return hi;
            }
        }

        /// <summary>
        /// Get the value at the lower end of the interval.
        /// </summary>
        public double FLo
        {
            get
            {
                return fLo;
            }
        }

        /// <summary>
        /// Get the value at the higher end of the interval.
        /// </summary>
        public double FHi
        {
            get
            {
                return fHi;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="lo">Lower end of the interval.</param>
        /// <param name="hi">Higher end of the interval.</param>
        /// <param name="fLo">Value at lower end of the interval.</param>
        /// <param name="fHi">Value at higher end of the interval.</param>
        public NoBracketingException(double lo, double hi, double fLo, double fHi): this(LocalizedResources.Instance().SAME_SIGN_AT_ENDPOINTS, lo, hi, fLo, fHi)
        {

        }

        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific">specific Contextual information on what caused the exception.</param>
        /// <param name="lo">Lower end of the interval.</param>
        /// <param name="hi">Higher end of the interval.</param>
        /// <param name="fLo">Value at lower end of the interval.</param>
        /// <param name="fHi">Value at higher end of the interval.</param>
        /// <param name="args">Additional arguments.</param>
        public NoBracketingException(String specific, double lo, double hi, double fLo, double fHi, params Object[] args): base(specific)
        {
            this.lo = lo;
            this.hi = hi;
            this.fLo = fLo;
            this.fHi = fHi;
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
