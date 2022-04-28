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
    }
}