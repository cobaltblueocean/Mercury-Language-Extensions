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
using System.Threading.Tasks;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Differentiation;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language.Math.Analysis.Integration;
using Mercury.Language.Extensions;

namespace Mercury.Language.Math.Analysis.Function
{
    /// <summary>
    /// FunctionBuilder Description
    /// </summary>
    public static class FunctionBuilder
    {

        /// <summary>
        /// Composes functions.
        /// <p>
        /// The functions in the argument list are composed sequentially, in the
        /// given orderd  For example, compose(f1,f2,f3) acts like f1(f2(f3(x))).</p>
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>the composite function.</returns>
        public static UnivariateRealFunction Compose(params UnivariateRealFunction[] f)
        {
            return new UnivariateRealFunction()
            {
                function = new Func<double?, double?>((x) =>
               {
                   double r = x.Value;
                   for (int i = f.Length - 1; i >= 0; i--)
                   {
                       r = f[i].Value(r);
                   }
                   return r;
               })
            };
        }

        /// <summary>
        /// Composes functions.
        /// <p>
        /// The functions in the argument list are composed sequentially, in the
        /// given orderd  For example, compose(f1,f2,f3) acts like f1(f2(f3(x))).</p>
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>the composite function.</returns>
        /// @since 3.1
        public static IUnivariateDifferentiableFunction Compose(params IUnivariateDifferentiableFunction[] f)
        {
            return new UnivariateDifferentiableFunction()
            {
                /// <summary>{@inheritDoc} */
                function1 = new Func<double?, double?>((x) =>
                {
                    double r = x.Value;
                    for (int i = f.Length - 1; i >= 0; i--)
                    {
                        r = f[i].Value(r);
                    }
                    return r;
                }),

                function2 = new Func<DerivativeStructure, DerivativeStructure>((x) =>
                {
                    DerivativeStructure r = x;
                    for (int i = f.Length - 1; i >= 0; i--)
                    {
                        r = f[i].Value(r);
                    }
                    return r;
                })

            };
        }


        /// <summary>
        /// Adds functions.
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>a function that computes the sum of the functions.</returns>
        public static IUnivariateRealFunction Add(params IUnivariateRealFunction[] f)
        {
            return new UnivariateRealFunction()
            {
                function = new Func<double?, double?>((x) =>
                {
                    double r = f[0].Value(x.Value);
                    for (int i = 1; i < f.Length; i++)
                    {
                        r += f[i].Value(x.Value);
                    }
                    return r;
                })
            };
        }

        /// <summary>
        /// Adds functions.
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>a function that computes the sum of the functions.</returns>
        /// @since 3.1
        public static IUnivariateDifferentiableFunction Add(params IUnivariateDifferentiableFunction[] f)
        {
            return new UnivariateDifferentiableFunction()
            {
                function1 = new Func<double?, double?>((x) =>
                {
                    double r = f[0].Value(x.Value);
                    for (int i = 1; i < f.Length; i++)
                    {
                        r += f[i].Value(x.Value);
                    }
                    return r;
                }),
                function2 = new Func<DerivativeStructure, DerivativeStructure>((t) =>
                {
                    DerivativeStructure r = f[0].Value(t);
                    for (int i = 1; i < f.Length; i++)
                    {
                        r = r.Add(f[i].Value(t));
                    }
                    return r;
                })

            };
        }


        /// <summary>
        /// Multiplies functions.
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>a function that computes the product of the functions.</returns>
        public static IUnivariateRealFunction Multiply(IUnivariateRealFunction[] f)
        {
            return new UnivariateRealFunction()
            {
                function = new Func<double?, double?>((x) =>
               {
                   double r = f[0].Value(x.Value);
                   for (int i = 1; i < f.Length; i++)
                   {
                       r *= f[i].Value(x.Value);
                   }
                   return r;
               })
            };
        }

