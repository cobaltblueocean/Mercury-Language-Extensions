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
using Mercury.Language.Exception;
using NodaTime;
using Range = Mercury.Language.Math.Ranges.Range;

namespace Mercury.Language.Time
{
    /// <summary>
    /// TemporalAdjusters Description
    /// </summary>
    public class TemporalAdjuster : AbstractTemporalAdjuster
    {
        public static DateTimeZone ORIGINAL_TIME_ZONE = DateTimeZoneProviders.Bcl.GetSystemDefault(); //(new LocalDateTime()).ToZonedDateTime().Zone; //DateTimeZoneProviders.Bcl.GetSystemDefault();

        #region Static Methods
        /// <summary>
        /// Obtains a {@code TemporalAdjuster} that wraps a date adjuster.
        /// <p>
        /// The {@code TemporalAdjuster} is based on the low level {@code Temporal} interface.
        /// This method allows an adjustment from {@code LocalDate} to {@code LocalDate}
        /// to be wrapped to match the temporal-based interface.
        /// This is provided for convenience to make user-written adjusters simpler.
        /// <p>
        /// In general, user-written adjusters should be static constants:
        /// <pre>{@code
        ///  static TemporalAdjuster TWO_DAYS_LATER =
        ///       TemporalAdjusters.ofDateAdjuster(date -> date.PlusDays(2));
        /// }</pre>
        /// </summary>
        /// <param name="dateBasedAdjuster">the date-based adjuster, not null</param>
        /// <returns>the temporal adjuster wrapping on the date adjuster, not null</returns>
        public static ITemporalAdjuster OfDateAdjuster(ZonedDateTime dateBasedAdjuster)
        {
            // ArgumentChecker.NotNull(dateBasedAdjuster, "dateBasedAdjuster");

            return new TemporalAdjuster(dateBasedAdjuster);
        }

        public static ITemporalAdjuster Default()
        {
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            return OfDateAdjuster(new ZonedDateTime());
        }

        /// <summary>
        /// Returns the "first day of month" adjuster, which returns a new date set to
        /// the first day of the current month.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-01-01.<br>
        /// The input 2011-02-15 will return 2011-02-01.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_MONTH, 1);
        /// </pre>
        /// </summary>
        /// <returns>the first day-of-month adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfMonth()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            LocalDate t1 = now.InZone(timeZone).Date;

