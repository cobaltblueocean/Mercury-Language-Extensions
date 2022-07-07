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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;

namespace System
{
    /// <summary>
    /// The BitConverter class contains methods for
    /// converting an array of bytes to one of the base data
    /// types, as well as for converting a base data type to an
    /// array of bytes.    /// </summary>
    public static class BitConverter2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe long DecimalToInt64Bits(decimal value)
        {
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            // Since System.Runtime.Intrinsics.X86 Namespace is not available in .NET Standard 2.1, unused this code.
            //if (Sse2.X64.IsSupported)
            //{
            //    Vector128<long> vec = Vector128.CreateScalarUnsafe(value).AsInt64();
            //    return Sse2.X64.ConvertToInt64(vec);
            //}

            return *((long*)&value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe decimal Int64BitsToDecimal(long value)
        {
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            // Since System.Runtime.Intrinsics.X86 Namespace is not available in .NET Standard 2.1, unused this code.
            //if (Sse2.X64.IsSupported)
            //{
            //    Vector128<double> vec = Vector128.CreateScalarUnsafe(value).AsDouble();
            //    return vec.ToScalar();
            //}

            return *((decimal*)&value);
        }


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
