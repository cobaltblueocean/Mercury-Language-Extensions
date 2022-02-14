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

/// <summary>
/// Licensed to the Apache Software Foundation (ASF) under one or more
/// contributor license agreementsd  See the NOTICE file distributed with
/// this work for additional information regarding copyright ownership.
/// The ASF licenses this file to You under the Apache License, Version 2.0
/// (the "License"); you may not use this file except in compliance with
/// the Licensed  You may obtain a copy of the License at
/// 
///      http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// <summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis.Differentiation;

namespace Mercury.Language.Math.Analysis.Function
{
    using Math = System.Math;

    /// <summary>
    /// Functions Description
    /// </summary>
    public static partial class Functions
    {
        #region IUnivariateFunction
        /// <summary>
        /// Absolute function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Abs : IUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Abs(x);
            }
        }

        /// <summary>
        /// Ceil function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Ceil : IUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Ceiling(x);
            }
        }

        /// <summary>
        /// Floor function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Floor : IUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Floor(x);
            }
        }

        /// <summary>
        /// Rint function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Rint : IUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public double Value(double x)
            {
                _paramValue = x;
                return rint(x);
            }

            /// <summary>Get the whole number that is the nearest to x, or the even one if x is exactly half way between two ints.
            /// <summary>
            /// <param name="x">number from which nearest whole number is requested</param>
            /// <returns>a double number r such that r is an int r - 0.5 <= x <= r + 0.5</returns>
            public static double rint(double x)
            {
                double y = System.Math.Floor(x);
                double d = x - y;

                if (d > 0.5)
                {
                    if (y == -1.0)
                    {
                        return -0.0; // Preserve sign of operand
                    }
                    return y + 1.0;
                }
                if (d < 0.5)
                {
                    return y;
                }

                /* half way, round to even */
                long z = (long)y;
                return (z & 1) == 0 ? y : y + 1.0;
            }
        }

        /// <summary>
        /// Signum function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Signum : IUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public double Value(double x)
            {
                _paramValue = x;
                return (x < 0.0) ? -1.0 : ((x > 0.0) ? 1.0 : x); // return +0.0/-0.0/NaN depending on a
            }
        }

        /// <summary>
        /// Step function.
        /// 
        /// @since 3.0
        /// <summary>
        public class StepFunction : IUnivariateFunction
        {
            private double _paramValue;

            /// <summary>Abscissaed */
            private double[] abscissa;
            /// <summary>Ordinatesd */
            private double[] ordinate;
            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>
            /// Builds a step function from a list of arguments and the corresponding
            /// valuesd Specifically, returns the function h(x) defined by <pre><code>
            /// h(x) = y[0] for all x &lt; x[1]
            ///        y[1] for x[1] &le; x &lt; x[2]
            ///        ...
            ///        y[y.Length - 1] for x &ge; x[x.Length - 1]
            /// </code></pre>
            /// The value of {@code x[0]} is ignored, but it must be strictly less than
            /// {@code x[1]}.
            /// 
            /// <summary>
            /// <param name="x">Domain values where the function changes value.</param>
            /// <param name="y">Values of the function.</param>
            /// <exception cref="NonMonotonicSequenceException"></exception>
            /// if the {@code x} array is not sorted in strictly increasing order.
            /// <exception cref="ArgumentNullException">if {@code x} or {@code y} are {@code null}d </exception>
            /// <exception cref="DataNotFoundException">if {@code x} or {@code y} are zero-Lengthd </exception>
            /// <exception cref="DimensionMismatchException">if {@code x} and {@code y} do not </exception>
            /// have the same Length.
            public StepFunction(double[] x,
                                double[] y)
            {
                if (x == null ||
                    y == null)
                {
                    throw new ArgumentNullException();
                }
                if (x.Length == 0 ||
                    y.Length == 0)
                {
                    throw new DataNotFoundException("Given data is empty.");
                }
                if (y.Length != x.Length)
                {
                    throw new DimensionMismatchException(y.Length, x.Length);
                }
                x.CheckOrder();

                abscissa = x.CopyOf(x.Length);
                ordinate = y.CopyOf(y.Length);
            }

            public double Value(double x)
            {
                _paramValue = x;
                int index = Array.BinarySearch(abscissa, x);
                double fx = 0;

                if (index < -1)
                {
                    // "x" is between "abscissa[-index-2]" and "abscissa[-index-1]".
                    fx = ordinate[-index - 2];
                }
                else if (index >= 0)
                {
                    // "x" is exactly "abscissa[index]".
                    fx = ordinate[index];
                }
                else
                {
                    // Otherwise, "x" is smaller than the first value in "abscissa"
                    // (hence the returned value should be "ordinate[0]").
                    fx = ordinate[0];
                }

                return fx;
            }
        }

        /// <summary>
        /// Ulp function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Ulp : IUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public double Value(double x)
            {
                _paramValue = x;
                if (x.IsInfinite())
                {
                    return Double.PositiveInfinity;
                }
                return System.Math.Abs(x - BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) ^ 1));
            }
        }
        #endregion

        #region IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        /// <summary>
        /// Arc-cosine function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Acos : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Acos(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Acos();
            }

        }

        /// <summary>
        /// Hyperbolic arc-cosine function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Acosh : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                //return System.Math.Acosh(x);
                return System.Math.Log(x + System.Math.Sqrt(x * x - 1));
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated", true)]
            public UnivariateFunction Derivative()
            {
                //return FunctionBuilder.ToDifferentiableUnivariateFunction(this).derivative();
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Acosh();
            }
        }


        /// <summary>
        /// Arc-sine function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Asin : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Asin(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated", true)]
            public UnivariateFunction Derivative()
            {
                //return FunctionBuilder.ToDifferentiableUnivariateFunction(this).derivative();
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Asin();
            }
        }

        /// <summary>
        /// Hyperbolic arc-sine function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Asinh : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;

                //return System.Math.Asinh(x);
                Boolean negative = false;
                if (x < 0)
                {
                    negative = true;
                    x = -x;
                }

                double absAsinh;
                if (x > 0.167)
                {
                    absAsinh = System.Math.Log(System.Math.Sqrt(x * x + 1) + x);
                }
                else
                {
                    double a2 = x * x;
                    if (x > 0.097)
                    {
                        absAsinh = x * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * (F_1_9 - a2 * (F_1_11 - a2 * (F_1_13 - a2 * (F_1_15 - a2 * F_1_17 * F_15_16) * F_13_14) * F_11_12) * F_9_10) * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                    }
                    else if (x > 0.036)
                    {
                        absAsinh = x * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * (F_1_9 - a2 * (F_1_11 - a2 * F_1_13 * F_11_12) * F_9_10) * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                    }
                    else if (x > 0.0036)
                    {
                        absAsinh = x * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * F_1_9 * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                    }
                    else
                    {
                        absAsinh = x * (1 - a2 * (F_1_3 - a2 * F_1_5 * F_3_4) * F_1_2);
                    }
                }

                return negative ? -absAsinh : absAsinh;
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated", true)]
            public UnivariateFunction Derivative()
            {
                //return FunctionBuilder.ToDifferentiableUnivariateFunction(this).derivative();
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Asinh();
            }
        }

        /// <summary>
        /// Arc-tangent function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Atan : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Atan(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Atan();
            }
        }

        /// <summary>
        /// Hyperbolic arc-tangent function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Atanh : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;

                Boolean negative = false;
                if (x < 0)
                {
                    negative = true;
                    x = -x;
                }

                double absAtanh;
                if (x > 0.15)
                {
                    absAtanh = 0.5 * System.Math.Log((1 + x) / (1 - x));
                }
                else
                {
                    double a2 = x * x;
                    if (x > 0.087)
                    {
                        absAtanh = x * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * (F_1_9 + a2 * (F_1_11 + a2 * (F_1_13 + a2 * (F_1_15 + a2 * F_1_17))))))));
                    }
                    else if (x > 0.031)
                    {
                        absAtanh = x * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * (F_1_9 + a2 * (F_1_11 + a2 * F_1_13))))));
                    }
                    else if (x > 0.003)
                    {
                        absAtanh = x * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * F_1_9))));
                    }
                    else
                    {
                        absAtanh = x * (1 + a2 * (F_1_3 + a2 * F_1_5));
                    }
                }

                return negative ? -absAtanh : absAtanh;
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Atanh();
            }

        }

        /// <summary>
        /// Cube root function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Cbrt : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;

                /* Convert input double to bits */
                long inbits = BitConverter.DoubleToInt64Bits(x);
                int exponent = (int)((inbits >> 52) & 0x7ff) - 1023;
                Boolean subnormal = false;

                if (exponent == -1023)
                {
                    if (x == 0)
                    {
                        return x;
                    }

                    /* Subnormal, so normalize */
                    subnormal = true;
                    x *= 1.8014398509481984E16;  // 2^54
                    inbits = BitConverter.DoubleToInt64Bits(x);
                    exponent = (int)((inbits >> 52) & 0x7ff) - 1023;
                }

                if (exponent == 1024)
                {
                    // Nan or infinityd  Don't care which.
                    return x;
                }

                /* Divide the exponent by 3 */
                int exp3 = exponent / 3;

                //                                                   0xFFFFFFFFFFFFFFFF
                /* p2 will be the nearest power of 2 to x with its exponent divided by 3 */
                //double p2 = BitConverter.Int64BitsToDouble((inbits & 0x8000000000000000L) | (long)(((exp3 + 1023) & 0x7ff)) << 52);
                double p2 = ((ulong)inbits & 0x8000000000000000L) | (ulong)(((exp3 + 1023) & 0x7ff)) << 52;

                /* This will be a number between 1 and 2 */
                double mant = BitConverter.Int64BitsToDouble((inbits & 0x000fffffffffffffL) | 0x3ff0000000000000L);

                /* Estimate the cube root of mant by polynomial */
                double est = -0.010714690733195933;
                est = est * mant + 0.0875862700108075;
                est = est * mant + -0.3058015757857271;
                est = est * mant + 0.7249995199969751;
                est = est * mant + 0.5039018405998233;

                est *= CBRTTWO[exponent % 3 + 2];

                // est should now be good to about 15 bits of precisiond   Do 2 rounds of
                // Newton's method to get closer,  this should get us full double precision
                // Scale down x for the purpose of doing newtons methodd  This avoids over/under flows.
                double xs = x / (p2 * p2 * p2);
                est += (xs - est * est * est) / (3 * est * est);
                est += (xs - est * est * est) / (3 * est * est);

                // Do one round of Newton's method in extended precision to get the last bit right.
                double temp = est * HEX_40000000;
                double ya = est + temp - temp;
                double yb = est - ya;

                double za = ya * ya;
                double zb = ya * yb * 2.0 + yb * yb;
                temp = za * HEX_40000000;
                double temp2 = za + temp - temp;
                zb += za - temp2;
                za = temp2;

                zb = za * yb + ya * zb + zb * yb;
                za *= ya;

                double na = xs - za;
                double nb = -(na - xs + za);
                nb -= zb;

                est += (na + nb) / (3 * est * est);

                /* Scale by a power of two, so this is exactd */
                est *= p2;

                if (subnormal)
                {
                    est *= 3.814697265625E-6;  // 2^-18
                }

                return est;
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Cbrt();
            }

        }

        /// <summary>
        /// Constant Description
        /// </summary>
        public class Constant : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;
            public double ParamValue { get { return _paramValue; } }

            /// <summary>Constantd */
            private double c;

            /// <summary>
            /// <summary>
            /// <param name="c">Constant.</param>
            public Constant(double c)
            {
                this.c = c;
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                return (new Constant(0)).CastType<UnivariateFunction>();
            }

            public DerivativeStructure Value(DerivativeStructure t)
            {
                return new DerivativeStructure(t.FreeParameters, t.Order, c);
            }

            public double Value(double x)
            {
                _paramValue = x;
                return c;
            }
        }

        /// <summary>
        /// Cosign function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Cos : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Cos(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Cos();
            }
        }

        /// <summary>
        /// Hyperbolic cosign function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Cosh : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Cosh(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Cosh();
            }
        }

        /// <summary>
        /// Exponential function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Exp : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Exp(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Expo();
            }
        }

        /// <summary>
        /// <code>e<sup>x</sup>-1</code> function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Expm1 : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return expm1(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Expm1();
            }

            /// <summary>Compute exp(x) - 1
            /// <summary>
            /// <param name="x">number to compute shifted exponential</param>
            /// <returns>exp(x) - 1</returns>
            public static double expm1(double x)
            {
                return expm1(x, null);
            }

            /// <summary>Internal helper method for expm1
            /// <summary>
            /// <param name="x">number to compute shifted exponential</param>
            /// <param name="hiPrecOut">receive high precision result for -1.0 < x < 1.0</param>
            /// <returns>exp(x) - 1</returns>
            private static double expm1(double x, double[] hiPrecOut)
            {
                if (double.IsNaN(x) || x == 0.0)
                { // NaN or zero
                    return x;
                }

                if (x <= -1.0 || x >= 1.0)
                {
                    // If not between +/- 1.0
                    //return exp(x) - 1.0;
                    double[] hiPrec = new double[2];
                    exp(x, 0.0, hiPrec);
                    if (x > 0.0)
                    {
                        return -1.0 + hiPrec[0] + hiPrec[1];
                    }
                    else
                    {
                        double ra = -1.0 + hiPrec[0];
                        double rb = -(ra + 1.0 - hiPrec[0]);
                        rb += hiPrec[1];
                        return ra + rb;
                    }
                }

                double baseA;
                double baseB;
                double epsilon;
                Boolean negative = false;

                if (x < 0.0)
                {
                    x = -x;
                    negative = true;
                }

                {
                    int intFrac = (int)(x * 1024.0);
                    double tempA = EXP_FRAC_A[intFrac] - 1.0;
                    double tempB = EXP_FRAC_B[intFrac];

                    double temp1 = tempA + tempB;
                    tempB = -(temp1 - tempA - tempB);
                    tempA = temp1;

                    temp1 = tempA * HEX_40000000;
                    baseA = tempA + temp1 - temp1;
                    baseB = tempB + (tempA - baseA);

                    epsilon = x - intFrac / 1024.0;
                }


                /* Compute expm1(epsilon) */
                double zb = 0.008336750013465571;
                zb = zb * epsilon + 0.041666663879186654;
                zb = zb * epsilon + 0.16666666666745392;
                zb = zb * epsilon + 0.49999999999999994;
                zb *= epsilon;
                zb *= epsilon;

                double za = epsilon;
                double temp2 = za + zb;
                zb = -(temp2 - za - zb);
                za = temp2;

                temp2 = za * HEX_40000000;
                temp2 = za + temp2 - temp2;
                zb += za - temp2;
                za = temp2;

                /* Combine the partsd   expm1(a+b) = expm1(a) + expm1(b) + expm1(a)*expm1(b) */
                double ya = za * baseA;
                //double yb = za*baseB + zb*baseA + zb*baseB;
                temp2 = ya + za * baseB;
                double yb = -(temp2 - ya - za * baseB);
                ya = temp2;

                temp2 = ya + zb * baseA;
                yb += -(temp2 - ya - zb * baseA);
                ya = temp2;

                temp2 = ya + zb * baseB;
                yb += -(temp2 - ya - zb * baseB);
                ya = temp2;

                //ya = ya + za + baseA;
                //yb = yb + zb + baseB;
                temp2 = ya + baseA;
                yb += -(temp2 - baseA - ya);
                ya = temp2;

                temp2 = ya + za;
                //yb += (ya > za) ? -(temp - ya - za) : -(temp - za - ya);
                yb += -(temp2 - ya - za);
                ya = temp2;

                temp2 = ya + baseB;
                //yb += (ya > baseB) ? -(temp - ya - baseB) : -(temp - baseB - ya);
                yb += -(temp2 - ya - baseB);
                ya = temp2;

                temp2 = ya + zb;
                //yb += (ya > zb) ? -(temp - ya - zb) : -(temp - zb - ya);
                yb += -(temp2 - ya - zb);
                ya = temp2;

                if (negative)
                {
                    /* Compute expm1(-x) = -expm1(x) / (expm1(x) + 1) */
                    double denom = 1.0 + ya;
                    double denomr = 1.0 / denom;
                    double denomb = -(denom - 1.0 - ya) + yb;
                    double ratio = ya * denomr;
                    temp2 = ratio * HEX_40000000;
                    double ra = ratio + temp2 - temp2;
                    double rb = ratio - ra;

                    temp2 = denom * HEX_40000000;
                    za = denom + temp2 - temp2;
                    zb = denom - za;

                    rb += (ya - za * ra - za * rb - zb * ra - zb * rb) * denomr;

                    // f(x) = x/1+x
                    // Compute f'(x)
                    // Product rule:  d(uv) = du*v + u*dv
                    // Chain rule:  d(f(g(x)) = f'(g(x))*f(g'(x))
                    // d(1/x) = -1/(x*x)
                    // d(1/1+x) = -1/( (1+x)^2) *  1 =  -1/((1+x)*(1+x))
                    // d(x/1+x) = -x/((1+x)(1+x)) + 1/1+x = 1 / ((1+x)(1+x))

                    // Adjust for yb
                    rb += yb * denomr;                      // numerator
                    rb += -ya * denomb * denomr * denomr;   // denominator

                    // negate
                    ya = -ra;
                    yb = -rb;
                }

                if (hiPrecOut != null)
                {
                    hiPrecOut[0] = ya;
                    hiPrecOut[1] = yb;
                }

                return ya + yb;
            }

            /// <summary>
            /// Internal helper method for exponential function.
            /// <summary>
            /// <param name="x">original argument of the exponential function</param>
            /// <param name="extra">extra bits of precision on input (To Be Confirmed)</param>
            /// <param name="hiPrec">extra bits of precision on output (To Be Confirmed)</param>
            /// <returns>exp(x)</returns>
            private static double exp(double x, double extra, double[] hiPrec)
            {
                double intPartA;
                double intPartB;
                int intVal = (int)x;
                double result = 0;

                /* Lookup exp(floor(x)).
                 * intPartA will have the upper 22 bits, intPartB will have the lower
                 * 52 bits.
                 */
                if (x < 0.0)
                {

                    // We don't check against intVal here as conversion of large negative double values
                    // may be affected by a JIT bugd Subsequent comparisons can safely use intVal
                    if (x < -746d)
                    {
                        if (hiPrec != null)
                        {
                            hiPrec[0] = 0.0;
                            hiPrec[1] = 0.0;
                        }
                        return 0.0;
                    }

                    if (intVal < -709)
                    {
                        /* This will produce a subnormal output */
                        result = exp(x + 40.19140625, extra, hiPrec) / 285040095144011776.0;
                        if (hiPrec != null)
                        {
                            hiPrec[0] /= 285040095144011776.0;
                            hiPrec[1] /= 285040095144011776.0;
                        }
                        return result;
                    }

                    if (intVal == -709)
                    {
                        /* exp(1.494140625) is nearly a machine number..d */
                        result = exp(x + 1.494140625, extra, hiPrec) / 4.455505956692756620;
                        if (hiPrec != null)
                        {
                            hiPrec[0] /= 4.455505956692756620;
                            hiPrec[1] /= 4.455505956692756620;
                        }
                        return result;
                    }

                    intVal--;

                }
                else
                {
                    if (intVal > 709)
                    {
                        if (hiPrec != null)
                        {
                            hiPrec[0] = Double.PositiveInfinity;
                            hiPrec[1] = 0.0;
                        }
                        return Double.PositiveInfinity;
                    }

                }

                intPartA = EXP_INT_A[EXP_INT_TABLE_MAX_INDEX + intVal];
                intPartB = EXP_INT_B[EXP_INT_TABLE_MAX_INDEX + intVal];

                /* Get the fractional part of x, find the greatest multiple of 2^-10 less than
                 * x and look up the exp function of it.
                 * fracPartA will have the upper 22 bits, fracPartB the lower 52 bits.
                 */
                int intFrac = (int)((x - intVal) * 1024.0);
                double fracPartA = EXP_FRAC_A[intFrac];
                double fracPartB = EXP_FRAC_B[intFrac];

                /* epsilon is the difference in x from the nearest multiple of 2^-10d  It
                 * has a value in the range 0 <= epsilon < 2^-10.
                 * Do the subtraction from x as the last step to avoid possible loss of precision.
                 */
                double epsilon = x - (intVal + intFrac / 1024.0);

                /* Compute z = exp(epsilon) - 1.0 via a minimax polynomiald  z has
               full double precision (52 bits)d  Since z < 2^-10, we will have
               62 bits of precision when combined with the constant 1d  This will be
               used in the last addition below to get proper roundingd */

                /* Remez generated polynomiald  Converges on the interval [0, 2^-10], error
               is less than 0.5 ULP */
                double z = 0.04168701738764507;
                z = z * epsilon + 0.1666666505023083;
                z = z * epsilon + 0.5000000000042687;
                z = z * epsilon + 1.0;
                z = z * epsilon + -3.940510424527919E-20;

                /* Compute (intPartA+intPartB) * (fracPartA+fracPartB) by binomial
               expansion.
               tempA is exact since intPartA and intPartB only have 22 bits each.
               tempB will have 52 bits of precision.
                 */
                double tempA = intPartA * fracPartA;
                double tempB = intPartA * fracPartB + intPartB * fracPartA + intPartB * fracPartB;

                /* Compute the resultd  (1+z)(tempA+tempB)d  Order of operations is
               importantd  For accuracy add by increasing sized  tempA is exact and
               much larger than the othersd  If there are extra bits specified from the
               pow() function, use themd */
                double tempC = tempB + tempA;

                // If tempC is positive infinite, the evaluation below could result in NaN,
                // because z could be negative at the same time.
                if (tempC == Double.PositiveInfinity)
                {
                    return Double.PositiveInfinity;
                }

                if (extra != 0.0)
                {
                    result = tempC * extra * z + tempC * extra + tempC * z + tempB + tempA;
                }
                else
                {
                    result = tempC * z + tempB + tempA;
                }

                if (hiPrec != null)
                {
                    // If requesting high precision
                    hiPrec[0] = tempA;
                    hiPrec[1] = tempC * extra * z + tempC * extra + tempC * z + tempB;
                }

                return result;
            }
        }


        /// <summary>
        /// <a href="http://en.wikipedia.org/wiki/Gaussian_function">
        ///  Gaussian</a> function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Gaussian : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            /// <summary>Meand */
            private double mean;
            /// <summary>Inverse of the standard deviationd */
            private double inverse;
            /// <summary>Inverse of twice the square of the standard deviationd */
            private double i2s2;
            /// <summary>Normalization factord */
            private double norm;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>
            /// Gaussian with given normalization factor, mean and standard deviation.
            /// 
            /// <summary>
            /// <param name="norm">Normalization factor.</param>
            /// <param name="mean">Mean.</param>
            /// <param name="sigma">Standard deviation.</param>
            /// <exception cref="NotStrictlyPositiveException">if {@code sigma <= 0}d </exception>
            public Gaussian(double norm, double mean, double sigma)
            {
                if (sigma <= 0)
                {
                    throw new NotStrictlyPositiveException(sigma);
                }

                this.norm = norm;
                this.mean = mean;
                this.inverse = 1 / sigma;
                this.i2s2 = 0.5 * inverse * inverse;
            }

            /// <summary>
            /// Normalized gaussian with given mean and standard deviation.
            /// 
            /// <summary>
            /// <param name="mean">Mean.</param>
            /// <param name="sigma">Standard deviation.</param>
            /// <exception cref="NotStrictlyPositiveException">if {@code sigma <= 0}d </exception>
            public Gaussian(double mean, double sigma) : this(1 / (sigma * System.Math.Sqrt(2 * System.Math.PI)), mean, sigma)

            {

            }

            /// <summary>
            /// Normalized gaussian with zero mean and unit standard deviation.
            /// <summary>
            public Gaussian() : this(0, 1)
            {

            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return Value(x - mean, norm, i2s2);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parametric function where the input array contains the parameters of
            /// the Gaussian, ordered as follows:
            /// <ul>
            ///  <li>Norm</li>
            ///  <li>Mean</li>
            ///  <li>Standard deviation</li>
            /// </ul>
            /// <summary>
            public class Parametric : IParametricUnivariateFunction
            {
                /// <summary>
                /// Computes the value of the Gaussian at {@code x}.
                /// 
                /// <summary>
                /// <param name="x">Value for which the function must be computed.</param>
                /// <param name="param">Values of norm, mean and standard deviation.</param>
                /// <returns>the value of the function.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 3.
                /// <exception cref="NotStrictlyPositiveException">if {@code param[2]} is negatived </exception>
                public double Value(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);

                    double diff = x - param[1];
                    double i2s2 = 1 / (2 * param[2] * param[2]);
                    return Gaussian.Value(diff, param[0], i2s2);
                }

                /// <summary>
                /// Computes the value of the gradient at {@code x}.
                /// The components of the gradient vector are the partial
                /// derivatives of the function with respect to each of the
                /// <em>parameters</em> (norm, mean and standard deviation).
                /// 
                /// <summary>
                /// <param name="x">Value at which the gradient must be computed.</param>
                /// <param name="param">Values of norm, mean and standard deviation.</param>
                /// <returns>the gradient vector at {@code x}.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 3.
                /// <exception cref="NotStrictlyPositiveException">if {@code param[2]} is negatived </exception>
                public double[] Gradient(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);

                    double norm = param[0];
                    double diff = x - param[1];
                    double sigma = param[2];
                    double i2s2 = 1 / (2 * sigma * sigma);

                    double n = Gaussian.Value(diff, 1, i2s2);
                    double m = norm * n * 2 * i2s2 * diff;
                    double s = m * diff / sigma;

                    return new double[] { n, m, s };
                }

                /// <summary>
                /// ArgumentCheckers parameters to ensure they are appropriate for the evaluation of
                /// the {@link #value(double,double[])} and {@link #gradient(double,double[])}
                /// methods.
                /// 
                /// <summary>
                /// <param name="param">Values of norm, mean and standard deviation.</param>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 3.
                /// <exception cref="NotStrictlyPositiveException">if {@code param[2]} is negatived </exception>
                private void ArgumentCheckerParameters(double[] param)
                {
                    if (param == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (param.Length != 3)
                    {
                        throw new DimensionMismatchException(param.Length, 3);
                    }
                    if (param[2] <= 0)
                    {
                        throw new NotStrictlyPositiveException(param[2]);
                    }
                }
            }

            /// <summary>
            /// <summary>
            /// <param name="xMinusMean">{@code x - mean}.</param>
            /// <param name="norm">Normalization factor.</param>
            /// <param name="i2s2">Inverse of twice the square of the standard deviation.</param>
            /// <returns>the value of the Gaussian at {@code x}.</returns>
            private static double Value(double xMinusMean, double norm, double i2s2)
            {
                return norm * System.Math.Exp(-xMinusMean * xMinusMean * i2s2);
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                double u = inverse * (t.Value() - mean);
                double[] f = new double[t.Order + 1];

                // the nth order derivative of the Gaussian has the form:
                // dn(g(x)/dxn = (norm / s^n) P_n(u) exp(-u^2/2) with u=(x-m)/s
                // where P_n(u) is a degree n polynomial with same parity as n
                // P_0(u) = 1, P_1(u) = -u, P_2(u) = u^2 - 1, P_3(u) = -u^3 + 3 u...
                // the general recurrence relation for P_n is:
                // P_n(u) = P_(n-1)'(u) - u P_(n-1)(u)
                // as per polynomial parity, we can store coefficients of both P_(n-1) and P_n in the same array
                double[] p = new double[f.Length];
                p[0] = 1;
                double u2 = u * u;
                double coeff = norm * System.Math.Exp(-0.5 * u2);
                if (coeff <= Precision2.SAFE_MIN)
                {
                    f.Fill(0.0);
                }
                else
                {
                    f[0] = coeff;
                    for (int n = 1; n < f.Length; ++n)
                    {

                        // update and evaluate polynomial P_n(x)
                        double v = 0;
                        p[n] = -p[n - 1];
                        for (int k = n; k >= 0; k -= 2)
                        {
                            v = v * u2 + p[k];
                            if (k > 2)
                            {
                                p[k - 2] = (k - 1) * p[k - 1] - p[k - 3];
                            }
                            else if (k == 2)
                            {
                                p[0] = p[1];
                            }
                        }
                        if ((n & 0x1) == 1)
                        {
                            v *= u;
                        }

                        coeff *= inverse;
                        f[n] = coeff * v;

                    }
                }

                return t.Compose(f);

            }
        }

        /// <summary>
        /// <a href="http://en.wikipedia.org/wiki/Harmonic_oscillator">simple harmonic oscillator</a> function.
        /// </summary>
        public class HarmonicOscillator : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paranValue;

            public double ParamValue { get { return _paranValue; } }

            /// <summary>Amplituded */
            private double amplitude;
            /// <summary>Angular frequencyd */
            private double omega;
            /// <summary>Phased */
            private double phase;

            /// <summary>
            /// Harmonic oscillator function.
            /// 
            /// <summary>
            /// <param name="amplitude">Amplitude.</param>
            /// <param name="omega">Angular frequency.</param>
            /// <param name="phase">Phase.</param>
            public HarmonicOscillator(double amplitude, double omega, double phase)
            {
                this.amplitude = amplitude;
                this.omega = omega;
                this.phase = phase;
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paranValue = x;
                return Value(omega * x + phase, amplitude);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated", true)]
            public UnivariateFunction Derivative()
            {
                //return FunctionBuilder.toDifferentiableUnivariateFunction(this).derivative();
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parametric function where the input array contains the parameters of
            /// the harmonic oscillator function, ordered as follows:
            /// <ul>
            ///  <li>Amplitude</li>
            ///  <li>Angular frequency</li>
            ///  <li>Phase</li>
            /// </ul>
            /// <summary>
            public class Parametric : IParametricUnivariateFunction
            {
                /// <summary>
                /// Computes the value of the harmonic oscillator at {@code x}.
                /// 
                /// <summary>
                /// <param name="x">Value for which the function must be computed.</param>
                /// <param name="param">Values of norm, mean and standard deviation.</param>
                /// <returns>the value of the function.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 3.
                public double Value(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);
                    return HarmonicOscillator.Value(x * param[1] + param[2], param[0]);
                }

                /// <summary>
                /// Computes the value of the gradient at {@code x}.
                /// The components of the gradient vector are the partial
                /// derivatives of the function with respect to each of the
                /// <em>parameters</em> (amplitude, angular frequency and phase).
                /// 
                /// <summary>
                /// <param name="x">Value at which the gradient must be computed.</param>
                /// <param name="param">Values of amplitude, angular frequency and phase.</param>
                /// <returns>the gradient vector at {@code x}.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 3.
                public double[] Gradient(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);

                    double amplitude = param[0];
                    double omega = param[1];
                    double phase = param[2];

                    double xTimesOmegaPlusPhase = omega * x + phase;
                    double a = HarmonicOscillator.Value(xTimesOmegaPlusPhase, 1);
                    double p = -amplitude * System.Math.Sin(xTimesOmegaPlusPhase);
                    double w = p * x;

                    return new double[] { a, w, p };
                }

                /// <summary>
                /// ArgumentCheckers parameters to ensure they are appropriate for the evaluation of
                /// the {@link #value(double,double[])} and {@link #gradient(double,double[])}
                /// methods.
                /// 
                /// <summary>
                /// <param name="param">Values of norm, mean and standard deviation.</param>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 3.
                private void ArgumentCheckerParameters(double[] param)
                {
                    if (param == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (param.Length != 3)
                    {
                        throw new DimensionMismatchException(param.Length, 3);
                    }
                }
            }

            /// <summary>
            /// <summary>
            /// <param name="xTimesOmegaPlusPhase">{@code omega * x + phase}.</param>
            /// <param name="amplitude">Amplitude.</param>
            /// <returns>the value of the harmonic oscillator function at {@code x}.</returns>
            private static double Value(double xTimesOmegaPlusPhase, double amplitude)
            {
                return amplitude * System.Math.Cos(xTimesOmegaPlusPhase);
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                double x = t.Value();
                double[]
            f = new double[t.Order + 1];

                double alpha = omega * x + phase;
                f[0] = amplitude * System.Math.Cos(alpha);
                if (f.Length > 1)
                {
                    f[1] = -amplitude * omega * System.Math.Sin(alpha);
                    double mo2 = -omega * omega;
                    for (int i = 2; i < f.Length; ++i)
                    {
                        f[i] = mo2 * f[i - 2];
                    }
                }
                return t.Compose(f);
            }
        }

        /// <summary>
        /// Identity Description
        /// </summary>
        public class Identity : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paranValue;

            public double ParamValue { get { return _paranValue; } }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                return (new Constant(1)).CastType<UnivariateFunction>();
            }

            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t;
            }

            public double Value(double x)
            {
                _paranValue = x;
                return x;
            }
        }

        /// <summary>
        /// Natural logarithm function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Log : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Log(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Log();
            }
        }

        /// <summary>
        /// Base 10 logarithm function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Log10 : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Log10(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Log10();
            }
        }

        /// <summary>
        /// Base 10 logarithm function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Log1P : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                if (x == -1)
                {
                    return Double.NegativeInfinity;
                }

                if (x == Double.PositiveInfinity)
                {
                    return Double.PositiveInfinity;
                }

                if (x > 1e-6 ||
                    x < -1e-6)
                {
                    double xpa = 1 + x;
                    double xpb = -(xpa - 1 - x);

                    double[] hiPrec = new double[2];
                    double lores = log(xpa, hiPrec);
                    if ((Double.PositiveInfinity == lores) || (Double.NegativeInfinity == lores))
                    { // Don't allow this to be converted to NaN
                        return lores;
                    }

                    // Do a taylor series expansion around xpa:
                    //   f(x+y) = f(x) + f'(x) y + f''(x)/2 y^2
                    double fx1 = xpb / xpa;
                    double epsilon = 0.5 * fx1 + 1;
                    return epsilon * fx1 + hiPrec[1] + hiPrec[0];
                }
                else
                {
                    // Value is small |x| < 1e6, do a Taylor series centered on 1.
                    double y = (x * F_1_3 - F_1_2) * x + 1;
                    return y * x;
                }
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Log1p();
            }

            /// <summary>
            /// Computes the <a href="http://mathworld.wolfram.com/Logarithm.html">
            /// logarithm</a> in a given base.
            /// 
            /// Returns {@code NaN} if either argument is negative.
            /// If {@code base} is 0 and {@code x} is positive, 0 is returned.
            /// If {@code base} is positive and {@code x} is 0,
            /// {@code Double.NegativeInfinity} is returned.
            /// If both arguments are 0, the result is {@code NaN}.
            /// 
            /// <summary>
            /// <param name="base">Base of the logarithm, must be greater than 0.</param>
            /// <param name="x">Argument, must be greater than 0.</param>
            /// <returns>the value of the logarithm, i.ed the number {@code y} such that</returns>
            /// <code>base<sup>y</sup> = x</code>.
            /// @since 1.2 (previously in {@code MathUtils}, moved as of version 3.0)
            public static double log(double b, double x)
            {
                return System.Math.Log(x) / System.Math.Log(b);
            }

            /// <summary>
            /// Internal helper method for natural logarithm function.
            /// <summary>
            /// <param name="x">original argument of the natural logarithm function</param>
            /// <param name="hiPrec">extra bits of precision on output (To Be Confirmed)</param>
            /// <returns>log(x)</returns>
            private static double log(double x, double[] hiPrec)
            {
                if (x == 0)
                { // Handle special case of +0/-0
                    return Double.NegativeInfinity;
                }
                long bits = BitConverter.DoubleToInt64Bits(x);

                /* Handle special cases of negative input, and NaN */
                if ((((ulong)bits & 0x8000000000000000L) != 0 || double.IsNaN(x)) && x != 0.0)
                {
                    if (hiPrec != null)
                    {
                        hiPrec[0] = Double.NaN;
                    }

                    return Double.NaN;
                }

                /* Handle special cases of Positive infinityd */
                if (x == Double.PositiveInfinity)
                {
                    if (hiPrec != null)
                    {
                        hiPrec[0] = Double.PositiveInfinity;
                    }

                    return Double.PositiveInfinity;
                }

                /* Extract the exponent */
                int exp = (int)(bits >> 52) - 1023;

                if ((bits & 0x7ff0000000000000L) == 0)
                {
                    // Subnormal!
                    if (x == 0)
                    {
                        // Zero
                        if (hiPrec != null)
                        {
                            hiPrec[0] = Double.NegativeInfinity;
                        }

                        return Double.NegativeInfinity;
                    }

                    /* Normalize the subnormal numberd */
                    bits <<= 1;
                    while ((bits & 0x0010000000000000L) == 0)
                    {
                        --exp;
                        bits <<= 1;
                    }
                }


                if ((exp == -1 || exp == 0) && x < 1.01 && x > 0.99 && hiPrec == null)
                {
                    /* The normal method doesn't work well in the range [0.99, 1.01], so call do a straight
                   polynomial expansion in higer precisiond */

                    /* Compute x - 1.0 and split it */
                    double xa = x - 1.0;
                    double xb = xa - x + 1.0;
                    double tmp = xa * HEX_40000000;
                    double aa = xa + tmp - tmp;
                    double ab = xa - aa;
                    xa = aa;
                    xb = ab;

                    double[] lnCoef_last = LN_QUICK_COEF[LN_QUICK_COEF.Length - 1];
                    double ya = lnCoef_last[0];
                    double yb = lnCoef_last[1];

                    for (int i = LN_QUICK_COEF.Length - 2; i >= 0; i--)
                    {
                        /* Multiply a = y * x */
                        aa = ya * xa;
                        ab = ya * xb + yb * xa + yb * xb;
                        /* split, so now y = a */
                        tmp = aa * HEX_40000000;
                        ya = aa + tmp - tmp;
                        yb = aa - ya + ab;

                        /* Add  a = y + lnQuickCoef */
                        double[] lnCoef_i = LN_QUICK_COEF[i];
                        aa = ya + lnCoef_i[0];
                        ab = yb + lnCoef_i[1];
                        /* Split y = a */
                        tmp = aa * HEX_40000000;
                        ya = aa + tmp - tmp;
                        yb = aa - ya + ab;
                    }

                    /* Multiply a = y * x */
                    aa = ya * xa;
                    ab = ya * xb + yb * xa + yb * xb;
                    /* split, so now y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;

                    return ya + yb;
                }

                // lnm is a log of a number in the range of 1.0 - 2.0, so 0 <= lnm < ln(2)
                double[] lnm = LN_MANT[(int)((bits & 0x000ffc0000000000L) >> 42)];

                /*
            double epsilon = x / BitConverter.Int64BitsToDouble(bits & 0xfffffc0000000000L);

            epsilon -= 1.0;
                 */

                // y is the most significant 10 bits of the mantissa
                //double y = BitConverter.Int64BitsToDouble(bits & 0xfffffc0000000000L);
                //double epsilon = (x - y) / y;
                double epsilon = (bits & 0x3ffffffffffL) / (TWO_POWER_52 + (bits & 0x000ffc0000000000L));

                double lnza = 0.0;
                double lnzb = 0.0;

                if (hiPrec != null)
                {
                    /* split epsilon -> x */
                    double tmp = epsilon * HEX_40000000;
                    double aa = epsilon + tmp - tmp;
                    double ab = epsilon - aa;
                    double xa = aa;
                    double xb = ab;

                    /* Need a more accurate epsilon, so adjust the divisiond */
                    double numer = bits & 0x3ffffffffffL;
                    double denom = TWO_POWER_52 + (bits & 0x000ffc0000000000L);
                    aa = numer - xa * denom - xb * denom;
                    xb += aa / denom;

                    /* Remez polynomial evaluation */
                    double[] lnCoef_last = LN_HI_PREC_COEF[LN_HI_PREC_COEF.Length - 1];
                    double ya = lnCoef_last[0];
                    double yb = lnCoef_last[1];

                    for (int i = LN_HI_PREC_COEF.Length - 2; i >= 0; i--)
                    {
                        /* Multiply a = y * x */
                        aa = ya * xa;
                        ab = ya * xb + yb * xa + yb * xb;
                        /* split, so now y = a */
                        tmp = aa * HEX_40000000;
                        ya = aa + tmp - tmp;
                        yb = aa - ya + ab;

                        /* Add  a = y + lnHiPrecCoef */
                        double[] lnCoef_i = LN_HI_PREC_COEF[i];
                        aa = ya + lnCoef_i[0];
                        ab = yb + lnCoef_i[1];
                        /* Split y = a */
                        tmp = aa * HEX_40000000;
                        ya = aa + tmp - tmp;
                        yb = aa - ya + ab;
                    }

                    /* Multiply a = y * x */
                    aa = ya * xa;
                    ab = ya * xb + yb * xa + yb * xb;

                    /* split, so now lnz = a */
                    /*
              tmp = aa * 1073741824.0;
              lnza = aa + tmp - tmp;
              lnzb = aa - lnza + ab;
                     */
                    lnza = aa + ab;
                    lnzb = -(lnza - aa - ab);
                }
                else
                {
                    /* High precision not requiredd  Eval Remez polynomial
                 using standard double precision */
                    lnza = -0.16624882440418567;
                    lnza = lnza * epsilon + 0.19999954120254515;
                    lnza = lnza * epsilon + -0.2499999997677497;
                    lnza = lnza * epsilon + 0.3333333333332802;
                    lnza = lnza * epsilon + -0.5;
                    lnza = lnza * epsilon + 1.0;
                    lnza *= epsilon;
                }

                /* Relative sizes:
                 * lnzb     [0, 2.33E-10]
                 * lnm[1]   [0, 1.17E-7]
                 * ln2B*exp [0, 1.12E-4]
                 * lnza      [0, 9.7E-4]
                 * lnm[0]   [0, 0.692]
                 * ln2A*exp [0, 709]
                 */

                /* Compute the following sum:
                 * lnzb + lnm[1] + ln2B*exp + lnza + lnm[0] + ln2A*exp;
                 */

                //return lnzb + lnm[1] + ln2B*exp + lnza + lnm[0] + ln2A*exp;
                double a = LN_2_A * exp;
                double b = 0.0;
                double c = a + lnm[0];
                double d = -(c - a - lnm[0]);
                a = c;
                b += d;

                c = a + lnza;
                d = -(c - a - lnza);
                a = c;
                b += d;

                c = a + LN_2_B * exp;
                d = -(c - a - LN_2_B * exp);
                a = c;
                b += d;

                c = a + lnm[1];
                d = -(c - a - lnm[1]);
                a = c;
                b += d;

                c = a + lnzb;
                d = -(c - a - lnzb);
                a = c;
                b += d;

                if (hiPrec != null)
                {
                    hiPrec[0] = a;
                    hiPrec[1] = b;
                }

                return a + b;
            }
        }

        /// <summary>
        /// <a href="http://en.wikipedia.org/wiki/Generalised_logistic_function">Generalised logistic</a> function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Logistic : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            /// <summary>Lower asymptoted */
            private double a;
            /// <summary>Upper asymptoted */
            private double k;
            /// <summary>Growth rated */
            private double b;
            /// <summary>Parameter that affects near which asymptote maximum growth occursd */
            private double oneOverN;
            /// <summary>Parameter that affects the position of the curve along the ordinate axisd */
            private double q;
            /// <summary>Abscissa of maximum growthd */
            private double m;

            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>
            /// <summary>
            /// <param name="k">If {@code b > 0}, value of the function for x going towards +&infin;.</param>
            /// If {@code b < 0}, value of the function for x going towards -&infin;.
            /// <param name="m">Abscissa of maximum growth.</param>
            /// <param name="b">Growth rate.</param>
            /// <param name="q">Parameter that affects the position of the curve along the</param>
            /// ordinate axis.
            /// <param name="a">If {@code b > 0}, value of the function for x going towards -&infin;.</param>
            /// If {@code b < 0}, value of the function for x going towards +&infin;.
            /// <param name="n">Parameter that affects near which asymptote the maximum</param>
            /// growth occurs.
            /// <exception cref="NotStrictlyPositiveException">if {@code n <= 0}d </exception>
            public Logistic(double k,
                            double m,
                            double b,
                            double q,
                            double a,
                            double n)

            {
                if (n <= 0)
                {
                    throw new NotStrictlyPositiveException(n);
                }

                this.k = k;
                this.m = m;
                this.b = b;
                this.q = q;
                this.a = a;
                oneOverN = 1 / n;
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return Value(m - x, k, b, q, a, oneOverN);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Negate().Add(m).Multiply(b).Expo().Multiply(q).Add(1).Pow(oneOverN).Reciprocal().Multiply(k - a).Add(a);
            }

            /// <summary>
            /// <summary>
            /// <param name="mMinusX">{@code m - x}.</param>
            /// <param name="k">{@code k}.</param>
            /// <param name="b">{@code b}.</param>
            /// <param name="q">{@code q}.</param>
            /// <param name="a">{@code a}.</param>
            /// <param name="oneOverN">{@code 1 / n}.</param>
            /// <returns>the value of the function.</returns>
            private static double Value(double mMinusX,
                                        double k,
                                        double b,
                                        double q,
                                        double a,
                                        double oneOverN)
            {
                return a + (k - a) / System.Math.Pow(1 + q * System.Math.Exp(b * mMinusX), oneOverN);
            }

            /// <summary>
            /// Parametric function where the input array contains the parameters of
            /// the {@link Logistic#Logistic(double,double,double,double,double,double)
            /// logistic function}, ordered as follows:
            /// <ul>
            ///  <li>k</li>
            ///  <li>m</li>
            ///  <li>b</li>
            ///  <li>q</li>
            ///  <li>a</li>
            ///  <li>n</li>
            /// </ul>
            /// <summary>
            public class Parametric : IParametricUnivariateFunction
            {
                /// <summary>
                /// Computes the value of the sigmoid at {@code x}.
                /// 
                /// <summary>
                /// <param name="x">Value for which the function must be computed.</param>
                /// <param name="param">Values for {@code k}, {@code m}, {@code b}, {@code q},</param>
                /// {@code a} and  {@code n}.
                /// <returns>the value of the function.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 6.
                /// <exception cref="NotStrictlyPositiveException">if {@code param[5] <= 0}d </exception>
                public double Value(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);
                    return Logistic.Value(param[1] - x, param[0],
                                          param[2], param[3],
                                          param[4], 1 / param[5]);
                }

                /// <summary>
                /// Computes the value of the gradient at {@code x}.
                /// The components of the gradient vector are the partial
                /// derivatives of the function with respect to each of the
                /// <em>parameters</em>.
                /// 
                /// <summary>
                /// <param name="x">Value at which the gradient must be computed.</param>
                /// <param name="param">Values for {@code k}, {@code m}, {@code b}, {@code q},</param>
                /// {@code a} and  {@code n}.
                /// <returns>the gradient vector at {@code x}.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 6.
                /// <exception cref="NotStrictlyPositiveException">if {@code param[5] <= 0}d </exception>
                public double[] Gradient(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);

                    double b = param[2];
                    double q = param[3];

                    double mMinusX = param[1] - x;
                    double oneOverN = 1 / param[5];
                    double exp = System.Math.Exp(b * mMinusX);
                    double qExp = q * exp;
                    double qExp1 = qExp + 1;
                    double factor1 = (param[0] - param[4]) * oneOverN / System.Math.Pow(qExp1, oneOverN);
                    double factor2 = -factor1 / qExp1;

                    // Components of the gradient.
                    double gk = Logistic.Value(mMinusX, 1, b, q, 0, oneOverN);
                    double gm = factor2 * b * qExp;
                    double gb = factor2 * mMinusX * qExp;
                    double gq = factor2 * exp;
                    double ga = Logistic.Value(mMinusX, 0, b, q, 1, oneOverN);
                    double gn = factor1 * System.Math.Log(qExp1) * oneOverN;

                    return new double[] { gk, gm, gb, gq, ga, gn };
                }

                /// <summary>
                /// ArgumentCheckers parameters to ensure they are appropriate for the evaluation of
                /// the {@link #value(double,double[])} and {@link #gradient(double,double[])}
                /// methods.
                /// 
                /// <summary>
                /// <param name="param">Values for {@code k}, {@code m}, {@code b}, {@code q},</param>
                /// {@code a} and {@code n}.
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 6.
                /// <exception cref="NotStrictlyPositiveException">if {@code param[5] <= 0}d </exception>
                private void ArgumentCheckerParameters(double[] param)
                {
                    if (param == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (param.Length != 6)
                    {
                        throw new DimensionMismatchException(param.Length, 6);
                    }
                    if (param[5] <= 0)
                    {
                        throw new NotStrictlyPositiveException(param[5]);
                    }
                }
            }
        }


        /// <summary>
        /// Logit function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Logit : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>Lower boundd */
            private double lo;
            /// <summary>Higher boundd */
            private double hi;

            /// <summary>
            /// Usual logit function, where the lower bound is 0 and the higher
            /// bound is 1.
            /// <summary>
            public Logit() : this(0, 1)
            {

            }

            /// <summary>
            /// Logit function.
            /// 
            /// <summary>
            /// <param name="lo">Lower bound of the function domain.</param>
            /// <param name="hi">Higher bound of the function domain.</param>
            public Logit(double lo,
                         double hi)
            {
                this.lo = lo;
                this.hi = hi;
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Log10(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                double x = t.Value();
                if (x < lo || x > hi)
                {
                    throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, lo, hi));
                }
                double[] f = new double[t.Order + 1];

                // function value
                f[0] = System.Math.Log((x - lo) / (hi - x));

                if (Double.IsInfinity(f[0]))
                {

                    if (f.Length > 1)
                    {
                        f[1] = Double.PositiveInfinity;
                    }
                    // fill the array with infinities
                    // (for x close to lo the signs will flip between -inf and +inf,
                    //  for x close to hi the signs will always be +inf)
                    // this is probably overkill, since the call to compose at the end
                    // of the method will transform most infinities into NaN ...
                    for (int i = 2; i < f.Length; ++i)
                    {
                        f[i] = f[i - 2];
                    }

                }
                else
                {

                    // function derivatives
                    double invL = 1.0 / (x - lo);
                    double xL = invL;
                    double invH = 1.0 / (hi - x);
                    double xH = invH;
                    for (int i = 1; i < f.Length; ++i)
                    {
                        f[i] = xL + xH;
                        xL *= -i * invL;
                        xH *= i * invH;
                    }
                }

                return t.Compose(f);
            }

            /// <summary>
            /// <summary>
            /// <param name="x">Value at which to compute the logit.</param>
            /// <param name="lo">Lower bound.</param>
            /// <param name="hi">Higher bound.</param>
            /// <returns>the value of the logit function at {@code x}.</returns>
            /// <exception cref="IndexOutOfRangeException">if {@code x < lo} or {@code x > hi}d </exception>
            private static double Value(double x,
                                        double lo,
                                        double hi)
            {
                if (x < lo || x > hi)
                {
                    throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, lo, hi));
                }
                return System.Math.Log((x - lo) / (hi - x));
            }

            /// <summary>
            /// Parametric function where the input array contains the parameters of
            /// the logit function, ordered as follows:
            /// <ul>
            ///  <li>Lower bound</li>
            ///  <li>Higher bound</li>
            /// </ul>
            /// <summary>
            public class Parametric : IParametricUnivariateFunction
            {
                /// <summary>
                /// Computes the value of the logit at {@code x}.
                /// 
                /// <summary>
                /// <param name="x">Value for which the function must be computed.</param>
                /// <param name="param">Values of lower bound and higher bounds.</param>
                /// <returns>the value of the function.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 2.
                public double Value(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);
                    return Logit.Value(x, param[0], param[1]);
                }

                /// <summary>
                /// Computes the value of the gradient at {@code x}.
                /// The components of the gradient vector are the partial
                /// derivatives of the function with respect to each of the
                /// <em>parameters</em> (lower bound and higher bound).
                /// 
                /// <summary>
                /// <param name="x">Value at which the gradient must be computed.</param>
                /// <param name="param">Values for lower and higher bounds.</param>
                /// <returns>the gradient vector at {@code x}.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 2.
                public double[] Gradient(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);

                    double lo = param[0];
                    double hi = param[1];

                    return new double[] { 1 / (lo - x), 1 / (hi - x) };
                }

                /// <summary>
                /// ArgumentCheckers parameters to ensure they are appropriate for the evaluation of
                /// the {@link #value(double,double[])} and {@link #gradient(double,double[])}
                /// methods.
                /// 
                /// <summary>
                /// <param name="param">Values for lower and higher bounds.</param>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 2.
                private void ArgumentCheckerParameters(double[] param)
                {
                    if (param == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (param.Length != 2)
                    {
                        throw new DimensionMismatchException(param.Length, 2);
                    }
                }
            }
        }

        /// <summary>
        /// Minus function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Minus : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return -x;
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Negate();
            }
        }

        /// <summary>
        /// Power function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Power : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            /// <summary>Powerd */
            private double p;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>
            /// <summary>
            /// <param name="p">Power.</param>
            public Power(double p)
            {
                this.p = p;
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Pow(x, p);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Pow(p);
            }
        }

        /// <summary>
        /// <a href="http://en.wikipedia.org/wiki/Sigmoid_function">
        ///  Sigmoid</a> function.
        /// It is the inverse of the {@link Logit logit} function.
        /// A more flexible version, the generalised logistic, is implemented
        /// by the {@link Logistic} class.
        /// 
        /// @since 3.0
        /// <summary>
        public class Sigmoid : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            /// <summary>Lower asymptoted */
            private double lo;
            /// <summary>Higher asymptoted */
            private double hi;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>
            /// Usual sigmoid function, where the lower asymptote is 0 and the higher
            /// asymptote is 1.
            /// <summary>
            public Sigmoid() : this(0, 1)
            {

            }

            /// <summary>
            /// Sigmoid function.
            /// 
            /// <summary>
            /// <param name="lo">Lower asymptote.</param>
            /// <param name="hi">Higher asymptote.</param>
            public Sigmoid(double lo, double hi)
            {
                this.lo = lo;
                this.hi = hi;
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return Value(x, lo, hi);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                double[] f = new double[t.Order + 1];
                double exp = System.Math.Exp(-t.Value());
                if (Double.IsInfinity(exp))
                {

                    // special handling near lower boundary, to avoid NaN
                    f[0] = lo;
                    f.Fill(1, f.Length, 0.0);

                }
                else
                {

                    // the nth order derivative of sigmoid has the form:
                    // dn(sigmoid(x)/dxn = P_n(exp(-x)) / (1+exp(-x))^(n+1)
                    // where P_n(t) is a degree n polynomial with normalized higher term
                    // P_0(t) = 1, P_1(t) = t, P_2(t) = t^2 - t, P_3(t) = t^3 - 4 t^2 + t...
                    // the general recurrence relation for P_n is:
                    // P_n(x) = n t P_(n-1)(t) - t (1 + t) P_(n-1)'(t)
                    double[] p = new double[f.Length];

                    double inv = 1 / (1 + exp);
                    double coeff = hi - lo;
                    for (int n = 0; n < f.Length; ++n)
                    {

                        // update and evaluate polynomial P_n(t)
                        double v = 0;
                        p[n] = 1;
                        for (int k = n; k >= 0; --k)
                        {
                            v = v * exp + p[k];
                            if (k > 1)
                            {
                                p[k - 1] = (n - k + 2) * p[k - 2] - (k - 1) * p[k - 1];
                            }
                            else
                            {
                                p[0] = 0;
                            }
                        }

                        coeff *= inv;
                        f[n] = coeff * v;

                    }

                    // fix function value
                    f[0] += lo;

                }

                return t.Compose(f);
            }

            /// <summary>
            /// Parametric function where the input array contains the parameters of
            /// the {@link Sigmoid#Sigmoid(double,double) sigmoid function}, ordered
            /// as follows:
            /// <ul>
            ///  <li>Lower asymptote</li>
            ///  <li>Higher asymptote</li>
            /// </ul>
            /// <summary>
            public class Parametric : IParametricUnivariateFunction
            {
                /// <summary>
                /// Computes the value of the sigmoid at {@code x}.
                /// 
                /// <summary>
                /// <param name="x">Value for which the function must be computed.</param>
                /// <param name="param">Values of lower asymptote and higher asymptote.</param>
                /// <returns>the value of the function.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 2.
                public double Value(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);
                    return Sigmoid.Value(x, param[0], param[1]);
                }

                /// <summary>
                /// Computes the value of the gradient at {@code x}.
                /// The components of the gradient vector are the partial
                /// derivatives of the function with respect to each of the
                /// <em>parameters</em> (lower asymptote and higher asymptote).
                /// 
                /// <summary>
                /// <param name="x">Value at which the gradient must be computed.</param>
                /// <param name="param">Values for lower asymptote and higher asymptote.</param>
                /// <returns>the gradient vector at {@code x}.</returns>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 2.
                public double[] Gradient(double x, params double[] param)
                {
                    ArgumentCheckerParameters(param);

                    double invExp1 = 1 / (1 + System.Math.Exp(-x));

                    return new double[] { 1 - invExp1, invExp1
    };
                }

                /// <summary>
                /// ArgumentCheckers parameters to ensure they are appropriate for the evaluation of
                /// the {@link #value(double,double[])} and {@link #gradient(double,double[])}
                /// methods.
                /// 
                /// <summary>
                /// <param name="param">Values for lower and higher asymptotes.</param>
                /// <exception cref="ArgumentNullException">if {@code param} is {@code null}d </exception>
                /// <exception cref="DimensionMismatchException">if the size of {@code param} is </exception>
                /// not 2.
                private void ArgumentCheckerParameters(double[] param)
                {
                    if (param == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (param.Length != 2)
                    {
                        throw new DimensionMismatchException(param.Length, 2);
                    }
                }
            }

            /// <summary>
            /// <summary>
            /// <param name="x">Value at which to compute the sigmoid.</param>
            /// <param name="lo">Lower asymptote.</param>
            /// <param name="hi">Higher asymptote.</param>
            /// <returns>the value of the sigmoid function at {@code x}.</returns>
            private static double Value(double x,
                                        double lo,
                                        double hi)
            {
                return lo + (hi - lo) / (1 + System.Math.Exp(-x));
            }
        }


        /// <summary>
        /// Square root function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Sqrt : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Sqrt(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Sqrt();
            }
        }

        /// <summary>
        /// Hyperbolic sine function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Sinh : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Sinh(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Sinh();
            }
        }

        /// <summary>
        /// <a href="http://en.wikipedia.org/wiki/Sinc_function">Sinc</a> function,
        /// defined by
        /// <pre><code>
        ///   sinc(x) = 1            if x = 0,
        ///             sin(x) / x   otherwise.
        /// </code></pre>
        /// 
        /// @since 3.0
        /// <summary>
        public class Sinc : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            /// <summary>
            /// Value below which the computations are done using Taylor series.
            /// <p>
            /// The Taylor series for sinc even order derivatives are:
            /// <pre>
            /// d^(2n)sinc/dx^(2n)     = Sum_(k>=0)(-1)^(n+k) / ((2k)!(2n+2k+1)) x^(2k)
            ///                        = (-1)^n     [ 1/(2n+1) - x^2/(4n+6) + x^4/(48n+120) - x^6/(1440n+5040) + O(x^8) ]
            /// </pre>
            /// </p>
            /// <p>
            /// The Taylor series for sinc odd order derivatives are:
            /// <pre>
            /// d^(2n+1)sinc/dx^(2n+1) = Sum_(k>=0)(-1)^(n+k+1) / ((2k+1)!(2n+2k+3)) x^(2k+1)
            ///                        = (-1)^(n+1) [ x/(2n+3) - x^3/(12n+30) + x^5/(240n+840) - x^7/(10080n+45360) + O(x^9) ]
            /// </pre>
            /// </p>
            /// <p>
            /// So the ratio of the fourth term with respect to the first term
            /// is always smaller than x^6/720, for all derivative orders.
            /// This implies that neglecting this term and using only the first three terms induces
            /// a relative error bounded by x^6/720d The SHORTCUT value is chosen such that this
            /// relative error is below double precision accuracy when |x| <= SHORTCUT.
            /// </p>
            /// <summary>
            private static double SHORTCUT = 6.0e-3;
            /// <summary>For normalized sinc functiond */
            private Boolean normalized;

            /// <summary>
            /// The sinc function, {@code sin(x) / x}.
            /// <summary>
            public Sinc() : this(false)
            {

            }

            /// <summary>
            /// Instantiates the sinc function.
            /// 
            /// <summary>
            /// <param name="normalized">If {@code true}, the function is</param>
            /// <code> sin(&pi;x) / &pi;x</code>, otherwise {@code sin(x) / x}.
            public Sinc(Boolean normalized)
            {
                this.normalized = normalized;
            }
            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                double scaledX = normalized ? System.Math.PI * x : x;
                if (System.Math.Abs(scaledX) <= SHORTCUT)
                {
                    // use Taylor series
                    double scaledX2 = scaledX * scaledX;
                    return ((scaledX2 - 20) * scaledX2 + 120) / 120;
                }
                else
                {
                    // use definition expression
                    return System.Math.Sin(scaledX) / scaledX;
                }
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                double scaledX = (normalized ? System.Math.PI : 1) * t.Value();
                double scaledX2 = scaledX * scaledX;

                double[] f = new double[t.Order + 1];

                if (System.Math.Abs(scaledX) <= SHORTCUT)
                {

                    for (int i = 0; i < f.Length; ++i)
                    {
                        int k = i / 2;
                        if ((i & 0x1) == 0)
                        {
                            // even derivation order
                            f[i] = (((k & 0x1) == 0) ? 1 : -1) *
                                   (1.0 / (i + 1) - scaledX2 * (1.0 / (2 * i + 6) - scaledX2 / (24 * i + 120)));
                        }
                        else
                        {
                            // odd derivation order
                            f[i] = (((k & 0x1) == 0) ? -scaledX : scaledX) *
                                   (1.0 / (i + 2) - scaledX2 * (1.0 / (6 * i + 24) - scaledX2 / (120 * i + 720)));
                        }
                    }

                }
                else
                {

                    double inv = 1 / scaledX;
                    double cos = System.Math.Cos(scaledX);
                    double sin = System.Math.Sin(scaledX);

                    f[0] = inv * sin;

                    // the nth order derivative of sinc has the form:
                    // dn(sinc(x)/dxn = [S_n(x) sin(x) + C_n(x) cos(x)] / x^(n+1)
                    // where S_n(x) is an even polynomial with degree n-1 or n (depending on parity)
                    // and C_n(x) is an odd polynomial with degree n-1 or n (depending on parity)
                    // S_0(x) = 1, S_1(x) = -1, S_2(x) = -x^2 + 2, S_3(x) = 3x^2 - 6...
                    // C_0(x) = 0, C_1(x) = x, C_2(x) = -2x, C_3(x) = -x^3 + 6x...
                    // the general recurrence relations for S_n and C_n are:
                    // S_n(x) = x S_(n-1)'(x) - n S_(n-1)(x) - x C_(n-1)(x)
                    // C_n(x) = x C_(n-1)'(x) - n C_(n-1)(x) + x S_(n-1)(x)
                    // as per polynomials parity, we can store both S_n and C_n in the same array
                    double[] sc = new double[f.Length];
                    sc[0] = 1;

                    double coeff = inv;
                    for (int n = 1; n < f.Length; ++n)
                    {

                        double s = 0;
                        double c = 0;

                        // update and evaluate polynomials S_n(x) and C_n(x)
                        int kStart;
                        if ((n & 0x1) == 0)
                        {
                            // even derivation order, S_n is degree n and C_n is degree n-1
                            sc[n] = 0;
                            kStart = n;
                        }
                        else
                        {
                            // odd derivation order, S_n is degree n-1 and C_n is degree n
                            sc[n] = sc[n - 1];
                            c = sc[n];
                            kStart = n - 1;
                        }

                        // in this loop, k is always even
                        for (int k = kStart; k > 1; k -= 2)
                        {

                            // sine part
                            sc[k] = (k - n) * sc[k] - sc[k - 1];
                            s = s * scaledX2 + sc[k];

                            // cosine part
                            sc[k - 1] = (k - 1 - n) * sc[k - 1] + sc[k - 2];
                            c = c * scaledX2 + sc[k - 1];

                        }
                        sc[0] *= -n;
                        s = s * scaledX2 + sc[0];

                        coeff *= inv;
                        f[n] = coeff * (s * sin + c * scaledX * cos);

                    }

                }

                if (normalized)
                {
                    double scale = System.Math.PI;
                    for (int i = 1; i < f.Length; ++i)
                    {
                        f[i] *= scale;
                        scale *= System.Math.PI;
                    }
                }

                return t.Compose(f);
            }
        }

        /// <summary>
        /// Sine function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Sin : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Sin(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Sin();
            }
        }

        /// <summary>
        /// Tangent function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Tan : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Tan(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Tan();
            }
        }

        /// <summary>
        /// Hyperbolic tangent function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Tanh : IUnivariateDifferentiableFunction, IDifferentiableUnivariateFunction
        {
            private double _paramValue;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            /// <summary>{@inheritDoc} */
            public double Value(double x)
            {
                _paramValue = x;
                return System.Math.Tanh(x);
            }

            /// <summary>{@inheritDoc}
            /// [Obsolete("Deprecated", true)] as of 3.1, replaced by {@link #value(DerivativeStructure)}
            /// <summary>
            [Obsolete("Deprecated, replaced by Value(DerivativeStructure)", true)]
            public UnivariateFunction Derivative()
            {
                throw new NotImplementedException();
            }

            /// <summary>{@inheritDoc}
            /// @since 3.1
            /// <summary>
            public DerivativeStructure Value(DerivativeStructure t)
            {
                return t.Tanh();
            }
        }
        #endregion

        #region IBivariateFunction
        /// <summary>
        /// Add the two operands.
        /// 
        /// @since 3.0
        /// <summary>
        public class Add : IBivariateFunction
        {
            public double Value(double x, double y)
            {
                return x + y;
            }
        }

        /// <summary>
        /// Arc-tangent function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Atan2 : IBivariateFunction
        {
            public double Value(double x, double y)
            {
                return System.Math.Atan2(x, y);
            }
        }

        /// <summary>
        /// Add the two operands.
        /// 
        /// @since 3.0
        /// <summary>
        public class Devide : IBivariateFunction
        {
            public double Value(double x, double y)
            {
                return x / y;
            }
        }

        /// <summary>
        /// Maximum function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Max : IBivariateFunction
        {
            /// <summary>{@inheritDoc} */
            public double Value(double x, double y)
            {
                return System.Math.Max(x, y);
            }
        }

        /// <summary>
        /// Minimum function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Min : IBivariateFunction
        {
            /// <summary>{@inheritDoc} */
            public double Value(double x, double y)
            {
                return System.Math.Min(x, y);
            }
        }

        /// <summary>
        /// Multiply the two operands.
        /// 
        /// @since 3.0
        /// <summary>
        public class Multiply : IBivariateFunction
        {
            /// <summary>{@inheritDoc} */
            public double Value(double x, double y)
            {
                return x * y;
            }
        }

        /// <summary>
        /// Power function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Pow : IBivariateFunction
        {
            /// <summary>{@inheritDoc} */
            public double Value(double x, double y)
            {
                return System.Math.Pow(x, y);
            }
        }

        /// <summary>
        /// Subtract function.
        /// 
        /// @since 3.0
        /// <summary>
        public class Subtract : IBivariateFunction
        {
            /// <summary>{@inheritDoc} */
            public double Value(double x, double y)
            {
                return x - y;
            }
        }

        #endregion
    }
}

