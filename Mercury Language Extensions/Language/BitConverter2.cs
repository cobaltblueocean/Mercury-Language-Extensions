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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace System
{
    /// <summary>
    /// BitConverter2 Description
    /// </summary>
    public static class BitConverter2
    {
        public static int FloatToRawIntBits(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return BitConverter.ToInt32(bytes, 0);
        }

        public static float IntBitsToFloat(int x)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
