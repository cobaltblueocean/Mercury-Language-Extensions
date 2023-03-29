// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
using Mercury.Language.Exceptions;

namespace Mercury.Language.Math
{
    /// <summary>
    /// Interface representing a <a href="http://mathworld.wolfram.com/RealNumber.html">real</a>
    /// <a href="http://mathworld.wolfram.com/Field.html">field</a>.
    /// </summary>
    /// <typeparam name="T">the type of the field elements</typeparam>
    /// <see cref="IFieldElement{T}"/>
    public interface IRealFieldElement<T>: IFieldElement<T>
    {
        /// <summary>
        /// Get the real value of the number.
        /// </summary>
        /// <returns>real value</returns>
        double Real();

        /// <summary>
        /// '+' operator.
        /// </summary>
        /// <param name="a">right hand side parameter of the operator</param>
        /// <returns>this + a</returns>
        T Add(double a);

        /// <summary>
        /// '-' operator.
        /// </summary>
        /// <param name="a">right hand side parameter of the operator</param>
        /// <returns>this - a</returns>
        T Subtract(double a);

        /// <summary>
        /// '&times;' operator.
        /// </summary>
        /// <param name="a">right hand side parameter of the operator</param>
        /// <returns>this &times; a</returns>
        T Multiply(double a);

        /// <summary>
        /// '&divide;' operator.
        /// </summary>
        /// <param name="a">right hand side parameter of the operator</param>
        /// <returns>this &divide; a</returns>
        T Divide(double a);

        /// <summary>
        /// IEEE remainder operator.
        /// </summary>
        /// <param name="a">right hand side parameter of the operator</param>
        /// <returns>
        /// this - n &times; a where n is the closest integer to this/a
        /// (the even integer is chosen for n if this/a is halfway between two integers)
        /// </returns>
        T Remainder(double a);

        /// <summary>
        /// IEEE remainder operator.
        /// </summary>
        /// <param name="a">right hand side parameter of the operator</param>
        /// <returns>
        /// this - n &times; a where n is the closest integer to this/a
        /// (the even integer is chosen for n if this/a is halfway between two integers)
        /// </returns>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent</exception>
        T Remainder(T a);

        /// <summary>
        /// absolute value.
        /// </summary>
        /// <returns>Abs(this)</returns>
        T Abs();

        /// <summary>
        /// Get the smallest whole number larger than instance.
        /// </summary>
        /// <returns>Ceil(this)</returns>
        T Ceil();

        /// <summary>
        /// Get the largest whole number smaller than instance.
        /// </summary>
        /// <returns>Floor(this)</returns>
        T Floor();

        /// <summary>
        /// Get the whole number that is the nearest to the instance, or the even one if x is exactly half way between two integers.
        /// </summary>
        /// <returns>a double number r such that r is an integer r - 0.5 &le; this &le; r + 0.5</returns>
        T Rint();

        /// <summary>
        /// Get the closest long to instance value.
        /// </summary>
        /// <returns>closest long to <see cref="Real()"/></returns>
        long Round();

        /// <summary>
        /// Compute the signum of the instance.
        /// The signum is -1 for negative numbers, +1 for positive numbers and 0 otherwise
        /// </summary>
        /// <returns>-1.0, -0.0, +0.0, +1.0 or NaN depending on sign of a</returns>
        T Signum();

        /// <summary>
        /// Returns the instance with the sign of the argument.
        /// A NaN <see cref="sign"/> argument is treated as positive.
        /// </summary>
        /// <param name="sign">the sign for the returned value</param>
        /// <returns>the instance with the same sign as the {@code sign} argument</returns>
        T CopySign(T sign);

        /// <summary>
        /// Returns the instance with the sign of the argument.
        /// A NaN <see cref="sign"/> argument is treated as positive.
        /// </summary>
        /// <param name="sign">the sign for the returned value</param>
        /// <returns>the instance with the same sign as the <see cref="sign"/> argument</returns>
        T CopySign(double sign);

