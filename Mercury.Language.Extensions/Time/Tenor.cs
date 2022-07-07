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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using OpenGamma.Utility;
using Mercury.Language.Exception;
using NodaTime;
using NodaTime.Text;

namespace Mercury.Language.Time
{
    using Duration = NodaTime.Duration;
    using Period = NodaTime.Period;

    /// <summary>
    /// Tenor Description
    /// </summary>
    [Serializable]
    public class Tenor : IComparable<Tenor>
    {
        #region Fields
        /// <summary>
        /// An overnight tenor.
        /// </summary>
        [Obsolete("Deprecated, use Tenor.ON", true)]
        public static Tenor OVERNIGHT = Tenor.Of(Period.FromDays(1));

        /// <summary>
        /// A tenor of one day.
        /// </summary>
        public static Tenor DAY = Tenor.Of(Period.FromDays(1));
        /// <summary>
        /// A tenor of one day.
        /// </summary>
        public static Tenor ONE_DAY = Tenor.Of(Period.FromDays(1));
        /// <summary>
        /// A tenor of two days.
        /// </summary>
        public static Tenor TWO_DAYS = Tenor.Of(Period.FromDays(2));
        /// <summary>
        /// A tenor of two days.
        /// </summary>
        public static Tenor THREE_DAYS = Tenor.Of(Period.FromDays(3));
        /// <summary>
        /// A tenor of 1 week.
        /// </summary>
        public static Tenor ONE_WEEK = Tenor.Of(Period.FromDays(7));
        /// <summary>
        /// A tenor of 2 weeks.
        /// </summary>
        public static Tenor TWO_WEEKS = Tenor.Of(Period.FromDays(14));
        /// <summary>
        /// A tenor of 3 weeks.
        /// </summary>
        public static Tenor THREE_WEEKS = Tenor.Of(Period.FromDays(21));
        /// <summary>
        /// A tenor of 6 weeks.
        /// </summary>
        public static Tenor SIX_WEEKS = Tenor.Of(Period.FromDays(42));
        /// <summary>
        /// A tenor of 1 month.
        /// </summary>
        public static Tenor ONE_MONTH = Tenor.Of(Period.FromMonths(1));
        /// <summary>
        /// A tenor of 2 months.
        /// </summary>
        public static Tenor TWO_MONTHS = Tenor.Of(Period.FromMonths(2));
        /// <summary>
        /// A tenor of 3 months.
        /// </summary>
        public static Tenor THREE_MONTHS = Tenor.Of(Period.FromMonths(3));
        /// <summary>
        /// A tenor of 4 months.
        /// </summary>
        public static Tenor FOUR_MONTHS = Tenor.Of(Period.FromMonths(4));
        /// <summary>
        /// A tenor of 5 months.
        /// </summary>
        public static Tenor FIVE_MONTHS = Tenor.Of(Period.FromMonths(5));
        /// <summary>
        /// A tenor of 6 months.
        /// </summary>
        public static Tenor SIX_MONTHS = Tenor.Of(Period.FromMonths(6));
        /// <summary>
        /// A tenor of 7 months.
        /// </summary>
        public static Tenor SEVEN_MONTHS = Tenor.Of(Period.FromMonths(7));
        /// <summary>
        /// A tenor of 8 months.
        /// </summary>
        public static Tenor EIGHT_MONTHS = Tenor.Of(Period.FromMonths(8));
        /// <summary>
        /// A tenor of 9 months.
        /// </summary>
        public static Tenor NINE_MONTHS = Tenor.Of(Period.FromMonths(9));
        /// <summary>
        /// A tenor of 10 months.
        /// </summary>
        public static Tenor TEN_MONTHS = Tenor.Of(Period.FromMonths(10));
        /// <summary>
        /// A tenor of 11 months.
        /// </summary>
        public static Tenor ELEVEN_MONTHS = Tenor.Of(Period.FromMonths(11));
        /// <summary>
        /// A tenor of 12 months.
        /// </summary>
        public static Tenor TWELVE_MONTHS = Tenor.Of(Period.FromMonths(12));
        /// <summary>
        /// A tenor of 18 months.
        /// </summary>
        public static Tenor EIGHTEEN_MONTHS = Tenor.Of(Period.FromMonths(18));
        /// <summary>
        /// A tenor of 1 year.
        /// </summary>
        public static Tenor ONE_YEAR = Tenor.Of(Period.FromYears(1));
        /// <summary>
        /// A tenor of 2 years.
        /// </summary>
        public static Tenor TWO_YEARS = Tenor.Of(Period.FromYears(2));
        /// <summary>
        /// A tenor of 3 years.
        /// </summary>
        public static Tenor THREE_YEARS = Tenor.Of(Period.FromYears(3));
        /// <summary>
        /// A tenor of 4 years.
        /// </summary>
        public static Tenor FOUR_YEARS = Tenor.Of(Period.FromYears(4));
        /// <summary>
        /// A tenor of 5 years.
        /// </summary>
        public static Tenor FIVE_YEARS = Tenor.Of(Period.FromYears(5));
        /// <summary>
        /// A tenor of 6 years.
        /// </summary>
        public static Tenor SIX_YEARS = Tenor.Of(Period.FromYears(6));
        /// <summary>
        /// A tenor of 7 years.
        /// </summary>
        public static Tenor SEVEN_YEARS = Tenor.Of(Period.FromYears(7));
        /// <summary>
        /// A tenor of 8 years.
        /// </summary>
        public static Tenor EIGHT_YEARS = Tenor.Of(Period.FromYears(8));
        /// <summary>
        /// A tenor of 9 years.
        /// </summary>
        public static Tenor NINE_YEARS = Tenor.Of(Period.FromYears(9));
        /// <summary>
        /// A tenor of 10 years.
        /// </summary>
        public static Tenor TEN_YEARS = Tenor.Of(Period.FromYears(10));
        /// <summary>
        /// A tenor of one working week (5 days).
        /// </summary>
        public static Tenor WORKING_WEEK = Tenor.Of(Period.FromDays(5));
        /// <summary>
        /// A tenor of the days in a standard year (365 days).
        /// </summary>
        public static Tenor YEAR = Tenor.Of(Period.FromDays(365));
        /// <summary>
        /// A tenor of the days in a leap year (366 days).
        /// </summary>
        public static Tenor LEAP_YEAR = Tenor.Of(Period.FromDays(366));
        /// <summary>
        /// An overnight / next (O/N) tenor.
        /// </summary>
        public static Tenor ON = Tenor.Of(BusinessDayTenor.OVERNIGHT);
        /// <summary>
        /// A spot / next (S/N) tenor.
        /// </summary>
        public static Tenor SN = Tenor.Of(BusinessDayTenor.SPOT_NEXT);
        /// <summary>
        /// A tomorrow / next (a.k.ad tom next, T/N) tenor.
        /// </summary>
        public static Tenor TN = Tenor.Of(BusinessDayTenor.TOM_NEXT);
        #endregion

