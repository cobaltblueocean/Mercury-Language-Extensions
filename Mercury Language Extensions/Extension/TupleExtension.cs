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
    public static class TupleExtension
    {
        /// <summary>
        /// Compare for two pair instances capturing the generic types ensuring they are comparable.
        /// A Tuple<S, T> <i>(x<sub>1</sub>, y<sub>1</sub>)</i> is less than another pair
        /// <i>(x<sub>2</sub>, y<sub>2</sub>)</i> if one of these is true:<br />
        /// <i>x<sub>1</sub> < x<sub>2</sub></i><br>
        /// <i>x<sub>1</sub> = x<sub>2</sub></i> and <i>y<sub>1</sub> < y<sub>2</sub></i><br>
        /// <p>
        /// This comparator does not support null elements in the pair.
        /// </summary>
        /// <typeparam name="T">the first element type</typeparam>
        /// <typeparam name="S">the second element type</typeparam>
        /// <param name="tuple"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Boolean FirstThenSecondPairCompare<S, T>(this Tuple<S, T> tuple, Tuple<S, T> other)
        {
            if (tuple.Item1.Equals(other.Item1))
            {
                return tuple.Item2.Equals(other.Item2);
            }
            return tuple.Item1.Equals(other.Item1);
        }

        /// <summary>
        /// Compare for two triple instances capturing the generic types ensuring they are comparable.
        /// A Tuple<S, T, U> <i>(x<sub>1</sub>, y<sub>1</sub>)</i> is less than another pair
        /// <i>(x<sub>2</sub>, y<sub>2</sub>)</i> if one of these is true:<br />
        /// <i>x<sub>1</sub> < x<sub>2</sub></i><br>
        /// <i>x<sub>1</sub> = x<sub>2</sub></i> and <i>y<sub>1</sub> < y<sub>2</sub></i><br>
        /// <p>
        /// This comparator does not support null elements in the pair.
        /// </summary>
        /// <typeparam name="T">the first element type</typeparam>
        /// <typeparam name="S">the second element type</typeparam>
        /// <param name="tuple"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="tuple"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Boolean FirstThenSecondThenThirdTripleCompare<S, T, U>(this Tuple<S, T, U> tuple, Tuple<S, T, U> second)
        {
            if (tuple.Item1.Equals(second.Item1))
            {
                if (tuple.Item1.Equals(second.Item1))
                {
                    return tuple.Item3.Equals(second.Item3);
                }
                return tuple.Item2.Equals(second.Item2);
            }
            return tuple.Item1.Equals(second.Item1);
        }
    }
}
