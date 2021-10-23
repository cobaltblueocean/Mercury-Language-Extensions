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
using System.Text;
using System.Threading.Tasks;
using OpenGamma.Utility;
using Mercury.Language.Exception;
using Mercury.Language.Time;
using NodaTime;
using Range = Mercury.Language.Math.Ranges.Range;

namespace Mercury.Language.Time
{
    /// <summary>
    /// Temporal Description
    /// </summary>
    public class Temporal
    {
        public enum ValueType
        {
            ZonedDateTime,
            LocalDate
        }

        private ZonedDateTime _zonedDateTime;
        private LocalDate _localDate;

        private ValueType _valueType;

        public ValueType Type{ get; private set; }

        public Temporal(ZonedDateTime zonedDateTime)
        {
            _valueType = ValueType.ZonedDateTime;
            _zonedDateTime = zonedDateTime;
        }

        public Temporal(LocalDate localDate)
        {
            _valueType = ValueType.LocalDate;
            _localDate = localDate;
        }

        public dynamic GetOriginal()
        {
            if (_valueType == ValueType.LocalDate)
                return _localDate;
            else
                return _zonedDateTime;
        }

        public int CompareTo(Temporal target)
        {
            if (this.Type == target.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    return date.CompareTo(target.GetOriginal());
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = target.GetOriginal();
                    return date.CompareTo(other);
                }
            }
            throw new InvalidCastException();
        }

        public Boolean IsAfter(Temporal target)
        {
            if (this.Type == target.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = target.GetOriginal();
                    return date.IsAfter(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = target.GetOriginal();
                    return date.IsAfter(other);
                }
            }
            throw new InvalidCastException();
        }

        public Boolean IsBefore(Temporal target)
        {
            if (this.Type == target.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = target.GetOriginal();
                    return date.IsBefore(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = target.GetOriginal();
                    return date.IsBefore(other);
                }
            }
            throw new InvalidCastException();
        }

