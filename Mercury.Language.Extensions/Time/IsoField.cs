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
using NodaTime;
using OpenGamma.Utility;
using Mercury.Language.Exception;
using Range = Mercury.Language.Math.Ranges.Range;

namespace Mercury.Language.Time
{
    /// <summary>
    /// IsoFields Description
    /// </summary>
    public class IsoField : ITemporalField
    {
        private static int[] QUARTER_DAYS = { 0, 90, 181, 273, 0, 91, 182, 274 };

        #region enum members
        public static readonly IsoField DAY_OF_QUARTER = new IsoField("DayOfQuarter", ChronoUnit.DAYS, ChronoUnit.QUARTER_YEARS, new Range(1, 90, 92), new Func<ZonedDateTime, long>((temporal) =>
        {
            int doy = temporal.Get(ChronoField.DAY_OF_YEAR);
            int moy = temporal.Get(ChronoField.MONTH_OF_YEAR);
            long year = temporal.GetLong(ChronoField.YEAR);
            return doy - QUARTER_DAYS[((moy - 1) / 3) + (IsoChronology.GetInstance().IsLeapYear(year) ? 4 : 0)];
        }), new Func<LocalDate, long>((temporal) =>
        {
            int doy = (int)temporal.Get(ChronoField.DAY_OF_YEAR);
            int moy = (int)temporal.Get(ChronoField.MONTH_OF_YEAR);
            long year = temporal.GetLong(ChronoField.YEAR);
            return doy - QUARTER_DAYS[((moy - 1) / 3) + (IsoChronology.GetInstance().IsLeapYear(year) ? 4 : 0)];
        }));
        public static readonly IsoField QUARTER_OF_YEAR = new IsoField("QuarterOfYear", ChronoUnit.QUARTER_YEARS, ChronoUnit.YEARS, new Range(1, 4), new Func<ZonedDateTime, long>((temporal) =>
        {
            long moy = temporal.GetLong(ChronoField.MONTH_OF_YEAR);
            return ((moy + 2) / 3);
        }), new Func<LocalDate, long>((temporal) =>
        {
            long moy = temporal.GetLong(ChronoField.MONTH_OF_YEAR);
            return ((moy + 2) / 3);
        }));
        public static readonly IsoField WEEK_BASED_YEAR = new IsoField("WeekBasedYear", ChronoUnit.WEEK_BASED_YEARS, ChronoUnit.FOREVER, ChronoField.YEAR.Range, new Func<ZonedDateTime, long>((temporal) =>
        {
            return GetWeekBasedYear(LocalDate.FromDateTime(temporal.ToDateTimeUtc()));
        }), new Func<LocalDate, long>((temporal) =>
        {
            long moy = temporal.GetLong(ChronoField.MONTH_OF_YEAR);
            return ((moy + 2) / 3);
        }));

        #endregion

        public String Name { get; private set; }
        public ChronoUnit BaseUnit { get; private set; }
        public ChronoUnit RangeUnit { get; private set; }
        public Range Range { get; private set; }
        private Func<ZonedDateTime, long> getFunc1;
        private Func<LocalDate, long> getFunc2;

        public bool IsDateBased
        {
            get
            {
                return true;
            }
        }

        public bool IsTimeBased
        {
            get
            {
                return false;
            }
        }

        private IsoField(String name, ChronoUnit baseUnit, ChronoUnit rangeUnit, Range range, Func<ZonedDateTime, long> getFrom1, Func<LocalDate, long> getFrom2)
        {
            this.Name = name;
            this.BaseUnit = baseUnit;
            this.RangeUnit = rangeUnit;
            this.Range = range;
            this.getFunc1 = getFrom1;
            this.getFunc2 = getFrom2;
        }

        private static Range GetWeekRange(LocalDate date)
        {
            int wby = GetWeekBasedYear(date);
            return new Range(1, GetWeekRange(wby));
        }

