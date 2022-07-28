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
using Mercury.Language.Math.Fraction;
using Mercury.Language.Math.Analysis.Polynomial;

namespace Mercury.Language.Utility
{
    /// <summary>
    /// A collection of static methods that operate on or return polynomials.
    /// </summary>
    public class PolynomialUtility
    {

        /// <summary>Coefficients for Chebyshev polynomialsd */
        private static List<BigFraction> CHEBYSHEV_COEFFICIENTS;

        /// <summary>Coefficients for Hermite polynomialsd */
        private static List<BigFraction> HERMITE_COEFFICIENTS;

        /// <summary>Coefficients for Laguerre polynomialsd */
        private static List<BigFraction> LAGUERRE_COEFFICIENTS;

        /// <summary>Coefficients for Legendre polynomialsd */
        private static List<BigFraction> LEGENDRE_COEFFICIENTS;

        static PolynomialUtility()
        {

            // initialize recurrence for Chebyshev polynomials
            // T0(X) = 1, T1(X) = 0 + 1 * X
            CHEBYSHEV_COEFFICIENTS = new List<BigFraction>();
            CHEBYSHEV_COEFFICIENTS.Add(BigFraction.ONE);
            CHEBYSHEV_COEFFICIENTS.Add(BigFraction.ZERO);
            CHEBYSHEV_COEFFICIENTS.Add(BigFraction.ONE);

            // initialize recurrence for Hermite polynomials
            // H0(X) = 1, H1(X) = 0 + 2 * X
            HERMITE_COEFFICIENTS = new List<BigFraction>();
            HERMITE_COEFFICIENTS.Add(BigFraction.ONE);
            HERMITE_COEFFICIENTS.Add(BigFraction.ZERO);
            HERMITE_COEFFICIENTS.Add(BigFraction.TWO);

            // initialize recurrence for Laguerre polynomials
            // L0(X) = 1, L1(X) = 1 - 1 * X
            LAGUERRE_COEFFICIENTS = new List<BigFraction>();
            LAGUERRE_COEFFICIENTS.Add(BigFraction.ONE);
            LAGUERRE_COEFFICIENTS.Add(BigFraction.ONE);
            LAGUERRE_COEFFICIENTS.Add(BigFraction.MINUS_ONE);

            // initialize recurrence for Legendre polynomials
            // P0(X) = 1, P1(X) = 0 + 1 * X
            LEGENDRE_COEFFICIENTS = new List<BigFraction>();
            LEGENDRE_COEFFICIENTS.Add(BigFraction.ONE);
            LEGENDRE_COEFFICIENTS.Add(BigFraction.ZERO);
            LEGENDRE_COEFFICIENTS.Add(BigFraction.ONE);

        }

        /// <summary>
        /// Private constructor, to prevent instantiation.
        /// </summary>
        private PolynomialUtility()
        {
        }

        /// <summary>
        /// Create a Chebyshev polynomial of the first kind.
        /// <p><a href="http://mathworld.wolfram.com/ChebyshevPolynomialoftheFirstKind.html">Chebyshev
        /// polynomials of the first kind</a> are orthogonal polynomials.
        /// They can be defined by the following recurrence relations:
        /// <pre>
        ///  T<sub>0</sub>(X)   = 1
        ///  T<sub>1</sub>(X)   = X
        ///  T<sub>k+1</sub>(X) = 2X T<sub>k</sub>(X) - T<sub>k-1</sub>(X)
        /// </pre></p>
        /// </summary>
        /// <param Name="degree">degree of the polynomial</param>
        /// <returns>Chebyshev polynomial of specified degree</returns>
        public static PolynomialFunction createChebyshevPolynomial(int degree)
        {
            return buildPolynomial(degree, CHEBYSHEV_COEFFICIENTS,
                    new RecurrenceCoefficientsGenerator()
                    {
                        generator = new Func<int, BigFraction[]>((k) =>
                        {
                            BigFraction[] coeffs = { BigFraction.ZERO, BigFraction.TWO, BigFraction.ONE };
                            return coeffs;
                        })
                    }
        );
        }