        /// <summary>
        /// Multiply the instance by a power of 2.
        /// </summary>
        /// <param name="n">power of 2</param>
        /// <returns>this &times; 2<sup>n</sup></returns>
        T Scalb(int n);

        /// <summary>
        /// Returns the hypotenuse of a triangle with sides {@code this} and {@code y}
        /// - sqrt(<i>this</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)
        /// avoiding intermediate overflow or underflow.
        ///
        /// <ul>
        /// <li> If either argument is infinite, then the result is positive infinity.</li>
        /// <li> else, if either argument is NaN then the result is NaN.</li>
        /// </ul>
        /// </summary>
        /// <param name="y">a value</param>
        /// <returns>sqrt(<i>this</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)</returns>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent</exception>
        T Hypot(T y);

        /// <summary>
        /// Square root.
        /// </summary>
        /// <returns>root of the instance</returns>
        T Sqrt();

        /// <summary>
        /// Cubic root.
        /// </summary>
        /// <returns>cubic root of the instance</returns>
        T Cbrt();

        /// <summary>
        /// N<sup>th</sup> root.
        /// </summary>
        /// <param name="n">order of the root</param>
        /// <returns>n<sup>th</sup> root of the instance</returns>
        T RootN(int n);

        /// <summary>
        /// Power operation.
        /// </summary>
        /// <param name="p">power to apply</param>
        /// <returns>this<sup>p</sup></returns>
        T Pow(double p);

        /// <summary>
        /// Integer power operation.
        /// </summary>
        /// <param name="n">power to apply</param>
        /// <returns>this<sup>n</sup></returns>
        T Pow(int n);

        /// <summary>
        /// Power operation.
        /// </summary>
        /// <param name="e">exponent</param>
        /// <returns>this<sup>e</sup></returns>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent</exception>
        T Pow(T e);

        /// <summary>
        /// Exponential.
        /// </summary>
        /// <returns>exponential of the instance</returns>
        T Expo();

        /// <summary>
        /// Exponential minus 1.
        /// </summary>
        /// <returns>exponential minus one of the instance</returns>
        T Expm1();

        /// <summary>
        /// Natural logarithm.
        /// </summary>
        /// <returns>logarithm of the instance</returns>
        T Log();

        /// <summary>
        /// Shifted natural logarithm.
        /// </summary>
        /// <returns>logarithm of one plus the instance</returns>
        T Log1p();

        //    TODO: add this method in 4.0, as it is not possible to do it in 3.2
        //          due to incompatibility of the return type in the Dfp class
        //    /** Base 10 logarithm.
        //     * @return base 10 logarithm of the instance
        //     */
        //    T log10();

        /// <summary>
        /// Cosine operation.
        /// </summary>
        /// <returns>cos(this)</returns>
        T Cos();

        /// <summary>
        /// Sine operation.
        /// </summary>
        /// <returns>sin(this)</returns>
        T Sin();

        /// <summary>
        /// Tangent operation.
        /// </summary>
        /// <returns>tan(this)</returns>
        T Tan();

        /// <summary>
        /// Arc cosine operation.
        /// </summary>
        /// <returns>acos(this)</returns>
        T Acos();

        /// <summary>
        /// Arc sine operation.
        /// </summary>
        /// <returns>asin(this)</returns>
        T Asin();

        /// <summary>
        /// Arc tangent operation.
        /// </summary>
        /// <returns>atan(this)</returns>
        T Atan();

        /// <summary>
        /// Two arguments arc tangent operation.
        /// </summary>
        /// <param name="x">second argument of the arc tangent</param>
        /// <returns>atan2(this, x)</returns>
        /// <exception cref="DimensionMismatchException">if number of free parameters or orders are inconsistent</exception>
        T Atan2(T x);

        /// <summary>
        /// Hyperbolic cosine operation.
        /// </summary>
        /// <returns>cosh(this)</returns>
        T Cosh();

        /// <summary>
        /// Hyperbolic sine operation.
        /// </summary>
        /// <returns>sinh(this)</returns>
        T Sinh();