        /// <summary>
        /// Multiplies functions.
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>a function that computes the product of the functions.</returns>
        /// @since 3.1
        public static IUnivariateDifferentiableFunction Multiply(params IUnivariateDifferentiableFunction[] f)
        {
            return new UnivariateDifferentiableFunction()
            {
                function1 = new Func<double?, double?>((t) =>
                {
                    double r = f[0].Value(t.Value);
                    for (int i = 1; i < f.Length; i++)
                    {
                        r *= f[i].Value(t.Value);
                    }
                    return r;
                }),
                function2 = new Func<DerivativeStructure, DerivativeStructure>((t) =>
            {
                DerivativeStructure r = f[0].Value(t);
                for (int i = 1; i < f.Length; i++)
                {
                    r = r.Multiply(f[i].Value(t));
                }
                return r;
            })

            };
        }


        /// <summary>
        /// Returns the univariate function
        /// {@code h(x) = combiner(f(x), g(x)).}
        /// 
        /// <summary>
        /// <param name="combiner">Combiner function.</param>
        /// <param name="f">Function.</param>
        /// <param name="g">Function.</param>
        /// <returns>the composite function.</returns>
        public static UnivariateRealFunction Combine(IBivariateRealFunction combiner, UnivariateRealFunction f, UnivariateRealFunction g)
        {
            return new UnivariateRealFunction()
            {
                function = new Func<double?, double?>((x) =>
                {
                    return combiner.Value(f.Value(x.Value), g.Value(x.Value));
                })
            };
        }

        /// <summary>
        /// Returns a MultivariateFunction h(x[]) defined by <pre> <code>
        /// h(x[]) = combiner(...combiner(combiner(initialValue,f(x[0])),f(x[1]))..d),f(x[x.Length-1]))
        /// </code></pre>
        /// 
        /// <summary>
        /// <param name="combiner">Combiner function.</param>
        /// <param name="f">Function.</param>
        /// <param name="initialValue">Initial value.</param>
        /// <returns>a collector function.</returns>
        public static MultivariateRealFunction Collector(IBivariateRealFunction combiner, UnivariateRealFunction f, double initialValue)
        {
            return new MultivariateRealFunction()
            {
                function = new Func<Double[], Double>((x) =>
                {
                    double result = combiner.Value(initialValue, f.Value(x[0]));
                    for (int i = 1; i < x.Length; i++)
                    {
                        result = combiner.Value(result, f.Value(x[i]));
                    }
                    return result;
                })
            };
        }

        /// <summary>
        /// Returns a MultivariateFunction h(x[]) defined by <pre> <code>
        /// h(x[]) = combiner(...combiner(combiner(initialValue,x[0]),x[1])..d),x[x.Length-1])
        /// </code></pre>
        /// 
        /// <summary>
        /// <param name="combiner">Combiner function.</param>
        /// <param name="initialValue">Initial value.</param>
        /// <returns>a collector function.</returns>
        public static MultivariateRealFunction Collector(IBivariateRealFunction combiner, double initialValue)
        {
            return Collector(combiner, (new Functions.Identity()).CastType<UnivariateRealFunction>(), initialValue);
        }

        /// <summary>
        /// Creates a unary function by fixing the first argument of a binary function.
        /// 
        /// <summary>
        /// <param name="f">Binary function.</param>
        /// <param name="fixed">value to which the first argument of {@code f} is set.</param>
        /// <returns>the unary function h(x) = f(fixed, x)</returns>
        public static UnivariateRealFunction Fix1stArgument(IBivariateRealFunction f, double fxd)
        {
            return new UnivariateRealFunction()
            {
                function = new Func<double?, double?>((x) =>
                {
                    return f.Value(fxd, x.Value);
                })
            };
        }
        /// <summary>
        /// Creates a unary function by fixing the second argument of a binary function.
        /// 
        /// <summary>
        /// <param name="f">Binary function.</param>
        /// <param name="fixed">value to which the second argument of {@code f} is set.</param>
        /// <returns>the unary function h(x) = f(x, fixed)</returns>
        public static UnivariateRealFunction Fix2ndArgument(IBivariateRealFunction f, double fxd)
        {
            return new UnivariateRealFunction()
            {
                function = new Func<double?, double?>((x) =>
                {
                    return f.Value(x.Value, fxd);
                })
            };
        }