        #region Variables
        /// <summary>
        /// The period of the tenor.
        /// </summary>
        private Period _period;
        /// <summary>
        /// The business day tenor.
        /// </summary>
        private BusinessDayTenor _businessDayTenor;

        #endregion

        #region Static Constructor Methods
        /// <summary>
        /// Obtains a <see cref="Tenor"/> from a <see cref="Period"/>.
        /// </summary>
        /// <param name="period">the period to convert to a tenor, not null</param>
        /// <returns>the tenor, not null</returns>
        public static Tenor Of(Period period)
        {
            // ArgumentChecker.NotNull(period, "period");
            return new Tenor(period);
        }

        /// <summary>
        /// Obtains a <see cref="Tenor"/> from a <see cref="BusinessDayTenor"/>.
        /// </summary>
        /// <param name="businessDayTenor">the tenor to convert, not null</param>
        /// <returns>the tenor, not null</returns>
        public static Tenor Of(BusinessDayTenor businessDayTenor)
        {
            // ArgumentChecker.NotNull(businessDayTenor, "businessDayTenor");
            return new Tenor(businessDayTenor);
        }


        /// <summary>
        /// Returns a tenor backed by a period of days.
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The tenor</returns>
        public static Tenor FromDays(int days)
        {
            return Tenor.Of(NodaTime.Period.FromDays(days));
        }

        /// <summary>
        /// Returns a tenor backed by a period of weeks.
        /// </summary>
        /// <param name="weeks">The number of weeks</param>
        /// <returns>The tenor</returns>
        public static Tenor FromWeeks(int weeks)
        {
            return Tenor.Of(NodaTime.Period.FromDays(weeks * 7));
        }

        /// <summary>
        /// Returns a tenor backed by a period of months.
        /// </summary>
        /// <param name="months">The number of months</param>
        /// <returns>The tenor</returns>
        public static Tenor FromMonths(int months)
        {
            return Tenor.Of(NodaTime.Period.FromMonths(months)); // TODO: what do we do here
        }

        /// <summary>
        /// Returns a tenor backed by a period of years.
        /// </summary>
        /// <param name="years">The number of years</param>
        /// <returns>The tenor</returns>
        public static Tenor FromYears(int years)
        {
            return Tenor.Of(NodaTime.Period.FromYears(years)); // TODO: what do we do here
        }

