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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Numerics;
using Conditional = System.Diagnostics.ConditionalAttribute;

namespace System.Numerics
{
    /// <summary>
    /// BigintExtension Description
    /// </summary>
    public static class BigIntegerExtension
    {
        static long LONG_MASK = 0xffffffffL;

        private const int knMaskHighBit = int.MinValue;
        private const uint kuMaskHighBit = unchecked((uint)int.MinValue);
        private const int kcbitUint = 32;
        private const int kcbitUlong = 64;
        private const int DecimalScaleFactorMask = 0x00FF0000;
        private const int DecimalSignMask = unchecked((int)0x80000000);
        private const int offset = 0;

        [Pure]
        internal static int Length(uint[] rgu)
        {
            int cu = rgu.Length;
            if (rgu[cu - 1] != 0)
                return cu;
            Contract.Assert(cu >= 2 && rgu[cu - 2] != 0);
            return cu - 1;
        }

        // For values int.MinValue < n <= int.MaxValue, the value is stored in sign
        // and _bits is null. For all other values, sign is +1 or -1 and the bits are in _bits
        internal static int _sign;
        internal static uint[] _bits;

        // We have to make a choice of how to represent int.MinValue. This is the one
        // value that fits in an int, but whose negation does not fit in an int.
        // We choose to use a large representation, so we're symmetric with respect to negation.
        private static readonly BigInteger s_bnMinInt = new BigInteger(ParseToByteArray(-1, new uint[] { kuMaskHighBit }));
        private static readonly BigInteger s_bnOneInt = new BigInteger(1);
        private static readonly BigInteger s_bnZeroInt = new BigInteger(0);
        private static readonly BigInteger s_bnMinusOneInt = new BigInteger(-1);

        [Conditional("DEBUG")]
        private static void AssertValid(this BigInteger a)
        {
            if (_bits != null)
            {
                Contract.Assert(_sign == 1 || _sign == -1 /*, "_sign must be +1 or -1 when _bits is non-null"*/);
                Contract.Assert(Length(_bits) > 0 /*, "_bits must contain at least 1 element or be null"*/);
                if (Length(_bits) == 1)
                    Contract.Assert(_bits[0] >= kuMaskHighBit /*, "Wasted space _bits[0] could have been packed into _sign"*/);
            }
            else
                Contract.Assert(_sign > int.MinValue /*, "Int32.MinValue should not be stored in the _sign field"*/);
        }

        // Do an in-place twos complement of d and also return the result.
        // "Dangerous" because it causes a mutation and needs to be used
        // with care for immutable types
        public static uint[] DangerousMakeTwosComplement(uint[] d)
        {
            // first do complement and +1 as long as carry is needed
            int i = 0;
            uint v = 0;
            for (; i < d.Length; i++)
            {
                v = ~d[i] + 1;
                d[i] = v;
                if (v != 0) { i++; break; }
            }
            if (v != 0)
            {
                // now ones complement is sufficient
                for (; i < d.Length; i++)
                {
                    d[i] = ~d[i];
                }
            }
            else
            {
                //??? this is weird
                d = Resize(d, d.Length + 1);
                d[d.Length - 1] = 1;
            }
            return d;
        }

        public static uint[] Resize(uint[] v, int len)
        {
            if (v.Length == len) return v;
            uint[] ret = new uint[len];
            int n = System.Math.Min(v.Length, len);
            for (int i = 0; i < n; i++)
            {
                ret[i] = v[i];
            }
            return ret;
        }

