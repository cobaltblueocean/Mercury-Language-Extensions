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
    /// Fraction Description
    /// </summary>
    [Serializable]
    public class Fraction : IComparable<Fraction>
    {

        /// <summary>A fraction representing "2 / 1"d */
        public static Fraction TWO = new Fraction(2, 1);

        /// <summary>A fraction representing "1"d */
        public static Fraction ONE = new Fraction(1, 1);

        /// <summary>A fraction representing "0"d */
        public static Fraction ZERO = new Fraction(0, 1);

        /// <summary>A fraction representing "4/5"d */
        public static Fraction FOUR_FIFTHS = new Fraction(4, 5);

        /// <summary>A fraction representing "1/5"d */
        public static Fraction ONE_FIFTH = new Fraction(1, 5);

        /// <summary>A fraction representing "1/2"d */
        public static Fraction ONE_HALF = new Fraction(1, 2);

        /// <summary>A fraction representing "1/4"d */
        public static Fraction ONE_QUARTER = new Fraction(1, 4);

        /// <summary>A fraction representing "1/3"d */
        public static Fraction ONE_THIRD = new Fraction(1, 3);

        /// <summary>A fraction representing "3/5"d */
        public static Fraction THREE_FIFTHS = new Fraction(3, 5);

        /// <summary>A fraction representing "3/4"d */
        public static Fraction THREE_QUARTERS = new Fraction(3, 4);

        /// <summary>A fraction representing "2/5"d */
        public static Fraction TWO_FIFTHS = new Fraction(2, 5);

        /// <summary>A fraction representing "2/4"d */
        public static Fraction TWO_QUARTERS = new Fraction(2, 4);

        /// <summary>A fraction representing "2/3"d */
        public static Fraction TWO_THIRDS = new Fraction(2, 3);

        /// <summary>A fraction representing "-1 / 1"d */
        public static Fraction MINUS_ONE = new Fraction(-1, 1);


        /// <summary>The denominatord */
        private int denominator;

        /// <summary>The numeratord */
        private int numerator;

        /// <summary>
        /// Create a fraction given the double value.
        /// </summary>
        /// <param Name="value">the double value to convert to a fraction.</param>
        /// <exception cref="FractionConversionException">if the continued fraction failed to </exception>
        ///         converge.
        public Fraction(double value) : this(value, 1.0e-5, 100)
        {

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
        /// </summary>
        /// <param Name="value">the double value to convert to a fraction.</param>
        /// <param Name="epsilon">maximum error allowedd  The resulting fraction is within</param>
        ///        <code>epsilon</code> of <code>value</code>, in absolute terms.
        /// <param Name="maxIterations">maximum number of convergents</param>
        /// <exception cref="FractionConversionException">if the continued fraction failed to </exception>
        ///         converge.
        public Fraction(double value, double epsilon, int maxIterations) : this(value, epsilon, Int32.MaxValue, maxIterations)
        {

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
        /// </summary>
        /// <param Name="value">the double value to convert to a fraction.</param>
        /// <param Name="maxDenominator">The maximum allowed value for denominator</param>
        /// <exception cref="FractionConversionException">if the continued fraction failed to </exception>
        ///         converge
        public Fraction(double value, int maxDenominator) : this(value, 0, maxDenominator, 100)
        {

        }

        /// <summary>
        /// Create a fraction given the double value and either the maximum error
        /// allowed or the maximum number of denominator digits.
        /// <p>
        /// 
        /// NOTE: This constructor is called with EITHER
        ///   - a valid epsilon value and the maxDenominator set to Int32.MaxValue
        ///     (that way the maxDenominator has no effect).
        /// OR
        ///   - a valid maxDenominator value and the epsilon value set to zero
        ///     (that way epsilon only has effect if there is an exact match before
        ///     the maxDenominator value is reached).
        /// </p><p>
        /// 
        /// It has been done this way so that the same code can be (re)used for both
        /// scenariosd However this could be confusing to users if it were part of
        /// the public API and this constructor should therefore remain PRIVATE.
        /// </p>
        /// 
        /// See JIRA issue ticket MATH-181 for more details:
        /// 
        ///     https://issues.apache.org/jira/browse/MATH-181
        /// 
        /// </summary>
        /// <param Name="value">the double value to convert to a fraction.</param>
        /// <param Name="epsilon">maximum error allowedd  The resulting fraction is within</param>
        ///        <code>epsilon</code> of <code>value</code>, in absolute terms.
        /// <param Name="maxDenominator">maximum denominator value allowed.</param>
        /// <param Name="maxIterations">maximum number of convergents</param>
        /// <exception cref="FractionConversionException">if the continued fraction failed to </exception>
        ///         converge.
        private Fraction(double value, double epsilon, int maxDenominator, int maxIterations)
        {
            long overflow = Int32.MaxValue;
            double r0 = value;
            long a0 = (long)System.Math.Floor(r0);
            if (a0 > overflow)
            {
                throw new FractionConversionException(value, a0, 1L);
            }

            // check AutoParallel.AutoParallelForEach(almost) integer arguments, which should not go
            // to iterations.
            if (System.Math.Abs(a0 - value) < epsilon)
            {
                this.numerator = (int)a0;
                this.denominator = 1;
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
            do
            {
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
                if (n < maxIterations && System.Math.Abs(convergent - value) > epsilon && q2 < maxDenominator)
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

            if (n >= maxIterations)
            {
                throw new FractionConversionException(value, maxIterations);
            }

            if (q2 < maxDenominator)
            {
                this.numerator = (int)p2;
                this.denominator = (int)q2;
            }
            else
            {
                this.numerator = (int)p1;
                this.denominator = (int)q1;
            }

        }

        /// <summary>
        /// Create a fraction given the numerator and denominatord  The fraction is
        /// reduced to lowest terms.
        /// </summary>
        /// <param Name="num">the numerator.</param>
        /// <param Name="den">the denominator.</param>
        /// <exception cref="ArithmeticException">if the denominator is <code>zero</code> </exception>
        public Fraction(int num) : this(num, 1)
        {

        }

        /// <summary>
        /// Create a fraction given the numerator and denominatord  The fraction is
        /// reduced to lowest terms.
        /// </summary>
        /// <param Name="num">the numerator.</param>
        /// <param Name="den">the denominator.</param>
        /// <exception cref="ArithmeticException">if the denominator is <code>zero</code> </exception>
        public Fraction(int num, int den)
        {
            if (den == 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().ZERO_DENOMINATOR_IN_FRACTION, num, den));
            }
            if (den < 0)
            {
                if (num == Int32.MinValue || den == Int32.MinValue)
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_FRACTION, num, den));
                }
                num = -num;
                den = -den;
            }
            // reduce numerator and denominator by greatest common denominator.
            int d = Math2.GreatestCommonDivisor(num, den);
            if (d > 1)
            {
                num /= d;
                den /= d;
            }

            // move sign to numerator.
            if (den < 0)
            {
                num = -num;
                den = -den;
            }
            this.numerator = num;
            this.denominator = den;
        }

        #region Operator and Method
        /// <summary>
        /// Returns the absolute value of this fraction.
        /// </summary>
        /// <returns>the absolute value.</returns>
        public Fraction Abs()
        {
            Fraction ret;
            if (numerator >= 0)
            {
                ret = this;
            }
            else
            {
                ret = Negate;
            }
            return ret;
        }


        /// <summary>
        /// Gets the fraction as a <i>double</i>d This calculates the fraction as
        /// the numerator divided by denominator.
        /// </summary>
        /// <returns>the fraction as a <i>double</i></returns>
        public double Value()
        {
            return (double)numerator / (double)denominator;
        }

        /// <summary>
        /// Test for the equality of two fractionsd  If the lowest term
        /// numerator and denominators are the same for both fractions, the two
        /// fractions are considered to be equal.
        /// </summary>
        /// <param Name="other">fraction to test for equality to this fraction</param>
        /// <returns>true if two fractions are equal, false if object is</returns>
        ///         <i>null</i>, not an instance of {@link Fraction}, or not equal
        ///         to this fraction instance.


        /// <summary>
        /// Return the additive inverse of this fraction.
        /// </summary>
        /// <returns>the negation of this fraction.</returns>
        public Fraction Negate
        {
            get
            {
                if (numerator == Int32.MinValue)
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_FRACTION, numerator, denominator));
                }
                return new Fraction(-numerator, denominator);
            }
        }

        /// <summary>
        /// Return the multiplicative inverse of this fraction.
        /// </summary>
        /// <returns>the reciprocal fraction</returns>
        public Fraction Reciprocal()
        {
            return new Fraction(denominator, numerator);
        }

        /// <summary>
        /// <p>Adds the value of this fraction to another, returning the result in reduced form.
        /// The algorithm follows Knuth, 4.5.1d</p>
        /// 
        /// </summary>
        /// <param Name="fraction"> the fraction to add, must not be <code>null</code></param>
        /// <returns>a <code>Fraction</code> instance with the resulting values</returns>
        /// <exception cref="ArgumentException">if the fraction is <code>null</code> </exception>
        /// <exception cref="ArithmeticException">if the resulting numerator or denominator exceeds </exception>
        ///  <code>Int32.MaxValue</code>
        public Fraction Add(Fraction fraction)
        {
            return AddSub(fraction, true /* add */);
        }

        /// <summary>
        /// Add an integer to the fraction.
        /// </summary>
        /// <param Name="i">the <i>integer</i> to add.</param>
        /// <returns>this + i</returns>
        public Fraction Add(int i)
        {
            return new Fraction(numerator + i * denominator, denominator);
        }

        /// <summary>
        /// <p>Subtracts the value of another fraction from the value of this one,
        /// returning the result in reduced form.</p>
        /// 
        /// </summary>
        /// <param Name="fraction"> the fraction to subtract, must not be <code>null</code></param>
        /// <returns>a <code>Fraction</code> instance with the resulting values</returns>
        /// <exception cref="ArgumentException">if the fraction is <code>null</code> </exception>
        /// <exception cref="ArithmeticException">if the resulting numerator or denominator </exception>
        ///   cannot be represented in an <code>int</code>.
        public Fraction Subtract(Fraction fraction)
        {
            return AddSub(fraction, false /* subtract */);
        }

        /// <summary>
        /// Subtract an integer from the fraction.
        /// </summary>
        /// <param Name="i">the <i>integer</i> to subtract.</param>
        /// <returns>this - i</returns>
        public Fraction Subtract(int i)
        {
            return new Fraction(numerator - i * denominator, denominator);
        }

        /// <summary>
        /// Implement add and subtract using algorithm described in Knuth 4.5.1d
        /// 
        /// </summary>
        /// <param Name="fraction">the fraction to subtract, must not be <code>null</code></param>
        /// <param Name="isAdd">true to add, false to subtract</param>
        /// <returns>a <code>Fraction</code> instance with the resulting values</returns>
        /// <exception cref="ArgumentException">if the fraction is <code>null</code> </exception>
        /// <exception cref="ArithmeticException">if the resulting numerator or denominator </exception>
        ///   cannot be represented in an <code>int</code>.
        private Fraction AddSub(Fraction fraction, Boolean isAdd)
        {
            if (fraction == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().FRACTION);
            }
            // zero is identity for addition.
            if (numerator == 0)
            {
                return isAdd ? fraction : fraction.Negate;
            }
            if (fraction.numerator == 0)
            {
                return this;
            }
            // if denominators are randomly distributed, d1 will be 1 about 61%
            // of the time.
            int d1 = Math2.GreatestCommonDivisor(denominator, fraction.denominator);
            if (d1 == 1)
            {
                // result is ( (u*v' +/- u'v) / u'v')
                int uvpi = Math2.MultiplyAndCheck(numerator, fraction.denominator);
                int upvi = Math2.MultiplyAndCheck(fraction.numerator, denominator);
                return new Fraction
                    (isAdd ? Math2.AddAndCheck(uvpi, upvi) :
                     Math2.SubAndCheck(uvpi, upvi),
                     Math2.MultiplyAndCheck(denominator, fraction.denominator));
            }
            // the quantity 't' requires 65 bits of precision; see knuth 4.5.1
            // exercise 7d  we're going to use a BigInteger.
            // t = u(v'/d1) +/- v(u'/d1)
            BigInteger uvp = BigInteger.Multiply(new BigInteger(numerator), new BigInteger(fraction.denominator / d1));
            BigInteger upv = BigInteger.Multiply(new BigInteger(fraction.numerator), new BigInteger(denominator / d1));
            BigInteger t = isAdd ? BigInteger.Add(uvp, upv) : BigInteger.Subtract(uvp, upv);
            // but d2 doesn't need extra precision because
            // d2 = GreatestCommonDivisor(t,d1) = GreatestCommonDivisor(t mod d1, d1)
            int tmodd1 = (int)t.Mod(new BigInteger(d1));
            long d2 = (tmodd1 == 0) ? d1 : Math2.GreatestCommonDivisor(tmodd1, d1);

            // result is (t/d2) / (u'/d1)(v'/d2)
            BigInteger w = BigInteger.Divide(t, new BigInteger(d2));
            if (w.BitLength() > 31)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().NUMERATOR_OVERFLOW_AFTER_MULTIPLY, w));
            }
            return new Fraction((int)w, Math2.MultiplyAndCheck(denominator / d1, fraction.denominator / (int)d2));
        }

        /// <summary>
        /// <p>Multiplies the value of this fraction by another, returning the
        /// result in reduced form.</p>
        /// 
        /// </summary>
        /// <param Name="fraction"> the fraction to multiply by, must not be <code>null</code></param>
        /// <returns>a <code>Fraction</code> instance with the resulting values</returns>
        /// <exception cref="ArgumentException">if the fraction is <code>null</code> </exception>
        /// <exception cref="ArithmeticException">if the resulting numerator or denominator exceeds </exception>
        ///  <code>Int32.MaxValue</code>
        public Fraction Multiply(Fraction fraction)
        {
            if (fraction == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().FRACTION);
            }
            if (numerator == 0 || fraction.numerator == 0)
            {
                return ZERO;
            }
            // knuth 4.5.1
            // make sure we don't overflow unless the result *must* overflow.
            int d1 = Math2.GreatestCommonDivisor(numerator, fraction.denominator);
            int d2 = Math2.GreatestCommonDivisor(fraction.numerator, denominator);
            return getReducedFraction
            (Math2.MultiplyAndCheck(numerator / d1, fraction.numerator / d2),
                    Math2.MultiplyAndCheck(denominator / d2, fraction.denominator / d1));
        }

        /// <summary>
        /// Multiply the fraction by an integer.
        /// </summary>
        /// <param Name="i">the <i>integer</i> to multiply by.</param>
        /// <returns>this * i</returns>
        public Fraction Multiply(int i)
        {
            return new Fraction(numerator * i, denominator);
        }

        /// <summary>
        /// <p>Divide the value of this fraction by another.</p>
        /// 
        /// </summary>
        /// <param Name="fraction"> the fraction to divide by, must not be <code>null</code></param>
        /// <returns>a <code>Fraction</code> instance with the resulting values</returns>
        /// <exception cref="ArgumentException">if the fraction is <code>null</code> </exception>
        /// <exception cref="ArithmeticException">if the fraction to divide by is zero </exception>
        /// <exception cref="ArithmeticException">if the resulting numerator or denominator exceeds </exception>
        ///  <code>Int32.MaxValue</code>
        public Fraction Divide(Fraction fraction)
        {
            if (fraction == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().FRACTION);
            }
            if (fraction.numerator == 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().ZERO_FRACTION_TO_DIVIDE_BY, fraction.numerator, fraction.denominator));
            }
            return Multiply(fraction.Reciprocal());
        }

        /// <summary>
        /// Divide the fraction by an integer.
        /// </summary>
        /// <param Name="i">the <i>integer</i> to divide by.</param>
        /// <returns>this * i</returns>
        public Fraction Divide(int i)
        {
            return new Fraction(numerator, denominator * i);
        }

        /// <summary>
        /// <p>Creates a <code>Fraction</code> instance with the 2 parts
        /// of a fraction Y/Z.</p>
        /// 
        /// <p>Any negative signs are resolved to be on the numerator.</p>
        /// 
        /// </summary>
        /// <param Name="numerator"> the numerator, for example the three in 'three sevenths'</param>
        /// <param Name="denominator"> the denominator, for example the seven in 'three sevenths'</param>
        /// <returns>a new fraction instance, with the numerator and denominator reduced</returns>
        /// <exception cref="ArithmeticException">if the denominator is <code>zero</code> </exception>
        public static Fraction getReducedFraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new MathArithmeticException(String.Format(LocalizedResources.Instance().ZERO_DENOMINATOR_IN_FRACTION, numerator, denominator));
            }
            if (numerator == 0)
            {
                return ZERO; // normalize zero.
            }
            // allow 2^k/-2^31 as a valid fraction (where k>0)
            if (denominator == Int32.MinValue && (numerator & 1) == 0)
            {
                numerator /= 2; denominator /= 2;
            }
            if (denominator < 0)
            {
                if (numerator == Int32.MinValue ||
                        denominator == Int32.MinValue)
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().OVERFLOW_IN_FRACTION, numerator, denominator));
                }
                numerator = -numerator;
                denominator = -denominator;
            }
            // simplify fraction.
            int gcd = Math2.GreatestCommonDivisor(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
            return new Fraction(numerator, denominator);
        }
        #endregion;

        public int CompareTo(Fraction other)
        {
            long nOd = ((long)numerator) * other.denominator;
            long dOn = ((long)denominator) * other.numerator;
            return (nOd < dOn) ? -1 : ((nOd > dOn) ? +1 : 0);
        }
    }
}
