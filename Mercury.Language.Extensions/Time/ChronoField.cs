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
    /// ChronoField Description
    /// </summary>
    public sealed class ChronoField: ITemporalField
    {
        private static int YEAR_MIN_VALUE = -999999999;
        private static int YEAR_MAX_VALUE = 999999999;

        #region enum members
        public static readonly ChronoField NANO_OF_SECOND = new ChronoField("NanoOfSecond", ChronoUnit.NANOS, ChronoUnit.SECONDS, new Range(0, 999999999));
        public static readonly ChronoField NANO_OF_DAY = new ChronoField("NanoOfDay", ChronoUnit.NANOS, ChronoUnit.DAYS, new Range(0, 86400L * 1000000000L - 1));
        public static readonly ChronoField MICRO_OF_SECOND = new ChronoField("MicroOfSecond", ChronoUnit.MICROS, ChronoUnit.SECONDS, new Range(0, 999999));
        public static readonly ChronoField MICRO_OF_DAY = new ChronoField("MicroOfDay", ChronoUnit.MICROS, ChronoUnit.DAYS, new Range(0, 86400L * 1000000L - 1));
        public static readonly ChronoField MILLI_OF_SECOND = new ChronoField("MilliOfSecond", ChronoUnit.MILLIS, ChronoUnit.SECONDS, new Range(0, 999));
        public static readonly ChronoField MILLI_OF_DAY = new ChronoField("MilliOfDay", ChronoUnit.MILLIS, ChronoUnit.DAYS, new Range(0, 86400L * 1000L - 1));
        public static readonly ChronoField SECOND_OF_MINUTE = new ChronoField("SecondOfMinute", ChronoUnit.SECONDS, ChronoUnit.MINUTES, new Range(0, 59));
        public static readonly ChronoField SECOND_OF_DAY = new ChronoField("SecondOfDay", ChronoUnit.SECONDS, ChronoUnit.DAYS, new Range(0, 86400L - 1));
        public static readonly ChronoField MINUTE_OF_HOUR = new ChronoField("MinuteOfHour", ChronoUnit.MINUTES, ChronoUnit.HOURS, new Range(0, 59));
        public static readonly ChronoField MINUTE_OF_DAY = new ChronoField("MinuteOfDay", ChronoUnit.MINUTES, ChronoUnit.DAYS, new Range(0, (24 * 60) - 1));
        public static readonly ChronoField HOUR_OF_AMPM = new ChronoField("HourOfAmPm", ChronoUnit.HOURS, ChronoUnit.HALF_DAYS, new Range(0, 11));
        public static readonly ChronoField CLOCK_HOUR_OF_AMPM = new ChronoField("ClockHourOfAmPm", ChronoUnit.HOURS, ChronoUnit.HALF_DAYS, new Range(1, 12));
        public static readonly ChronoField HOUR_OF_DAY = new ChronoField("HourOfDay", ChronoUnit.HOURS, ChronoUnit.DAYS, new Range(0, 23));
        public static readonly ChronoField CLOCK_HOUR_OF_DAY = new ChronoField("ClockHourOfDay", ChronoUnit.HOURS, ChronoUnit.DAYS, new Range(1, 24));
        public static readonly ChronoField AMPM_OF_DAY = new ChronoField("AmPmOfDay", ChronoUnit.HALF_DAYS, ChronoUnit.DAYS, new Range(0, 1));
        public static readonly ChronoField DAY_OF_WEEK = new ChronoField("DayOfWeek", ChronoUnit.DAYS, ChronoUnit.WEEKS, new Range(1, 7));
        public static readonly ChronoField ALIGNED_DAY_OF_WEEK_IN_MONTH = new ChronoField("AlignedDayOfWeekInMonth", ChronoUnit.DAYS, ChronoUnit.WEEKS, new Range(1, 7));
        public static readonly ChronoField ALIGNED_DAY_OF_WEEK_IN_YEAR = new ChronoField("AlignedDayOfWeekInYear", ChronoUnit.DAYS, ChronoUnit.WEEKS, new Range(1, 7));
        public static readonly ChronoField DAY_OF_MONTH = new ChronoField("DayOfMonth", ChronoUnit.DAYS, ChronoUnit.MONTHS, new Range(1, 28, 31));
        public static readonly ChronoField DAY_OF_YEAR = new ChronoField("DayOfYear", ChronoUnit.DAYS, ChronoUnit.YEARS, new Range(1, 365, 366));
        public static readonly ChronoField EPOCH_DAY = new ChronoField("EpochDay", ChronoUnit.DAYS, ChronoUnit.FOREVER, new Range(-365243219162L, 365241780471L));
        public static readonly ChronoField ALIGNED_WEEK_OF_MONTH = new ChronoField("AlignedWeekOfMonth", ChronoUnit.WEEKS, ChronoUnit.MONTHS, new Range(1, 4, 5));
        public static readonly ChronoField ALIGNED_WEEK_OF_YEAR = new ChronoField("AlignedWeekOfYear", ChronoUnit.WEEKS, ChronoUnit.YEARS, new Range(1, 53));
        public static readonly ChronoField MONTH_OF_YEAR = new ChronoField("MonthOfYear", ChronoUnit.MONTHS, ChronoUnit.YEARS, new Range(1, 12));
        public static readonly ChronoField PROLEPTIC_MONTH = new ChronoField("ProlepticMonth", ChronoUnit.MONTHS, ChronoUnit.FOREVER, new Range(YEAR_MIN_VALUE * 12L, YEAR_MAX_VALUE * 12L + 11));
        public static readonly ChronoField YEAR_OF_ERA = new ChronoField("YearOfEra", ChronoUnit.YEARS, ChronoUnit.FOREVER, new Range(1, YEAR_MAX_VALUE, YEAR_MAX_VALUE + 1));
        public static readonly ChronoField YEAR = new ChronoField("Year", ChronoUnit.YEARS, ChronoUnit.FOREVER, new Range(YEAR_MIN_VALUE, YEAR_MAX_VALUE));
        public static readonly ChronoField ERA = new ChronoField("Era", ChronoUnit.ERAS, ChronoUnit.FOREVER, new Range(0, 1));
        public static readonly ChronoField INSTANT_SECONDS = new ChronoField("InstantSeconds", ChronoUnit.SECONDS, ChronoUnit.FOREVER, new Range(long.MinValue, long.MaxValue));
        public static readonly ChronoField OFFSET_SECONDS = new ChronoField("OffsetSeconds", ChronoUnit.SECONDS, ChronoUnit.FOREVER, new Range(-18 * 3600, 18 * 3600));
        #endregion

        public String Name { get; private set; }
        public ChronoUnit BaseUnit { get; private set; }
        public ChronoUnit RangeUnit { get; private set; }
        public Range Range { get; private set; }

        public bool IsDateBased
        {
            get {
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

        private ChronoField(String name, ChronoUnit baseUnit, ChronoUnit rangeUnit, Range range)
        {
            this.Name = name;
            this.BaseUnit = baseUnit;
            this.RangeUnit = rangeUnit;
            this.Range = range;
        }

        public bool IsSupportedBy(Temporal temporal)
        {
            throw new NotImplementedException();
        }

        public long GetFrom(Temporal temporal)
        {
            if (temporal.Type == Temporal.ValueType.ZonedDateTime)
            {
                ZonedDateTime temp = temporal.GetOriginal();
                return temp.GetLong(this);
            }
            else if (temporal.Type == Temporal.ValueType.LocalDate)
            {
                LocalDate temp = temporal.GetOriginal();
                return temp.GetLong(this);
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Checks that the specified value is valid.
        /// <p>
        /// This ArgumentCheckers that the value is within the valid range of values.
        /// The field is only used to improve the error message.
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <param name="field">the field being checked, may be null</param>
        /// <returns>the value that was passed in</returns>
        /// <see cref="Range.IsValidValue(long)"/>
        public long CheckValidValue(long value)
        {
            if (this.Range.IsValidValue(value) == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_VALUE_VALID_RANGE, this , value));
            }
            return value;
        }

        /// <summary>
        /// Checks that the specified value is valid.
        /// <p>
        /// This ArgumentCheckers that the value is within the valid range of values.
        /// The field is only used to improve the error message.
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <param name="field">the field being checked, may be null</param>
        /// <returns>the value that was passed in</returns>
        /// <see cref="Range.IsValidValue(long)"/>
        public long CheckValidIntValue(long value)
        {
            if (this.Range.IsValidIntValue(value) == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_VALUE_VALID_RANGE, this, value));
            }
            return value;
        }

        public Temporal AdjustInto(Temporal temporal, long newValue)
        {
            return temporal.With(this, newValue);
        }

        public Range RangeRefinedBy(Temporal temporal)
        {
            throw new NotImplementedException();
        }
    }
}
