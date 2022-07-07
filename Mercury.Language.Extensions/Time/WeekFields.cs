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
using System.Collections.Concurrent;
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
    /// Localized definitions of the day-of-week, week-of-month and week-of-year fields.
    /// <p>
    /// A standard week is seven days long, but cultures have different definitions for some
    /// other aspects of a week. This class represents the definition of the week, for the
    /// purpose of providing <see cref="ITemporalField"/> instances.
    /// <p>
    /// WeekFields provides three fields,
    /// <see cref="DayOfWeek"/>, <see cref="WeekOfMonth"/>, and <see cref="WeekOfYear"/>
    /// that provide access to the values from any {@link Temporal temporal object}.
    /// <p>
    /// The computations for day-of-week, week-of-month, and week-of-year are based
    /// on the <see cref="ChronoField.YEAR">proleptic-year</see>,
    /// <see cref="ChronoField.MONTH_OF_YEAR">month-of-year</see>,
    /// <see cref="ChronoField.DAY_OF_MONTH">day-of-month</see>, and
    /// <see cref="ChronoField.DAY_OF_WEEK">ISO day-of-week</see> which are based on the
    /// <see cref="ChronoField.EPOCH_DAY">epoch-day</see> and the chronology.
    /// The values may not be aligned with the <see cref="ChronoField.YEAR_OF_ERA">year-of-Era</see>
    /// depending on the Chronology.
    /// <p>A week is defined by:
    /// <ul>
    /// <li>The first day-of-week.
    /// For example, the ISO-8601 standard considers Monday to be the first day-of-week.
    /// <li>The minimal number of days in the first week.
    /// For example, the ISO-8601 standard counts the first week as needing at least 4 days.
    /// </ul><p>
    /// Together these two values allow a year or month to be divided into weeks.
    /// <p>
    /// <h3>Week of Month</h3>
    /// One field is used: week-of-month.
    /// The calculation ensures that weeks never overlap a month boundary.
    /// The month is divided into periods where each period starts on the defined first day-of-week.
    /// The earliest period is referred to as week 0 if it has less than the minimal number of days
    /// and week 1 if it has at least the minimal number of days.
    /// <p>
    /// <table cellpadding="0" cellspacing="3" border="0" style="text-align: left; width: 50%;">
    /// <caption>Examples of WeekFields</caption>
    /// <tr><th>Date</th><td>Day-of-week</td>
    ///  <td>First day: Monday<br>Minimal days: 4</td><td>First day: Monday<br>Minimal days: 5</td></tr>
    /// <tr><th>2008-12-31</th><td>Wednesday</td>
    ///  <td>Week 5 of December 2008</td><td>Week 5 of December 2008</td></tr>
    /// <tr><th>2009-01-01</th><td>Thursday</td>
    ///  <td>Week 1 of January 2009</td><td>Week 0 of January 2009</td></tr>
    /// <tr><th>2009-01-04</th><td>Sunday</td>
    ///  <td>Week 1 of January 2009</td><td>Week 0 of January 2009</td></tr>
    /// <tr><th>2009-01-05</th><td>Monday</td>
    ///  <td>Week 2 of January 2009</td><td>Week 1 of January 2009</td></tr>
    /// </table>
    /// <p>
    /// <h3>Week of Year</h3>
    /// One field is used: week-of-year.
    /// The calculation ensures that weeks never overlap a year boundary.
    /// The year is divided into periods where each period starts on the defined first day-of-week.
    /// The earliest period is referred to as week 0 if it has less than the minimal number of days
    /// and week 1 if it has at least the minimal number of days.
    /// <p>
    /// This class is immutable and thread-safe.
    /// </summary>
    public sealed class WeekFields
    {
        private static ConcurrentDictionary<String, WeekFields> CACHE = new ConcurrentDictionary<String, WeekFields>();
        private static Range DAY_OF_WEEK_RANGE = new Range(1, 7);
        private static Range WEEK_OF_MONTH_RANGE = new Range(0, 1, 4, 6);
        private static Range WEEK_OF_YEAR_RANGE = new Range(0, 1, 52, 54);
        private static Range WEEK_OF_WEEK_BASED_YEAR_RANGE = new Range(1, 52, 53);
        private static Range WEEK_BASED_YEAR_RANGE = ChronoField.YEAR.Range;

        /// <summary>
        /// The first day-of-week.
        /// </summary>
        //private IsoDayOfWeek firstDayOfWeek;
        /// <summary>
        /// The minimal number of days in the first week.
        /// </summary>
        //private int minimalDays;

        /// <summary>
        /// Gets the first day-of-week.
        /// <p>
        /// The first day-of-week varies by culture.
        /// For example, the US uses Sunday, while France and the ISO-8601 standard use Monday.
        /// This method returns the first day using the standard <see cref="IsoDayOfWeek"/> enum.
        /// </summary>
        /// <returns>the first day-of-week, not null</returns>
        public IsoDayOfWeek FirstDayOfWeek { get; private set; }

        /// <summary>
        /// Gets the minimal number of days in the first week.
        /// <p>
        /// The number of days considered to define the first week of a month or year
        /// varies by culture.
        /// For example, the ISO-8601 requires 4 days (more than half a week) to
        /// be present before counting the first week.
        /// </summary>
        /// <returns>the minimal number of days in the first week of a month or year, from 1 to 7</returns>
        public int MinimalDaysInFirstWeek { get; private set; }

        /// <summary>
        /// Returns a field to access the day of week based on this <see cref="WeekFields"/>.
        /// <p>
        /// This is similar to <see cref="ChronoField.DAY_OF_WEEK"/> but uses values for
        /// the day-of-week based on this <see cref="WeekFields"/>.
        /// The days are numbered from 1 to 7 where the
        /// {@link #getFirstDayOfWeek() first day-of-week} is assigned the value 1.
        /// <p>
        /// For example, if the first day-of-week is Sunday, then that will have the
        /// value 1, with other days ranging from Monday as 2 to Saturday as 7.
        /// <p>
        /// In the resolving phase of parsing, a localized day-of-week will be converted
        /// to a standardized <see cref="ChronoField"/> day-of-week.
        /// The day-of-week must be in the valid range 1 to 7.
        /// Other fields in this class build dates using the standardized day-of-week.
        /// </summary>
        /// <returns>a field providing access to the day-of-week with localized numbering, not null</returns>
        public ITemporalField DayOfWeek
        {
            get { return dayOfWeek; }
        }

        /// <summary>
        /// Returns a field to access the week of month based on this <see cref="WeekFields"/>.
        /// <p>
        /// This represents the concept of the count of weeks within the month where weeks
        /// start on a fixed day-of-week, such as Monday.
        /// This field is typically used with <see cref="WeekFields.DayOfWeek"/>.
        /// <p>
        /// Week one (1) is the week starting on the <see cref="WeekFields.FirstDayOfWeek"/>
        /// where there are at least <see cref="WeekFields.MinimalDaysInFirstWeek"/> days in the month.
        /// Thus, week one may start up to <see cref="minimalDays"/> days before the start of the month.
        /// If the first week starts after the start of the month then the period before is week zero (0).
        /// <p>
        /// For example:<br>
        /// - if the 1st day of the month is a Monday, week one starts on the 1st and there is no week zero<br>
        /// - if the 2nd day of the month is a Monday, week one starts on the 2nd and the 1st is in week zero<br>
        /// - if the 4th day of the month is a Monday, week one starts on the 4th and the 1st to 3rd is in week zero<br>
        /// - if the 5th day of the month is a Monday, week two starts on the 5th and the 1st to 4th is in week one<br>
        /// <p>
        /// This field can be used with any calendar system.
        /// <p>
        /// In the resolving phase of parsing, a date can be created from a year,
        /// week-of-month, month-of-year and day-of-week.
        /// <p>
        /// In strict mode, all four fields are
        /// validated against their range of valid values. The week-of-month field
        /// is validated to ensure that the resulting month is the month requested.
        /// <p>
        /// In smart mode, all four fields are
        /// validated against their range of valid values. The week-of-month field
        /// is validated from 0 to 6, meaning that the resulting date can be in a
        /// different month to that specified.
        /// <p>
        /// In lenient mode, the year and day-of-week
        /// are validated against the range of valid values. The resulting date is calculated
        /// equivalent to the following four stage approach.
        /// First, create a date on the first day of the first week of January in the requested year.
        /// Then take the month-of-year, subtract one, and add the amount in months to the date.
        /// Then take the week-of-month, subtract one, and add the amount in weeks to the date.
        /// Finally, adjust to the correct day-of-week within the localized week.
        /// </summary>
        /// <returns>a field providing access to the week-of-month, not null</returns>
        public ITemporalField WeekOfMonth
        {
            get { return weekOfMonth; }
        }

        /// <summary>
        /// Returns a field to access the week of year based on this <see cref="WeekFields"/>.
        /// <p>
        /// This represents the concept of the count of weeks within the year where weeks
        /// start on a fixed day-of-week, such as Monday.
        /// This field is typically used with <see cref="WeekFields.DayOfWeek"/>.
        /// <p>
        /// Week one(1) is the week starting on the <see cref="WeekFields.FirstDayOfWeek"/>
        /// where there are at least <see cref="WeekFields.MinimalDaysInFirstWeek"/> days in the year.
        /// Thus, week one may start up to <see cref="minimalDays"/> days before the start of the year.
        /// If the first week starts after the start of the year then the period before is week zero (0).
        /// <p>
        /// For example:<br>
        /// - if the 1st day of the year is a Monday, week one starts on the 1st and there is no week zero<br>
        /// - if the 2nd day of the year is a Monday, week one starts on the 2nd and the 1st is in week zero<br>
        /// - if the 4th day of the year is a Monday, week one starts on the 4th and the 1st to 3rd is in week zero<br>
        /// - if the 5th day of the year is a Monday, week two starts on the 5th and the 1st to 4th is in week one<br>
        /// <p>
        /// This field can be used with any calendar system.
        /// <p>
        /// In the resolving phase of parsing, a date can be created from a year,
        /// week-of-year and day-of-week.
        /// <p>
        /// In strict mode, all three fields are
        /// validated against their range of valid values. The week-of-year field
        /// is validated to ensure that the resulting year is the year requested.
        /// <p>
        /// In smart mode, all three fields are
        /// validated against their range of valid values. The week-of-year field
        /// is validated from 0 to 54, meaning that the resulting date can be in a
        /// different year to that specified.
        /// <p>
        /// In lenient mode, the year and day-of-week
        /// are validated against the range of valid values. The resulting date is calculated
        /// equivalent to the following three stage approach.
        /// First, create a date on the first day of the first week in the requested year.
        /// Then take the week-of-year, subtract one, and add the amount in weeks to the date.
        /// Finally, adjust to the correct day-of-week within the localized week.
        /// </summary>
        /// <returns>a field providing access to the week-of-year, not null</returns>
        public ITemporalField WeekOfYear
        {
            get { return weekOfYear; }
        }

        /// <summary>
        /// Returns a field to access the week of a week-based-year based on this <see cref="WeekFields"/>.
        /// <p>
        /// This represents the concept of the count of weeks within the year where weeks
        /// start on a fixed day-of-week, such as Monday and each week belongs to exactly one year.
        /// This field is typically used with <see cref="WeekFields.DayOfWeek"/> and
        /// <see cref="WeekFields.WeekBasedYear"/>.
        /// <p>
        /// Week one(1) is the week starting on the <see cref="WeekFields.FirstDayOfWeek"/>
        /// where there are at least <see cref="WeekFields.MinimalDaysInFirstWeek"/> days in the year.
        /// If the first week starts after the start of the year then the period before
        /// is in the last week of the previous year.
        /// <p>
        /// For example:<br>
        /// - if the 1st day of the year is a Monday, week one starts on the 1st<br>
        /// - if the 2nd day of the year is a Monday, week one starts on the 2nd and
        ///   the 1st is in the last week of the previous year<br>
        /// - if the 4th day of the year is a Monday, week one starts on the 4th and
        ///   the 1st to 3rd is in the last week of the previous year<br>
        /// - if the 5th day of the year is a Monday, week two starts on the 5th and
        ///   the 1st to 4th is in week one<br>
        /// <p>
        /// This field can be used with any calendar system.
        /// <p>
        /// In the resolving phase of parsing, a date can be created from a week-based-year,
        /// week-of-year and day-of-week.
        /// <p>
        /// In strict mode, all three fields are
        /// validated against their range of valid values. The week-of-year field
        /// is validated to ensure that the resulting week-based-year is the
        /// week-based-year requested.
        /// <p>
        /// In smart mode, all three fields are
        /// validated against their range of valid values. The week-of-week-based-year field
        /// is validated from 1 to 53, meaning that the resulting date can be in the
        /// following week-based-year to that specified.
        /// <p>
        /// In lenient mode, the year and day-of-week
        /// are validated against the range of valid values. The resulting date is calculated
        /// equivalent to the following three stage approach.
        /// First, create a date on the first day of the first week in the requested week-based-year.
        /// Then take the week-of-week-based-year, subtract one, and add the amount in weeks to the date.
        /// Finally, adjust to the correct day-of-week within the localized week.
        /// </summary>
        /// <returns>a field providing access to the week-of-week-based-year, not null</returns>
        public ITemporalField WeekOfWeekBasedYear
        {
            get { return weekOfWeekBasedYear; }
        }

        /// <summary>
        /// Returns a field to access the year of a week-based-year based on this <see cref="WeekFields"/>.
        /// <p>
        /// This represents the concept of the year where weeks start on a fixed day-of-week,
        /// such as Monday and each week belongs to exactly one year.
        /// This field is typically used with <see cref="WeekFields.DayOfWeek"/> and
        /// <see cref="WeekFields.WeekOfWeekBasedYear"/>.
        /// <p>
        /// Week one(1) is the week starting on the <see cref="WeekFields.FirstDayOfWeek"/>
        /// where there are at least <see cref="WeekFields.MinimalDaysInFirstWeek"/> days in the year.
        /// Thus, week one may start before the start of the year.
        /// If the first week starts after the start of the year then the period before
        /// is in the last week of the previous year.
        /// <p>
        /// This field can be used with any calendar system.
        /// <p>
        /// In the resolving phase of parsing, a date can be created from a week-based-year,
        /// week-of-year and day-of-week.
        /// <p>
        /// In strict mode, all three fields are
        /// validated against their range of valid values. The week-of-year field
        /// is validated to ensure that the resulting week-based-year is the
        /// week-based-year requested.
        /// <p>
        /// In smart mode, all three fields are
        /// validated against their range of valid values. The week-of-week-based-year field
        /// is validated from 1 to 53, meaning that the resulting date can be in the
        /// following week-based-year to that specified.
        /// <p>
        /// In lenient mode, the year and day-of-week
        /// are validated against the range of valid values. The resulting date is calculated
        /// equivalent to the following three stage approach.
        /// First, create a date on the first day of the first week in the requested week-based-year.
        /// Then take the week-of-week-based-year, subtract one, and add the amount in weeks to the date.
        /// Finally, adjust to the correct day-of-week within the localized week.
        /// </summary>
        /// <returns>a field providing access to the week-based-year, not null</returns>
        public ITemporalField WeekBasedYear
        {
            get { return weekBasedYear; }
        }

        #region public enum members
        /// <summary>
        /// The ISO-8601 definition, where a week starts on Monday and the first week
        /// has a minimum of 4 days.
        /// <p>
        /// The ISO-8601 standard defines a calendar system based on weeks.
        /// It uses the week-based-year and week-of-week-based-year concepts to split
        /// up the passage of days instead of the standard year/month/day.
        /// <p>
        /// Note that the first week may start in the previous calendar year.
        /// Note also that the first few days of a calendar year may be in the
        /// week-based-year corresponding to the previous calendar year.
        /// </summary>
        public static WeekFields ISO = new WeekFields(IsoDayOfWeek.Monday, 4);

        /// <summary>
        /// The common definition of a week that starts on Sunday.
        /// <p>
        /// Defined as starting on Sunday and with a minimum of 1 day in the month.
        /// This week definition is in use in the US and other European countries.
        /// </summary>
        public static WeekFields SUNDAY_START = new WeekFields(IsoDayOfWeek.Sunday, 1);
        #endregion

        #region private field values
        private WeekField dayOfWeek;
        private WeekField weekOfMonth;
        private WeekField weekOfYear;
        private WeekField weekOfWeekBasedYear;
        private WeekField weekBasedYear;
        #endregion

        private WeekFields(IsoDayOfWeek _firstDayOfWeek, int minimalDaysInFirstWeek)
        {
            // ArgumentChecker.NotNull(_firstDayOfWeek, "firstDayOfWeek");
            if (minimalDaysInFirstWeek < 1 || minimalDaysInFirstWeek > 7)
            {
                throw new ArgumentException(LocalizedResources.Instance().MINIMAL_NUMBER_OF_DAYS_IS_INVALID);
            }
            this.FirstDayOfWeek = _firstDayOfWeek;
            this.MinimalDaysInFirstWeek = minimalDaysInFirstWeek;

            dayOfWeek = WeekField.ofDayOfWeekField(this);
            weekOfMonth = WeekField.ofWeekOfMonthField(this);
            weekOfYear = WeekField.ofWeekOfYearField(this);
            weekOfWeekBasedYear = WeekField.ofWeekOfWeekBasedYearField(this);
            weekBasedYear = WeekField.ofWeekBasedYearField(this);
        }


        private sealed class WeekField : ITemporalField
        {
            /// <summary>
            /// Returns a field to access the day of week,
            /// computed based on a WeekFields.
            /// <p>
            /// The WeekDefintion of the first day of the week is used with
            /// the ISO DAY_OF_WEEK field to compute week boundaries.
            /// </summary>
            public static WeekField ofDayOfWeekField(WeekFields weekDef)
            {
                return new WeekField("DayOfWeek", weekDef,
                        ChronoUnit.DAYS, ChronoUnit.WEEKS, DAY_OF_WEEK_RANGE);
            }

            /// <summary>
            /// Returns a field to access the week of month,
            /// computed based on a WeekFields.
            /// </summary>
            /// <see cref="WeekFields.WeekOfMonth"/>
            public static WeekField ofWeekOfMonthField(WeekFields weekDef)
            {
                return new WeekField("WeekOfMonth", weekDef,
                        ChronoUnit.WEEKS, ChronoUnit.MONTHS, WEEK_OF_MONTH_RANGE);
            }

            /// <summary>
            /// Returns a field to access the week of year,
            /// computed based on a WeekFields.
            /// </summary>
            /// <see cref="WeekFields.WeekOfYear"/>
            public static WeekField ofWeekOfYearField(WeekFields weekDef)
            {
                return new WeekField("WeekOfYear", weekDef,
                        ChronoUnit.WEEKS, ChronoUnit.YEARS, WEEK_OF_YEAR_RANGE);
            }

            /// <summary>
            /// Returns a field to access the week of week-based-year,
            /// computed based on a WeekFields.
            /// </summary>
            /// <see cref="WeekFields.WeekOfWeekBasedYear"/>
            public static WeekField ofWeekOfWeekBasedYearField(WeekFields weekDef)
            {
                return new WeekField("WeekOfWeekBasedYear", weekDef,
                        ChronoUnit.WEEKS, ChronoUnit.WEEK_BASED_YEARS, WEEK_OF_WEEK_BASED_YEAR_RANGE);
            }

            /// <summary>
            /// Returns a field to access the week-based-year,
            /// computed based on a WeekFields.
            /// </summary>
            /// <see cref="WeekFields.WeekBasedYear"/>
            public static WeekField ofWeekBasedYearField(WeekFields weekDef)
            {
                return new WeekField("WeekBasedYear", weekDef,
                        ChronoUnit.WEEK_BASED_YEARS, ChronoUnit.FOREVER, WEEK_BASED_YEAR_RANGE);
            }


            public String Name { get; private set; }
            private WeekFields weekDef;
            public ChronoUnit BaseUnit { get; private set; }
            public ChronoUnit RangeUnit { get; private set; }
            public Range Range { get; private set; }

            #region static methods

            #endregion

            public bool IsDateBased
            {
                get { return true; }
            }

            public bool IsTimeBased
            {
                get { return false; }
            }

            private WeekField(String name, WeekFields weekDef, ChronoUnit baseUnit, ChronoUnit rangeUnit, Range range)
            {
                this.Name = name;
                this.weekDef = weekDef;
                this.BaseUnit = baseUnit;
                this.RangeUnit = rangeUnit;
                this.Range = range;
            }

            private int LocalizedDayOfWeek(LocalDate temporal, int sow)
            {
                int isoDow = (int)temporal.Get(ChronoField.DAY_OF_WEEK);
                return Math2.FloorMod(isoDow - sow, 7) + 1;
            }

            private long LocalizedWeekOfMonth(LocalDate temporal, int dow)
            {
                int dom = (int)temporal.Get(ChronoField.DAY_OF_MONTH);
                int offset = StartOfWeekOffset(dom, dow);
                return ComputeWeek(offset, dom);
            }

            private long LocalizedWeekOfYear(LocalDate temporal, int dow)
            {
                int doy = (int)temporal.Get(ChronoField.DAY_OF_YEAR);
                int offset = StartOfWeekOffset(doy, dow);
                return ComputeWeek(offset, doy);
            }

            private int LocalizedWOWBY(LocalDate temporal)
            {
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = (int)temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;
                long woy = LocalizedWeekOfYear(temporal, dow);
                if (woy == 0)
                {
                    LocalDate previous = LocalDate.FromDateTime(temporal.PlusWeeks(-1).ToDateTimeUnspecified());
                    return (int)LocalizedWeekOfYear(previous, dow) + 1;
                }
                else if (woy >= 53)
                {
                    int offset = StartOfWeekOffset((int)temporal.Get(ChronoField.DAY_OF_YEAR), dow);
                    int year = (int)temporal.Get(ChronoField.YEAR);
                    int yearLen = IsoChronology.GetInstance().IsLeapYear(year) ? 366 : 365;
                    int weekIndexOfFirstWeekNextYear = ComputeWeek(offset, yearLen + weekDef.MinimalDaysInFirstWeek);
                    if (woy >= weekIndexOfFirstWeekNextYear)
                    {
                        return (int)(woy - (weekIndexOfFirstWeekNextYear - 1));
                    }
                }
                return (int)woy;
            }

            private int LocalizedWBY(LocalDate temporal)
            {
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = (int)temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;
                int year = (int)temporal.Get(ChronoField.YEAR);
                long woy = LocalizedWeekOfYear(temporal, dow);
                if (woy == 0)
                {
                    return year - 1;
                }
                else if (woy < 53)
                {
                    return year;
                }
                int offset = StartOfWeekOffset((int)temporal.Get(ChronoField.DAY_OF_YEAR), dow);
                int yearLen = IsoChronology.GetInstance().IsLeapYear(year) ? 366 : 365;
                int weekIndexOfFirstWeekNextYear = ComputeWeek(offset, yearLen + weekDef.MinimalDaysInFirstWeek);
                if (woy >= weekIndexOfFirstWeekNextYear)
                {
                    return year + 1;
                }
                return year;
            }

            private int LocalizedDayOfWeek(ZonedDateTime temporal, int sow)
            {
                int isoDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                return Math2.FloorMod(isoDow - sow, 7) + 1;
            }

            private long LocalizedWeekOfMonth(ZonedDateTime temporal, int dow)
            {
                int dom = temporal.Get(ChronoField.DAY_OF_MONTH);
                int offset = StartOfWeekOffset(dom, dow);
                return ComputeWeek(offset, dom);
            }

            private long LocalizedWeekOfYear(ZonedDateTime temporal, int dow)
            {
                int doy = temporal.Get(ChronoField.DAY_OF_YEAR);
                int offset = StartOfWeekOffset(doy, dow);
                return ComputeWeek(offset, doy);
            }

            private int LocalizedWOWBY(ZonedDateTime temporal)
            {
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;
                long woy = LocalizedWeekOfYear(temporal, dow);
                if (woy == 0)
                {
                    temporal = temporal.PlusDays(-7);
                    DateTimeZone timeZone = DateTimeZoneProviders.Bcl.GetSystemDefault();
                    ZonedDateTime previous = new ZonedDateTime(temporal.ToInstant(), timeZone);
                    return (int)LocalizedWeekOfYear(previous, dow) + 1;
                }
                else if (woy >= 53)
                {
                    int offset = StartOfWeekOffset(temporal.Get(ChronoField.DAY_OF_YEAR), dow);
                    int year = temporal.Get(ChronoField.YEAR);
                    int yearLen = IsoChronology.GetInstance().IsLeapYear(year) ? 366 : 365;
                    int weekIndexOfFirstWeekNextYear = ComputeWeek(offset, yearLen + weekDef.MinimalDaysInFirstWeek);
                    if (woy >= weekIndexOfFirstWeekNextYear)
                    {
                        return (int)(woy - (weekIndexOfFirstWeekNextYear - 1));
                    }
                }
                return (int)woy;
            }

            private int LocalizedWBY(ZonedDateTime temporal)
            {
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;
                int year = temporal.Get(ChronoField.YEAR);
                long woy = LocalizedWeekOfYear(temporal, dow);
                if (woy == 0)
                {
                    return year - 1;
                }
                else if (woy < 53)
                {
                    return year;
                }
                int offset = StartOfWeekOffset(temporal.Get(ChronoField.DAY_OF_YEAR), dow);
                int yearLen = IsoChronology.GetInstance().IsLeapYear(year) ? 366 : 365;
                int weekIndexOfFirstWeekNextYear = ComputeWeek(offset, yearLen + weekDef.MinimalDaysInFirstWeek);
                if (woy >= weekIndexOfFirstWeekNextYear)
                {
                    return year + 1;
                }
                return year;
            }

            /// <summary>
            /// Returns an offset to align week start with a day of month or day of year.
            /// <param name="day the day; 1 through infinity
            /// <param name="dow the day of the week of that day; 1 through 7
            /// <returns> an offset in days to align a day with the start of the first 'full' week
            /// </summary>
            private int StartOfWeekOffset(int day, int dow)
            {
                // offset of first day corresponding to the day of week in first 7 days (zero origin)
                int weekStart = Math2.FloorMod(day - dow, 7);
                int offset = -weekStart;
                if (weekStart + 1 > weekDef.MinimalDaysInFirstWeek)
                {
                    // The previous week has the minimum days in the current month to be a 'week'
                    offset = 7 - weekStart;
                }
                return offset;
            }

            /// <summary>
            /// Returns the week number computed from the reference day and reference dayOfWeek.
            /// <param name="offset the offset to align a date with the start of week
            ///     from {@link #startOfWeekOffset}.
            /// <param name="day  the day for which to compute the week number
            /// <returns> the week number where zero is used for a partial week and 1 for the first full week
            /// </summary>
            private int ComputeWeek(int offset, int day)
            {
                return ((7 + offset + (day - 1)) / 7);
            }

            public bool IsSupportedBy(Temporal temporal)
            {
                if (temporal.IsSupported(ChronoField.DAY_OF_WEEK))
                {
                    if (RangeUnit == ChronoUnit.WEEKS)
                    {
                        return true;
                    }
                    else if (RangeUnit == ChronoUnit.MONTHS)
                    {
                        return temporal.IsSupported(ChronoField.DAY_OF_MONTH);
                    }
                    else if (RangeUnit == ChronoUnit.YEARS)
                    {
                        return temporal.IsSupported(ChronoField.DAY_OF_YEAR);
                    }
                    else if (RangeUnit == ChronoUnit.WEEK_BASED_YEARS)
                    {
                        return temporal.IsSupported(ChronoField.EPOCH_DAY);
                    }
                    else if (RangeUnit == ChronoUnit.FOREVER)
                    {
                        return temporal.IsSupported(ChronoField.EPOCH_DAY);
                    }
                }
                return false;
            }

            public Temporal AdjustInto(Temporal temporal, long newValue)
            {
                // Check the new value and get the old value of the field
                int newVal = Range.CheckValidIntValue(newValue, this);
                int currentVal = temporal.Get(this);
                if (newVal == currentVal)
                {
                    return temporal;
                }
                if (RangeUnit == ChronoUnit.FOREVER)
                {
                    // adjust in whole weeks so dow never changes
                    int baseWowby = temporal.Get(weekDef.weekOfWeekBasedYear);
                    long diffWeeks = (long)((newValue - currentVal) * 52.1775);
                    Temporal result = temporal.Plus(diffWeeks, ChronoUnit.WEEKS);
                    if (result.Get(this) > newVal)
                    {
                        // ended up in later week-based-year
                        // move to last week of previous year
                        int newWowby = result.Get(weekDef.weekOfWeekBasedYear);
                        result = result.Minus(newWowby, ChronoUnit.WEEKS);
                    }
                    else
                    {
                        if (result.Get(this) < newVal)
                        {
                            // ended up in earlier week-based-year
                            result = result.Plus(2, ChronoUnit.WEEKS);
                        }
                        // reset the week-of-week-based-year
                        int newWowby = result.Get(weekDef.weekOfWeekBasedYear);
                        result = result.Plus(baseWowby - newWowby, ChronoUnit.WEEKS);
                        if (result.Get(this) > newVal)
                        {
                            result = result.Minus(1, ChronoUnit.WEEKS);
                        }
                    }
                    return result;
                }
                // Compute the difference and add that using the base using of the field
                int delta = newVal - currentVal;
                return temporal.Plus(delta, BaseUnit);
            }

            public Range RangeRefinedBy(Temporal temporal)
            {
                if (RangeUnit == ChronoUnit.WEEKS)
                {
                    return Range;
                }

                ITemporalField field = null;
                if (RangeUnit == ChronoUnit.MONTHS)
                {
                    field = ChronoField.DAY_OF_MONTH;
                }
                else if (RangeUnit == ChronoUnit.YEARS)
                {
                    field = ChronoField.DAY_OF_YEAR;
                }
                else if (RangeUnit == ChronoUnit.WEEK_BASED_YEARS)
                {
                    return rangeWOWBY(temporal);
                }
                else if (RangeUnit == ChronoUnit.FOREVER)
                {
                    return temporal.GetRange(ChronoField.YEAR);
                }
                else
                {
                    throw new NotSupportedException("unreachable");
                }

                // Offset the ISO DOW by the start of this week
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;

                int offset = StartOfWeekOffset(temporal.Get(field), dow);
                Range fieldRange = temporal.GetRange(field);
                return new Range(ComputeWeek(offset, (int)fieldRange.MinSmallest), ComputeWeek(offset, (int)fieldRange.MaxLargest));
            }

            private Range rangeWOWBY(Temporal temporal)
            {
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;

                long woy = LocalizedWeekOfYear(temporal.GetOriginal(), dow);
                if (woy == 0)
                {
                    return rangeWOWBY(temporal.GetOriginal().Minus(2, ChronoUnit.WEEKS));
                }
                int offset = StartOfWeekOffset(temporal.Get(ChronoField.DAY_OF_YEAR), dow);
                int yearLen = temporal.IsLeapYear() ? 366 : 365;
                int weekIndexOfFirstWeekNextYear = ComputeWeek(offset, yearLen + weekDef.MinimalDaysInFirstWeek);
                if (woy >= weekIndexOfFirstWeekNextYear)
                {
                    return rangeWOWBY(temporal.GetOriginal().Plus(2, ChronoUnit.WEEKS));
                }
                return new Range(1, weekIndexOfFirstWeekNextYear - 1);
            }


            public long GetFrom(Temporal temporal)
            {
                if (temporal.Type == Temporal.ValueType.LocalDate)
                {
                    LocalDate date = temporal.GetOriginal();
                    return GetFrom(date);
                }
                else if (temporal.Type == Temporal.ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = temporal.GetOriginal();
                    return GetFrom(date);
                }
                throw new InvalidCastException();

            }

            private long GetFrom(LocalDate temporal)
            {
                // Offset the ISO DOW by the start of this week
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = (int)temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;

                if (this.RangeUnit == ChronoUnit.WEEKS)
                {
                    return dow;
                }
                else if (this.RangeUnit == ChronoUnit.MONTHS)
                {
                    int dom = (int)temporal.Get(ChronoField.DAY_OF_MONTH);
                    int offset = StartOfWeekOffset(dom, dow);
                    return ComputeWeek(offset, dom);
                }
                else if (this.RangeUnit == ChronoUnit.YEARS)
                {
                    int doy = (int)temporal.Get(ChronoField.DAY_OF_YEAR);
                    int offset = StartOfWeekOffset(doy, dow);
                    return ComputeWeek(offset, doy);
                }
                else if (this.RangeUnit == ChronoUnit.WEEK_BASED_YEARS)
                {
                    return LocalizedWOWBY(temporal);
                }
                else if (this.RangeUnit == ChronoUnit.FOREVER)
                {
                    return LocalizedWBY(temporal);
                }
                else
                {
                    throw new InvalidOperationException("unreachable");
                }
            }

            private long GetFrom(ZonedDateTime temporal)
            {
                // Offset the ISO DOW by the start of this week
                int sow = weekDef.FirstDayOfWeek.GetValue();
                int isoDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                int dow = Math2.FloorMod(isoDow - sow, 7) + 1;

                if (this.RangeUnit == ChronoUnit.WEEKS)
                {
                    return dow;
                }
                else if (this.RangeUnit == ChronoUnit.MONTHS)
                {
                    int dom = temporal.Get(ChronoField.DAY_OF_MONTH);
                    int offset = StartOfWeekOffset(dom, dow);
                    return ComputeWeek(offset, dom);
                }
                else if (this.RangeUnit == ChronoUnit.YEARS)
                {
                    int doy = temporal.Get(ChronoField.DAY_OF_YEAR);
                    int offset = StartOfWeekOffset(doy, dow);
                    return ComputeWeek(offset, doy);
                }
                else if (this.RangeUnit == ChronoUnit.WEEK_BASED_YEARS)
                {
                    return LocalizedWOWBY(temporal);
                }
                else if (this.RangeUnit == ChronoUnit.FOREVER)
                {
                    return LocalizedWBY(temporal);
                }
                else
                {
                    throw new InvalidOperationException("unreachable");
                }
            }
        }
    }
}
