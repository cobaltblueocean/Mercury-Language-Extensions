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
//     http://www.Apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;

namespace Mercury.Language.Math.Analysis.Differentiation
{
    /// <summary>
    /// Class representing both the value and the differentials of a function.
    /// <p>This class is the workhorse of the differentiation package.</p>
    /// <p>This class is an implementation of the extension to Rall's
    /// numbers described in Dan Kalman's paper <a
    /// href="http://www1.American.edu/cas/mathstat/People/kalman/pdffiles/mmgautodiff.Pdf">Doubly
    /// Recursive Multivariate Automatic Differentiation</a>, Mathematics Magazine, vold 75,
    /// nod 3, June 2002d Rall's numbers are an extension to the real numbers used
    /// throughout mathematical expressions; they hold the derivative together with the
    /// value of a functiond Dan Kalman's derivative structures hold all partial derivatives
    /// up to any specified order, with respect to any number of free parametersd Rall's
    /// numbers therefore can be seen as derivative structures for order one derivative and
    /// one free parameter, and real numbers can be seen as derivative structures with zero
    /// order derivative and no free parameters.</p>
    /// <p><see cref="DerivativeStructure"/> instances can be used directly thanks to
    /// the arithmetic operators to the mathematical functions provided as
    /// methods by this class (+, -, *, /, %, sin, cos ..d).</p>
    /// <p>Implementing complex expressions by hand using these classes is
    /// a tedious and error-prone task but has the advantage of having no limitation
    /// on the derivation order despite no requiring users to compute the derivatives by
    /// themselvesd Implementing complex expression can also be done by developing computation
    /// code using standard primitive double values and to use <see cref="UnivariateFunctionDifferentiator"/> 
    /// to create the <see cref="DerivativeStructure"/>-based instancesd This method is simpler but may be limited in
    /// the accuracy and derivation orders and may be computationally intensive (this is
    /// typically the case for <see cref="FiniteDifferencesDifferentiator">finite differences differentiator</see>.</p>
    /// <p>Instances of this class are guaranteed to be immutable.</p>
    /// </summary>
    /// <see cref="DSCompiler"/>
    [Serializable]
    public class DerivativeStructure : IRealFieldElement<DerivativeStructure>
    {
        #region Local Variables
        public IField<DerivativeStructure> Field
        {
            get { return new DerivativeStructureField(compiler); }
        }

        /// <summary>Compiler for the current dimensionsd */
        [NonSerialized]
        private DSCompiler compiler;

        /// <summary>Combined array holding all valuesd */
        private double[] data;

        #endregion

        #region Property
        /// <summary>Get the number of free parameters.
        /// <summary>
        /// <returns>number of free parameters</returns>
        public int FreeParameters
        {
            get { return compiler.FreeParameters; }
        }

        /// <summary>Get the derivation order.
        /// <summary>
        /// <returns>derivation order</returns>
        public int Order
        {
            get { return compiler.Order; }
        }

        #endregion

        #region Constructor

        /// <summary>Build an instance with all values and derivatives set to 0.
        /// <summary>
        /// <param name="compiler">compiler to use for computation</param>
        private DerivativeStructure(DSCompiler compiler)
        {
            this.compiler = compiler;
            this.data = new double[compiler.Size];
        }

        /// <summary>Build an instance with all values and derivatives set to 0.
        /// <summary>
        /// <param name="parameters">number of free parameters</param>
        /// <param name="order">derivation order</param>
        /// <exception cref="NumberIsTooLargeException">if order is too large </exception>
        public DerivativeStructure(int parameters, int order) : this(DSCompiler.GetCompiler(parameters, order))
        {

        }

        /// <summary>Build an instance representing a constant value.
        /// <summary>
        /// <param name="parameters">number of free parameters</param>
        /// <param name="order">derivation order</param>
        /// <param name="value">value of the constant</param>
        /// <exception cref="NumberIsTooLargeException">if order is too large </exception>
        /// <see cref="#DerivativeStructure(int,">int, int, double) </see>
        public DerivativeStructure(int parameters, int order, double value) : this(parameters, order)
        {
            this.data[0] = value;
        }