        private static byte[] ParseToByteArray(int n, uint[] rgu)
        {
            _sign = n;
            _bits = rgu;

            if (_bits == null && _sign == 0)
                return new byte[] { 0 };

            // We could probably make this more efficient by eliminating one of the passes.
            // The current code does one pass for uint array -> byte array conversion,
            // and then another pass to remove unneeded bytes at the top of the array.
            uint[] dwords;
            byte highByte;

            if (_bits == null)
            {
                dwords = new uint[] { (uint)_sign };
                highByte = (byte)((_sign < 0) ? 0xff : 0x00);
            }
            else if (_sign == -1)
            {
                dwords = (uint[])_bits.Clone();
                DangerousMakeTwosComplement(dwords);  // mutates dwords
                highByte = 0xff;
            }
            else
            {
                dwords = _bits;
                highByte = 0x00;
            }

            byte[] bytes = new byte[checked(4 * dwords.Length)];
            int curByte = 0;
            uint dword;
            for (int i = 0; i < dwords.Length; i++)
            {
                dword = dwords[i];
                for (int j = 0; j < 4; j++)
                {
                    bytes[curByte++] = (byte)(dword & 0xff);
                    dword >>= 8;
                }
            }

            // find highest significant byte
            int msb;
            for (msb = bytes.Length - 1; msb > 0; msb--)
            {
                if (bytes[msb] != highByte) break;
            }
            // ensure high bit is 0 if positive, 1 if negative
            bool needExtraByte = (bytes[msb] & 0x80) != (highByte & 0x80);

            byte[] trimmedBytes = new byte[msb + 1 + (needExtraByte ? 1 : 0)];
            Array.Copy(bytes, trimmedBytes, msb + 1);

            if (needExtraByte) trimmedBytes[trimmedBytes.Length - 1] = highByte;
            return trimmedBytes;
        }

        /// <summary>
        /// Compare this against half of a BigInteger object (Needed for
        /// remainder tests).
        /// Assumes no leading unnecessary zeros, which holds for results
        /// from divide().
        /// <summary>
        public static int CompareHalf(this BigInteger a, BigInteger b)
        {
            int blen = b.ToByteArray().Length;
            int len = a.ToByteArray().Length;
            if (len <= 0)
                return blen <= 0 ? 0 : -1;
            if (len > blen)
                return 1;
            if (len < blen - 1)
                return -1;
            uint[] bval = b.Value();
            uint bstart = 0;
            uint carry = 0;
            // Only 2 cases left:len == blen or len == blen - 1
            if (len != blen)
            { // len == blen - 1
                if (bval[bstart] == 1)
                {
                    ++bstart;
                    carry = 0x80000000;
                }
                else
                    return -1;
            }

            //// compare values with right-shifted values of b,
            //// carrying shifted-out bits across words
            uint[] val = a.Value();
            for (uint i = offset, j = bstart; i < len + offset;)
            {
                uint bv = bval[j++];
                long hb = ((long)(UInt64.Parse(bv.ToString()) >> 1) + carry) & LONG_MASK;
                long v = val[i++] & LONG_MASK;
                if (v != hb)
                    return v < hb ? -1 : 1;
                carry = (bv & 1) << 31; // carray will be either 0x80000000 or 0
            }
            return carry == 0 ? 0 : -1;
        }

