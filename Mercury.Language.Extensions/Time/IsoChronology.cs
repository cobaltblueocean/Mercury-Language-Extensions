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

namespace Mercury.Language.Time
{
    /// <summary>
    /// IsoChronology Description
    /// </summary>
    public sealed class IsoChronology
    {
        /**
     * Singleton instance of the ISO chronology.
     */
        private static IsoChronology INSTANCE = new IsoChronology();

        /**
     * Restricted constructor.
     */
        private IsoChronology()
        {
        }

        public String Id
        {
            get {return "ISO"; }
        }

        public String CalendarType
        {
            get { return "iso8601"; }
        }

        public static IsoChronology GetInstance()
        {
            return INSTANCE;
        }

        public Boolean IsLeapYear(long prolepticYear)
        {
            return ((prolepticYear & 3) == 0) && ((prolepticYear % 100) != 0 || (prolepticYear % 400) == 0);
        }

    }
}
