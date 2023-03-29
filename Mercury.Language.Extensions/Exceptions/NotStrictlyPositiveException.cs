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
using Mercury.Language.Exceptions;

namespace Mercury.Language.Exceptions
{
    /// <summary>
    /// Exception to be thrown when the argument is not greater than 0.
    /// </summary>
    public class NotStrictlyPositiveException : NumberIsTooSmallException
    {
        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="value">Argument</param>
        public NotStrictlyPositiveException(Double value) : base(value, 0, false)
        {

        }

        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific">Specific context where the error occurred.</param>
        /// <param name="value">Argument</param>
        public NotStrictlyPositiveException(String specific, Double value) : base(specific, value, 0, false)
        {

        }
    }
}
