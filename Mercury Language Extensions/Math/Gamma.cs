using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Math
{
    public static class Gamma
    {
        /// <summary>Maximum gamma on the machine.</summary>
        public const double GammaMax = 171.624376956302725; // TODO: Rename to Max

        private static double DEFAULT_EPSILON = 10e-15;

        private static readonly double[] gamma_P =
        {
            1.60119522476751861407E-4,
            1.19135147006586384913E-3,
            1.04213797561761569935E-2,
            4.76367800457137231464E-2,
            2.07448227648435975150E-1,
            4.94214826801497100753E-1,
            9.99999999999999996796E-1
        };

        private static readonly double[] gamma_Q =
        {
            -2.31581873324120129819E-5,
            5.39605580493303397842E-4,
            -4.45641913851797240494E-3,
            1.18139785222060435552E-2,
            3.58236398605498653373E-2,
            -2.34591795718243348568E-1,
            7.14304917030273074085E-2,
            1.00000000000000000320E0
        };

        /// <summary>
        ///   Gamma function of the specified value.
        /// </summary>
        public static double Function(double x)
        {
            double p, z;

            double q = System.Math.Abs(x);

            if (q > 33.0)
            {
                if (x < 0.0)
                {
                    p = System.Math.Floor(q);

                    if (p == q)
                        throw new OverflowException();

                    z = q - p;
                    if (z > 0.5)
                    {
                        p += 1.0;
                        z = q - p;
                    }
                    z = q * System.Math.Sin(System.Math.PI * z);

                    if (z == 0.0)
                        throw new OverflowException();

                    z = System.Math.Abs(z);
                    z = System.Math.PI / (z * Stirling(q));

                    return -z;
                }
                else
                {
                    return Stirling(x);
                }
            }

            z = 1.0;
            while (x >= 3.0)
            {
                x -= 1.0;
                z *= x;
            }

            while (x < 0.0)
            {
                if (x == 0.0)
                {
                    throw new ArithmeticException();
                }
                else if (x > -1.0E-9)
                {
                    return (z / ((1.0 + 0.5772156649015329 * x) * x));
                }
                z /= x;
                x += 1.0;
            }

            while (x < 2.0)
            {
                if (x == 0.0)
                {
                    throw new ArithmeticException();
                }
                else if (x < 1.0E-9)
                {
                    return (z / ((1.0 + 0.5772156649015329 * x) * x));
                }

                z /= x;
                x += 1.0;
            }

            if ((x == 2.0) || (x == 3.0))
                return z;

            x -= 2.0;
            p = Special.Polevl(x, gamma_P, 6);
            q = Special.Polevl(x, gamma_Q, 7);
            return z * p / q;

        }

        /// <summary>
        ///   Multivariate Gamma function
        /// </summary>
        public static double Multivariate(double x, int p)
        {
            if (p < 1)
                throw new ArgumentOutOfRangeException("p",
                    "Parameter p must be higher than 1.");

            if (p == 1)
                return Function(x);


            double prod = System.Math.Pow(System.Math.PI, (1 / 4.0) * p * (p - 1));

            for (int i = 0; i < p; i++)
                prod *= Function(x - 0.5 * i);

            return prod;
        }

        /// <summary>
        ///   Digamma function.
        /// </summary>
        /// 
        public static double Digamma(double x)
        {
            if (x == 0)
                return System.Double.NegativeInfinity;
            if (x < 0)
                return Digamma(1 - x) + System.Math.PI / System.Math.Tan(-System.Math.PI * x);

            double s = 0;
            double w = 0;
            double y = 0;
            double z = 0;
            double nz = 0;

            bool negative = false;

            if (x <= 0.0)
            {
                negative = true;
                double q = x;
                double p = (int)System.Math.Floor(q);

                if (p == q)
                    throw new OverflowException("Function computation resulted in arithmetic overflow.");

                nz = q - p;

                if (nz != 0.5)
                {
                    if (nz > 0.5)
                    {
                        p = p + 1.0;
                        nz = q - p;
                    }
                    nz = System.Math.PI / System.Math.Tan(System.Math.PI * nz);
                }
                else
                {
                    nz = 0.0;
                }

                x = 1.0 - x;
            }

            if (x <= 10.0 & x == System.Math.Floor(x))
            {
                y = 0.0;
                int n = (int)System.Math.Floor(x);
                for (int i = 1; i <= n - 1; i++)
                {
                    w = i;
                    y = y + 1.0 / w;
                }
                y = y - 0.57721566490153286061;
            }
            else
            {
                s = x;
                w = 0.0;

                while (s < 10.0)
                {
                    w = w + 1.0 / s;
                    s = s + 1.0;
                }

                if (s < 1.0E17)
                {
                    z = 1.0 / (s * s);

                    double polv = 8.33333333333333333333E-2;
                    polv = polv * z - 2.10927960927960927961E-2;
                    polv = polv * z + 7.57575757575757575758E-3;
                    polv = polv * z - 4.16666666666666666667E-3;
                    polv = polv * z + 3.96825396825396825397E-3;
                    polv = polv * z - 8.33333333333333333333E-3;
                    polv = polv * z + 8.33333333333333333333E-2;
                    y = z * polv;
                }
                else
                {
                    y = 0.0;
                }
                y = System.Math.Log(s) - 0.5 / s - y - w;
            }

            if (negative == true)
            {
                y = y - nz;
            }

            return y;
        }

        /// <summary>
        ///   Trigamma function.
        /// </summary>
        /// 
        /// <remarks>
        ///   This code has been adapted from the FORTRAN77 and subsequent
        ///   C code by B. E. Schneider and John Burkardt. The code had been
        ///   made public under the GNU LGPL license.
        /// </remarks>
        public static double Trigamma(double x)
        {
            if (x < 0)
            {
                double v = System.Math.PI / System.Math.Sin(-System.Math.PI * x);
                return -Trigamma(1 - x) + v * v;
            }


            double a = 0.0001;
            double b = 5.0;
            double b2 = 0.1666666667;
            double b4 = -0.03333333333;
            double b6 = 0.02380952381;
            double b8 = -0.03333333333;
            double value;
            double y;
            double z;

            // Check the input.
            if (x <= 0.0)
            {
                throw new ArgumentException("The input parameter x must be positive.", "x");
            }

            z = x;

            // Use small value approximation if X <= A.
            if (x <= a)
            {
                value = 1.0 / x / x;
                return value;
            }

            // Increase argument to ( X + I ) >= B.
            value = 0.0;

            while (z < b)
            {
                value = value + 1.0 / z / z;
                z = z + 1.0;
            }

            // Apply asymptotic formula if argument is B or greater.
            y = 1.0 / z / z;

            value = value + 0.5 *
                y + (1.0
              + y * (b2
              + y * (b4
              + y * (b6
              + y * b8)))) / z;

            return value;
        }

        private static readonly double[] STIR =
        {
                7.87311395793093628397E-4,
            -2.29549961613378126380E-4,
            -2.68132617805781232825E-3,
                3.47222221605458667310E-3,
                8.33333333333482257126E-2,
        };

        /// <summary>
        ///   Gamma function as computed by Stirling's formula.
        /// </summary>
        public static double Stirling(double x)
        {
            double MAXSTIR = 143.01608;

            double w = 1.0 / x;
            double y = System.Math.Exp(x);

            w = 1.0 + w * Special.Polevl(w, STIR, 4);

            if (x > MAXSTIR)
            {
                double v = System.Math.Pow(x, 0.5 * x - 0.25);

                if (System.Double.IsPositiveInfinity(v) && System.Double.IsPositiveInfinity(y))
                {
                    // lim x -> inf { (x^(0.5*x - 0.25)) * (x^(0.5*x - 0.25) / exp(x))  }
                    y = System.Double.PositiveInfinity;
                }
                else
                {
                    y = v * (v / y);
                }
            }
            else
            {
                y = System.Math.Pow(x, x - 0.5) / y;
            }

            y = MathConstants.SqareRootOf2Pi * y * w;
            return y;
        }

        /// <summary>
        ///   Upper incomplete regularized Gamma function Q
        ///   (a.k.a the incomplete complemented Gamma function)
        /// </summary>
        /// 
        /// <remarks>
        ///   This function is equivalent to Q(x) = Γ(s, x) / Γ(s).
        /// </remarks>
        public static double UpperIncomplete(double a, double x)
        {
            const double big = 4.503599627370496e15;
            const double biginv = 2.22044604925031308085e-16;
            double ans, ax, c, yc, r, t, y, z;
            double pk, pkm1, pkm2, qk, qkm1, qkm2;

            if (x <= 0 || a <= 0)
                return 1.0;

            if (x < 1.0 || x < a)
                return 1.0 - LowerIncomplete(a, x);

            if (System.Double.IsPositiveInfinity(x))
                return 0;

            ax = a * System.Math.Log(x) - x - Log(a);

            if (ax < -MathConstants.MaxLog)
                return 0.0;

            ax = System.Math.Exp(ax);

            // continued fraction
            y = 1.0 - a;
            z = x + y + 1.0;
            c = 0.0;
            pkm2 = 1.0;
            qkm2 = x;
            pkm1 = x + 1.0;
            qkm1 = z * x;
            ans = pkm1 / qkm1;

            do
            {
                c += 1.0;
                y += 1.0;
                z += 2.0;
                yc = y * c;
                pk = pkm1 * z - pkm2 * yc;
                qk = qkm1 * z - qkm2 * yc;
                if (qk != 0)
                {
                    r = pk / qk;
                    t = System.Math.Abs((ans - r) / r);
                    ans = r;
                }
                else
                    t = 1.0;

                pkm2 = pkm1;
                pkm1 = pk;
                qkm2 = qkm1;
                qkm1 = qk;
                if (System.Math.Abs(pk) > big)
                {
                    pkm2 *= biginv;
                    pkm1 *= biginv;
                    qkm2 *= biginv;
                    qkm1 *= biginv;
                }
            } while (t > System.Double.Epsilon);

            return ans * ax;
        }

        /// <summary>
        ///   Lower incomplete regularized gamma function P
        ///   (a.k.a. the incomplete Gamma function).
        /// </summary>
        /// 
        /// <remarks>
        ///   This function is equivalent to P(x) = γ(s, x) / Γ(s).
        /// </remarks>
        public static double LowerIncomplete(double a, double x)
        {
            if (a <= 0)
                return 1.0;

            if (x <= 0)
                return 0.0;

            if (x > 1.0 && x > a)
                return 1.0 - UpperIncomplete(a, x);

            double ax = a * System.Math.Log(x) - x - Log(a);

            if (ax < -MathConstants.MaxLog)
                return 0.0;

            ax = System.Math.Exp(ax);

            double r = a;
            double c = 1.0;
            double ans = 1.0;

            do
            {
                r += 1.0;
                c *= x / r;
                ans += c;
            } while (c / ans > System.Double.Epsilon);

            return ans * ax / a;
        }

        private static readonly double[] log_A =
        {
                8.11614167470508450300E-4,
            -5.95061904284301438324E-4,
                7.93650340457716943945E-4,
            -2.77777777730099687205E-3,
                8.33333333333331927722E-2
        };

        private static readonly double[] log_B =
        {
            -1.37825152569120859100E3,
            -3.88016315134637840924E4,
            -3.31612992738871184744E5,
            -1.16237097492762307383E6,
            -1.72173700820839662146E6,
            -8.53555664245765465627E5
        };

        private static readonly double[] log_C =
        {
            -3.51815701436523470549E2,
            -1.70642106651881159223E4,
            -2.20528590553854454839E5,
            -1.13933444367982507207E6,
            -2.53252307177582951285E6,
            -2.01889141433532773231E6
        };

        /// <summary>
        ///   Natural logarithm of the gamma function.
        /// </summary>
        public static double Log(double x)
        {
            if (x == 0)
                return System.Double.PositiveInfinity;

            double p, q, w, z;

            if (x < -34.0)
            {
                q = -x;
                w = Log(q);
                p = System.Math.Floor(q);

                if (p == q)
                    throw new OverflowException();

                z = q - p;
                if (z > 0.5)
                {
                    p += 1.0;
                    z = p - q;
                }
                z = q * System.Math.Sin(System.Math.PI * z);

                if (z == 0.0)
                    throw new OverflowException();

                z = MathConstants.LogOfPi - System.Math.Log(z) - w;
                return z;
            }

            if (x < 13.0)
            {
                z = 1.0;
                while (x >= 3.0)
                {
                    x -= 1.0;
                    z *= x;
                }
                while (x < 2.0)
                {
                    if (x == 0.0)
                        throw new OverflowException();

                    z /= x;
                    x += 1.0;
                }

                if (z < 0.0)
                    z = -z;

                if (x == 2.0)
                    return System.Math.Log(z);

                x -= 2.0;

                p = x * Special.Polevl(x, log_B, 5) / Special.P1evl(x, log_C, 6);

                return (System.Math.Log(z) + p);
            }

            if (x > 2.556348e305)
                throw new OverflowException();

            q = (x - 0.5) * System.Math.Log(x) - x + 0.91893853320467274178;

            if (x > 1.0e8)
                return (q);

            p = 1.0 / (x * x);

            if (x >= 1000.0)
            {
                q += ((7.9365079365079365079365e-4 * p
                    - 2.7777777777777777777778e-3) * p
                    + 0.0833333333333333333333) / x;
            }
            else
            {
                q += Special.Polevl(p, log_A, 4) / x;
            }

            return q;
        }

        /// <summary>
        ///   Natural logarithm of the multivariate Gamma function.
        /// </summary>
        public static double Log(double x, int p)
        {
            if (p < 1)
                throw new ArgumentOutOfRangeException("p", "Parameter p must be higher than 1.");

            if (p == 1)
                return Log(x);

            double sum = MathConstants.LogOfPi / p;
            for (int i = 0; i < p; i++)
                sum += Log(x - 0.5 * i);

            return sum;
        }

        /// <summary>
        ///   Inverse of the <see cref="LowerIncomplete"> 
        ///   incomplete Gamma integral (LowerIncomplete, P)</see>.
        /// </summary>
        public static double InverseLowerIncomplete(double a, double y)
        {
            return InverseUpperIncomplete(a, 1 - y);
        }

        /// <summary>
        ///   Inverse of the <see cref="UpperIncomplete">complemented 
        ///   incomplete Gamma integral (UpperIncomplete, Q)</see>.
        /// </summary>
        public static double InverseUpperIncomplete(double a, double y)
        {
            // bound the solution
            double x0 = System.Double.MaxValue;
            double yl = 0;
            double x1 = 0;
            double yh = 1.0;
            double dithresh = 5.0 * System.Double.Epsilon;

            // approximation to inverse function
            double d = 1.0 / (9.0 * a);
            double yy = (1.0 - d - Normal.Inverse(y) * System.Math.Sqrt(d));
            double x = a * yy * yy * yy;

            double lgm = Gamma.Log(a);

            for (int i = 0; i < 10; i++)
            {
                if (x > x0 || x < x1)
                    goto ihalve;

                yy = Gamma.UpperIncomplete(a, x);
                if (yy < yl || yy > yh)
                    goto ihalve;

                if (yy < y)
                {
                    x0 = x;
                    yl = yy;
                }
                else
                {
                    x1 = x;
                    yh = yy;
                }

                // compute the derivative of the function at this point
                d = (a - 1.0) * System.Math.Log(x) - x - lgm;
                if (d < -MathConstants.MaxLog)
                    goto ihalve;
                d = -System.Math.Exp(d);

                // compute the step to the next approximation of x
                d = (yy - y) / d;
                if (System.Math.Abs(d / x) < System.Double.Epsilon)
                    return x;
                x = x - d;
            }

            // Resort to interval halving if Newton iteration did not converge. 
            ihalve:

            d = 0.0625;
            if (x0 == System.Double.MaxValue)
            {
                if (x <= 0.0)
                    x = 1.0;

                while (x0 == System.Double.MaxValue && !System.Double.IsNaN(x))
                {
                    x = (1.0 + d) * x;
                    yy = Gamma.UpperIncomplete(a, x);
                    if (yy < y)
                    {
                        x0 = x;
                        yl = yy;
                        break;
                    }
                    d = d + d;
                }
            }

            d = 0.5;
            double dir = 0;

            for (int i = 0; i < 400; i++)
            {
                double t = x1 + d * (x0 - x1);

                if (System.Double.IsNaN(t))
                    break;

                x = t;
                yy = Gamma.UpperIncomplete(a, x);
                lgm = (x0 - x1) / (x1 + x0);

                if (System.Math.Abs(lgm) < dithresh)
                    break;

                lgm = (yy - y) / y;

                if (System.Math.Abs(lgm) < dithresh)
                    break;

                if (x <= 0.0)
                    break;

                if (yy >= y)
                {
                    x1 = x;
                    yh = yy;
                    if (dir < 0)
                    {
                        dir = 0;
                        d = 0.5;
                    }
                    else if (dir > 1)
                        d = 0.5 * d + 0.5;
                    else
                        d = (y - yl) / (yh - yl);
                    dir += 1;
                }
                else
                {
                    x0 = x;
                    yl = yy;
                    if (dir > 0)
                    {
                        dir = 0;
                        d = 0.5;
                    }
                    else if (dir < -1)
                        d = 0.5 * d;
                    else
                        d = (y - yl) / (yh - yl);
                    dir -= 1;
                }
            }

            if (x == 0.0 || System.Double.IsNaN(x))
                throw new ArithmeticException();

            return x;
        }

        /// <summary>
        ///   Inverse of the <see cref="UpperIncomplete">complemented 
        ///   incomplete Gamma integral (UpperIncomplete, Q)</see>.
        /// </summary>
        /// 
        [Obsolete("Please use InverseUpperIncomplete instead.")]
        public static double Inverse(double a, double y)
        {
            return InverseUpperIncomplete(a, y);
        }

        /// <summary>
        /// Returns the regularized gamma function P(a, x).
        /// </summary>
        /// <param name="a">the a parameter.</param>
        /// <param name="x">the value.</param>
        /// <returns>the regularized gamma function P(a, x)</returns>
        public static double RegularizedGammaP(double a, double x)
        {
            return RegularizedGammaP(a, x, DEFAULT_EPSILON, int.MaxValue);
        }

        /// <summary>
        /// Returns the regularized gamma function P(a, x).
        /// The implementation of this method is based on:
        /// <ul>
        /// <li>
        /// <a href="http://mathworld.wolfram.com/RegularizedGammaFunction.html">
        /// Regularized Gamma Function</a>, equation (1).</li>
        /// <li>
        /// <a href="http://mathworld.wolfram.com/IncompleteGammaFunction.html">
        /// Incomplete Gamma Function</a>, equation (4).</li>
        /// <li>
        /// <a href="http://mathworld.wolfram.com/ConfluentHypergeometricFunctionoftheFirstKind.html">
        /// Confluent Hypergeometric Function of the First Kind</a>, equation (1).
        /// </li>
        /// </ul>
        /// </summary>
        /// <param name="a">the a parameter.</param>
        /// <param name="x">the value.</param>
        /// <param name="epsilon">When the absolute value of the nth item in the series is less than epsilon the approximation ceases to calculate further elements in the series.</param>
        /// <param name="maxIterations">Maximum number of "iterations" to complete.</param>
        /// <returns>the regularized gamma function P(a, x)</returns>
        public static double RegularizedGammaP(double a, double x, double epsilon, int maxIterations)
        {
            double ret;
            if (System.Double.IsNaN(a) || System.Double.IsNaN(x) || (a <= 0.0) || (x < 0.0))
            {
                ret = System.Double.NaN;
            }
            else if (x == 0.0)
            {
                ret = 0.0;
            }
            else if (a >= 1.0 && x > a)
            {
                // use regularizedGammaQ because it should converge faster in this
                // case.
                ret = 1.0 - RegularizedGammaQ(a, x, epsilon, maxIterations);
            }
            else
            {
                // calculate series
                double n = 0.0; // current element index
                double an = 1.0 / a; // n-th element in the series
                double sum = an; // partial sum
                while (System.Math.Abs(an) > epsilon && n < maxIterations)
                {
                    // compute next element in the series
                    n = n + 1.0;
                    an = an * (x / (a + n));
                    // update partial sum
                    sum = sum + an;
                }
                if (n >= maxIterations)
                {
                    throw new IndexOutOfRangeException(maxIterations.ToString());
                }
                else
                {
                    ret = System.Math.Exp(-x + (a * System.Math.Log(x)) - Log(a)) * sum;
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns the regularized gamma function Q(a, x) = 1 - P(a, x).
        /// </summary>
        /// <param name="a">the a parameter.</param>
        /// <param name="x">the value.</param>
        /// <returns>the regularized gamma function Q(a, x)</returns>
        public static double RegularizedGammaQ(double a, double x)
        {
            return RegularizedGammaQ(a, x, DEFAULT_EPSILON, int.MaxValue);
        }

        /// <summary>
        /// Returns the regularized gamma function Q(a, x) = 1 - P(a, x).
        /// 
        /// The implementation of this method is based on:
        /// <ul>
        /// <li>
        /// <a href="http://mathworld.wolfram.com/RegularizedGammaFunction.html">
        /// Regularized Gamma Function</a>, equation (1).</li>
        /// <li>
        /// <a href="    http://functions.wolfram.com/GammaBetaErf/GammaRegularized/10/0003/">
        /// Regularized incomplete gamma function: Continued fraction representations  (formula 06.08.10.0003)</a></li>
        /// </ul>
        /// </summary>
        /// <param name="a">the a parameter.</param>
        /// <param name="x">the value.</param>
        /// <param name="epsilon">When the absolute value of the nth item in the series is less than epsilon the approximation ceases to calculate further elements in the series.</param>
        /// <param name="maxIterations">Maximum number of "iterations" to complete.</param>
        /// <returns>the regularized gamma function P(a, x)</returns>
        public static double RegularizedGammaQ(double a, double x, double epsilon, int maxIterations)
        {
            double ret;
            if (System.Double.IsNaN(a) || System.Double.IsNaN(x) || (a <= 0.0) || (x < 0.0))
            {
                ret = System.Double.NaN;
            }
            else if (x == 0.0)
            {
                ret = 1.0;
            }
            else if (x < a || a < 1.0)
            {
                // use regularizedGammaP because it should converge faster in this
                // case.
                ret = 1.0 - RegularizedGammaP(a, x, epsilon, maxIterations);
            }
            else
            {
                // create continued fraction
                ContinuedFraction cf = new GammaContinuedFraction(a);

                ret = 1.0 / cf.Evaluate(x, epsilon, maxIterations);
                ret = System.Math.Exp(-x + (a * System.Math.Log(x)) - Log(a)) * ret;
            }
            return ret;
        }

        private class GammaContinuedFraction : ContinuedFraction
        {
            double _a;
            public GammaContinuedFraction(System.Double a)
            {
                _a = a;
            }

            protected override double GetA(int n, double x)
            {
                return ((2.0 * n) + 1.0) - _a + x;
            }
            protected override double GetB(int n, double x)
            {
                return n * (_a - n);
            }
        }
    }
}

