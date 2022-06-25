// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
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
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ListExtension
    {
        public static Boolean IsEmpty<T>(this List<T> originalList)
        {
            return originalList.Count == 0 ? true : false;
        }

        public static Boolean ContainsAll<T>(this IEnumerable<T> originalList, IEnumerable<T> ts)
        {
            return !ts.Except(originalList).Any();
        }

        public static Complex[] ConvertToComplex(this List<double> real)
        {
            Complex[] c = new Complex[real.Count];
            for (int i = 0; i < real.Count; i++)
            {
                c[i] = new Complex(real[i], 0);
            }

            return c;
        }

        public static ISet<T> AddAll<T>(this ISet<T> originalList, params T[] items)
        {
            foreach(T item in items)
            {
                originalList.Add(item);
            }

            return originalList;
        }

        public static ISet<T> AddAll<T>(this ISet<T> originalList, ISet<T> items)
        {
            foreach (T item in items)
            {
                originalList.Add(item);
            }

            return originalList;
        }

        public static IList<T> AddAll<T>(this IList<T> originalList, params T[] items)
        {
            if (items.Length == 0)
                return originalList;

            if (originalList.GetType().IsArray)
            {
                if (originalList.Count == 0)
                    return items;

                var newLength = originalList.Count + items.Length;
                var newArray = new T[newLength];
                originalList.CopyTo(newArray, 0);
                items.CopyTo(newArray, originalList.Count);

                return newArray;
            }
            else
            {
                foreach (T item in items)
                {
                    originalList.Add(item);
                }
                return originalList;
            }
        }

        public static IList<T> AddAll<T>(this IList<T> originalList, IList<T> items)
        {
            if (items.Count == 0)
                return originalList;

            if (originalList.GetType().IsArray)
            {
                if (originalList.Count == 0)
                    return items;

                var newLength = originalList.Count + items.Count;
                var newArray = new T[newLength];
                originalList.CopyTo(newArray, 0);
                items.CopyTo(newArray, originalList.Count);

                return newArray;
            }
            else
            {
                foreach (T item in items)
                {
                    originalList.Add(item);
                }
                return originalList;
            }
        }

        public static T1[] GetKeys<T1, T2>(this List<KeyValuePair<T1, T2>> list)
        {
            T1[] tmp = new T1[list.Count];

            for (int i = 0; i<list.Count; i++)
            {
                tmp[i] = list[i].Key;
            }

            return tmp;
        }

        public static T2[] GetValues<T1, T2>(this List<KeyValuePair<T1, T2>> list)
        {
            T2[] tmp = new T2[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                tmp[i] = list[i].Value;
            }

            return tmp;
        }

        public static T[] ToPremitiveArray<T>(this IList<Nullable<T>> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Where(x => x.HasValue).Cast<T>().ToArray();
        }
        public static T[] ToPremitiveArray<T>(this IList<Nullable<T>> val, int length) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Take(length).Cast<T>().ToArray();
        }

        public static T[] ToPremitiveArrayWithDefaultIfNull<T>(this IList<Nullable<T>> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return ToPremitiveArrayWithDefaultIfNull(val, default(T));
        }

        public static T[] ToPremitiveArrayWithDefaultIfNull<T>(this IList<Nullable<T>> val, T value) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            var r = new List<T>();
            AutoParallel.AutoParallelFor(0, val.Count, (i) => {
                if (!val[i].HasValue)
                    r.Add(value);
                else
                    r.Add(val[i].Value);
            });
            return r.ToArray();
        }

        public static Nullable<T>[] ToNullableArray<T>(this IList<T> val) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.ToArray().Cast<Nullable<T>>().ToArray();
        }

        public static Nullable<T>[] ToNullableArray<T>(this IList<T> val, int length) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return val.Take(length).ToArray().Cast<Nullable<T>>().ToArray();
        }

        public static Boolean InsertValueAtKey<T1, T2>(this List<KeyValuePair<T1, T2>> list, T1 key, T2 value)
        {
            int search = Array.BinarySearch(list.GetKeys(), 0, list.Count, key);
            if (search >= 0)
            {
                list[search] = new KeyValuePair<T1, T2>(key, value);
                return true;
            }
            else
            {
                list.Add(new KeyValuePair<T1, T2>(key, value));
                return true;
            }
        }

        public static Boolean ValuesEqual<T>(this IList<T> arrayA, IList<T> arrayB)
        {
            if (arrayA.Count == arrayB.Count)
            {
                if (typeof(T).IsPrimitive)
                {
                    for (int i = 0; i < arrayA.Count; i++)
                    {
                        if (!arrayA[i].Equals(arrayB[i]))
                            return false;
                    }
                    return true;
                }
                else
                {
                    for (int i = 0; i < arrayA.Count; i++)
                    {
                        if (!arrayA[i].AreObjectsEqual(arrayB[i]))
                            return false;
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
