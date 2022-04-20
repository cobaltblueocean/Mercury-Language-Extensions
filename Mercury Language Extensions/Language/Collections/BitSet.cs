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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace System.Collections
{
    /// <summary>
    /// Implementation for BitSet
    /// </summary>
    /// <see cref="https://stackoverflow.com/questions/14035687/what-is-the-c-sharp-equivalent-of-bitset-of-java"/>
    public class BitSet : IEnumerable, ICollection, ICloneable
    {
        private UInt32[] bits = null;
        private int _length = 0;
        private static UInt32 ONE = (UInt32)1 << 31;
        private object _syncRoot;
        private static Func<byte[], byte[]> EndianFixer = null;

        #region Constructors

        static BitSet()
        {
            if (BitConverter.IsLittleEndian) EndianFixer = (a) => a.Reverse().ToArray();
            else EndianFixer = (a) => a;
        }

        public BitSet(BitSet srcBits)
        {
            this.InitializeFrom(srcBits.ToArray());
        }

        public BitSet(BitArray srcBits)
        {
            this._length = srcBits.Count;
            this.bits = new UInt32[RequiredSize(this._length)];

            for (int i = 0; i < srcBits.Count; i++) this[i] = srcBits[i];
        }

        public BitSet(int v)
        {
            ICollection<byte> bytes = EndianFixer(BitConverter.GetBytes(v)).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(ICollection<bool> srcBits)
        {
            this.InitializeFrom(srcBits.ToArray());
        }

        public BitSet(ICollection<byte> srcBits)
        {
            InitializeFrom(srcBits);
        }

        public BitSet(ICollection<short> srcBits)
        {
            ICollection<byte> bytes = srcBits.SelectMany(v => EndianFixer(BitConverter.GetBytes(v))).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(ICollection<ushort> srcBits)
        {
            ICollection<byte> bytes = srcBits.SelectMany(v => EndianFixer(BitConverter.GetBytes(v))).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(ICollection<int> srcBits)
        {
            ICollection<byte> bytes = srcBits.SelectMany(v => EndianFixer(BitConverter.GetBytes(v))).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(ICollection<uint> srcBits)
        {
            ICollection<byte> bytes = srcBits.SelectMany(v => EndianFixer(BitConverter.GetBytes(v))).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(ICollection<long> srcBits)
        {
            ICollection<byte> bytes = srcBits.SelectMany(v => EndianFixer(BitConverter.GetBytes(v))).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(ICollection<ulong> srcBits)
        {
            ICollection<byte> bytes = srcBits.SelectMany(v => EndianFixer(BitConverter.GetBytes(v))).ToList();
            InitializeFrom(bytes);
        }

        public BitSet(int capacity, bool defaultValue = false)
        {
            this.bits = new UInt32[RequiredSize(capacity)];
            this._length = capacity;

            // Only need to do this if true, because default for all bits is false
            if (defaultValue) for (int i = 0; i < this._length; i++) this[i] = true;
        }

        private void InitializeFrom(ICollection<byte> srcBits)
        {
            this._length = srcBits.Count * 8;
            this.bits = new UInt32[RequiredSize(this._length)];
            for (int i = 0; i < srcBits.Count; i++)
            {
                uint bv = srcBits.Skip(i).Take(1).Single();
                for (int b = 0; b < 8; b++)
                {
                    bool bitVal = ((bv << b) & 0x0080) != 0;
                    int bi = 8 * i + b;
                    this[bi] = bitVal;
                }
            }
        }

        private void InitializeFrom(ICollection<bool> srcBits)
        {
            this._length = srcBits.Count;
            this.bits = new UInt32[RequiredSize(this._length)];

            int index = 0;
            foreach (var b in srcBits) this[index++] = b;
        }

        private static int RequiredSize(int bitCapacity)
        {
            return (bitCapacity + 31) >> 5;
        }

        #endregion

        public bool this[int index]
        {
            get
            {
                if (index >= _length) throw new IndexOutOfRangeException();

                int byteIndex = index >> 5;
                int bitIndex = index & 0x1f;
                return ((bits[byteIndex] << bitIndex) & ONE) != 0;
            }
            set
            {
                if (index >= _length) throw new IndexOutOfRangeException();

                int byteIndex = index >> 5;
                int bitIndex = index & 0x1f;
                if (value) bits[byteIndex] |= (ONE >> bitIndex);
                else bits[byteIndex] &= ~(ONE >> bitIndex);
            }
        }

        #region Interfaces implementation
        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            //for (int i = 0; i < _length; i++) yield return this[i];
            return this.ToArray().GetEnumerator();
        }
        #endregion
        #region ICollection
        public void CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (array.Rank != 1) throw new ArgumentException("Multidimensional array not supported");
            if (array is UInt32[]) Array.Copy(this.bits, 0, array, index, (this.Count + sizeof(UInt32) - 1) / sizeof(UInt32));
            else if (array is bool[]) Array.Copy(this.ToArray(), 0, array, index, this.Count);
            else throw new ArgumentException("Array type not supported (UInt32[] or bool[] only)");

        }

        public int Count
        {
            get { return this._length; }
            private set
            {
                if (value > this._length) Extend(value - this._length);
                else this._length = System.Math.Max(0, value);
            }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null) Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                return _syncRoot;
            }
        }
        #endregion
        #region ICloneable
        public object Clone()
        {
            return new BitSet(this);
        }
        // Not part of ICloneable, but better - returns a strongly-typed result
        public BitSet Dup()
        {
            return new BitSet(this);
        }
        #endregion 
        #endregion

        #region String Conversions

        public override string ToString()
        {
            return ToBinaryString();
            //return ToHexString(" ", " ■ ");
        }

        public static BitSet FromHexString(string hex)
        {
            if (hex == null) throw new ArgumentNullException("hex");

            List<bool> bits = new List<bool>();
            for (int i = 0; i < hex.Length; i++)
            {
                int b = byte.Parse(hex[i].ToString(), NumberStyles.HexNumber);
                bits.Add((b >> 3) == 1);
                bits.Add(((b & 0x7) >> 2) == 1);
                bits.Add(((b & 0x3) >> 1) == 1);
                bits.Add((b & 0x1) == 1);
            }
            BitSet ba = new BitSet(bits.ToArray());
            return ba;
        }

        public string ToHexString(string bitSep8 = null, string bitSep128 = null)
        {
            string s = string.Empty;
            int b = 0;
            bool[] bbits = this.ToArray();

            for (int i = 1; i <= bbits.Length; i++)
            {
                b = (b << 1) | (bbits[i - 1] ? 1 : 0);
                if (i % 4 == 0)
                {
                    s = s + string.Format("{0:x}", b);
                    b = 0;
                }

                if (i % (8 * 16) == 0)
                {
                    s = s + bitSep128;
                }
                else if (i % 8 == 0)
                {
                    s = s + bitSep8;
                }
            }
            int ebits = bbits.Length % 4;
            if (ebits != 0)
            {
                b = b << (4 - ebits);
                s = s + string.Format("{0:x}", b);
            }
            return s;
        }

        public static BitSet FromBinaryString(string bin, char[] trueChars = null)
        {
            if (trueChars == null) trueChars = new char[] { '1', 'Y', 'y', 'T', 't' };
            if (bin == null) throw new ArgumentNullException("bin");
            BitSet ba = new BitSet(bin.Length);
            for (int i = 0; i < bin.Length; i++) ba[i] = bin[i].In(trueChars);
            return ba;
        }

        public string ToBinaryString(char setChar = '1', char unsetChar = '0')
        {
            return new string(this.ToArray().Select(v => v ? setChar : unsetChar).ToArray());
        }

        #endregion

        #region Class Methods
        public bool[] ToArray()
        {
            bool[] vbits = new bool[this._length];
            for (int i = 0; i < _length; i++) vbits[i] = this[i];
            return vbits;
        }

        public BitSet Append(ICollection<bool> addBits)
        {
            int startPos = this._length;
            Extend(addBits.Count);
            bool[] bitArray = addBits.ToArray();
            for (int i = 0; i < bitArray.Length; i++) this[i + startPos] = bitArray[i];
            return this;
        }

        public BitSet Append(BitSet addBits)
        {
            return this.Append(addBits.ToArray());
        }

        public static BitSet Concatenate(params BitSet[] bArrays)
        {
            return new BitSet(bArrays.SelectMany(ba => ba.ToArray()).ToArray());
        }

        private void Extend(int numBits)
        {
            numBits += this._length;
            int reqBytes = RequiredSize(numBits);
            if (reqBytes > this.bits.Length)
            {
                UInt32[] newBits = new UInt32[reqBytes];
                this.bits.CopyTo(newBits, 0);
                this.bits = newBits;
            }
            this._length = numBits;
        }

        public bool Get(int index)
        {
            return this[index];
        }

        public BitSet GetBits(int startBit = 0, int numBits = -1)
        {
            if (numBits == -1) numBits = bits.Length;
            return new BitSet(this.ToArray().Skip(startBit).Take(numBits).ToArray());
        }

        public BitSet Repeat(int numReps)
        {
            bool[] oBits = this.ToArray();
            List<bool> nBits = new List<bool>();
            for (int i = 0; i < numReps; i++) nBits.AddRange(oBits);
            this.InitializeFrom(nBits);
            return this;
        }

        public BitSet Reverse()
        {
            int n = this.Count;
            for (int i = 0; i < n / 2; i++)
            {
                bool b1 = this[i];
                this[i] = this[n - i - 1];
                this[n - i - 1] = b1;
            }
            return this;
        }

        public BitSet Set(int bitIndex)
        {
            if (bitIndex < 0)
                throw new IndexOutOfRangeException("bitIndex < 0: " + bitIndex);

            Extend(bitIndex);
            var data = bits[bitIndex];
            data |= (uint)(1L << bitIndex);
            bits[bitIndex] = data;
            return this;
        }

        public BitSet Set(int index, bool v)
        {
            this[index] = v;
            return this;
        }

        public BitSet SetAll(bool v)
        {
            for (int i = 0; i < this.Count; i++) this[i] = v;
            return this;
        }

        public BitSet SetBits(ICollection<bool> setBits, int destStartBit = 0, int srcStartBit = 0, int numBits = -1, bool allowExtend = false)
        {
            if (setBits == null) throw new ArgumentNullException("setBits");
            if ((destStartBit < 0) || (destStartBit >= this.Count)) throw new ArgumentOutOfRangeException("destStartBit");
            if ((srcStartBit < 0) || (srcStartBit >= setBits.Count)) throw new ArgumentOutOfRangeException("srcStartBit");

            bool[] sBits;
            if (setBits is bool[]) sBits = (bool[])setBits;
            else sBits = setBits.ToArray();

            if (numBits == -1) numBits = setBits.Count;
            if (numBits > (setBits.Count - srcStartBit)) numBits = setBits.Count - srcStartBit;

            int diffSize = numBits - (this.Count - destStartBit);
            if (diffSize > 0)
            {
                if (allowExtend) Extend(diffSize);
                else numBits = this.Count - destStartBit;
            }
            for (int i = 0; i < numBits; i++) this[destStartBit + i] = sBits[srcStartBit + i];
            return this;
        }

        public List<BitSet> SplitEvery(int numBits)
        {
            int i = 0;
            List<BitSet> bitSplits = new List<BitSet>();
            while (i < this.Count)
            {
                bitSplits.Add(this.GetBits(i, numBits));
                i += numBits;
            }
            return bitSplits;
        }

        public byte[] ToBytes(int startBit = 0, int numBits = -1)
        {
            if (numBits == -1) numBits = this._length - startBit;
            BitSet ba = GetBits(startBit, numBits);
            int nb = (numBits + 7) / 8;
            byte[] bb = new byte[nb];
            for (int i = 0; i < ba.Count; i++)
            {
                if (!ba[i]) continue;
                int bp = 7 - (i % 8);
                bb[i / 8] = (byte)((int)bb[i / 8] | (1 << bp));
            }
            return bb;
        }


        #endregion

        #region Logical Bitwise Operations
        public BitSet BinaryBitwiseOp(Func<bool, bool, bool> op, BitSet ba, int start = 0)
        {
            for (int i = 0; i < ba.Count; i++)
            {
                if (start + i >= this.Count) break;
                this[start + i] = op(this[start + i], ba[i]);
            }
            return this;
        }

        public BitSet Xor(BitSet xor, int start = 0)
        {
            return BinaryBitwiseOp((a, b) => (a ^ b), xor, start);
        }

        public BitSet And(BitSet and, int start = 0)
        {
            return BinaryBitwiseOp((a, b) => (a & b), and, start);
        }

        public BitSet Or(BitSet or, int start = 0)
        {
            return BinaryBitwiseOp((a, b) => (a | b), or, start);
        }

        public BitSet Not(int start = 0, int len = -1)
        {
            for (int i = start; i < this.Count; i++)
            {
                if (--len == -1) break;
                this[i] = !this[i];
            }
            return this;
        }
        #endregion

        #region Class Operators

        public static BitSet operator +(BitSet a, BitSet b)
        {
            return a.Dup().Append(b);
        }

        public static BitSet operator |(BitSet a, BitSet b)
        {
            return a.Dup().Or(b);
        }

        public static BitSet operator &(BitSet a, BitSet b)
        {
            return a.Dup().And(b);
        }

        public static BitSet operator ^(BitSet a, BitSet b)
        {
            return a.Dup().Xor(b);
        }

        public static BitSet operator ~(BitSet a)
        {
            return a.Dup().Not();
        }

        public static BitSet operator <<(BitSet a, int shift)
        {
            return a.Dup().Append(new bool[shift]);
        }

        public static BitSet operator >>(BitSet a, int shift)
        {
            return new BitSet(a.ToArray().Take(System.Math.Max(0, a.Count - shift)).ToArray());
        }

        public static bool operator ==(BitSet a, BitSet b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++) if (a[i] != b[i]) return false;
            return true;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is BitSet)) return false;
            return (this == (BitSet)obj);
        }
        public override int GetHashCode()
        {
            return this.ToHexString().GetHashCode();
        }

        public static bool operator !=(BitSet a, BitSet b)
        {
            return !(a == b);
        }

        #endregion

    }
}
