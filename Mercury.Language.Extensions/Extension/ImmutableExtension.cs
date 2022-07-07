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
using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language;

namespace System.Collections.Immutable
{
    /// <summary>
    /// ImmutableExtension Description
    /// </summary>
    public static class ImmutableExtension
    {
        public static ImmutableHashSet<T> AddRange<T>(this ImmutableHashSet<T> immutableHashSet, ICollection<T> collection)
        {
            var hashSet = new HashSet<T>(immutableHashSet.ToList());
            foreach (var item in collection)
            {
                hashSet.Add(item);
            }

            return ImmutableHashSet.CreateRange<T>(hashSet);
        }

        public static HashSet<T> ToHashSet<T>(this ImmutableHashSet<T> immutableHashSet)
        {
            return new HashSet<T>(immutableHashSet.ToList());
        }

        public static ImmutableList<T> AddRange<T>(this ImmutableList<T> immutableList, ICollection<T> collection)
        {
            var list = new List<T>(immutableList.ToList());
            foreach (var item in collection)
            {
                list.Add(item);
            }

            return ImmutableList.CreateRange<T>(list);
        }

        public static ImmutableArray<T> AddRange<T>(this ImmutableArray<T> immutableList, ICollection<T> collection)
        {
            var list = new List<T>(immutableList.ToList());
            foreach (var item in collection)
            {
                list.Add(item);
            }

            return ImmutableArray.CreateRange<T>(list);
        }

        public static ImmutableDictionary<T, V> AddRange<T, V>(this ImmutableDictionary<T, V> immutableDictionary, ICollection<T> keys, ICollection<V> values)
        {
            if (keys.Count != values.Count)
                throw new ArgumentException(LocalizedResources.Instance().NUMBERS_OF_KEYS_AND_VALUE_NOT_MATCH);

            var originalKeys = new List<T>(immutableDictionary.Keys);
            var originalValues = new List<V>(immutableDictionary.Values);

            originalKeys.AddRange(keys);
            originalValues.AddRange(values);

            var dict = new Dictionary<T, V>();
            
            for (int i = 0; i<originalKeys.Count; i++)
            {
                dict.Add(originalKeys[i], originalValues[i]);
            }

            return ImmutableDictionary.CreateRange<T, V>(dict);
        }
    }
}
