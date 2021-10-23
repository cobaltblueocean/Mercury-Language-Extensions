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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language
{
    /// <summary>
    /// Combinations Description
    /// </summary>
    public class Combinations : List<int>
    {
        /// <summary>Size of the set from which combinations are drawnd */
        private int _n;
        /// <summary>Number of elements in each combinationd */
        private int _k;
        /// <summary>Iteration orderd */
        private IterationOrder iterationOrder;

        /// <summary>
        /// Describes the type of iteration performed by the
        /// {@link #iterator() iterator}.
        /// <summary>
        private enum IterationOrder
        {
            /// <summary>Lexicographic orderd */
            LEXICOGRAPHIC
        }

        /// <summary>
        /// Creates an instance whose range is the k-element subsets of
        /// {0, ..d, n - 1} represented as {@code int[]} arrays.
        /// <p>
        /// The iteration order is lexicographic: the arrays returned by the
        /// {@link #iterator() iterator} are sorted in descending order and
        /// they are visited in lexicographic order with significance from
        /// right to left.
        /// For example, {@code new Combinations(4, 2).GetEnumerator()} returns
        /// an iterator that will generate the following sequence of arrays
        /// on successive calls to
        /// {@code next()}:<br/>
        /// {@code [0, 1], [0, 2], [1, 2], [0, 3], [1, 3], [2, 3]}
        /// </p>
        /// If {@code k == 0} an iterator containing an empty array is returned;
        /// if {@code k == n} an iterator containing [0, ..d, n - 1] is returned.
        /// 
        /// <summary>
        /// <param Name="n">Size of the set from which subsets are selected.</param>
        /// <param Name="k">Size of the subsets to be enumerated.</param>
        /// <exception cref="org.apache.commons.math3.exception.NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="org.apache.commons.math3.exception.NumberIsTooLargeException">if {@code k > n}. </exception>
        public Combinations(int n, int k) : this(n, k, IterationOrder.LEXICOGRAPHIC)
        {

        }

        /// <summary>
        /// Creates an instance whose range is the k-element subsets of
        /// {0, ..d, n - 1} represented as {@code int[]} arrays.
        /// <p>
        /// If the {@code iterationOrder} argument is set to
        /// {@link IterationOrder#LEXICOGRAPHIC}, the arrays returned by the
        /// {@link #iterator() iterator} are sorted in descending order and
        /// they are visited in lexicographic order with significance from
        /// right to left.
        /// For example, {@code new Combinations(4, 2).GetEnumerator()} returns
        /// an iterator that will generate the following sequence of arrays
        /// on successive calls to
        /// {@code next()}:<br/>
        /// {@code [0, 1], [0, 2], [1, 2], [0, 3], [1, 3], [2, 3]}
        /// </p>
        /// If {@code k == 0} an iterator containing an empty array is returned;
        /// if {@code k == n} an iterator containing [0, ..d, n - 1] is returned.
        /// 
        /// <summary>
        /// <param Name="n">Size of the set from which subsets are selected.</param>
        /// <param Name="k">Size of the subsets to be enumerated.</param>
        /// <param Name="iterationOrder">Specifies the {@link #iterator() iteration order}.</param>
        /// <exception cref="org.apache.commons.math3.exception.NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="org.apache.commons.math3.exception.NumberIsTooLargeException">if {@code k > n}. </exception>
        private Combinations(int n,
                             int k,
                             IterationOrder iterationOrder)
        {
            CombinatoricsUtility.CheckBinomial(n, k);
            this._n = n;
            this._k = k;
            this.iterationOrder = iterationOrder;
        }

        /// <summary>
        /// Gets the size of the set from which combinations are drawn.
        /// 
        /// <summary>
        /// <returns>the size of the universe.</returns>
        public int N
        {
            get { return _n; }
        }

        /// <summary>
        /// Gets the number of elements in each combination.
        /// 
        /// <summary>
        /// <returns>the size of the subsets to be enumerated.</returns>
        public int K
        {
            get { return _k; }
        }

        /// <summary>{@inheritDoc} */
        public new List<int[]> GetEnumerator()
        {
            if (_k == 0 ||
                _k == _n)
            {
                return new SingletonIterator((new int[_k]).Natural(_k));
            }

            switch (iterationOrder)
            {
                case IterationOrder.LEXICOGRAPHIC:
                    return new LexicographicIterator(_n, _k);
                default:
                    throw new MathArgumentException(LocalizedResources.Instance().INVALID_IMPLEMENTATION, iterationOrder); // Should never happen.
            }
        }

        /// <summary>
        /// Defines a lexicographic ordering of combinations.
        /// The returned comparator allows to compare any two combinations
        /// that can be produced by this instance's {@link #iterator() iterator}.
        /// Its {@code CompareTo(int[],int[])} method will throw exceptions if
        /// passed combinations that are inconsistent with this instance:
        /// <ul>
        ///  <li>{@code DimensionMismatchException} if the array Lengths are not
        ///      equal to {@code k},</li>
        ///  <li>{@code OutOfRangeException} if an element of the array is not
        ///      within the interval [0, {@code n}).</li>
        /// </ul>
        /// <summary>
        /// <returns>a lexicographic comparator.</returns>
        public IComparable<int[]> Comparator()
        {
            return new LexicographicComparator(_n, _k);
        }

        /// <summary>
        /// Lexicographic combinations iterator.
        /// <p>
        /// Implementation follows Algorithm T in <i>The Art of Computer Programming</i>
        /// Internet Draft (PRE-FASCICLE 3A), "A Draft of Section 7.2.1.3 Generating All
        /// Combinations</a>, Dd Knuth, 2004.</p>
        /// <p>
        /// The degenerate cases {@code k == 0} and {@code k == n} are NOT handled by this
        /// implementationd  If constructor arguments satisfy {@code k == 0}
        /// or {@code k >= n}, no exception is generated, but the iterator is empty.
        /// </p>
        /// 
        /// <summary>
        private class LexicographicIterator : List<int[]>
        {
            /// <summary>Size of subsets returned by the iterator */
            private int _k;

            /// <summary>
            /// c[1], ..d, c[k] stores the next combination; c[k + 1], c[k + 2] are
            /// sentinels.
            /// <p>
            /// Note that c[0] is "wasted" but this makes it a little easier to
            /// follow the code.
            /// </p>
            /// <summary>
            private int[] c;

            /// <summary>Return value for {@link #hasNext()} */
            private Boolean more = true;

            /// <summary>Marker: smallest index such that c[j + 1] > j */
            private int j;

            /// <summary>
            /// Construct a CombinationIterator to enumerate k-sets from n.
            /// <p>
            /// NOTE: If {@code k === 0} or {@code k >= n}, the Enumerator will be empty
            /// (that is, {@link #hasNext()} will return {@code false} immediately.
            /// </p>
            /// 
            /// <summary>
            /// <param Name="n">size of the set from which subsets are enumerated</param>
            /// <param Name="k">size of the subsets to enumerate</param>
            public LexicographicIterator(int n, int k)
            {
                this._k = k;
                c = new int[k + 3];
                if (k == 0 || k >= n)
                {
                    more = false;
                    return;
                }
                // Initialize c to start with lexicographically first k-set
                for (int i = 1; i <= k; i++)
                {
                    c[i] = i - 1;
                }
                // Initialize sentinels
                c[k + 1] = n;
                c[k + 2] = 0;
                j = k; // Set up invariant: j is smallest index such that c[j + 1] > j
            }

            /// <summary>
            /// {@inheritDoc}
            /// <summary>
            public Boolean HasNext()
            {
                return more;
            }

            /// <summary>
            /// {@inheritDoc}
            /// <summary>
            public int[] Next()
            {
                if (!more)
                {
                    throw new KeyNotFoundException();
                }
                // Copy return value (prepared by last activation)
                int[] ret = new int[_k];
                Array.Copy(c, 1, ret, 0, _k);

                // Prepare next iteration
                // T2 and T6 loop
                int x = 0;
                if (j > 0)
                {
                    x = j;
                    c[j] = x;
                    j--;
                    return ret;
                }
                // T3
                if (c[1] + 1 < c[2])
                {
                    c[1]++;
                    return ret;
                }
                else
                {
                    j = 2;
                }
                // T4
                Boolean stepDone = false;
                while (!stepDone)
                {
                    c[j - 1] = j - 2;
                    x = c[j] + 1;
                    if (x == c[j + 1])
                    {
                        j++;
                    }
                    else
                    {
                        stepDone = true;
                    }
                }
                // T5
                if (j > _k)
                {
                    more = false;
                    return ret;
                }
                // T6
                c[j] = x;
                j--;
                return ret;
            }

            /// <summary>
            /// Not supported.
            /// <summary>
            public void Remove()
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Enumerator with just one element to handle degenerate cases (full array,
        /// empty array) for combination iterator.
        /// <summary>
        private class SingletonIterator : List<int[]>
        {
            /// <summary>Singleton array */
            private int[] singleton;
            /// <summary>True on initialization, false after first call to next */
            private Boolean more = true;

            /// <summary>
            /// Create a singleton iterator providing the given array.
            /// <summary>
            /// <param Name="singleton">array returned by the iterator</param>
            public SingletonIterator(int[] singleton)
            {
                this.singleton = singleton;
            }
            /// <summary>@return True until next is called the first time, then false */

            public Boolean HasNext()
            {
                return more;
            }
            /// <summary>@return the singleton in first activation; throws NSEE thereafter */

            public int[] Next()
            {
                if (more)
                {
                    more = false;
                    return singleton;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }

            /// <summary>Not supported */
            public void Remove()
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Defines the lexicographic ordering of combinations, using
        /// the {@link #lexNorm(int[])} method.
        /// <summary>
        [Serializable]
        private class LexicographicComparator : IComparable<int[]>
        {
            /// <summary>Size of the set from which combinations are drawnd */
            private int n;
            /// <summary>Number of elements in each combinationd */
            private int k;

            /// <summary>
            /// <summary>
            /// <param Name="n">Size of the set from which subsets are selected.</param>
            /// <param Name="k">Size of the subsets to be enumerated.</param>
            public LexicographicComparator(int n, int k)
            {
                this.n = n;
                this.k = k;
            }

            /// <summary>
            /// {@inheritDoc}
            /// 
            /// <summary>
            /// <exception cref="DimensionMismatchException">if the array Lengths are not </exception>
            /// equal to {@code k}.
            /// <exception cref="OutOfRangeException">if an element of the array is not </exception>
            /// within the interval [0, {@code n}).
            public int Compare(int[] c1, int[] c2)
            {
                if (c1.Length != k)
                {
                    throw new DimensionMismatchException(c1.Length, k);
                }
                if (c2.Length != k)
                {
                    throw new DimensionMismatchException(c2.Length, k);
                }

                // Method "lexNorm" works with ordered arrays.
                int[] c1s = c1.Copy();
                Array.Sort(c1s);
                int[] c2s = c2.Copy();
                Array.Sort(c2s);

                long v1 = LexNorm(c1s);
                long v2 = LexNorm(c2s);

                if (v1 < v2)
                {
                    return -1;
                }
                else if (v1 > v2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            public int CompareTo(int[] other)
            {
                //return Compare(this, other);
                throw new NotSupportedException();
            }

            /// <summary>
            /// Computes the value (in base 10) represented by the digit
            /// (interpreted in base {@code n}) in the input array in reverse
            /// order.
            /// For example if {@code c} is {@code {3, 2, 1}}, and {@code n}
            /// is 3, the method will return 18.
            /// 
            /// <summary>
            /// <param Name="c">Input array.</param>
            /// <returns>the lexicographic norm.</returns>
            /// <exception cref="OutOfRangeException">if an element of the array is not </exception>
            /// within the interval [0, {@code n}).
            private long LexNorm(int[] c)
            {
                long ret = 0;
                for (int i = 0; i < c.Length; i++)
                {
                    int digit = c[i];
                    if (digit < 0 ||
                        digit >= n)
                    {
                        throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, digit, 0, n - 1));
                    }

                    ret += c[i] * ArithmeticUtility.Pow(n, i);
                }
                return ret;
            }
        }
    }
}