        public static uint[] Value(this BigInteger a)
        {
            var value = a.ToByteArray();

            if (value == null)
                throw new ArgumentNullException("value");
            Contract.EndContractBlock();

            int byteCount = value.Length;
            bool isNegative = byteCount > 0 && ((value[byteCount - 1] & 0x80) == 0x80);

            // Try to conserve space as much as possible by checking for wasted leading byte[] entries 
            while (byteCount > 0 && value[byteCount - 1] == 0) byteCount--;

            if (byteCount == 0)
            {
                // BigInteger.Zero
                _sign = 0;
                _bits = null;
                AssertValid(a);
                return new uint[0];
            }


            if (byteCount <= 4)
            {
                if (isNegative)
                    _sign = unchecked((int)0xffffffff);
                else
                    _sign = 0;
                for (int i = byteCount - 1; i >= 0; i--)
                {
                    _sign <<= 8;
                    _sign |= value[i];
                }
                _bits = null;

                if (_sign < 0 && !isNegative)
                {
                    // int32 overflow
                    // example: Int64 value 2362232011 (0xCB, 0xCC, 0xCC, 0x8C, 0x0)
                    // can be naively packed into 4 bytes (due to the leading 0x0)
                    // it overflows into the int32 sign bit
                    _bits = new uint[1];
                    _bits[0] = (uint)_sign;
                    _sign = +1;
                }
                //if (_sign == Int32.MinValue)
                //    this = s_bnMinInt;
            }
            else
            {
                int unalignedBytes = byteCount % 4;
                int dwordCount = byteCount / 4 + (unalignedBytes == 0 ? 0 : 1);
                bool isZero = true;
                uint[] val = new uint[dwordCount];

                // Copy all dwords, except but don't do the last one if it's not a full four bytes
                int curDword, curByte, byteInDword;
                curByte = 3;
                for (curDword = 0; curDword < dwordCount - (unalignedBytes == 0 ? 0 : 1); curDword++)
                {
                    byteInDword = 0;
                    while (byteInDword < 4)
                    {
                        if (value[curByte] != 0x00) isZero = false;
                        val[curDword] <<= 8;
                        val[curDword] |= value[curByte];
                        curByte--;
                        byteInDword++;
                    }
                    curByte += 8;
                }

                // Copy the last dword specially if it's not aligned
                if (unalignedBytes != 0)
                {
                    if (isNegative) val[dwordCount - 1] = 0xffffffff;
                    for (curByte = byteCount - 1; curByte >= byteCount - unalignedBytes; curByte--)
                    {
                        if (value[curByte] != 0x00) isZero = false;
                        val[curDword] <<= 8;
                        val[curDword] |= value[curByte];
                    }
                }

                if (isZero)
                {
                    a = s_bnZeroInt;
                }
                else if (isNegative)
                {
                    DangerousMakeTwosComplement(val); // mutates val

                    // pack _bits to remove any wasted space after the twos complement
                    int len = val.Length;
                    while (len > 0 && val[len - 1] == 0)
                        len--;
                    if (len == 1 && ((int)(val[0])) > 0)
                    {
                        if (val[0] == 1 /* abs(-1) */)
                        {
                            //this = s_bnMinusOneInt;
                            _sign = (-1) * ((int)val[0]);
                            _bits = null;
                        }
                        else if (val[0] == kuMaskHighBit /* abs(Int32.MinValue) */)
                        {
                            //this = s_bnMinInt;
                            _sign = -1;
                            _bits = val;
                        }
                        else
                        {
                            _sign = (-1) * ((int)val[0]);
                            _bits = null;
                        }
                    }
                    else if (len != val.Length)
                    {
                        _sign = -1;
                        _bits = new uint[len];
                        Array.Copy(val, _bits, len);
                    }
                    else
                    {
                        _sign = -1;
                        _bits = val;
                    }
                }
                else
                {
                    _sign = +1;
                    _bits = val;
                }
            }

            uint[] dwords;
            uint highDWord;

            if (_bits == null)
            {
                dwords = new uint[] { (uint)_sign };
                highDWord = (_sign < 0) ? UInt32.MaxValue : 0;
            }
            else if (_sign == -1)
            {
                dwords = (uint[])_bits.Clone();
                DangerousMakeTwosComplement(dwords);  // mutates dwords
                highDWord = UInt32.MaxValue;
            }
            else
            {
                dwords = _bits;
                highDWord = 0;
            }

            // find highest significant byte
            int msb;
            for (msb = dwords.Length - 1; msb > 0; msb--)
            {
                if (dwords[msb] != highDWord) break;
            }
            // ensure high bit is 0 if positive, 1 if negative
            bool needExtraByte = (dwords[msb] & 0x80000000) != (highDWord & 0x80000000);

            uint[] trimmed = new uint[msb + 1 + (needExtraByte ? 1 : 0)];
            Array.Copy(dwords, trimmed, msb + 1);

            if (needExtraByte) trimmed[trimmed.Length - 1] = highDWord;
            return trimmed;
        }

        public static Boolean TestBit(this BigInteger i, int n)
        {
            return !(i >> n).IsEven;
        }

        public static BigDecimal ToBigDecimal(this BigInteger i, int scale)
        {
            return new BigDecimal(i, scale);
        }
    }
}