        private static int GetWeekRange(int wby)
        {
            LocalDate date = LocalDate.FromDateTime(new DateTime(wby, 1, 1));
            // 53 weeks if standard year starts on Thursday, or Wed in a leap year
            if (date.DayOfWeek == IsoDayOfWeek.Thursday || (date.DayOfWeek == IsoDayOfWeek.Wednesday && date.IsLeapYear()))
            {
                return 53;
            }
            return 52;
        }

        private static int GetWeek(LocalDate date)
        {
            int dow0 = date.DayOfWeek.Ordinal();
            int doy0 = date.DayOfYear - 1;
            int doyThu0 = doy0 + (3 - dow0);  // adjust to mid-week Thursday (which is 3 indexed from zero)
            int alignedWeek = doyThu0 / 7;
            int firstThuDoy0 = doyThu0 - (alignedWeek * 7);
            int firstMonDoy0 = firstThuDoy0 - 3;
            if (firstMonDoy0 < -3)
            {
                firstMonDoy0 += 7;
            }
            if (doy0 < firstMonDoy0)
            {
                return (int)GetWeekRange(date.WithDayOfYear(180).PlusYears(-1)).MaxLargest;
            }
            int week = ((doy0 - firstMonDoy0) / 7) + 1;
            if (week == 53)
            {
                if ((firstMonDoy0 == -3 || (firstMonDoy0 == -2 && date.IsLeapYear())) == false)
                {
                    week = 1;
                }
            }
            return week;
        }

        private static int GetWeekBasedYear(LocalDate date)
        {
            int year = date.Year;
            int doy = date.DayOfYear;
            if (doy <= 3)
            {
                int dow = date.DayOfWeek.Ordinal();
                if (doy - dow < -2)
                {
                    year--;
                }
            }
            else if (doy >= 363)
            {
                int dow = date.DayOfWeek.Ordinal();
                doy = doy - 363 - (date.IsLeapYear() ? 1 : 0);
                if (doy - dow >= 0)
                {
                    year++;
                }
            }
            return year;
        }

        public bool IsSupportedBy(Temporal temporal)
        {
            return temporal.IsSupported(ChronoField.DAY_OF_YEAR) && temporal.IsSupported(ChronoField.MONTH_OF_YEAR) && temporal.IsSupported(ChronoField.YEAR); // && IsIso(temporal);
        }

        public long GetFrom(Temporal temporal)
        {
            if (temporal.Type == Temporal.ValueType.ZonedDateTime)
            {
                ZonedDateTime temp = temporal.GetOriginal();
                return this.getFunc1(temp);
            }
            else if (temporal.Type == Temporal.ValueType.LocalDate)
            {
                LocalDate temp = temporal.GetOriginal();
                return this.getFunc2(temp);
            }
            throw new NotSupportedException();
        }

        public Temporal AdjustInto(Temporal temporal, long newValue)
        {
            long curValue = GetFrom(temporal);
            Range.CheckValidValue(newValue, this);
            return temporal.With(ChronoField.DAY_OF_YEAR, temporal.GetLong(ChronoField.DAY_OF_YEAR) + (newValue - curValue));
        }

        public Range RangeRefinedBy(Temporal temporal)
        {
            if (temporal.IsSupported(this) == false)
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, "DayOfQuarter"));
            }
            long qoy = temporal.GetLong(IsoField.QUARTER_OF_YEAR);
            if (qoy == 1)
            {
                long year = temporal.GetLong(ChronoField.YEAR);
                return (IsoChronology.GetInstance().IsLeapYear(year) ? new Range(1, 91) : new Range(1, 90));
            }
            else if (qoy == 2)
            {
                return new Range(1, 91);
            }
            else if (qoy == 3 || qoy == 4)
            {
                return new Range(1, 92);
            } // else value not from 1 to 4, so drop through
            return Range;
        }

        //private static Boolean IsIso(Temporal temporal)
        //{
        //    System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;

        //    DateTimeZone timeZone = (new LocalDateTime()).ToZonedDateTime().Zone;
        //    ZonedDateTime dateTime = new ZonedDateTime(DateTime.UtcNow.ToInstant(), timeZone);

        //    dateTime.Calendar.

        //}
    }
}