        /// <summary>
        /// Returns a tenor of business days.
        /// </summary>
        /// <param name="businessDayTenor">The business day</param>
        /// <returns>The tenor</returns>
        public static Tenor OfBusinessDay(BusinessDayTenor businessDayTenor)
        {
            return Tenor.Of(businessDayTenor);
        }

        /// <summary>
        /// Returns a tenor of business days.
        /// </summary>
        /// <param name="businessDayTenor">The business days name</param>
        /// <returns>The tenor</returns>
        public static Tenor OfBusinessDay(String businessDayTenor)
        {
            return Tenor.Of(BusinessDayTenor.ValueOf(businessDayTenor));
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Parses a formatted string representing the tenor.
        /// <p>
        /// The format is based on ISO-8601, such as 'P3M'.
        /// </summary>
        /// <param name="tenorStr">the string representing the tenor, not null</param>
        /// <returns>the tenor, not null</returns>
        [Obsolete("Deprecated, Use the static factory method Tenor.of(Period).", true)]
        public Tenor Parse(String tenorStr)
        {
            // ArgumentChecker.NotNull(tenorStr, "tenorStr");
            try
            {
                NodaTime.Period period;
                if ("PT0S".Equals(tenorStr))
                {
                    period = NodaTime.Period.Zero;
                }
                else
                {
                    period = DateTimeUtility.ParsePeriod(tenorStr);
                }

                return Tenor.Of(period);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e.Message);
                return Tenor.Of(BusinessDayTenor.ValueOf(tenorStr));
            }
        }

        /// <summary>
        /// Creates a tenor.
        /// </summary>
        /// <param name="period">the period to represent</param>
        protected Tenor(NodaTime.Period period)
        {
            // ArgumentChecker.NotNull(period, "period"); //change of behaviour
            _period = period;
            _businessDayTenor = null;
        }

        /// <summary>
        /// Creates a tenor without a periodd This is used for overnight,
        /// spot next and tomorrow next tenors.
        /// </summary>
        /// <param name="businessDayTenor"></param>
        private Tenor(BusinessDayTenor businessDayTenor)
        {
            // ArgumentChecker.NotNull(businessDayTenor, "business day tenor");
            _period = businessDayTenor.GetPeriod();
            _businessDayTenor = businessDayTenor;
        }

        /// <summary>
        /// Gets the tenor period.
        /// </summary>
        /// <returns>the period</returns>
        /// <exception cref="InvalidOperationException">If the tenor is not backed by a <see cref="Period"/></exception>
        public NodaTime.Period GetPeriod()
        {
            if (_period == null)
            {
                throw new InvalidOperationException(String.Format(LocalizedResources.Instance().COULD_NOT_GET_PERIOD_FOR, ToString()));
            }
            return _period;
        }

        /// <summary>
        /// Gets the business day tenor if the tenor is of appropriate type.
        /// </summary>
        /// <returns>The business day tenor</returns>
        /// <exception cref="InvalidOperationException">If the tenor is backed by a period</exception>
        public BusinessDayTenor GetBusinessDayTenor()
        {
            if (_businessDayTenor == null)
            {
                throw new InvalidOperationException(String.Format(LocalizedResources.Instance().COULD_NOT_GET_BUSINESS_DAY_TENOR_FOR, ToString()));
            }
            return _businessDayTenor;
        }

        /// <summary>
        /// Returns true if the tenor is a business day tenor.
        /// </summary>
        /// <returns>True if the tenor is a business day tenor</returns>
        public Boolean IsBusinessDayTenor
        {
            get { return _period == null; }
        }

        /// <summary>
        /// Returns a formatted string representing the tenor.
        /// <p>
        /// The format is based on ISO-8601, such as 'P3M'.
        /// </summary>
        /// <returns>the formatted tenor, not null</returns>
        public String ToFormattedString()
        {
            if (_period != null)
            {
                return GetPeriod().ToString();
            }
            return GetBusinessDayTenor().ToString();
        }

        public int CompareTo(Tenor other)
        {
            Duration thisDur, otherDur;
            if (_period == null)
            {
                thisDur = _businessDayTenor.GetApproximateDuration();
            }
            else
            {
                thisDur = _period.EstimatedDuration();
            }
            if (other._period == null)
            {
                otherDur = other._businessDayTenor.GetApproximateDuration();
            }
            else
            {
                otherDur = other.GetPeriod().EstimatedDuration();
            }
            return thisDur.CompareTo(otherDur);
        }

