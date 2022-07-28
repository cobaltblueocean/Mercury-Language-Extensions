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

namespace Mercury.Language.Exception
{
    /// <summary>
    /// Error thrown when a double value cannot be converted to a fraction
    /// in the allowed number of iterations.
    /// 
    /// @version $Revision: 983921 $ $Date: 2010-08-10 12:46:06 +0200 (mard 10 août 2010) $
    /// @since 1.2
    /// </summary>
    public class FractionConversionException : ConvergenceException
    {

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to {@link java.text.MessageFormat}.
        /// </summary>
        /// <param Name="value">double value to convert</param>
        /// <param Name="maxIterations">maximal number of iterations allowed</param>
        public FractionConversionException(double value, int maxIterations) : base(String.Format(LocalizedResources.Instance().FAILED_FRACTION_CONVERSION, value, maxIterations))
        {

        }

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to {@link java.text.MessageFormat}.
        /// </summary>
        /// <param Name="value">double value to convert</param>
        /// <param Name="p">current numerator</param>
        /// <param Name="q">current denominator</param>
        public FractionConversionException(double value, long p, long q) : base(String.Format(LocalizedResources.Instance().FRACTION_CONVERSION_OVERFLOW, value, p, q))
        {

        }
    }
}