        /// <summary>
        /// Samples the specified univariate real function on the specified interval.
        /// <p>
        /// The interval is divided equally into {@code n} sections and sample points
        /// are taken from {@code min} to {@code max - (max - min) / n}; therefore
        /// {@code f} is not sampled at the upper bound {@code max}.</p>
        /// 
        /// <summary>
        /// <param name="f">Function to be sampled</param>
        /// <param name="min">Lower bound of the interval (included).</param>
        /// <param name="max">Upper bound of the interval (excluded).</param>
        /// <param name="n">Number of sample points.</param>
        /// <returns>the array of samples.</returns>
        /// <exception cref="NumberIsTooLargeException">if the lower bound {@code min} is </exception>
        /// greater than, or equal to the upper bound {@code max}.
        /// <exception cref="NotStrictlyPositiveException">if the number of sample points </exception>
        /// {@code n} is negative.
        public static double[] Sample(UnivariateRealFunction f, double min, double max, int n)
        {

            if (n <= 0)
            {
                throw new NotStrictlyPositiveException(
                        LocalizedResources.Instance().NOT_POSITIVE_NUMBER_OF_SAMPLES,
                        n);
            }
            if (min >= max)
            {
                throw new NumberIsTooLargeException(min, max, false);
            }

            double[] s = new double[n];
            double h = (max - min) / n;
            for (int i = 0; i < n; i++)
            {
                s[i] = f.Value(min + i * h);
            }
            return s;
        }

        #region Deprecated functions
        /// <summary>
        /// Composes functions.
        /// <p>
        /// The functions in the argument list are composed sequentially, in the
        /// given orderd  For example, compose(f1,f2,f3) acts like f1(f2(f3(x))).</p>
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>the composite function.</returns>
        /// [Obsolete("Deprecated", true)] as of 3.1 replaced by {@link #compose(UnivariateDifferentiableFunction..d)}
        //[Obsolete("Deprecated", true)]
        //    public static DifferentiableUnivariateFunction compose(DifferentiableUnivariateFunction ..d f)
        //{
        //    return new DifferentiableUnivariateFunction()
        //    {
        ///// <summary>{@inheritDoc} */
        //            public double value(double x)
        //    {
        //        double r = x;
        //        for (int i = f.Length - 1; i >= 0; i--)
        //        {
        //            r = f[i].Value(r);
        //        }
        //        return r;
        //    }

        ///// <summary>{@inheritDoc} */
        //    public UnivariateFunction derivative()
        //    {
        //        return new UnivariateFunction()
        //        {
        ///// <summary>{@inheritDoc} */
        //                    public double value(double x)
        //        {
        //            double p = 1;
        //            double r = x;
        //            for (int i = f.Length - 1; i >= 0; i--)
        //            {
        //                p *= f[i].derivative().Value(r);
        //                r = f[i].Value(r);
        //            }
        //            return p;
        //        }
        //    };
        //}
        //        };
        //    }

        /// <summary>
        /// Adds functions.
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>a function that computes the sum of the functions.</returns>
        /// [Obsolete("Deprecated", true)] as of 3.1 replaced by {@link #add(UnivariateDifferentiableFunction..d)}
        //    [Obsolete("Deprecated", true)]
        //    public static DifferentiableUnivariateFunction add(DifferentiableUnivariateFunction ..d f)
        //{
        //    return new DifferentiableUnivariateFunction()
        //    {
        ///// <summary>{@inheritDoc} */
        //            public double value(double x)
        //    {
        //        double r = f[0].Value(x);
        //        for (int i = 1; i < f.Length; i++)
        //        {
        //            r += f[i].Value(x);
        //        }
        //        return r;
        //    }

        ///// <summary>{@inheritDoc} */
        //    public UnivariateFunction derivative()
        //    {
        //        return new UnivariateFunction()
        //        {
        ///// <summary>{@inheritDoc} */
        //                    public double value(double x)
        //        {
        //            double r = f[0].derivative().Value(x);
        //            for (int i = 1; i < f.Length; i++)
        //            {
        //                r += f[i].derivative().Value(x);
        //            }
        //            return r;
        //        }
        //    };
        //}
        //        };
        //    }

