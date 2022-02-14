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

namespace Mercury.Language.Extension
{
    /// <summary>
    /// IEnumerableExtension Description
    /// </summary>
    /// <see cref="https://stackoverflow.com/questions/1290603/how-to-get-the-index-of-an-element-in-an-ienumerable"/>
    public static class IEnumerableExtension
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T item) => enumerable.IndexOf(item, EqualityComparer<T>.Default);

        public static int IndexOf<T>(this IEnumerable<T> enumerable, T item, EqualityComparer<T> comparer)
        {
            int index = enumerable.TakeWhile(x => comparer.Equals(x, item)).Count();
            return index == enumerable.Count() ? -1 : index;
        }
    }
}
