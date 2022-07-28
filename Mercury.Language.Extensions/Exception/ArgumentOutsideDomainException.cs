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
    /// Error thrown when a method is called with an out of bounds argument.
    /// 
    /// @since 1.2
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// </summary>
    public class ArgumentOutsideDomainException : FunctionEvaluationException
    {

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to {@link java.text.MessageFormat}.
        /// </summary>
        /// <param Name="argument"> the failing function argument</param>
        /// <param Name="lower">lower bound of the domain</param>
        /// <param Name="upper">upper bound of the domain</param>
        public ArgumentOutsideDomainException(double argument, double lower, double upper) : base(argument, LocalizedResources.Instance().ARGUMENT_OUTSIDE_DOMAIN, argument, lower, upper)
        {

        }
    }
}
