// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Exception Inc.
//
// Copyright (C) 2012 - present by System.Exception Inc. and the System.Exception group of companies
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

namespace Mercury.Language.Exception
{
    /// <summary>
    /// Exception thrown when an error occurs evaluating a function.
    /// Maintains an <see cref="FunctionEvaluationException.Argument"/> property holding the input value that caused the function evaluation to fail.
    /// </summary>
    public class FunctionEvaluationException: MathArithmeticException
    {
        private double[] argument;

        /// <summary>
        /// Argument causing function evaluation failure
        /// </summary>
        public double[] Argument
        {
            get { return argument; }
        }

        /// <summary>
        /// Construct an exception indicating the argument value that caused the function evaluation to fail.
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        public FunctionEvaluationException(double argument): base(String.Format(Mercury.Language.LocalizedResources.Instance().EVALUATION_FAILED_FOR_ARGUMENT, argument))
        {
            this.argument = new double[] { argument };
        }

        /// <summary>
        /// Construct an exception indicating the argument value that caused the function evaluation to fail.
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        public FunctionEvaluationException(double[] argument): base(String.Format(Mercury.Language.LocalizedResources.Instance().EVALUATION_FAILED_FOR_ARGUMENT, argument.ToString()))
        {
            this.argument = (double[])argument.Clone();
        }

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to MessageFormat
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        /// <param name="pattern">format specifier</param>
        /// <param name="arguments">format arguments</param>
        public FunctionEvaluationException(double argument, String pattern, params Object[] arguments): base(String.Format(pattern, arguments))
        {
            this.argument = new double[] { argument };
        }

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to MessageFormat
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        /// <param name="pattern">format specifier</param>
        /// <param name="arguments">format arguments</param>
        public FunctionEvaluationException(double[] argument, String pattern, params Object[] arguments) : base(String.Format(pattern, arguments))
        {
            this.argument = (double[])argument.Clone();
        }

        /// <summary>
        /// Construct an exception indicating the argument value that caused the function evaluation to fail.
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        /// <param name="cause">the exception or error that caused this exception to be thrown</param>
        public FunctionEvaluationException(double argument, System.Exception cause): base(String.Format(Mercury.Language.LocalizedResources.Instance().EVALUATION_FAILED_FOR_ARGUMENT, argument), cause)
        {
            this.argument = new double[] { argument };
        }

        /// <summary>
        /// Construct an exception indicating the argument value that caused the function evaluation to fail.
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        /// <param name="cause">the exception or error that caused this exception to be thrown</param>
        public FunctionEvaluationException(double[] argument, System.Exception cause) : base(String.Format(Mercury.Language.LocalizedResources.Instance().EVALUATION_FAILED_FOR_ARGUMENT, argument.ToString()), cause)
        {
            this.argument = (double[])argument.Clone();
        }

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to MessageFormat
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        /// <param name="cause">the exception or error that caused this exception to be thrown</param>
        /// <param name="pattern">format specifier</param>
        /// <param name="arguments">format arguments</param>
        public FunctionEvaluationException(double argument, System.Exception cause, String pattern, params Object[] arguments) : base(String.Format(pattern, arguments), cause)
        {
            this.argument = new double[] { argument };
        }

        /// <summary>
        /// Constructs an exception with specified formatted detail message.
        /// Message formatting is delegated to MessageFormat
        /// </summary>
        /// <param name="argument">the failing function argument</param>
        /// <param name="cause">the exception or error that caused this exception to be thrown</param>
        /// <param name="pattern">format specifier</param>
        /// <param name="arguments">format arguments</param>
        public FunctionEvaluationException(double[] argument, System.Exception cause, String pattern, params Object[] arguments) : base(String.Format(pattern, arguments), cause)
        {
            this.argument = (double[])argument.Clone();
        }

    }
}
