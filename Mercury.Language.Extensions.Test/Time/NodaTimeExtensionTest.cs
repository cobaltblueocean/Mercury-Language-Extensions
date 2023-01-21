using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Assert.AreEqual(endDate2.Year, 2014);
            Assert.AreEqual(endDate2.Month, 8);
            Assert.AreEqual(endDate2.Day, 31);
        }


        [Test]
        public void TestZonedDateTimeCalc()
        {
            ZonedDateTime startDate = NodaTimeUtility.GetUTCDate(2011, 6, 20);
            ZonedDateTime endDate = startDate.PlusYears(1).PlusDays(15);
            Period tenorPeriod = Period.FromMonths(6);
            Boolean fromEnd = true;
            Boolean stubShort = true;

            List<ZonedDateTime> dates = new List<ZonedDateTime>();
            int nbPeriod = 0;
            if (!fromEnd)
            { // Add the periods from the start date
                ZonedDateTime sdt = startDate.Plus(tenorPeriod);
                while (sdt.IsBefore(endDate))
                { // date is strictly before endDate
                    dates.Add(sdt);
                    nbPeriod++;
                    sdt = startDate.Plus(tenorPeriod.MultipliedBy(nbPeriod + 1));
                }
                if (!stubShort && !sdt.AreObjectsEqual(endDate) && nbPeriod >= 1)
                { // For long stub the last date before end date, if any, is removed.
                    dates.RemoveAt(nbPeriod - 1);
                }
                dates.Add(endDate);
            }
            // From end - Subtract the periods from the end date
            ZonedDateTime? date = endDate;
            while (date.IsAfter(startDate))
            { // date is strictly after startDate
                dates.Add(date.Value);
                nbPeriod++;
                date = endDate.Minus(tenorPeriod.MultipliedBy(nbPeriod));
            }
            if (!stubShort && !date.AreObjectsEqual(startDate) && nbPeriod > 1)
            { // For long stub the last date before end date, if any, is removed.
                dates.RemoveAt(nbPeriod - 1);
            }

            var TARGET_DATE1 = NodaTimeUtility.GetUTCDate(2012, 7, 5);
            var TARGET_DATE2 = NodaTimeUtility.GetUTCDate(2012, 1, 5);
            var TARGET_DATE3 = NodaTimeUtility.GetUTCDate(2011, 7, 5);


            Assert.AreEqual(dates[0], TARGET_DATE1);
            Assert.AreEqual(dates[1], TARGET_DATE2);
            Assert.AreEqual(dates[2], TARGET_DATE3);
        }

        [Test]
        public void TestGetDateOffsetWithYearFraction()
        {
            ZonedDateTime FIXING_DATE = NodaTimeUtility.GetUTCDate(2011, 1, 3);
            ZonedDateTime TARGET_DATE2 = NodaTimeUtility.GetUTCDate(2012, 1, 3);
            ZonedDateTime TARGET_DATE3 = NodaTimeUtility.GetUTCDate(2012, 1, 3).PlusHours(6);
            ZonedDateTime[] FIXING_DATES = { FIXING_DATE, FIXING_DATE.PlusYears(1), FIXING_DATE.GetDateOffsetWithYearFraction(1.0) };


            Assert.AreEqual(FIXING_DATES[0], FIXING_DATE);
            Assert.AreEqual(FIXING_DATES[1], TARGET_DATE2);
            Assert.AreEqual(FIXING_DATES[2], TARGET_DATE3);
        }


        [Test]
        public void TestGetZonedDateTime()
        {
            //ZonedDateTime FIXING_DATE = new DateTime(2014, 4, 2, 0, 0, 0, 0).ToZonedDateTime();

            DateTimeZone timeZone = DateTimeZoneProviders.Tzdb["EST"];

            ZonedDateTime time2 = new DateTime(2014, 4, 2, 15, 0, 0, 0).ToZonedDateTime(timeZone);

            Assert.AreEqual(timeZone.Id, "EST");
            Assert.AreEqual(timeZone.Id, time2.Zone.Id);
            Assert.AreEqual(time2.Year, 2014);
            Assert.AreEqual(time2.Month, 4);
            Assert.AreEqual(time2.Day, 2);
            Assert.AreEqual(time2.Hour, 15);
            Assert.AreEqual(time2.Minute, 0);
            Assert.AreEqual(time2.Second, 0);
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
        public void TestZonedDateTimeWithTemporalAdjuster()
        {
            ZonedDateTime zdt = NodaTimeUtility.GetUTCDate(2011, 1, 1);
            zdt = zdt.PlusMonths(-2);
            var zdt1 = zdt.With(TemporalAdjuster.LastDayOfMonth(zdt.Zone)).WithHour(0).WithMinute(0);
            var zdt2 = NodaTimeUtility.GetUTCDate(2010, 11, 30);

            long secs1 = Math2.SafeMultiply(zdt1.GetEpochSecond(), 1_000_000_000);
            long epochSec1 =  Math2.SafeAdd(secs1, zdt1.NanosecondOfSecond);

            long secs2 = Math2.SafeMultiply(zdt2.GetEpochSecond(), 1_000_000_000);
            long epochSec2 = Math2.SafeAdd(secs2, zdt2.NanosecondOfSecond);

            Assert.AreEqual(epochSec1, 1291075200000000000);
            Assert.AreEqual(epochSec2, 1291075200000000000);
        }

        [Test]
        public void TestTemporai()
        {
            ZonedDateTime BASE_DATE = NodaTimeUtility.GetUTCDate(2011, 6, 20);
            ZonedDateTime TARGET_DATE = NodaTimeUtility.GetUTCDate(2011, 6, 30);
            //Temporal temporal;

            //temporal.With(ChronoField.DAY_OF_MONTH, temporal.GetRange(ChronoField.DAY_OF_MONTH).MaxLargest);

            var result = BASE_DATE.With(TemporalAdjuster.LastDayOfMonth());
            Assert.AreEqual(TARGET_DATE, result);
        }

        [Test]
        public void TestZonedDateTimeMinusMonths()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2011, 3, 9, 0, 0, 0, 0, DateTimeKind.Local).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2012, 3, 9, 0, 0, 0, 0, DateTimeKind.Local).ToZonedDateTime();
            BASE_DATE = BASE_DATE.PlusMonths(-12);

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }

        [Test]
        public void TestZonedDateTimePlusDays()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2014, 3, 31, 0, 0, 0, 0, DateTimeKind.Local).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2014, 3, 27, 0, 0, 0, 0, DateTimeKind.Local).ToZonedDateTime();
            BASE_DATE = BASE_DATE.PlusDays(4);

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }

        [Test]
        public void TestZonedDateTimePlusMonthsWithEndOfMonthDay()
        {
            ZonedDateTime TARGET_DATE = new DateTime(2012, 6, 30, 0, 0, 0, 0, DateTimeKind.Local).ToZonedDateTime();
            ZonedDateTime BASE_DATE = new DateTime(2012, 3, 31, 0, 0, 0, 0, DateTimeKind.Local).ToZonedDateTime();
            Period period = Period.FromMonths(3);
            BASE_DATE = BASE_DATE.Plus(period);

            Assert.AreEqual(TARGET_DATE, BASE_DATE);
        }

        [Test]
        public void GetZonedDateTimeFromLocalDateAndTimeTest()
        {
            var ldt = DateTimeUtility.ToLocalDate("20150821", "yyyyMMdd");

            var ldt2 = DateTimeUtility.ToLocalDate("08/21/2015", "MM/dd/yyyy");

            Assert.AreEqual(ldt.Year, ldt2.Year);
            Assert.AreEqual(ldt.Month, ldt2.Month);
            Assert.AreEqual(ldt.Day, ldt2.Day);

            var lt = new LocalTime(13, 11);
            var zoneName = NodaTimeUtility.GetTimeZoneIdByOffsetString("GMT-4");
            var info = TimeZoneInfo.FindSystemTimeZoneById(NodaTimeUtility.GetTimeZoneIdByOffsetString("GMT-4"));

            var zdt = NodaTimeUtility.GetZonedDateTime(ldt, lt, info);
            var dt = new DateTime(2015, 8, 21, 13, 11, 0);

            Assert.AreEqual(zdt.Year, dt.Year);
            Assert.AreEqual(zdt.Month, dt.Month);
            Assert.AreEqual(zdt.Day, dt.Day);
            Assert.AreEqual(zdt.Hour, dt.Hour);
            Assert.AreEqual(zdt.Minute, dt.Minute);
            Assert.AreEqual(zdt.Second, dt.Second);
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