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
// 
// This code is written by Nick Berardi
// https://gist.github.com/nberardi/2667136

using System;
using System.Linq;

namespace System.Numerics
{
    public struct BigDecimal : IConvertible, IFormattable, IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        public static readonly BigDecimal MinusOne = new BigDecimal(BigInteger.MinusOne, 0);
        public static readonly BigDecimal Zero = new BigDecimal(BigInteger.Zero, 0);
        public static readonly BigDecimal One = new BigDecimal(BigInteger.One, 0);

        private BigInteger _unscaledValue;
        private int _scale;
        private RoundMode _roundMode;


        //static long INFLATED = long.MinValue;
        // Half of Long.MIN_VALUE & Long.MAX_VALUE.
        //private static  long HALF_LONG_MAX_VALUE = long.MaxValue / 2;
        //private static  long HALF_LONG_MIN_VALUE = long.MinValue / 2;
        public static int Precision = 50;

        public BigInteger Mantissa {
            get { return _unscaledValue; }
        }

        public int Exponent {
            get { return _scale; }
        }

        public RoundMode Mode
        {
            get { return _roundMode; }
            set { _roundMode = value; }
        }

        public enum RoundMode
        {
            Up = 0,
            Down = 1,
            Ceiling = 2,
            Floor = 3,
            HalfUp = 4,
            HalfDown = 5,
            HalfEven = 6,
            Unnecessary = 7
        }

        public BigDecimal(double value)
            : this((decimal)value) { }

        public BigDecimal(float value)
            : this((decimal)value) { }

        public BigDecimal(decimal value): this(value, RoundMode.Unnecessary)
        {

        }

        public BigDecimal(decimal value, RoundMode mode)
        {
            var bytes = FromDecimal(value);

            var unscaledValueBytes = new byte[12];
            Array.Copy(bytes, unscaledValueBytes, unscaledValueBytes.Length);

            var unscaledValue = new BigInteger(unscaledValueBytes);
            var scale = bytes[14];

            if (bytes[15] == 128)
                unscaledValue *= BigInteger.MinusOne;

            _unscaledValue = unscaledValue;
            _scale = scale;
            _roundMode = mode;
        }

