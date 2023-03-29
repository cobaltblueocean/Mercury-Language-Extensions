// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Utility Inc.
//
// Copyright (C) 2012 - present by System.Utility Inc. and the System.Utility group of companies
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
using System.Threading;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;

namespace System
{
    /// <summary>
    /// LinkedHashDictionary Description
    /// </summary>
    public class LinkedHashDictionary<T, U> : IDictionary<T, U>
    {
        Dictionary<T, LinkedListNode<Tuple<U, T>>> D = new Dictionary<T, LinkedListNode<Tuple<U, T>>>();
        LinkedList<Tuple<U, T>> LL = new LinkedList<Tuple<U, T>>();

        public U this[T c]
        {
            get
            {
                return D[c].Value.Item1;
            }

            set
            {
                if (D.ContainsKey(c))
                {
                    LL.Remove(D[c]);
                }

                D[c] = new LinkedListNode<Tuple<U, T>>(Tuple.Create(value, c));
                LL.AddLast(D[c]);
            }
        }

        public bool ContainsKey(T k)
        {
            return D.ContainsKey(k);
        }

        public U PopFirst()
        {
            var node = LL.First;
            LL.Remove(node);
            D.Remove(node.Value.Item2);
            return node.Value.Item1;
        }

        public void Add(T key, U value)
        {
            D[key] = new LinkedListNode<Tuple<U, T>>(Tuple.Create(value, key));
            LL.AddLast(D[key]);
        }

        public bool Remove(T key)
        {
            if (D.ContainsKey(key))
            {
                LL.Remove(D[key]);
                D.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(T key, out U value)
        {
            if (D.ContainsKey(key))
            {
                value = D[key].Value.Item1;
                return true;
            }
            else
            {
                value = default(U);
                return false;
            }
        }

        public void Add(KeyValuePair<T, U> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            D.Clear();
            LL.Clear();
        }

        public bool Contains(KeyValuePair<T, U> item)
        {
            var l = Tuple.Create(item.Value, item.Key);
            if (D.ContainsKey(item.Key) && LL.Contains(l))
            {
                return true;
            }else
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
        {
            int j = 0;
            for (int i = arrayIndex; i < array.Length; i++)
            {
                array[i] = new KeyValuePair<T, U>(LL.ElementAt(j).Item2, LL.ElementAt(j).Item1);
                j++;

                // If reach to end of LL, stop
                if (j == LL.Count)
                {
                    break;
                }
            }
        }

        public bool Remove(KeyValuePair<T, U> item)
        {
            var l = Tuple.Create(item.Value, item.Key);
            if (D.ContainsKey(item.Key) && LL.Contains(l))
            {
                LL.Remove(D[item.Key]);
                D.Remove(item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<KeyValuePair<T, U>> GetEnumerator()
        {
           return (IEnumerator<KeyValuePair<T, U>>)LL.Select(l => new KeyValuePair<T, U>(l.Item2, l.Item1));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get
            {
                return D.Count;
            }
        }

        public T[] Keys
        {
            get {
                var t = LL.Select(e => e.Item2);
                var List = new T[t.Count()];
                t.ToList().CopyTo(List);
                //return LL.Select(e => e.Item2).ToArray();
                return List;  
            }
        }

        public U[] Values
        {
            get
            {
                var t = LL.Select(e => e.Item1);
                var List = new U[t.Count()];
                t.ToList().CopyTo(List);
                //return LL.Select(e => e.Item1).ToArray();
                return List;
            }
        }

        public LinkedList<Tuple<U, T>> LinkedList
        {
            get { return LL; }
        }

        ICollection<T> IDictionary<T, U>.Keys => (ICollection<T>)LL.Select(l => l.Item2);

        ICollection<U> IDictionary<T, U>.Values => (ICollection<U>)LL.Select(l => l.Item1);

        public bool IsReadOnly => false;
    }
}
