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
// This implementation is base on Microsoft's reference code.
// https://referencesource.microsoft.com/#mscorlib/system/collections/ilist.cs,5d74f6adfeaf6c7d,references
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public interface IBigList<T> : IBigCollection<T>
    {
        // The Item property provides methods to read and edit entries in the List.
        T this[long index]
        {
            get;
            set;
        }

        // Adds an item to the list.  The exact position in the list is 
        // implementation-dependent, so while ArrayList may always insert
        // in the last available location, a SortedList most likely would not.
        // The return value is the position the new element was inserted in.
        long Add(T value);

        // Returns whether the list contains a particular item.
        bool Contains(T value);

        // Removes all items from the list.
        void Clear();

        bool IsReadOnly
        { get; }


        bool IsFixedSize
        {
            get;
        }


        // Returns the index of a particular item, if it is in the list.
        // Returns -1 if the item isn't in the list.
        long IndexOf(T value);

        // Inserts value into the list at position index.
        // index must be non-negative and less than or equal to the 
        // number of elements in the list.  If index equals the number
        // of items in the list, then value is appended to the end.
        void Insert(long index, T value);

        // Removes an item from the list.
        void Remove(T value);

        // Removes the item at position index.
        void RemoveAt(long index);
    }
}
