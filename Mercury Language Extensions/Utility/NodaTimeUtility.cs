﻿// Copyright (c) 2017 - presented by Kei Nakai
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
using Mercury.Language.Exception;
using NodaTime;
using Mercury.Language.Time;

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


        public static ZonedDateTime GetUTCDate(int year, int month, int day)
        {
            LocalDate localDate = new LocalDate(year, month, day);
            return new ZonedDateTime(localDate.ToInstant(), DateTimeZone.Utc);
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
    }
}
