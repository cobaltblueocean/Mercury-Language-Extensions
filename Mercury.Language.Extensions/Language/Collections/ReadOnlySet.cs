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
using Mercury.Language.Extensions;
using Mercury.Language.Extensions;
using Mercury.Language.Exceptions;
using Mercury.Language.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class SetExtensionMethods
    {
        public static ReadOnlySet<T> AsReadOnly<T>(this ISet<T> set)
        {
            return new ReadOnlySet<T>(set);
        }
    }

    /// <summary>
    /// ReadOnlySet Description
    /// Thanks for Scott Chamberlain
    /// </summary>
    /// <see cref="https://stackoverflow.com/questions/36815062/c-sharp-hashsett-read-only-workaround"/>
    public class ReadOnlySet<T> : IReadOnlyCollection<T>, ISet<T>
    {
        private readonly ISet<T> _set;
        public ReadOnlySet(ISet<T> set)
        {
            _set = set;
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _set.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _set.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _set.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _set.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _set.SetEquals(other);
        }

        public bool Add(T item)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public void Clear()
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public bool Contains(T item)
        {
            return _set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _set.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException(LocalizedResources.Instance().SET_IS_A_READ_ONLY);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_set).GetEnumerator();
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }
    }
}
