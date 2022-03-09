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

namespace System
{
    public static class DateTimeExtension
    {

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

        public static Boolean IsAfter(this DateTime now, DateTime target)
        {
            if (DateTime.Compare(now, target) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this DateTime now, DateTime target)
        {
            if (DateTime.Compare(now, target) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsAfter(this DateTime? now, DateTime target)
        {
            if (DateTime.Compare((DateTime)now, target) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this DateTime? now, DateTime target)
        {
            if (DateTime.Compare((DateTime)now, target) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime? FirstNonNull(this DateTime? source, DateTime val)
        {
            if (source == null)
                source = val;

            return source;
        }

        public static DateTime AddNanos(this DateTime source, long nanos)
        {
            return source.AddTicks(nanos * 100);
        }

        public static DateTime AddNanos(this DateTime source, double nanos)
        {
            return AddNanos(source, Convert.ToInt64(nanos));
        }

        public static ZonedDateTime ToZonedDateTime(this DateTime source)
        {
            DateTimeZone timeZone = DateTimeZoneProviders.Bcl.GetSystemDefault();
            return new ZonedDateTime(source.ToInstant(), timeZone);
        }

        public static LocalDate ToLocalDate(this DateTime source)
        {
            return LocalDate.FromDateTime(source);
        }

        public static Instant ToInstant(this DateTime source)
        {
            return Instant.FromDateTimeUtc(source.ToUniversalTime());
        }

        public static double GetDifferenceInMonths(this Instant startDate, Instant endDate)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException("Start date was null");
            }
            if (endDate == null)
            {
                throw new ArgumentNullException("End date was null");
            }

            return (double)(Instant.Subtract(endDate, startDate).TotalMilliseconds) / MILLISECONDS_PER_MONTH;
        }

        public static double GetDifferenceInMonths(this DateTime startDate, DateTime endDate)
        {
            return GetDifferenceInMonths(startDate.ToInstant(), endDate.ToInstant());
        }

        public static double GetDifferenceInYears(this DateTime startDate, DateTime endDate)
        {
            return startDate.ToInstant().GetDifferenceInYears(endDate.ToInstant());
        }

        public static DateTime GetDateOffsetWithYearFraction(this DateTime startDate, double yearFraction)
        {
            if (startDate == null)
            {
                throw new ArgumentNullException("Date was null");
            }
            double nanos = System.Math.Round(1e9 * SECONDS_PER_YEAR * yearFraction);
            return startDate.AddNanos(nanos);
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long CurrentTimeMillis(this DateTime dt)
        {
            return (long)(dt.ToUniversalTime() - Jan1st1970).TotalMilliseconds;
        }

    }
}
