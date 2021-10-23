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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mercury.Language;
using Mercury.Language.Exception;

namespace System
{
    /// <summary>
    /// CombinatoricsUtility Description
    /// </summary>
    public class CombinatoricsUtility
    {
        /// <summary>All long-representable factorials */
        static long[] FACTORIALS = new long[] {
                       1L,                  1L,                   2L,
                       6L,                 24L,                 120L,
                     720L,               5040L,               40320L,
                  362880L,            3628800L,            39916800L,
               479001600L,         6227020800L,         87178291200L,
           1307674368000L,     20922789888000L,     355687428096000L,
        6402373705728000L, 121645100408832000L, 2432902008176640000L };

        /// <summary>Stirling numbers of the second kindd */
        static long[][] STIRLING_S2 = null; //new long[][];

        /// <summary>Private constructor (class contains only static methods)d */
        private CombinatoricsUtility() { }


        /// <summary>
        /// Returns an exact representation of the <a
        /// href="http://mathworld.wolfram.com/BinomialCoefficient.html"> Binomial
        /// Coefficient</a>, "{@code n choose k}", the number of
        /// {@code k}-element subsets that can be selected from an
        /// {@code n}-element set.
        /// <p>
        /// <Strong>Preconditions</strong>:
        /// <ul>
        /// <li> {@code 0 <= k <= n } (otherwise
        /// {@code MathArgumentException} is thrown)</li>
        /// <li> The result is small enough to fit into a {@code long}d The
        /// largest value of {@code n} for which all coefficients are
        /// {@code  < long.MaxValue} is 66d If the computed value exceeds
        /// {@code long.MaxValue} a {@code MathArithMeticException} is
        /// thrown.</li>
        /// </ul></p>
        /// 
        /// <summary>
        /// <param name="n">the size of the set</param>
        /// <param name="k">the size of the subsets to be counted</param>
        /// <returns>{@code n choose k}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="NumberIsTooLargeException">if {@code k > n}. </exception>
        /// <exception cref="MathArithmeticException">if the result is too large to be </exception>
        /// represented by a long int.
        public static long BinomialCoefficient(int n, int k)
        {
            CombinatoricsUtility.CheckBinomial(n, k);
            if ((n == k) || (k == 0))
            {
                return 1;
            }
            if ((k == 1) || (k == n - 1))
            {
                return n;
            }
            // Use symmetry for large k
            if (k > n / 2)
            {
                return BinomialCoefficient(n, n - k);
            }

            // We use the formula
            // (n choose k) = n! / (n-k)! / k!
            // (n choose k) == ((n-k+1)*...*n) / (1*...*k)
            // which could be written
            // (n choose k) == (n-1 choose k-1) * n / k
            long result = 1;
            if (n <= 61)
            {
                // For n <= 61, the naive implementation cannot overflow.
                int i = n - k + 1;
                for (int j = 1; j <= k; j++)
                {
                    result = result * i / j;
                    i++;
                }
            }
            else if (n <= 66)
            {
                // For n > 61 but n <= 66, the result cannot overflow,
                // but we must take care not to overflow intermediate values.
                int i = n - k + 1;
                for (int j = 1; j <= k; j++)
                {
                    // We know that (result * i) is divisible by j,
                    // but (result * i) may overflow, so we split j:
                    // Filter out the gcd, d, so j/d and i/d are int.
                    // result is divisible by (j/d) because (j/d)
                    // is relative prime to (i/d) and is a divisor of
                    // result * (i/d).
                    long d = ArithmeticUtility.Gcd(i, j);
                    result = (result / (j / d)) * (i / d);
                    i++;
                }
            }
            else
            {
                // For n > 66, a result overflow might occur, so we check
                // the multiplication, taking care to not overflow
                // unnecessary.
                int i = n - k + 1;
                for (int j = 1; j <= k; j++)
                {
                    long d = ArithmeticUtility.Gcd(i, j);
                    result = ArithmeticUtility.MulAndCheck(result / (j / d), i / d);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a {@code double} representation of the <a
        /// href="http://mathworld.wolfram.com/BinomialCoefficient.html"> Binomial
        /// Coefficient</a>, "{@code n choose k}", the number of
        /// {@code k}-element subsets that can be selected from an
        /// {@code n}-element set.
        /// <p>
        /// <Strong>Preconditions</strong>:
        /// <ul>
        /// <li> {@code 0 <= k <= n } (otherwise
        /// {@code ArgumentException} is thrown)</li>
        /// <li> The result is small enough to fit into a {@code double}d The
        /// largest value of {@code n} for which all coefficients are less than
        /// Double.MaxValue is 1029d If the computed value exceeds Double.MaxValue,
        /// Double.PositiveInfinity is returned</li>
        /// </ul></p>
        /// 
        /// <summary>
        /// <param name="n">the size of the set</param>
        /// <param name="k">the size of the subsets to be counted</param>
        /// <returns>{@code n choose k}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="NumberIsTooLargeException">if {@code k > n}. </exception>
        /// <exception cref="MathArithmeticException">if the result is too large to be </exception>
        /// represented by a long int.
        public static double BinomialCoefficientDouble(int n, int k)
        {
            CombinatoricsUtility.CheckBinomial(n, k);
            if ((n == k) || (k == 0))
            {
                return 1d;
            }
            if ((k == 1) || (k == n - 1))
            {
                return n;
            }
            if (k > n / 2)
            {
                return BinomialCoefficientDouble(n, n - k);
            }
            if (n < 67)
            {
                return BinomialCoefficient(n, k);
            }

            double result = 1d;
            for (int i = 1; i <= k; i++)
            {
                result *= (double)(n - k + i) / (double)i;
            }

            return Math2.Floor(result + 0.5);
        }

        /// <summary>
        /// Returns the natural {@code log} of the <a
        /// href="http://mathworld.wolfram.com/BinomialCoefficient.html"> Binomial
        /// Coefficient</a>, "{@code n choose k}", the number of
        /// {@code k}-element subsets that can be selected from an
        /// {@code n}-element set.
        /// <p>
        /// <Strong>Preconditions</strong>:
        /// <ul>
        /// <li> {@code 0 <= k <= n } (otherwise
        /// {@code MathArgumentException} is thrown)</li>
        /// </ul></p>
        /// 
        /// <summary>
        /// <param name="n">the size of the set</param>
        /// <param name="k">the size of the subsets to be counted</param>
        /// <returns>{@code n choose k}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="NumberIsTooLargeException">if {@code k > n}. </exception>
        /// <exception cref="MathArithmeticException">if the result is too large to be </exception>
        /// represented by a long int.
        public static double BinomialCoefficientLog(int n, int k)
        {
            CombinatoricsUtility.CheckBinomial(n, k);
            if ((n == k) || (k == 0))
            {
                return 0;
            }
            if ((k == 1) || (k == n - 1))
            {
                return Math2.Log(n);
            }

            /*
             * For values small enough to do exact int computation,
             * return the log of the exact value
             */
            if (n < 67)
            {
                return Math2.Log(BinomialCoefficient(n, k));
            }

            /*
             * Return the log of binomialCoefficientDouble? for values that will not
             * overflow binomialCoefficientDouble?
             */
            if (n < 1030)
            {
                return Math2.Log(BinomialCoefficientDouble(n, k));
            }

            if (k > n / 2)
            {
                return BinomialCoefficientLog(n, n - k);
            }

            /*
             * Sum logs for values that could overflow
             */
            double logSum = 0;

            // n!/(n-k)!
            for (int i = n - k + 1; i <= n; i++)
            {
                logSum += Math2.Log(i);
            }

            // divide by k!
            for (int i = 2; i <= k; i++)
            {
                logSum -= Math2.Log(i);
            }

            return logSum;
        }

        /// <summary>
        /// Returns n!d Shorthand for {@code n} <a
        /// href="http://mathworld.wolfram.com/Factorial.html"> Factorial</a>, the
        /// product of the numbers {@code 1,..d,n}.
        /// <p>
        /// <Strong>Preconditions</strong>:
        /// <ul>
        /// <li> {@code n >= 0} (otherwise
        /// {@code MathArgumentException} is thrown)</li>
        /// <li> The result is small enough to fit into a {@code long}d The
        /// largest value of {@code n} for which {@code n!} does not exceed
        /// long.MaxValue} is 20d If the computed value exceeds {@code long.MaxValue}
        /// an {@code MathArithMeticException } is thrown.</li>
        /// </ul>
        /// </p>
        /// 
        /// <summary>
        /// <param name="n">argument</param>
        /// <returns>{@code n!}</returns>
        /// <exception cref="MathArithmeticException">if the result is too large to be represented </exception>
        /// by a {@code long}.
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="MathArithmeticException">if {@code n > 20}: The factorial value is too </exception>
        /// large to fit in a {@code long}.
        public static long Factorial(int n)
        {
            if (n < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().FACTORIAL_NEGATIVE_PARAMETER,
                                               n);
            }
            if (n > 20)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().NUMBER_TOO_LARGE);
            }
            return FACTORIALS[n];
        }

