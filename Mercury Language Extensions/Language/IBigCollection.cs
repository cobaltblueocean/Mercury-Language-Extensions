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
// This implementation is based on Microsoft's reference code.
// https://referencesource.microsoft.com/#mscorlib/system/collections/icollection.cs,cde8022b6e680e40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public interface IBigCollection<T> : IEnumerable<T>
    {
        // Interfaces are not serialable
        // CopyTo copies a collection into an Array, starting at a particular
        // index into the array.
        // 
        void CopyTo(Array array, long index);

        // Number of items in the collections.
        long Count
        { get; }

        // SyncRoot will return an Object to use for synchronization 
        // (thread safety).  You can use this object in your code to take a
        // lock on the collection, even if this collection is a wrapper around
        // another collection.  The intent is to tunnel through to a real 
        // implementation of a collection, and use one of the internal objects
        // found in that code.
        //
        // In the absense of a static Synchronized method on a collection, 
        // the expected usage for SyncRoot would look like this:
        // 
        // ICollection col = ...
        // lock (col.SyncRoot) {
        //     // Some operation on the collection, which is now thread safe.
        //     // This may include multiple operations.
        // }
        // 
        // 
        // The system-provided collections have a static method called 
        // Synchronized which will create a thread-safe wrapper around the 
        // collection.  All access to the collection that you want to be 
        // thread-safe should go through that wrapper collection.  However, if
        // you need to do multiple calls on that collection (such as retrieving
        // two items, or checking the count then doing something), you should
        // NOT use our thread-safe wrapper since it only takes a lock for the
        // duration of a single method call.  Instead, use Monitor.Enter/Exit
        // or your language's equivalent to the C# lock keyword as mentioned 
        // above.
        // 
        // For collections with no publically available underlying store, the 
        // expected implementation is to simply return the this pointer.  Note 
        // that the this pointer may not be sufficient for collections that 
        // wrap other collections;  those should return the underlying 
        // collection's SyncRoot property.
        Object SyncRoot
        { get; }

        // Is this collection synchronized (i.e., thread-safe)?  If you want a 
        // thread-safe collection, you can use SyncRoot as an object to 
        // synchronize your collection with.  If you're using one of the 
        // collections in System.Collections, you could call the static 
        // Synchronized method to get a thread-safe wrapper around the 
        // underlying collection.
        bool IsSynchronized
        { get; }
    }
}
