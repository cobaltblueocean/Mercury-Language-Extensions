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
using NodaTime;
using Mercury.Language.Time;
using System.Security.Policy;

namespace System
{
    /// <summary>
    /// NodaTimeUtility Description
    /// </summary>
    public static class NodaTimeUtility
    {
        public static DateTimeZone ORIGINAL_TIME_ZONE = DateTimeZoneProviders.Bcl.GetSystemDefault(); //(new LocalDateTime()).ToZonedDateTime().Zone; //DateTimeZoneProviders.Bcl.GetSystemDefault();
        /// <summary>
        /// The number of days in a 400 year cycle.
        /// </summary>
        public const int DAYS_PER_CYCLE = 146097;
        /// <summary>
        /// The number of days from year zero to year 1970.
        /// There are five 400 year cycles from year zero to 2000.
        /// There are 7 leap years from 1970 to 2000.
        /// </summary>
        public const long DAYS_0000_TO_1970 = (DAYS_PER_CYCLE * 5) - (30 * 365 + 7);

        /// <summary>
        /// The number of seconds in one day.
        /// </summary>
        public const long SECONDS_PER_DAY = 86400L;
        /// <summary>
        /// The number of days in one year (estimated as 365.25).
        /// </summary>
        //TODO change this to 365.2425 to be consistent with JSR-310
        public const double DAYS_PER_YEAR = 365.25;
        /// <summary>
        /// The number of milliseconds in one day.
        /// </summary>
        public const long MILLISECONDS_PER_DAY = SECONDS_PER_DAY * 1000;
        /// <summary>
        /// The number of seconds in one year.
        /// </summary>
        public const long SECONDS_PER_YEAR = (long)(SECONDS_PER_DAY * DAYS_PER_YEAR);
        /// <summary>
        /// The number of milliseconds in one year.
        /// </summary>
        public const long MILLISECONDS_PER_YEAR = SECONDS_PER_YEAR * 1000;
        /// <summary>
        /// The number of milliseconds in one month.
        /// </summary>
        public const long MILLISECONDS_PER_MONTH = MILLISECONDS_PER_YEAR / 12L;
        /// <summary>
        /// The number of milliseconds in one week.
        /// </summary>
        public const long MILLISECONDS_PER_WEEK = MILLISECONDS_PER_DAY * 7;

        public static Duration _days = Duration.FromTimeSpan(new TimeSpan(24, 0, 0));
        public static Duration _months = Duration.FromTimeSpan(TimeSpan.FromSeconds(31556952L / 12));
        public static Duration _years = Duration.FromTimeSpan(TimeSpan.FromSeconds(31556952L));

        /// <summary>
        /// The minimum supported year, '-999,999,999'.
        /// </summary>
        public static int MIN_VALUE = -999999999;

        /// <summary>
        /// The maximum supported year, '+999,999,999'.
        /// </summary>
        public static int MAX_VALUE = 999999999;

        /// <summary>
        /// Represents the number of ticks in 1 day. This field is constant.
        /// </summary>
        public static long TicksPerDay = 864000000000;

        /// <summary>
        /// Represents the number of ticks in 1 Hour. This field is constant.
        /// </summary>
        public static long TicksPerHour = 36000000000;

        /// <summary>
        /// Represents the number of ticks in 1 minute. This field is constant.
        /// </summary>
        public static long TicksPerMinute = 600000000;

        /// <summary>
        /// Represents the number of ticks in 1 Second. This field is constant.
        /// </summary>
        public static long TicksPerSecond = 10000000;

        /// <summary>
        /// Represents the number of ticks in 1 Millisecond. This field is constant.
        /// </summary>
        public static long TicksPerMillisecond = 10000;

        public static DateTimeZone Unspecified = new UnspecifiedDateTimeZone();

        public static LocalDateTime GetLocalDateTime(LocalDate localDate)
        {
            return GetLocalDateTime(localDate.Year, localDate.Month, localDate.Day);
        }

        public static LocalDateTime GetLocalDateTime(LocalDate localDate, LocalTime localTime)
        {
            return GetLocalDateTime(localDate.Year, localDate.Month, localDate.Day, localTime.Hour, localTime.Minute, localTime.Second, localTime.Millisecond);
        }

        public static LocalDateTime GetLocalDateTime(LocalDateTime localDateTime)
        {
            return GetLocalDateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day, localDateTime.Hour, localDateTime.Minute, localDateTime.Second, localDateTime.Millisecond);
        }

