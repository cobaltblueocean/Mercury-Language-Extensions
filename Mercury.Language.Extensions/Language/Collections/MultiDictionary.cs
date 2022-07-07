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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// MultiDictionary Description
    /// </summary>
    public class MultiDictionary<K, V> : Dictionary<K, List<V>>
    {
        public new int Count => base.Values.Sum(x => x.Count);

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(K key, V value)
        {
            if (base.ContainsKey(key))
            {
                base[key].Add(value);
            }
            else
            {
                base.Add(key, new List<V>() { value });
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            if (base.ContainsKey(item.Key))
            {
                return base[item.Key].Contains(item.Value);
            }
            else
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            if (base.ContainsKey(item.Key))
            {
                return base[item.Key].Remove(item.Value);
            }
            else
            {
                return false;
            }
        }

        public Dictionary<K, List<V>> ToCollectionDictionary()
        {
            var result = new Dictionary<K, List<V>>();
            var enumerator = base.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                result.Add(item.Key, item.Value);
            }

            return result;
        }
    }
}
