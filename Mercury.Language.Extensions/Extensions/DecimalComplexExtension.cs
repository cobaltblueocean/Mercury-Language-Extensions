// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// <copyright file="DecimalComplexExtensions.cs" company="QuickMath.NET">
// QuickMath.NET Numerics, part of the QuickMath.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2010 QuickMath.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Globalization;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using DecimalComplex = System.Numerics.DecimalComplex;
using System.Runtime;
using Mercury.Language.Math;
using Mercury.Language.Exceptions;
using Mercury.Language;

namespace MathNet.Numerics
{

    /// <summary>
    /// Extension methods for the DecimalComplex type provided by System.Numerics.DecimalDecimalComplex
    /// </summary>
    public static class DecimalComplexExtension
    {
        /// <summary>
        /// Gets the squared magnitude of the <c>DecimalComplex</c> number.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="Complex32"/> number to perform this operation on.</param>
        /// <returns>The squared magnitude of the <c>DecimalComplex</c> number.</returns>
        public static float MagnitudeSquared(this Complex32 DecimalComplex)
        {
            return (DecimalComplex.Real * DecimalComplex.Real) + (DecimalComplex.Imaginary * DecimalComplex.Imaginary);
        }

        /// <summary>
        /// Gets the squared magnitude of the <c>DecimalComplex</c> number.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>The squared magnitude of the <c>DecimalComplex</c> number.</returns>
        public static decimal MagnitudeSquared(this DecimalComplex DecimalComplex)
        {
            return (DecimalComplex.Real * DecimalComplex.Real) + (DecimalComplex.Imaginary * DecimalComplex.Imaginary);
        }

        /// <summary>
        /// Gets the unity of this DecimalComplex (same argument, but on the unit circle; exp(I*arg))
        /// </summary>
        /// <returns>The unity of this <c>DecimalComplex</c>.</returns>
        public static DecimalComplex Sign(this DecimalComplex decimalComplex)
        {
            //if (decimal.IsPositiveInfinity(DecimalComplex.Real) && decimal.IsPositiveInfinity(DecimalComplex.Imaginary))
            //{
            //    return new DecimalComplex(Constants.Sqrt1Over2, Constants.Sqrt1Over2);
            //}

            //if (decimal.IsPositiveInfinity(DecimalComplex.Real) && decimal.IsNegativeInfinity(DecimalComplex.Imaginary))
            //{
            //    return new DecimalComplex(Constants.Sqrt1Over2, -Constants.Sqrt1Over2);
            //}

            //if (decimal.IsNegativeInfinity(DecimalComplex.Real) && decimal.IsPositiveInfinity(DecimalComplex.Imaginary))
            //{
            //    return new DecimalComplex(-Constants.Sqrt1Over2, -Constants.Sqrt1Over2);
            //}

            //if (decimal.IsNegativeInfinity(DecimalComplex.Real) && decimal.IsNegativeInfinity(DecimalComplex.Imaginary))
            //{
            //    return new DecimalComplex(-Constants.Sqrt1Over2, Constants.Sqrt1Over2);
            //}

            // don't replace this with "Magnitude"!
            var mod = SpecialFunctions2.Hypotenuse(decimalComplex.Real, decimalComplex.Imaginary);
            if (mod == 0.0M)
            {
                return DecimalComplex.Zero;
            }

            return new DecimalComplex(decimalComplex.Real / mod, decimalComplex.Imaginary / mod);
        }

        /// <summary>
        /// Gets the conjugate of the <c>DecimalComplex</c> number.
        /// </summary>
        /// <param name="decimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <remarks>
        /// The semantic of <i>setting the conjugate</i> is such that
        /// <code>
        /// // a, b of type Complex32
        /// a.Conjugate = b;
        /// </code>
        /// is equivalent to
        /// <code>
        /// // a, b of type Complex32
        /// a = b.Conjugate
        /// </code>
        /// </remarks>
        /// <returns>The conjugate of the <see cref="DecimalComplex"/> number.</returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static DecimalComplex Conjugate(this DecimalComplex decimalComplex)
        {
            return DecimalComplex.Conjugate(decimalComplex);
        }

        /// <summary>
        /// Returns the multiplicative inverse of a DecimalComplex number.
        /// </summary>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static DecimalComplex Reciprocal(this DecimalComplex decimalComplex)
        {
            return DecimalComplex.Reciprocal(decimalComplex);
        }

