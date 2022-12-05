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
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using NodaTime;
using NodaTime.Text;

namespace Mercury.Language.Time
{
    /// <summary>
    /// DateTimeUtility Description
    /// </summary>
    public static class DateTimeUtility
    {
        public static NodaTime.Period ParsePeriod(String tenorStr)
        {
            var pattern = DurationPattern.CreateWithInvariantCulture("H'h'm'm's's'");
            var duration = pattern.Parse(tenorStr).Value;

            return NodaTime.Period.FromTicks((long) duration.TotalTicks);
        }

        public static Boolean IsLeap(int year)
        {
            DateTimeZone timeZone = DateTimeZoneProviders.Bcl.GetSystemDefault();
            var date = new ZonedDateTime(new DateTime(year, 1, 1).ToInstant(), timeZone);
            return date.IsLeapYear();
        }

        public static LocalDate ToLocalDate(String dateString)
        {
            DateTime dt = DateTime.Parse(dateString);
            return dt.ToLocalDate();
        }

        public static LocalDate ToLocalDate(String dateString, String format)
        {
            DateTime dt = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            return dt.ToLocalDate();
        }

        public static LocalDate ToLocalDate(String dateString, IFormatProvider provider)
        {
            DateTime dt = DateTime.Parse(dateString, provider);
            return dt.ToLocalDate();
        }

        public static LocalDate ToLocalDate(String dateString, IFormatProvider provider, System.Globalization.DateTimeStyles styles)
        {
            DateTime dt = DateTime.Parse(dateString, provider, styles);
            return dt.ToLocalDate();
        }
    }
}
