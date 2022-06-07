using NUnit.Framework;
using System;
using Mercury.Language.Time;
using NodaTime;

namespace Mercury_Language_Extensions.Test
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

    }
}