        /// <summary>
        /// Exponential of this <c>DecimalComplex</c> (exp(x), E^x).
        /// </summary>
        /// <param name="decimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        /// The exponential of this DecimalComplex number.
        /// </returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static DecimalComplex Exp(this DecimalComplex decimalComplex)
        {
            return DecimalComplex.Exp(decimalComplex);
        }

        /// <summary>
        /// Natural Logarithm of this <c>DecimalComplex</c> (Base E).
        /// </summary>
        /// <param name="decimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        /// The natural logarithm of this DecimalComplex number.
        /// </returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static DecimalComplex Ln(this DecimalComplex decimalComplex)
        {
            return DecimalComplex.Log(decimalComplex);
        }

        /// <summary>
        /// Common Logarithm of this <c>DecimalComplex</c> (Base 10).
        /// </summary>
        /// <returns>The common logarithm of this DecimalComplex number.</returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static DecimalComplex Log10(this DecimalComplex decimalComplex)
        {
            return DecimalComplex.Log10(decimalComplex);
        }

        /// <summary>
        /// Logarithm of this <c>DecimalComplex</c> with custom base.
        /// </summary>
        /// <returns>The logarithm of this DecimalComplex number.</returns>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static DecimalComplex Log(this DecimalComplex decimalComplex, decimal baseValue)
        {
            return DecimalComplex.Log(decimalComplex, baseValue);
        }

        /// <summary>
        /// Raise this <c>DecimalComplex</c> to the given value.
        /// </summary>
        /// <param name="decimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <param name="exponent">
        /// The exponent.
        /// </param>
        /// <returns>
        /// The DecimalComplex number raised to the given exponent.
        /// </returns>
        public static DecimalComplex Power(this DecimalComplex decimalComplex, DecimalComplex exponent)
        {
            if (decimalComplex.IsZero())
            {
                if (exponent.IsZero())
                {
                    return DecimalComplex.One;
                }

                if (exponent.Real > 0M)
                {
                    return DecimalComplex.Zero;
                }

                if (exponent.Real < 0M)
                {
                    //return exponent.Imaginary == 0M
                    //    ? new DecimalComplex(double.PositiveInfinity, 0M)
                    //    : new DecimalComplex(decimal.PositiveInfinity, decimal.PositiveInfinity);
                    throw new MathArgumentException(LocalizedResources.Instance().DECIMAL_COMPLEX_CANNOT_OPERATE_WITH_THIS_CONDITION, exponent);
                }
            }

            return DecimalComplex.Pow(decimalComplex, exponent);
        }

        /// <summary>
        /// Raise this <c>DecimalComplex</c> to the inverse of the given value.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <param name="rootExponent">
        /// The root exponent.
        /// </param>
        /// <returns>
        /// The DecimalComplex raised to the inverse of the given exponent.
        /// </returns>
        public static DecimalComplex Root(this DecimalComplex DecimalComplex, DecimalComplex rootExponent)
        {
            return DecimalComplex.Pow(DecimalComplex, 1 / rootExponent);
        }

        /// <summary>
        /// The Square (power 2) of this <c>DecimalComplex</c>
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        /// The square of this DecimalComplex number.
        /// </returns>
        public static DecimalComplex Square(this DecimalComplex DecimalComplex)
        {
            if (DecimalComplex.IsReal())
            {
                return new DecimalComplex(DecimalComplex.Real * DecimalComplex.Real, 0.0M);
            }

            return new DecimalComplex((DecimalComplex.Real * DecimalComplex.Real) - (DecimalComplex.Imaginary * DecimalComplex.Imaginary), 2 * DecimalComplex.Real * DecimalComplex.Imaginary);
        }