        /// <summary>
        /// Create a Hermite polynomial.
        /// <p><a href="http://mathworld.wolfram.com/HermitePolynomial.html">Hermite
        /// polynomials</a> are orthogonal polynomials.
        /// They can be defined by the following recurrence relations:
        /// <pre>
        ///  H<sub>0</sub>(X)   = 1
        ///  H<sub>1</sub>(X)   = 2X
        ///  H<sub>k+1</sub>(X) = 2X H<sub>k</sub>(X) - 2k H<sub>k-1</sub>(X)
        /// </pre></p>

        /// </summary>
        /// <param Name="degree">degree of the polynomial</param>
        /// <returns>Hermite polynomial of specified degree</returns>
        public static PolynomialFunction createHermitePolynomial(int degree)
        {
            return buildPolynomial(degree, HERMITE_COEFFICIENTS,
                    new RecurrenceCoefficientsGenerator()
                    {
                        generator = new Func<int, BigFraction[]>((k) =>
                        {
                            return new BigFraction[] {
                                                    BigFraction.ZERO,
                                                    BigFraction.TWO,
                                                    new BigFraction(2 * k)};
                        })
                    }
                    );

        }

        /// <summary>
        /// Create a Laguerre polynomial.
        /// <p><a href="http://mathworld.wolfram.com/LaguerrePolynomial.html">Laguerre
        /// polynomials</a> are orthogonal polynomials.
        /// They can be defined by the following recurrence relations:
        /// <pre>
        ///        L<sub>0</sub>(X)   = 1
        ///        L<sub>1</sub>(X)   = 1 - X
        ///  (k+1) L<sub>k+1</sub>(X) = (2k + 1 - X) L<sub>k</sub>(X) - k L<sub>k-1</sub>(X)
        /// </pre></p>
        /// </summary>
        /// <param Name="degree">degree of the polynomial</param>
        /// <returns>Laguerre polynomial of specified degree</returns>
        public static PolynomialFunction createLaguerrePolynomial(int degree)
        {
            return buildPolynomial(degree, LAGUERRE_COEFFICIENTS,
                    new RecurrenceCoefficientsGenerator()
                    {
                        generator = new Func<int, BigFraction[]>((k) =>
                        {
                            int kP1 = k + 1;
                            return new BigFraction[] {
                            new BigFraction(2 * k + 1, kP1),
                                                    new BigFraction(-1, kP1),
                                                    new BigFraction(k, kP1)};
                        })
                    }
                );
        }

        /// <summary>
        /// Create a Legendre polynomial.
        /// <p><a href="http://mathworld.wolfram.com/LegendrePolynomial.html">Legendre
        /// polynomials</a> are orthogonal polynomials.
        /// They can be defined by the following recurrence relations:
        /// <pre>
        ///        P<sub>0</sub>(X)   = 1
        ///        P<sub>1</sub>(X)   = X
        ///  (k+1) P<sub>k+1</sub>(X) = (2k+1) X P<sub>k</sub>(X) - k P<sub>k-1</sub>(X)
        /// </pre></p>
        /// </summary>
        /// <param Name="degree">degree of the polynomial</param>
        /// <returns>Legendre polynomial of specified degree</returns>
        public static PolynomialFunction createLegendrePolynomial(int degree)
        {
            return buildPolynomial(degree, LEGENDRE_COEFFICIENTS,
                                   new RecurrenceCoefficientsGenerator()
                                   {
                                       generator = new Func<int, BigFraction[]>((k) =>
                                       {
                                           int kP1 = k + 1;
                                           return new BigFraction[] {
                                                               BigFraction.ZERO,
                                                               new BigFraction(k + kP1, kP1),
                                                               new BigFraction(k, kP1)};
                                       })
                                   }

            );
        }

