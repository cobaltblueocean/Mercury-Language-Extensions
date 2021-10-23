// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
using System.Threading;

namespace System
{
    public class AtomicBoolean
    {
        private const int TRUE_VALUE = 1;
        private const int FALSE_VALUE = 0;
        private int zeroOrOne = FALSE_VALUE;

        public AtomicBoolean()
            : this(false)
        { }

        public AtomicBoolean(bool initialValue)
        {
            this.Value = initialValue;
        }

        /// <summary>
        /// Provides (non-thread-safe) access to the backing value
        /// </summary>
        public bool Value
        {
            get
            {
                return zeroOrOne == TRUE_VALUE;
            }
            set
            {
                zeroOrOne = (value ? TRUE_VALUE : FALSE_VALUE);
            }
        }

        /// <summary>
        /// Attempt changing the backing value from true to false.
        /// </summary>
        /// <returns>Whether the value was (atomically) changed from false to true.</returns>
        public bool FalseToTrue()
        {
            return SetWhen(true, false);
        }

        /// <summary>
        /// Attempt changing the backing value from false to true.
        /// </summary>
        /// <returns>Whether the value was (atomically) changed from true to false.</returns>
        public bool TrueToFalse()
        {
            return SetWhen(false, true);
        }

        /// <summary>
        /// Attempt changing from "whenValue" to "setToValue".
        /// Fails if this.Value is not "whenValue".
        /// </summary>
        /// <param name="setToValue"></param>
        /// <param name="whenValue"></param>
        /// <returns></returns>
        public bool SetWhen(bool setToValue, bool whenValue)
        {
            int comparand = whenValue ? TRUE_VALUE : FALSE_VALUE;
            int result = Interlocked.CompareExchange(ref zeroOrOne, (setToValue ? TRUE_VALUE : FALSE_VALUE), comparand);
            bool originalValue = result == TRUE_VALUE;
            return originalValue == whenValue;
        }
    }
}