        protected static DateTimeOffset ParseIso8601(string iso8601String)
        {
            return DateTimeOffset.ParseExact(
                iso8601String,
                new string[] { "yyyy-MM-dd'T'HH:mm:ss.FFFK" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None);
        }

        protected static string FormatIso8601(DateTimeOffset dto)
        {
            string format = dto.Offset == TimeSpan.Zero
                ? "yyyy-MM-ddTHH:mm:ss.fffZ"
                : "yyyy-MM-ddTHH:mm:ss.fffzzz";

            return dto.ToString(format, CultureInfo.InvariantCulture);
        }
        #endregion

        #region Utility Static Methods
        /// <summary>
        /// Gets the number of days in the tenor.
        /// This method assumes 24-hour days.
        /// Minutes are ignored.
        /// </summary>
        /// <param name="tenor">the tenor, not null</param>
        /// <returns>the number of days</returns>
        public static double GetDaysInTenor(Tenor tenor)
        {
            return tenor.GetPeriod().Days;
        }

        /// <summary>
        /// Subtracts a tenor from a date-time.
        /// </summary>
        /// <param name="dateTime">the date-time to adjust, not null</param>
        /// <param name="tenor">the tenor, not null</param>
        /// <returns>the adjusted date-time</returns>
        [Obsolete("Deprecated, this method name is not clear about whether the tenor is added to or subtracted from the date.", true)]
        public static ZonedDateTime GetDateWithTenorOffset(ZonedDateTime dateTime, Tenor tenor)
        {
            return dateTime.Minus(tenor.GetPeriod());
        }

        /// <summary>
        /// Gets the fraction representing the the number of days in the first tenor
        /// divided by the number of days in the second tenor.
        /// </summary>
        /// <param name="first">the first tenor, not null</param>
        /// <param name="second">the second tenor, not null</param>
        /// <returns>the number of the second tenor in the first</returns>
        [Obsolete("Deprecated, the method name does not make it clear that this applies only to tenors with a number of days in them and would give 0 for first = P2Y, second = P1Y and is not a general-purpose method.", true)]
        public static double GetTenorsInTenor(Tenor first, Tenor second)
        {
            return GetDaysInTenor(first) / GetDaysInTenor(second);
        }
        #endregion

        /// <summary>
        /// Business day tenor.
        /// </summary>
        public class BusinessDayTenor : Tenor
        {
            /// <summary>
            /// Overnight.
            /// </summary>
            public static readonly new BusinessDayTenor OVERNIGHT = new BusinessDayTenor(NodaTime.Period.FromDays(1));

            /// <summary>
            /// Tomorrow / next.
            /// </summary>
            public static readonly BusinessDayTenor TOM_NEXT = new BusinessDayTenor(NodaTime.Period.FromDays(2));

            /// <summary>
            /// Spot / next.
            /// </summary>
            public static readonly BusinessDayTenor SPOT_NEXT = new BusinessDayTenor(NodaTime.Period.FromDays(3));

            /// <summary>
            /// The approximate duration of a business day tenor
            /// </summary>
            private Duration _approximateDuration;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="approximateDuration">The approximate duration of a business day tenord It is not exact because there could be holidays in the period.</param>
            public BusinessDayTenor(NodaTime.Period approximateDuration) : base(approximateDuration)
            {
                NodaTime.Duration _days = NodaTime.Duration.FromTimeSpan(new TimeSpan(24, 0, 0));
                _approximateDuration = _days.MultipliedBy(approximateDuration.Days);
            }

            /// <summary>
            /// Gets the approximate duration.
            /// </summary>
            /// <returns>The approximate duration</returns>
            public Duration GetApproximateDuration()
            {
                return _approximateDuration;
            }

            /// <summary>
            /// Create an instance of BusinessDayTenor by provided ISO-8601 formatted string
            /// </summary>
            /// <param name="strTenor">A tenor string of ISO-8601 format</param>
            /// <returns>An instance of BusinessDayTenor</returns>
            public static BusinessDayTenor ValueOf(string strTenor)
            {
                DateTime dt = ParseIso8601(strTenor).UtcDateTime;
                return new BusinessDayTenor(NodaTime.Period.FromTicks(dt.Ticks));
            }

            private BusinessDayTenor() : base(NodaTime.Period.FromTicks(DateTime.UtcNow.Ticks))
            {

            }

            /// <summary>
            /// Create an instance of BusinessDayTenor by provided ISO-8601 formatted string
            /// </summary>
            /// <param name="strTenor">A tenor string of ISO-8601 format</param>
            /// <returns>An instance of BusinessDayTenor</returns>
            public static BusinessDayTenor From(NodaTime.Period period)
            {
                return new BusinessDayTenor(period);
            }
        }
    }
}
