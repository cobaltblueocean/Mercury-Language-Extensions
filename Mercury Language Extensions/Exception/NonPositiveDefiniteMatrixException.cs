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
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language.Exception
{
    /// <summary>
    ///   Non-Positive Definite Matrix Exception.
    /// </summary>
    /// 
    /// <remarks><para>The non-positive definite matrix exception is thrown in cases where a method 
    /// expects a matrix to have only positive eigenvalues, such when dealing with covariance matrices.</para>
    /// </remarks>
    /// 
    [Serializable]
    public class NonPositiveDefiniteMatrixException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonPositiveDefiniteMatrixException"/> class.
        /// </summary>
        public NonPositiveDefiniteMatrixException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonPositiveDefiniteMatrixException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// 
        public NonPositiveDefiniteMatrixException(string message) :
            base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonPositiveDefiniteMatrixException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// 
        public NonPositiveDefiniteMatrixException(string message, System.Exception innerException) :
            base(message, innerException)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="NonPositiveDefiniteMatrixException"/> class.
        /// </summary>
        /// 
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        /// 
        protected NonPositiveDefiniteMatrixException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }

    }
}