        /// <summary>
        /// Multiplies functions.
        /// 
        /// <summary>
        /// <param name="f">List of functions.</param>
        /// <returns>a function that computes the product of the functions.</returns>
        /// [Obsolete("Deprecated", true)] as of 3.1 replaced by {@link #multiply(UnivariateDifferentiableFunction..d)}
        //    [Obsolete("Deprecated", true)]
        //    public static DifferentiableUnivariateFunction multiply(DifferentiableUnivariateFunction ..d f)
        //{
        //    return new DifferentiableUnivariateFunction()
        //    {
        ///// <summary>{@inheritDoc} */
        //            public double value(double x)
        //    {
        //        double r = f[0].Value(x);
        //        for (int i = 1; i < f.Length; i++)
        //        {
        //            r *= f[i].Value(x);
        //        }
        //        return r;
        //    }

        ///// <summary>{@inheritDoc} */
        //    public UnivariateFunction derivative()
        //    {
        //        return new UnivariateFunction()
        //        {
        ///// <summary>{@inheritDoc} */
        //                    public double value(double x)
        //        {
        //            double sum = 0;
        //            for (int i = 0; i < f.Length; i++)
        //            {
        //                double prod = f[i].derivative().Value(x);
        //                for (int j = 0; j < f.Length; j++)
        //                {
        //                    if (i != j)
        //                    {
        //                        prod *= f[j].Value(x);
        //                    }
        //                }
        //                sum += prod;
        //            }
        //            return sum;
        //        }
        //    };
        //}
        //        };
        //    }

        /// <summary>
        /// Convert a {@link UnivariateDifferentiableFunction} into a {@link DifferentiableUnivariateFunction}.
        /// 
        /// <summary>
        /// <param name="f">function to convert</param>
        /// <returns>converted function</returns>
        /// [Obsolete("Deprecated", true)] this conversion method is temporary in version 3.1, as the {@link
        /// DifferentiableUnivariateFunction} interface itself is deprecated
        //    [Obsolete("Deprecated", true)]
        //    public static DifferentiableUnivariateFunction toDifferentiableUnivariateFunction(UnivariateDifferentiableFunction f)
        //{
        //    return new DifferentiableUnivariateFunction()
        //    {

        ///// <summary>{@inheritDoc} */
        //            public double value(double x)
        //    {
        //        return f.Value(x);
        //    }

        ///// <summary>{@inheritDoc} */
        //    public UnivariateFunction derivative()
        //    {
        //        return new UnivariateFunction()
        //        {
        ///// <summary>{@inheritDoc} */
        //                    public double value(double x)
        //        {
        //            return f.Value(new DerivativeStructure(1, 1, 0, x)).getPartialDerivative(1);
        //        }
        //    };
        //}

        //        };
        //    }

        /// <summary>
        /// Convert a {@link DifferentiableUnivariateFunction} into a {@link UnivariateDifferentiableFunction}.
        /// <p>
        /// Note that the converted function is able to handle {@link DerivativeStructure} up to order one.
        /// If the function is called with higher order, a {@link NumberIsTooLargeException} is thrown.
        /// </p>
        /// <summary>
        /// <param name="f">function to convert</param>
        /// <returns>converted function</returns>
        /// [Obsolete("Deprecated", true)] this conversion method is temporary in version 3.1, as the {@link
        /// DifferentiableUnivariateFunction} interface itself is deprecated
        //    [Obsolete("Deprecated", true)]
        //    public static UnivariateDifferentiableFunction toUnivariateDifferential(DifferentiableUnivariateFunction f)
        //{
        //    return new UnivariateDifferentiableFunction()
        //    {

        ///// <summary>{@inheritDoc} */
        //            public double value(double x)
        //    {
        //        return f.Value(x);
        //    }

        ///// <summary>{@inheritDoc}
        ///// <summary>
        ///// <exception cref="NumberIsTooLargeException">if derivation order is greater than 1 </exception>
        //    public DerivativeStructure value(DerivativeStructure t)
        //                throws NumberIsTooLargeException {
        //        switch (t.getOrder())
        //        {
        //            case 0:
        //                return new DerivativeStructure(t.getFreeParameters(), 0, f.Value(t.Value));
        //            case 1:
        //                {
        //                    int parameters = t.getFreeParameters();
        //                    double[] derivatives = new double[parameters + 1];
        //                    derivatives[0] = f.Value(t.Value);
        //                    double fPrime = f.derivative().Value(t.Value);
        //                    int[] orders = new int[parameters];
        //                    for (int i = 0; i < parameters; ++i)
        //                    {
        //                        orders[i] = 1;
        //                        derivatives[i + 1] = fPrime * t.getPartialDerivative(orders);
        //                        orders[i] = 0;
        //                    }
        //                    return new DerivativeStructure(parameters, 1, derivatives);
        //                }
        //            default:
        //                throw new NumberIsTooLargeException(t.getOrder(), 1, true);
        //        }
        //    }

