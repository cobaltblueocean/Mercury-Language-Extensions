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
using Mercury.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.TimeZones;
using Microsoft.VisualBasic;
using System.Xml.Linq;

namespace NodaTime
{
    /// <summary>
    /// Represent of UnspecifiedDateTimeZone
    /// </summary>
    public sealed class UnspecifiedDateTimeZone : DateTimeZone
    {
        public UnspecifiedDateTimeZone() : base("Unspecified", true, Offset.FromSeconds(0), Offset.FromSeconds(0))
        {
        }

        public override ZoneInterval GetZoneInterval(Instant instant)
        {
            return new ZoneInterval("Unspecified", null, null, Offset.FromSeconds(0), Offset.FromSeconds(0));
        }
    }
}