        /// <summary>
        /// The Square Root (power 1/2) of this <c>DecimalComplex</c>
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        /// The square root of this DecimalComplex number.
        /// </returns>
        public static DecimalComplex SquareRoot(this DecimalComplex DecimalComplex)
        {
            // Note: the following code should be equivalent to DecimalComplex.Sqrt(DecimalComplex),
            // but it turns out that is implemented poorly in System.Numerics,
            // hence we provide our own implementation here. Do not replace.

            if (DecimalComplex.IsRealNonNegative())
            {
                return new DecimalComplex(QuickMath.Sqrt(DecimalComplex.Real), 0.0M);
            }

            DecimalComplex result;

            var absReal = QuickMath.Abs(DecimalComplex.Real);
            var absImag = QuickMath.Abs(DecimalComplex.Imaginary);
            decimal w;
            if (absReal >= absImag)
            {
                var ratio = DecimalComplex.Imaginary / DecimalComplex.Real;
                w = QuickMath.Sqrt(absReal) * QuickMath.Sqrt(0.5M * (1.0M + QuickMath.Sqrt(1.0M + (ratio * ratio))));
            }
            else
            {
                var ratio = DecimalComplex.Real / DecimalComplex.Imaginary;
                w = QuickMath.Sqrt(absImag) * QuickMath.Sqrt(0.5M * (QuickMath.Abs(ratio) + QuickMath.Sqrt(1.0M + (ratio * ratio))));
            }

            if (DecimalComplex.Real >= 0.0M)
            {
                result = new DecimalComplex(w, DecimalComplex.Imaginary / (2.0M * w));
            }
            else if (DecimalComplex.Imaginary >= 0.0M)
            {
                result = new DecimalComplex(absImag / (2.0M * w), w);
            }
            else
            {
                result = new DecimalComplex(absImag / (2.0M * w), -w);
            }

            return result;
        }

        /// <summary>
        /// Evaluate all square roots of this <c>DecimalComplex</c>.
        /// </summary>
        public static (DecimalComplex, DecimalComplex) SquareRoots(this DecimalComplex DecimalComplex)
        {
            var principal = SquareRoot(DecimalComplex);
            return (principal, -principal);
        }

