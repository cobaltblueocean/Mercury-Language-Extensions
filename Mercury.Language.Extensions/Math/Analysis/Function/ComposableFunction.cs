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

namespace Mercury.Language.Math.Analysis.Function
{
    /// <summary>
    /// Base class for {@link UnivariateRealFunction} that can be composed with other functions.
    /// 
    /// @since 2.1
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// </summary>
    public class ComposableFunction : IUnivariateRealFunction
    {
        Func<double, double> function;

        IBivariateRealFunction ADD = new BinaryAddFunction();


        /// <summary>The constant function always returning 0d */
        public static ComposableFunction ZERO = new ComposableFunction()
        {
            function = new Func<double, double>((d) =>
            {
                return 0;
            })
        };

        /// <summary>The constant function always returning 1d */
        public static ComposableFunction ONE = new ComposableFunction()
        {
            function = new Func<double, double>((d) =>
            {
                return 1;
            })
        };

        /// <summary>The identity functiond */
        public static ComposableFunction IDENTITY = new ComposableFunction()
        {
            function = new Func<double, double>((d) =>
            {
                return d;
            })
        };

        /// <summary>The {@code System.Math.abs} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction ABS = new ComposableFunction()
        {
            function = new Func<double, double>((d) =>
            {
                return System.Math.Abs(d);
            })
        };

