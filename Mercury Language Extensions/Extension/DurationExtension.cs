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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mercury.Language.Exceptions;
using System.Time;
using NodaTime;

namespace Mercury.Language.Time
{
    /// <summary>
    /// DurationExtension Description
    /// </summary>
    public static class DurationExtension
    {
        public static Duration MultipliedBy(this Duration duration, long multiplicand)
        {
            if (multiplicand == 0)
            {
                return new Duration(new TimeSpan(0));
            }

            if (multiplicand == 1)
            {
                return duration;
            }
            return new Duration(new TimeSpan(duration.TimeSpan.Ticks * multiplicand));
        }

        public static Duration MultipliedBy(this Duration duration, double multiplicand)
        {

            return MultipliedBy(duration, (long)multiplicand);
        }

        public static Duration EstimatedDuration(this Duration duration)
        {
            Period period = new Period(duration.TimeSpan);
            Duration monthsDuration = new Duration(Period.MONTHS.MultipliedBy(period.TotalMonths).TimeSpan);
            Duration daysDuration = Period.DAYS.MultipliedBy(period.Days);
            return monthsDuration.Add(daysDuration);
        }

        public static int CompareTo(this Duration duration, Duration compareing)
        {
            if (duration.Equals(compareing)) return 0;
            if (duration.TimeSpan.Ticks > compareing.TimeSpan.Ticks) return -1;
            return 1;
        }
    }
}
