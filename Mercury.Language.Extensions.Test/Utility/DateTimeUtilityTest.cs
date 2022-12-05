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
using NUnit.Framework;
using System;
using System.Globalization;
using Mercury.Language.Time;

namespace Mercury.Language.Extensions.Test.Utility
{
    /// <summary>
    /// DateTimeUtilityTest Description
    /// </summary>
    public class DateTimeUtilityTest
    {
        [Test]
        public void CreateDateTimeTest()
        {
            var dateString1 = "20150817";
            var dateString2 = "8/17/2015";
            var format = "yyyyMMdd";
            var date = DateTime.ParseExact(dateString1, format, CultureInfo.InvariantCulture);

            var ldt1 = DateTimeUtility.ToLocalDate(dateString1, format);
            var ldt2 = DateTimeUtility.ToLocalDate(dateString2);

            Assert.IsTrue(date.Year == ldt1.Year);
            Assert.IsTrue(date.Month == ldt1.Month);
            Assert.IsTrue(date.Day == ldt1.Day);

            Assert.IsTrue(ldt1.Year == ldt2.Year);
            Assert.IsTrue(ldt1.Month == ldt2.Month);
            Assert.IsTrue(ldt1.Day == ldt2.Day);
        }
    }
}
