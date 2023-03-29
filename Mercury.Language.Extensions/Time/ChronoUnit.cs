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
//
// Copyright (c) 2007-present, Stephen Colebourne & Michael Nascimento Santos
// All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//  * Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//  * Neither the name of JSR-310 nor the names of its contributors
//    may be used to endorse or promote products derived from this software
//    without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;
using NodaTime;

namespace Mercury.Language.Time
{

    /// <summary>
    /// A standard set of date periods units.
    /// <p>
    /// This set of units provide unit-based access to manipulate a date, time or date-time.
    /// The standard set of units can be extended by implementing {@link TemporalUnit}.
    /// <p>
    /// These units are intended to be applicable in multiple calendar systems.
    /// For example, most non-ISO calendar systems define units of years, months and days,
    /// just with slightly different rules.
    /// The documentation of each unit explains how it operates.
    /// <h3>Specification for implementors</h3>
    /// This is a final, immutable and thread-safe enum.
    /// </summary>
    public class ChronoUnit : ITemporalUnit
    {
        #region enum members
        /// <summary>
        /// Unit that represents the concept of a nanosecond, the smallest supported unit of time.
        /// For the ISO calendar system, it is equal to the 1,000,000,000th part of the second unit.
        /// </summary>
        public static readonly ChronoUnit NANOS = new ChronoUnit("Nanos", Duration.FromNanoseconds(1));
        /// <summary>
        /// Unit that represents the concept of a microsecond.
        /// For the ISO calendar system, it is equal to the 1,000,000th part of the second unit.
        /// </summary>
        public static readonly ChronoUnit MICROS = new ChronoUnit("Nanos", Duration.FromNanoseconds(1000));
        /// <summary>
        /// Unit that represents the concept of a millisecond.
        /// For the ISO calendar system, it is equal to the 1000th part of the second unit.
        /// </summary>
        public static readonly ChronoUnit MILLIS = new ChronoUnit("Nanos", Duration.FromNanoseconds(1000000));
        /// <summary>
        /// Unit that represents the concept of a second.
        /// For the ISO calendar system, it is equal to the second in the SI system
        /// of units, except around a leap-second.
        /// </summary>
        public static readonly ChronoUnit SECONDS = new ChronoUnit("Nanos", Duration.FromSeconds(1));
        /// <summary>
        /// Unit that represents the concept of a minute.
        /// For the ISO calendar system, it is equal to 60 seconds.
        /// </summary>
        public static readonly ChronoUnit MINUTES = new ChronoUnit("Nanos", Duration.FromSeconds(60));
        /// <summary>
        /// Unit that represents the concept of an hour.
        /// For the ISO calendar system, it is equal to 60 minutes.
        /// </summary>
        public static readonly ChronoUnit HOURS = new ChronoUnit("Nanos", Duration.FromSeconds(3600));
        /// <summary>
        /// Unit that represents the concept of half a day, as used in AM/PM.
        /// For the ISO calendar system, it is equal to 12 hours.
        /// </summary>
        public static readonly ChronoUnit HALF_DAYS = new ChronoUnit("Nanos", Duration.FromSeconds(43200));
        /// <summary>
        /// Unit that represents the concept of a day.
        /// For the ISO calendar system, it is the standard day from midnight to midnight.
        /// The estimated duration of a day is {@code 24 Hours}.
        /// <p>
        /// When used with other calendar systems it must correspond to the day defined by
        /// the rising and setting of the Sun on Earth. It is not required that days begin
        /// at midnight - when converting between calendar systems, the date should be
        /// equivalent at midday.
        /// </summary>
        public static readonly ChronoUnit DAYS = new ChronoUnit("Nanos", Duration.FromSeconds(86400));
        /// <summary>
        /// Unit that represents the concept of a week.
        /// For the ISO calendar system, it is equal to 7 days.
        /// <p>
        /// When used with other calendar systems it must correspond to an integral number of days.
        /// </summary>
        public static readonly ChronoUnit WEEKS = new ChronoUnit("Nanos", Duration.FromSeconds(7 * 86400L));
        /// <summary>
        /// Unit that represents the concept of a month.
        /// For the ISO calendar system, the length of the month varies by month-of-year.
        /// The estimated duration of a month is one twelfth of {@code 365.2425 Days}.
        /// <p>
        /// When used with other calendar systems it must correspond to an integral number of days.
        /// </summary>
        public static readonly ChronoUnit MONTHS = new ChronoUnit("Nanos", Duration.FromSeconds(31556952L / 12));
        /// <summary>
        /// Unit that represents the concept of a year.
        /// For the ISO calendar system, it is equal to 12 months.
        /// The estimated duration of a year is {@code 365.2425 Days}.
        /// <p>
        /// When used with other calendar systems it must correspond to an integral number of days
        /// or months roughly equal to a year defined by the passage of the Earth around the Sun.
        /// </summary>
        public static readonly ChronoUnit YEARS = new ChronoUnit("Nanos", Duration.FromSeconds(31556952L));
        /// <summary>
        /// Unit that represents the concept of a decade.
        /// For the ISO calendar system, it is equal to 10 years.
        /// <p>
        /// When used with other calendar systems it must correspond to an integral number of days
        /// and is normally an integral number of years.
        /// </summary>
        public static readonly ChronoUnit DECADES = new ChronoUnit("Nanos", Duration.FromSeconds(31556952L * 10L));
        /// <summary>
        /// Unit that represents the concept of a century.
        /// For the ISO calendar system, it is equal to 100 years.
        /// <p>
        /// When used with other calendar systems it must correspond to an integral number of days
        /// and is normally an integral number of years.
        /// </summary>
        public static readonly ChronoUnit CENTURIES = new ChronoUnit("Centuries", Duration.FromSeconds(31556952L * 100L));
        /// <summary>
        /// Unit that represents the concept of a millennium.
        /// For the ISO calendar system, it is equal to 1000 years.
        /// <p>
        /// When used with other calendar systems it must correspond to an integral number of days
        /// and is normally an integral number of years.
        /// </summary>
        public static readonly ChronoUnit MILLENNIA = new ChronoUnit("Millennia", Duration.FromSeconds(31556952L * 1000L));
        /// <summary>
        /// Unit that represents the concept of an era.
        /// The ISO calendar system doesn't have eras thus it is impossible to add
        /// an era to a date or date-time.
        /// The estimated duration of the era is artificially defined as {@code 1,000,000,000 Years}.
        /// <p>
        /// When used with other calendar systems there are no restrictions on the unit.
        /// NodaTime has limit to create Duration from days of 16777215.
        /// </summary>
        public static readonly ChronoUnit ERAS = new ChronoUnit("Eras", Duration.FromDays(16777215));
        /// <summary>
        /// Artificial unit that represents the concept of forever.
        /// This is primarily used with {@link TemporalField} to represent unbounded fields
        /// such as the year or era.
        /// The estimated duration of the era is artificially defined as the largest duration
        /// supported by {@code Duration}.
        /// </summary>
        public static readonly ChronoUnit FOREVER = new ChronoUnit("Forever", Duration.MaxValue);

