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
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// LinkedListMultiMap Description
    /// </summary>
    public class LinkedListMultiDictionary<K, V> : Dictionary<K, List<V>> where V : ITuple
    {
        public bool IsReadOnly => false;

        public new List<V> this [K key]
        {
            get { return base[key]; }
            set {
                if (base.ContainsKey(key))
                {
                    base[key].AddRange(value);
                }
                else
                {
                    base.Add(key, value);
                }
            }
        }

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

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            if (base.ContainsKey(item.Key))
            {
                if(base[item.Key].Contains(item.Value))
                {
                    base[item.Key].Remove(item.Value);
                    if (!base[item.Key].Any())
                    {
                        base.Remove(item.Key);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public new bool TryGetValue(K key, out List<V> value)
        {
            try
            {
                if (base.ContainsKey(key))
                {
                    value = base[key];
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            catch
            {
                value = null;
                return false;
            }
        }
    }
}