        public static LocalDateTime GetLocalDateTime(LocalDateTime localDateTime, LocalTime localTime)
        {
            return GetLocalDateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day, localTime.Hour, localTime.Minute, localTime.Second, localTime.Millisecond);
        }

        public static LocalDateTime GetLocalDateTime(int year, int month, int day)
        {
            return GetLocalDateTime(year, month, day, 0, 0, 0);
        }

        public static LocalDateTime GetLocalDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return GetLocalDateTime(year, month, day, hour, minute, second, 0);
        }

        public static LocalDateTime GetLocalDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return new LocalDateTime(year, month, day, hour, minute, second, millisecond);
        }

        public static ZonedDateTime GetUTCDate(int year, int month, int day)
        {
            LocalDate localDate = new LocalDate(year, month, day);
            return new ZonedDateTime(localDate.ToInstant(), DateTimeZone.Utc);
        }

        public static ZonedDateTime GetUTCZonedDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return GetUTCZonedDateTime(year, month, day, hour, minute, second, 0, 0);
        }

        public static ZonedDateTime GetUTCZonedDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            var localDate = new LocalDateTime(year, month, day, hour, minute, second, millisecond);
            ZonedDateTime zdt = localDate.ChangeToDifferentTimeZoneWithSameDateTime(DateTimeZone.Utc);
            zdt = zdt.PlusNanoseconds(nanosecond);
            return zdt;
        }

        public static ZonedDateTime GetZonedDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return GetZonedDateTime(year, month, day, hour, minute, second, 0, 0, NodaTimeUtility.ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime GetZonedDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return GetZonedDateTime(year, month, day, hour, minute, second, millisecond, 0, NodaTimeUtility.ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime GetZonedDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            return GetZonedDateTime(year, month, day, hour, minute, second, millisecond, nanosecond, NodaTimeUtility.ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime GetZonedDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeZone zone)
        {
            return GetZonedDateTime(year, month, day, hour, minute, second, 0, 0, zone);
        }

        public static ZonedDateTime GetZonedDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond, DateTimeZone zone)
        {
            var _tmp = new LocalDateTime(year, month, day, hour, minute, second, millisecond);
            var result = _tmp.ChangeToDifferentTimeZoneWithSameDateTime(zone);

            return result.PlusNanoseconds(nanosecond);
        }

        public static ZonedDateTime GetZonedDateTime(LocalDate date, LocalTime time)
        {
            return GetZonedDateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond, time.NanosecondOfSecond, ORIGINAL_TIME_ZONE);
        }

        public static ZonedDateTime GetZonedDateTime(LocalDate date, LocalTime time, DateTimeZone zone)
        {
            return GetZonedDateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond, time.NanosecondOfSecond, zone);
        }

        public static ZonedDateTime GetZonedDateTime(LocalDate date, LocalTime time, TimeZoneInfo info)
        {
            var tz = DateTimeZoneProviders.Bcl.GetZoneOrNull(info.Id);
            DateTimeZone zone;
            if (tz == null)
            {
                zone = ORIGINAL_TIME_ZONE;
            }else
            {
                zone = tz;
            }
            return GetZonedDateTime(date, time, zone);
        }

        public static ZonedDateTime GetZonedDateTimeNow()
        {
            DateTime now = DateTime.Now;

            return GetZonedDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
        }


        public static String GetTimeZoneIdByOffsetString(String Id)
        {
            String TimeZoneIdString = "";

            Id = Id.Replace("GMT", "UTC");
            String time = Id.Replace("UTC", "");

            switch (time)
            {
                case "-12":
                    time = "-12:00";
                    break;
                case "-11":
                    time = "-11:00";
                    break;
                case "-10":
                    time = "-10:00";
                    break;
                case "-9":
                    time = "-09:00";
                    break;
                case "-8":
                    time = "-08:00";
                    break;
                case "-7":
                    time = "-07:00";
                    break;
                case "-6":
                    time = "-06:00";
                    break;
                case "-5":
                    time = "-05:00";
                    break;
                case "-4":
                    time = "-04:00";
                    break;
                case "-3":
                    time = "-03:00";
                    break;
                case "-2":
                    time = "-02:00";
                    break;
                case "-1":
                    time = "-01:00";
                    break;
                case "+0":
                    time = "+00:00";
                    break;
                case "+1":
                    time = "+01:00";
                    break;
                case "+2":
                    time = "+02:00";
                    break;
                case "+3":
                    time = "+03:00";
                    break;
                case "+4":
                    time = "+04:00";
                    break;
                case "+5":
                    time = "+05:00";
                    break;
                case "+6":
                    time = "+06:00";
                    break;
                case "+7":
                    time = "+07:00";
                    break;
                case "+8":
                    time = "+08:00";
                    break;
                case "+9":
                    time = "+09:00";
                    break;
                case "+10":
                    time = "+10:00";
                    break;
                case "+11":
                    time = "+11:00";
                    break;
                case "+12":
                    time = "+12:00";
                    break;
                case "+13":
                    time = "+13:00";
                    break;
                case "+14":
                    time = "+14:00";
                    break;
            }

            Id = "UTC" + time;

            switch (Id)
            {
                case "UTC-12:00":
                    TimeZoneIdString = "Dateline Standard Time";
                    break;
                case "UTC-11:00":
                    TimeZoneIdString = "UTC-11";
                    break;
                case "UTC-10:00":
                    TimeZoneIdString = "Hawaiian Standard Time";
                    break;
                case "UTC-09:30":
                    TimeZoneIdString = "Marquesas Standard Time";
                    break;
                case "UTC-09:00":
                    TimeZoneIdString = "UTC-09";
                    break;
                case "UTC-08:00":
                    TimeZoneIdString = "Pacific Standard Time";
                    break;
                case "UTC-07:00":
                    TimeZoneIdString = "Mountain Standard Time";
                    break;
                case "UTC-06:00":
                    TimeZoneIdString = "Central Standard Time";
                    break;
                case "UTC-05:00":
                    TimeZoneIdString = "Eastern Standard Time";
                    break;
                case "UTC-04:00":
                    TimeZoneIdString = "Atlantic Standard Time";
                    break;
                case "UTC-03:30":
                    TimeZoneIdString = "Newfoundland Standard Time";
                    break;
                case "UTC-03:00":
                    TimeZoneIdString = "Bahia Standard Time";
                    break;
                case "UTC-02:00":
                    TimeZoneIdString = "UTC-02";
                    break;
                case "UTC-01:00":
                    TimeZoneIdString = "Azores Standard Time";
                    break;
                case "UTC":
                    TimeZoneIdString = "UTC";
                    break;
                case "UTC+00:00":
                    TimeZoneIdString = "GMT Standard Time";
                    break;
                case "UTC+01:00":
                    TimeZoneIdString = "W. Europe Standard Time";
                    break;
                case "UTC+02:00":
                    TimeZoneIdString = "Israel Standard Time";
                    break;
                case "UTC+03:00":
                    TimeZoneIdString = "Russian Standard Time";
                    break;
                case "UTC+03:30":
                    TimeZoneIdString = "Iran Standard Time";
                    break;
                case "UTC+04:00":
                    TimeZoneIdString = "Arabian Standard Time";
                    break;
                case "UTC+04:30":
                    TimeZoneIdString = "Afghanistan Standard Time";
                    break;
                case "UTC+05:00":
                    TimeZoneIdString = "Pakistan Standard Time";
                    break;
                case "UTC+05:30":
                    TimeZoneIdString = "Chennai, Kolkata, Mumbai, New Delhi";
                    break;
                case "UTC+05:45":
                    TimeZoneIdString = "India Standard Time";
                    break;
                case "UTC+06:00":
                    TimeZoneIdString = "Central Asia Standard Time";
                    break;
                case "UTC+06:30":
                    TimeZoneIdString = "Myanmar Standard Time";
                    break;
                case "UTC+07:00":
                    TimeZoneIdString = "SE Asia Standard Time";
                    break;
                case "UTC+08:00":
                    TimeZoneIdString = "China Standard Time";
                    break;
                case "UTC+08:45":
                    TimeZoneIdString = "Aus Central W. Standard Time";
                    break;
                case "UTC+09:00":
                    TimeZoneIdString = "Tokyo Standard Time";
                    break;
                case "UTC+09:30":
                    TimeZoneIdString = "Cen. Australia Standard Time";
                    break;
                case "UTC+10:00":
                    TimeZoneIdString = "AUS Eastern Standard Time";
                    break;
                case "UTC+10:30":
                    TimeZoneIdString = "Lord Howe Standard Time";
                    break;
                case "UTC+11:00":
                    TimeZoneIdString = "Central Pacific Standard Time";
                    break;
                case "UTC+12:00":
                    TimeZoneIdString = "UTC+12";
                    break;
                case "UTC+12:45":
                    TimeZoneIdString = "Chatham Islands Standard Time";
                    break;
                case "UTC+13:00":
                    TimeZoneIdString = "UTC+13";
                    break;
                case "UTC+14:00":
                    TimeZoneIdString = "Line Islands Standard Time";
                    break;
            }

            return TimeZoneIdString;
        }

        public static LocalDate OfEpochDay(long epochDay)
        {
            ChronoField.EPOCH_DAY.CheckValidValue(epochDay);
            long zeroDay = epochDay + DAYS_0000_TO_1970;
            // find the march-based year
            zeroDay -= 60;  // adjust to 0000-03-01 so leap day is at end of four year cycle
            long adjust = 0;
            if (zeroDay < 0)
            {
                // adjust negative years to positive for calculation
                long adjustCycles = (zeroDay + 1) / DAYS_PER_CYCLE - 1;
                adjust = adjustCycles * 400;
                zeroDay += -adjustCycles * DAYS_PER_CYCLE;
            }
            long yearEst = (400 * zeroDay + 591) / DAYS_PER_CYCLE;
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

        public static Boolean IsDaylightSavingTime(DateTime now, DateTimeZone zone)
        {
            ZonedDateTime zdt = new ZonedDateTime(now.ToInstant(), zone);
            return zdt.IsDaylightSavingTime();
        }
    }
}