        /// <summary>
        /// Evaluate all cubic roots of this <c>DecimalComplex</c>.
        /// </summary>
        public static (DecimalComplex, DecimalComplex, DecimalComplex) CubicRoots(this DecimalComplex DecimalComplex)
        {
            var r = QuickMath.Pow(DecimalComplex.Magnitude, 1M / 3M);
            var theta = DecimalComplex.Phase / 3;
            const decimal shift = (decimal)Constants.Pi2 / 3M;
            return (DecimalComplex.FromPolarCoordinates(r, theta),
                DecimalComplex.FromPolarCoordinates(r, theta + shift),
                DecimalComplex.FromPolarCoordinates(r, theta - shift));
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is zero.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns><c>true</c> if this instance is zero; otherwise, <c>false</c>.</returns>
        public static bool IsZero(this DecimalComplex DecimalComplex)
        {
            return DecimalComplex.Real == 0.0M && DecimalComplex.Imaginary == 0.0M;
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is one.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns><c>true</c> if this instance is one; otherwise, <c>false</c>.</returns>
        public static bool IsOne(this DecimalComplex DecimalComplex)
        {
            return DecimalComplex.Real == 1.0M && DecimalComplex.Imaginary == 0.0M;
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is the imaginary unit.
        /// </summary>
        /// <returns><c>true</c> if this instance is ImaginaryOne; otherwise, <c>false</c>.</returns>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        public static bool IsImaginaryOne(this DecimalComplex DecimalComplex)
        {
            return DecimalComplex.Real == 0.0M && DecimalComplex.Imaginary == 1.0M;
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c>evaluates
        /// to a value that is not a number.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        /// <c>true</c> if this instance is <c>NaN</c>; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsNaN(this DecimalComplex DecimalComplex)
        {
            //return decimal.IsNaN(DecimalComplex.Real) || decimal.IsNaN(DecimalComplex.Imaginary);
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> evaluates to an
        /// infinite value.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        ///     <c>true</c> if this instance is infinite; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// True if it either evaluates to a DecimalComplex infinity
        /// or to a directed infinity.
        /// </remarks>
        public static bool IsInfinity(this DecimalComplex DecimalComplex)
        {
            //return decimal.IsInfinity(DecimalComplex.Real) || decimal.IsInfinity(DecimalComplex.Imaginary);
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> is real.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns><c>true</c> if this instance is a real number; otherwise, <c>false</c>.</returns>
        public static bool IsReal(this DecimalComplex DecimalComplex)
        {
            return DecimalComplex.Imaginary == 0.0M;
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> is real and not negative, that is &gt;= 0.
        /// </summary>
        /// <param name="DecimalComplex">The <see cref="DecimalComplex"/> number to perform this operation on.</param>
        /// <returns>
        ///     <c>true</c> if this instance is real nonnegative number; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRealNonNegative(this DecimalComplex DecimalComplex)
        {
            return DecimalComplex.Imaginary == 0.0M && DecimalComplex.Real >= 0;
        }

        /// <summary>
        /// Returns a Norm of a value of this type, which is appropriate for measuring how
        /// close this value is to zero.
        /// </summary>
        public static decimal Norm(this DecimalComplex DecimalComplex)
        {
            return DecimalComplex.MagnitudeSquared();
        }

        /// <summary>
        /// Returns a Norm of a value of this type, which is appropriate for measuring how
        /// close this value is to zero.
        /// </summary>
        public static float Norm(this Complex32 decimalComplex)
        {
            return decimalComplex.MagnitudeSquared;
        }

        /// <summary>
        /// Returns a Norm of the difference of two values of this type, which is
        /// appropriate for measuring how close together these two values are.
        /// </summary>
        public static decimal NormOfDifference(this DecimalComplex DecimalComplex, DecimalComplex otherValue)
        {
            return (DecimalComplex - otherValue).MagnitudeSquared();
        }

        /// <summary>
        /// Returns a Norm of the difference of two values of this type, which is
        /// appropriate for measuring how close together these two values are.
        /// </summary>
        public static float NormOfDifference(this Complex32 decimalComplex, Complex32 otherValue)
        {
            return (decimalComplex - otherValue).MagnitudeSquared;
        }

        /// <summary>
        /// Creates a DecimalComplex number based on a string. The string can be in the
        /// following formats (without the quotes): 'n', 'ni', 'n +/- ni',
        /// 'ni +/- n', 'n,n', 'n,ni,' '(n,n)', or '(n,ni)', where n is a decimal.
        /// </summary>
        /// <returns>
        /// A DecimalComplex number containing the value specified by the given string.
        /// </returns>
        /// <param name="value">
        /// The string to parse.
        /// </param>
        public static DecimalComplex ToDecimalComplex(this string value)
        {
            return value.ToDecimalComplex(CultureInfo.CurrentCulture.NumberFormat);
        }

        /// <summary>
        /// Creates a DecimalComplex number based on a string. The string can be in the
        /// following formats (without the quotes): 'n', 'ni', 'n +/- ni',
        /// 'ni +/- n', 'n,n', 'n,ni,' '(n,n)', or '(n,ni)', where n is a decimal.
        /// </summary>
        /// <returns>
        /// A DecimalComplex number containing the value specified by the given string.
        /// </returns>
        /// <param name="value">
        /// the string to parse.
        /// </param>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information.
        /// </param>
        public static DecimalComplex ToDecimalComplex(this string value, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            value = value.Trim();
            if (value.Length == 0)
            {
                throw new FormatException();
            }

            // strip out parens
            if (value.StartsWith("(", StringComparison.Ordinal))
            {
                if (!value.EndsWith(")", StringComparison.Ordinal))
                {
                    throw new FormatException();
                }

                value = value.Substring(1, value.Length - 2).Trim();
            }

            // keywords
            var numberFormatInfo = formatProvider.GetNumberFormatInfo();
            var textInfo = formatProvider.GetTextInfo();
            var keywords =
                new[]
                {
                    textInfo.ListSeparator, numberFormatInfo.NaNSymbol,
                    numberFormatInfo.NegativeInfinitySymbol, numberFormatInfo.PositiveInfinitySymbol,
                    "+", "-", "i", "j"
                };

            // lexing
            var tokens = new LinkedList<string>();
            GlobalizationHelper.Tokenize(tokens.AddFirst(value), keywords, 0);
            var token = tokens.First;

            // parse the left part
            var leftPart = ParsePart(ref token, out var isLeftPartImaginary, formatProvider);
            if (token == null)
            {
                return isLeftPartImaginary ? new DecimalComplex(0, leftPart) : new DecimalComplex(leftPart, 0);
            }

            // parse the right part
            if (token.Value == textInfo.ListSeparator)
            {
                // format: real,imag
                token = token.Next;

                if (isLeftPartImaginary)
                {
                    // left must not contain 'i', right doesn't matter.
                    throw new FormatException();
                }

                var rightPart = ParsePart(ref token, out _, formatProvider);

                return new DecimalComplex(leftPart, rightPart);
            }
            else
            {
                // format: real + imag
                var rightPart = ParsePart(ref token, out var isRightPartImaginary, formatProvider);

                if (!(isLeftPartImaginary ^ isRightPartImaginary))
                {
                    // either left or right part must contain 'i', but not both.
                    throw new FormatException();
                }

                return isLeftPartImaginary ? new DecimalComplex(rightPart, leftPart) : new DecimalComplex(leftPart, rightPart);
            }
        }

        /// <summary>
        /// Parse a part (real or DecimalComplex) from a DecimalComplex number.
        /// </summary>
        /// <param name="token">Start Token.</param>
        /// <param name="imaginary">Is set to <c>true</c> if the part identified itself as being imaginary.</param>
        /// <param name="format">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information.
        /// </param>
        /// <returns>Resulting part as decimal.</returns>
        /// <exception cref="FormatException"/>
        static decimal ParsePart(ref LinkedListNode<string> token, out bool imaginary, IFormatProvider format)
        {
            imaginary = false;
            if (token == null)
            {
                throw new FormatException();
            }

            // handle prefix modifiers
            if (token.Value == "+")
            {
                token = token.Next;

                if (token == null)
                {
                    throw new FormatException();
                }
            }

            var negative = false;
            if (token.Value == "-")
            {
                negative = true;
                token = token.Next;

                if (token == null)
                {
                    throw new FormatException();
                }
            }

            // handle prefix imaginary symbol
            if (String.Compare(token.Value, "i", StringComparison.OrdinalIgnoreCase) == 0
                || String.Compare(token.Value, "j", StringComparison.OrdinalIgnoreCase) == 0)
            {
                imaginary = true;
                token = token.Next;

                if (token == null)
                {
                    return negative ? -1 : 1;
                }
            }

            var value = GlobalizationHelper.ParseDecimal(ref token, format.GetCultureInfo());

            // handle suffix imaginary symbol
            if (token != null && (String.Compare(token.Value, "i", StringComparison.OrdinalIgnoreCase) == 0
                                  || String.Compare(token.Value, "j", StringComparison.OrdinalIgnoreCase) == 0))
            {
                if (imaginary)
                {
                    // only one time allowed: either prefix or suffix, or neither.
                    throw new FormatException();
                }

                imaginary = true;
                token = token.Next;
            }

            return negative ? -value : value;
        }

        /// <summary>
        /// Converts the string representation of a DecimalComplex number to a decimal-precision DecimalComplex number equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">
        /// A string containing a DecimalComplex number to convert.
        /// </param>
        /// <param name="result">
        /// The parsed value.
        /// </param>
        /// <returns>
        /// If the conversion succeeds, the result will contain a DecimalComplex number equivalent to value.
        /// Otherwise the result will contain DecimalComplex.Zero.  This parameter is passed uninitialized.
        /// </returns>
        public static bool TryToDecimalComplex(this string value, out DecimalComplex result)
        {
            return value.TryToDecimalComplex(CultureInfo.CurrentCulture.NumberFormat, out result);
        }

        /// <summary>
        /// Converts the string representation of a DecimalComplex number to decimal-precision DecimalComplex number equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">
        /// A string containing a DecimalComplex number to convert.
        /// </param>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about value.
        /// </param>
        /// <param name="result">
        /// The parsed value.
        /// </param>
        /// <returns>
        /// If the conversion succeeds, the result will contain a DecimalComplex number equivalent to value.
        /// Otherwise the result will contain Complex32.Zero.  This parameter is passed uninitialized
        /// </returns>
        public static bool TryToDecimalComplex(this string value, IFormatProvider formatProvider, out DecimalComplex result)
        {
            bool ret;
            try
            {
                result = value.ToDecimalComplex(formatProvider);
                ret = true;
            }
            catch (ArgumentNullException)
            {
                result = DecimalComplex.Zero;
                ret = false;
            }
            catch (FormatException)
            {
                result = DecimalComplex.Zero;
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Creates a <c>Complex32</c> number based on a string. The string can be in the
        /// following formats (without the quotes): 'n', 'ni', 'n +/- ni',
        /// 'ni +/- n', 'n,n', 'n,ni,' '(n,n)', or '(n,ni)', where n is a decimal.
        /// </summary>
        /// <returns>
        /// A DecimalComplex number containing the value specified by the given string.
        /// </returns>
        /// <param name="value">
        /// the string to parse.
        /// </param>
        public static Complex32 ToComplex32(this string value)
        {
            return Complex32.Parse(value);
        }

        /// <summary>
        /// Creates a <c>Complex32</c> number based on a string. The string can be in the
        /// following formats (without the quotes): 'n', 'ni', 'n +/- ni',
        /// 'ni +/- n', 'n,n', 'n,ni,' '(n,n)', or '(n,ni)', where n is a decimal.
        /// </summary>
        /// <returns>
        /// A DecimalComplex number containing the value specified by the given string.
        /// </returns>
        /// <param name="value">
        /// the string to parse.
        /// </param>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information.
        /// </param>
        public static Complex32 ToComplex32(this string value, IFormatProvider formatProvider)
        {
            return Complex32.Parse(value, formatProvider);
        }

        /// <summary>
        /// Converts the string representation of a DecimalComplex number to a single-precision DecimalComplex number equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">
        /// A string containing a DecimalComplex number to convert.
        /// </param>
        /// <param name="result">
        /// The parsed value.
        /// </param>
        /// <returns>
        /// If the conversion succeeds, the result will contain a DecimalComplex number equivalent to value.
        /// Otherwise the result will contain Complex32.Zero.  This parameter is passed uninitialized.
        /// </returns>
        public static bool TryToComplex32(this string value, out Complex32 result)
        {
            return Complex32.TryParse(value, out result);
        }

        /// <summary>
        /// Converts the string representation of a DecimalComplex number to single-precision DecimalComplex number equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">
        /// A string containing a DecimalComplex number to convert.
        /// </param>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about value.
        /// </param>
        /// <param name="result">
        /// The parsed value.
        /// </param>
        /// <returns>
        /// If the conversion succeeds, the result will contain a DecimalComplex number equivalent to value.
        /// Otherwise the result will contain DecimalComplex.Zero.  This parameter is passed uninitialized.
        /// </returns>
        public static bool TryToComplex32(this string value, IFormatProvider formatProvider, out Complex32 result)
        {
            return Complex32.TryParse(value, formatProvider, out result);
        }

        public static Complex[] ToComplexArray(this DecimalComplex[] value)
        {
            return value.Select(x => new Complex((double)x.Real, (double)x.Imaginary)).ToArray();
        }

        public static Complex32[] ToComplex32Array(this DecimalComplex[] value)
        {
            return value.Select(x => new Complex32((Single)x.Real, (Single)x.Imaginary)).ToArray();
        }

        /// <summary>
        /// Compares two doubles and determines if they are equal
        /// within the specified maximum absolute error.
        /// </summary>
        /// <param name="a">The norm of the first value (can be negative).</param>
        /// <param name="b">The norm of the second value (can be negative).</param>
        /// <param name="diff">The norm of the difference of the two values (can be negative).</param>
        /// <param name="maximumAbsoluteError">The absolute accuracy required for being almost equal.</param>
        /// <returns>True if both doubles are almost equal up to the specified maximum absolute error, false otherwise.</returns>
        private static bool AlmostEqualNorm(this decimal a, decimal b, decimal diff, double maximumAbsoluteError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            //if (double.IsInfinity(a) || double.IsInfinity(b))
            //{
            //    return a == b;
            //}

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            //if (double.IsNaN(a) || double.IsNaN(b))
            //{
            //    return false;
            //}

            return QuickMath.Abs(diff) < (decimal)maximumAbsoluteError;
        }

        /// <summary>
        /// Compares two complex and determines if they are equal within
        /// the specified maximum error.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="maximumAbsoluteError">The accuracy required for being almost equal.</param>
        public static bool AlmostEqual(this DecimalComplex a, DecimalComplex b, double maximumAbsoluteError)
        {
            return AlmostEqualNorm(a.Norm(), b.Norm(), a.NormOfDifference(b), maximumAbsoluteError);
        }

        /// <summary>
        /// Checks whether two Complex numbers are almost equal.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>true if the two values differ by no more than 10 * 2^(-52); false otherwise.</returns>
        public static bool AlmostEqual(this DecimalComplex a, DecimalComplex b)
        {
            return AlmostEqualNorm(a.Norm(), b.Norm(), a.NormOfDifference(b), Precision2.DefaultDoubleAccuracy);
        }
    }
}