        /// <summary>Build an instance representing a variable.
        /// <p>Instances built using this constructor are considered
        /// to be the free variables with respect to which differentials
        /// are computedd As such, their differential with respect to
        /// themselves is +1.</p>
        /// <summary>
        /// <param name="parameters">number of free parameters</param>
        /// <param name="order">derivation order</param>
        /// <param name="index">index of the variable (from 0 to {@code parameters - 1})</param>
        /// <param name="value">value of the variable</param>
        /// <exception cref="NumberIsTooLargeException">if {@code index >= parameters}. </exception>
        /// <see cref="#DerivativeStructure(int,">int, double) </see>
        public DerivativeStructure(int parameters, int order,
                                   int index, double value) : this(parameters, order, value)
        {


            if (index >= parameters)
            {
                throw new NumberIsTooLargeException(index, parameters, false);
            }

            if (order > 0)
            {
                // the derivative of the variable with respect to itself is 1.
                data[DSCompiler.GetCompiler(index, order).Size] = 1.0;
            }

        }

        /// <summary>Linear combination constructor.
        /// The derivative structure built will be a1 * ds1 + a2 * ds2
        /// <summary>
        /// <param name="a1">first scale factor</param>
        /// <param name="ds1">first base (unscaled) derivative structure</param>
        /// <param name="a2">second scale factor</param>
        /// <param name="ds2">second base (unscaled) derivative structure</param>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent </exception>
        public DerivativeStructure(double a1, DerivativeStructure ds1,
                                   double a2, DerivativeStructure ds2) : this(ds1.compiler)
        {
            int resultOffset = 0;
            compiler.CheckCompatibility(ds2.compiler);
            compiler.LinearCombination(a1, ds1.data, 0, a2, ds2.data, 0, ref data, ref resultOffset);
        }

        /// <summary>Linear combination constructor.
        /// The derivative structure built will be a1 * ds1 + a2 * ds2 + a3 * ds3
        /// <summary>
        /// <param name="a1">first scale factor</param>
        /// <param name="ds1">first base (unscaled) derivative structure</param>
        /// <param name="a2">second scale factor</param>
        /// <param name="ds2">second base (unscaled) derivative structure</param>
        /// <param name="a3">third scale factor</param>
        /// <param name="ds3">third base (unscaled) derivative structure</param>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent </exception>
        public DerivativeStructure(double a1, DerivativeStructure ds1,
                                   double a2, DerivativeStructure ds2,
                                   double a3, DerivativeStructure ds3) : this(ds1.compiler)
        {
            int resultOffset = 0;
            compiler.CheckCompatibility(ds2.compiler);
            compiler.CheckCompatibility(ds3.compiler);
            compiler.LinearCombination(a1, ds1.data, 0, a2, ds2.data, 0, a3, ds3.data, 0, ref data, ref resultOffset);
        }

        /// <summary>Linear combination constructor.
        /// The derivative structure built will be a1 * ds1 + a2 * ds2 + a3 * ds3 + a4 * ds4
        /// <summary>
        /// <param name="a1">first scale factor</param>
        /// <param name="ds1">first base (unscaled) derivative structure</param>
        /// <param name="a2">second scale factor</param>
        /// <param name="ds2">second base (unscaled) derivative structure</param>
        /// <param name="a3">third scale factor</param>
        /// <param name="ds3">third base (unscaled) derivative structure</param>
        /// <param name="a4">fourth scale factor</param>
        /// <param name="ds4">fourth base (unscaled) derivative structure</param>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent </exception>
        public DerivativeStructure(double a1, DerivativeStructure ds1,
                                   double a2, DerivativeStructure ds2,
                                   double a3, DerivativeStructure ds3,
                                   double a4, DerivativeStructure ds4) : this(ds1.compiler)
        {

            int resultOffset = 0;
            compiler.CheckCompatibility(ds2.compiler);
            compiler.CheckCompatibility(ds3.compiler);
            compiler.CheckCompatibility(ds4.compiler);
            compiler.LinearCombination(a1, ds1.data, 0, a2, ds2.data, 0,
                                       a3, ds3.data, 0, a4, ds4.data, 0,
                                       ref data, ref resultOffset);
        }

