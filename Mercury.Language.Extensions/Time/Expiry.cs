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
using NodaTime;

namespace Mercury.Language.Time
{
    /// <summary>
    /// Expiry Description
    /// </summary>
    [Serializable]
    public sealed class Expiry
    {
        private ZonedDateTime _expiry;
        private ExpiryAccuracy _accuracy;

        /// <summary>
        /// Creates an expiry with no specific accuracy.
        /// </summary>
        /// <param name="expiry">the expiry date-time</param>
        public Expiry(ZonedDateTime expiry) : this(expiry, ExpiryAccuracy.DAY_MONTH_YEAR)
        {

        }

        /// <summary>
        /// Creates an expiry with an accuracy.
        /// </summary>
        /// <param name="expiry">the expiry date-time, not-null</param>
        /// <param name="accuracy">the accuracy</param>
        public Expiry(ZonedDateTime expiry, ExpiryAccuracy accuracy)
        {
            // ArgumentChecker.NotNull(expiry, "expiry");
            // ArgumentChecker.NotNull(accuracy, "accuracy");
            _expiry = expiry;
            _accuracy = accuracy;
        }

        /// <summary>
        /// Gets the expiry date-time.
        /// </summary>
        public ZonedDateTime ExpiryDateTime
        {
            get { return _expiry; }
        }

        /// <summary>
        /// Gets the accuracy of the expiry.
        /// </summary>
        public ExpiryAccuracy Accuracy
        {
            get { return _accuracy; }
        }

        /// <summary>
        /// Converts the expiry date-time to an instant.
        /// </summary>
        /// <returns></returns>
        public Instant ToInstant()
        {
            return _expiry.ToInstant();
        }

        /// <summary>
        /// Compares two expiry dates for equality to the given level of accuracy only.
        /// </summary>
        /// <param name="accuracy">the accuracy to compare to, not null</param>
        /// <param name="expiry1">the first date/time to compare, not null</param>
        /// <param name="expiry2">the second date/time to compare, not null</param>
        /// <returns>true if the two dates/times are equal to the requested accuracy</returns>
        public static Boolean EqualsToAccuracy(ExpiryAccuracy accuracy, ZonedDateTime expiry1, ZonedDateTime expiry2)
        {
            switch (accuracy)
            {
                case ExpiryAccuracy.MIN_HOUR_DAY_MONTH_YEAR:
                    return (expiry1.Minute == expiry2.Minute) && (expiry1.Hour == expiry2.Hour) && (expiry1.Day == expiry2.Day)
                        && (expiry1.Month == expiry2.Month) && (expiry1.Year == expiry2.Year);
                case ExpiryAccuracy.HOUR_DAY_MONTH_YEAR:
                    return (expiry1.Hour == expiry2.Hour) && (expiry1.Day == expiry2.Day) && (expiry1.Month == expiry2.Month)
                        && (expiry1.Year == expiry2.Year);
                case ExpiryAccuracy.DAY_MONTH_YEAR:
                    return (expiry1.Day == expiry2.Day) && (expiry1.Month == expiry2.Month) && (expiry1.Year == expiry2.Year);
                case ExpiryAccuracy.MONTH_YEAR:
                    return (expiry1.Month == expiry2.Month) && (expiry1.Year == expiry2.Year);
                case ExpiryAccuracy.YEAR:
                    return (expiry1.Year == expiry2.Year);
                default:
                    throw new ArgumentException("accuracy");
            }
        }

    }
    public enum ExpiryAccuracy
    {
        /// <summary>
        /// Accurate to a minute.
        /// </summary>
        MIN_HOUR_DAY_MONTH_YEAR,
        /// <summary>
        /// Accurate to an hour.
        /// </summary>
        HOUR_DAY_MONTH_YEAR,
        /// <summary>
        /// Accurate to a day.
        /// </summary>
        DAY_MONTH_YEAR,
        /// <summary>
        /// Accurate to a month.
        /// </summary>
        MONTH_YEAR,
        /// <summary>
        /// Accurate to a year.
        /// </summary>
        YEAR
    }
}