        public BigDecimal(int value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(long value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(uint value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(ulong value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(BigInteger unscaledValue, int scale): this(unscaledValue, scale, RoundMode.Unnecessary)
        {
        }

        public BigDecimal(BigInteger unscaledValue, int scale, RoundMode mode)
        {
            _unscaledValue = unscaledValue;
            _scale = scale;
            _roundMode = mode;
        }

        public BigDecimal(byte[] value): this (value, RoundMode.Unnecessary)
        {

        }

        public BigDecimal(byte[] value, RoundMode mode)
        {
            byte[] number = new byte[value.Length - 4];
            byte[] flags = new byte[4];

            Array.Copy(value, 0, number, 0, number.Length);
            Array.Copy(value, value.Length - 4, flags, 0, 4);

            _unscaledValue = new BigInteger(number);
            _scale = BitConverter.ToInt32(flags, 0);
            _roundMode = mode;
        }


        public bool IsEven { get { return _unscaledValue.IsEven; } }
        public bool IsOne { get { return _unscaledValue.IsOne; } }
        public bool IsPowerOfTwo { get { return _unscaledValue.IsPowerOfTwo; } }
        public bool IsZero { get { return _unscaledValue.IsZero; } }
        public int Sign { get { return _unscaledValue.Sign; } }

        public override string ToString()
        {
            var number = _unscaledValue.ToString("G");

            if (_scale > 0)
                return number.Insert(number.Length - _scale, ".");

            return number;
        }

        public byte[] ToByteArray()
        {
            var unscaledValue = _unscaledValue.ToByteArray();
            var scale = BitConverter.GetBytes(_scale);

            var bytes = new byte[unscaledValue.Length + scale.Length];
            Array.Copy(unscaledValue, 0, bytes, 0, unscaledValue.Length);
            Array.Copy(scale, 0, bytes, unscaledValue.Length, scale.Length);

            return bytes;
        }

        private static byte[] FromDecimal(decimal d)
        {
            byte[] bytes = new byte[16];

            int[] bits = decimal.GetBits(d);
            int lo = bits[0];
            int mid = bits[1];
            int hi = bits[2];
            int flags = bits[3];

            bytes[0] = (byte)lo;
            bytes[1] = (byte)(lo >> 8);
            bytes[2] = (byte)(lo >> 0x10);
            bytes[3] = (byte)(lo >> 0x18);
            bytes[4] = (byte)mid;
            bytes[5] = (byte)(mid >> 8);
            bytes[6] = (byte)(mid >> 0x10);
            bytes[7] = (byte)(mid >> 0x18);
            bytes[8] = (byte)hi;
            bytes[9] = (byte)(hi >> 8);
            bytes[10] = (byte)(hi >> 0x10);
            bytes[11] = (byte)(hi >> 0x18);
            bytes[12] = (byte)flags;
            bytes[13] = (byte)(flags >> 8);
            bytes[14] = (byte)(flags >> 0x10);
            bytes[15] = (byte)(flags >> 0x18);

            return bytes;
        }

        #region Operators

        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) >= 0);
        }

        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) <= 0);
        }

        public static bool operator ==(BigDecimal left, decimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BigDecimal left, decimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator >=(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) >= 0);
        }

        public static bool operator <(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator <=(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) <= 0);
        }

        public static bool operator ==(decimal left, BigDecimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(decimal left, BigDecimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator >=(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) >= 0);
        }

        public static bool operator <(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator <=(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) <= 0);
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            return new BigDecimal(left._unscaledValue * right._unscaledValue, left._scale + right._scale);
        }

        #endregion

        #region Explicity and Implicit Casts

        public static explicit operator byte(BigDecimal value) { return value.ToType<byte>(); }
        public static explicit operator sbyte(BigDecimal value) { return value.ToType<sbyte>(); }
        public static explicit operator short(BigDecimal value) { return value.ToType<short>(); }
        public static explicit operator int(BigDecimal value) { return value.ToType<int>(); }
        public static explicit operator long(BigDecimal value) { return value.ToType<long>(); }
        public static explicit operator ushort(BigDecimal value) { return value.ToType<ushort>(); }
        public static explicit operator uint(BigDecimal value) { return value.ToType<uint>(); }
        public static explicit operator ulong(BigDecimal value) { return value.ToType<ulong>(); }
        public static explicit operator float(BigDecimal value) { return value.ToType<float>(); }
        public static explicit operator double(BigDecimal value) { return value.ToType<double>(); }
        public static explicit operator decimal(BigDecimal value) { return value.ToType<decimal>(); }
        public static explicit operator BigInteger(BigDecimal value)
        {
            var scaleDivisor = BigInteger.Pow(new BigInteger(10), value._scale);
            var scaledValue = BigInteger.Divide(value._unscaledValue, scaleDivisor);
            return scaledValue;
        }

        public static implicit operator BigDecimal(byte value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(sbyte value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(short value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(int value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(long value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(ushort value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(uint value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(ulong value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(float value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(double value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(decimal value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(BigInteger value) { return new BigDecimal(value, 0); }

        #endregion

        public double ToDoubleValue()
        {
            return ToType<Double>();
        }

        public T ToType<T>() where T : struct
        {
            return (T)((IConvertible)this).ToType(typeof(T), null);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            var scaleDivisor = BigInteger.Pow(new BigInteger(10), this._scale);
            var remainder = BigInteger.Remainder(this._unscaledValue, scaleDivisor);
            var scaledValue = BigInteger.Divide(this._unscaledValue, scaleDivisor);

            if (scaledValue > new BigInteger(Decimal.MaxValue))
                throw new ArgumentOutOfRangeException("value", String.Format(Mercury.Language.LocalizedResources.Instance().BIGDECIMAL_THE_VALUE_CANNOT_FIT_INTO, this._unscaledValue, conversionType.Name));

            var leftOfDecimal = (decimal)scaledValue;
            var rightOfDecimal = ((decimal)remainder) / ((decimal)scaleDivisor);

            var value = leftOfDecimal + rightOfDecimal;
            return Convert.ChangeType(value, conversionType);
        }


        private static int LongCompareMagnitude(long x, long y)
        {
            if (x < 0)
                x = -x;
            if (y < 0)
                y = -y;
            return (x < y) ? -1 : ((x == y) ? 0 : 1);
        }

        #region Additional mathematical functions

        /// <summary>
        /// Removes trailing zeros on the mantissa
        /// </summary>
        public void Normalize()
        {
            if (Mantissa.IsZero)
            {
                _scale = 0;
            }
            else
            {
                BigInteger remainder = 0;
                while (remainder == 0)
                {
                    var shortened = BigInteger.DivRem(Mantissa, 10, out remainder);
                    if (remainder == 0)
                    {
                        _unscaledValue = shortened;
                        _scale++;
                    }
                }
            }
        }

        /// <summary>
        /// Truncate the number to the given precision by removing the least significant digits.
        /// </summary>
        /// <returns>The truncated number</returns>
        public BigDecimal Truncate(int precision)
        {
            // copy this instance (remember it's a struct)
            var shortened = this;
            // save some time because the number of digits is not needed to remove trailing zeros
            shortened.Normalize();
            // remove the least significant digits, as long as the number of digits is higher than the given Precision
            while (NumberOfDigits(shortened.Mantissa) > precision)
            {
                shortened._unscaledValue /= 10;
                shortened._scale++;
            }
            // normalize again to make sure there are no trailing zeros left
            shortened.Normalize();
            return shortened;
        }

        public BigDecimal Truncate()
        {
            return Truncate(Precision);
        }

        public BigDecimal Floor()
        {
            return Truncate(BigDecimal.NumberOfDigits(Mantissa) + Exponent);
        }

        public static int NumberOfDigits(BigInteger value)
        {
            // do not count the sign
            //return (value * value.Sign).ToString().Length;
            // faster version
            return (int)Math.Ceiling(BigInteger.Log10(value * value.Sign));
        }

        public static BigDecimal Exp(double exponent)
        {
            var tmp = (BigDecimal)1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Exp(diff);
                exponent -= diff;
            }
            return tmp * Math.Exp(exponent);
        }

        public static BigDecimal Pow(double basis, double exponent)
        {
            var tmp = (BigDecimal)1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Pow(basis, diff);
                exponent -= diff;
            }
            return tmp * Math.Pow(basis, exponent);
        }

        #endregion

        #region IConvertible Members

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(Mercury.Language.LocalizedResources.Instance().BIGDECIMAL_CANNOT_CAST_TO_CHAR);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(Mercury.Language.LocalizedResources.Instance().BIGDECIMAL_CANNOT_CAST_TO_DATETIME);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return Convert.ToString(this);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is BigDecimal))
                throw new ArgumentException(Mercury.Language.LocalizedResources.Instance().BIGDECIMAL_COMPARE_TO_OBJECT_MUST_BE_A_BIGDECIMAL, "obj");

            return CompareTo((BigDecimal)obj);
        }

        #endregion

        #region IComparable<BigDecimal> Members

        public int CompareTo(BigDecimal other)
        {
            var unscaledValueCompare = this._unscaledValue.CompareTo(other._unscaledValue);
            var scaleCompare = this._scale.CompareTo(other._scale);

            // if both are the same value, return the value
            if (unscaledValueCompare == scaleCompare)
                return unscaledValueCompare;

            // if the scales are both the same return unscaled value
            if (scaleCompare == 0)
                return unscaledValueCompare;

            var scaledValue = BigInteger.Divide(this._unscaledValue, BigInteger.Pow(new BigInteger(10), this._scale));
            var otherScaledValue = BigInteger.Divide(other._unscaledValue, BigInteger.Pow(new BigInteger(10), other._scale));

            return scaledValue.CompareTo(otherScaledValue);
        }

        #endregion

        #region IEquatable<BigDecimal> Members

        public bool Equals(BigDecimal other)
        {
            return this._scale == other._scale && this._unscaledValue == other._unscaledValue;
        }

        public override bool Equals(object obj)
        {
            return Equals((BigDecimal)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion


    }
}