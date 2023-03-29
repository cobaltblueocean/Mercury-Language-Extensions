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
using Mercury.Language.Extensions;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Time;
using NodaTime;
using Range = Mercury.Language.Math.Ranges.Range;
using System.Runtime.CompilerServices;

namespace NodaTime
{
    /// <summary>
    /// NodaTimeExtension Description
    /// </summary>
    public static class NodaTimeExtension
    {
        ////public static DateTimeZone ORIGINAL_TIME_ZONE = DateTimeZoneProviders.Bcl.GetSystemDefault(); //(new LocalDateTime()).ToZonedDateTime().Zone; //DateTimeZoneProviders.Bcl.GetSystemDefault();
        ///// <summary>
        ///// The number of days in a 400 year cycle.
        ///// </summary>
        //private const int DAYS_PER_CYCLE = 146097;
        ///// <summary>
        ///// The number of days from year zero to year 1970.
        ///// There are five 400 year cycles from year zero to 2000.
        ///// There are 7 leap years from 1970 to 2000.
        ///// </summary>
        //private const long DAYS_0000_TO_1970 = (DAYS_PER_CYCLE * 5) - (30 * 365 + 7);

        ///// <summary>
        ///// The number of seconds in one day.
        ///// </summary>
        //public const long SECONDS_PER_DAY = 86400L;
        ///// <summary>
        ///// The number of days in one year (estimated as 365.25).
        ///// </summary>
        ////TODO change this to 365.2425 to be consistent with JSR-310
        //public const double DAYS_PER_YEAR = 365.25;
        ///// <summary>
        ///// The number of milliseconds in one day.
        ///// </summary>
        //public const long MILLISECONDS_PER_DAY = SECONDS_PER_DAY * 1000;
        ///// <summary>
        ///// The number of seconds in one year.
        ///// </summary>
        //public const long SECONDS_PER_YEAR = (long)(SECONDS_PER_DAY * DAYS_PER_YEAR);
        ///// <summary>
        ///// The number of milliseconds in one year.
        ///// </summary>
        //public const long MILLISECONDS_PER_YEAR = SECONDS_PER_YEAR * 1000;
        ///// <summary>
        ///// The number of milliseconds in one month.
        ///// </summary>
        //public const long MILLISECONDS_PER_MONTH = MILLISECONDS_PER_YEAR / 12L;
        ///// <summary>
        ///// The number of milliseconds in one week.
        ///// </summary>
        //public const long MILLISECONDS_PER_WEEK = MILLISECONDS_PER_DAY * 7;

        //public static Duration _days = Duration.FromTimeSpan(new TimeSpan(24, 0, 0));
        //public static Duration _months = Duration.FromTimeSpan(TimeSpan.FromSeconds(31556952L / 12));
        //public static Duration _years = Duration.FromTimeSpan(TimeSpan.FromSeconds(31556952L));

        ///// <summary>
        ///// The minimum supported year, '-999,999,999'.
        ///// </summary>
        //private static int MIN_VALUE = -999999999;

        ///// <summary>
        ///// The maximum supported year, '+999,999,999'.
        ///// </summary>
        //private static int MAX_VALUE = 999999999;


        #region DateTime Operation

        /// <summary>
        /// The number of seconds in one day.
        /// </summary>
        public static long SECONDS_PER_DAY = 86400L;

        /// <summary>
        /// The number of days in one year (estimated as 365.25).
        /// </summary>
        //TODO change this to 365.2425 to be consistent with JSR-310
        public static double DAYS_PER_YEAR = 365.25;

        /// <summary>
        /// The number of milliseconds in one day.
        /// </summary>
        public static long MILLISECONDS_PER_DAY = SECONDS_PER_DAY * 1000;

        /// <summary>
        /// The number of seconds in one year.
        /// </summary>
        public static long SECONDS_PER_YEAR = (long)(SECONDS_PER_DAY * DAYS_PER_YEAR);

        /// <summary>
        /// The number of milliseconds in one year.
        /// </summary>
        public static long MILLISECONDS_PER_YEAR = SECONDS_PER_YEAR * 1000;

        /// <summary>
        /// The number of milliseconds in one month.
        /// </summary>
        public static long MILLISECONDS_PER_MONTH = MILLISECONDS_PER_YEAR / 12L;


        public static ZonedDateTime ToZonedDateTime(this DateTime source)
        {
            DateTimeZone timeZone = DateTimeZoneProviders.Bcl.GetSystemDefault();
            return ToZonedDateTime(source, timeZone);
        }


        public static ZonedDateTime ToZonedDateTime(this DateTime source, string timeZoneId)
        {
            DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            if (timeZone == null)
            {
                throw new TimeZoneNotFoundException();
            }
            return ToZonedDateTime(source, timeZone);
        }


        public static ZonedDateTime ToZonedDateTime(this DateTime source, DateTimeZone timeZone)
        {
            Offset offset = timeZone.GetUtcOffset(Instant.FromDateTimeUtc(source.ToUniversalTime()));
            return (new ZonedDateTime(source.ToInstant(), timeZone)).PlusSeconds(-offset.Seconds);
        }

        public static LocalDate ToLocalDate(this DateTime source)
        {
            return LocalDate.FromDateTime(source);
        }

        public static Instant ToInstant(this DateTime now)
        {
            DateTime source = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, DateTimeKind.Utc);
            return Instant.FromDateTimeUtc(source.ToUniversalTime());
        }

