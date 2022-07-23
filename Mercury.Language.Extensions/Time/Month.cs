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
using Mercury.Language.Exception;
using NodaTime;

namespace Mercury.Language.Time
{
    /// <summary>
    /// Month Description
    /// </summary>
    public class Month
    {
        #region enum members
        /// <summary>
        /// Unit that represents the concept of a nanosecond, the smallest supported unit of time.
        /// For the ISO calendar system, it is equal to the 1,000,000,000th part of the second unit.
        /// </summary>
        public static readonly Month JANUARY = new Month("January", 1);
        public static readonly Month FEBRUARY = new Month("February", 2);
        public static readonly Month MARCH = new Month("March", 3);
        public static readonly Month APRIL = new Month("April", 4);
        public static readonly Month MAY = new Month("May", 5);
        public static readonly Month JUNE = new Month("June", 6);
        public static readonly Month JULY = new Month("July", 7);
        public static readonly Month AUGUST = new Month("August", 8);
        public static readonly Month SEPTEMBER = new Month("September", 9);
        public static readonly Month OCTOBER = new Month("October", 10);
        public static readonly Month NOVEMBER = new Month("Nobember", 11);
        public static readonly Month DECEMBER = new Month("December", 12);
        #endregion

        public string Name { get; private set; }
        public int Value { get; private set; }

        public IEnumerable<Month> Values {
            get {
                yield return JANUARY;
                yield return FEBRUARY;
                yield return MARCH;
                yield return APRIL;
                yield return MAY;
                yield return JUNE;
                yield return JULY;
                yield return AUGUST;
                yield return SEPTEMBER;
                yield return OCTOBER;
                yield return NOVEMBER;
                yield return DECEMBER;
            }
        }

        private Month(string name, int monthValue)
        {
            this.Name = name;
            this.Value = monthValue;
        }

        public static Month Of(int month)
        {
            if ((0 < month) && (month < 12))
            {
                switch (month)
                {
                    case 1:
                        return JANUARY;
                    case 2:
                        return FEBRUARY;
                    case 3:
                        return MARCH;
                    case 4:
                        return APRIL;
                    case 5:
                        return MAY;
                    case 6:
                        return JUNE;
                    case 7:
                        return JULY;
                    case 8:
                        return AUGUST;
                    case 9:
                        return SEPTEMBER;
                    case 10:
                        return OCTOBER;
                    case 11:
                        return NOVEMBER;
                    case 12:
                        return DECEMBER;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public int Ordinal()
        {
            return Value - 1;
        }

        public Month Plus(long months)
        {
            int amount = (int)(months % 12);
            return Of((Value + (amount + 12)) % 12);
        }

        public Month Minus(long months)
        {
            return Plus(-(months % 12));
        }

        public Boolean InRange(int day)
        {
            LocalDate now = LocalDate.FromDateTime(DateTime.UtcNow);
            return InRange(day, now.Year);
        }

        public Boolean InRange(int day, int year)
        {
            if (day < 1)
                return false;

            LocalDate localDate = new LocalDate(year, this.Value, 1);

            return (day < 1) ?
                false
                : (day <= localDate.LengthOfMonth()) ?
                true :
                false;
        }

        /**
     * Gets the Length of this month in days.
     * <p>
     * This takes a flag to determine whether to return the Length for a leap year or not.
     * <p>
     * February has 28 days in a standard year and 29 days in a leap year.
     * April, June, September and November have 30 days.
     * All other months have 31 days.
     *
     * @param leapYear  true if the Length is required for a leap year
     * @return the Length of this month in days, from 28 to 31
     */
        public int Length(Boolean leapYear)
        {
            if (this == FEBRUARY)
            {
                return (leapYear ? 29 : 28);
            }else if ((this == APRIL) || (this == JUNE) || (this == SEPTEMBER) || (this == NOVEMBER) )
            {
                return 30;
            }
            else
            {
                return 31;
            }
        }

        /**
     * Gets the day-of-year corresponding to the first day of this month.
     * <p>
     * This returns the day-of-year that this month begins on, using the leap
     * year flag to determine the Length of February.
     *
     * @param leapYear  true if the Length is required for a leap year
     * @return the day of year corresponding to the first day of this month, from 1 to 336
     */
        public int firstDayOfYear(Boolean leapYear)
        {
            int leap = leapYear ? 1 : 0;

            if (this == JANUARY)
            {
                return 1;
            }
            else if (this == FEBRUARY)
            {
                return 32;
            }
            else if (this == MARCH)
            {
                return 60 + leap;
            }
            else if (this == APRIL)
            {
                return 91 + leap;
            }
            else if (this == MAY)
            {
                return 121 + leap;
            }
            else if (this == JUNE)
            {
                return 152 + leap;
            }
            else if (this == JULY)
            {
                return 182 + leap;
            }
            else if (this == AUGUST)
            {
                return 213 + leap;
            }
            else if (this == SEPTEMBER)
            {
                return 244 + leap;
            }
            else if (this == OCTOBER)
            {
                return 274 + leap;
            }
            else if (this == NOVEMBER)
            {
                return 305 + leap;
            }
            else
            {
                return 335 + leap;
            }
        }

        /**
         * Gets the month corresponding to the first month of this quarter.
         * <p>
         * The year can be divided into four quarters.
         * This method returns the first month of the quarter for the base month.
         * January, February and March return January.
         * April, May and June return April.
         * July, August and September return July.
         * October, November and December return October.
         *
         * @return the first month of the quarter corresponding to this month, not null
         */
        public Month firstMonthOfQuarter()
        {
            return Values.ToArray<Month>()[(Ordinal() / 3) * 3];
        }
    }
}