        /// <summary>Build an instance from all its derivatives.
        /// <summary>
        /// <param name="parameters">number of free parameters</param>
        /// <param name="order">derivation order</param>
        /// <param name="derivatives">derivatives sorted according to</param>
        /// {@link DSCompiler#getPartialDerivativeIndex(int..d)}
        /// <exception cref="DimensionMismatchException">if derivatives array does not match the </exception>
        /// {@link DSCompiler#getSize() size} expected by the compiler
        /// <exception cref="NumberIsTooLargeException">if order is too large </exception>
        /// <see cref="#GetAllDerivatives()"></see>
        public DerivativeStructure(int parameters, int order, params double[] derivatives) : this(parameters, order)
        {

            if (derivatives.Length != data.Length)
            {
                throw new DimensionMismatchException(derivatives.Length, data.Length);
            }
            Array.Copy(derivatives, 0, data, 0, data.Length);
        }

        /// <summary>Copy constructor.
        /// <summary>
        /// <param name="ds">instance to copy</param>
        private DerivativeStructure(DerivativeStructure ds)
        {
            this.compiler = ds.compiler;
            this.data = ds.data.Copy();
        }
        #endregion

        #region Implement Methods
        public DerivativeStructure Abs()
        {
            if (BitConverter.DoubleToInt64Bits(data[0]) < 0)
            {
                // we use the bits representation to also handle -0.0
                return Negate();
            }
            else
            {
                return this;
            }
        }

