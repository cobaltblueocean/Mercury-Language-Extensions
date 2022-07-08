using NUnit.Framework;
using System;
using Mercury.Language.Time;
using NodaTime;
using Mercury.Test.Utility;

namespace Mercury.Language.Extensions.Test
{
    public class NodaTimeExtensionTest
    {
        private static ZonedDateTime DATE_0 = NodaTimeUtility.GetUTCDate(2013, 9, 30);
        private static ZonedDateTime DATE_1 = NodaTimeUtility.GetUTCDate(2013, 12, 31);
        private static ZonedDateTime DATE_2 = NodaTimeUtility.GetUTCDate(2014, 3, 31);
        private static ZonedDateTime DATE_3 = NodaTimeUtility.GetUTCDate(2014, 6, 30);

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            ZonedDateTime startDate = NodaTimeUtility.GetUTCDate(2014, 5, 26);   //{2014-08-25T07:30:00 UTC (+00)}
            Period tenor = Period.FromMonths(3);
            ZonedDateTime endDate = startDate.Plus(tenor);

            ZonedDateTime endDate2 = endDate.With(TemporalAdjuster.LastDayOfMonth());

            Assert.Pass();
        }

        [Test]
        public void TestGetDateOffsetWithYearFraction()
        {
            ZonedDateTime FIXING_DATE = NodaTimeUtility.GetUTCDate(2011, 1, 3);
            ZonedDateTime[] FIXING_DATES = { FIXING_DATE, FIXING_DATE.PlusYears(1), FIXING_DATE.GetDateOffsetWithYearFraction(1.0) };

            Assert.Pass();
        }


        [Test]
        public void TestGetZonedDateTime()
        {
            ZonedDateTime FIXING_DATE = new DateTime(2014, 4, 2, 0, 0, 0, 0).ToZonedDateTime();

            DateTimeZone timeZone = DateTimeZoneProviders.Tzdb["EST"];

            ZonedDateTime time2 = new DateTime(2014, 4, 2, 15, 0, 0, 0).ToZonedDateTime(timeZone);

            Assert.Pass();
        }

        [Test]
        public void TestZonedDateTimeOperation()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2012, 3, 9, 0, 0, 0, 0).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2012, 12, 9, 0, 0, 0, 0).ToZonedDateTime();
            Period tenorPeriod = Period.FromMonths(3);
            BASE_DATE = BASE_DATE.Minus(tenorPeriod.MultipliedBy(3));

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }


        [Test]
        public void TestZonedDateTimeMinusMonths()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2011, 3, 9, 0, 0, 0, 0).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2012, 3, 9, 0, 0, 0, 0).ToZonedDateTime();
            BASE_DATE = BASE_DATE.PlusMonths(-12);

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }

        [Test]
        public void TestZonedDateTimePlusDays()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2014, 3, 31, 0, 0, 0, 0).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2014, 3, 27, 0, 0, 0, 0).ToZonedDateTime();
            BASE_DATE = BASE_DATE.PlusDays(4);

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }

        [Test]
        public void TestZonedDateTimePlusMonthsWithEndOfMonthDay()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2012, 6, 30, 0, 0, 0, 0).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2012, 3, 31, 0, 0, 0, 0).ToZonedDateTime();
            Period period = Period.FromMonths(3);
            BASE_DATE = BASE_DATE.Plus(period);

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }

        [Test]
        public void TestGetZonedDateTimeWithZone()
        {
            int year = 2014;
            int month = 10;
            int day = 23;
            int hour = 13;
            int minute = 46;
            int second = 0;

            int utcYear = 2014;
            int utcMonth = 10;
            int utcDay = 23;
            int utcHour = 18;
            int utcMinute = 46;
            int utcSecond = 0;

            DateTimeZone ZID = DateTimeZoneProviders.Tzdb["America/New_York"];
            ZonedDateTime TEST_DATE = NodaTimeUtility.GetZonedDateTime(year, month, day, hour, minute, second, ZID);

            Assert.AreEqual(TEST_DATE.Year, year);
            Assert.AreEqual(TEST_DATE.Month, month);
            Assert.AreEqual(TEST_DATE.Day, day);
            Assert.AreEqual(TEST_DATE.Hour, hour);
            Assert.AreEqual(TEST_DATE.Minute, minute);
            Assert.AreEqual(TEST_DATE.Second, second);

            ZonedDateTime UTC_DATE = TEST_DATE.ChangeToDifferentTimeZone(DateTimeZone.Utc);

            Assert.AreEqual(UTC_DATE.Year, utcYear);
            Assert.AreEqual(UTC_DATE.Month, utcMonth);
            Assert.AreEqual(UTC_DATE.Day, utcDay);
            Assert.AreEqual(UTC_DATE.Hour, utcHour);
            Assert.AreEqual(UTC_DATE.Minute, utcMinute);
            Assert.AreEqual(UTC_DATE.Second, utcSecond);
        }

        [Test]
        public void TestEndOfMonthDateCreate()
        {
            int year = 2014;
            int month = 6;
            int day = 30;
            int hour = 0;
            int minute = 0;
            int second = 0;

            DateTimeZone ZID = DateTimeZoneProviders.Tzdb["America/New_York"];
            ZonedDateTime TEST_DATE = NodaTimeUtility.GetZonedDateTime(year, month, day, hour, minute, second, ZID);

            Assert.AreEqual(TEST_DATE.Year, year);
            Assert.AreEqual(TEST_DATE.Month, month);
            Assert.AreEqual(TEST_DATE.Day, day);
            Assert.AreEqual(TEST_DATE.Hour, hour);
            Assert.AreEqual(TEST_DATE.Minute, minute);
            Assert.AreEqual(TEST_DATE.Second, second);
        }
    }
}