        public static double GetDifferenceInMonths(this Instant? startDate, Instant? endDate)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().START_DATE_WAS_NULL);
            }
            if (endDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().END_DATE_WAS_NULL);
            }

            return (double)(Instant.Subtract(endDate.Value, startDate.Value).TotalMilliseconds) / MILLISECONDS_PER_MONTH;
        }
        #endregion

        #region Array
        #region ZonedDateTime
        public static ZonedDateTime[] ToPremitiveArray(this IList<ZonedDateTime?> val)
        {
            return val.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
        public static ZonedDateTime[] ToPremitiveArray(this IList<ZonedDateTime?> val, int length)
        {
            return val.Take(length).Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }

        public static ZonedDateTime?[] ToNullableArray(this IList<ZonedDateTime> val)
        {
            return val.ToArray().Cast<Nullable<ZonedDateTime>>().ToArray();
        }

        public static ZonedDateTime?[] ToNullableArray(this IList<ZonedDateTime> val, int length)
        {
            return val.Take(length).ToArray().Cast<Nullable<ZonedDateTime>>().ToArray();
        }
        #endregion

        #region LocalDateTime
        public static LocalDateTime[] ToPremitiveArray(this IList<LocalDateTime?> val)
        {
            return val.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
        public static LocalDateTime[] ToPremitiveArray(this IList<LocalDateTime?> val, int length)
        {
            return val.Take(length).Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }

        public static LocalDateTime?[] ToNullableArray(this IList<LocalDateTime> val)
        {
            return val.ToArray().Cast<Nullable<LocalDateTime>>().ToArray();
        }

        public static LocalDateTime?[] ToNullableArray(this IList<LocalDateTime> val, int length)
        {
            return val.Take(length).ToArray().Cast<Nullable<LocalDateTime>>().ToArray();
        }
        #endregion

        #region LocalDate
        public static LocalDate[] ToPremitiveArray(this IList<LocalDate?> val)
        {
            return val.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
        public static LocalDate[] ToPremitiveArray(this IList<LocalDate?> val, int length)
        {
            return val.Take(length).Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }

        public static LocalDate?[] ToNullableArray(this IList<LocalDate> val)
        {
            return val.ToArray().Cast<Nullable<LocalDate>>().ToArray();
        }

        public static LocalDate?[] ToNullableArray(this IList<LocalDate> val, int length)
        {
            return val.Take(length).ToArray().Cast<Nullable<LocalDate>>().ToArray();
        }
        #endregion

        #region Duration
        public static Duration[] ToPremitiveArray(this IList<Duration?> val)
        {
            return val.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
        public static Duration[] ToPremitiveArray(this IList<Duration?> val, int length)
        {
            return val.Take(length).Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }

        public static Duration?[] ToNullableArray(this IList<Duration> val)
        {
            return val.ToArray().Cast<Nullable<Duration>>().ToArray();
        }

        public static Duration?[] ToNullableArray(this IList<Duration> val, int length)
        {
            return val.Take(length).ToArray().Cast<Nullable<Duration>>().ToArray();
        }
        #endregion

        #region Instant
        public static Instant[] ToPremitiveArray(this IList<Instant?> val)
        {
            return val.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
        public static Instant[] ToPremitiveArray(this IList<Instant?> val, int length)
        {
            return val.Take(length).Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }

        public static Instant?[] ToNullableArray(this IList<Instant> val)
        {
            return val.ToArray().Cast<Nullable<Instant>>().ToArray();
        }

        public static Instant?[] ToNullableArray(this IList<Instant> val, int length)
        {
            return val.Take(length).ToArray().Cast<Nullable<Instant>>().ToArray();
        }
        #endregion

        #region Range
        public static Range[] ToPremitiveArray(this IList<Range?> val)
        {
            return val.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
        public static Range[] ToPremitiveArray(this IList<Range?> val, int length)
        {
            return val.Take(length).Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }

        public static Range?[] ToNullableArray(this IList<Range> val)
        {
            return val.ToArray().Cast<Nullable<Range>>().ToArray();
        }

        public static Range?[] ToNullableArray(this IList<Range> val, int length)
        {
            return val.Take(length).ToArray().Cast<Nullable<Range>>().ToArray();
        }
        #endregion
        #endregion

        #region ZonedDateTime Methods
        #region Regular

        public static Nullable<ZonedDateTime> ToNullable(this ZonedDateTime now)
        {
            ZonedDateTime? ret = now;
            return ret;
        }

        public static int CompareTo(this ZonedDateTime now, ZonedDateTime target)
        {
            return DateTime.Compare(now.ToDateTimeUtc(), target.ToDateTimeUtc());
        }

        public static Boolean IsAfter(this ZonedDateTime now, ZonedDateTime target)
        {
            if (DateTime.Compare(now.ToDateTimeUtc(), target.ToDateTimeUtc()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this ZonedDateTime now, ZonedDateTime target)
        {
            if (DateTime.Compare(now.ToDateTimeUtc(), target.ToDateTimeUtc()) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsLeapYear(this ZonedDateTime now)
        {
            return DateTime.IsLeapYear(now.Year);
        }

        public static Boolean IsSupported(this ZonedDateTime now, ITemporalField field)
        {
            return field is ChronoField || (field != null && field.IsSupportedBy(new Temporal(now)));
        }

        public static Boolean IsSupported(this ZonedDateTime now, ChronoUnit unit)
        {
            if (unit is ChronoUnit)
            {
                return unit.IsDateBased || unit.IsTimeBased;
            }
            return unit != null && unit.IsSupportedBy(new Temporal(now));
        }

        public static ZonedDateTime AdjustDayOfWeek(this ZonedDateTime now, IsoDayOfWeek dayOfWeek)
        {
            IsoDayOfWeek currentDayOfWeek = now.DayOfWeek;
            int diffDayOfWeek = now.DayOfWeek - dayOfWeek;

            return now.PlusDays(diffDayOfWeek);
        }

        public static ZonedDateTime GetDateOffsetWithYearFraction(this ZonedDateTime startDate, double yearFraction)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().DATE_WAS_NULL);
            }
            long nanos = (long)System.Math.Round((decimal)1e9 * NodaTimeUtility.SECONDS_PER_YEAR * (decimal)yearFraction);

            return startDate.PlusNanoseconds(nanos);
        }

        public static ZonedDateTime PlusTime(this ZonedDateTime nodaTime, LocalTime time)
        {
            return nodaTime.PlusHours(time.Hour).PlusMinutes(time.Minute).PlusSeconds(time.Second).PlusMilliseconds(time.Millisecond);
        }

        public static ZonedDateTime PlusDays(this ZonedDateTime nodaTime, int days)
        {
            return nodaTime.Plus(Duration.FromDays(days));
        }

        public static ZonedDateTime PlusWeeks(this ZonedDateTime nodaTime, int weeks)
        {
            return nodaTime.Plus(Duration.FromDays(weeks * 7));
        }

        public static ZonedDateTime PlusMonths(this ZonedDateTime nodaTime, int months)
        {
            bool isMinus = false;
            if (months < 0)
                isMinus = true;

            int absMonths = Math.Abs(months);
            int y = (int)Math.Truncate(absMonths / 12d);
            int m = absMonths - y * 12;
            DateTimeZone zone = nodaTime.Zone;
            DateTime dt = new DateTime(nodaTime.Year, nodaTime.Month, nodaTime.Day, nodaTime.Hour, nodaTime.Minute, nodaTime.Second, nodaTime.Millisecond, DateTimeKind.Unspecified);

            if (!isMinus)
            {
                if (y > 0)
                {
                    dt = dt.AddYears(y);
                }
                dt = dt.AddMonths(m);
            }
            else
            {
                if (y > 0)
                {
                    dt = dt.AddYears(-y);
                }
                dt = dt.AddMonths(-m);
            }

            return NodaTimeUtility.GetZonedDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, 0, zone);
        }

        public static ZonedDateTime PlusYears(this ZonedDateTime nodaTime, int years)
        {
            DateTime dt = new DateTime(nodaTime.Year, nodaTime.Month, nodaTime.Day, nodaTime.Hour, nodaTime.Minute, nodaTime.Second, nodaTime.Millisecond, DateTimeKind.Unspecified);
            DateTimeZone zone = nodaTime.Zone;

            dt = dt.AddYears(years);

            return NodaTimeUtility.GetZonedDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, 0, zone);
        }

        public static ZonedDateTime PlusDateTimeValues(this ZonedDateTime nodaTime, int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            if (year > 0)
            {
                Period py = Period.FromYears(year);
                nodaTime.Plus(py.ToDuration());
            }
            else if(year < 0)
            {
                Period py = Period.FromYears(System.Math.Abs(year));
                nodaTime.Minus(py.ToDuration());
            }

            if (month > 0)
            {
                Period pm = Period.FromMonths(month);
                nodaTime.Plus(pm.ToDuration());
            }
            else if (month < 0)
            {
                Period pm = Period.FromMonths(System.Math.Abs(month));
                nodaTime.Minus(pm.ToDuration());
            }

            if (day > 0)
            {
                Period pd = Period.FromDays(day);
                nodaTime.Plus(pd.ToDuration());
            }
            else if (day < 0)
            {
                Period pd = Period.FromDays(System.Math.Abs(day));
                nodaTime.Minus(pd.ToDuration());
            }

            if (hour != 0)
            {
                nodaTime.PlusHours(hour);
            }

            if (minute != 0)
            {
                nodaTime.PlusMinutes(minute);
            }

            if (second != 0)
            {
                nodaTime.PlusSeconds(second);
            }

            if (millisecond != 0)
            {
                nodaTime.PlusMilliseconds(millisecond);
            }

            if (nanosecond != 0)
            {
                nodaTime.PlusNanoseconds(nanosecond);
            }

            return nodaTime;
        }

        public static ZonedDateTime AsZonedDateTime(this Instant instant)
        {
            
            return new ZonedDateTime(instant, NodaTimeUtility.ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime AsZonedDateTime(this Instant instant, DateTimeZone zone)
        {
            return new ZonedDateTime(instant, zone);
        }

        public static ZonedDateTime AsZonedDateTimeUTC(this Instant instant)
        {

            return new ZonedDateTime(instant, DateTimeZone.Utc);
        }

        public static ZonedDateTime With(this ZonedDateTime now, ITemporalAdjuster tmp)
        {
            return tmp.AdjustInto(new Temporal(now)).GetOriginal();
        }

        public static ZonedDateTime With(this ZonedDateTime now, LocalTime time)
        {
            return now.WithHour(time.Hour).WithMinute(time.Minute).WithSecond(time.Second).WithNano(time.NanosecondOfSecond);
        }

        public static ZonedDateTime WithZoneSameInstant(this ZonedDateTime now, DateTimeZone zone)
        {
            // ArgumentChecker.NotNull(zone, "zone");
            return now.Zone == zone ? now : new ZonedDateTime(now.ToInstant(), zone);
        }

        public static ZonedDateTime Plus(this ZonedDateTime now, Period period)
        {
            try
            {
                return now.Plus(period.ToDuration());
            }
            catch
            {
                ZonedDateTime result;
                result = now.PlusYears(period.Years);
                result = result.PlusMonths(period.Months);
                result = result.PlusDays(period.Days);
                result = result.PlusHours((int)period.Hours);
                result = result.PlusMinutes((int)period.Minutes);
                result = result.PlusSeconds(period.Seconds);
                result = result.PlusMilliseconds(period.Milliseconds);
                result = result.PlusNanoseconds(period.Nanoseconds);

                return result;
            }
        }

        public static IList<ZonedDateTime> SortList(this IList<ZonedDateTime> dates)
        {
            var result = dates;
            int i, j;
            int N = result.Count;

            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if (result[i].IsAfter(result[i + 1]))
                        exchange(result, i, i + 1);
                }
            }
            return result;
        }

        private static void exchange(IList<ZonedDateTime> data, int m, int n)
        {
            ZonedDateTime temporary;

            temporary = data[m];
            data[m] = data[n];
            data[n] = temporary;
        }

        public static ZonedDateTime Minus(this ZonedDateTime now, Period period)
        {
            try
            {
                return now.Plus(-period.ToDuration());
            }
            catch
            {
                ZonedDateTime result;
                result = now.PlusYears(-period.Years);
                result = result.PlusMonths(-period.Months);
                result = result.PlusDays(-period.Days);
                result = result.PlusHours(-(int)period.Hours);
                result = result.PlusMinutes(-(int)period.Minutes);
                result = result.PlusSeconds(-period.Seconds);
                result = result.PlusMilliseconds(-period.Milliseconds);
                result = result.PlusNanoseconds(-period.Nanoseconds);

                return result;
            }
        }

        public static ZonedDateTime Plus(this ZonedDateTime now, long amountToAdd, ITemporalUnit unit)
        {
            if (unit is ChronoUnit)
            {
                ChronoUnit f = (ChronoUnit)unit;

                if (f == ChronoUnit.DAYS)
                {
                    return now.PlusDays((int)amountToAdd);
                }
                else if (f == ChronoUnit.WEEKS)
                {
                    return now.PlusWeeks((int)amountToAdd);
                }
                else if (f == ChronoUnit.MONTHS)
                {
                    return now.PlusMonths((int)amountToAdd);
                }
                else if (f == ChronoUnit.YEARS)
                {
                    return now.PlusYears((int)amountToAdd);
                }
                else if (f == ChronoUnit.DECADES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 10));
                }
                else if (f == ChronoUnit.CENTURIES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 100));
                }
                else if (f == ChronoUnit.MILLENNIA)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 1000));
                }
                else if (f == ChronoUnit.ERAS)
                {
                    return now.With(ChronoField.ERA, Math2.SafeAdd(now.GetLong(ChronoField.ERA), amountToAdd));
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_UNIT, unit));
            }
            return unit.AddTo(new Temporal(now), amountToAdd);
        }

        public static ZonedDateTime Minus(this ZonedDateTime now, long amountToSubtract, ITemporalUnit unit)
        {
            return (amountToSubtract == long.MinValue ? now.Plus(long.MaxValue, unit).Plus(1, unit) : now.Plus(-amountToSubtract, unit));
        }

        public static ZonedDateTime With(this ZonedDateTime now, MonthDay monthDay)
        {
            DateTimeZone timeZone = now.Zone;
            return new ZonedDateTime(new DateTime(now.Year, monthDay.Month.Value, monthDay.Day).ToInstant(), now.Zone);
        }

        public static ZonedDateTime With(this ZonedDateTime now, LocalDate localDate)
        {
            return new ZonedDateTime(localDate.ToDateTimeUnspecified().ToInstant(), now.Zone);
        }

        public static ZonedDateTime With(this ZonedDateTime now, ITemporalField field, long newValue)
        {
            if (field is ChronoField)
            {
                ChronoField f = (ChronoField)field;
                f.CheckValidValue(newValue);
                if (f == ChronoField.DAY_OF_WEEK)
                {
                    return now.PlusDays((int)newValue - now.DayOfWeek.GetValue());
                }
                else if (f == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)
                {
                    return now.PlusDays((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH));
                }
                else if (f == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)
                {
                    return now.PlusDays((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR));
                }
                else if (f == ChronoField.DAY_OF_MONTH)
                {
                    return now.WithDayOfMonth((int)newValue);
                }
                else if (f == ChronoField.DAY_OF_YEAR)
                {
                    return now.WithDayOfYear((int)newValue);
                }
                else if (f == ChronoField.EPOCH_DAY)
                {
                    return now.OfEpochDay(newValue);
                }
                else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH)
                {
                    return now.PlusWeeks((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_WEEK_OF_MONTH));
                }
                else if (f == ChronoField.ALIGNED_WEEK_OF_YEAR)
                {
                    return now.PlusWeeks((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_WEEK_OF_YEAR));
                }
                else if (f == ChronoField.MONTH_OF_YEAR)
                {
                    return now.WithMonth((int)newValue);
                }
                else if (f == ChronoField.PROLEPTIC_MONTH)
                {
                    return now.PlusMonths((int)newValue - (int)now.GetLong(ChronoField.PROLEPTIC_MONTH));
                }
                else if (f == ChronoField.YEAR_OF_ERA)
                {
                    return now.WithYear((int)(now.Year >= 1 ? newValue : 1 - newValue));
                }
                else if (f == ChronoField.YEAR)
                {
                    return now.WithYear((int)newValue);
                }
                else if (f == ChronoField.ERA)
                {
                    return (now.GetLong(ChronoField.ERA) == newValue ? now : now.WithYear(1 - now.Year));
                }

                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }

            return field.AdjustInto(new Temporal(now), newValue).GetOriginal();
        }

        public static ZonedDateTime WithHour(this ZonedDateTime now, int hour)
        {
            if (now.Hour == hour)
            {
                return now;
            }
            
            return new ZonedDateTime(new DateTime(now.Year, now.Month, now.Day, hour, now.Minute, now.Second).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithMinute(this ZonedDateTime now, int minute)
        {
            if (now.Minute == minute)
            {
                return now;
            }
            
            return new ZonedDateTime(new DateTime(now.Year, now.Month, now.Day, now.Hour, minute, now.Second).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithSecond(this ZonedDateTime now, int second)
        {
            if (now.Second == second)
            {
                return now;
            }
            
            return new ZonedDateTime(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, second).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithNano(this ZonedDateTime now, int nano)
        {
            if (now.NanosecondOfSecond == nano)
            {
                return now;
            }
            
            return new ZonedDateTime(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second).AddNanos(nano).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithYear(this ZonedDateTime now, int year)
        {
            if (now.Year == year)
            {
                return now;
            }
            
            return new ZonedDateTime(new DateTime(year, now.Month, now.Day).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithMonth(this ZonedDateTime now, int month)
        {
            if (now.Month == month)
            {
                return now;
            }

            if ((0 < month) && (month < 13))
            {
                throw new ArgumentOutOfRangeException();
            }

            
            return new ZonedDateTime(new DateTime(now.Year, month, now.Day).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithDayOfMonth(this ZonedDateTime now, int dayOfMonth)
        {
            if (now.Day == dayOfMonth)
            {
                return now;
            }

            
            return new ZonedDateTime(new DateTime(now.Year, now.Month, dayOfMonth).ToInstant(), now.Zone);
        }

        public static ZonedDateTime WithDayOfYear(this ZonedDateTime now, int dayOfYear)
        {
            if (now.DayOfYear == dayOfYear)
            {
                return now;
            }
            return OfYearDay(now, now.Year, dayOfYear);
        }

        public static ZonedDateTime OfYearDay(this ZonedDateTime now, int year, int dayOfYear)
        {
            ChronoField.YEAR.CheckValidValue(year);
            ChronoField.DAY_OF_YEAR.CheckValidValue(dayOfYear);
            Boolean leap = IsoChronology.GetInstance().IsLeapYear(year);
            if (dayOfYear == 366 && leap == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_DATE_DAYOFYEAR366_IS_NOT_A_LEAP_YEAR, year));
            }
            Month moy = Month.Of((dayOfYear - 1) / 31 + 1);
            int monthEnd = moy.firstDayOfYear(leap) + moy.Length(leap) - 1;
            if (dayOfYear > monthEnd)
            {
                moy = moy.Plus(1);
            }
            int dom = dayOfYear - moy.firstDayOfYear(leap) + 1;
            
            return new ZonedDateTime(new DateTime(year, moy.Value, dom).ToInstant(), now.Zone);
        }

        public static ZonedDateTime OfEpochDay(this ZonedDateTime now, long epochDay)
        {
            ChronoField.EPOCH_DAY.CheckValidValue(epochDay);
            long zeroDay = epochDay + NodaTimeUtility.DAYS_0000_TO_1970;
            // find the march-based year
            zeroDay -= 60;  // adjust to 0000-03-01 so leap day is at end of four year cycle
            long adjust = 0;
            if (zeroDay < 0)
            {
                // adjust negative years to positive for calculation
                long adjustCycles = (zeroDay + 1) / NodaTimeUtility.DAYS_PER_CYCLE - 1;
                adjust = adjustCycles * 400;
                zeroDay += -adjustCycles * NodaTimeUtility.DAYS_PER_CYCLE;
            }
            long yearEst = (400 * zeroDay + 591) / NodaTimeUtility.DAYS_PER_CYCLE;
            long doyEst = zeroDay - (365 * yearEst + yearEst / 4 - yearEst / 100 + yearEst / 400);
            if (doyEst < 0)
            {
                // fix estimate
                yearEst--;
                doyEst = zeroDay - (365 * yearEst + yearEst / 4 - yearEst / 100 + yearEst / 400);
            }
            yearEst += adjust;  // reset any negative year
            int marchDoy0 = (int)doyEst;

            // convert march-based values back to january-based
            int marchMonth0 = (marchDoy0 * 5 + 2) / 153;
            int month = (marchMonth0 + 2) % 12 + 1;
            int dom = marchDoy0 - (marchMonth0 * 306 + 5) / 10 + 1;
            yearEst += marchMonth0 / 10;

            // check year now we are certain it is correct
            int year = (int)ChronoField.YEAR.CheckValidIntValue(yearEst);
            
            return new ZonedDateTime(new DateTime(year, month, dom).ToInstant(), now.Zone);
        }

        public static int Get(this ZonedDateTime now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                return Get0(now, field);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
        }

        public static long GetLong(this ZonedDateTime now, ITemporalField field)
        {
            if (field == ChronoField.EPOCH_DAY)
            {
                return now.ToEpochDay();
            }
            if (field == ChronoField.PROLEPTIC_MONTH)
            {
                return now.GetProlepticMonth();
            }
            return Get0(now, field);
        }

        public static long GetProlepticMonth(this ZonedDateTime now)
        {
            return (now.Year * 12L + now.Month - 1);
        }

        public static long GetProlepticYear(this ZonedDateTime now)
        {
            System.Globalization.Calendar cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            System.DateTime utcDateTime = now.ToDateTimeUtc();

            return cal.GetEra(utcDateTime) - 1;
        }

        public static long ToEpochDay(this ZonedDateTime now)
        {
            TimeSpan t = now.ToDateTimeUtc() - new DateTime(1970, 1, 1);
            //int secondsSinceEpoch = (int)t.TotalSeconds;
            return t.Days;
        }

        public static long ToEpochMonths(this ZonedDateTime now)
        {
            long y = now.Year;
            long m = now.Month;
            long ey = 1970;
            long em = 1;

            return (y - ey) * 12 + (m - em);
        }

        public static long ToEpochYears(this ZonedDateTime now)
        {
            long y = now.Year;
            long ey = 1970;

            return (y - ey);
        }

        public static LocalDate ToLocalDate(this ZonedDateTime now)
        {
            return new LocalDate(now.Year, now.Month, now.Day, now.Calendar);
        }

        public static LocalTime ToLocalTime(this ZonedDateTime now)
        {
            return new LocalTime(now.Hour, now.Minute, now.Second, now.Millisecond);
        }

        public static LocalDateTime ToLocalDateTime(this ZonedDateTime now)
        {
            return new LocalDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, now.Calendar);
        }

        public static ZonedDateTime ToZonedDateTimeUtc(this ZonedDateTime now)
        {
            return now.ToDateTimeUtc().ToZonedDateTime();
        }

        public static ZonedDateTime ToZonedFixingDateTimeUtc(this ZonedDateTime now)
        {
            return now.ToLocalDate().ToDateTimeUnspecified().ToUniversalTime().ToZonedDateTime();
        }

        public static int LengthOfMonth(this ZonedDateTime now)
        {
            switch (now.Month)
            {
                case 2:
                    return (now.IsLeapYear() ? 29 : 28);
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }

        public static int LengthOfYear(this ZonedDateTime now)
        {
            return (now.IsLeapYear() ? 366 : 365);
        }

        public static int GetMinYear(this ZonedDateTime now)
        {
            return NodaTimeUtility.MIN_VALUE;
        }

        public static int GetMaxYear(this ZonedDateTime now)
        {
            return NodaTimeUtility.MAX_VALUE;
        }

        private static int Get0(ZonedDateTime now, ITemporalField field)
        {
            if (field == ChronoField.DAY_OF_WEEK)
            {
                return now.DayOfWeek.GetValue();
            }
            else if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)
            {
                return ((now.Day - 1) % 7) + 1;
            }
            if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)
            {
                return ((now.DayOfYear - 1) % 7) + 1;
            }
            if (field == ChronoField.DAY_OF_MONTH)
            {
                return now.Day;
            }
            if (field == ChronoField.DAY_OF_YEAR)
            {
                return now.DayOfYear;
            }
            if (field == ChronoField.EPOCH_DAY)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().FIELD_TOO_LARGE_FOR_AN_INT, field));
            }
            if (field == ChronoField.ALIGNED_WEEK_OF_MONTH)
            {
                return ((now.Day - 1) / 7) + 1;
            }
            if (field == ChronoField.ALIGNED_WEEK_OF_YEAR)
            {
                return ((now.DayOfYear - 1) / 7) + 1;
            }
            else if (field == ChronoField.MONTH_OF_YEAR)
            {
                return now.Month;
            }
            else if (field == ChronoField.PROLEPTIC_MONTH)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().FIELD_TOO_LARGE_FOR_AN_INT, field));
            }
            else if (field == ChronoField.YEAR_OF_ERA)
            {
                return (now.Year >= 1 ? now.Year : 1 - now.Year);
            }
            else if (field == ChronoField.YEAR)
            {
                return now.Year;
            }
            else if (field == ChronoField.ERA)
            {
                return (now.Year >= 1 ? 1 : 0);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
        }

        public static double GetDifferenceInDays(this ZonedDateTime startDate, ZonedDateTime endDate)
        {
            return GetDifferenceInDays(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInMonths(this ZonedDateTime startDate, ZonedDateTime endDate)
        {
            return GetDifferenceInMonths(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInYears(this ZonedDateTime startDate, ZonedDateTime endDate)
        {
            return GetDifferenceInYears(startDate.ToInstant(), endDate.ToInstant());
        }

        public static long GetEpochSecond(this ZonedDateTime now)
        {
            TimeSpan t = now.ToDateTimeUtc() - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }

        public static double GetExactDaysBetween(this ZonedDateTime startDate, ZonedDateTime endDate)
        {
            return GetExactDaysBetween(startDate.ToInstant(), endDate.ToInstant());
        }

        public static int GetDaysBetween(this ZonedDateTime startDate, ZonedDateTime endDate)
        {
            return GetDaysBetween(startDate.ToInstant(), true, endDate.ToInstant(), false);
        }

        public static int GetDaysBetween(this ZonedDateTime startDate, Boolean includeStart, ZonedDateTime endDate, Boolean includeEnd)
        {
            return GetDaysBetween(startDate.ToInstant(), includeStart, endDate.ToInstant(), includeEnd);
        }

        public static Range GetRange(this ZonedDateTime now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                ChronoField f = (ChronoField)field;
                if (f.IsDateBased)
                {
                    if (f == ChronoField.DAY_OF_MONTH)
                    {
                        return new Range(1, now.LengthOfMonth());
                    }
                    else if (f == ChronoField.DAY_OF_YEAR)
                    {
                        return new Range(1, now.LengthOfYear());
                    }
                    else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH)
                    {
                        return new Range(1, now.Month == Month.FEBRUARY.Value && now.IsLeapYear() == false ? 4 : 5);
                    }
                    else if (f == ChronoField.YEAR_OF_ERA)
                    {
                        return (now.Year <= 0 ? new Range(1, now.GetMaxYear() + 1) : new Range(1, now.GetMaxYear()));
                    }
                    return field.Range;
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
            return field.RangeRefinedBy(new Temporal(now));
        }

        public static ZonedDateTime Plus(this ZonedDateTime now, long amountToAdd, ChronoUnit unit)
        {
            if (unit is ChronoUnit)
            {
                ChronoUnit f = (ChronoUnit)unit;

                if (f == ChronoUnit.DAYS)
                {
                    return now.PlusDays((int)amountToAdd);
                }
                else if (f == ChronoUnit.WEEKS)
                {
                    return now.PlusWeeks((int)amountToAdd);
                }
                else if (f == ChronoUnit.MONTHS)
                {
                    return now.PlusMonths((int)amountToAdd);
                }
                else if (f == ChronoUnit.YEARS)
                {
                    return now.PlusYears((int)amountToAdd);
                }
                else if (f == ChronoUnit.DECADES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 10));
                }
                else if (f == ChronoUnit.CENTURIES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 100));
                }
                else if (f == ChronoUnit.MILLENNIA)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 1000));

                }
                else if (f == ChronoUnit.ERAS)
                {
                    return now.With(ChronoField.ERA, Math2.SafeAdd((int)now.GetLong(ChronoField.ERA), amountToAdd));
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_UNIT, unit));

            }
            return unit.AddTo(new Temporal(now), amountToAdd);
        }

        public static ZonedDateTime ChangeToDifferentTimeZoneWithSameDateTime(this ZonedDateTime now, DateTimeZone zone)
        {
            int dateYear = now.Year;
            int dateMonth = now.Month;
            int dateDay = now.Day;
            int dateHour = now.Hour;
            int dateMinute = now.Minute;
            int dateSecond = now.Second;
            int dateMillisecond = now.Millisecond;

            DateTime current = new DateTime(dateYear, dateMonth, dateDay, dateHour, dateMinute, dateSecond, dateMillisecond, DateTimeKind.Utc);
            if (now.IsDaylightSaving())
            {
                return new ZonedDateTime(current.AddMilliseconds(-zone.MaxOffset.Milliseconds).ToInstant(), zone);
            }
            else
            {
                return new ZonedDateTime(current.AddMilliseconds(-zone.MinOffset.Milliseconds).ToInstant(), zone);
            }
        }

        public static ZonedDateTime ChangeToDifferentTimeZone(this ZonedDateTime now, DateTimeZone zone)
        {
            DateTime nodaTimeUtc = now.ToDateTimeUtc();
            int dateYear = nodaTimeUtc.Year;
            int dateMonth = nodaTimeUtc.Month;
            int dateDay = nodaTimeUtc.Day;

            //DateTime temp = new DateTime(dateYear, dateMonth, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
            //int lastDayOfheMonth = temp.AddMonths(1).AddDays(-1).Day;

            //if (dateDay > lastDayOfheMonth)
            //    dateDay = lastDayOfheMonth;

            int dateHour = nodaTimeUtc.Hour;
            int dateMinute = nodaTimeUtc.Minute;
            int dateSecond = nodaTimeUtc.Second;
            int dateMillisecond = nodaTimeUtc.Millisecond;

            DateTime current = new DateTime(dateYear, dateMonth, dateDay, dateHour, dateMinute, dateSecond, dateMillisecond, DateTimeKind.Utc);
            if (now.IsDaylightSaving())
            {
                return new ZonedDateTime(current.AddHours(1).ToInstant(), zone);
             }
            else
            {
                return new ZonedDateTime(current.ToInstant(), zone);
            }
        }


        /// <summary>
        /// Find out this time zone is in Daylight Saving, now
        /// </summary>
        /// <param name="zone">DateTimeZone to find out</param>
        /// <returns>Return true if the DateTimeZone is in Daylight Saving</returns>
        /// <see cref="https://stackoverflow.com/questions/24373618/getting-daylight-savings-time-start-and-end-in-nodatime"/>
        public static Boolean IsDaylightSaving(this ZonedDateTime now)
        {
            DateTimeZone zone = now.Zone;
            ZonedDateTime tmp = new ZonedDateTime(now.ToInstant(), zone);
            DateTimeOffset dateTimeOffset = tmp.ToDateTimeOffset();
            var timezone = "Europe/London"; //https://nodatime.org/TimeZones
            DateTimeZone london = DateTimeZoneProviders.Tzdb[timezone];
            ZonedDateTime timeInZone = dateTimeOffset.DateTime.ToInstant().InZone(london);
            var instant = timeInZone.ToInstant();
            var zoneInterval = timeInZone.Zone.GetZoneInterval(instant);
            return zoneInterval.Savings != Offset.Zero;
        }


        #endregion

        #region Nullable Methods

        public static Boolean IsAfter(this Nullable<ZonedDateTime> now, ZonedDateTime target)
        {
            if (now is null)
                return false;

            if (DateTime.Compare(now.Value.ToDateTimeUtc(), target.ToDateTimeUtc()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsAfter(this Nullable<ZonedDateTime> now, Nullable<ZonedDateTime> target)
        {
            if (target is null)
                return false;

            return now.IsAfter(target.Value);
        }

        public static Boolean IsBefore(this Nullable<ZonedDateTime> now, ZonedDateTime target)
        {
            if (now is null)
                return false;

            if (DateTime.Compare(now.Value.ToDateTimeUtc(), target.ToDateTimeUtc()) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this Nullable<ZonedDateTime> now, Nullable<ZonedDateTime> target)
        {
            if (target is null)
                return false;

            return now.IsBefore(target.Value);
        }

        public static Boolean IsLeapYear(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return false;

            return now.Value.IsLeapYear();
        }

        public static Boolean IsSupported(this Nullable<ZonedDateTime> now, ITemporalField field)
        {
            if (now is null)
                return false;

            return now.Value.IsSupported(field);
        }

        public static Boolean IsSupported(this Nullable<ZonedDateTime> now, ChronoUnit unit)
        {
            if (now is null)
                return false;

            return now.Value.IsSupported(unit);
        }

        public static Nullable<ZonedDateTime> AdjustDayOfWeek(this Nullable<ZonedDateTime> now, IsoDayOfWeek dayOfWeek)
        {
            if (now is null)
                return null;

            return now.Value.AdjustDayOfWeek(dayOfWeek);
        }

        public static Nullable<ZonedDateTime> GetDateOffsetWithYearFraction(this Nullable<ZonedDateTime> startDate, double yearFraction)
        {
            if (startDate is null)
                return null;

            return startDate.Value.GetDateOffsetWithYearFraction(yearFraction);
        }

        public static Nullable<ZonedDateTime> PlusTime(this Nullable<ZonedDateTime> nodaTime, LocalTime time)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusTime(time);
        }

        public static Nullable<ZonedDateTime> PlusDays(this Nullable<ZonedDateTime> nodaTime, int days)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusDays(days);
        }

        public static Nullable<ZonedDateTime> PlusWeeks(this Nullable<ZonedDateTime> nodaTime, int weeks)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusWeeks(weeks);
        }

        public static Nullable<ZonedDateTime> PlusMonths(this Nullable<ZonedDateTime> nodaTime, int months)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusMonths(months);
        }

        public static Nullable<ZonedDateTime> PlusYears(this Nullable<ZonedDateTime> nodaTime, int years)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusYears(years);
        }

        public static Nullable<ZonedDateTime> With(this Nullable<ZonedDateTime> now, ITemporalAdjuster tmp)
        {
            if (now is null)
                return null;

            return now.Value.With(tmp);
        }

        public static Nullable<ZonedDateTime> With(this Nullable<ZonedDateTime> now, LocalTime time)
        {
            if (now is null)
                return null;

            return now.Value.With(time);
        }

        public static Nullable<ZonedDateTime> WithZoneSameInstant(this Nullable<ZonedDateTime> now, DateTimeZone zone)
        {
            if (now is null)
                return null;

            return now.Value.WithZoneSameInstant(zone);
        }

        public static Nullable<ZonedDateTime> Plus(this Nullable<ZonedDateTime> now, Period period)
        {
            if (now is null)
                return null;

            return now.Value.Plus(period);
        }

        public static IList<Nullable<ZonedDateTime>> SortList(this IList<Nullable<ZonedDateTime>> dates)
        {
            var result = dates;
            int i, j;
            int N = result.Count;

            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if (result[i].IsAfter(result[i + 1]))
                        exchange(result, i, i + 1);
                }
            }
            return result;
        }

        private static void exchange(IList<Nullable<ZonedDateTime>> data, int m, int n)
        {
            Nullable<ZonedDateTime> temporary;

            temporary = data[m];
            data[m] = data[n];
            data[n] = temporary;
        }

        public static Nullable<ZonedDateTime> Minus(this Nullable<ZonedDateTime> now, Period period)
        {
            if (now is null)
                return null;

            return now.Value.Minus(period);
        }

        public static Nullable<ZonedDateTime> Plus(this Nullable<ZonedDateTime> now, long amountToAdd, ITemporalUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Plus(amountToAdd, unit);
        }

        public static Nullable<ZonedDateTime> Minus(this Nullable<ZonedDateTime> now, long amountToSubtract, ITemporalUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Minus(amountToSubtract, unit);
        }

        public static Nullable<ZonedDateTime> With(this Nullable<ZonedDateTime> now, MonthDay monthDay)
        {
            if (now is null)
                return null;

            return now.Value.With(monthDay);
        }

        public static Nullable<ZonedDateTime> With(this Nullable<ZonedDateTime> now, LocalDate localDate)
        {
            if (now is null)
                return null;

            return now.Value.With(localDate);
        }

        public static Nullable<ZonedDateTime> With(this Nullable<ZonedDateTime> now, ITemporalField field, long newValue)
        {
            if (now is null)
                return null;

            return now.Value.With(field, newValue);
        }

        public static Nullable<ZonedDateTime> WithHour(this Nullable<ZonedDateTime> now, int hour)
        {
            if (now is null)
                return null;

            return now.Value.WithHour(hour);
        }

        public static Nullable<ZonedDateTime> WithMinute(this Nullable<ZonedDateTime> now, int minute)
        {
            if (now is null)
                return null;

            return now.Value.WithMinute(minute);
        }

        public static Nullable<ZonedDateTime> WithSecond(this Nullable<ZonedDateTime> now, int second)
        {
            if (now is null)
                return null;

            return now.Value.WithSecond(second);
        }

        public static Nullable<ZonedDateTime> WithNano(this Nullable<ZonedDateTime> now, int nano)
        {
            if (now is null)
                return null;

            return now.Value.WithNano(nano);
        }

        public static Nullable<ZonedDateTime> WithYear(this Nullable<ZonedDateTime> now, int year)
        {
            if (now is null)
                return null;

            return now.Value.WithYear(year);
        }

        public static Nullable<ZonedDateTime> WithMonth(this Nullable<ZonedDateTime> now, int month)
        {
            if (now is null)
                return null;

            return now.Value.WithMonth(month);
        }

        public static Nullable<ZonedDateTime> WithDayOfMonth(this Nullable<ZonedDateTime> now, int dayOfMonth)
        {
            if (now is null)
                return null;

            return now.Value.WithDayOfMonth(dayOfMonth);
        }

        public static Nullable<ZonedDateTime> WithDayOfYear(this Nullable<ZonedDateTime> now, int dayOfYear)
        {
            if (now is null)
                return null;

            return now.Value.WithDayOfYear(dayOfYear);
        }

        public static Nullable<ZonedDateTime> OfYearDay(this Nullable<ZonedDateTime> now, int year, int dayOfYear)
        {
            if (now is null)
                return null;

            return now.Value.OfYearDay(year, dayOfYear);
        }

        public static Nullable<ZonedDateTime> OfEpochDay(this Nullable<ZonedDateTime> now, long epochDay)
        {
            if (now is null)
                return null;

            return now.Value.OfEpochDay(epochDay);
        }

        public static int Get(this Nullable<ZonedDateTime> now, ITemporalField field)
        {
            if (now is null)
                return -1;

            return now.Value.Get(field);
        }

        public static long GetLong(this Nullable<ZonedDateTime> now, ITemporalField field)
        {
            if (now is null)
                return -1;

            return now.Value.GetLong(field);
        }

        public static long GetProlepticMonth(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetProlepticMonth();
        }

        public static long GetProlepticYear(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetProlepticYear();
        }

        public static long ToEpochDay(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochDay();
        }

        public static long ToEpochMonths(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochMonths();
        }

        public static long ToEpochYears(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochYears();
        }

        public static Nullable<LocalDate> ToLocalDate(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return null;

            return now.Value.ToLocalDate();
        }

        public static Nullable<LocalTime> ToLocalTime(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return null;

            return now.Value.ToLocalTime();
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUtc(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedDateTimeUtc();
        }

        public static Nullable<ZonedDateTime> ToZonedFixingDateTimeUtc(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedFixingDateTimeUtc();
        }

        public static int LengthOfMonth(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.LengthOfMonth();
        }

        public static int LengthOfYear(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.LengthOfYear();
        }

        public static int GetMinYear(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetMinYear();
        }

        public static int GetMaxYear(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetMaxYear();
        }

        public static double GetDifferenceInDays(this Nullable<ZonedDateTime> startDate, ZonedDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDifferenceInDays(endDate);
        }

        public static double GetDifferenceInMonths(this Nullable<ZonedDateTime> startDate, ZonedDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDifferenceInMonths(endDate);
        }

        public static double GetDifferenceInYears(this Nullable<ZonedDateTime> startDate, ZonedDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDifferenceInYears(endDate);
        }

        public static long GetEpochSecond(this Nullable<ZonedDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetEpochSecond();
        }

        public static double GetExactDaysBetween(this Nullable<ZonedDateTime> startDate, ZonedDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetExactDaysBetween(endDate);
        }

        public static int GetDaysBetween(this Nullable<ZonedDateTime> startDate, ZonedDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDaysBetween(endDate);
        }

        public static int GetDaysBetween(this Nullable<ZonedDateTime> startDate, Boolean includeStart, ZonedDateTime endDate, Boolean includeEnd)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDaysBetween(includeStart, endDate, includeEnd);
        }

        public static Nullable<ZonedDateTime> Plus(this Nullable<ZonedDateTime> now, long amountToAdd, ChronoUnit unit)
        {
            if (now.HasValue)
                return now.Value.Plus(amountToAdd, unit);
            else
                return null;
        }
        #endregion
        #endregion

        #region LodalDateTime methods
        #region Regular

        public static Nullable<LocalDateTime> ToNullable(this LocalDateTime now)
        {
            LocalDateTime? ret = now;
            return ret;
        }

        public static int CompareTo(this LocalDateTime now, LocalDateTime target)
        {
            return DateTime.Compare(now.ToDateTimeUnspecified(), target.ToDateTimeUnspecified());
        }

        public static Boolean IsAfter(this LocalDateTime now, LocalDateTime target)
        {
            if (DateTime.Compare(now.ToDateTimeUnspecified(), target.ToDateTimeUnspecified()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this LocalDateTime now, LocalDateTime target)
        {
            if (DateTime.Compare(now.ToDateTimeUnspecified(), target.ToDateTimeUnspecified()) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static LocalDateTime AdjustDayOfWeek(this LocalDateTime now, IsoDayOfWeek dayOfWeek)
        {
            IsoDayOfWeek currentDayOfWeek = now.DayOfWeek;
            int diffDayOfWeek = now.DayOfWeek - dayOfWeek;

            return now.PlusDays(diffDayOfWeek);
        }

        public static Boolean IsLeapYear(this LocalDateTime now)
        {
            return DateTime.IsLeapYear(now.Year);
        }

        public static Boolean IsSupported(this LocalDateTime now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                return field.IsDateBased;
            }
            return field != null && field.IsSupportedBy(new Temporal(now.ToZonedDateTime()));
        }

        public static Boolean IsSupported(this LocalDateTime now, ChronoUnit field)
        {
            if (field is ChronoUnit)
            {
                return field.IsDateBased;
            }
            return field != null && field.IsSupportedBy(new Temporal(now.ToZonedDateTime()));
        }

        public static int GetDaysOfMonth(this LocalDateTime now)
        {
            var tmp = now.PlusMonths(1);
            var eom = (new LocalDateTime(tmp.Year, tmp.Month, 1, tmp.Hour, tmp.Minute, tmp.Second)).PlusDays(-1);

            return eom.Day;
        }

        public static int PeriodUntil(this LocalDateTime now, LocalDateTime end, ChronoUnit span)
        {
            if (span == ChronoUnit.NANOS)
                throw new NotSupportedException(LocalizedResources.Instance().NANO_SECOND_BASE_IS_NOT_SUPPORTED);
            if (span == ChronoUnit.MICROS)
                throw new NotSupportedException(LocalizedResources.Instance().MICRO_SECOND_BASE_IS_NOT_SUPPORTED);
            if (span == ChronoUnit.MILLIS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalMilliseconds;
            if (span == ChronoUnit.SECONDS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalSeconds;
            if (span == ChronoUnit.MINUTES)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalMinutes;
            if (span == ChronoUnit.HOURS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalHours;
            if (span == ChronoUnit.DAYS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalDays;
            if (span == ChronoUnit.MONTHS)
                return (int)(now.ToDateTimeUnspecified().Date.GetDifferenceInMonths(end.ToDateTimeUnspecified().Date));
            if (span == ChronoUnit.YEARS)
                return (int)(now.ToDateTimeUnspecified().Date.GetDifferenceInYears(end.ToDateTimeUnspecified().Date));

            return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalDays;
        }

        public static Period Until(this LocalDateTime now, LocalDateTime end)
        {
            //LocalDateTime end = LocalDateTime.From(target);
            //long totalMonths = end.GetProlepticMonth() - now.GetProlepticMonth();  // safe
            //int days = end.Day - now.Day;
            //if (totalMonths > 0 && days < 0)
            //{
            //    totalMonths--;
            //    LocalDateTime calcDate = now.PlusMonths((int)totalMonths);
            //    days = (int)(end.ToEpochDay() - calcDate.ToEpochDay());  // safe
            //}
            //else if (totalMonths < 0 && days > 0)
            //{
            //    totalMonths++;
            //    days -= end.LengthOfMonth();
            //}
            //long years = totalMonths / 12;  // safe
            //int months = (int)(totalMonths % 12);  // safe
            return Period.FromDays((int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalDays);
        }

        public static long GetProlepticMonth(this LocalDateTime now)
        {
            return (now.Year * 12L + now.Month - 1);
        }

        public static long GetProlepticYear(this LocalDateTime now)
        {
            System.Globalization.Calendar cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            System.DateTime utcDateTime = now.ToDateTimeUnspecified();

            return cal.GetEra(utcDateTime) - 1;
        }

        public static long ToEpochDay(this LocalDateTime now)
        {
            long y = now.Year;
            long m = now.Month;
            long total = 0;
            total += 365 * y;
            if (y >= 0)
            {
                total += (y + 3) / 4 - (y + 99) / 100 + (y + 399) / 400;
            }
            else
            {
                total -= y / -4 - y / -100 + y / -400;
            }
            total += ((367 * m - 362) / 12);
            total += now.Day - 1;
            if (m > 2)
            {
                total--;
                if (now.IsLeapYear() == false)
                {
                    total--;
                }
            }
            return total - NodaTimeUtility.DAYS_0000_TO_1970;
        }

        public static long ToEpochMonths(this LocalDateTime now)
        {
            long y = now.Year;
            long m = now.Month;
            long ey = 1970;
            long em = 1;

            return (y - ey) * 12 + (m - em);
        }

        public static long ToEpochYears(this LocalDateTime now)
        {
            long y = now.Year;
            long ey = 1970;

            return (y - ey);
        }

        public static int LengthOfMonth(this LocalDateTime now)
        {
            switch (now.Month)
            {
                case 2:
                    return (now.IsLeapYear() ? 29 : 28);
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }

        public static int LengthOfYear(this LocalDateTime now)
        {
            return (now.IsLeapYear() ? 366 : 365);
        }

        public static int GetMinYear(this LocalDateTime now)
        {
            return NodaTimeUtility.MIN_VALUE;
        }

        public static int GetMaxYear(this LocalDateTime now)
        {
            return NodaTimeUtility.MAX_VALUE;
        }

        public static LocalDateTime With(this LocalDateTime now, ITemporalAdjuster tmp)
        {
            return tmp.AdjustInto(new Temporal(now.ToZonedDateTime())).GetOriginal();
        }

        public static LocalDateTime With(this LocalDateTime now, ITemporalField field, long newValue)
        {
            if (field is ChronoField)
            {
                ChronoField f = (ChronoField)field;
                f.CheckValidValue(newValue);
                if (f == ChronoField.DAY_OF_WEEK)
                {
                    return now.PlusDays((int)newValue - now.DayOfWeek.GetValue());
                }
                else if (f == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)
                {
                    return now.PlusDays((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH));
                }
                else if (f == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)
                {
                    return now.PlusDays((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR));
                }
                else if (f == ChronoField.DAY_OF_MONTH)
                {
                    return now.WithDayOfMonth((int)newValue);
                }
                else if (f == ChronoField.DAY_OF_YEAR)
                {
                    return now.WithDayOfYear((int)newValue);
                }
                else if (f == ChronoField.EPOCH_DAY)
                {
                    return now.OfEpochDay(newValue);
                }
                else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH)
                {
                    return now.PlusWeeks((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_WEEK_OF_MONTH));
                }
                else if (f == ChronoField.ALIGNED_WEEK_OF_YEAR)
                {
                    return now.PlusWeeks((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_WEEK_OF_YEAR));
                }
                else if (f == ChronoField.MONTH_OF_YEAR)
                {
                    return now.WithMonth((int)newValue);
                }
                else if (f == ChronoField.PROLEPTIC_MONTH)
                {
                    return now.PlusMonths((int)newValue - (int)now.GetLong(ChronoField.PROLEPTIC_MONTH));
                }
                else if (f == ChronoField.YEAR_OF_ERA)
                {
                    return now.WithYear((int)(now.Year >= 1 ? newValue : 1 - newValue));
                }
                else if (f == ChronoField.YEAR)
                {
                    return now.WithYear((int)newValue);
                }
                else if (f == ChronoField.ERA)
                {
                    return (now.GetLong(ChronoField.ERA) == newValue ? now : now.WithYear(1 - now.Year));
                }


                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
            return field.AdjustInto(new Temporal(now.ToZonedDateTime()), newValue).GetOriginal();
        }

        public static LocalDateTime With(this LocalDateTime now, MonthDay monthDay)
        {
            return LocalDateTime.FromDateTime(new DateTime(now.Year, monthDay.Month.Value, monthDay.Day));
        }

        public static LocalDateTime With(this LocalDateTime now, LocalDate localDate)
        {
            return new LocalDateTime(localDate.Year, localDate.Month, localDate.Day, now.Hour, now.Minute, now.Second);
        }

        public static LocalDateTime With(this LocalDateTime now, LocalTime time)
        {
            return now.WithHour(time.Hour).WithMinute(time.Minute).WithSecond(time.Second).WithNano(time.NanosecondOfSecond);
        }

        public static LocalDateTime WithHour(this LocalDateTime now, int hour)
        {
            if (now.Hour == hour)
            {
                return now;
            }

            return new LocalDateTime(now.Year, now.Month, now.Day, hour, now.Minute, now.Second);
        }

        public static LocalDateTime WithMinute(this LocalDateTime now, int minute)
        {
            if (now.Minute == minute)
            {
                return now;
            }

            return new LocalDateTime(now.Year, now.Month, now.Day, now.Hour, minute, now.Second);
        }

        public static LocalDateTime WithSecond(this LocalDateTime now, int second)
        {
            if (now.Second == second)
            {
                return now;
            }

            return new LocalDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, second);
        }

        public static LocalDateTime WithNano(this LocalDateTime now, int nano)
        {
            if (now.NanosecondOfSecond == nano)
            {
                return now;
            }

            return new LocalDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }

        public static LocalDateTime WithYear(this LocalDateTime now, int year)
        {
            if (now.Year == year)
            {
                return now;
            }
            return new LocalDateTime(year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }

        public static LocalDateTime WithMonth(this LocalDateTime now, int month)
        {
            if (now.Month == month)
            {
                return now;
            }

            if ((0 < month) && (month < 13))
            {
                throw new ArgumentOutOfRangeException();
            }

            return new LocalDateTime(now.Year, month, now.Day, now.Hour, now.Minute, now.Second);
        }

        public static LocalDateTime WithDayOfMonth(this LocalDateTime now, int dayOfMonth)
        {
            if (now.Day == dayOfMonth)
            {
                return now;
            }

            return new LocalDateTime(now.Year, now.Month, dayOfMonth, now.Hour, now.Minute, now.Second);
        }

        public static LocalDateTime WithDayOfYear(this LocalDateTime now, int dayOfYear)
        {
            if (now.DayOfYear == dayOfYear)
            {
                return now;
            }
            return OfYearDay(now, now.Year, dayOfYear);
        }

        public static LocalDateTime Plus(this LocalDateTime now, long amountToAdd, ITemporalUnit unit)
        {
            if (unit is ChronoUnit)
            {
                ChronoUnit f = (ChronoUnit)unit;

                if (f == ChronoUnit.DAYS)
                {
                    return now.PlusDays((int)amountToAdd);
                }
                else if (f == ChronoUnit.WEEKS)
                {
                    return now.PlusWeeks((int)amountToAdd);
                }
                else if (f == ChronoUnit.MONTHS)
                {
                    return now.PlusMonths((int)amountToAdd);
                }
                else if (f == ChronoUnit.YEARS)
                {
                    return now.PlusYears((int)amountToAdd);
                }
                else if (f == ChronoUnit.DECADES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 10));
                }
                else if (f == ChronoUnit.CENTURIES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 100));
                }
                else if (f == ChronoUnit.MILLENNIA)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 1000));
                }
                else if (f == ChronoUnit.ERAS)
                {
                    return now.With(ChronoField.ERA, Math2.SafeAdd(now.GetLong(ChronoField.ERA), amountToAdd));
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_UNIT, unit));
            }
            return unit.AddTo(new Temporal(now.ToZonedDateTime()), amountToAdd);
        }

        public static LocalDateTime Minus(this LocalDateTime now, long amountToSubtract, ITemporalUnit unit)
        {
            return (amountToSubtract == long.MinValue ? now.Plus(long.MaxValue, unit).Plus(1, unit) : now.Plus(-amountToSubtract, unit));
        }

        public static LocalDateTime OfYearDay(this LocalDateTime now, int year, int dayOfYear)
        {
            ChronoField.YEAR.CheckValidValue(year);
            ChronoField.DAY_OF_YEAR.CheckValidValue(dayOfYear);
            Boolean leap = IsoChronology.GetInstance().IsLeapYear(year);
            if (dayOfYear == 366 && leap == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_DATE_DAYOFYEAR366_IS_NOT_A_LEAP_YEAR, year ));
            }
            Month moy = Month.Of((dayOfYear - 1) / 31 + 1);
            int monthEnd = moy.firstDayOfYear(leap) + moy.Length(leap) - 1;
            if (dayOfYear > monthEnd)
            {
                moy = moy.Plus(1);
            }
            int dom = dayOfYear - moy.firstDayOfYear(leap) + 1;
            return LocalDateTime.FromDateTime(new DateTime(year, moy.Value, dom));
        }

        public static LocalDateTime OfEpochDay(this LocalDateTime now, long epochDay)
        {
            ChronoField.EPOCH_DAY.CheckValidValue(epochDay);
            long zeroDay = epochDay + NodaTimeUtility.DAYS_0000_TO_1970;
            // find the march-based year
            zeroDay -= 60;  // adjust to 0000-03-01 so leap day is at end of four year cycle
            long adjust = 0;
            if (zeroDay < 0)
            {
                // adjust negative years to positive for calculation
                long adjustCycles = (zeroDay + 1) / NodaTimeUtility.DAYS_PER_CYCLE - 1;
                adjust = adjustCycles * 400;
                zeroDay += -adjustCycles * NodaTimeUtility.DAYS_PER_CYCLE;
            }
            long yearEst = (400 * zeroDay + 591) / NodaTimeUtility.DAYS_PER_CYCLE;
            long doyEst = zeroDay - (365 * yearEst + yearEst / 4 - yearEst / 100 + yearEst / 400);
            if (doyEst < 0)
            {
                // fix estimate
                yearEst--;
                doyEst = zeroDay - (365 * yearEst + yearEst / 4 - yearEst / 100 + yearEst / 400);
            }
            yearEst += adjust;  // reset any negative year
            int marchDoy0 = (int)doyEst;

            // convert march-based values back to january-based
            int marchMonth0 = (marchDoy0 * 5 + 2) / 153;
            int month = (marchMonth0 + 2) % 12 + 1;
            int dom = marchDoy0 - (marchMonth0 * 306 + 5) / 10 + 1;
            yearEst += marchMonth0 / 10;

            // check year now we are certain it is correct
            int year = (int)ChronoField.YEAR.CheckValidIntValue(yearEst);
            return new LocalDateTime(year, month, dom, now.Hour, now.Minute, now.Second);
        }

        public static int Get(this LocalDateTime now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                return Get0(now, field);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
        }

        public static long GetLong(this LocalDateTime now, ITemporalField field)
        {
            if (field == ChronoField.EPOCH_DAY)
            {
                return now.ToEpochDay();
            }
            if (field == ChronoField.PROLEPTIC_MONTH)
            {
                return now.GetProlepticMonth();
            }
            return Get0(now, field);
        }

        private static int Get0(LocalDateTime now, ITemporalField field)
        {
            if (field == ChronoField.DAY_OF_WEEK)
            {
                return now.DayOfWeek.GetValue();
            }
            else if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)
            {
                return ((now.Day - 1) % 7) + 1;
            }
            if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)
            {
                return ((now.DayOfYear - 1) % 7) + 1;
            }
            if (field == ChronoField.DAY_OF_MONTH)
            {
                return now.Day;
            }
            if (field == ChronoField.DAY_OF_YEAR)
            {
                return now.DayOfYear;
            }
            if (field == ChronoField.EPOCH_DAY)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().FIELD_TOO_LARGE_FOR_AN_INT, field));
            }
            if (field == ChronoField.ALIGNED_WEEK_OF_MONTH)
            {
                return ((now.Day - 1) / 7) + 1;
            }
            if (field == ChronoField.ALIGNED_WEEK_OF_YEAR)
            {
                return ((now.DayOfYear - 1) / 7) + 1;
            }
            else if (field == ChronoField.MONTH_OF_YEAR)
            {
                return now.Month;
            }
            else if (field == ChronoField.PROLEPTIC_MONTH)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().FIELD_TOO_LARGE_FOR_AN_INT, field));
            }
            else if (field == ChronoField.YEAR_OF_ERA)
            {
                return (now.Year >= 1 ? now.Year : 1 - now.Year);
            }
            else if (field == ChronoField.YEAR)
            {
                return now.Year;
            }
            else if (field == ChronoField.ERA)
            {
                return (now.Year >= 1 ? 1 : 0);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
        }

        public static Instant ToInstant(this LocalDateTime source)
        {
            return Instant.FromDateTimeUtc(source.ToDateTimeUnspecified());
        }

        public static ZonedDateTime ToZonedDateTime(this LocalDateTime source)
        {
             
            return new ZonedDateTime(source.ToInstant(), NodaTimeUtility.ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime ToZonedDateTime(this LocalDateTime source, DateTimeZone zone)
        {

            return new ZonedDateTime(source.ToInstant(), zone);
        }

        public static LocalDate ToLocalDate(this LocalDateTime now)
        {
            
            System.Globalization.Calendar cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;

            return new LocalDate(now.Year, now.Month, now.Day, now.Calendar);
        }

        public static LocalTime ToLocalTime(this LocalDateTime now)
        {
            return new LocalTime(now.Hour, now.Minute, now.Second, now.Millisecond);
        }

        //public static LocalDateTime ToLocalDateTimeUtc(this LocalDateTime now)
        //{
        //    return new LocalDateTime( now.ToDateTimeUnspecified().ToUniversalTime();
        //}

        //public static LocalDateTime ToZonedFixingDateTimeUtc(this LocalDateTime now)
        //{
        //    return now.ToLocalDate().ToDateTimeUnspecified().ToUniversalTime().ToLocalDateTime();
        //}

        public static double GetDifferenceInDays(this LocalDateTime startDate, LocalDateTime endDate)
        {
            return GetDifferenceInDays(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInMonths(this LocalDateTime startDate, LocalDateTime endDate)
        {
            return GetDifferenceInMonths(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInYears(this LocalDateTime startDate, LocalDateTime endDate)
        {
            return GetDifferenceInYears(startDate.ToInstant(), endDate.ToInstant());
        }

        public static long GetEpochSecond(this LocalDateTime now)
        {
            TimeSpan t = now.ToDateTimeUnspecified() - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }

        public static double GetExactDaysBetween(this LocalDateTime startDate, LocalDateTime endDate)
        {
            return GetExactDaysBetween(startDate.ToInstant(), endDate.ToInstant());
        }

        public static int GetDaysBetween(this LocalDateTime startDate, LocalDateTime endDate)
        {
            return GetDaysBetween(startDate.ToInstant(), true, endDate.ToInstant(), false);
        }

        public static int GetDaysBetween(this LocalDateTime startDate, Boolean includeStart, LocalDateTime endDate, Boolean includeEnd)
        {
            return GetDaysBetween(startDate.ToInstant(), includeStart, endDate.ToInstant(), includeEnd);
        }

        public static Range GetRange(this LocalDateTime now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                ChronoField f = (ChronoField)field;
                if (f.IsDateBased)
                {
                    if (f == ChronoField.DAY_OF_MONTH)
                    {
                        return new Range(1, now.LengthOfMonth());
                    }
                    else if (f == ChronoField.DAY_OF_YEAR)
                    {
                        return new Range(1, now.LengthOfYear());
                    }
                    else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH)
                    {
                        return new Range(1, now.Month == Month.FEBRUARY.Value && now.IsLeapYear() == false ? 4 : 5);
                    }
                    else if (f == ChronoField.YEAR_OF_ERA)
                    {
                        return (now.Year <= 0 ? new Range(1, now.GetMaxYear() + 1) : new Range(1, now.GetMaxYear()));
                    }
                    return field.Range;
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
            return field.RangeRefinedBy(new Temporal(now.ToZonedDateTime()));
        }

        public static LocalDateTime Plus(this LocalDateTime now, long amountToAdd, ChronoUnit unit)
        {
            if (unit is ChronoUnit)
            {
                ChronoUnit f = (ChronoUnit)unit;

                if (f == ChronoUnit.DAYS)
                {
                    return now.PlusDays((int)amountToAdd);
                }
                else if (f == ChronoUnit.WEEKS)
                {
                    return now.PlusWeeks((int)amountToAdd);
                }
                else if (f == ChronoUnit.MONTHS)
                {
                    return now.PlusMonths((int)amountToAdd);
                }
                else if (f == ChronoUnit.YEARS)
                {
                    return now.PlusYears((int)amountToAdd);
                }
                else if (f == ChronoUnit.DECADES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 10));
                }
                else if (f == ChronoUnit.CENTURIES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 100));
                }
                else if (f == ChronoUnit.MILLENNIA)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 1000));

                }
                else if (f == ChronoUnit.ERAS)
                {
                    return now.With(ChronoField.ERA, Math2.SafeAdd((int)now.GetLong(ChronoField.ERA), amountToAdd));
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_UNIT, unit));

            }
            return unit.AddTo(new Temporal(now.ToZonedDateTime()), amountToAdd);
        }
        #endregion

        #region Nullable Methods
        public static Boolean IsAfter(this Nullable<LocalDateTime> now, LocalDateTime target)
        {
            if (now is null)
                return false;

            if (DateTime.Compare(now.Value.ToDateTimeUnspecified(), target.ToDateTimeUnspecified()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsAfter(this Nullable<LocalDateTime> now, Nullable<LocalDateTime> target)
        {
            if (target is null)
                return false;

            return now.IsAfter(target.Value);
        }

        public static Boolean IsBefore(this Nullable<LocalDateTime> now, LocalDateTime target)
        {
            if (now is null)
                return false;

            if (DateTime.Compare(now.Value.ToDateTimeUnspecified(), target.ToDateTimeUnspecified()) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this Nullable<LocalDateTime> now, Nullable<LocalDateTime> target)
        {
            if (target is null)
                return false;

            return now.IsBefore(target.Value);
        }

        public static Boolean IsLeapYear(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return false;

            return now.Value.IsLeapYear();
        }

        public static Boolean IsSupported(this Nullable<LocalDateTime> now, ITemporalField field)
        {
            if (now is null)
                return false;

            return now.Value.IsSupported(field);
        }

        public static Boolean IsSupported(this Nullable<LocalDateTime> now, ChronoUnit unit)
        {
            if (now is null)
                return false;

            return now.Value.IsSupported(unit);
        }

        public static Nullable<LocalDateTime> AdjustDayOfWeek(this Nullable<LocalDateTime> now, IsoDayOfWeek dayOfWeek)
        {
            if (now is null)
                return null;

            return now.Value.AdjustDayOfWeek(dayOfWeek);
        }

        public static Nullable<LocalDateTime> PlusDays(this Nullable<LocalDateTime> nodaTime, int days)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusDays(days);
        }

        public static Nullable<LocalDateTime> PlusWeeks(this Nullable<LocalDateTime> nodaTime, int weeks)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusWeeks(weeks);
        }

        public static Nullable<LocalDateTime> PlusMonths(this Nullable<LocalDateTime> nodaTime, int months)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusMonths(months);
        }

        public static Nullable<LocalDateTime> PlusYears(this Nullable<LocalDateTime> nodaTime, int years)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusYears(years);
        }

        public static Nullable<LocalDateTime> With(this Nullable<LocalDateTime> now, ITemporalAdjuster tmp)
        {
            if (now is null)
                return null;

            return now.Value.With(tmp);
        }

        public static Nullable<LocalDateTime> With(this Nullable<LocalDateTime> now, LocalTime time)
        {
            if (now is null)
                return null;

            return now.Value.With(time);
        }

        public static Nullable<LocalDateTime> Plus(this Nullable<LocalDateTime> now, Period period)
        {
            if (now is null)
                return null;

            return now.Value.Plus(period);
        }

        public static IList<Nullable<LocalDateTime>> SortList(this IList<Nullable<LocalDateTime>> dates)
        {
            var result = dates;
            int i, j;
            int N = result.Count;

            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if (result[i].IsAfter(result[i + 1]))
                        exchange(result, i, i + 1);
                }
            }
            return result;
        }

        private static void exchange(IList<Nullable<LocalDateTime>> data, int m, int n)
        {
            Nullable<LocalDateTime> temporary;

            temporary = data[m];
            data[m] = data[n];
            data[n] = temporary;
        }

        public static Nullable<LocalDateTime> Minus(this Nullable<LocalDateTime> now, Period period)
        {
            if (now is null)
                return null;

            return now.Value.Minus(period);
        }

        public static Nullable<LocalDateTime> Plus(this Nullable<LocalDateTime> now, long amountToAdd, ITemporalUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Plus(amountToAdd, unit);
        }

        public static Nullable<LocalDateTime> Minus(this Nullable<LocalDateTime> now, long amountToSubtract, ITemporalUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Minus(amountToSubtract, unit);
        }

        public static Nullable<LocalDateTime> With(this Nullable<LocalDateTime> now, MonthDay monthDay)
        {
            if (now is null)
                return null;

            return now.Value.With(monthDay);
        }

        public static Nullable<LocalDateTime> With(this Nullable<LocalDateTime> now, LocalDate localDate)
        {
            if (now is null)
                return null;

            return now.Value.With(localDate);
        }

        public static Nullable<LocalDateTime> With(this Nullable<LocalDateTime> now, ITemporalField field, long newValue)
        {
            if (now is null)
                return null;

            return now.Value.With(field, newValue);
        }

        public static Nullable<LocalDateTime> WithHour(this Nullable<LocalDateTime> now, int hour)
        {
            if (now is null)
                return null;

            return now.Value.WithHour(hour);
        }

        public static Nullable<LocalDateTime> WithMinute(this Nullable<LocalDateTime> now, int minute)
        {
            if (now is null)
                return null;

            return now.Value.WithMinute(minute);
        }

        public static Nullable<LocalDateTime> WithSecond(this Nullable<LocalDateTime> now, int second)
        {
            if (now is null)
                return null;

            return now.Value.WithSecond(second);
        }

        public static Nullable<LocalDateTime> WithNano(this Nullable<LocalDateTime> now, int nano)
        {
            if (now is null)
                return null;

            return now.Value.WithNano(nano);
        }

        public static Nullable<LocalDateTime> WithYear(this Nullable<LocalDateTime> now, int year)
        {
            if (now is null)
                return null;

            return now.Value.WithYear(year);
        }

        public static Nullable<LocalDateTime> WithMonth(this Nullable<LocalDateTime> now, int month)
        {
            if (now is null)
                return null;

            return now.Value.WithMonth(month);
        }

        public static Nullable<LocalDateTime> WithDayOfMonth(this Nullable<LocalDateTime> now, int dayOfMonth)
        {
            if (now is null)
                return null;

            return now.Value.WithDayOfMonth(dayOfMonth);
        }

        public static Nullable<LocalDateTime> WithDayOfYear(this Nullable<LocalDateTime> now, int dayOfYear)
        {
            if (now is null)
                return null;

            return now.Value.WithDayOfYear(dayOfYear);
        }

        public static Nullable<LocalDateTime> OfYearDay(this Nullable<LocalDateTime> now, int year, int dayOfYear)
        {
            if (now is null)
                return null;

            return now.Value.OfYearDay(year, dayOfYear);
        }

        public static Nullable<LocalDateTime> OfEpochDay(this Nullable<LocalDateTime> now, long epochDay)
        {
            if (now is null)
                return null;

            return now.Value.OfEpochDay(epochDay);
        }

        public static int Get(this Nullable<LocalDateTime> now, ITemporalField field)
        {
            if (now is null)
                return -1;

            return now.Value.Get(field);
        }

        public static long GetLong(this Nullable<LocalDateTime> now, ITemporalField field)
        {
            if (now is null)
                return -1;

            return now.Value.GetLong(field);
        }

        public static long GetProlepticMonth(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetProlepticMonth();
        }

        public static long GetProlepticYear(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetProlepticYear();
        }

        public static long ToEpochDay(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochDay();
        }

        public static long ToEpochMonths(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochMonths();
        }

        public static long ToEpochYears(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochYears();
        }

        public static Nullable<LocalDate> ToLocalDate(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return null;

            return now.Value.ToLocalDate();
        }

        public static Nullable<LocalTime> ToLocalTime(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return null;

            return now.Value.ToLocalTime();
        }

        //public static Nullable<LocalDateTime> ToLocalDateTimeUtc(this Nullable<LocalDateTime> now)
        //{
        //    if (now is null)
        //        return null;

        //    return now.Value.ToLocalDateTimeUtc();
        //}

        //public static Nullable<LocalDateTime> ToZonedFixingDateTimeUtc(this Nullable<LocalDateTime> now)
        //{
        //    if (now is null)
        //        return null;

        //    return now.Value.ToZonedFixingDateTimeUtc();
        //}

        public static int LengthOfMonth(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.LengthOfMonth();
        }

        public static int LengthOfYear(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.LengthOfYear();
        }

        public static int GetMinYear(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetMinYear();
        }

        public static int GetMaxYear(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetMaxYear();
        }

        public static double GetDifferenceInDays(this Nullable<LocalDateTime> startDate, LocalDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDifferenceInDays(endDate);
        }

        public static double GetDifferenceInMonths(this Nullable<LocalDateTime> startDate, LocalDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDifferenceInMonths(endDate);
        }

        public static double GetDifferenceInYears(this Nullable<LocalDateTime> startDate, LocalDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDifferenceInYears(endDate);
        }

        public static long GetEpochSecond(this Nullable<LocalDateTime> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetEpochSecond();
        }

        public static double GetExactDaysBetween(this Nullable<LocalDateTime> startDate, LocalDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetExactDaysBetween(endDate);
        }

        public static int GetDaysBetween(this Nullable<LocalDateTime> startDate, LocalDateTime endDate)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDaysBetween(endDate);
        }

        public static int GetDaysBetween(this Nullable<LocalDateTime> startDate, Boolean includeStart, LocalDateTime endDate, Boolean includeEnd)
        {
            if (startDate is null)
                return -1;

            return startDate.Value.GetDaysBetween(includeStart, endDate, includeEnd);
        }

        public static Nullable<LocalDateTime> Plus(this Nullable<LocalDateTime> now, long amountToAdd, ChronoUnit unit)
        {
            if (now.HasValue)
                return now.Value.Plus(amountToAdd, unit);
            else
                return null;
        }
        #endregion

        #endregion

        #region LodalDate methods

        #region Regular Methods

        public static Nullable<LocalDate> ToNullable(this LocalDate now)
        {
            LocalDate? ret = now;
            return ret;
        }

        public static ZonedDateTime AtZone(this LocalDate now, DateTimeZone zone)
        {
            return new ZonedDateTime(now.ToInstant(), zone);
        }

        public static int CompareTo(this LocalDate now, LocalDate target)
        {
            return DateTime.Compare(now.ToDateTimeUnspecified(), target.ToDateTimeUnspecified());
        }

        public static Boolean IsAfter(this LocalDate now, LocalDate target)
        {
            if (DateTime.Compare(now.ToDateTimeUnspecified(), target.ToDateTimeUnspecified()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this LocalDate now, LocalDate target)
        {
            if (DateTime.Compare(now.ToDateTimeUnspecified(), target.ToDateTimeUnspecified()) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static LocalDate AdjustDayOfWeek(this LocalDate now, IsoDayOfWeek dayOfWeek)
        {
            IsoDayOfWeek currentDayOfWeek = now.DayOfWeek;
            int diffDayOfWeek = now.DayOfWeek - dayOfWeek;

            return now.PlusDays(diffDayOfWeek);
        }

        public static Boolean IsLeapYear(this LocalDate now)
        {
            return DateTime.IsLeapYear(now.Year);
        }

        public static Boolean IsSupported(this LocalDate now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                return field.IsDateBased;
            }
            return field != null && field.IsSupportedBy(new Temporal(now));
        }

        public static Boolean IsSupported(this LocalDate now, ChronoUnit field)
        {
            if (field is ChronoUnit)
            {
                return field.IsDateBased;
            }
            return field != null && field.IsSupportedBy(new Temporal(now));
        }

        public static int GetDaysOfMonth(this LocalDate now)
        {
            var tmp = now.PlusMonths(1);
            var eom = (new LocalDate(tmp.Year, tmp.Month, 1)).PlusDays(-1);

            return eom.Day;
        }

        public static int PeriodUntil(this LocalDate now, LocalDate end, ChronoUnit span)
        {
            if (span == ChronoUnit.NANOS)
                throw new NotSupportedException(LocalizedResources.Instance().NANO_SECOND_BASE_IS_NOT_SUPPORTED);
            if (span == ChronoUnit.MICROS)
                throw new NotSupportedException(LocalizedResources.Instance().MICRO_SECOND_BASE_IS_NOT_SUPPORTED);
            if (span == ChronoUnit.MILLIS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalMilliseconds;
            if (span == ChronoUnit.SECONDS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalSeconds;
            if (span == ChronoUnit.MINUTES)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalMinutes;
            if (span == ChronoUnit.HOURS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalHours;
            if (span == ChronoUnit.DAYS)
                return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalDays;
            if (span == ChronoUnit.MONTHS)
                return (int)(now.ToDateTimeUnspecified().Date.GetDifferenceInMonths(end.ToDateTimeUnspecified().Date));
            if (span == ChronoUnit.YEARS)
                return (int)(now.ToDateTimeUnspecified().Date.GetDifferenceInYears(end.ToDateTimeUnspecified().Date));

            return (int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalDays;
        }

        public static Period Until(this LocalDate now, LocalDate end)
        {
            //LocalDate end = LocalDate.From(target);
            //long totalMonths = end.GetProlepticMonth() - now.GetProlepticMonth();  // safe
            //int days = end.Day - now.Day;
            //if (totalMonths > 0 && days < 0)
            //{
            //    totalMonths--;
            //    LocalDate calcDate = now.PlusMonths((int)totalMonths);
            //    days = (int)(end.ToEpochDay() - calcDate.ToEpochDay());  // safe
            //}
            //else if (totalMonths < 0 && days > 0)
            //{
            //    totalMonths++;
            //    days -= end.LengthOfMonth();
            //}
            //long years = totalMonths / 12;  // safe
            //int months = (int)(totalMonths % 12);  // safe
            return Period.FromDays((int)(now.ToDateTimeUnspecified().Date - end.ToDateTimeUnspecified().Date).TotalDays);
        }

        public static long GetProlepticMonth(this LocalDate now)
        {
            return (now.Year * 12L + now.Month - 1);
        }

        public static long GetProlepticYear(this LocalDate now)
        {
            System.Globalization.Calendar cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            System.DateTime utcDateTime = now.ToDateTimeUnspecified();

            return cal.GetEra(utcDateTime) - 1;
        }

        public static ZonedDateTime ToZonedDateTimeUtc(this LocalDate now)
        {
            return now.ToDateTimeUnspecified().ToUniversalTime().ToZonedDateTime();
        }

        public static ZonedDateTime ToZonedDateTimeUnspecified(this LocalDate now)
        {
            return now.ToDateTimeUnspecified().ToZonedDateTime();
        }

        public static ZonedDateTime ToZonedDateTime(this LocalDate now, LocalTime time)
        {
            return (new DateTime(now.Year, now.Month, now.Day, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Local)).ToZonedDateTime();
        }

        public static ZonedDateTime ToZonedDateTimeUtc(this LocalDate now, LocalTime time)
        {
            return (new DateTime(now.Year, now.Month, now.Day, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Utc)).ToZonedDateTime();
        }

        public static ZonedDateTime ToZonedDateTimeUnspecified(this LocalDate now, LocalTime time)
        {
            return (new DateTime(now.Year, now.Month, now.Day, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Unspecified)).ToZonedDateTime();
        }

        public static long ToEpochDay(this LocalDate now)
        {
            long y = now.Year;
            long m = now.Month;
            long total = 0;
            total += 365 * y;
            if (y >= 0)
            {
                total += (y + 3) / 4 - (y + 99) / 100 + (y + 399) / 400;
            }
            else
            {
                total -= y / -4 - y / -100 + y / -400;
            }
            total += ((367 * m - 362) / 12);
            total += now.Day - 1;
            if (m > 2)
            {
                total--;
                if (now.IsLeapYear() == false)
                {
                    total--;
                }
            }
            return total - NodaTimeUtility.DAYS_0000_TO_1970;
        }

        public static long ToEpochMonths(this LocalDate now)
        {
            long y = now.Year;
            long m = now.Month;
            long ey = 1970;
            long em = 1;

            return (y - ey) * 12 + (m - em);
        }

        public static long ToEpochYears(this LocalDate now)
        {
            long y = now.Year;
            long ey = 1970;

            return (y - ey);
        }

        public static int LengthOfMonth(this LocalDate now)
        {
            switch (now.Month)
            {
                case 2:
                    return (now.IsLeapYear() ? 29 : 28);
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }

        public static int LengthOfYear(this LocalDate now)
        {
            return (now.IsLeapYear() ? 366 : 365);
        }

        public static int GetMinYear(this LocalDate now)
        {
            return NodaTimeUtility.MIN_VALUE;
        }

        public static int GetMaxYear(this LocalDate now)
        {
            return NodaTimeUtility.MAX_VALUE;
        }

        public static LocalDate With(this LocalDate now, ITemporalAdjuster tmp)
        {
            return tmp.AdjustInto(new Temporal(now)).GetOriginal();
        }

        public static LocalDate With(this LocalDate now, ITemporalField field, long newValue)
        {
            if (field is ChronoField)
            {
                ChronoField f = (ChronoField)field;
                f.CheckValidValue(newValue);
                if (f == ChronoField.DAY_OF_WEEK)
                {
                    return now.PlusDays((int)newValue - now.DayOfWeek.GetValue());
                }
                else if (f == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)
                {
                    return now.PlusDays((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH));
                }
                else if (f == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)
                {
                    return now.PlusDays((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR));
                }
                else if (f == ChronoField.DAY_OF_MONTH)
                {
                    return now.WithDayOfMonth((int)newValue);
                }
                else if (f == ChronoField.DAY_OF_YEAR)
                {
                    return now.WithDayOfYear((int)newValue);
                }
                else if (f == ChronoField.EPOCH_DAY)
                {
                    return now.OfEpochDay(newValue);
                }
                else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH)
                {
                    return now.PlusWeeks((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_WEEK_OF_MONTH));
                }
                else if (f == ChronoField.ALIGNED_WEEK_OF_YEAR)
                {
                    return now.PlusWeeks((int)newValue - (int)now.GetLong(ChronoField.ALIGNED_WEEK_OF_YEAR));
                }
                else if (f == ChronoField.MONTH_OF_YEAR)
                {
                    return now.WithMonth((int)newValue);
                }
                else if (f == ChronoField.PROLEPTIC_MONTH)
                {
                    return now.PlusMonths((int)newValue - (int)now.GetLong(ChronoField.PROLEPTIC_MONTH));
                }
                else if (f == ChronoField.YEAR_OF_ERA)
                {
                    return now.WithYear((int)(now.Year >= 1 ? newValue : 1 - newValue));
                }
                else if (f == ChronoField.YEAR)
                {
                    return now.WithYear((int)newValue);
                }
                else if (f == ChronoField.ERA)
                {
                    return (now.GetLong(ChronoField.ERA) == newValue ? now : now.WithYear(1 - now.Year));
                }


                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
            return field.AdjustInto(new Temporal(now), newValue).GetOriginal();
        }

        public static LocalDate With(this LocalDate now, MonthDay monthDay)
        {
            return LocalDate.FromDateTime(new DateTime(now.Year, monthDay.Month.Value, monthDay.Day));
        }

        public static LocalDate WithYear(this LocalDate now, int year)
        {
            if (now.Year == year)
            {
                return now;
            }
            return new LocalDate(year, now.Month, now.Day);
        }

        public static LocalDate WithMonth(this LocalDate now, int month)
        {
            if (now.Month == month)
            {
                return now;
            }

            if ((0 < month) && (month < 13))
            {
                throw new ArgumentOutOfRangeException();
            }

            return new LocalDate(now.Year, month, now.Day);
        }

        public static LocalDate WithDayOfMonth(this LocalDate now, int dayOfMonth)
        {
            if (now.Day == dayOfMonth)
            {
                return now;
            }

            return new LocalDate(now.Year, now.Month, dayOfMonth);
        }

        public static LocalDate WithDayOfYear(this LocalDate now, int dayOfYear)
        {
            if (now.DayOfYear == dayOfYear)
            {
                return now;
            }
            return OfYearDay(now, now.Year, dayOfYear);
        }

        public static LocalDate Plus(this LocalDate now, long amountToAdd, ITemporalUnit unit)
        {
            if (unit is ChronoUnit)
            {
                ChronoUnit f = (ChronoUnit)unit;

                if (f == ChronoUnit.DAYS)
                {
                    return now.PlusDays((int)amountToAdd);
                }
                else if (f == ChronoUnit.WEEKS)
                {
                    return now.PlusWeeks((int)amountToAdd);
                }
                else if (f == ChronoUnit.MONTHS)
                {
                    return now.PlusMonths((int)amountToAdd);
                }
                else if (f == ChronoUnit.YEARS)
                {
                    return now.PlusYears((int)amountToAdd);
                }
                else if (f == ChronoUnit.DECADES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 10));
                }
                else if (f == ChronoUnit.CENTURIES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 100));
                }
                else if (f == ChronoUnit.MILLENNIA)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 1000));
                }
                else if (f == ChronoUnit.ERAS)
                {
                    return now.With(ChronoField.ERA, Math2.SafeAdd(now.GetLong(ChronoField.ERA), amountToAdd));
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_UNIT, unit));
            }
            return unit.AddTo(new Temporal(now), amountToAdd);
        }

        public static LocalDate Minus(this LocalDate now, long amountToSubtract, ITemporalUnit unit)
        {
            return (amountToSubtract == long.MinValue ? now.Plus(long.MaxValue, unit).Plus(1, unit) : now.Plus(-amountToSubtract, unit));
        }

        public static LocalDate OfYearDay(this LocalDate now, int year, int dayOfYear)
        {
            ChronoField.YEAR.CheckValidValue(year);
            ChronoField.DAY_OF_YEAR.CheckValidValue(dayOfYear);
            Boolean leap = IsoChronology.GetInstance().IsLeapYear(year);
            if (dayOfYear == 366 && leap == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_DATE_DAYOFYEAR366_IS_NOT_A_LEAP_YEAR, year));
            }
            Month moy = Month.Of((dayOfYear - 1) / 31 + 1);
            int monthEnd = moy.firstDayOfYear(leap) + moy.Length(leap) - 1;
            if (dayOfYear > monthEnd)
            {
                moy = moy.Plus(1);
            }
            int dom = dayOfYear - moy.firstDayOfYear(leap) + 1;
            return LocalDate.FromDateTime(new DateTime(year, moy.Value, dom));
        }

        public static LocalDate OfEpochDay(this LocalDate now, long epochDay)
        {
            ChronoField.EPOCH_DAY.CheckValidValue(epochDay);
            long zeroDay = epochDay + NodaTimeUtility.DAYS_0000_TO_1970;
            // find the march-based year
            zeroDay -= 60;  // adjust to 0000-03-01 so leap day is at end of four year cycle
            long adjust = 0;
            if (zeroDay < 0)
            {
                // adjust negative years to positive for calculation
                long adjustCycles = (zeroDay + 1) / NodaTimeUtility.DAYS_PER_CYCLE - 1;
                adjust = adjustCycles * 400;
                zeroDay += -adjustCycles * NodaTimeUtility.DAYS_PER_CYCLE;
            }
            long yearEst = (400 * zeroDay + 591) / NodaTimeUtility.DAYS_PER_CYCLE;
            long doyEst = zeroDay - (365 * yearEst + yearEst / 4 - yearEst / 100 + yearEst / 400);
            if (doyEst < 0)
            {
                // fix estimate
                yearEst--;
                doyEst = zeroDay - (365 * yearEst + yearEst / 4 - yearEst / 100 + yearEst / 400);
            }
            yearEst += adjust;  // reset any negative year
            int marchDoy0 = (int)doyEst;

            // convert march-based values back to january-based
            int marchMonth0 = (marchDoy0 * 5 + 2) / 153;
            int month = (marchMonth0 + 2) % 12 + 1;
            int dom = marchDoy0 - (marchMonth0 * 306 + 5) / 10 + 1;
            yearEst += marchMonth0 / 10;

            // check year now we are certain it is correct
            int year = (int)ChronoField.YEAR.CheckValidIntValue(yearEst);
            return new LocalDate(year, month, dom);
        }

        public static long Get(this LocalDate now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                return Get0(now, field);
            }
            else
            {
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
        }

        public static long GetLong(this LocalDate now, ITemporalField field)
        {
            if (field == ChronoField.EPOCH_DAY)
            {
                return now.ToEpochDay();
            }
            if (field == ChronoField.PROLEPTIC_MONTH)
            {
                return now.GetProlepticMonth();
            }
            return Get0(now, field);
        }

        private static long Get0(LocalDate now, ITemporalField field)
        {
            if (field == ChronoField.DAY_OF_WEEK)
            {
                return now.DayOfWeek.GetValue();
            }
            else if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)
            {
                return ((now.Day - 1) % 7) + 1;
            }
            if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)
            {
                return ((now.DayOfYear - 1) % 7) + 1;
            }
            if (field == ChronoField.DAY_OF_MONTH)
            {
                return now.Day;
            }
            if (field == ChronoField.DAY_OF_YEAR)
            {
                return now.DayOfYear;
            }
            if (field == ChronoField.EPOCH_DAY)
            {
                return now.ToEpochDay();
            }
            if (field == ChronoField.ALIGNED_WEEK_OF_MONTH)
            {
                return ((now.Day - 1) / 7) + 1;
            }
            if (field == ChronoField.ALIGNED_WEEK_OF_YEAR)
            {
                return ((now.DayOfYear - 1) / 7) + 1;
            }
            else if (field == ChronoField.MONTH_OF_YEAR)
            {
                return now.Month;
            }
            else if (field == ChronoField.PROLEPTIC_MONTH)
            {
                return now.GetProlepticMonth();
            }
            else if (field == ChronoField.YEAR_OF_ERA)
            {
                return (now.Year >= 1 ? now.Year : 1 - now.Year);
            }
            else if (field == ChronoField.YEAR)
            {
                return now.Year;
            }
            else if (field == ChronoField.ERA)
            {
                return (now.Year >= 1 ? 1 : 0);
            }
            else
            {
                if (field.BaseUnit == ChronoUnit.DAYS)
                {
                    return now.ToEpochDay();
                }
                else if (field.BaseUnit == ChronoUnit.MONTHS)
                {
                    return now.ToEpochMonths();
                }
                else if (field.BaseUnit == ChronoUnit.YEARS)
                {
                    return now.ToEpochYears();
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
        }

        public static Instant ToInstant(this LocalDate source)
        {
            return Instant.FromDateTimeUtc(source.ToDateTimeUTC());
        }

        public static DateTime ToDateTimeUTC(this LocalDate source)
        {
            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime ToDateTimeLocal(this LocalDate source)
        {
            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0, DateTimeKind.Local);
        }

        public static ZonedDateTime ToZonedDateTime(this LocalDate source)
        {
            
            return new ZonedDateTime(source.ToInstant(), NodaTimeUtility.ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime ToZonedDateTime(this LocalDate source, DateTimeZone zone)
        {

            return new ZonedDateTime(source.ToInstant(), zone);
        }

        public static double GetDifferenceInDays(this LocalDate startDate, LocalDate endDate)
        {
            return GetDifferenceInDays(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInMonths(this LocalDate startDate, LocalDate endDate)
        {
            return GetDifferenceInMonths(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInYears(this LocalDate startDate, LocalDate endDate)
        {
            return GetDifferenceInYears(startDate.ToInstant(), endDate.ToInstant());
        }

        public static long GetEpochSecond(this LocalDate now)
        {
            TimeSpan t = now.ToDateTimeUnspecified() - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }

        public static double GetExactDaysBetween(this LocalDate startDate, LocalDate endDate)
        {
            return GetExactDaysBetween(startDate.ToInstant(), endDate.ToInstant());
        }

        public static int GetDaysBetween(this LocalDate startDate, LocalDate endDate)
        {
            return GetDaysBetween(startDate.ToInstant(), true, endDate.ToInstant(), false);
        }

        public static int GetDaysBetween(this LocalDate startDate, Boolean includeStart, LocalDate endDate, Boolean includeEnd)
        {
            return GetDaysBetween(startDate.ToInstant(), includeStart, endDate.ToInstant(), includeEnd);
        }

        public static Range GetRange(this LocalDate now, ITemporalField field)
        {
            if (field is ChronoField)
            {
                ChronoField f = (ChronoField)field;
                if (f.IsDateBased)
                {
                    if (f == ChronoField.DAY_OF_MONTH)
                    {
                        return new Range(1, now.LengthOfMonth());
                    }
                    else if (f == ChronoField.DAY_OF_YEAR)
                    {
                        return new Range(1, now.LengthOfYear());
                    }
                    else if (f == ChronoField.ALIGNED_WEEK_OF_MONTH)
                    {
                        return new Range(1, now.Month == Month.FEBRUARY.Value && now.IsLeapYear() == false ? 4 : 5);
                    }
                    else if (f == ChronoField.YEAR_OF_ERA)
                    {
                        return (now.Year <= 0 ? new Range(1, now.GetMaxYear() + 1) : new Range(1, now.GetMaxYear()));
                    }
                    return field.Range;
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_FIELD, field));
            }
            return field.RangeRefinedBy(new Temporal(now));
        }

        public static LocalDate Plus(this LocalDate now, long amountToAdd, ChronoUnit unit)
        {
            if (unit is ChronoUnit)
            {
                ChronoUnit f = (ChronoUnit)unit;

                if (f == ChronoUnit.DAYS)
                {
                    return now.PlusDays((int)amountToAdd);
                }
                else if (f == ChronoUnit.WEEKS)
                {
                    return now.PlusWeeks((int)amountToAdd);
                }
                else if (f == ChronoUnit.MONTHS)
                {
                    return now.PlusMonths((int)amountToAdd);
                }
                else if (f == ChronoUnit.YEARS)
                {
                    return now.PlusYears((int)amountToAdd);
                }
                else if (f == ChronoUnit.DECADES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 10));
                }
                else if (f == ChronoUnit.CENTURIES)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 100));
                }
                else if (f == ChronoUnit.MILLENNIA)
                {
                    return now.PlusYears((int)Math2.SafeMultiply(amountToAdd, 1000));

                }
                else if (f == ChronoUnit.ERAS)
                {
                    return now.With(ChronoField.ERA, Math2.SafeAdd((int)now.GetLong(ChronoField.ERA), amountToAdd));
                }
                throw new NotSupportedException(String.Format(LocalizedResources.Instance().UNSUPPORTED_UNIT, unit));

            }
            return unit.AddTo(new Temporal(now), amountToAdd);
        }

        public static LocalDate ConvertToLocalDate(this int fromValue)
        {
            return ((long)fromValue).ConvertToLocalDate();
        }

        public static LocalDate ConvertToLocalDate(this long fromValue)
        {
            DateTime dt;
            DateTime.TryParseExact(fromValue.ToString(), "yyyyMMdd",
                          System.Globalization.CultureInfo.InvariantCulture,
                          System.Globalization.DateTimeStyles.None, out dt);
            return dt.ToLocalDate();
        }
        #endregion

        #region Nullable
        public static Nullable<ZonedDateTime> AtZone(this Nullable<LocalDate> now, DateTimeZone zone)
        {
            if (now is null)
                return null;

            return now.Value.AtZone(zone);
        }

        public static int CompareTo(this Nullable<LocalDate> now, LocalDate target)
        {
            if (now is null)
                return -1;

            return now.Value.CompareTo(target);
        }

        public static int CompareTo(this Nullable<LocalDate> now, Nullable<LocalDate> target)
        {
            if (now is null)
                return -1;

            if (target is null)
                return -1;

            return now.Value.CompareTo(target.Value);
        }

        public static Boolean IsAfter(this Nullable<LocalDate> now,  Nullable<LocalDate> target)
        {
            if (now is null)
                return false;

            if (target is null)
                return false;

            return now.Value.IsAfter(target.Value);
        }

        public static Boolean IsBefore(this Nullable<LocalDate> now,  Nullable<LocalDate> target)
        {
            if (now is null)
                return false;

            if (target is null)
                return false;

            return now.Value.IsBefore(target.Value);
        }

        public static Boolean IsAfter(this Nullable<LocalDate> now,  LocalDate target)
        {
            if (now is null)
                return false;

            return now.Value.IsAfter(target);
        }

        public static Boolean IsBefore(this Nullable<LocalDate> now,  LocalDate target)
        {
            if (now is null)
                return false;

            return now.Value.IsBefore(target);
        }

        public static Nullable<LocalDate> AdjustDayOfWeek(this Nullable<LocalDate> now, IsoDayOfWeek dayOfWeek)
        {
            if (now is null)
                return null;

            return now.Value.AdjustDayOfWeek(dayOfWeek);
        }

        public static Boolean IsLeapYear(this Nullable<LocalDate> now)
        {
            if (now is null)
                return false;

            return now.Value.IsLeapYear();
        }

        public static Boolean IsSupported(this Nullable<LocalDate> now, ITemporalField field)
        {
            if (now is null)
                return false;

            return now.Value.IsSupported(field);
        }

        public static Boolean IsSupported(this Nullable<LocalDate> now, ChronoUnit field)
        {
            if (now is null)
                return false;

            return now.Value.IsSupported(field);
        }

        public static int GetDaysOfMonth(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetDaysOfMonth();
        }

        public static int PeriodUntil(this Nullable<LocalDate> now,  Nullable<LocalDate> end, ChronoUnit span)
        {
            if (now is null)
                return -1;

            if (end is null)
                return -1;

            return now.Value.PeriodUntil(end.Value, span);
        }

        public static Period Until(this Nullable<LocalDate> now,  Nullable<LocalDate> end)
        {
            if (now is null)
                return null;

            if (end is null)
                return null;

            return now.Value.Until(end.Value);
        }

        public static long GetProlepticMonth(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetProlepticMonth();
        }

        public static long GetProlepticYear(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetProlepticYear();
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUtc(this Nullable<LocalDate> now)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedDateTimeUtc();
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUnspecified(this Nullable<LocalDate> now)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedDateTimeUnspecified();
        }

        public static Nullable<ZonedDateTime> ToZonedDateTime(this Nullable<LocalDate> now, LocalTime time)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedDateTime(time);
        }

        public static Nullable<ZonedDateTime> ToZonedDateTime(this Nullable<LocalDate> now, Nullable<LocalTime> time)
        {
            if (now is null)
                return null;

            if (time is null)
                return null;

            return now.Value.ToZonedDateTime(time.Value);
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUtc(this Nullable<LocalDate> now, LocalTime time)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedDateTimeUtc(time);
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUtc(this Nullable<LocalDate> now, Nullable<LocalTime> time)
        {
            if (now is null)
                return null;

            if (time is null)
                return null;

            return now.Value.ToZonedDateTimeUtc(time.Value);
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUnspecified(this Nullable<LocalDate> now, LocalTime time)
        {
            if (now is null)
                return null;

            return now.Value.ToZonedDateTimeUnspecified(time);
        }

        public static Nullable<ZonedDateTime> ToZonedDateTimeUnspecified(this Nullable<LocalDate> now, Nullable<LocalTime> time)
        {
            if (now is null)
                return null;

            if (time is null)
                return null;

            return now.Value.ToZonedDateTimeUnspecified(time.Value);
        }

        public static long ToEpochDay(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochDay();
        }

        public static long ToEpochMonths(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochMonths();
        }

        public static long ToEpochYears(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.ToEpochYears();
        }

        public static int LengthOfMonth(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.LengthOfMonth();
        }

        public static int LengthOfYear(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.LengthOfYear();
        }

        public static int GetMinYear(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetMinYear();
        }

        public static int GetMaxYear(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetMaxYear();
        }

        public static Nullable<LocalDate> PlusDays(this Nullable<LocalDate> nodaTime, int days)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusDays(days);
        }

        public static Nullable<LocalDate> PlusWeeks(this Nullable<LocalDate> nodaTime, int weeks)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusWeeks(weeks);
        }

        public static Nullable<LocalDate> PlusMonths(this Nullable<LocalDate> nodaTime, int months)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusMonths(months);
        }

        public static Nullable<LocalDate> PlusYears(this Nullable<LocalDate> nodaTime, int years)
        {
            if (nodaTime is null)
                return null;

            return nodaTime.Value.PlusYears(years);
        }

        public static Nullable<LocalDate> With(this Nullable<LocalDate> now, ITemporalAdjuster tmp)
        {
            if (now is null)
                return null;

            return now.Value.With(tmp);
        }

        public static Nullable<LocalDate> With(this Nullable<LocalDate> now, ITemporalField field, long newValue)
        {
            if (now is null)
                return null;

            return now.Value.With(field, newValue);
        }

        public static Nullable<LocalDate> With(this Nullable<LocalDate> now, MonthDay monthDay)
        {
            if (now is null)
                return null;

            return now.Value.With(monthDay);
        }

        public static Nullable<LocalDate> WithYear(this Nullable<LocalDate> now, int year)
        {
            if (now is null)
                return null;

            return now.Value.WithYear(year);
        }

        public static Nullable<LocalDate> WithMonth(this Nullable<LocalDate> now, int month)
        {
            if (now is null)
                return null;

            return now.Value.WithMonth(month);
        }

        public static Nullable<LocalDate> WithDayOfMonth(this Nullable<LocalDate> now, int dayOfMonth)
        {
            if (now is null)
                return null;

            return now.Value.WithDayOfMonth(dayOfMonth);
        }

        public static Nullable<LocalDate> WithDayOfYear(this Nullable<LocalDate> now, int dayOfYear)
        {
            if (now is null)
                return null;

            return now.Value.WithDayOfYear(dayOfYear);
        }

        public static Nullable<LocalDate> Plus(this Nullable<LocalDate> now, long amountToAdd, ITemporalUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Plus(amountToAdd, unit);
        }

        public static Nullable<LocalDate> Minus(this Nullable<LocalDate> now, long amountToSubtract, ITemporalUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Minus(amountToSubtract, unit);
        }

        public static Nullable<LocalDate> OfYearDay(this Nullable<LocalDate> now, int year, int dayOfYear)
        {
            if (now is null)
                return null;

            return now.Value.OfYearDay(year, dayOfYear);
        }

        public static Nullable<LocalDate> OfEpochDay(this Nullable<LocalDate> now, long epochDay)
        {
            if (now is null)
                return null;

            return now.Value.OfEpochDay(epochDay);
        }

        public static long Get(this Nullable<LocalDate> now, ITemporalField field)
        {
            if (now is null)
                return -1;

            return now.Value.Get(field);
        }

        public static long GetLong(this Nullable<LocalDate> now, ITemporalField field)
        {
            if (now is null)
                return -1;

            return now.Value.GetLong(field);
        }

        public static Nullable<Instant> ToInstant(this Nullable<LocalDate> source)
        {
            if (source is null)
                return null;

            return source.Value.ToInstant();
        }

        public static Nullable<DateTime> ToDateTimeUTC(this Nullable<LocalDate> source)
        {
            if (source is null)
                return null;

            return source.Value.ToDateTimeUTC();
        }

        public static Nullable<DateTime> ToDateTimeLocal(this Nullable<LocalDate> source)
        {
            if (source is null)
                return null;

            return source.Value.ToDateTimeLocal();
        }

        public static Nullable<ZonedDateTime> ToZonedDateTime(this Nullable<LocalDate> source)
        {
            if (source is null)
                return null;

            return source.Value.ToZonedDateTime();
        }

        public static double GetDifferenceInDays(this Nullable<LocalDate> startDate,  Nullable<LocalDate> endDate)
        {
            if (startDate is null)
                return -1;

            if (endDate is null)
                return -1;

            return startDate.Value.GetDifferenceInDays(endDate.Value);
        }

        public static double GetDifferenceInMonths(this Nullable<LocalDate> startDate,  Nullable<LocalDate> endDate)
        {
            if (startDate is null)
                return -1;

            if (endDate is null)
                return -1;

            return startDate.Value.GetDifferenceInMonths(endDate.Value);
        }

        public static double GetDifferenceInYears(this Nullable<LocalDate> startDate,  Nullable<LocalDate> endDate)
        {
            if (startDate is null)
                return -1;

            if (endDate is null)
                return -1;

            return startDate.Value.GetDifferenceInYears(endDate.Value);
        }

        public static long GetEpochSecond(this Nullable<LocalDate> now)
        {
            if (now is null)
                return -1;

            return now.Value.GetEpochSecond();
        }

        public static double GetExactDaysBetween(this Nullable<LocalDate> startDate,  Nullable<LocalDate> endDate)
        {
            if (startDate is null)
                return -1;

            if (endDate is null)
                return -1;

            return startDate.Value.GetExactDaysBetween(endDate.Value);
        }

        public static int GetDaysBetween(this Nullable<LocalDate> startDate,  Nullable<LocalDate> endDate)
        {
            if (startDate is null)
                return -1;

            if (endDate is null)
                return -1;

            return startDate.Value.GetDaysBetween(endDate.Value);
        }

        public static int GetDaysBetween(this Nullable<LocalDate> startDate, Boolean includeStart,  Nullable<LocalDate> endDate, Boolean includeEnd)
        {
            if (startDate is null)
                return -1;

            if (endDate is null)
                return -1;

            return startDate.Value.GetDaysBetween(includeStart, endDate.Value, includeEnd);
        }

        public static Nullable<LocalDate> Plus(this Nullable<LocalDate> now, long amountToAdd, ChronoUnit unit)
        {
            if (now is null)
                return null;

            return now.Value.Plus(amountToAdd, unit);
        }
        #endregion
        #endregion

        #region Duration methods
        public static Duration EstimatedDuration(this Period period)
        {
            Duration monthsDuration = NodaTimeUtility._months.MultipliedBy(period.Months);
            Duration daysDuration = NodaTimeUtility._days.MultipliedBy(period.Days);
            return monthsDuration.Plus(daysDuration);
        }

        public static Duration MultipliedBy(this Duration duration, long multiplicand)
        {
            if (multiplicand == 0)
            {
                return Duration.FromTimeSpan(new TimeSpan(0));
            }

            if (multiplicand == 1)
            {
                return duration;
            }
            return Duration.FromTimeSpan(new TimeSpan((long)duration.TotalTicks * multiplicand));
        }

        public static Duration MultipliedBy(this Duration duration, double multiplicand)
        {

            return MultipliedBy(duration, (long)multiplicand);
        }
        #endregion

        #region Instant methods

        public static int GetNano(this Instant now)
        {
            return now.AsZonedDateTime().NanosecondOfSecond;
        }

        public static long GetEpochSecond(this Instant now)
        {
            TimeSpan t = now.ToDateTimeUtc() - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }

        public static double GetDifferenceInDays(this Instant startDate, Instant endDate)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().START_DATE_WAS_NULL);
            }
            if (endDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().END_DATE_WAS_NULL);
            }

            return (double)(Instant.Subtract(endDate, startDate).TotalMilliseconds) / NodaTimeUtility.MILLISECONDS_PER_DAY;
        }

        public static double GetDifferenceInMonths(this Instant startDate, Instant endDate)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().START_DATE_WAS_NULL);
            }
            if (endDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().END_DATE_WAS_NULL);
            }

            return (double)(Instant.Subtract(endDate, startDate).TotalMilliseconds) / NodaTimeUtility.MILLISECONDS_PER_MONTH;
        }

        public static double GetDifferenceInYears(this Instant startDate, Instant endDate)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().START_DATE_WAS_NULL);
            }
            if (endDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().END_DATE_WAS_NULL);
            }

            return (double)(Instant.Subtract(endDate, startDate).TotalMilliseconds) / NodaTimeUtility.MILLISECONDS_PER_YEAR;
        }


        public static Instant GetDateOffsetWithYearFraction(this Instant startDate, double yearFraction)
        {
            return GetDateOffsetWithYearFraction(startDate.ToDateTimeUtc().ToZonedDateTime(), yearFraction).ToInstant();
        }

        public static double GetExactDaysBetween(this Instant startDate, Instant endDate)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().START_DATE_WAS_NULL);
            }
            if (endDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().END_DATE_WAS_NULL);
            }
            return (endDate.GetEpochSecond() - startDate.GetEpochSecond()) / (double)NodaTimeUtility.SECONDS_PER_DAY;
        }

        public static int GetDaysBetween(this Instant startDate, Boolean includeStart, Instant endDate, Boolean includeEnd)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().START_DATE_WAS_NULL);
            }
            if (endDate == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().END_DATE_WAS_NULL);
            }
            int daysBetween = (int)Math.Abs(startDate.GetDifferenceInDays(endDate));
            if (includeStart && includeEnd)
            {
                daysBetween++;
            }
            else if (!includeStart && !includeEnd)
            {
                daysBetween--;
            }
            return daysBetween;
        }

        public static Instant PlusDays(this Instant now, int days)
        {
            return now.Plus(Duration.FromDays(days));
        }

        public static Instant PlusHours(this Instant now, int hours)
        {
            return now.Plus(Duration.FromHours(hours));
        }

        public static Instant PlusMinutes(this Instant now, int minutes)
        {
            return now.Plus(Duration.FromMinutes(minutes));
        }

        public static Instant PlusSeconds(this Instant now, int seconds)
        {
            return now.Plus(Duration.FromSeconds(seconds));
        }

        public static Instant PlusMillis(this Instant now, long millis)
        {
            return now.Plus(Duration.FromMilliseconds(millis));
        }

        //public static Instant PlusNanos(this Instant now, long nanos)
        //{
        //    return now.Plus(Duration.FromNanoseconds(nanos));
        //}

        public static Instant PlusTicks(this Instant now, long ticks)
        {
            return now.Plus(Duration.FromTicks(ticks));
        }

        #endregion

        #region Period Methods

        public static Period Plus(this Period period, Period add)
        {

            period = period.Normalize();
            add = add.Normalize();

            return period + add;
        }

        public static Period Minus(this Period now, Period period)
        {
            var totalTicks = now.Ticks - period.Ticks;
            return Period.FromTicks(totalTicks);
        }

        /// <summary>
        /// Checks if all three units of this period are zero.
        /// <p>
        /// A zero period has the value zero for the years, months and days units.
        /// 
        /// </summary>
        /// <returns>true if this period is zero-length</returns>
        public static Boolean IsZero(this Period period)
        {
            return (period == Period.Zero);
        }

        /// <summary>
        /// Checks if any of the three units of this period are negative.
        /// <p>
        /// This checks whether the years, months or days units are less than zero.
        /// 
        /// </summary>
        /// <returns>true if any unit of this period is negative</returns>
        public static Boolean IsNegative(this Period period)
        {
            return period.Years < 0 || period.Months < 0 || period.Days < 0;
        }

        /// <summary>
        /// Returns a new instance with each element in this period multiplied
        /// by the specified scalar.
        /// <p>
        /// This simply multiplies each field, years, months, days and normalized time,
        /// by the scalar. No normalization is performed.
        /// 
        /// </summary>
        /// <param name="scalar"> the scalar to multiply by, not null</param>
        /// <returns>a {@code Period} based on this period with the amounts multiplied by the scalar, not null</returns>
        /// <exception cref="ArithmeticException">if numeric overflow occurs </exception>
        public static Period MultipliedBy(this Period period, int scalar)
        {
            if (period == Period.Zero || scalar == 1)
            {
                return period;
            }

            if (period.Years != 0)
                return Period.FromYears(Math2.SafeMultiply(period.Years, scalar));
            else if (period.Months != 0)
                return Period.FromMonths(Math2.SafeMultiply(period.Months, scalar));
            else if (period.Weeks != 0)
                return Period.FromWeeks(Math2.SafeMultiply(period.Weeks, scalar));
            else if (period.Days != 0)
                return Period.FromDays(Math2.SafeMultiply(period.Days, scalar));
            else if (period.Hours != 0)
                return Period.FromHours(Math2.SafeMultiply(period.Hours, scalar));
            else if (period.Minutes != 0)
                return Period.FromMinutes(Math2.SafeMultiply(period.Minutes, scalar));
            else if (period.Seconds != 0)
                return Period.FromSeconds(Math2.SafeMultiply(period.Seconds, scalar));
            else if (period.Milliseconds != 0)
                return Period.FromMilliseconds(Math2.SafeMultiply(period.Milliseconds, scalar));
            else if (period.Nanoseconds != 0)
                return Period.FromNanoseconds(Math2.SafeMultiply(period.Nanoseconds, scalar));
            else
                return Period.FromTicks(Math2.SafeMultiply(period.Ticks, scalar));
        }

        public static Duration ToDurationEX(this Period period)
        {
            try
            {
                return period.ToDuration();
            }
            catch
            {
                ZonedDateTime start = new ZonedDateTime();
                ZonedDateTime end = new ZonedDateTime();

                end = end.PlusYears(period.Years);
                end = end.PlusMonths(period.Months);
                end = end.PlusDays(period.Days);
                end = end.PlusHours((int)period.Hours);
                end = end.PlusMinutes((int)period.Minutes);
                end = end.PlusSeconds(period.Seconds);
                end = end.PlusMilliseconds(period.Milliseconds);
                end = end.PlusNanoseconds(period.Nanoseconds);

                return end - start;
            }
        }

        public static int GetTotalYears(this Period period)
        {
            var years = period.Years;
            years += (period.Months/12);
            years += (period.Days / (12 * 30));
            years += (int)(period.Hours / (12 * 30 * 24));
            years += (int)(period.Minutes / (12 * 30 * 24 * 60));
            years += (int)(period.Seconds / (12 * 30 * 24 * 60 * 60));
            years += (int)(period.Milliseconds / (12L * 30L * 24L * 60L * 60L * 100L));

            return years;
        }

        public static int GetTotalMonths(this Period period)
        {
            var months = 0;
            months += period.Years * 12;
            months += period.Months;
            months += (period.Days / 30);
            months += (int)(period.Hours / (30 * 24));
            months += (int)(period.Minutes / (30 * 24 * 60));
            months += (int)(period.Seconds / (30 * 24 * 60 * 60));
            months += (int)(period.Milliseconds / (30 * 24 * 60 * 60 * 100));

            return months;
        }

        public static int GetTotalDays(this Period period)
        {
            var days = 0;
            days += period.Years * 365;
            days += period.Months * 30;
            days += period.Days;
            days += (int)(period.Hours / 24);
            days += (int)(period.Minutes / (24 * 60));
            days += (int)(period.Seconds / (24 * 60 * 60));
            days += (int)(period.Milliseconds / (24 * 60 * 60 * 100));

            return days;
        }

        public static long GetTotalHours(this Period period)
        {
            var hours = 0L;
            hours += period.Years * 365 * 24;
            hours += period.Months * 30 * 24;
            hours += period.Days * 24;
            hours += period.Hours;
            hours += period.Minutes / 60;
            hours += period.Seconds / (60 * 60);
            hours += period.Milliseconds / (60 * 60 * 100);

            return hours;
        }

        public static long GetTotalMinutes(this Period period)
        {
            var minutes = 0L;
            minutes += period.Years * 365 * 24 * 60;
            minutes += period.Months * 30 * 24 * 60;
            minutes += period.Days * 24 * 60;
            minutes += period.Hours * 60;
            minutes += period.Minutes;
            minutes += period.Seconds / 60;
            minutes += period.Milliseconds / (60 * 100);

            return minutes;
        }

        public static long GetTotalSeconds(this Period period)
        {
            var seconds = 0L;
            seconds += period.Years * 365 * 24 * 60 * 60;
            seconds += period.Months * 30 * 24 * 60 * 60;
            seconds += period.Days * 24 * 60 * 60;
            seconds += period.Hours * 60 * 60;
            seconds += period.Minutes * 60;
            seconds += period.Seconds;
            seconds += period.Milliseconds / 100;

            return seconds;
        }

        public static long GetTotalMilliseconds(this Period period)
        {
            var milliseconds = 0L;
            milliseconds += period.Years * 365 * 24 * 60 * 60 * 100;
            milliseconds += period.Months * 30 * 24 * 60 * 60 * 100;
            milliseconds += period.Days * 24 * 60 * 60 * 100;
            milliseconds += period.Hours * 60 * 60 * 100;
            milliseconds += period.Minutes * 60 * 100;
            milliseconds += period.Seconds * 100;
            milliseconds += period.Milliseconds;

            return milliseconds;
        }

        #endregion

        #region Range methods

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
        public static long CheckValidValue(this Range range, long value, ITemporalField field)
        {
            if (range.IsValidValue(value) == false)
            {
                if (field != null)
                {
                    throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_VALUE_FOR_FIELD_VALID_VALUE_RANGE, field , range , value));
                }
                else
                {
                    throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_VALUE_VALID_RANGE, range , value));
                }
            }
            return value;
        }

        /// <summary>
        /// Checks that the specified value is valid and fits in an <see cref="int"/>.
        /// <p>
        /// This ArgumentCheckers that the value is within the valid range of values and that
        /// all valid values are within the bounds of an <see cref="int"/>.
        /// The field is only used to improve the error message.
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <param name="field">the field being checked, may be null</param>
        /// <returns>the value that was passed in</returns>
        /// <see cref="Range.IsValidIntValue(long)"/>
        public static int CheckValidIntValue(this Range range, long value, ITemporalField field)
        {
            if (range.IsValidIntValue(value) == false)
            {
                throw new DateTimeException(String.Format(LocalizedResources.Instance().INVALID_INT_VALUE_FOR_FIELD, field, value));
            }
            return (int)value;
        }
        #endregion

        #region DayOfWeek Methods
        public static DayOfWeek ToDayOfWeek(this IsoDayOfWeek isoDayOfWeek)
        {
            if (isoDayOfWeek == IsoDayOfWeek.Monday)
                return DayOfWeek.Monday;
            else if (isoDayOfWeek == IsoDayOfWeek.Tuesday)
                return DayOfWeek.Tuesday;
            else if (isoDayOfWeek == IsoDayOfWeek.Wednesday)
                return DayOfWeek.Wednesday;
            else if (isoDayOfWeek == IsoDayOfWeek.Thursday)
                return DayOfWeek.Thursday;
            else if (isoDayOfWeek == IsoDayOfWeek.Friday)
                return DayOfWeek.Friday;
            else if (isoDayOfWeek == IsoDayOfWeek.Saturday)
                return DayOfWeek.Saturday;
            else if (isoDayOfWeek == IsoDayOfWeek.Sunday)
                return DayOfWeek.Sunday;
            else
                throw new ArgumentOutOfRangeException();
        }

        public static int GetValue(this IsoDayOfWeek isoDayOfWeek)
        {
            if (isoDayOfWeek == IsoDayOfWeek.Monday)
                return 1;
            else if (isoDayOfWeek == IsoDayOfWeek.Tuesday)
                return 2;
            else if (isoDayOfWeek == IsoDayOfWeek.Wednesday)
                return 3;
            else if (isoDayOfWeek == IsoDayOfWeek.Thursday)
                return 4;
            else if (isoDayOfWeek == IsoDayOfWeek.Friday)
                return 5;
            else if (isoDayOfWeek == IsoDayOfWeek.Saturday)
                return 6;
            else if (isoDayOfWeek == IsoDayOfWeek.Sunday)
                return 7;
            else
                throw new ArgumentOutOfRangeException();
        }

        public static int Ordinal(this IsoDayOfWeek isoDayOfWeek)
        {
            return isoDayOfWeek.GetValue() - 1;
        }

        public static IsoDayOfWeek ToIsoDayOfWeek(this DayOfWeek DayOfWeek)
        {
            if (DayOfWeek == DayOfWeek.Monday)
                return IsoDayOfWeek.Monday;
            else if (DayOfWeek == DayOfWeek.Tuesday)
                return IsoDayOfWeek.Tuesday;
            else if (DayOfWeek == DayOfWeek.Wednesday)
                return IsoDayOfWeek.Wednesday;
            else if (DayOfWeek == DayOfWeek.Thursday)
                return IsoDayOfWeek.Thursday;
            else if (DayOfWeek == DayOfWeek.Friday)
                return IsoDayOfWeek.Friday;
            else if (DayOfWeek == DayOfWeek.Saturday)
                return IsoDayOfWeek.Saturday;
            else if (DayOfWeek == DayOfWeek.Sunday)
                return IsoDayOfWeek.Sunday;
            else
                throw new ArgumentOutOfRangeException();
        }

        public static int GetValue(this DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Monday)
                return 1;
            else if (dayOfWeek == DayOfWeek.Tuesday)
                return 2;
            else if (dayOfWeek == DayOfWeek.Wednesday)
                return 3;
            else if (dayOfWeek == DayOfWeek.Thursday)
                return 4;
            else if (dayOfWeek == DayOfWeek.Friday)
                return 5;
            else if (dayOfWeek == DayOfWeek.Saturday)
                return 6;
            else if (dayOfWeek == DayOfWeek.Sunday)
                return 7;
            else
                throw new ArgumentOutOfRangeException();
        }

        public static int Ordinal(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek.GetValue() - 1;
        }
        #endregion
    }
}