        public Boolean IsLeapYear()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.IsLeapYear();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.IsLeapYear();
            }
            throw new InvalidCastException();
        }

        public Boolean IsSupported(ITemporalField field)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.IsSupported(field);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.IsSupported(field);
            }
            throw new InvalidCastException();
        }

        public Boolean IsSupported(ChronoUnit unit)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.IsSupported(unit);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.IsSupported(unit);
            }
            throw new InvalidCastException();
        }

        public dynamic AdjustDayOfWeek(IsoDayOfWeek dayOfWeek)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.AdjustDayOfWeek(dayOfWeek);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.AdjustDayOfWeek(dayOfWeek);
            }
            throw new InvalidCastException();
        }

        public dynamic PlusDays(int days)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.PlusDays(days);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.PlusDays(days);
            }
            throw new InvalidCastException();
        }

        public dynamic PlusMonths(int months)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.PlusMonths(months);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.PlusMonths(months);
            }
            throw new InvalidCastException();
        }

        public dynamic PlusYears(int years)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.PlusYears(years);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.PlusYears(years);
            }
            throw new InvalidCastException();
        }

        public dynamic With(ITemporalAdjuster tmp)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.With(tmp);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.With(tmp);
            }
            throw new InvalidCastException();
        }

        public dynamic Plus(Period period)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.Plus(period);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.Plus(period);
            }
            throw new InvalidCastException();
        }

        public dynamic Plus(long amountToAdd, ChronoUnit unit)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.Plus(amountToAdd, unit);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.Plus(amountToAdd, unit);
            }
            throw new InvalidCastException();
        }

        public dynamic Minus(Period period)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.Minus(period);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.Minus(period);
            }
            throw new InvalidCastException();
        }

        public dynamic Minus(long amountToAdd, ChronoUnit unit)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.Minus(amountToAdd, unit);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.Minus(amountToAdd, unit);
            }
            throw new InvalidCastException();
        }

        public dynamic With(MonthDay monthDay)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.With(monthDay);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.With(monthDay);
            }
            throw new InvalidCastException();
        }

        public dynamic With(LocalDate localDate)
        {
            if (this.Type == ValueType.LocalDate)
            {
                return localDate;
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.With(localDate);
            }
            throw new InvalidCastException();
        }

        public dynamic With(ITemporalField field, long newValue)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.With(field, newValue);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.With(field, newValue);
            }
            throw new InvalidCastException();
        }

        public dynamic WithYear(int year)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.WithYear(year);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.WithYear(year);
            }
            throw new InvalidCastException();
        }

        public dynamic WithMonth(int month)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.WithMonth(month);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.WithMonth(month);
            }
            throw new InvalidCastException();
        }

        public dynamic WithDayOfMonth(int dayOfMonth)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.WithDayOfMonth(dayOfMonth);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.WithDayOfMonth(dayOfMonth);
            }
            throw new InvalidCastException();
        }

        public dynamic OfYearDay(int year, int dayOfYear)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.OfYearDay(year, dayOfYear);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.OfYearDay(year, dayOfYear);
            }
            throw new InvalidCastException();
        }

        public dynamic OfEpochDay(long epochDay)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.OfEpochDay(epochDay);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.OfEpochDay(epochDay);
            }
            throw new InvalidCastException();
        }

        public int Get(ITemporalField field)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return (int)date.Get(field);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.Get(field);
            }
            throw new InvalidCastException();
        }

        public long GetLong(ITemporalField field)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.GetLong(field);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.GetLong(field);
            }
            throw new InvalidCastException();
        }

        public long GetProlepticMonth()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.GetProlepticMonth();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.GetProlepticMonth();
            }
            throw new InvalidCastException();
        }

        public long ToEpochDay()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.ToEpochDay();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.ToEpochDay();
            }
            throw new InvalidCastException();
        }

        public int LengthOfMonth()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.LengthOfMonth();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.LengthOfMonth();
            }
            throw new InvalidCastException();
        }

        public int LengthOfYear()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.LengthOfYear();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.LengthOfYear();
            }
            throw new InvalidCastException();
        }

        public int GetMinYear()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.GetMinYear();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.GetMinYear();
            }
            throw new InvalidCastException();
        }

        public int GetMaxYear()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.GetMaxYear();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.GetMaxYear();
            }
            throw new InvalidCastException();
        }

        public double GetDifferenceInDays(Temporal endDate)
        {
            if (this.Type == endDate.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = endDate.GetOriginal();
                    return date.GetDifferenceInDays(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = endDate.GetOriginal();
                    return date.GetDifferenceInDays(other);
                }
            }
            throw new InvalidCastException();
        }

        public double GetDifferenceInMonths(Temporal endDate)
        {
            if (this.Type == endDate.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = endDate.GetOriginal();
                    return date.GetDifferenceInMonths(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = endDate.GetOriginal();
                    return date.GetDifferenceInMonths(other);
                }
            }
            throw new InvalidCastException();
        }

        public double GetDifferenceInYears(Temporal endDate)
        {
            if (this.Type == endDate.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = endDate.GetOriginal();
                    return date.GetDifferenceInYears(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = endDate.GetOriginal();
                    return date.GetDifferenceInYears(other);
                }
            }
            throw new InvalidCastException();
        }

        public long GetEpochSecond()
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.GetEpochSecond();
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.GetEpochSecond();
            }
            throw new InvalidCastException();
        }

        public double GetExactDaysBetween(Temporal endDate)
        {
            if (this.Type == endDate.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = endDate.GetOriginal();
                    return date.GetExactDaysBetween(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = endDate.GetOriginal();
                    return date.GetExactDaysBetween(other);
                }
            }
            throw new InvalidCastException();
        }

        public int GetDaysBetween(Temporal endDate)
        {
            if (this.Type == endDate.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = endDate.GetOriginal();
                    return date.GetDaysBetween(other);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = endDate.GetOriginal();
                    return date.GetDaysBetween(other);
                }
            }
            throw new InvalidCastException();
        }

        public int GetDaysBetween(Boolean includeStart, Temporal endDate, Boolean includeEnd)
        {
            if (this.Type == endDate.Type)
            {
                if (this.Type == ValueType.LocalDate)
                {
                    LocalDate date = this.GetOriginal();
                    LocalDate other = endDate.GetOriginal();
                    return date.GetDaysBetween(includeStart, other, includeEnd);
                }
                else if (this.Type == ValueType.ZonedDateTime)
                {
                    ZonedDateTime date = this.GetOriginal();
                    ZonedDateTime other = endDate.GetOriginal();
                    return date.GetDaysBetween(includeStart, other, includeEnd);
                }
            }
            throw new InvalidCastException();
        }

        public Range GetRange(ITemporalField field)
        {
            if (this.Type == ValueType.LocalDate)
            {
                LocalDate date = this.GetOriginal();
                return date.GetRange(field);
            }
            else if (this.Type == ValueType.ZonedDateTime)
            {
                ZonedDateTime date = this.GetOriginal();
                return date.GetRange(field);
            }
            throw new InvalidCastException();

        }
    }
}
