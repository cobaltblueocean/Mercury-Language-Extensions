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
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BigInteger = System.Numerics.BigInteger;

namespace MathNet.Numerics
{
    /// <summary>
    /// Integer number theory functions for Decimal
    /// </summary>
    /// <see cref="https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/Euclid.cs"/>
    public static class Euclid2
    {
        /// <summary>
        /// Canonical Modulus. The result has the sign of the divisor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Modulus(decimal dividend, decimal divisor)
        {
            return ((dividend % divisor) + divisor) % divisor;
        }

        /// <summary>
        /// Remainder (% operator). The result has the sign of the dividend.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Remainder(decimal dividend, decimal divisor)
        {
            return dividend % divisor;
        }
    }
}
