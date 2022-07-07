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
using Mercury.Language.Exception;

namespace Mercury.Language.Math
{
    /// <summary>
    /// Interface representing <a href="http://mathworld.wolfram.com/Field.html">field</a> elements.
    /// </summary>
    /// <typeparam name="T">the type of the field elements</typeparam>
    public interface IFieldElement<T>
    {
        /// <summary>
        /// Compute this + a.
        /// </summary>
        /// <param name="a">element to add</param>
        /// <returns>a new element representing this + a</returns>
        /// <exception cref="ArgumentNullException">if <see cref="a"/> is <see cref="null"/>.</exception>
        T Add(T a);

        /// <summary>
        /// Compute this - a.
        /// </summary>
        /// <param name="a">a element to subtract</param>
        /// <returns>a new element representing this - a</returns>
        /// <exception cref="ArgumentNullException">if <see cref="a"/> is <see cref="null"/>.</exception>
        T Subtract(T a);

        /// <summary>
        /// Returns the additive inverse of <see cref="this"/> element.
        /// </summary>
        /// <returns>the opposite of <see cref="this"/>.</returns>
        T Negate();

        /// <summary>
        /// Compute n &times; thisd Multiplication by an int number is defined
        /// as the following sum
        /// <center>
        /// n &times; this = &sum;<sub>i=1</sub><sup>n</sup> this.
        /// </center>
        /// </summary>
        /// <param name="a">Number of times <see cref="this"/> must be added to itself.</param>
        /// <returns>A new element representing n &times; this.</returns>
        T Multiply(int n);

        /// <summary>
        /// Compute this &times; a.
        /// </summary>
        /// <param name="a">element to multiply</param>
        /// <returns>a new element representing this &times; a</returns>
        /// <exception cref="ArgumentNullException">if <see cref="a"/> is <see cref="null"/>.</exception>
        T Multiply(T a);

        /// <summary>
        /// Compute this &divide; a.
        /// </summary>
        /// <param name="a">element to divide by</param>
        /// <returns>a new element representing this &divide; a</returns>
        /// <exception cref="ArgumentNullException">if <see cref="a"/> is <see cref="null"/>.</exception>
        /// <exception cref="MathArithmeticException">if <see cref="a"/> is zero.</exception>
        T Divide(T a);

        /// <summary>
        /// Returns the multiplicative inverse of <see cref="this"/> element.
        /// </summary>
        /// <returns>the inverse of <see cref="this"/>.</returns>
        /// <exception cref="MathArithmeticException">if <see cref="a"/> is zero.</exception>
        T Reciprocal();

        /// <summary>
        /// Get the <see cref="IField{T}"/> to which the instance belongs.
        /// </summary>
        IField<T> Field { get; }
    }
}
