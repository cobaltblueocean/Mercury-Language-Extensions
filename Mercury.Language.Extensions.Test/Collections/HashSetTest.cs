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

namespace Mercury.Language.Extensions.Test.Collections
{
    /// <summary>
    /// HashSetTest Description
    /// </summary>
    public class HashSetTest
    {
        [Test]
        public void HashSetSortTest()
        {
            var PAIR_SET = new HashSet<Tuple<Double?, Double?>>();
            var PAIR_SET_SORTED = new HashSet<Tuple<Double?, Double?>>();
            var SORTED_SET = new SortedSet<Tuple<Double?, Double?>>();
            double x, y;
            for (int i = 0; i < 5; i++)
            {
                x = 2 * i;
                y = 3 * x;
                PAIR_SET.Add(new Tuple<Double?, Double?>(x, y));
                SORTED_SET.Add(new Tuple<Double?, Double?>(x, y));
            }
            for (int i = 5, j = 0; i < 10; i++, j++)
            {
                x = 2 * j + 1;
                y = 3 * x;
                PAIR_SET.Add(new Tuple<Double?, Double?>(x, y));
                SORTED_SET.Add(new Tuple<Double?, Double?>(x, y));
            }
            PAIR_SET_SORTED = new HashSet<Tuple<Double?, Double?>>();
            PAIR_SET_SORTED.AddAll(PAIR_SET);
            PAIR_SET_SORTED = (HashSet<Tuple<Double?, Double?>>)PAIR_SET_SORTED.Sort();

            for (int i = 0; i < PAIR_SET.Count; i++)
            {
                Assert.AreEqual(PAIR_SET_SORTED.ElementAt(i), SORTED_SET.ElementAt(i));
            }
        }
    }
}
