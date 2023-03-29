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
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using NodaTime;
using Range = Mercury.Language.Math.Ranges.Range;

namespace Mercury.Language.Time
{
    /// <summary>
    /// JulianField Description
    /// </summary>
    public class JulianField : ITemporalField
    {

        #region enum members
        public static readonly JulianField JULIAN_DAY = new JulianField("JulianDay", ChronoUnit.DAYS, ChronoUnit.FOREVER, 2440588L);
        public static readonly JulianField MODIFIED_JULIAN_DAY = new JulianField("ModifiedJulianDay", ChronoUnit.DAYS, ChronoUnit.FOREVER, 40587L);
        public static readonly JulianField RATA_DIE = new JulianField("RataDie", ChronoUnit.DAYS, ChronoUnit.FOREVER, 719163L);
        #endregion

        private long _baseValue = 365243219162L;

        public String Name { get; private set; }
        public ChronoUnit BaseUnit { get; private set; }
        public ChronoUnit RangeUnit { get; private set; }
        public Range Range { get; private set; }
        private long _offset;

        private JulianField(String name, ChronoUnit baseUnit, ChronoUnit rangeUnit,  long offset)
        {
            this.Name = name;
            this.BaseUnit = baseUnit;
            this.RangeUnit = rangeUnit;
            this.Range = new Range(-_baseValue + offset, _baseValue + offset);
            _offset = offset;
        }

        public bool IsDateBased
        {
            get
            {
                return ((this.BaseUnit == ChronoUnit.DAYS) || (this.BaseUnit == ChronoUnit.MONTHS) || (this.BaseUnit == ChronoUnit.YEARS)) ? true : false;
            }
        }

        public bool IsTimeBased
        {
            get
            {
                return ((this.BaseUnit == ChronoUnit.NANOS)
                    || (this.BaseUnit == ChronoUnit.MICROS)
                    || (this.BaseUnit == ChronoUnit.MILLIS)
                    || (this.BaseUnit == ChronoUnit.SECONDS)
                    || (this.BaseUnit == ChronoUnit.MINUTES)
                    || (this.BaseUnit == ChronoUnit.HOURS)
                    || (this.BaseUnit == ChronoUnit.HALF_DAYS)) ? true : false;
            }
        }

        private long GetFrom(LocalDate temporal)
        {
            return temporal.GetLong(ChronoField.EPOCH_DAY) + _offset;
        }

        private long GetFrom(ZonedDateTime temporal)
        {
            return temporal.GetLong(ChronoField.EPOCH_DAY) + _offset;
        }

        public bool IsSupportedBy(Temporal temporal)
        {
            return temporal.IsSupported(ChronoField.EPOCH_DAY);
        }

        public long GetFrom(Temporal temporal)
        {
            if (temporal.Type == Temporal.ValueType.ZonedDateTime)
            {
                ZonedDateTime temp = temporal.GetOriginal();
                return temp.GetLong(ChronoField.EPOCH_DAY) + _offset;
            }
            else if (temporal.Type == Temporal.ValueType.LocalDate)
            {
                LocalDate temp = temporal.GetOriginal();
                return temp.GetLong(ChronoField.EPOCH_DAY) + _offset;
            }
            throw new NotSupportedException();
        }

        public Temporal AdjustInto(Temporal temporal, long newValue)
        {
            if (Range.IsValidValue(newValue) == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_VALUE, Name, newValue));
            }
            return (Temporal)temporal.With(ChronoField.EPOCH_DAY, Math2.SafeSubtract(newValue, _offset));
        }

        public Range RangeRefinedBy(Temporal temporal)
        {
            if (IsSupportedBy(temporal) == false)
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, this));
            }
            return Range;
        }
    }
}