        /// <summary>The - operator wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction NEGATE = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return -d;
            })
        };

        /// <summary>The invert operator wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction INVERT = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return 1 / d;
            })
        };

        /// <summary>The {@code System.Math.sin} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction SIN = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Sin(d);
            })
        };

        /// <summary>The {@code System.Math.sqrt} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction SQRT = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Sqrt(d);
            })
        };

        /// <summary>The {@code System.Math.sinh} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction SINH = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Sinh(d);
            })
        };

        /// <summary>The {@code System.Math.exp} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction EXP = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Exp(d);
            })
        };

        /// <summary>The {@code System.Math.expm1} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction EXPM1 = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math2.Expm1(d);
            })
        };

        /// <summary>The {@code System.Math.asin} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction ASIN = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Asin(d);
            })
        };

        /// <summary>The {@code System.Math.atan} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction ATAN = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Atan(d);
            })
        };

        /// <summary>The {@code System.Math.tan} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction TAN = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Tan(d);
            })
        };

        /// <summary>The {@code System.Math.tanh} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction TANH = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Tanh(d);
            })
        };

        /// <summary>The {@code System.Math.cbrt} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction CBRT = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Cbrt(d);
            })
        };

        /// <summary>The {@code System.Math.ceil} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction CEIL = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Ceiling(d);
            })
        };

        /// <summary>The {@code System.Math.floor} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction FLOOR = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Floor(d);
            })
        };

        /// <summary>The {@code System.Math.log} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction LOG = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Log(d);
            })
        };

        /// <summary>The {@code System.Math.log10} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction LOG10 = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Log10(d);
            })
        };

        /// <summary>The {@code System.Math.log1p} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction LOG1P = new ComposableFunction()
        {
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math2.Log1p(d);
            })
        };

        /// <summary>The {@code System.Math.cos} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction COS = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Cos(d);
            })
        };

        /// <summary>The {@code System.Math.abs} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction ACOS = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Acos(d);
            })
        };

        /// <summary>The {@code System.Math.cosh} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction COSH = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Cosh(d);
            })
        };

        /// <summary>The {@code System.Math.rint} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction RINT = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Round(d);
            })
        };

        /// <summary>The {@code System.Math.signum} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction SIGNUM = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math.Sign(d);
            })
        };

        /// <summary>The {@code System.Math.ulp} method wrapped as a {@link ComposableFunction}d */
        public static ComposableFunction ULP = new ComposableFunction()
        {
            /// <summary>{@inheritDoc} */
            // @Override
            function = new Func<double, double>((d) =>
            {
                return System.Math2.Ulp(d);
            })
        };

        public double ParamValue => throw new NotImplementedException();

        /// <summary>Precompose the instance with another function.
        /// <p>
        /// The composed function h created by {@code h = g.Of(f)} is such
        /// that {@code h.Value(x) == g.Value(f.Value(x))} for all x.
        /// </p>
        /// </summary>
        /// <param Name="f">function to compose with</param>
        /// <returns>a new function which computes {@code this.Value(f.Value(x))}</returns>
        /// <see cref="#postCompose(UnivariateRealFunction)"></see>
        public ComposableFunction Of(UnivariateRealFunction f)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(f.Value(x));
                })
            };
        }

        /// <summary>Postcompose the instance with another function.
        /// <p>
        /// The composed function h created by {@code h = g.postCompose(f)} is such
        /// that {@code h.Value(x) == f.Value(g.Value(x))} for all x.
        /// </p>
        /// </summary>
        /// <param Name="f">function to compose with</param>
        /// <returns>a new function which computes {@code f.Value(this.Value(x))}</returns>
        /// <see cref="#Of(UnivariateRealFunction)"></see>
        public ComposableFunction PostCompose(UnivariateRealFunction f)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return f.Value(_base.Value(x));
                })
            };
        }

        /// <summary>
        /// Return a function combining the instance and another function.
        /// <p>
        /// The function h created by {@code h = g.combine(f, combiner)} is such that
        /// {@code h.Value(x) == combiner.Value(g.Value(x), f.Value(x))} for all x.
        /// </p>
        /// </summary>
        /// <param Name="f">function to combine with the instance</param>
        /// <param Name="combiner">bivariate function used for combining</param>
        /// <returns>a new function which computes {@code combine.Value(this.Value(x), f.Value(x))}</returns>
        public ComposableFunction Combine(UnivariateRealFunction f, IBivariateRealFunction combiner)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return combiner.Value(_base.Value(x), f.Value(x));
                })
            };
        }

        /// <summary>
        /// Return a function adding the instance and another function.
        /// </summary>
        /// <param Name="f">function to combine with the instance</param>
        /// <returns>a new function which computes {@code this.Value(x) + f.Value(x)}</returns>
        public ComposableFunction Add(UnivariateRealFunction f)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(x) + f.Value(x);
                })
            };
        }

        /// <summary>
        /// Return a function adding a constant term to the instance.
        /// </summary>
        /// <param Name="a">term to add</param>
        /// <returns>a new function which computes {@code this.Value(x) + a}</returns>
        public ComposableFunction Add(double a)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(x) + a;
                })
            };
        }

        /// <summary>
        /// Return a function subtracting another function from the instance.
        /// </summary>
        /// <param Name="f">function to combine with the instance</param>
        /// <returns>a new function which computes {@code this.Value(x) - f.Value(x)}</returns>
        public ComposableFunction Subtract(UnivariateRealFunction f)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(x) - f.Value(x);
                })
            };
        }

        /// <summary>
        /// Return a function multiplying the instance and another function.
        /// </summary>
        /// <param Name="f">function to combine with the instance</param>
        /// <returns>a new function which computes {@code this.Value(x) * f.Value(x)}</returns>
        public ComposableFunction Multiply(UnivariateRealFunction f)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(x) * f.Value(x);
                })
            };
        }

        /// <summary>
        /// Return a function scaling the instance by a constant factor.
        /// </summary>
        /// <param Name="scaleFactor">constant scaling factor</param>
        /// <returns>a new function which computes {@code this.Value(x) * scaleFactor}</returns>
        public ComposableFunction Multiply(double scaleFactor)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(x) * scaleFactor;
                })
            };
        }
        /// <summary>
        /// Return a function dividing the instance by another function.
        /// </summary>
        /// <param Name="f">function to combine with the instance</param>
        /// <returns>a new function which computes {@code this.Value(x) / f.Value(x)}</returns>
        public ComposableFunction Divide(UnivariateRealFunction f)
        {
            ComposableFunction _base = this;

            return new ComposableFunction()
            {
                // @Override
                /// <summary>{@inheritDoc} */
                function = new Func<double, double>((x) =>
                {
                    return _base.Value(x) / f.Value(x);
                })
            };
        }

        /// <summary>
        /// Generates a function that iteratively apply instance function on all
        /// elements of an array.
        /// <p>
        /// The generated function behaves as follows:
        /// <ul>
        ///   <li>initialize result = initialValue</li>
        ///   <li>iterate: {@code result = combiner.Value(result,
        ///   this.Value(nextMultivariateEntry));}</li>
        ///   <li>return result</li>
        /// </ul>
        /// </p>
        /// </summary>
        /// <param Name="combiner">combiner to use between entries</param>
        /// <param Name="initialValue">initial value to use before first entry</param>
        /// <returns>a new function that iteratively apply instance function on all</returns>
        /// elements of an array.
        public IMultivariateRealFunction AsCollector(IBivariateRealFunction combiner, double initialValue)
        {
            ComposableFunction _base = this;

            return new MultivariateRealFunction()
            {
                function = new Func<double[], double>((point) =>
                {
                    double result = initialValue;
                    AutoParallel.AutoParallelForEach(point, (entry) =>
                    {
                        result = combiner.Value(result, _base.Value(entry));
                    });
                    return result;
                })
            };
        }

        /// <summary>
        /// Generates a function that iteratively apply instance function on all
        /// elements of an array.
        /// <p>
        /// Calling this method is equivalent to call {@link
        /// #asCollector(BivariateRealFunction, double) asCollector(BivariateRealFunction, 0.0)}.
        /// </p>
        /// </summary>
        /// <param Name="combiner">combiner to use between entries</param>
        /// <returns>a new function that iteratively apply instance function on all</returns>
        /// elements of an array.
        /// <see cref="#asCollector(BivariateRealFunction,">double) </see>
        public IMultivariateRealFunction AsCollector(IBivariateRealFunction combiner)
        {
            return AsCollector(combiner, 0.0);
        }

        /// <summary>
        /// Generates a function that iteratively apply instance function on all
        /// elements of an array.
        /// <p>
        /// Calling this method is equivalent to call {@link
        /// #asCollector(BivariateRealFunction, double) asCollector(BinaryFunction.ADD, initialValue)}.
        /// </p>
        /// </summary>
        /// <param Name="initialValue">initial value to use before first entry</param>
        /// <returns>a new function that iteratively apply instance function on all</returns>
        /// elements of an array.
        /// <see cref="#asCollector(BivariateRealFunction,">double) </see>
        /// <see cref="BinaryFunction#ADD"></see>
        public IMultivariateRealFunction AsCollector(double initialValue)
        {
            return AsCollector(ADD, initialValue);
        }

        /// <summary>
        /// Generates a function that iteratively apply instance function on all
        /// elements of an array.
        /// <p>
        /// Calling this method is equivalent to call {@link
        /// #asCollector(BivariateRealFunction, double) asCollector(BinaryFunction.ADD, 0.0)}.
        /// </p>
        /// </summary>
        /// <returns>a new function that iteratively apply instance function on all</returns>
        /// elements of an array.
        /// <see cref="#asCollector(BivariateRealFunction,">double) </see>
        /// <see cref="BinaryFunction#ADD"></see>
        public IMultivariateRealFunction AsCollector()
        {
            return AsCollector(ADD, 0.0);
        }

        /// <summary>{@inheritDoc} */
        public virtual double Value(double x)
        {
            return function(x);
        }

        private class BinaryAddFunction : IBivariateRealFunction
        {
            public double Value(double x, double y)
            {
                return x + y;
            }
        }
    }
}