        public DerivativeStructure Acos()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Acos(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Acosh()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Acosh(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Add(double a)
        {
            DerivativeStructure ds = new DerivativeStructure(this);
            ds.data[0] += a;
            return ds;
        }

        public DerivativeStructure Add(DerivativeStructure a)
        {
            compiler.CheckCompatibility(a.compiler);
            DerivativeStructure ds = new DerivativeStructure(this);
            compiler.Add(data, 0, a.data, 0, ds.data, 0);
            return ds;
        }

        public DerivativeStructure Asin()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Asin(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Asinh()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Asinh(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Atan()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Atan(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Atan2(DerivativeStructure x)
        {
            compiler.CheckCompatibility(x.compiler);
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Atan2(data, 0, x.data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Atanh()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Atanh(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Cbrt()
        {
            return RootN(3);
        }

        public DerivativeStructure Ceil()
        {
            return new DerivativeStructure(compiler.FreeParameters,
                                       compiler.Order,
                                       System.Math.Ceiling(data[0]));
        }

        public DerivativeStructure CopySign(DerivativeStructure sign)
        {
            long m = BitConverter.DoubleToInt64Bits(data[0]);
            long s = BitConverter.DoubleToInt64Bits(sign.data[0]);
            if ((m >= 0 && s >= 0) || (m < 0 && s < 0))
            { // Sign is currently OK
                return this;
            }
            return Negate(); // flip sign
        }

        public DerivativeStructure CopySign(double sign)
        {
            long m = BitConverter.DoubleToInt64Bits(data[0]);
            long s = BitConverter.DoubleToInt64Bits(sign);
            if ((m >= 0 && s >= 0) || (m < 0 && s < 0))
            { // Sign is currently OK
                return this;
            }
            return Negate(); // flip sign
        }

        public DerivativeStructure Cos()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Cos(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Cosh()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Cosh(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Divide(double a)
        {
            DerivativeStructure ds = new DerivativeStructure(this);
            for (int i = 0; i < ds.data.Length; ++i)
            {
                ds.data[i] /= a;
            }
            return ds;
        }

        public DerivativeStructure Divide(DerivativeStructure a)
        {
            compiler.CheckCompatibility(a.compiler);
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Divide(data, 0, a.data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Expm1()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Expm1(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Expo()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Exp(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Floor()
        {
            return new DerivativeStructure(compiler.FreeParameters,
                                       compiler.Order,
                                       System.Math.Floor(data[0]));
        }

        public DerivativeStructure Hypot(DerivativeStructure y)
        {
            compiler.CheckCompatibility(y.compiler);

            if (Double.IsInfinity(data[0]) || Double.IsInfinity(y.data[0]))
            {
                return new DerivativeStructure(compiler.FreeParameters,
                                               compiler.FreeParameters,
                                               Double.PositiveInfinity);
            }
            else if (Double.IsNaN(data[0]) || Double.IsNaN(y.data[0]))
            {
                return new DerivativeStructure(compiler.FreeParameters,
                                               compiler.FreeParameters,
                                               Double.NaN);
            }
            else
            {

                int expX = GetExponent();
                int expY = y.GetExponent();
                if (expX > expY + 27)
                {
                    // y is neglectible with respect to x
                    return Abs();
                }
                else if (expY > expX + 27)
                {
                    // x is neglectible with respect to y
                    return y.Abs();
                }
                else
                {

                    // find an intermediate scale to avoid both overflow and underflow
                    int middleExp = (expX + expY) / 2;

                    // scale parameters without losing precision
                    DerivativeStructure scaledX = Scalb(-middleExp);
                    DerivativeStructure scaledY = y.Scalb(-middleExp);

                    // compute scaled hypotenuse
                    DerivativeStructure scaledH =
                            scaledX.Multiply(scaledX).Add(scaledY.Multiply(scaledY)).Sqrt();

                    // remove scaling
                    return scaledH.Scalb(middleExp);

                }

            }
        }

        public DerivativeStructure LinearCombination(DerivativeStructure[] a, DerivativeStructure[] b)
        {
            // compute an accurate value, taking care of cancellations
            double[] aDouble = new double[a.Length];
            for (int i = 0; i < a.Length; ++i)
            {
                aDouble[i] = a[i].Value();
            }
            double[] bDouble = new double[b.Length];
            for (int i = 0; i < b.Length; ++i)
            {
                bDouble[i] = b[i].Value();
            }
            double accurateValue = aDouble.LinearCombination(bDouble);

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = a[0].Field.Zero;
            for (int i = 0; i < a.Length; ++i)
            {
                simpleValue = simpleValue.Add(a[i].Multiply(b[i]));
            }

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(simpleValue.FreeParameters, simpleValue.Order, all);
        }

        public DerivativeStructure LinearCombination(double[] a, DerivativeStructure[] b)
        {
            // compute an accurate value, taking care of cancellations
            double[] bDouble = new double[b.Length];
            for (int i = 0; i < b.Length; ++i)
            {
                bDouble[i] = b[i].Value();
            }
            double accurateValue = a.LinearCombination(bDouble);

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = b[0].Field.Zero;
            for (int i = 0; i < a.Length; ++i)
            {
                simpleValue = simpleValue.Add(b[i].Multiply(a[i]));
            }

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(simpleValue.FreeParameters, simpleValue.Order, all);
        }

        public DerivativeStructure LinearCombination(DerivativeStructure a1, DerivativeStructure b1, DerivativeStructure a2, DerivativeStructure b2)
        {
            // compute an accurate value, taking care of cancellations
            double accurateValue = a1.Value().LinearCombination(b1.Value(),
                                                                      a2.Value(), b2.Value());

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = a1.Multiply(b1).Add(a2.Multiply(b2));

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(FreeParameters, Order, all);

        }

        public DerivativeStructure LinearCombination(double a1, DerivativeStructure b1, double a2, DerivativeStructure b2)
        {
            // compute an accurate value, taking care of cancellations
            double accurateValue = a1.LinearCombination(b1.Value(),
                                                                      a2, b2.Value());

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = b1.Multiply(a1).Add(b2.Multiply(a2));

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(FreeParameters, Order, all);
        }

        public DerivativeStructure LinearCombination(DerivativeStructure a1, DerivativeStructure b1, DerivativeStructure a2, DerivativeStructure b2, DerivativeStructure a3, DerivativeStructure b3)
        {
            // compute an accurate value, taking care of cancellations
            double accurateValue = a1.Value().LinearCombination(b1.Value(),
                                                                      a2.Value(), b2.Value(),
                                                                      a3.Value(), b3.Value());

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = a1.Multiply(b1).Add(a2.Multiply(b2)).Add(a3.Multiply(b3));

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(FreeParameters, Order, all);
        }

        public DerivativeStructure LinearCombination(double a1, DerivativeStructure b1, double a2, DerivativeStructure b2, double a3, DerivativeStructure b3)
        {
            // compute an accurate value, taking care of cancellations
            double accurateValue = a1.LinearCombination(b1.Value(),
                                                                      a2, b2.Value(),
                                                                      a3, b3.Value());

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = b1.Multiply(a1).Add(b2.Multiply(a2)).Add(b3.Multiply(a3));

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(FreeParameters, Order, all);
        }

        public DerivativeStructure LinearCombination(DerivativeStructure a1, DerivativeStructure b1, DerivativeStructure a2, DerivativeStructure b2, DerivativeStructure a3, DerivativeStructure b3, DerivativeStructure a4, DerivativeStructure b4)
        {
            // compute an accurate value, taking care of cancellations
            double accurateValue = a1.Value().LinearCombination(b1.Value(),
                                                                      a2.Value(), b2.Value(),
                                                                      a3.Value(), b3.Value(),
                                                                      a4.Value(), b4.Value());

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = a1.Multiply(b1).Add(a2.Multiply(b2)).Add(a3.Multiply(b3)).Add(a4.Multiply(b4));

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(FreeParameters, Order, all);
        }

        public DerivativeStructure LinearCombination(double a1, DerivativeStructure b1, double a2, DerivativeStructure b2, double a3, DerivativeStructure b3, double a4, DerivativeStructure b4)
        {
            // compute an accurate value, taking care of cancellations
            double accurateValue = a1.LinearCombination(b1.Value(),
                                                                      a2, b2.Value(),
                                                                      a3, b3.Value(),
                                                                      a4, b4.Value());

            // compute a simple value, with all partial derivatives
            DerivativeStructure simpleValue = b1.Multiply(a1).Add(b2.Multiply(a2)).Add(b3.Multiply(a3)).Add(b4.Multiply(a4));

            // create a result with accurate value and all derivatives (not necessarily as accurate as the value)
            double[] all = simpleValue.GetAllDerivatives();
            all[0] = accurateValue;
            return new DerivativeStructure(FreeParameters, Order, all);
        }

        public DerivativeStructure Log()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Log(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Log1p()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Log1p(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Multiply(double a)
        {
            DerivativeStructure ds = new DerivativeStructure(this);
            for (int i = 0; i < ds.data.Length; ++i)
            {
                ds.data[i] *= a;
            }
            return ds;
        }

        public DerivativeStructure Multiply(int n)
        {
            return Multiply((double)n);
        }

        public DerivativeStructure Multiply(DerivativeStructure a)
        {
            compiler.CheckCompatibility(a.compiler);
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Multiply(data, 0, a.data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Negate()
        {
            DerivativeStructure ds = new DerivativeStructure(compiler);
            for (int i = 0; i < ds.data.Length; ++i)
            {
                ds.data[i] = -data[i];
            }
            return ds;
        }

        public DerivativeStructure Pow(double p)
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Pow(data, 0, p, result.data, 0);
            return result;
        }

        public DerivativeStructure Pow(int n)
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Pow(data, 0, n, result.data, 0);
            return result;
        }

        public DerivativeStructure Pow(DerivativeStructure e)
        {
            compiler.CheckCompatibility(e.compiler);
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Pow(data, 0, e.data, 0, result.data, 0);
            return result;
        }

        public double Real()
        {
            return data[0];
        }

        public DerivativeStructure Reciprocal()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Pow(data, 0, -1, result.data, 0);
            return result;
        }

        public DerivativeStructure Remainder(double a)
        {
            DerivativeStructure ds = new DerivativeStructure(this);
            ds.data[0] = System.Math.IEEERemainder(ds.data[0], a);
            return ds;
        }

        public DerivativeStructure Remainder(DerivativeStructure a)
        {
            compiler.CheckCompatibility(a.compiler);
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Remainder(data, 0, a.data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Rint()
        {
            return new DerivativeStructure(compiler.FreeParameters,
                                       compiler.Order,
                                       System.Math.Round(data[0]));
        }

        public DerivativeStructure RootN(int n)
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.RootN(data, 0, n, result.data, 0);
            return result;
        }

        public long Round()
        {
            return (long)System.Math.Round(data[0]);
        }

        public DerivativeStructure Scalb(int n)
        {
            DerivativeStructure ds = new DerivativeStructure(compiler);
            for (int i = 0; i < ds.data.Length; ++i)
            {
                ds.data[i] = Math2.Scalb(data[i], n);
            }
            return ds;
        }

        public DerivativeStructure Signum()
        {
            return new DerivativeStructure(compiler.FreeParameters,
                                       compiler.Order,
                                       System.Math.Sign(data[0]));
        }

        public DerivativeStructure Sin()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Sin(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Sinh()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Sinh(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Sqrt()
        {
            return RootN(2);
        }

        public DerivativeStructure Subtract(double a)
        {
            return Add(-a);
        }

        public DerivativeStructure Subtract(DerivativeStructure a)
        {
            compiler.CheckCompatibility(a.compiler);
            DerivativeStructure ds = new DerivativeStructure(this);
            compiler.Subtract(data, 0, a.data, 0, ds.data, 0);
            return ds;
        }

        public DerivativeStructure Tan()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Tan(data, 0, result.data, 0);
            return result;
        }

        public DerivativeStructure Tanh()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Tanh(data, 0, result.data, 0);
            return result;
        }

        /// <summary>Get the value part of the derivative structure.
        /// <summary>
        /// <returns>value part of the derivative structure</returns>
        /// <see cref="#getPartialDerivative(int..d)"></see>
        public double Value()
        {
            return data[0];
        }

        #endregion

        #region Local Public Methods

        /// <summary>Two arguments arc tangent operation.
        /// <summary>
        /// <param name="y">first argument of the arc tangent</param>
        /// <param name="x">second argument of the arc tangent</param>
        /// <returns>atan2(y, x)</returns>
        /// <exception cref="DimensionMismatchException">if number of free parameters </exception>
        /// or orders do not match
        /// @since 3.2
        public static DerivativeStructure Atan2(DerivativeStructure y, DerivativeStructure x)
        {
            return y.Atan2(x);
        }

        /// <summary>Convert radians to degrees, with error of less than 0.5 ULP
        /// <summary>
        ///  <returns>instance converted into degrees</returns>
        public DerivativeStructure ToDegrees()
        {
            DerivativeStructure ds = new DerivativeStructure(compiler);
            for (int i = 0; i < ds.data.Length; ++i)
            {
                ds.data[i] = Math2.ToDegrees(data[i]);
            }
            return ds;
        }

        /// <summary>Convert degrees to radians, with error of less than 0.5 ULP
        /// <summary>
        ///  <returns>instance converted into radians</returns>
        public DerivativeStructure ToRadians()
        {
            DerivativeStructure ds = new DerivativeStructure(compiler);
            for (int i = 0; i < ds.data.Length; ++i)
            {
                ds.data[i] = Math2.ToRadians(data[i]);
            }
            return ds;
        }

        /// <summary>Evaluate Taylor expansion a derivative structure.
        /// <summary>
        /// <param name="delta">parameters offsets (&Delta;x, &Delta;y, ..d)</param>
        /// <returns>value of the Taylor expansion at x + &Delta;x, y + &Delta;y, ...</returns>
        /// <exception cref="MathArithmeticException">if factorials becomes too large </exception>
        public double Taylor(params double[] delta)
        {
            return compiler.Taylor(data, 0, delta);
        }

        /// <summary>Compute composition of the instance by a univariate function.
        /// <summary>
        /// <param name="f">array of value and derivatives of the function at</param>
        /// the current point (i.ed [f({@link #Value()}),
        /// f'({@link #Value()}), f''({@link #Value()})...]).
        /// <returns>f(this)</returns>
        /// <exception cref="DimensionMismatchException">if the number of derivatives </exception>
        /// in the array is not equal to {@link #Order order} + 1
        public DerivativeStructure Compose(params double[] f)

        {
            if (f.Length != Order + 1)
            {
                throw new DimensionMismatchException(f.Length, Order + 1);
            }
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Compose(data, 0, f, result.data, 0);
            return result;
        }

        /// <summary>Create a constant compatible with instance order and number of parameters.
        /// <p>
        /// This method is a convenience factory method, it simply calls
        /// {@code new DerivativeStructure(FreeParameters, Order, c)}
        /// </p>
        /// <summary>
        /// <param name="c">value of the constant</param>
        /// <returns>a constant compatible with instance order and number of parameters</returns>
        /// <see cref="#DerivativeStructure(int,">int, double) </see>
        /// @since 3.3
        public DerivativeStructure CreateConstant(double c)
        {
            return new DerivativeStructure(FreeParameters, Order, c);
        }

        /// <summary>
        /// Return the exponent of the instance value, removing the bias.
        /// <p>
        /// For double numbers of the form 2<sup>x</sup>, the unbiased
        /// exponent is exactly x.
        /// </p>
        /// <summary>
        /// <returns>exponent for instance in IEEE754 representation, without bias</returns>
        public int GetExponent()
        {
            return Math2.GetExponent(data[0]);
        }

        /// <summary>Get a partial derivative.
        /// <summary>
        /// <param name="orders">derivation orders with respect to each variable (if all orders are 0,</param>
        /// the value is returned)
        /// <returns>partial derivative</returns>
        /// <see cref="#Value()"></see>
        /// <exception cref="DimensionMismatchException">if the numbers of variables does not </exception>
        /// match the instance
        /// <exception cref="NumberIsTooLargeException">if sum of derivation orders is larger </exception>
        /// than the instance limits
        public double GetPartialDerivative(params int[] orders)
        {
            return data[compiler.GetPartialDerivativeIndex(orders)];
        }

        /// <summary>Get all partial derivatives.
        /// <summary>
        /// <returns>a fresh copy of partial derivatives, in an array sorted according to</returns>
        /// {@link DSCompiler#getPartialDerivativeIndex(int..d)}
        public double[] GetAllDerivatives()
        {
            return data.Copy();
        }

        /// <summary>Base 10 logarithm.
        /// <summary>
        /// <returns>base 10 logarithm of the instance</returns>
        public DerivativeStructure Log10()
        {
            DerivativeStructure result = new DerivativeStructure(compiler);
            compiler.Log10(data, 0, result.data, 0);
            return result;
        }

        /// <summary>
        /// Returns the hypotenuse of a triangle with sides {@code x} and {@code y}
        /// - sqrt(<i>x</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)
        /// avoiding intermediate overflow or underflow.
        /// 
        /// <ul>
        /// <li> If either argument is infinite, then the result is positive infinity.</li>
        /// <li> else, if either argument is NaN then the result is NaN.</li>
        /// </ul>
        /// 
        /// <summary>
        /// <param name="x">a value</param>
        /// <param name="y">a value</param>
        /// <returns>sqrt(<i>x</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)</returns>
        /// <exception cref="DimensionMismatchException">if number of free parameters </exception>
        /// or orders do not match
        /// @since 3.2
        public static DerivativeStructure Hypot(DerivativeStructure x, DerivativeStructure y)

        {
            return x.Hypot(y);
        }

        /// <summary>Compute a<sup>x</sup> where a is a double and x a {@link DerivativeStructure}
        /// <summary>
        /// <param name="a">number to exponentiate</param>
        /// <param name="x">power to apply</param>
        /// <returns>a<sup>x</sup></returns>
        /// @since 3.3
        public static DerivativeStructure Pow(double a, DerivativeStructure x)
        {
            DerivativeStructure result = new DerivativeStructure(x.compiler);
            x.compiler.Pow(a, x.data, 0, result.data, 0);
            return result;
        }

        /// <summary>
        /// Replace the instance with a data transfer object for serialization.
        /// <summary>
        /// <returns>data transfer object that will be serialized</returns>
        private Object WriteReplace()
        {
            return new DataTransferObject(compiler.FreeParameters, compiler.Order, data);
        }


        #endregion

        #region Local Private Methods

        #endregion

        /// <summary>Internal class used only for serializationd */
        [Serializable]
        private class DataTransferObject
        {

            /// <summary>Number of variables.
            /// @serial
            /// <summary>
            private int variables;

            /// <summary>Derivation order.
            /// @serial
            /// <summary>
            private int order;

            /// <summary>Partial derivatives.
            /// @serial
            /// <summary>
            private double[] data;

            /// <summary>Simple constructor.
            /// <summary>
            /// <param name="variables">number of variables</param>
            /// <param name="order">derivation order</param>
            /// <param name="data">partial derivatives</param>
            public DataTransferObject(int variables, int order, double[] data)
            {
                this.variables = variables;
                this.order = order;
                this.data = data;
            }

            /// <summary>Replace the deserialized data transfer object with a {@link DerivativeStructure}.
            /// <summary>
            /// <returns>replacement {@link DerivativeStructure}</returns>
            private Object ReadResolve()
            {
                return new DerivativeStructure(variables, order, data);
            }

        }

        public class DerivativeStructureField : IField<DerivativeStructure>
        {
            DSCompiler _compiler;

            public DerivativeStructureField(DSCompiler compiler)
            {
                _compiler = compiler;
            }

            public DerivativeStructure Zero
            {
                get
                {
                    return new DerivativeStructure(_compiler.FreeParameters, _compiler.Order, 0.0);
                }
            }

            public DerivativeStructure One
            {
                get
                {
                    return new DerivativeStructure(_compiler.FreeParameters, _compiler.Order, 1.0);
                }
            }
        }
    }
}
