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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Mercury.Language.Time
{
    /// <summary>
    /// AbstractDateTime Description
    /// </summary>
    public abstract class AbstractZonedDateTime : IComparable, IFormattable, IConvertible, IComparable<ZonedDateTime>, IEquatable<ZonedDateTime>
    {
        private ZonedDateTime _baseTime = new ZonedDateTime();

        protected static long MIN_SECOND = -31557014167219200L;
        protected static long MAX_SECOND = 31556889864403199L;
        //private static int NANOS_PER_SECOND = 1000000000;

        private long seconds;
        private int nanos;

        public AbstractZonedDateTime() : this(new ZonedDateTime())
        {
        }

        public AbstractZonedDateTime(ZonedDateTime now) : this(ToUnixTime(now), 0)
        {
        }

        protected AbstractZonedDateTime(long epochSecond, int nanos)
        {
            this.seconds = epochSecond;
            this.nanos = nanos;
            _baseTime = _baseTime.PlusMilliseconds(epochSecond * 1000);
            _baseTime = _baseTime.PlusMilliseconds(nanos / 1000);
        }

        public DateTime AsDateTime
        {
            get { return _baseTime.ToDateTimeUtc(); }
        }

        public ZonedDateTime AsZonedDateTime
        {
            get { return _baseTime; }
        }

        public static long ToUnixTime(ZonedDateTime date)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = DateTimeZoneProviders.Bcl.GetSystemDefault();
            LocalDate t1 = now.InZone(timeZone).Date;

            LocalDateTime local = new LocalDateTime(1970, 1, 1, 0, 0);
            ZonedDateTime epoch = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(DateTime.UtcNow)));

            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        #region Implements Interface Methods
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj != null)
                return ZonedDateTime.Comparer.Instant.Compare(_baseTime, (ZonedDateTime)obj);
            else
                throw new ArgumentException(LocalizedResources.Instance().OBJECT_IS_NOT_AN_INSTANT);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return _baseTime.ToString();
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.DateTime;
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return _baseTime.ToDateTimeUtc();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return _baseTime.ToString();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {

            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        int IComparable<ZonedDateTime>.CompareTo(ZonedDateTime other)
        {
            throw new NotImplementedException();
        }

        /// <returns>Not supported</returns>
        /// <exception cref="NotImplementedException">NotImplementedException</exception>
        bool IEquatable<ZonedDateTime>.Equals(ZonedDateTime other)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