        /// <summary>Get the coefficients array for a given degree.
        /// </summary>
        /// <param Name="degree">degree of the polynomial</param>
        /// <param Name="coefficients">list where the computed coefficients are stored</param>
        /// <param Name="generator">recurrence coefficients generator</param>
        /// <returns>coefficients array</returns>
        private static PolynomialFunction buildPolynomial(int degree, List<BigFraction> coefficients, IRecurrenceCoefficientsGenerator generator)
        {

            int maxDegree = (int)System.Math.Floor(System.Math.Sqrt(2 * coefficients.Count)) - 1;
            //synchronized(typeof(PolynomialUtility)) {
            if (degree > maxDegree)
            {
                computeUpToDegree(degree, maxDegree, generator, coefficients);
            }
            //}

            // coefficient  for polynomial 0 is  l [0]
            // coefficients for polynomial 1 are l [1] [] l [2] (degrees 0 [] 1)
            // coefficients for polynomial 2 are l [3] [] l [5] (degrees 0 [] 2)
            // coefficients for polynomial 3 are l [6] [] l [9] (degrees 0 [] 3)
            // coefficients for polynomial 4 are l[10] [] l[14] (degrees 0 [] 4)
            // coefficients for polynomial 5 are l[15] [] l[20] (degrees 0 [] 5)
            // coefficients for polynomial 6 are l[21] [] l[27] (degrees 0 [] 6)
            // ...
            int start = degree * (degree + 1) / 2;

            double[] a = new double[degree + 1];
            for (int i = 0; i <= degree; ++i)
            {
                a[i] = coefficients[start + i].DoubleValue();
            }

            // build the polynomial
            return new PolynomialFunction(a);

        }

        /// <summary>Compute polynomial coefficients up to a given degree.
        /// </summary>
        /// <param Name="degree">maximal degree</param>
        /// <param Name="maxDegree">current maximal degree</param>
        /// <param Name="generator">recurrence coefficients generator</param>
        /// <param Name="coefficients">list where the computed coefficients should be appended</param>
        private static void computeUpToDegree(int degree, int maxDegree, IRecurrenceCoefficientsGenerator generator, List<BigFraction> coefficients)
        {

            int startK = (maxDegree - 1) * maxDegree / 2;
            AutoParallel.AutoParallelFor(maxDegree, degree, (k) =>
            {

                // start indices of two previous polynomials Pk(X) and Pk-1(X)
                int startKm1 = startK;
                startK += k;

                // Pk+1(X) = (a[0] + a[1] X) Pk(X) - a[2] Pk-1(X)
                BigFraction[] ai = generator.Generate(k);

                BigFraction ck = coefficients[startK];
                BigFraction ckm1 = coefficients[startKm1];

                // degree 0 coefficient
                coefficients.Add(ck.Multiply(ai[0]).Subtract(ckm1.Multiply(ai[2])));

                // degree 1 to degree k-1 coefficients
                AutoParallel.AutoParallelFor(1, k, (i) =>
                        {
                            BigFraction ckPrev = ck;
                            ck = coefficients[startK + i];
                            ckm1 = coefficients[startKm1 + i];
                            coefficients.Add(ck.Multiply(ai[0]).Add(ckPrev.Multiply(ai[1])).Subtract(ckm1.Multiply(ai[2])));
                        });

                // degree k coefficient
                BigFraction ckPrev = ck;
                ck = coefficients[startK + k];
                coefficients.Add(ck.Multiply(ai[0]).Add(ckPrev.Multiply(ai[1])));

                // degree k+1 coefficient
                coefficients.Add(ck.Multiply(ai[1]));
            });
        }

        private class RecurrenceCoefficientsGenerator : IRecurrenceCoefficientsGenerator
        {
            public Func<int, BigFraction[]> generator { get; set; }

            public BigFraction[] Generate(int k)
            {
                return generator(k);
            }
        }

        /// <summary>Interface for recurrence coefficients generationd */
        private interface IRecurrenceCoefficientsGenerator
        {
            Func<int, BigFraction[]> generator { get; set; }


            /// <summary>
            /// Generate recurrence coefficients.
            /// </summary>
            /// <param Name="k">highest degree of the polynomials used in the recurrence</param>
            /// <returns>an array of three coefficients such that</returns>
            /// P<sub>k+1</sub>(X) = (a[0] + a[1] X) P<sub>k</sub>(X) - a[2] P<sub>k-1</sub>(X)
            BigFraction[] Generate(int k);
        }
    }
}
