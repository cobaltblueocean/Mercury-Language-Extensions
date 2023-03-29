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
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;

namespace Mercury.Language.Math.Analysis.Polynomial
{

    /// <summary>
    /// Represents a polynomial spline function.
    /// <p>
    /// A <strong>polynomial spline function</strong> consists of a set of
    /// <i>interpolating polynomials</i> and an ascending array of domain
    /// <i>knot points</i>, determining the intervals over which the spline function
    /// is defined by the constituent polynomialsd  The polynomials are assumed to
    /// have been computed to match the values of another function at the knot
    /// pointsd  The value consistency constraints are not currently enforced by
    /// <code>PolynomialSplineFunction</code> itself, but are assumed to hold among
    /// the polynomials and knot points passed to the constructor.</p>
    /// <p>
    /// N.B.:  The polynomials in the <code>polynomials</code> property must be
    /// centered on the knot points to compute the spline function values.
    /// See below.</p>
    /// <p>
    /// The domain of the polynomial spline function is
    /// <code>[smallest knot, largest knot]</code>d  Attempts to evaluate the
    /// function at values outside of this range generate IllegalArgumentExceptions.
    /// </p>
    /// <p>
    /// The value of the polynomial spline function for an argument <code>x</code>
    /// is computed as follows:
    /// <ol>
    /// <li>The knot array is searched to find the segment to which <code>x</code>
    /// belongsd  If <code>x</code> is less than the smallest knot point or greater
    /// than the largest one, an <code>ArgumentException</code>
    /// is thrown.</li>
    /// <li> Let <code>j</code> be the index of the largest knot point that is less
    /// than or equal to <code>x</code>d  The value returned is <br>
    /// <code>polynomials[j](x - knot[j])</code></li></ol></p>
    /// 
    /// @version $Revision: 1037327 $ $Date: 2010-11-20 21:57:37 +0100 (samd 20 novd 2010) $
    /// </summary>
    public class PolynomialSplineFunction : IDifferentiableUnivariateRealFunction
    {

        /// <summary>Spline segment interval delimiters (knots)d   Size is n+1 for n segmentsd */
        private double[] knots;

        /// <summary>
        /// The polynomial functions that make up the splined  The first element
        /// determines the value of the spline over the first subinterval, the
        /// second over the second, etcd   Spline function values are determined by
        /// evaluating these functions at <code>(x - knot[i])</code> where i is the
        /// knot segment to which x belongs.
        /// </summary>
        private PolynomialFunction[] _polynomials;

        /// <summary>
        /// Number of spline segments = number of polynomials
        ///  = number of partition points - 1
        /// </summary>
        private int n;

        private double _paramValue;