        /// <summary>
        /// Hyperbolic tangent operation.
        /// </summary>
        /// <returns>tanh(this)</returns>
        T Tanh();

        /// <summary>
        /// Inverse hyperbolic cosine operation.
        /// </summary>
        /// <returns>acosh(this)</returns>
        T Acosh();

        /// <summary>
        /// Inverse hyperbolic sine operation.
        /// </summary>
        /// <returns>asin(this)</returns>
        T Asinh();

        /// <summary>
        /// Inverse hyperbolic  tangent operation.
        /// </summary>
        /// <returns>atanh(this)</returns>
        T Atanh();

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a">Factors</param>
        /// <param name="b">Factors</param>
        /// <returns>
        /// <code>&Sigma;<sub>i</sub> a<sub>i</sub> b<sub>i</sub></code>.
        /// </returns>
        /// <exception cref="DimensionMismatchException">if arrays dimensions don't match</exception>
        T LinearCombination(T[] a, T[] b);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a">Factors</param>
        /// <param name="b">Factors</param>
        /// <returns>
        /// <code>&Sigma;<sub>i</sub> a<sub>i</sub> b<sub>i</sub></code>.
        /// </returns>
        /// <exception cref="DimensionMismatchException">if arrays dimensions don't match</exception>
        T LinearCombination(double[] a, T[] b);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <returns>
        /// a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub>
        /// </returns>
        /// <see cref="LinearCombination(T, T, T, T, T, T)"/>
        /// <see cref="LinearCombination(T, T, T, T, T, T, T, T)"/>
        T LinearCombination(T a1, T b1, T a2, T b2);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <returns>
        /// a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub>
        /// </returns>
        /// <see cref="LinearCombination(double, T, double, T, double, T)"/>
        /// <see cref="LinearCombination(double, T, double, T, double, T, double, T)"/>
        T LinearCombination(double a1, T b1, double a2, T b2);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <returns>
        /// a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub>
        /// </returns>
        /// <see cref="LinearCombination(T, T, T, T)"/>
        /// <see cref="LinearCombination(T, T, T, T, T, T, T, T)"/>
        T LinearCombination(T a1, T b1, T a2, T b2, T a3, T b3);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <returns>
        /// a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub>
        /// </returns>
        /// <see cref="LinearCombination(double, T, double, T)"/>
        /// <see cref="LinearCombination(double, T, double, T, double, T)"/>
        T LinearCombination(double a1, T b1, double a2, T b2, double a3, T b3);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <param name="a4">first factor of the third term</param>
        /// <param name="b4">second factor of the third term</param>
        /// <returns>
        /// a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub> +
        /// a<sub>4</sub>&times;b<sub>4</sub>
        /// </returns>
        /// <see cref="LinearCombination(T, T, T, T)"/>
        /// <see cref="LinearCombination(T, T, T, T, T, T)"/>
        T LinearCombination(T a1, T b1, T a2, T b2, T a3, T b3, T a4, T b4);

        /// <summary>
        /// Compute a linear combination.
        /// </summary>
        /// <param name="a1">first factor of the first term</param>
        /// <param name="b1">second factor of the first term</param>
        /// <param name="a2">first factor of the second term</param>
        /// <param name="b2">second factor of the second term</param>
        /// <param name="a3">first factor of the third term</param>
        /// <param name="b3">second factor of the third term</param>
        /// <param name="a4">first factor of the third term</param>
        /// <param name="b4">second factor of the third term</param>
        /// <returns>
        /// a<sub>1</sub>&times;b<sub>1</sub> +
        /// a<sub>2</sub>&times;b<sub>2</sub> + a<sub>3</sub>&times;b<sub>3</sub> +
        /// a<sub>4</sub>&times;b<sub>4</sub>
        /// </returns>
        /// <see cref="LinearCombination(double, T, double, T)"/>
        /// <see cref="LinearCombination(double, T, double, T, double, T)"/>
        T LinearCombination(double a1, T b1, double a2, T b2, double a3, T b3, double a4, T b4);
    }
}