        //};
        //    }

        /// <summary>
        /// Convert a {@link MultivariateDifferentiableFunction} into a {@link DifferentiableMultivariateFunction}.
        /// 
        /// <summary>
        /// <param name="f">function to convert</param>
        /// <returns>converted function</returns>
        /// [Obsolete("Deprecated", true)] this conversion method is temporary in version 3.1, as the {@link
        /// DifferentiableMultivariateFunction} interface itself is deprecated
        //    [Obsolete("Deprecated", true)]
        //    public static DifferentiableMultivariateFunction toDifferentiableMultivariateFunction(MultivariateDifferentiableFunction f)
        //{
        //    return new DifferentiableMultivariateFunction()
        //    {

        ///// <summary>{@inheritDoc} */
        //            public double value(double[] x)
        //    {
        //        return f.Value(x);
        //    }

        ///// <summary>{@inheritDoc} */
        //    public MultivariateFunction partialDerivative(int k)
        //    {
        //        return new MultivariateFunction()
        //        {
        ///// <summary>{@inheritDoc} */
        //                    public double value(double[] x)
        //        {

        //            int n = x.Length;

        //            // delegate computation to underlying function
        //            DerivativeStructure[] dsX = new DerivativeStructure[n];
        //            for (int i = 0; i < n; ++i)
        //            {
        //                if (i == k)
        //                {
        //                    dsX[i] = new DerivativeStructure(1, 1, 0, x[i]);
        //                }
        //                else
        //                {
        //                    dsX[i] = new DerivativeStructure(1, 1, x[i]);
        //                }
        //            }
        //            DerivativeStructure y = f.Value(dsX);

        //            // extract partial derivative
        //            return y.getPartialDerivative(1);

        //        }
        //    };
        //}

        ///// <summary>{@inheritDoc} */
        //public MultivariateVectorFunction gradient()
        //{
        //    return new MultivariateVectorFunction()
        //    {
        ///// <summary>{@inheritDoc} */
        //                    public double[] value(double[] x)
        //    {

        //        int n = x.Length;

        //        // delegate computation to underlying function
        //        DerivativeStructure[] dsX = new DerivativeStructure[n];
        //        for (int i = 0; i < n; ++i)
        //        {
        //            dsX[i] = new DerivativeStructure(n, 1, i, x[i]);
        //        }
        //        DerivativeStructure y = f.Value(dsX);

        //        // extract gradient
        //        double[] gradient = new double[n];
        //        int[] orders = new int[n];
        //        for (int i = 0; i < n; ++i)
        //        {
        //            orders[i] = 1;
        //            gradient[i] = y.getPartialDerivative(orders);
        //            orders[i] = 0;
        //        }

        //        return gradient;

        //    }
        //};
        //            }

        //        };
        //    }

        /// <summary>
        /// Convert a {@link DifferentiableMultivariateFunction} into a {@link MultivariateDifferentiableFunction}.
        /// <p>
        /// Note that the converted function is able to handle {@link DerivativeStructure} elements
        /// that all have the same number of free parameters and order, and with order at most 1.
        /// If the function is called with inconsistent numbers of free parameters or higher order, a
        /// {@link DimensionMismatchException} or a {@link NumberIsTooLargeException} will be thrown.
        /// </p>
        /// <summary>
        /// <param name="f">function to convert</param>
        /// <returns>converted function</returns>
        /// [Obsolete("Deprecated", true)] this conversion method is temporary in version 3.1, as the {@link
        /// DifferentiableMultivariateFunction} interface itself is deprecated
        //    [Obsolete("Deprecated", true)]
        //    public static MultivariateDifferentiableFunction toMultivariateDifferentiableFunction(DifferentiableMultivariateFunction f)
        //{
        //    return new MultivariateDifferentiableFunction()
        //    {

