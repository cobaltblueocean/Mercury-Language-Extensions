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

namespace Mercury.Language.Extensions.Test.Collections
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

        [Test]
        public void DictionarySafeGetTest()
        {
            int N = 16;
            Dictionary<String, Double?> dummy = new Dictionary<String, Double?>();

            AutoParallel.AutoParallelFor(0, N, (i) =>
            {
                dummy.Add(i.ToString(), i);
            });

            String key = (N * 2).ToString();

            Assert2.ThrowsException<KeyNotFoundException>(() =>
            {
                var data1 = dummy[key];
            });

            var data2 = dummy.GetSafe(key);

            Assert.IsTrue(data2 == null);
        }

        [Test]
        public void LinkedDictionaryTest()
        {
            IDictionary<Tuple<Double?, Double?>, Double?> XYZ_MAP = new LinkedDictionary<Tuple<Double?, Double?>, Double?>();
            int n = 10;

            for (int i = 0; i < n; i++)
            {
                double x = i < 5 ? i : i - 5;
                double y = i < 5 ? 0 : 1;
                double z = 4 * x;
                Tuple<Double?, Double?> xy = new Tuple<Double?, Double?>(x, y);
                XYZ_MAP.AddOrUpdate(xy, z);
            }

            XYZ_MAP = XYZ_MAP.Sort();
        }

        [Test]
        public void TreeDictionaryTest()
        {
            TreeDictionary<String, DummyObject> treeDictionary1 = new TreeDictionary<string, DummyObject>();
            TreeDictionary<String, DummyObjectComparable> treeDictionary2 = new TreeDictionary<string, DummyObjectComparable>();

            treeDictionary1.AddOrUpdate("Code1", new DummyObject() { Code = "Code1" });
            treeDictionary1.AddOrUpdate("Code2", new DummyObject() { Code = "Code2" });

            treeDictionary2.AddOrUpdate("Code1", new DummyObjectComparable() { Code = "Code1" });
            treeDictionary2.AddOrUpdate("Code2", new DummyObjectComparable() { Code = "Code2" });
        }
    }
}
