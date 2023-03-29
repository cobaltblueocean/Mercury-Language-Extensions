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
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;

namespace Mercury.Language.Math.Fraction
{
    /// <summary>
    /// Representation of a rational number without any overflowd This class is
    /// immutable.
    /// 
    /// @version $Revision: 1073687 $ $Date: 2011-02-23 11:39:25 +0100 (merd 23 févrd 2011) $
    /// @since 2.0
    /// </summary>
    [Serializable]
    public class BigFraction : IComparable<BigFraction>
    {

        /// <summary>A fraction representing "2 / 1"d */
        public static BigFraction TWO = new BigFraction(2);

        /// <summary>A fraction representing "1"d */
        public static BigFraction ONE = new BigFraction(1);

        /// <summary>A fraction representing "0"d */
        public static BigFraction ZERO = new BigFraction(0);

        /// <summary>A fraction representing "-1 / 1"d */
        public static BigFraction MINUS_ONE = new BigFraction(-1);

        /// <summary>A fraction representing "4/5"d */
        public static BigFraction FOUR_FIFTHS = new BigFraction(4, 5);

        /// <summary>A fraction representing "1/5"d */
        public static BigFraction ONE_FIFTH = new BigFraction(1, 5);

        /// <summary>A fraction representing "1/2"d */
        public static BigFraction ONE_HALF = new BigFraction(1, 2);

        /// <summary>A fraction representing "1/4"d */
        public static BigFraction ONE_QUARTER = new BigFraction(1, 4);

        /// <summary>A fraction representing "1/3"d */
        public static BigFraction ONE_THIRD = new BigFraction(1, 3);

        /// <summary>A fraction representing "3/5"d */
        public static BigFraction THREE_FIFTHS = new BigFraction(3, 5);

        /// <summary>A fraction representing "3/4"d */
        public static BigFraction THREE_QUARTERS = new BigFraction(3, 4);

        /// <summary>A fraction representing "2/5"d */
        public static BigFraction TWO_FIFTHS = new BigFraction(2, 5);

        /// <summary>A fraction representing "2/4"d */
        public static BigFraction TWO_QUARTERS = new BigFraction(2, 4);

        /// <summary>A fraction representing "2/3"d */
        public static BigFraction TWO_THIRDS = new BigFraction(2, 3);

        /// <summary><code>BigInteger</code> representation of 100d */
        private static BigInteger ONE_HUNDRED_DOUBLE = new BigInteger(100);

        /// <summary>The numeratord */
        private BigInteger numerator;

        /// <summary>The denominatord */
        private BigInteger denominator;

        /// <summary>
        /// <p>
        /// Create a {@link BigFraction} equivalent to the passed <i>BigInteger</i>, ie
        /// "num / 1".
        /// </p>
        /// 
        /// </summary>
        /// <param Name="num"></param>
        ///            the numerator.
        public BigFraction(BigInteger num) : this(num, BigInteger.One)
        {

        }