        ///// <summary>{@inheritDoc} */
        //            public double value(double[] x)
        //    {
        //        return f.Value(x);
        //    }

        ///// <summary>{@inheritDoc}
        ///// <summary>
        ///// <exception cref="NumberIsTooLargeException">if derivation order is higher than 1 </exception>
        ///// <exception cref="DimensionMismatchException">if numbers of free parameters are inconsistent </exception>
        //    public DerivativeStructure value(DerivativeStructure[] t)
        //                throws DimensionMismatchException, NumberIsTooLargeException {

        //        // check parameters and orders limits
        //        int parameters = t[0].getFreeParameters();
        //        int order = t[0].getOrder();
        //        int n = t.Length;
        //        if (order > 1)
        //        {
        //            throw new NumberIsTooLargeException(order, 1, true);
        //        }

        //        // check all elements in the array are consistent
        //        for (int i = 0; i < n; ++i)
        //        {
        //            if (t[i].getFreeParameters() != parameters)
        //            {
        //                throw new DimensionMismatchException(t[i].getFreeParameters(), parameters);
        //            }

        //            if (t[i].getOrder() != order)
        //            {
        //                throw new DimensionMismatchException(t[i].getOrder(), order);
        //            }
        //        }

        //        // delegate computation to underlying function
        //        double[] point = new double[n];
        //        for (int i = 0; i < n; ++i)
        //        {
        //            point[i] = t[i].Value;
        //        }
        //        double value = f.Value(point);
        //        double[] gradient = f.gradient().Value(point);

        //        // merge value and gradient into one DerivativeStructure
        //        double[] derivatives = new double[parameters + 1];
        //        derivatives[0] = value;
        //        int[] orders = new int[parameters];
        //        for (int i = 0; i < parameters; ++i)
        //        {
        //            orders[i] = 1;
        //            for (int j = 0; j < n; ++j)
        //            {
        //                derivatives[i + 1] += gradient[j] * t[j].getPartialDerivative(orders);
        //            }
        //            orders[i] = 0;
        //        }

        //        return new DerivativeStructure(parameters, order, derivatives);

        //    }

        //};
        //    }

        /// <summary>
        /// Convert a {@link MultivariateDifferentiableVectorFunction} into a {@link DifferentiableMultivariateVectorFunction}.
        /// 
        /// <summary>
        /// <param name="f">function to convert</param>
        /// <returns>converted function</returns>
        /// [Obsolete("Deprecated", true)] this conversion method is temporary in version 3.1, as the {@link
        /// DifferentiableMultivariateVectorFunction} interface itself is deprecated
        //    [Obsolete("Deprecated", true)]
        //    public static DifferentiableMultivariateVectorFunction toDifferentiableMultivariateVectorFunction(MultivariateDifferentiableVectorFunction f)
        //{
        //    return new DifferentiableMultivariateVectorFunction()
        //    {

        ///// <summary>{@inheritDoc} */
        //            public double[] value(double[] x)
        //    {
        //        return f.Value(x);
        //    }

        ///// <summary>{@inheritDoc} */
        //    public MultivariateMatrixFunction jacobian()
        //    {
        //        return new MultivariateMatrixFunction()
        //        {
        ///// <summary>{@inheritDoc} */
        //                    public double[][] value(double[] x)
        //        {

        //            int n = x.Length;

        //            // delegate computation to underlying function
        //            DerivativeStructure[] dsX = new DerivativeStructure[n];
        //            for (int i = 0; i < n; ++i)
        //            {
        //                dsX[i] = new DerivativeStructure(n, 1, i, x[i]);
        //            }
        //            DerivativeStructure[] y = f.Value(dsX);

        //            // extract Jacobian
        //            double[][] jacobian = new double[y.Length][n];
        //            int[] orders = new int[n];
        //            for (int i = 0; i < y.Length; ++i)
        //            {
        //                for (int j = 0; j < n; ++j)
        //                {
        //                    orders[j] = 1;
        //                    jacobian[i][j] = y[i].getPartialDerivative(orders);
        //                    orders[j] = 0;
        //                }
        //            }