            ZonedDateTime temp = NodaTimeUtility.GetZonedDateTime(t1.Year, t1.Month, 1, 0, 0, 0, timeZone);

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfMonth);
        }

        /// <summary>
        /// Returns the "first day of month" adjuster, which returns a new date set to
        /// the first day of the current month.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-01-01.<br>
        /// The input 2011-02-15 will return 2011-02-01.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_MONTH, 1);
        /// </pre>
        /// </summary>
        /// <returns>the first day-of-month adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfMonth(DateTimeZone timeZone)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            LocalDate t1 = now.InZone(timeZone).Date;

            ZonedDateTime temp = NodaTimeUtility.GetZonedDateTime(t1.Year, t1.Month, 1, 0, 0, 0, timeZone);

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfMonth);
        }

        /// <summary>
        /// Returns the "last day of month" adjuster, which returns a new date set to
        /// the last day of the current month.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-01-31.<br>
        /// The input 2011-02-15 will return 2011-02-28.<br>
        /// The input 2012-02-15 will return 2012-02-29 (leap year).<br>
        /// The input 2011-04-15 will return 2011-04-30.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  long lastDay = temporal.range(DAY_OF_MONTH).getMaximum();
        ///  temporal.with(DAY_OF_MONTH, lastDay);
        /// </pre>
        /// 
        /// </summary>
        /// <returns>the last day-of-month adjuster, not null</returns>
        public static ITemporalAdjuster LastDayOfMonth()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusMonths(1);
            ZonedDateTime temp = NodaTimeUtility.GetZonedDateTime(t1.Year, t1.Month, 1, 0, 0, 0, timeZone);

            temp = temp.PlusDays(-1);

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.LastDayOfMonth);
        }

        /// <summary>
        /// Returns the "last day of month" adjuster, which returns a new date set to
        /// the last day of the current month.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-01-31.<br>
        /// The input 2011-02-15 will return 2011-02-28.<br>
        /// The input 2012-02-15 will return 2012-02-29 (leap year).<br>
        /// The input 2011-04-15 will return 2011-04-30.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  long lastDay = temporal.range(DAY_OF_MONTH).getMaximum();
        ///  temporal.with(DAY_OF_MONTH, lastDay);
        /// </pre>
        /// 
        /// </summary>
        /// <returns>the last day-of-month adjuster, not null</returns>
        public static ITemporalAdjuster LastDayOfMonth(DateTimeZone timeZone)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
           LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusMonths(1);
            ZonedDateTime temp = NodaTimeUtility.GetZonedDateTime(t1.Year, t1.Month, 1, 0, 0, 0, timeZone);

            temp = temp.PlusDays(-1);

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.LastDayOfMonth);
        }

        /// <summary>
        /// Returns the "first day of next month" adjuster, which returns a new date set to
        /// the first day of the next month.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-02-01.<br>
        /// The input 2011-02-15 will return 2011-03-01.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_MONTH, 1).plus(1, MONTHS);
        /// </pre>
        /// </summary>
        /// <returns>the first day of next month adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfNextMonth()
        {
            //ZonedDateTime t1 = new ZonedDateTime().AddMonths(1);
            //ZonedDateTime temp = new ZonedDateTime(t1.Year, t1.Month, 1);
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusMonths(1);
            LocalDateTime local = new LocalDateTime(t1.Year, t1.Month, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfNextMonth);
        }

        /// <summary>
        /// Returns the "first day of next month" adjuster, which returns a new date set to
        /// the first day of the next month.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-02-01.<br>
        /// The input 2011-02-15 will return 2011-03-01.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_MONTH, 1).plus(1, MONTHS);
        /// </pre>
        /// </summary>
        /// <returns>the first day of next month adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfNextMonth(DateTimeZone timeZone)
        {
            //ZonedDateTime t1 = new ZonedDateTime().AddMonths(1);
            //ZonedDateTime temp = new ZonedDateTime(t1.Year, t1.Month, 1);
            Instant now = SystemClock.Instance.GetCurrentInstant();
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusMonths(1);
            LocalDateTime local = new LocalDateTime(t1.Year, t1.Month, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfNextMonth);
        }

        /// <summary>
        /// Returns the "first day of year" adjuster, which returns a new date set to
        /// the first day of the current year.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-01-01.<br>
        /// The input 2011-02-15 will return 2011-01-01.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_YEAR, 1);
        /// </pre>
        /// </summary>
        /// <returns>the first day-of-year adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfYear()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            LocalDate t1 = now.InZone(timeZone).Date;
            LocalDateTime local = new LocalDateTime(t1.Year, 1, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfYear);
        }

        /// <summary>
        /// Returns the "first day of year" adjuster, which returns a new date set to
        /// the first day of the current year.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-01-01.<br>
        /// The input 2011-02-15 will return 2011-01-01.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_YEAR, 1);
        /// </pre>
        /// </summary>
        /// <returns>the first day-of-year adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfYear(DateTimeZone timeZone)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            LocalDate t1 = now.InZone(timeZone).Date;
            LocalDateTime local = new LocalDateTime(t1.Year, 1, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfYear);
        }

        /// <summary>
        /// Returns the "last day of year" adjuster, which returns a new date set to
        /// the last day of the current year.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-12-31.<br>
        /// The input 2011-02-15 will return 2011-12-31.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  long lastDay = temporal.range(DAY_OF_YEAR).getMaximum();
        ///  temporal.with(DAY_OF_YEAR, lastDay);
        /// </pre>
        /// 
        /// </summary>
        /// <returns>the last day-of-year adjuster, not null</returns>
        public static ITemporalAdjuster LastDayOfYear()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusYears(1);
            LocalDateTime local = new LocalDateTime(t1.Year, 1, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));
            temp = temp.PlusDays(-1);

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.LastDayOfYear);
        }

        /// <summary>
        /// Returns the "last day of year" adjuster, which returns a new date set to
        /// the last day of the current year.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2011-12-31.<br>
        /// The input 2011-02-15 will return 2011-12-31.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  long lastDay = temporal.range(DAY_OF_YEAR).getMaximum();
        ///  temporal.with(DAY_OF_YEAR, lastDay);
        /// </pre>
        /// 
        /// </summary>
        /// <returns>the last day-of-year adjuster, not null</returns>
        public static ITemporalAdjuster LastDayOfYear(DateTimeZone timeZone)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusYears(1);
            LocalDateTime local = new LocalDateTime(t1.Year, 1, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));
            temp = temp.PlusDays(-1);

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.LastDayOfYear);
        }

        /// <summary>
        /// Returns the "first day of next year" adjuster, which returns a new date set to
        /// the first day of the next year.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2012-01-01.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_YEAR, 1).plus(1, YEARS);
        /// </pre>
        /// </summary>
        /// <returns>the first day of next month adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfNextYear()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone timeZone = ORIGINAL_TIME_ZONE;
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusYears(1);
            LocalDateTime local = new LocalDateTime(t1.Year, 1, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfNextYear);
        }

        /// <summary>
        /// Returns the "first day of next year" adjuster, which returns a new date set to
        /// the first day of the next year.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 will return 2012-01-01.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It is equivalent to:
        /// <pre>
        ///  temporal.with(DAY_OF_YEAR, 1).plus(1, YEARS);
        /// </pre>
        /// </summary>
        /// <returns>the first day of next month adjuster, not null</returns>
        public static ITemporalAdjuster FirstDayOfNextYear(DateTimeZone timeZone)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            LocalDate t1 = now.InZone(timeZone).Date;

            t1.PlusYears(1);
            LocalDateTime local = new LocalDateTime(t1.Year, 1, 1, 0, 0);
            ZonedDateTime temp = new ZonedDateTime(local, timeZone, timeZone.GetUtcOffset(Instant.FromDateTimeUtc(System.DateTime.UtcNow)));

            return DayTemporalAdjuster.Of(temp, DayTemporalAdjuster.DayOf.FirstDayOfNextYear);
        }

        /// <summary>
        /// Returns the first in month adjuster, which returns a new date
        /// in the same month with the first matching day-of-week.
        /// This is used for expressions like 'first Tuesday in March'.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-12-15 for (MONDAY) will return 2011-12-05.<br>
        /// The input 2011-12-15 for (FRIDAY) will return 2011-12-02.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} and {@code DAY_OF_MONTH} fields
        /// and the {@code DAYS} unit, and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week, not null</param>
        /// <returns>the first in month adjuster, not null</returns>
        public static ITemporalAdjuster FirstInMonth(DayOfWeek dayOfWeek)
        {
            return TemporalAdjuster.DayOfWeekInMonth(1, dayOfWeek.ToIsoDayOfWeek());
        }

        /// <summary>
        /// Returns the first in month adjuster, which returns a new date
        /// in the same month with the first matching day-of-week.
        /// This is used for expressions like 'first Tuesday in March'.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-12-15 for (MONDAY) will return 2011-12-05.<br>
        /// The input 2011-12-15 for (FRIDAY) will return 2011-12-02.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} and {@code DAY_OF_MONTH} fields
        /// and the {@code DAYS} unit, and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week, not null</param>
        /// <returns>the first in month adjuster, not null</returns>
        public static ITemporalAdjuster FirstInMonth(IsoDayOfWeek dayOfWeek)
        {
            return TemporalAdjuster.DayOfWeekInMonth(1, dayOfWeek);
        }

        /// <summary>
        /// Returns the last in month adjuster, which returns a new date
        /// in the same month with the last matching day-of-week.
        /// This is used for expressions like 'last Tuesday in March'.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-12-15 for (MONDAY) will return 2011-12-26.<br>
        /// The input 2011-12-15 for (FRIDAY) will return 2011-12-30.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} and {@code DAY_OF_MONTH} fields
        /// and the {@code DAYS} unit, and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week, not null</param>
        /// <returns>the first in month adjuster, not null</returns>
        public static ITemporalAdjuster LastInMonth(DayOfWeek dayOfWeek)
        {
            return TemporalAdjuster.DayOfWeekInMonth(-1, dayOfWeek.ToIsoDayOfWeek());
        }

        /// <summary>
        /// Returns the last in month adjuster, which returns a new date
        /// in the same month with the last matching day-of-week.
        /// This is used for expressions like 'last Tuesday in March'.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-12-15 for (MONDAY) will return 2011-12-26.<br>
        /// The input 2011-12-15 for (FRIDAY) will return 2011-12-30.<br>
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} and {@code DAY_OF_MONTH} fields
        /// and the {@code DAYS} unit, and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week, not null</param>
        /// <returns>the first in month adjuster, not null</returns>
        public static ITemporalAdjuster LastInMonth(IsoDayOfWeek dayOfWeek)
        {
            return TemporalAdjuster.DayOfWeekInMonth(-1, dayOfWeek);
        }

        /// <summary>
        /// Returns the day-of-week in month adjuster, which returns a new date
        /// in the same month with the ordinal day-of-week.
        /// This is used for expressions like the 'second Tuesday in March'.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-12-15 for (1,TUESDAY) will return 2011-12-06.<br>
        /// The input 2011-12-15 for (2,TUESDAY) will return 2011-12-13.<br>
        /// The input 2011-12-15 for (3,TUESDAY) will return 2011-12-20.<br>
        /// The input 2011-12-15 for (4,TUESDAY) will return 2011-12-27.<br>
        /// The input 2011-12-15 for (5,TUESDAY) will return 2012-01-03.<br>
        /// The input 2011-12-15 for (-1,TUESDAY) will return 2011-12-27 (last in month).<br>
        /// The input 2011-12-15 for (-4,TUESDAY) will return 2011-12-06 (3 weeks before last in month).<br>
        /// The input 2011-12-15 for (-5,TUESDAY) will return 2011-11-29 (4 weeks before last in month).<br>
        /// The input 2011-12-15 for (0,TUESDAY) will return 2011-11-29 (last in previous month).<br>
        /// <p>
        /// For a positive or zero ordinal, the algorithm is equivalent to finding the first
        /// day-of-week that matches within the month and then adding a number of weeks to it.
        /// For a negative ordinal, the algorithm is equivalent to finding the last
        /// day-of-week that matches within the month and then subtracting a number of weeks to it.
        /// The ordinal number of weeks is not ArgumentCheckerd and is interpreted leniently
        /// according to this algorithmd This definition means that an ordinal of zero finds
        /// the last matching day-of-week in the previous month.
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} and {@code DAY_OF_MONTH} fields
        /// and the {@code DAYS} unit, and assumes a seven day week.
        /// </summary>
        /// <param name="ordinal">the week within the month, unbounded but typically from -5 to 5</param>
        /// <param name="dayOfWeek">the day-of-week, not null</param>
        /// <returns>the day-of-week in month adjuster, not null</returns>
        public static ITemporalAdjuster DayOfWeekInMonth(int ordinal, IsoDayOfWeek dayOfWeek)
        {
            // ArgumentChecker.NotNull(dayOfWeek, "dayOfWeek");
            return DayOfWeekTemporalAdjuster.Of(ordinal, dayOfWeek);
        }

        /// <summary>
        /// Returns the next day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week after the date being adjusted.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-17 (two days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-19 (four days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-22 (seven days later).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to move the date to, not null</param>
        /// <returns>the next day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster Next(DayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(2, dayOfWeek.ToIsoDayOfWeek());
        }

        /// <summary>
        /// Returns the next day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week after the date being adjusted.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-17 (two days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-19 (four days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-22 (seven days later).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to move the date to, not null</param>
        /// <returns>the next day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster Next(IsoDayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(2, dayOfWeek);
        }

        /// <summary>
        /// Returns the next-or-same day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week after the date being adjusted
        /// unless it is already on that day in which case the same object is returned.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-17 (two days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-19 (four days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-15 (same as input).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to check for or move the date to, not null</param>
        /// <returns>the next-or-same day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster NextOrSame(DayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(0, dayOfWeek.ToIsoDayOfWeek());

        }

        /// <summary>
        /// Returns the next-or-same day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week after the date being adjusted
        /// unless it is already on that day in which case the same object is returned.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-17 (two days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-19 (four days later).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-15 (same as input).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to check for or move the date to, not null</param>
        /// <returns>the next-or-same day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster NextOrSame(IsoDayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(0, dayOfWeek);

        }
        /// <summary>
        /// Returns the previous day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week before the date being adjusted.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-10 (five days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-12 (three days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-08 (seven days earlier).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to move the date to, not null</param>
        /// <returns>the previous day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster Previous(DayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(3, dayOfWeek.ToIsoDayOfWeek());

        }

        /// <summary>
        /// Returns the previous day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week before the date being adjusted.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-10 (five days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-12 (three days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-08 (seven days earlier).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to move the date to, not null</param>
        /// <returns>the previous day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster Previous(IsoDayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(3, dayOfWeek);

        }

        /// <summary>
        /// Returns the previous-or-same day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week before the date being adjusted
        /// unless it is already on that day in which case the same object is returned.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-10 (five days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-12 (three days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-15 (same as input).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to check for or move the date to, not null</param>
        /// <returns>the previous-or-same day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster PreviousOrSame(DayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(1, dayOfWeek.ToIsoDayOfWeek());

        }

        /// <summary>
        /// Returns the previous-or-same day-of-week adjuster, which adjusts the date to the
        /// first occurrence of the specified day-of-week before the date being adjusted
        /// unless it is already on that day in which case the same object is returned.
        /// <p>
        /// The ISO calendar system behaves as follows:<br>
        /// The input 2011-01-15 (a Saturday) for parameter (MONDAY) will return 2011-01-10 (five days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (WEDNESDAY) will return 2011-01-12 (three days earlier).<br>
        /// The input 2011-01-15 (a Saturday) for parameter (SATURDAY) will return 2011-01-15 (same as input).
        /// <p>
        /// The behavior is suitable for use with most calendar systems.
        /// It uses the {@code DAY_OF_WEEK} field and the {@code DAYS} unit,
        /// and assumes a seven day week.
        /// </summary>
        /// <param name="dayOfWeek">the day-of-week to check for or move the date to, not null</param>
        /// <returns>the previous-or-same day-of-week adjuster, not null</returns>
        public static ITemporalAdjuster PreviousOrSame(IsoDayOfWeek dayOfWeek)
        {
            return RelativeDayOfWeekTemporalAdjuster.Of(1, dayOfWeek);

        }
        #endregion

        private TemporalAdjuster(ZonedDateTime dateTime):base (dateTime)
        {
        }

        #region Instance Methods

        public override ITemporalAdjuster With(ITemporalAdjuster adjuster)
        {
            ZonedDateTime temp = DateTime;
            return new TemporalAdjuster(temp);
        }

        public override Temporal AdjustInto(Temporal temporal)
        {
            throw new NotSupportedException("Unreachable");
        }

        public override ITemporalAdjuster With(ITemporalField field, long newValue)
        {
            throw new NotImplementedException();
        }

        public override int Get(ITemporalField field)
        {
            throw new NotImplementedException();
        }

        public override ITemporalAdjuster Plus(int value, ChronoUnit unit)
        {
            throw new NotImplementedException();
        }

        public override ITemporalAdjuster Minus(int value, ChronoUnit unit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Class Implementations
        public class DayTemporalAdjuster : AbstractTemporalAdjuster
        {
            public enum DayOf
            {
                FirstDayOfMonth,
                LastDayOfMonth,
                FirstDayOfNextMonth,
                FirstDayOfYear,
                LastDayOfYear,
                FirstDayOfNextYear,
            }

            DayOf _dayOf;

            public DayOf DayOfType
            {
                get { return _dayOf; }
            }

            private DayTemporalAdjuster(ZonedDateTime dateTime, DayOf dayOf) : base(dateTime)
            {
                _dayOf = dayOf;
            }

            public static ITemporalAdjuster Of(ZonedDateTime dateTime, DayOf dayOf)
            {
                return new DayTemporalAdjuster(dateTime, dayOf);
            }

            public override ITemporalAdjuster With(ITemporalAdjuster adjuster)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster With(ITemporalField field, long newValue)
            {
                throw new NotImplementedException();
            }

            public override int Get(ITemporalField field)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster Plus(int value, ChronoUnit unit)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster Minus(int value, ChronoUnit unit)
            {
                throw new NotImplementedException();
            }

            public override Temporal AdjustInto(Temporal temporal)
            {
                switch (_dayOf)
                {
                    case DayOf.FirstDayOfMonth: return new Temporal(temporal.With(ChronoField.DAY_OF_MONTH, 1));
                    case DayOf.LastDayOfMonth: return new Temporal(temporal.With(ChronoField.DAY_OF_MONTH, temporal.GetRange(ChronoField.DAY_OF_MONTH).MaxLargest));
                    case DayOf.FirstDayOfNextMonth: return new Temporal(temporal.With(ChronoField.DAY_OF_MONTH, 1).Plus(1, ChronoUnit.MONTHS));
                    case DayOf.FirstDayOfYear: return new Temporal(temporal.With(ChronoField.DAY_OF_YEAR, 1));
                    case DayOf.LastDayOfYear: return new Temporal(temporal.With(ChronoField.DAY_OF_YEAR, temporal.GetRange(ChronoField.DAY_OF_YEAR).MaxLargest));
                    case DayOf.FirstDayOfNextYear: return new Temporal(temporal.With(ChronoField.DAY_OF_YEAR, 1).Plus(1, ChronoUnit.YEARS));
                }
                throw new NotSupportedException("Unreachable");
            }
        }

        public class DayOfWeekTemporalAdjuster : AbstractTemporalAdjuster
        {
            private IsoDayOfWeek _dayOfWeek;
            private int _ordinal;

            public IsoDayOfWeek DayOfWeek
            {
                get { return _dayOfWeek; }
            }

            public int Ordinal
            {
                get { return _ordinal; }
            }

            private DayOfWeekTemporalAdjuster(int ordinal, IsoDayOfWeek dow) : base(System.DateTime.UtcNow.ToZonedDateTime().AdjustDayOfWeek(dow))
            {
                _ordinal = ordinal;
                _dayOfWeek = dow;
            }

            public static ITemporalAdjuster Of(int ordinal, IsoDayOfWeek dayOfWeek)
            {
                return new DayOfWeekTemporalAdjuster(ordinal, dayOfWeek);
            }

            public override ITemporalAdjuster With(ITemporalAdjuster adjuster)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster With(ITemporalField field, long newValue)
            {
                throw new NotImplementedException();
            }

            public override int Get(ITemporalField field)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster Plus(int value, ChronoUnit unit)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster Minus(int value, ChronoUnit unit)
            {
                throw new NotImplementedException();
            }

            public override Temporal AdjustInto(Temporal temporal)
            {
                int dowValue = _dayOfWeek.GetValue();

            int calDow = temporal.Get(ChronoField.DAY_OF_WEEK);
                if (_ordinal < 2 && calDow == dowValue)
                {
                    return temporal;
                }
                if ((_ordinal & 1) == 0)
                {
                    int daysDiff = calDow - dowValue;
                    return new Temporal(temporal.Plus(daysDiff >= 0 ? 7 - daysDiff : -daysDiff, ChronoUnit.DAYS));
                }
                else
                {
                    int daysDiff = dowValue - calDow;
                    return new Temporal(temporal.Minus(daysDiff >= 0 ? 7 - daysDiff : -daysDiff, ChronoUnit.DAYS));
                }
            }
        }

        public class RelativeDayOfWeekTemporalAdjuster : AbstractTemporalAdjuster
        {
            int _relative;
            private IsoDayOfWeek _dayOfWeek;

            public IsoDayOfWeek DayOfWeek
            {
                get { return _dayOfWeek; }
            }

            public int Relative
            {
                get { return _relative; }
            }

            private RelativeDayOfWeekTemporalAdjuster(int relative, IsoDayOfWeek dow) : base(System.DateTime.UtcNow.ToZonedDateTime().AdjustDayOfWeek(dow))
            {
                _relative = relative;
                _dayOfWeek = dow;
            }

            public static ITemporalAdjuster Of(int relative, IsoDayOfWeek dayOfWeek)
            {
                return new RelativeDayOfWeekTemporalAdjuster(relative, dayOfWeek);
            }

            public override ITemporalAdjuster With(ITemporalAdjuster adjuster)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster With(ITemporalField field, long newValue)
            {
                throw new NotImplementedException();
            }

            public override int Get(ITemporalField field)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster Plus(int value, ChronoUnit unit)
            {
                throw new NotImplementedException();
            }

            public override ITemporalAdjuster Minus(int value, ChronoUnit unit)
            {
                throw new NotImplementedException();
            }

            public override Temporal AdjustInto(Temporal temporal)
            {
                int dowValue = _dayOfWeek.GetValue();

                if (_relative >= 0)
                {
                    Temporal temp = temporal.With(ChronoField.DAY_OF_MONTH, 1);
                    int curDow = temp.Get(ChronoField.DAY_OF_WEEK);
                    int dowDiff = (dowValue - curDow + 7) % 7;
                    dowDiff += (int)(((long)_relative - 1L) * 7L);  // safe from overflow
                    return new Temporal(temp.Plus(dowDiff, ChronoUnit.DAYS));
                }
                else
                {
                    long maximum = temporal.GetRange(ChronoField.DAY_OF_MONTH).MaxLargest;
                    Temporal temp = temporal.With(ChronoField.DAY_OF_MONTH, maximum);
                    int curDow = temp.Get(ChronoField.DAY_OF_WEEK);
                    int daysDiff = dowValue - curDow;
                    daysDiff = (daysDiff == 0 ? 0 : (daysDiff > 0 ? daysDiff - 7 : daysDiff));
                    daysDiff -= (int)((-_relative - 1L) * 7L);  // safe from overflow
                    return new Temporal(temp.Plus(daysDiff, ChronoUnit.DAYS));
                }
            }
        }
        #endregion
    }

    #region Abstract implementation
    public abstract class AbstractTemporalAdjuster : ITemporalAdjuster
    {
        ZonedDateTime _dateTime;

        public AbstractTemporalAdjuster(ZonedDateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public abstract int Get(ITemporalField field);

        public abstract ITemporalAdjuster Plus(int value, ChronoUnit unit);
        public abstract ITemporalAdjuster Minus(int value, ChronoUnit unit);
        public abstract Temporal AdjustInto(Temporal temporal);
        public abstract ITemporalAdjuster With(ITemporalAdjuster adjuster);
        public abstract ITemporalAdjuster With(ITemporalField field, long newValue);

        public ZonedDateTime DateTime
        {
            get { return _dateTime; }
        }

        public Range GetRange(ITemporalField field)
        {
            if (field is ChronoField) {
                ChronoField f = (ChronoField)field;
                if (f.IsDateBased)
                {
                    if (f == ChronoField.DAY_OF_MONTH) return new Range(1, this.DateTime.LengthOfMonth());
                    else if (f == ChronoField.DAY_OF_YEAR) return new Range(1, this.DateTime.LengthOfYear());
                    else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH) return new Range(1, this.DateTime.Month == Month.FEBRUARY.Value && this.DateTime.IsLeapYear() == false ? 4 : 5);
                    else if (f == ChronoField.YEAR_OF_ERA)
                         return this.DateTime.Year <= 0 ? new Range(1, this.DateTime.GetMaxYear() + 1) : new Range(1, this.DateTime.GetMaxYear()); 

                    return field.Range;
                }
                throw new NotSupportedException("Unsupported field: " + field);
            }
            return field.RangeRefinedBy(new Temporal(_dateTime));
        }
    }
    #endregion
}
