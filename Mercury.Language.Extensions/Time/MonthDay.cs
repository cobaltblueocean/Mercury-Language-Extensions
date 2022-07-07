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
using OpenGamma.Utility;
using Mercury.Language.Exception;
using NodaTime;

namespace Mercury.Language.Time
{
    /// <summary>
    /// MonthDay Description
    /// </summary>
    public sealed class MonthDay
    {
        /// <summary>
        /// The month-of-year, not null.
        /// </summary>
        private Month _month;
        /// <summary>
        /// The day-of-month.
        /// </summary>
        private int _day;

        public Month Month
        {
            get { return _month; }
        }

        public int Day
        {
            get { return _day; }
        }

        private MonthDay()
        {
            ZonedDateTime now = new ZonedDateTime();

            _month = Month.Of(now.Month);
            _day = now.Day;
        }

        private MonthDay(int month, int day)
        {
            _month = Month.Of(month);
            if ((0 < day) && (day < 13))
                _day = day;
            else
                throw new ArgumentOutOfRangeException();
        }

        public static MonthDay Now()
        {
            return new MonthDay();
        }

        public static MonthDay Of(int month, int day)
        {
            return new MonthDay(month, day);
        }

        public static MonthDay Of(Month month, int day)
        {
            return new MonthDay(month.Value, day);
        }

        public static MonthDay From(LocalDate localDate)
        {
            return new MonthDay(localDate.Month, localDate.Day);
        }

        public static MonthDay From(ZonedDateTime localDate)
        {
            return new MonthDay(localDate.Month, localDate.Day);
        }

        public static MonthDay From(DateTime localDate)
        {
            return new MonthDay(localDate.Month, localDate.Day);
        }

    }
}
