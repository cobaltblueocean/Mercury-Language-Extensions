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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language;
using Mercury.Language.Exceptions;

namespace Mercury.Language.Time
{
    /// <summary>
    /// JapaneseEra Description
    /// </summary>
    public class JapaneseEra
    {
        public int Era { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public String LongName { get; private set; }
        public String ShortName { get; private set; }
        public String AlphabetName { get; private set; }

        public static List<JapaneseEra> Values
        {
            get
            {
                return _values;
            }
        }

        #region Private Members
        private static List<JapaneseEra> _values = new List<JapaneseEra>();
        public static Dictionary<int, string> eraTable = new Dictionary<int, string>();

        static JapaneseEra()
        {
            CultureInfo ci = new CultureInfo("ja-JP");
            ci.DateTimeFormat.Calendar = new JapaneseCalendar();
            DateTimeFormatInfo fi = ci.DateTimeFormat;

            for (char e = 'A'; e <= 'Z'; e++)
            {
                int eraIndex = fi.GetEra(e.ToString());
                if (eraIndex > 0)
                    eraTable.Add(eraIndex, e.ToString());
            }

            int[] Eras = fi.Calendar.Eras;

            foreach (int Era in Eras)
            {
                string EraShortName = fi.GetAbbreviatedEraName(Era);
                string EraLongName = fi.GetEraName(Era);
                string EraAlphabetName = eraTable[Era];
                DateTime startDate = EraStartDate(Era);
                DateTime endDate = EraEndDate(Era);

                _values.Add(new JapaneseEra(Era, EraLongName, EraShortName, EraAlphabetName, startDate, endDate));
            }
        }

        private JapaneseEra(int era, String longName, String shortName, String alphabetName, DateTime startDate, DateTime endDate)
        {
            Era = era;
            LongName = longName;
            ShortName = shortName;
            AlphabetName = alphabetName;
            StartDate = startDate;
            EndDate = endDate;
        }

        private static DateTime EraStartDate(int era)
        {
            switch(era)
            {
                case 1:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era1);
                case 2:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era2);
                case 3:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era3);
                case 4:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era4);
                case 5:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era5);
                case 6:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era6);
                case 7:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era7);
                case 8:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era8);
                case 9:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era9);
                case 10:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era10);
            }
            throw new ArgumentOutOfRangeException();
        }

        private static DateTime EraEndDate(int era)
        {
            switch (era)
            {
                case 1:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era2).AddDays(-1);
                case 2:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era3).AddDays(-1);
                case 3:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era4).AddDays(-1);
                case 4:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era5).AddDays(-1);
                case 5:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era6).AddDays(-1);
                case 6:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era7).AddDays(-1);
                case 7:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era8).AddDays(-1);
                case 8:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era9).AddDays(-1);
                case 9:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era10).AddDays(-1);
                case 10:
                    return DateTime.Parse(Mercury.Language.Extensions.Properties.JapaneseEra.Era10).AddDays(-1);
            }
            throw new ArgumentOutOfRangeException();
        }
        #endregion

        public Boolean IsInRange(DateTime date)
        {
            return ((StartDate <= date) && (date <= EndDate)) ? true : false;
        }

    }
}
