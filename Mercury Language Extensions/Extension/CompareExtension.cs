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

namespace System
{
    /// <summary>
    /// CompareExtension Description
    /// </summary>
    public static class CompareExtension
    {
        #region Operators
        public static bool GreatorThan<T>(this T value1, T value2)
        {
            IComparer<T> _comparer = Comparer<T>.Default;
            int cmp = _comparer.Compare(value1, value2);

            return cmp > 0;
        }

        public static bool GreatorThanOrEqualTo<T>(this T value1, T value2)
        {
            IComparer<T> _comparer = Comparer<T>.Default;
            int cmp = _comparer.Compare(value1, value2);

            return cmp > 0 || cmp == 0;
        }

        public static bool EqualTo<T>(this T value1, T value2)
        {
            IComparer<T> _comparer = Comparer<T>.Default;
            int cmp = _comparer.Compare(value1, value2);

            return cmp == 0;
        }
        public static bool NotEqualTo<T>(this T value1, T value2)
        {
            IComparer<T> _comparer = Comparer<T>.Default;
            int cmp = _comparer.Compare(value1, value2);

            return cmp != 0;
        }

        public static bool LessThan<T>(this T value1, T value2)
        {
            IComparer<T> _comparer = Comparer<T>.Default;
            int cmp = _comparer.Compare(value1, value2);

            return cmp < 0;
        }

        public static bool LessThanOrEqualTo<T>(this T value1, T value2)
        {
            IComparer<T> _comparer = Comparer<T>.Default;
            int cmp = _comparer.Compare(value1, value2);

            return cmp < 0 || cmp == 0;
        }

        #endregion
    }
}