        /// <summary>
        /// Compute n!, the<a href="http://mathworld.wolfram.com/Factorial.html">
        /// factorial</a> of {@code n} (the product of the numbers 1 to n), as a
        /// {@code double}.
        /// The result should be small enough to fit into a {@code double}: The
        /// largest {@code n} for which {@code n!} does not exceed
        /// {@code Double.MaxValue} is 170d If the computed value exceeds
        /// {@code Double.MaxValue}, {@code Double.PositiveInfinity} is returned.
        /// 
        /// <summary>
        /// <param name="n">Argument.</param>
        /// <returns>{@code n!}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        public static double FactorialDouble(int n)
        {
            if (n < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().FACTORIAL_NEGATIVE_PARAMETER,
                                               n);
            }
            if (n < 21)
            {
                return FACTORIALS[n];
            }
            return Math2.Floor(Math2.Exp(CombinatoricsUtility.FactorialLog(n)) + 0.5);
        }

        /// <summary>
        /// Compute the natural logarithm of the factorial of {@code n}.
        /// 
        /// <summary>
        /// <param name="n">Argument.</param>
        /// <returns>{@code n!}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        public static double FactorialLog(int n)
        {
            if (n < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().FACTORIAL_NEGATIVE_PARAMETER,
                                               n);
            }
            if (n < 21)
            {
                return Math2.Log(FACTORIALS[n]);
            }
            double logSum = 0;
            for (int i = 2; i <= n; i++)
            {
                logSum += Math2.Log(i);
            }
            return logSum;
        }

        /// <summary>
        /// Returns the <a
        /// href="http://mathworld.wolfram.com/StirlingNumberoftheSecondKind.html">
        /// Stirling number of the second kind</a>, "{@code S(n,k)}", the number of
        /// ways of partitioning an {@code n}-element set into {@code k} non-empty
        /// subsets.
        /// <p>
        /// The preconditions are {@code 0 <= k <= n } (otherwise
        /// {@code NotStrictlyPositiveException} is thrown)
        /// </p>
        /// <summary>
        /// <param name="n">the size of the set</param>
        /// <param name="k">the number of non-empty subsets</param>
        /// <returns>{@code S(n,k)}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code k < 0}. </exception>
        /// <exception cref="NumberIsTooLargeException">if {@code k > n}. </exception>
        /// <exception cref="MathArithmeticException">if some overflow happens, typically for n exceeding 25 and </exception>
        /// k between 20 and n-2 (S(n,n-1) is handled specifically and does not overflow)
        /// @since 3.1
        public static long StirlingS2(int n, int k)
        {
            if (k < 0)
            {
                throw new NotStrictlyPositiveException(k);
            }
            if (k > n)
            {
                throw new NumberIsTooLargeException(k, n, true);
            }

            long[][] stirlingS2 = STIRLING_S2.Copy();

            if (stirlingS2 == null)
            {
                // the cache has never been initialized, compute the first numbers
                // by direct recurrence relation

                // as S(26,9) = 11201516780955125625 is larger than long.MaxValue
                // we must stop computation at row 26
                int maxIndex = 26;
                stirlingS2 = new long[maxIndex][];
                stirlingS2[0] = new long[] { 1L };
                for (int i = 1; i < stirlingS2.Length; ++i)
                {
                    stirlingS2[i] = new long[i + 1];
                    stirlingS2[i][0] = 0;
                    stirlingS2[i][1] = 1;
                    stirlingS2[i][i] = 1;
                    for (int j = 2; j < i; ++j)
                    {
                        stirlingS2[i][j] = j * stirlingS2[i - 1][j] + stirlingS2[i - 1][j - 1];
                    }
                }

                // atomically save the cache
                //STIRLING_S2.compareAndSet(null, stirlingS2);
                long[][] _val = null;
                STIRLING_S2 = Interlocked.CompareExchange(ref _val, null, stirlingS2);

            }

            if (n < stirlingS2.Length)
            {
                // the number is in the small cache
                return stirlingS2[n][k];
            }
            else
            {
                // use explicit formula to compute the number without caching it
                if (k == 0)
                {
                    return 0;
                }
                else if (k == 1 || k == n)
                {
                    return 1;
                }
                else if (k == 2)
                {
                    return (1L << (n - 1)) - 1L;
                }
                else if (k == n - 1)
                {
                    return BinomialCoefficient(n, 2);
                }
                else
                {
                    // definition formula: note that this may trigger some overflow
                    long sum = 0;
                    long sign = ((k & 0x1) == 0) ? 1 : -1;
                    for (int j = 1; j <= k; ++j)
                    {
                        sign = -sign;
                        sum += sign * BinomialCoefficient(k, j) * ArithmeticUtility.Pow(j, n);
                        if (sum < 0)
                        {
                            // there was an overflow somewhere
                            throw new MathArithmeticException(String.Format(LocalizedResources.Instance().ARGUMENT_OUTSIDE_DOMAIN,
                                                              n, 0, stirlingS2.Length - 1));
                        }
                    }
                    return sum / Factorial(k);
                }
            }

        }

        /// <summary>
        /// Returns an iterator whose range is the k-element subsets of {0, ..d, n - 1}
        /// represented as {@code int[]} arrays.
        /// <p>
        /// The arrays returned by the iterator are sorted in descending order and
        /// they are visited in lexicographic order with significance from right to
        /// leftd For example, combinationsIterator(4, 2) returns an Enumerator that
        /// will generate the following sequence of arrays on successive calls to
        /// {@code next()}:</p><p>
        /// {@code [0, 1], [0, 2], [1, 2], [0, 3], [1, 3], [2, 3]}
        /// </p><p>
        /// If {@code k == 0} an Enumerator containing an empty array is returned and
        /// if {@code k == n} an Enumerator containing [0, ..d, n -1] is returned.</p>
        /// 
        /// <summary>
        /// <param name="n">Size of the set from which subsets are selected.</param>
        /// <param name="k">Size of the subsets to be enumerated.</param>
        /// <returns>an {@link Enumerator iterator} over the k-sets in n.</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="NumberIsTooLargeException">if {@code k > n}. </exception>
        public static List<int[]> CombinationsIterator(int n, int k)
        {
            return new Combinations(n, k).GetEnumerator();
        }

        /// <summary>
        /// Check binomial preconditions.
        /// 
        /// <summary>
        /// <param name="n">Size of the set.</param>
        /// <param name="k">Size of the subsets to be counted.</param>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// <exception cref="NumberIsTooLargeException">if {@code k > n}. </exception>
        public static void CheckBinomial(int n, int k)
        {
            if (n < k)
            {
                throw new NumberIsTooLargeException(LocalizedResources.Instance().BINOMIAL_INVALID_PARAMETERS_ORDER,
                                                    k, n, true);
            }
            if (n < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().BINOMIAL_NEGATIVE_PARAMETER, n);
            }
        }
    }
}
