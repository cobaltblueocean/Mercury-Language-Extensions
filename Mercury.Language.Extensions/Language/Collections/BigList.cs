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
using Mercury.Language.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// BigList Description
    /// </summary>
    public class BigList<T> : IBigList<T>
    {
        private List<List<T>> mInternalLists;
        private int mPartitionSize = int.MaxValue;
        //private long mSize;
        [NonSerialized]
        private Object _syncRoot;

        public T this[long index]
        {
            get {
                var pIndex = GetPartitionedIndex(index);
                return GetValue(pIndex.Item1, pIndex.Item2);
            }
            set {
                var pIndex = GetPartitionedIndex(index);
                mInternalLists[pIndex.Item1][pIndex.Item2] = value;
            }
        }

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public long Count
        {
            get
            {
                return ConvertLongIndex(GetPartitionedSize());
            }
        }


        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }

        public bool IsSynchronized => false;

        private Tuple<int, int> GetPartitionedIndex(long index)
        {
            int partition = (int)System.Math.Floor((double)index / (double)mPartitionSize);
            int rindex = (int)(index - (partition * mPartitionSize));

            return new Tuple<int, int>(partition, rindex);
        }

        private T GetValue(int partition, int rindex)
        {
            return mInternalLists[partition][rindex];
        }

        private Tuple<int, int> GetPartitionedSize()
        {
            if (mInternalLists.Count > 0)
            {
                int partition = mInternalLists.Count - 1;
                int rindex = (mInternalLists[partition]).Count;
                return new Tuple<int, int>(partition, rindex);
            }
            else
            {
                return new Tuple<int, int>(0, 0);
            }
        }

        private long ConvertLongIndex(Tuple<int, int> pIndex)
        {
            return ConvertLongIndex(pIndex.Item1, pIndex.Item2);
        }

        private long ConvertLongIndex(int partition, int rindex)
        {
            return (long)(((long)partition * (long)mPartitionSize) + (long)rindex);
        }

        public BigList()
        {
            mInternalLists = new List<List<T>>();
            mInternalLists.Add(new List<T>());
        }

        public long Add(T value)
        {
            var pIndex = GetPartitionedSize();
            int partition = pIndex.Item1;
            int rindex = pIndex.Item2;

            if (rindex == int.MaxValue)
            {
                mInternalLists.Add(new List<T>());
                partition++;
                rindex = 0;
            }

            mInternalLists[partition].Add(value);

            return ConvertLongIndex(partition, rindex);
        }

        public void Clear()
        {
            mInternalLists = new List<List<T>>();
            mInternalLists.Add(new List<T>());
        }

        public bool Contains(T value)
        {
            return PartitionIndexOf(value).Item2 >= 0;
        }

        public void CopyTo(Array array, long index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        public long IndexOf(T value)
        {
            return ConvertLongIndex(PartitionIndexOf(value));
        }

        private Tuple<int, int> PartitionIndexOf(T value)
        {
            int partition = -1;
            int rindex = -1;
            var pIndex = GetPartitionedSize();
            int partitionMax = pIndex.Item1;
            int rindexMax = pIndex.Item2;

            for (int i = 0; i < partitionMax; i++)
            {
                if (mInternalLists[i].Contains(value))
                {
                    partition = i;
                    rindex = mInternalLists[i].IndexOf(value);
                    break;
                }
            }
            return new Tuple<int, int>(partition, rindex);
        }

        public void Insert(long index, T value)
        {
            if (index > Count - 1)
            {
                throw new IndexOutOfRangeException();
            }
            var pIndex = GetPartitionedIndex(index);
            int partition = pIndex.Item1;
            int rindex = pIndex.Item2;

            var pIndexMax = GetPartitionedSize();
            int partitionMax = pIndex.Item1;
            int rindexMax = pIndex.Item2;

            var currentCount = ConvertLongIndex(pIndexMax);

            // If inserting to the last partition, 
            // Just insert to the last partition; however, if the list is full, add new list at end and move 1 item to the new list, and insert the item.
            if (partition == partitionMax)
            {
                if (rindexMax == int.MaxValue)
                {
                    mInternalLists.Add(new List<T>());
                    var item = mInternalLists[partition][rindexMax];
                    mInternalLists[partition + 1].Add(item);
                }
            }
            else
            { 
                // Move the last item of target partition list and rest of list except the last partition to the top of next partition.
                // need to process backword from second last list.
                AutoParallel.AutoParallelFor(partition, partitionMax - 1, (i) =>
                 {
                     int l = partitionMax - 1 - i;
                     var item = mInternalLists[l][int.MaxValue];
                     mInternalLists[l + 1].Insert(0, item);
                     mInternalLists[l].RemoveAt(int.MaxValue);
                 });
            }
            mInternalLists[partition].Insert(rindex, value);
        }

        public void Remove(T value)
        {
            RemoveAt(PartitionIndexOf(value));
        }

        public void RemoveAt(long index)
        {
            RemoveAt(GetPartitionedIndex(index));
        }

        private void RemoveAt(Tuple<int, int> pIndex)
        {
            if (pIndex.Item1 >= 0 && pIndex.Item2 >= 0)
            {
                mInternalLists[pIndex.Item1].RemoveAt(pIndex.Item2);

                int partition = pIndex.Item1;
                var pIndexMax = GetPartitionedSize();
                int partitionMax = pIndexMax.Item1;
                int rindexMax = pIndexMax.Item2;

                // Move the last item of target partition list and rest of list except the last partition to the top of next partition.
                // need to process backword from second last list.
                AutoParallel.AutoParallelFor(partition, partitionMax - 1, (i) =>
                {
                    var item = mInternalLists[i + 1][0];
                    mInternalLists[i].Insert(int.MaxValue, item);
                    mInternalLists[i + 1].RemoveAt(0);
                });

                if (!mInternalLists[mInternalLists.Count - 1].Any())
                {
                    mInternalLists.RemoveAt(mInternalLists.Count - 1);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var partition in mInternalLists)
            {
                foreach (var item in partition)
                {
                    yield return item;
                }
            }
        }
    }
}
