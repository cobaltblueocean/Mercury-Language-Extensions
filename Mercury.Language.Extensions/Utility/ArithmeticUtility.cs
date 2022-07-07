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
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language;
using Mercury.Language.Exception;

namespace System
{
    /// <summary>
    /// ArithmeticUtility Description
    /// </summary>
    public class ArithmeticUtility
    {
        /// <summary>
        /// Add two ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="x">an addend</param>
        /// <param name="y">an addend</param>
        /// <returns>the sum {@code x+y}</returns>
        /// <exception cref="MathArithmeticException">if the result can not be represented </exception>
        /// as an {@code int}.
        /// @since 1.1
        public static int AddAndCheck(int x, int y)
        {
            long s = (long)x + (long)y;
            if (s < int.MinValue || s > int.MaxValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, x, y));
            }
            return (int)s;
        }

        /// <summary>
        /// Add two long ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="a">an addend</param>
        /// <param name="b">an addend</param>
        /// <returns>the sum {@code a+b}</returns>
        /// <exception cref="MathArithmeticException">if the result can not be represented as an long </exception>
        /// @since 1.2
        public static long AddAndCheck(long a, long b)
        {
            return AddAndCheck(a, b, LocalizedResources.Instance().OVERFLOW_IN_ADDITION);
        }

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
        /// {@code ArgumentException} is thrown)</li>
        /// <li> The result is small enough to fit into a {@code long}d The
        /// largest value of {@code n} for which all coefficients are
        /// {@code  < long.MaxValue} is 66d If the computed value exceeds
        /// {@code long.MaxValue} an {@code ArithMeticException} is
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
        /// @deprecated use {@link CombinatoricsUtility#binomialCoefficient(int, int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static long BinomialCoefficient(int n, int k)
        {
            return CombinatoricsUtility.BinomialCoefficient(n, k);
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
        /// largest value of {@code n} for which all coefficients are <
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
        /// @deprecated use {@link CombinatoricsUtility#binomialCoefficientDouble(int, int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static double BinomialCoefficientDouble(int n, int k)
        {
            return CombinatoricsUtility.BinomialCoefficientDouble(n, k);
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
        /// {@code ArgumentException} is thrown)</li>
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
        /// @deprecated use {@link CombinatoricsUtility#binomialCoefficientLog(int, int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static double BinomialCoefficientLog(int n, int k)
        {
            return CombinatoricsUtility.BinomialCoefficientLog(n, k);
        }

        /// <summary>
        /// Returns n!d Shorthand for {@code n} <a
        /// href="http://mathworld.wolfram.com/Factorial.html"> Factorial</a>, the
        /// product of the numbers {@code 1,..d,n}.
        /// <p>
        /// <Strong>Preconditions</strong>:
        /// <ul>
        /// <li> {@code n >= 0} (otherwise
        /// {@code ArgumentException} is thrown)</li>
        /// <li> The result is small enough to fit into a {@code long}d The
        /// largest value of {@code n} for which {@code n!} <
        /// long.MaxValue} is 20d If the computed value exceeds {@code long.MaxValue}
        /// an {@code ArithMeticException } is thrown.</li>
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
        /// @deprecated use {@link CombinatoricsUtility#factorial(int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static long Factorial(int n)
        {
            return CombinatoricsUtility.Factorial(n);
        }

        /// <summary>
        /// Compute n!, the<a href="http://mathworld.wolfram.com/Factorial.html">
        /// factorial</a> of {@code n} (the product of the numbers 1 to n), as a
        /// {@code double}.
        /// The result should be small enough to fit into a {@code double}: The
        /// largest {@code n} for which {@code n! < Double.MaxValue} is 170.
        /// If the computed value exceeds {@code Double.MaxValue},
        /// {@code Double.PositiveInfinity} is returned.
        /// 
        /// <summary>
        /// <param name="n">Argument.</param>
        /// <returns>{@code n!}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// @deprecated use {@link CombinatoricsUtility#factorialDouble(int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static double FactorialDouble(int n)
        {
            return CombinatoricsUtility.FactorialDouble(n);
        }

        /// <summary>
        /// Compute the natural logarithm of the factorial of {@code n}.
        /// 
        /// <summary>
        /// <param name="n">Argument.</param>
        /// <returns>{@code n!}</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code n < 0}. </exception>
        /// @deprecated use {@link CombinatoricsUtility#factorialLog(int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static double FactorialLog(int n)
        {
            return CombinatoricsUtility.FactorialLog(n);
        }

        /// <summary>
        /// Computes the greatest common divisor of the absolute value of two
        /// numbers, using a modified version of the "binary gcd" method.
        /// See Knuth 4.5.2 algorithm B.
        /// The algorithm is due to Josef Stein (1961).
        /// <br/>
        /// Special cases:
        /// <ul>
        ///  <li>The invocations
        ///   {@code gcd(int.MinValue, int.MinValue)},
        ///   {@code gcd(int.MinValue, 0)} and
        ///   {@code gcd(0, int.MinValue)} throw an
        ///   {@code ArithmeticException}, because the result would be 2^31, which
        ///   is too large for an int value.</li>
        ///  <li>The result of {@code gcd(x, x)}, {@code gcd(0, x)} and
        ///   {@code gcd(x, 0)} is the absolute value of {@code x}, except
        ///   for the special cases above.</li>
        ///  <li>The invocation {@code gcd(0, 0)} is the only one which returns
        ///   {@code 0}.</li>
        /// </ul>
        /// 
        /// <summary>
        /// <param name="p">Number.</param>
        /// <param name="q">Number.</param>
        /// <returns>the greatest common divisor (never negative).</returns>
        /// <exception cref="MathArithmeticException">if the result cannot be represented as </exception>
        /// a non-negative {@code int} value.
        /// @since 1.1
        public static int Gcd(int p, int q)
        {
            int a = p;
            int b = q;
            if (a == 0 ||
                b == 0)
            {
                if (a == int.MinValue ||
                    b == int.MinValue)
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_32_BITS, p, q));
                }
                return System.Math.Abs(a + b);
            }

            long al = a;
            long bl = b;
            Boolean uselong = false;
            if (a < 0)
            {
                if (int.MinValue == a)
                {
                    uselong = true;
                }
                else
                {
                    a = -a;
                }
                al = -al;
            }
            if (b < 0)
            {
                if (int.MinValue == b)
                {
                    uselong = true;
                }
                else
                {
                    b = -b;
                }
                bl = -bl;
            }
            if (uselong)
            {
                if (al == bl)
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_32_BITS, p, q));
                }
                long blbu = bl;
                bl = al;
                al = blbu % al;
                if (al == 0)
                {
                    if (bl > int.MaxValue)
                    {
                        throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_32_BITS, p, q));
                    }
                    return (int)bl;
                }
                blbu = bl;

                // Now "al" and "bl" fit in an "int".
                b = (int)al;
                a = (int)(blbu % al);
            }

            return GcdPositive(a, b);
        }

        /// <summary>
        /// Computes the greatest common divisor of two <em>positive</em> numbers
        /// (this precondition is <em>not</em> checked and the result is undefined
        /// if not fulfilled) using the "binary gcd" method which avoids division
        /// and modulo operations.
        /// See Knuth 4.5.2 algorithm B.
        /// The algorithm is due to Josef Stein (1961).
        /// <br/>
        /// Special cases:
        /// <ul>
        ///  <li>The result of {@code gcd(x, x)}, {@code gcd(0, x)} and
        ///   {@code gcd(x, 0)} is the value of {@code x}.</li>
        ///  <li>The invocation {@code gcd(0, 0)} is the only one which returns
        ///   {@code 0}.</li>
        /// </ul>
        /// 
        /// <summary>
        /// <param name="a">Positive number.</param>
        /// <param name="b">Positive number.</param>
        /// <returns>the greatest common divisor.</returns>
        private static int GcdPositive(int a, int b)
        {
            if (a == 0)
            {
                return b;
            }
            else if (b == 0)
            {
                return a;
            }

            // Make "a" and "b" odd, keeping track of common power of 2.
            int aTwos = a.NumberOfTrailingZeros();
            a >>= aTwos;
            int bTwos = b.NumberOfTrailingZeros();
            b >>= bTwos;
            int shift = System.Math.Min(aTwos, bTwos);

            // "a" and "b" are positive.
            // If a > b then "gdc(a, b)" is equal to "gcd(a - b, b)".
            // If a < b then "gcd(a, b)" is equal to "gcd(b - a, a)".
            // Hence, in the successive iterations:
            //  "a" becomes the absolute difference of the current values,
            //  "b" becomes the minimum of the current values.
            while (a != b)
            {
                int delta = a - b;
                b = System.Math.Min(a, b);
                a = System.Math.Abs(delta);

                // Remove any power of 2 in "a" ("b" is guaranteed to be odd).
                a >>= a.NumberOfTrailingZeros();
            }

            // Recover the common power of 2.
            return a << shift;
        }

        /// <summary>
        /// <p>
        /// Gets the greatest common divisor of the absolute value of two numbers,
        /// using the "binary gcd" method which avoids division and modulo
        /// operationsd See Knuth 4.5.2 algorithm Bd This algorithm is due to Josef
        /// Stein (1961).
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations
        /// {@code gcd(long.MinValue, long.MinValue)},
        /// {@code gcd(long.MinValue, 0L)} and
        /// {@code gcd(0L, long.MinValue)} throw an
        /// {@code ArithmeticException}, because the result would be 2^63, which
        /// is too large for a long value.</li>
        /// <li>The result of {@code gcd(x, x)}, {@code gcd(0L, x)} and
        /// {@code gcd(x, 0L)} is the absolute value of {@code x}, except
        /// for the special cases above.
        /// <li>The invocation {@code gcd(0L, 0L)} is the only one which returns
        /// {@code 0L}.</li>
        /// </ul>
        /// 
        /// <summary>
        /// <param name="p">Number.</param>
        /// <param name="q">Number.</param>
        /// <returns>the greatest common divisor, never negative.</returns>
        /// <exception cref="MathArithmeticException">if the result cannot be represented as </exception>
        /// a non-negative {@code long} value.
        /// @since 2.1
        public static long Gcd(long p, long q)
        {
            long u = p;
            long v = q;
            if ((u == 0) || (v == 0))
            {
                if ((u == long.MinValue) || (v == long.MinValue))
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_64_BITS, p, q));
                }
                return System.Math.Abs(u) + System.Math.Abs(v);
            }
            // keep u and v negative, as negative ints range down to
            // -2^63, while positive numbers can only be as large as 2^63-1
            // (i.ed we can't necessarily negate a negative number without
            // overflow)
            /* assert u!=0 && v!=0; */
            if (u > 0)
            {
                u = -u;
            } // make u negative
            if (v > 0)
            {
                v = -v;
            } // make v negative
              // B1d [Find power of 2]
            int k = 0;
            while ((u & 1) == 0 && (v & 1) == 0 && k < 63)
            { // while u and v are
              // both even...
                u /= 2;
                v /= 2;
                k++; // cast out twos.
            }
            if (k == 63)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().GCD_OVERFLOW_64_BITS, p, q));
            }
            // B2d Initialize: u and v have been divided by 2^k and at least
            // one is odd.
            long t = ((u & 1) == 1) ? v : -(u / 2)/* B3 */;
            // t negative: u was odd, v may be even (t replaces v)
            // t positive: u was even, v is odd (t replaces u)
            do
            {
                /* assert u<0 && v<0; */
                // B4/B3: cast out twos from t.
                while ((t & 1) == 0)
                { // while t is even..
                    t /= 2; // cast out twos
                }
                // B5 [reset max(u,v)]
                if (t > 0)
                {
                    u = -t;
                }
                else
                {
                    v = t;
                }
                // B6/B3d at this point both u and v should be odd.
                t = (v - u) / 2;
                // |u| larger: t positive (replace u)
                // |v| larger: t negative (replace v)
            } while (t != 0);
            return -u * (1L << k); // gcd is u*2^k
        }

        /// <summary>
        /// <p>
        /// Returns the least common multiple of the absolute value of two numbers,
        /// using the formula {@code lcm(a,b) = (a / gcd(a,b)) * b}.
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations {@code lcm(int.MinValue, n)} and
        /// {@code lcm(n, int.MinValue)}, where {@code abs(n)} is a
        /// power of 2, throw an {@code ArithmeticException}, because the result
        /// would be 2^31, which is too large for an int value.</li>
        /// <li>The result of {@code lcm(0, x)} and {@code lcm(x, 0)} is
        /// {@code 0} for any {@code x}.
        /// </ul>
        /// 
        /// <summary>
        /// <param name="a">Number.</param>
        /// <param name="b">Number.</param>
        /// <returns>the least common multiple, never negative.</returns>
        /// <exception cref="MathArithmeticException">if the result cannot be represented as </exception>
        /// a non-negative {@code int} value.
        /// @since 1.1
        public static int Lcm(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            int lcm = System.Math.Abs(ArithmeticUtility.MulAndCheck(a / Gcd(a, b), b));
            if (lcm == int.MinValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().LCM_OVERFLOW_32_BITS, a, b));
            }
            return lcm;
        }

        /// <summary>
        /// <p>
        /// Returns the least common multiple of the absolute value of two numbers,
        /// using the formula {@code lcm(a,b) = (a / gcd(a,b)) * b}.
        /// </p>
        /// Special cases:
        /// <ul>
        /// <li>The invocations {@code lcm(long.MinValue, n)} and
        /// {@code lcm(n, long.MinValue)}, where {@code abs(n)} is a
        /// power of 2, throw an {@code ArithmeticException}, because the result
        /// would be 2^63, which is too large for an int value.</li>
        /// <li>The result of {@code lcm(0L, x)} and {@code lcm(x, 0L)} is
        /// {@code 0L} for any {@code x}.
        /// </ul>
        /// 
        /// <summary>
        /// <param name="a">Number.</param>
        /// <param name="b">Number.</param>
        /// <returns>the least common multiple, never negative.</returns>
        /// <exception cref="MathArithmeticException">if the result cannot be represented </exception>
        /// as a non-negative {@code long} value.
        /// @since 2.1
        public static long Lcm(long a, long b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            long lcm = System.Math.Abs(ArithmeticUtility.MulAndCheck(a / Gcd(a, b), b));
            if (lcm == long.MinValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().LCM_OVERFLOW_64_BITS, a, b));
            }
            return lcm;
        }

        /// <summary>
        /// Multiply two ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="x">Factor.</param>
        /// <param name="y">Factor.</param>
        /// <returns>the product {@code x * y}.</returns>
        /// <exception cref="MathArithmeticException">if the result can not be </exception>
        /// represented as an {@code int}.
        /// @since 1.1
        public static int MulAndCheck(int x, int y)
        {
            long m = ((long)x) * ((long)y);
            if (m < int.MinValue || m > int.MaxValue)
            {
                throw new MathArithmeticException(LocalizedResources.Instance().OUT_OF_RANGE);
            }
            return (int)m;
        }

        /// <summary>
        /// Multiply two long ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="a">Factor.</param>
        /// <param name="b">Factor.</param>
        /// <returns>the product {@code a * b}.</returns>
        /// <exception cref="MathArithmeticException">if the result can not be represented </exception>
        /// as a {@code long}.
        /// @since 1.2
        public static long MulAndCheck(long a, long b)
        {
            long ret;
            if (a > b)
            {
                // use symmetry to reduce boundary cases
                ret = MulAndCheck(b, a);
            }
            else
            {
                if (a < 0)
                {
                    if (b < 0)
                    {
                        // check for positive overflow with negative a, negative b
                        if (a >= long.MaxValue / b)
                        {
                            ret = a * b;
                        }
                        else
                        {
                            throw new MathArithmeticException(LocalizedResources.Instance().NUMBER_TOO_LARGE);
                        }
                    }
                    else if (b > 0)
                    {
                        // check for negative overflow with negative a, positive b
                        if (long.MinValue / b <= a)
                        {
                            ret = a * b;
                        }
                        else
                        {
                            throw new MathArithmeticException(LocalizedResources.Instance().NUMBER_TOO_SMALL);

                        }
                    }
                    else
                    {
                        // assert b == 0
                        ret = 0;
                    }
                }
                else if (a > 0)
                {
                    // assert a > 0
                    // assert b > 0

                    // check for positive overflow with positive a, positive b
                    if (a <= long.MaxValue / b)
                    {
                        ret = a * b;
                    }
                    else
                    {
                        throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW);
                    }
                }
                else
                {
                    // assert a == 0
                    ret = 0;
                }
            }
            return ret;
        }

        /// <summary>
        /// Subtract two ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="x">Minuend.</param>
        /// <param name="y">Subtrahend.</param>
        /// <returns>the difference {@code x - y}.</returns>
        /// <exception cref="MathArithmeticException">if the result can not be represented </exception>
        /// as an {@code int}.
        /// @since 1.1
        public static int SubAndCheck(int x, int y)
        {
            long s = (long)x - (long)y;
            if (s < int.MinValue || s > int.MaxValue)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_SUBTRACTION, x, y));
            }
            return (int)s;
        }

        /// <summary>
        /// Subtract two long ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="a">Value.</param>
        /// <param name="b">Value.</param>
        /// <returns>the difference {@code a - b}.</returns>
        /// <exception cref="MathArithmeticException">if the result can not be represented as a </exception>
        /// {@code long}.
        /// @since 1.2
        public static long SubAndCheck(long a, long b)
        {
            long ret;
            if (b == long.MinValue)
            {
                if (a < 0)
                {
                    ret = a - b;
                }
                else
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_ADDITION, a, -b));
                }
            }
            else
            {
                // use additive inverse
                ret = AddAndCheck(a, -b, LocalizedResources.Instance().OVERFLOW_IN_ADDITION);
            }
            return ret;
        }

        /// <summary>
        /// Raise an int to an int power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>\( k^e \)</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        /// <exception cref="MathArithmeticException">if the result would overflow. </exception>
        public static int Pow(int k,
                              int e)
        {
            if (e < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, e);
            }

            try
            {
                int exp = e;
                int result = 1;
                int k2p = k;
                while (true)
                {
                    if ((exp & 0x1) != 0)
                    {
                        result = MulAndCheck(result, k2p);
                    }

                    exp >>= 1;
                    if (exp == 0)
                    {
                        break;
                    }

                    k2p = MulAndCheck(k2p, k2p);
                }

                return result;
            }
            catch (MathArithmeticException mae)
            {
                // Add context information, andrethrow.
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW + ", " +
                    String.Format(LocalizedResources.Instance().BASE, k) + ", " +
                    String.Format(LocalizedResources.Instance().EXPONENT, e), mae);
            }
        }

        /// <summary>
        /// Raise an int to a long power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>k<sup>e</sup></returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        /// @deprecated As of 3.3d Please use {@link #pow(int,int)} instead.
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static int Pow(int k, long e)
        {
            if (e < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, e);
            }

            int result = 1;
            int k2p = k;
            while (e != 0)
            {
                if ((e & 0x1) != 0)
                {
                    result *= k2p;
                }
                k2p *= k2p;
                e >>= 1;
            }

            return result;
        }

        /// <summary>
        /// Raise a long to an int power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>\( k^e \)</returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        /// <exception cref="MathArithmeticException">if the result would overflow. </exception>
        public static long Pow(long k,
                               int e)
        {
            if (e < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, e);
            }

            try
            {
                int exp = e;
                long result = 1;
                long k2p = k;
                while (true)
                {
                    if ((exp & 0x1) != 0)
                    {
                        result = MulAndCheck(result, k2p);
                    }

                    exp >>= 1;
                    if (exp == 0)
                    {
                        break;
                    }

                    k2p = MulAndCheck(k2p, k2p);
                }

                return result;
            }
            catch (MathArithmeticException mae)
            {
                // Add context information, and rethrow.
                throw new MathArithmeticException(LocalizedResources.Instance().OVERFLOW + ", " +
                    String.Format(LocalizedResources.Instance().BASE, k) + ", " +
                    String.Format(LocalizedResources.Instance().EXPONENT, e), mae);
            }
        }

        /// <summary>
        /// Raise a long to a long power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>k<sup>e</sup></returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        /// @deprecated As of 3.3d Please use {@link #pow(long,int)} instead.
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static long Pow(long k, long e)
        {
            if (e < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, e);
            }

            long result = 1L;
            long k2p = k;
            while (e != 0)
            {
                if ((e & 0x1) != 0)
                {
                    result *= k2p;
                }
                k2p *= k2p;
                e >>= 1;
            }

            return result;
        }

        /// <summary>
        /// Raise a BigInteger to an int power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>k<sup>e</sup></returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        public static BigInteger Pow(BigInteger k, int e)
        {
            if (e < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, e);
            }

            return BigInteger.Pow(k, e);
        }

        /// <summary>
        /// Raise a BigInteger to a long power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>k<sup>e</sup></returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        public static BigInteger Pow(BigInteger k, long e)
        {
            if (e < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, e);
            }

            BigInteger result = BigInteger.One;
            BigInteger k2p = k;
            while (e != 0)
            {
                if ((e & 0x1) != 0)
                {
                    result = BigInteger.Multiply(result, k2p);
                }
                k2p = BigInteger.Multiply(k2p, k2p);
                e >>= 1;
            }

            return result;

        }

        /// <summary>
        /// Raise a BigInteger to a BigInteger power.
        /// 
        /// <summary>
        /// <param name="k">Number to raise.</param>
        /// <param name="e">Exponent (must be positive or zero).</param>
        /// <returns>k<sup>e</sup></returns>
        /// <exception cref="NotStrictlyPositiveException">if {@code e < 0}. </exception>
        public static BigInteger Pow(BigInteger k, BigInteger e)
        {
            if (e.CompareTo(BigInteger.Zero) < 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().EXPONENT, (double)e);
            }

            BigInteger result = BigInteger.One;
            BigInteger k2p = k;
            while (!BigInteger.Zero.Equals(e))
            {
                if (e.TestBit(0))
                {
                    result = BigInteger.Multiply(result, k2p);
                }
                k2p = BigInteger.Multiply(k2p, k2p);
                e = e >> 1;
            }

            return result;
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
        /// @deprecated use {@link CombinatoricsUtility#stirlingS2(int, int)}
        [Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
        public static long Stirling2(int n, int k)
        {
            return CombinatoricsUtility.StirlingS2(n, k);

        }

        /// <summary>
        /// Add two long ints, checking for overflow.
        /// 
        /// <summary>
        /// <param name="a">Addend.</param>
        /// <param name="b">Addend.</param>
        /// <param name="pattern">Pattern to use for any thrown exception.</param>
        /// <returns>the sum {@code a + b}.</returns>
        /// <exception cref="MathArithmeticException">if the result cannot be represented </exception>
        /// as a {@code long}.
        /// @since 1.2
        private static long AddAndCheck(long a, long b, String pattern)
        {
            long result = a + b;
            if (!((a ^ b) < 0 | (a ^ result) >= 0))
            {
                throw new MathArithmeticException(String.Format(pattern, a, b));
            }
            return result;
        }

        /// <summary>
        /// Returns true if the argument is a power of two.
        /// 
        /// <summary>
        /// <param name="n">the number to test</param>
        /// <returns>true if the argument is a power of two</returns>
        public static Boolean IsPowerOfTwo(long n)
        {
            return (n > 0) && ((n & (n - 1)) == 0);
        }
    }
}