        /// <summary>
        /// Construct a polynomial spline function with the given segment delimiters
        /// and interpolating polynomials.
        /// <p>
        /// The constructor copies both arrays and assigns the copies to the knots
        /// and polynomials properties, respectively.</p>
        /// 
        /// </summary>
        /// <param Name="knots">spline segment interval delimiters</param>
        /// <param Name="polynomials">polynomial functions that make up the spline</param>
        /// <exception cref="NullReferenceException">if either of the input arrays is null </exception>
        /// <exception cref="ArgumentException">if knots has Length less than 2, </exception>
        /// <code>polynomials.Length != knots.Length - 1 </code>, or the knots array
        /// is not strictly increasing.
        /// 
        public PolynomialSplineFunction(double[] knots, PolynomialFunction[] polynomials)
        {
            if (knots.Length < 2)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().NOT_ENOUGH_POINTS_IN_SPLINE_PARTITION, 2, knots.Length));
            }
            if (knots.Length - 1 != polynomials.Length)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().POLYNOMIAL_INTERPOLANTS_MISMATCH_SEGMENTS, polynomials.Length, knots.Length));
            }
            if (!isStrictlyIncreasing(knots))
            {
                throw new ArgumentException(LocalizedResources.Instance().NOT_STRICTLY_INCREASING_KNOT_VALUES);
            }

            this.n = knots.Length - 1;
            this.knots = new double[n + 1];
            Array.Copy(knots, 0, this.knots, 0, n + 1);
            _polynomials = new PolynomialFunction[n];
            Array.Copy(polynomials, 0, _polynomials, 0, n);
        }

        /// <summary>
        /// Compute the value for the function.
        /// See {@link PolynomialSplineFunction} for details on the algorithm for
        /// computing the value of the function.</p>
        /// 
        /// </summary>
        /// <param Name="v">the point for which the function value should be computed</param>
        /// <returns>the value</returns>
        /// <exception cref="ArgumentOutsideDomainException">if v is outside of the domain of </exception>
        /// of the spline function (less than the smallest knot point or greater
        /// than the largest knot point)
        public double Value(double v)
        {
            _paramValue = v;

            if (v < knots[0] || v > knots[n]) {
                throw new ArgumentOutsideDomainException(v, knots[0], knots[n]);
            }
            int i = Array.BinarySearch(knots, v);
            if (i < 0) {
                i = -i - 2;
            }
            //This will handle the case where v is the last knot value
            //There are only n-1 polynomials, so if v is the last knot
            //then we will use the last polynomial to calculate the value.
            if (i >= _polynomials.Length) {
                i--;
            }
            return _polynomials[i].Value(v - knots[i]);
        }

        /// <summary>
        /// Returns the derivative of the polynomial spline function as a IUnivariateRealFunction
        /// </summary>
        /// <returns> the derivative function</returns>
        public IUnivariateRealFunction Derivative()
        {
            return PolynomialSplineDerivative();
        }

        /// <summary>
        /// Returns the derivative of the polynomial spline function as a PolynomialSplineFunction
        /// 
        /// </summary>
        /// <returns> the derivative function</returns>
        public PolynomialSplineFunction PolynomialSplineDerivative()
        {
            PolynomialFunction[] derivativePolynomials = new PolynomialFunction[n];
            AutoParallel.AutoParallelFor(0, n, (i) =>
            {
                derivativePolynomials[i] = _polynomials[i].PolynomialDerivative();
            });
            return new PolynomialSplineFunction(knots, derivativePolynomials);
        }

        /// <summary>
        /// Returns the number of spline segments = the number of polynomials
        /// = the number of knot points - 1d
        /// 
        /// </summary>
        /// <returns>the number of spline segments</returns>
        public int N
        {
            get { return n; }
        }

        /// <summary>
        /// Returns a copy of the interpolating polynomials array.
        /// <p>
        /// Returns a fresh copy of the arrayd Changes made to the copy will
        /// not affect the polynomials property.</p>
        /// 
        /// </summary>
        /// <returns>the interpolating polynomials</returns>
        public PolynomialFunction[] GetPolynomials()
        {
            PolynomialFunction[] p = new PolynomialFunction[n];
            Array.Copy(_polynomials, 0, p, 0, n);
            return p;
        }

        /// <summary>
        /// Returns an array copy of the knot points.
        /// <p>
        /// Returns a fresh copy of the arrayd Changes made to the copy
        /// will not affect the knots property.</p>
        /// 
        /// </summary>
        /// <returns>the knot points</returns>
        public double[] Knots
        {
            get { double[] _out = new double[n + 1];
                Array.Copy(knots, 0, _out, 0, n + 1);
                return _out;
            }
        }

        public double ParamValue { get {
                return _paramValue;
            } }

        /// <summary>
        /// Determines if the given array is ordered in a strictly increasing
        /// fashion.
        /// 
        /// </summary>
        /// <param Name="x">the array to examine.</param>
        /// <returns><code>true</code> if the elements in <code>x</code> are ordered</returns>
        /// in a stricly increasing mannerd  <code>false</code>, otherwise.
        private static Boolean isStrictlyIncreasing(double[] x)
        {
            for (int i = 1; i < x.Length; ++i)
            {
                if (x[i - 1] >= x[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