        public static readonly ChronoUnit WEEK_BASED_YEARS = new ChronoUnit("WeekBasedYears", Duration.FromSeconds(31556952L));

        public static readonly ChronoUnit QUARTER_YEARS = new ChronoUnit("QuarterYears", Duration.FromSeconds(31556952L / 4));
        #endregion

        public string Name { get; private set; }
        public Duration Duration { get; private set; }

        private ChronoUnit(string name, Duration estimatedDuration)
        {
            this.Name = name;
            this.Duration = estimatedDuration;
        }

        public Boolean IsDurationEstimated
        {
            get { return IsDateBased || this == FOREVER; }
        }

        public Boolean IsDateBased
        {
            get { return this.Duration.CompareTo(DAYS.Duration) >= 0 && this != FOREVER; }
        }

        public Boolean IsTimeBased
        {
            get { return this.Duration.CompareTo(DAYS.Duration) < 0; }
        }

        public Boolean IsSupportedBy(Temporal temporal)
        {
            if (this == FOREVER)
            {
                return false;
            }
            if (temporal.Type == Temporal.ValueType.LocalDate) {
                return IsDateBased;
            }
            if (temporal.Type == Temporal.ValueType.ZonedDateTime) {
                return true;
            }
            try
            {
                temporal.Plus(1, this);
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                try
                {
                    temporal.Plus(-1, this);
                    return true;
                }
                catch (System.Exception ex2)
                {
                    Console.WriteLine(ex2.Message);
                    return false;
                }
            }
        }

        public dynamic AddTo(Temporal dateTime, long periodToAdd)
        {
            return dateTime.Plus(periodToAdd, this);
        }
    }
}
