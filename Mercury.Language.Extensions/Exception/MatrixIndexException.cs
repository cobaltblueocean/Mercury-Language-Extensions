// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by Apache Software Foundation (ASF).
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreementsd  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the Licensed  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
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
    /// MatrixIndexException Description
    /// </summary>
    public class MatrixIndexException: MathRuntimeException
    {
        /// <summary>
        /// Constructs a new <code>MatrixIndexException</code> with specified
        /// formatted detail message.
        /// Message formatting is delegated to {@link java.text.MessageFormat}.
        /// </summary>
        /// <param Name="message">Message</param>
        /// @since 2.2
        public MatrixIndexException(String message) : base(message)
        {
        }

        /// <summary>
        /// Constructs a new <code>MatrixIndexException</code> with specified
        /// nested <code>Throwable</code> root cause.
        /// 
        /// </summary>
        /// <param Name="message">Message</param>
        /// <param Name="rootCause"> the exception or error that caused this exception</param>
        ///                   to be thrown.
        public MatrixIndexException(String message, SystemException rootCause) : base(message, rootCause)
        {
        }

        /// <summary>
        /// Constructs a new <code>MatrixIndexException</code> with no extra message/route cause
        /// </summary>
        /// @since 2.2
        public MatrixIndexException() : base()
        {
        }
    }
}
