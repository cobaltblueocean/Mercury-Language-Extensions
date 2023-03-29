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
using System.Numerics;
using System.Linq;

namespace System.Numerics
{
    /// <summary>
    /// ComplexExtension Description
    /// </summary>
    public static class ComplexExtension
    {
        public static DecimalComplex ToDecimalComplex(this Complex value)
        {
            return new DecimalComplex((decimal)value.Real, (decimal)value.Imaginary);
        }

        public static DecimalComplex[] ToDecimalComplexArray(this Complex[] value)
        {
            return value.Select(x => new DecimalComplex((decimal)x.Real, (decimal)x.Imaginary)).ToArray();
        }
    }
}
