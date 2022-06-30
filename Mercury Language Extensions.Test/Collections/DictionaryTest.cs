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
using NUnit.Framework;
using Mercury.Test.Utility;

namespace Mercury_Language_Extensions.Test.Collections
{
    /// <summary>
    /// DictionaryTest Description
    /// </summary>
    public class DictionaryTest
    {
        [Test]
        public void DictionarySortTest()
        {
            var MAP = new Dictionary<Double?, Double?>();
            double x, y;
            for (int i = 0; i < 5; i++)
            {
                x = 2 * i;
                y = 3 * x;
                MAP.AddOrUpdate(x, y);
            }
            for (int i = 5, j = 0; i < 10; i++, j++)
            {
                x = 2 * j + 1;
                y = 3 * x;
                MAP.AddOrUpdate(x, y);
            }
            var MAP_SORTED = new TreeDictionary<Double?, Double?>();
            MAP_SORTED.AddOrUpdateAll(MAP);
            MAP_SORTED = (TreeDictionary<Double?, Double?>)MAP_SORTED.Sort();

            var MAP2 = new Dictionary<Double?, Double?>();
            MAP2.AddOrUpdateAll(MAP.Sort());

            Assert2.ArrayEquals(MAP.Keys.ToArray(), MAP_SORTED.Keys.ToArray());
            Assert2.ArrayEquals(MAP.Values.ToArray(), MAP_SORTED.Values.ToArray());
        }
    }
}