        /// <summary>
        /// Create a {@link BigFraction} given the numerator and denominator as
        /// {@code BigInteger}d The {@link BigFraction} is reduced to lowest terms.
        /// 
        /// </summary>
        /// <param Name="num">the numerator, must not be {@code null}.</param>
        /// <param Name="den">the denominator, must not be {@code null}..</param>
        /// <exception cref="ArithmeticException">if the denominator is zerod </exception>
        public BigFraction(BigInteger num, BigInteger den)
        {
            if (num == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().NUMERATOR);
            }
            if (den == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().DENOMINATOR);
            }
            if (BigInteger.Zero.Equals(den))
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }
            if (BigInteger.Zero.Equals(num))
            {
                numerator = BigInteger.Zero;
                denominator = BigInteger.One;
            }
            else
            {

                // reduce numerator and denominator by greatest common denominator
                BigInteger gcd = num.GreatestCommonDivisor(den);
                if (BigInteger.One.CompareTo(gcd) < 0)
                {
                    num = num.Divide(gcd);
                    den = den.Divide(gcd);
                }

                // move sign to numerator
                if (BigInteger.Zero.CompareTo(den) > 0)
                {
                    num = num.Negate();
                    den = den.Negate();
                }

                // store the values in the fields
                numerator = num;
                denominator = den;

            }
        }

        /// <summary>
        /// Create a fraction given the double value.
        /// <p>
        /// This constructor behaves <em>differently</em> from
        /// {@link #BigFraction(double, double, int)}d It converts the
        /// double value exactly, considering its internal bits representation.
        /// This does work for all values except NaN and infinities and does
        /// not requires any loop or convergence threshold.
        /// </p>
        /// <p>
        /// Since this conversion is exact and since double numbers are sometimes
        /// approximated, the fraction created may seem strange in some casesd For example
        /// calling <code>new BigFraction(1.0 / 3.0)</code> does <em>not</em> create
        /// the fraction 1/3 but the fraction 6004799503160661 / 18014398509481984
        /// because the double number passed to the constructor is not exactly 1/3
        /// (this number cannot be stored exactly in IEEE754).
        /// </p>
        /// </summary>
        /// <see cref="#BigFraction(double,">double, int) </see>
        /// <param Name="value">the double value to convert to a fraction.</param>
        /// <exception cref="ArgumentException">if value is NaN or infinite </exception>
        public BigFraction(double value)
        {
            if (Double.IsNaN(value)) {
                throw new ArgumentException(LocalizedResources.Instance().NAN_VALUE_CONVERSION);
            }
            if (value.IsInfinite()) {
                throw new ArgumentException(LocalizedResources.Instance().INFINITE_VALUE_CONVERSION);
            }

            // compute m and k such that value = m * 2^k
            long bits = BitConverter.DoubleToInt64Bits(value);
            ulong sign = (ulong)bits & 0x8000000000000000L;
            long exponent = bits & 0x7ff0000000000000L;
            long m = bits & 0x000fffffffffffffL;
            if (exponent != 0)
            {
                // this was a normalized number, add the implicit most significant bit
                m |= 0x0010000000000000L;
            }
            if (sign != 0)
            {
                m = -m;
            }
            int k = ((int)(exponent >> 52)) - 1075;
            while (((m & 0x001ffffffffffffeL) != 0) && ((m & 0x1) == 0))
            {
                m = m >> 1;
                ++k;
            }

            if (k < 0)
            {
                numerator = new BigInteger(m);
                denominator = BigInteger.Zero.FlipBit(-k);
            }
            else
            {
                numerator = new BigInteger(m).Multiply(BigInteger.Zero.FlipBit(k));
                denominator = BigInteger.One;
            }

        }

        /// <summary>
        /// Create a fraction given the double value and maximum error allowed.
        /// <p>
        /// References:
        /// <ul>
        /// <li><a href="http://mathworld.wolfram.com/ContinuedFraction.html">
        /// Continued Fraction</a> equations (11) and (22)-(26)</li>
        /// </ul>
        /// </p>
        /// 
        /// </summary>
        /// <param Name="value"></param>
        ///            the double value to convert to a fraction.
        /// <param Name="epsilon"></param>
        ///            maximum error allowedd The resulting fraction is within
        ///            <code>epsilon</code> of <code>value</code>, in absolute terms.
        /// <param Name="maxIterations"></param>
        ///            maximum number of convergents.
        /// <exception cref="FractionConversionException"></exception>
        ///             if the continued fraction failed to converge.
        /// <see cref="#BigFraction(double)"></see>
        public BigFraction(double value, double epsilon, int maxIterations) : this(value, epsilon, Int32.MaxValue, maxIterations)

        {

        }

        /// <summary>
        /// Create a fraction given the double value and either the maximum error
        /// allowed or the maximum number of denominator digits.
        /// <p>
        /// 
        /// NOTE: This constructor is called with EITHER - a valid epsilon value and
        /// the maxDenominator set to Int32.MaxValue (that way the maxDenominator
        /// has no effect)d OR - a valid maxDenominator value and the epsilon value
        /// set to zero (that way epsilon only has effect if there is an exact match
        /// before the maxDenominator value is reached).
        /// </p>
        /// <p>
        /// 
        /// It has been done this way so that the same code can be (re)used for both
        /// scenariosd However this could be confusing to users if it were part of
        /// the public API and this constructor should therefore remain PRIVATE.
        /// </p>
        /// 
        /// See JIRA issue ticket MATH-181 for more details:
        /// 
        /// https://issues.apache.org/jira/browse/MATH-181
        /// 
        /// </summary>
        /// <param Name="value"></param>
        ///            the double value to convert to a fraction.
        /// <param Name="epsilon"></param>
        ///            maximum error allowedd The resulting fraction is within
        ///            <code>epsilon</code> of <code>value</code>, in absolute terms.
        /// <param Name="maxDenominator"></param>
        ///            maximum denominator value allowed.
        /// <param Name="maxIterations"></param>
        ///            maximum number of convergents.
        /// <exception cref="FractionConversionException"></exception>
        ///             if the continued fraction failed to converge.
        private BigFraction(double value, double epsilon, int maxDenominator, int maxIterations)

        {
            long overflow = Int32.MaxValue;
            double r0 = value;
            long a0 = (long)System.Math.Floor(r0);
            if (a0 > overflow) {
                throw new FractionConversionException(value, a0, 1L);
            }

            // check AutoParallel.AutoParallelForEach(almost) integer arguments, which should not go
            // to iterations.
            if (System.Math.Abs(a0 - value) < epsilon) {
                numerator = new BigInteger(a0);
                denominator = BigInteger.One;
                return;
            }

            long p0 = 1;
            long q0 = 0;
            long p1 = a0;
            long q1 = 1;

            long p2 = 0;
            long q2 = 1;

            int n = 0;
            Boolean stop = false;
            do {
                ++n;
                double r1 = 1.0 / (r0 - a0);
                long a1 = (long)System.Math.Floor(r1);
                p2 = (a1 * p1) + p0;
                q2 = (a1 * q1) + q0;
                if ((p2 > overflow) || (q2 > overflow))
                {
                    throw new FractionConversionException(value, p2, q2);
                }

                double convergent = (double)p2 / (double)q2;
                if ((n < maxIterations) &&
                    (System.Math.Abs(convergent - value) > epsilon) &&
                    (q2 < maxDenominator))
                {
                    p0 = p1;
                    p1 = p2;
                    q0 = q1;
                    q1 = q2;
                    a0 = a1;
                    r0 = r1;
                }
                else
                {
                    stop = true;
                }
            } while (!stop);

            if (n >= maxIterations) {
                throw new FractionConversionException(value, maxIterations);
            }

            if (q2 < maxDenominator) {
                numerator = new BigInteger(p2);
                denominator = new BigInteger(q2);
            } else {
                numerator = new BigInteger(p1);
                denominator = new BigInteger(q1);
            }
        }

        /// <summary>
        /// Create a fraction given the double value and maximum denominator.
        /// <p>
        /// References:
        /// <ul>
        /// <li><a href="http://mathworld.wolfram.com/ContinuedFraction.html">
        /// Continued Fraction</a> equations (11) and (22)-(26)</li>
        /// </ul>
        /// </p>
        /// 
        /// </summary>
        /// <param Name="value"></param>
        ///            the double value to convert to a fraction.
        /// <param Name="maxDenominator"></param>
        ///            The maximum allowed value for denominator.
        /// <exception cref="FractionConversionException"></exception>
        ///             if the continued fraction failed to converge.
        public BigFraction(double value, int maxDenominator) : this(value, 0, maxDenominator, 100)

        {

        }

        /// <summary>
        /// <p>
        /// Create a {@link BigFraction} equivalent to the passed <i>int</i>, ie
        /// "num / 1".
        /// </p>
        /// 
        /// </summary>
        /// <param Name="num"></param>
        ///            the numerator.
        public BigFraction(int num) : this(new BigInteger(num), BigInteger.One)
        {

        }

        /// <summary>
        /// <p>
        /// Create a {@link BigFraction} given the numerator and denominator as simple
        /// <i>int</i>d The {@link BigFraction} is reduced to lowest terms.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="num"></param>
        ///            the numerator.
        /// <param Name="den"></param>
        ///            the denominator.
        public BigFraction(int num, int den) : this(new BigInteger(num), new BigInteger(den))
        {

        }

        /// <summary>
        /// <p>
        /// Create a {@link BigFraction} equivalent to the passed long, ie "num / 1".
        /// </p>
        /// 
        /// </summary>
        /// <param Name="num"></param>
        ///            the numerator.
        public BigFraction(long num) : this(new BigInteger(num), BigInteger.One)
        {

        }

        /// <summary>
        /// <p>
        /// Create a {@link BigFraction} given the numerator and denominator as simple
        /// <i>long</i>d The {@link BigFraction} is reduced to lowest terms.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="num"></param>
        ///            the numerator.
        /// <param Name="den"></param>
        ///            the denominator.
        public BigFraction(long num, long den) : this(new BigInteger(num), new BigInteger(den))
        {

        }

        /// <summary>
        /// <p>
        /// Creates a <code>BigFraction</code> instance with the 2 parts of a fraction
        /// Y/Z.
        /// </p>
        /// 
        /// <p>
        /// Any negative signs are resolved to be on the numerator.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="numerator"></param>
        ///            the numerator, for example the three in 'three sevenths'.
        /// <param Name="denominator"></param>
        ///            the denominator, for example the seven in 'three sevenths'.
        /// <returns>a new fraction instance, with the numerator and denominator</returns>
        ///         reduced.
        /// <exception cref="ArithmeticException"></exception>
        ///             if the denominator is <code>zero</code>.
        public static BigFraction GetReducedFraction(int numerator, int denominator)
        {
            if (numerator == 0)
            {
                return ZERO; // normalize zero.
            }

            return new BigFraction(numerator, denominator);
        }

        /// <summary>
        /// <p>
        /// Returns the absolute value of this {@link BigFraction}.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the absolute value as a {@link BigFraction}.</returns>
        public BigFraction Abs()
        {
            return (BigInteger.Zero.CompareTo(numerator) <= 0) ? this : Negate;
        }

        /// <summary>
        /// <p>
        /// Adds the value of this fraction to the passed {@link BigInteger},
        /// returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="bg"></param>
        ///            the {@link BigInteger} to add, must'nt be <code>null</code>.
        /// <returns>a <code>BigFraction</code> instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException"></exception>
        ///             if the {@link BigInteger} is <code>null</code>.
        public BigFraction Add(BigInteger bg)
        {
            return new BigFraction(numerator.Add(denominator.Multiply(bg)), denominator);
        }

        /// <summary>
        /// <p>
        /// Adds the value of this fraction to the passed <i>integer</i>, returning
        /// the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="i"></param>
        ///            the <i>integer</i> to add.
        /// <returns>a <code>BigFraction</code> instance with the resulting values.</returns>
        public BigFraction Add(int i)
        {
            return Add(new BigInteger(i));
        }

        /// <summary>
        /// <p>
        /// Adds the value of this fraction to the passed <i>long</i>, returning
        /// the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="l"></param>
        ///            the <i>long</i> to add.
        /// <returns>a <code>BigFraction</code> instance with the resulting values.</returns>
        public BigFraction Add(long l)
        {
            return Add(new BigInteger(l));
        }

        /// <summary>
        /// <p>
        /// Adds the value of this fraction to another, returning the result in
        /// reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="fraction"></param>
        ///            the {@link BigFraction} to add, must not be <code>null</code>.
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException">if the {@link BigFraction} is {@code null}d </exception>
        public BigFraction Add(BigFraction fraction)
        {
            if (fraction == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FRACTION);
            }
            if (ZERO.Equals(fraction))
            {
                return this;
            }

            BigInteger num = BigInteger.Zero;
            BigInteger den = BigInteger.Zero;

            if (denominator.Equals(fraction.denominator))
            {
                num = numerator.Add(fraction.numerator);
                den = denominator;
            }
            else
            {
                num = (numerator.Multiply(fraction.denominator)).Add((fraction.numerator).Multiply(denominator));
                den = denominator.Multiply(fraction.denominator);
            }
            return new BigFraction(num, den);

        }

        /// <summary>
        /// <p>
        /// Gets the fraction as a <code>BigDecimal</code>d This calculates the
        /// fraction as the numerator divided by denominator.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the fraction as a <code>BigDecimal</code>.</returns>
        /// <exception cref="ArithmeticException"></exception>
        ///             if the exact quotient does not have a terminating decimal
        ///             expansion.
        /// <see cref="BigDecimal"></see>
        public BigDecimal BigDecimalValue()
        {
            return numerator.Divide(denominator);
        }

        /// <summary>
        /// <p>
        /// Gets the fraction as a <code>BigDecimal</code> following the passed
        /// rounding moded This calculates the fraction as the numerator divided by
        /// denominator.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="roundingMode"></param>
        ///            rounding mode to applyd see {@link BigDecimal} constants.
        /// <returns>the fraction as a <code>BigDecimal</code>.</returns>
        /// <exception cref="ArgumentException"></exception>
        ///             if <i>roundingMode</i> does not represent a valid rounding
        ///             mode.
        /// <see cref="BigDecimal"></see>
        public BigDecimal BigDecimalValue(int roundingMode)
        {
            //return BigInteger.Divide(numerator, denominator, roundingMode);
            return numerator.Divide(denominator);
        }

        /// <summary>
        /// <p>
        /// Gets the fraction as a <code>BigDecimal</code> following the passed isScale
        /// and rounding moded This calculates the fraction as the numerator divided
        /// by denominator.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="isScale"></param>
        ///            isScale of the <code>BigDecimal</code> quotient to be returned.
        ///            see {@link BigDecimal} for more information.
        /// <param Name="roundingMode"></param>
        ///            rounding mode to applyd see {@link BigDecimal} constants.
        /// <returns>the fraction as a <code>BigDecimal</code>.</returns>
        /// <see cref="BigDecimal"></see>
        public BigDecimal bigDecimalValue(int scale, int roundingMode)
        {
            //return new BigDecimal(numerator).Divide(new BigDecimal(denominator), scale, roundingMode);
            return numerator.Divide(denominator);
        }

        /// <summary>
        /// <p>
        /// Compares this object to another based on size.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="object"></param>
        ///            the object to compare to, must not be <code>null</code>.
        /// <returns>-1 if this is less than <i>object</i>, +1 if this is greater</returns>
        ///         than <i>object</i>, 0 if they are equal.
        /// <see cref="java.lang.Comparable#CompareTo(java.lang.Object)"></see>
        public int CompareTo(BigFraction other)
        {
            BigInteger nOd = numerator.Multiply(other.denominator);
            BigInteger dOn = denominator.Multiply(other.numerator);
            return nOd.CompareTo(dOn);
        }

        /// <summary>
        /// <p>
        /// Divide the value of this fraction by the passed <code>BigInteger</code>,
        /// ie "this * 1 / bg", returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="bg"></param>
        ///            the <code>BigInteger</code> to divide by, must not be
        ///            <code>null</code>.
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException">if the {@code BigInteger} is {@code null}d </exception>
        /// <exception cref="ArithmeticException"></exception>
        ///             if the fraction to divide by is zero.
        public BigFraction Divide(BigInteger bg)
        {
            if (BigInteger.Zero.Equals(bg))
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }
            return new BigFraction(numerator, denominator.Multiply(bg));
        }

        /// <summary>
        /// <p>
        /// Divide the value of this fraction by the passed <i>int</i>, ie
        /// "this * 1 / i", returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="i"></param>
        ///            the <i>int</i> to divide by.
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        /// <exception cref="ArithmeticException"></exception>
        ///             if the fraction to divide by is zero.
        public BigFraction Divide(int i)
        {
            return Divide(new BigInteger(i));
        }

        /// <summary>
        /// <p>
        /// Divide the value of this fraction by the passed <i>long</i>, ie
        /// "this * 1 / l", returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="l"></param>
        ///            the <i>long</i> to divide by.
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        /// <exception cref="ArithmeticException"></exception>
        ///             if the fraction to divide by is zero.
        public BigFraction Divide(long l)
        {
            return Divide(new BigInteger(l));
        }

        /// <summary>
        /// <p>
        /// Divide the value of this fraction by another, returning the result in
        /// reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="fraction">Fraction to divide by, must not be {@code null}.</param>
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException">if the {@code fraction} is {@code null}d </exception>
        /// <exception cref="ArithmeticException">if the fraction to divide by is zerod </exception>
        public BigFraction Divide(BigFraction fraction)
        {
            if (fraction == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FRACTION);
            }
            if (BigInteger.Zero.Equals(fraction.numerator))
            {
                throw new MathArithmeticException(LocalizedResources.Instance().ZERO_DENOMINATOR);
            }

            return Multiply(fraction.Reciprocal());
        }

        /// <summary>
        /// <p>
        /// Gets the fraction as a <i>double</i>d This calculates the fraction as
        /// the numerator divided by denominator.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the fraction as a <i>double</i></returns>
        /// <see cref="java.lang.Number#doubleValue()"></see>
        public double DoubleValue()
        {
            return (double)numerator / (double)denominator;
        }


        /// <summary>
        /// <p>
        /// Access the denominator as a <code>BigInteger</code>.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the denominator as a <code>BigInteger</code>.</returns>
        public BigInteger Denominator
        {
            get { return denominator; }
        }

        /// <summary>
        /// <p>
        /// Access the denominator as a <i>int</i>.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the denominator as a <i>int</i>.</returns>
        public int DenominatorAsInt
        {
            get { return (int)denominator; }
        }

        /// <summary>
        /// <p>
        /// Access the denominator as a <i>long</i>.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the denominator as a <i>long</i>.</returns>
        public long DenominatorAsLong
        {
            get { return (long)denominator; }
        }

        /// <summary>
        /// <p>
        /// Access the numerator as a <code>BigInteger</code>.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the numerator as a <code>BigInteger</code>.</returns>
        public BigInteger Numerator
        {
            get { return numerator; }
        }

        /// <summary>
        /// <p>
        /// Access the numerator as a <i>int</i>.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the numerator as a <i>int</i>.</returns>
        public int NumeratorAsInt
        {
            get { return (int)numerator; }
        }

        /// <summary>
        /// <p>
        /// Access the numerator as a <i>long</i>.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the numerator as a <i>long</i>.</returns>
        public long NumeratorAsLong
        {
            get { return (long)numerator; }
        }

        /// <summary>
        /// <p>
        /// Gets the fraction as an <i>int</i>d This returns the whole number part
        /// of the fraction.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the whole number fraction part.</returns>
        /// <see cref="java.lang.Number#intValue()"></see>
        public int IntValue()
        {
            return (int)numerator.Divide(denominator);
        }

        /// <summary>
        /// <p>
        /// Gets the fraction as a <i>long</i>d This returns the whole number part
        /// of the fraction.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the whole number fraction part.</returns>
        /// <see cref="java.lang.Number#longValue()"></see>
        public long LongValue()
        {
            return (long)numerator.Divide(denominator);
        }

        /// <summary>
        /// <p>
        /// Multiplies the value of this fraction by the passed
        /// <code>BigInteger</code>, returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="bg">the {@code BigInteger} to multiply by.</param>
        /// <returns>a {@code BigFraction} instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException">if {@code bg} is {@code null}d </exception>
        public BigFraction Multiply(BigInteger bg)
        {
            if (bg == null)
            {
                throw new NullReferenceException();
            }
            return new BigFraction(bg.Multiply(numerator), denominator);
        }

        /// <summary>
        /// <p>
        /// Multiply the value of this fraction by the passed <i>int</i>, returning
        /// the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="i"></param>
        ///            the <i>int</i> to multiply by.
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        public BigFraction Multiply(int i)
        {
            return Multiply(new BigInteger(i));
        }

        /// <summary>
        /// <p>
        /// Multiply the value of this fraction by the passed <i>long</i>,
        /// returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="l"></param>
        ///            the <i>long</i> to multiply by.
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        public BigFraction Multiply(long l)
        {
            return Multiply(new BigInteger(l));
        }

        /// <summary>
        /// <p>
        /// Multiplies the value of this fraction by another, returning the result in
        /// reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="fraction">Fraction to multiply by, must not be {@code null}.</param>
        /// <returns>a {@link BigFraction} instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException">if {@code fraction} is {@code null}d </exception>
        public BigFraction Multiply(BigFraction fraction)
        {
            if (fraction == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FRACTION);
            }
            if (numerator.Equals(BigInteger.Zero) ||
                fraction.numerator.Equals(BigInteger.Zero))
            {
                return ZERO;
            }
            return new BigFraction(numerator.Multiply(fraction.numerator),
                                   denominator.Multiply(fraction.denominator));
        }

        /// <summary>
        /// <p>
        /// Return the additive inverse of this fraction, returning the result in
        /// reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the negation of this fraction.</returns>
        public BigFraction Negate
        {
            get { return new BigFraction(numerator.Negate(), denominator); }
        }

        /// <summary>
        /// <p>
        /// Gets the fraction percentage as a <i>double</i>d This calculates the
        /// fraction as the numerator divided by denominator multiplied by 100d
        /// </p>
        /// 
        /// </summary>
        /// <returns>the fraction percentage as a <i>double</i>.</returns>
        public double PercentageValue()
        {
            return (double)(numerator.Divide(denominator)).Multiply(ONE_HUNDRED_DOUBLE);
        }

        /// <summary>
        /// <p>
        /// Returns a <i>integer</i> whose value is
        /// <i>(this<sup>exponent</sup>)</i>, returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="exponent"></param>
        ///            exponent to which this <code>BigInteger</code> is to be
        ///            raised.
        /// <returns><i>this<sup>exponent</sup></i>.</returns>
        public BigFraction Pow(int exponent)
        {
            if (exponent < 0)
            {
                return new BigFraction(denominator.Pow(-exponent), numerator.Pow(-exponent));
            }
            return new BigFraction(numerator.Pow(exponent), denominator.Pow(exponent));
        }

        /// <summary>
        /// <p>
        /// Returns a <code>BigFraction</code> whose value is
        /// <i>(this<sup>exponent</sup>)</i>, returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="exponent"></param>
        ///            exponent to which this <code>BigFraction</code> is to be raised.
        /// <returns><i>this<sup>exponent</sup></i> as a <code>BigFraction</code>.</returns>
        public BigFraction Pow(long exponent)
        {
            if (exponent < 0)
            {
                return new BigFraction(BigInteger.Pow(denominator, (int)-exponent),
                                       BigInteger.Pow(numerator, (int)-exponent));
            }
            return new BigFraction(BigInteger.Pow(numerator, (int)exponent),
                                   BigInteger.Pow(denominator, (int)exponent));
        }

        /// <summary>
        /// <p>
        /// Returns a <code>BigFraction</code> whose value is
        /// <i>(this<sup>exponent</sup>)</i>, returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="exponent"></param>
        ///            exponent to which this <code>BigFraction</code> is to be raised.
        /// <returns><i>this<sup>exponent</sup></i> as a <code>BigFraction</code>.</returns>
        public BigFraction Pow(BigInteger exponent)
        {
            if (exponent.CompareTo(BigInteger.Zero) < 0)
            {
                BigInteger eNeg = exponent.Negate();
                return new BigFraction(BigInteger.Pow(denominator, (int)eNeg),
                                       BigInteger.Pow(numerator, (int)eNeg));
            }
            return new BigFraction(BigInteger.Pow(numerator, (int)exponent),
                                   BigInteger.Pow(denominator, (int)exponent));
        }

        /// <summary>
        /// <p>
        /// Returns a <code>double</code> whose value is
        /// <i>(this<sup>exponent</sup>)</i>, returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="exponent"></param>
        ///            exponent to which this <code>BigFraction</code> is to be raised.
        /// <returns><i>this<sup>exponent</sup></i>.</returns>
        public double Pow(double exponent)
        {
            return System.Math.Pow((double)numerator, exponent) /
                   System.Math.Pow((double)denominator, exponent);
        }

        /// <summary>
        /// <p>
        /// Return the multiplicative inverse of this fraction.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the reciprocal fraction.</returns>
        public BigFraction Reciprocal()
        {
            return new BigFraction(denominator, numerator);
        }

        /// <summary>
        /// <p>
        /// Reduce this <code>BigFraction</code> to its lowest terms.
        /// </p>
        /// 
        /// </summary>
        /// <returns>the reduced <code>BigFraction</code>d It doesn't change anything if</returns>
        ///         the fraction can be reduced.
        public BigFraction Reduce()
        {
            BigInteger gcd = numerator.GreatestCommonDivisor(denominator);
            return new BigFraction(numerator.Divide(gcd), denominator.Divide(gcd));
        }

        /// <summary>
        /// <p>
        /// Subtracts the value of an {@link BigInteger} from the value of this one,
        /// returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="bg">the {@link BigInteger} to subtract, cannot be {@code null}.</param>
        /// <returns>a {@code BigFraction} instance with the resulting values.</returns>
        /// <exception cref="NullReferenceException">if the {@link BigInteger} is {@code null}d </exception>
        public BigFraction Subtract(BigInteger bg)
        {
            if (bg == null)
            {
                throw new NullReferenceException();
            }
            return new BigFraction(numerator.Subtract(denominator.Multiply(bg)), denominator);
        }

        /// <summary>
        /// <p>
        /// Subtracts the value of an <i>integer</i> from the value of this one,
        /// returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="i"></param>
        ///            the <i>integer</i> to subtract.
        /// <returns>a <code>BigFraction</code> instance with the resulting values.</returns>
        public BigFraction Subtract(int i)
        {
            return Subtract(new BigInteger(i));
        }

        /// <summary>
        /// <p>
        /// Subtracts the value of an <i>integer</i> from the value of this one,
        /// returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="l"></param>
        ///            the <i>long</i> to subtract.
        /// <returns>a <code>BigFraction</code> instance with the resulting values, or</returns>
        ///         this object if the <i>long</i> is zero.
        public BigFraction Subtract(long l)
        {
            return Subtract(new BigInteger(l));
        }

        /// <summary>
        /// <p>
        /// Subtracts the value of another fraction from the value of this one,
        /// returning the result in reduced form.
        /// </p>
        /// 
        /// </summary>
        /// <param Name="fraction">{@link BigFraction} to subtract, must not be {@code null}.</param>
        /// <returns>a {@link BigFraction} instance with the resulting values</returns>
        /// <exception cref="NullReferenceException">if the {@code fraction} is {@code null}d </exception>
        public BigFraction Subtract(BigFraction fraction)
        {
            if (fraction == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FRACTION);
            }
            if (ZERO.Equals(fraction))
            {
                return this;
            }

            BigInteger num = BigInteger.Zero;
            BigInteger den = BigInteger.Zero;
            if (denominator.Equals(fraction.denominator))
            {
                num = numerator.Subtract(fraction.numerator);
                den = denominator;
            }
            else
            {
                num = (numerator.Multiply(fraction.denominator)).Subtract((fraction.numerator).Multiply(denominator));
                den = denominator.Multiply(fraction.denominator);
            }
            return new BigFraction(num, den);
        }
    }
}
