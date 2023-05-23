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
using NUnit.Framework;
using System;
using System.Globalization;
using Mercury.Language.Time;
using NodaTime;

namespace Mercury.Language.Extensions.Test.Utility
{
    /// <summary>
    /// DateTimeUtilityTest Description
    /// </summary>
    public class DateTimeUtilityTest
    {
        [Test]
        public void CreateDateTimeTest()
        {
            var dateString1 = "20150817";
            var dateString2 = "8/17/2015";
            var format = "yyyyMMdd";
            var date = DateTime.ParseExact(dateString1, format, CultureInfo.InvariantCulture);

            var ldt1 = DateTimeUtility.ToLocalDate(dateString1, format);
            var ldt2 = DateTimeUtility.ToLocalDate(dateString2);

            Assert.IsTrue(date.Year == ldt1.Year);
            Assert.IsTrue(date.Month == ldt1.Month);
            Assert.IsTrue(date.Day == ldt1.Day);

            Assert.IsTrue(ldt1.Year == ldt2.Year);
            Assert.IsTrue(ldt1.Month == ldt2.Month);
            Assert.IsTrue(ldt1.Day == ldt2.Day);
        }

        [Test]
        public void TemporalAdjusterTest()
        {
            ITemporalAdjuster THIRD_WEDNESDAY = TemporalAdjuster.DayOfWeekInMonth(3, IsoDayOfWeek.Wednesday);
            var date = NodaTimeUtility.GetUTCDate(2013, 9, 18);
            var expected = NodaTimeUtility.GetUTCDate(2013, 12, 18);
            var t1 = new Temporal(date);

            t1 = new Temporal(t1.With(TemporalAdjuster.FirstDayOfMonth()));
            t1 = new Temporal(t1.Plus(3L, ChronoUnit.MONTHS));
            var t2 = t1.With(THIRD_WEDNESDAY);

            Assert.AreEqual(expected, t2);
        }

        [Test]
        public void PeriodManupilationTest()
        {
            var period1 = Period.FromMonths(6);
            var period2 = Period.FromMonths(3);

            var result = period1.Minus(period2);

            Assert.AreEqual(result, period2);

            result = result.Plus(period2);

            Assert.AreEqual(result, period1);
        }

        [Test]
        public void LocalDateTest()
        {
            LocalDate TRADE_DATE = new LocalDate(2011, Month.JUNE.Value, 13);
            var dateLong = TRADE_DATE.GetLong(JulianField.MODIFIED_JULIAN_DAY);

            Assert.AreEqual(55725, dateLong);
        }
    }
}