        //            return jacobian;

        //        }
        //    };
        //}

        //        };
        //    }

        /// <summary>
        /// Convert a {@link DifferentiableMultivariateVectorFunction} into a {@link MultivariateDifferentiableVectorFunction}.
        /// <p>
        /// Note that the converted function is able to handle {@link DerivativeStructure} elements
        /// that all have the same number of free parameters and order, and with order at most 1.
        /// If the function is called with inconsistent numbers of free parameters or higher order, a
        /// {@link DimensionMismatchException} or a {@link NumberIsTooLargeException} will be thrown.
        /// </p>
        /// <summary>
        /// <param name="f">function to convert</param>
        /// <returns>converted function</returns>
        /// [Obsolete("Deprecated", true)] this conversion method is temporary in version 3.1, as the {@link
        /// DifferentiableMultivariateFunction} interface itself is deprecated
        //    [Obsolete("Deprecated", true)]
        //    public static MultivariateDifferentiableVectorFunction toMultivariateDifferentiableVectorFunction(DifferentiableMultivariateVectorFunction f)
        //{
        //    return new MultivariateDifferentiableVectorFunction()
        //    {

        ///// <summary>{@inheritDoc} */
        //            public double[] value(double[] x)
        //    {
        //        return f.Value(x);
        //    }

        ///// <summary>{@inheritDoc}
        ///// <summary>
        ///// <exception cref="NumberIsTooLargeException">if derivation order is higher than 1 </exception>
        ///// <exception cref="DimensionMismatchException">if numbers of free parameters are inconsistent </exception>
        //    public DerivativeStructure[] value(DerivativeStructure[] t)
        //                throws DimensionMismatchException, NumberIsTooLargeException {

        //        // check parameters and orders limits
        //        int parameters = t[0].getFreeParameters();
        //        int order = t[0].getOrder();
        //        int n = t.Length;
        //        if (order > 1)
        //        {
        //            throw new NumberIsTooLargeException(order, 1, true);
        //        }

        //        // check all elements in the array are consistent
        //        for (int i = 0; i < n; ++i)
        //        {
        //            if (t[i].getFreeParameters() != parameters)
        //            {
        //                throw new DimensionMismatchException(t[i].getFreeParameters(), parameters);
        //            }

        //            if (t[i].getOrder() != order)
        //            {
        //                throw new DimensionMismatchException(t[i].getOrder(), order);
        //            }
        //        }

        //        // delegate computation to underlying function
        //        double[] point = new double[n];
        //        for (int i = 0; i < n; ++i)
        //        {
        //            point[i] = t[i].Value;
        //        }
        //        double[] value = f.Value(point);
        //        double[][] jacobian = f.jacobian().Value(point);

        //        // merge value and Jacobian into a DerivativeStructure array
        //        DerivativeStructure[] merged = new DerivativeStructure[value.Length];
        //        for (int k = 0; k < merged.Length; ++k)
        //        {
        //            double[] derivatives = new double[parameters + 1];
        //            derivatives[0] = value[k];
        //            int[] orders = new int[parameters];
        //            for (int i = 0; i < parameters; ++i)
        //            {
        //                orders[i] = 1;
        //                for (int j = 0; j < n; ++j)
        //                {
        //                    derivatives[i + 1] += jacobian[k][j] * t[j].getPartialDerivative(orders);
        //                }
        //                orders[i] = 0;
        //            }
        //            merged[k] = new DerivativeStructure(parameters, order, derivatives);
        //        }

        //        return merged;

        //    }

        //};
        //    }
        #endregion

        public sealed class UnivariateDifferentiableFunction : IUnivariateDifferentiableFunction
        {
            double _paramValue;
            public Func<double?, double?> function1;
            public Func<DerivativeStructure, DerivativeStructure> function2;

            public double ParamValue
            {
                get
                {
                    return _paramValue;
                }
            }

            public DerivativeStructure Value(DerivativeStructure t)
            {
                return function2(t);
            }

            public double Value(double x)
            {
                _paramValue = x;
                return function1(x).Value;
            }
        }
    }
}
