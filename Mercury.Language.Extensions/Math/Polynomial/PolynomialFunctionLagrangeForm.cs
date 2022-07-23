// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Polynomial Inc.
//
// Copyright (C) 2012 - present by System.Math.Polynomial Incd and the System.Math.Polynomial group of companies
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

namespace Mercury.Language.Math.Polynomial
{
    /// <summary>
    /// PolynomialFunctionLagrangeForm Description
    /// </summary>
    public class PolynomialFunctionLagrangeForm
    {

        #region Local Variables
        private double[] coefficients;
        private double[] x;
        private double[] y;
        private Boolean coefficientsComputed;
        #endregion

        #region Property

        public int Degree
        {
            get { return x.Length - 1; }
        }

        public double[] InterpolatingPoints
        {
            get
            {
                double[] o = new double[x.Length];
                Array.Copy(x, 0, o, 0, x.Length);
                return o;
            }
        }

        public double[] InterpolatingValues
        {
            get
            {
                double[] o = new double[y.Length];
                Array.Copy(y, 0, o, 0, y.Length);
                return o;
            }
        }

        public double[] Coefficients
        {
            get
            {
                if (!coefficientsComputed)
                {
                    ComputeCoefficients();
                }
                double[] ot = new double[coefficients.Length];
                Array.Copy(coefficients, 0, ot, 0, coefficients.Length);
                return ot;
            }
        }
        #endregion

        #region Constructor
        public PolynomialFunctionLagrangeForm(double[] x, double[] y)
        {
            this.x = new double[x.Length];
            this.y = new double[y.Length];
            Array.Copy(x, 0, this.x, 0, x.Length);
            Array.Copy(y, 0, this.y, 0, y.Length);
            coefficientsComputed = false;

            if (!VerifyInterpolationArray(x, y, false))
            {
                Array.Sort(this.x, this.y);
                // Second check in case some abscissa is duplicated.
                VerifyInterpolationArray(this.x, this.y, true);
            }
        }
        #endregion

        #region Implement Methods

        #endregion

        #region Local Public Methods
        public double Value(double z)
        {
            return EvaluateInternal(x, y, z);
        }

        public static double Evaluate(double[] x, double[] y, double z)
        {
            if (VerifyInterpolationArray(x, y, false))
            {
                return EvaluateInternal(x, y, z);
            }

            // Array is not sorted.
            double[] xNew = new double[x.Length];
            double[] yNew = new double[y.Length];
            Array.Copy(x, 0, xNew, 0, x.Length);
            Array.Copy(y, 0, yNew, 0, y.Length);

            Array.Sort(xNew, yNew);
            // Second check in case some abscissa is duplicated.
            VerifyInterpolationArray(xNew, yNew, true);
            return EvaluateInternal(xNew, yNew, z);
        }

        protected void ComputeCoefficients()
        {
            int n = Degree + 1;
            coefficients = new double[n];
            for (int i = 0; i < n; i++)
            {
                coefficients[i] = 0.0;
            }

            // c[] are the coefficients of P(x) = (x-x[0])(x-x[1])...(x-x[n-1])
            double[] c = new double[n + 1];
            c[0] = 1.0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    c[j] = c[j - 1] - c[j] * x[i];
                }
                c[0] *= -x[i];
                c[i + 1] = 1;
            }

            double[] tc = new double[n];
            for (int i = 0; i < n; i++)
            {
                // d = (x[i]-x[0])...(x[i]-x[i-1])(x[i]-x[i+1])...(x[i]-x[n-1])
                double d = 1;
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        d *= x[i] - x[j];
                    }
                }
                double t = y[i] / d;
                // Lagrange polynomial is the sum of n terms, each of which is a
                // polynomial of degree n-1d tc[] are the coefficients of the i-th
                // numerator Pi(x) = (x-x[0])...(x-x[i-1])(x-x[i+1])...(x-x[n-1]).
                tc[n - 1] = c[n];     // actually c[n] = 1
                coefficients[n - 1] += t * tc[n - 1];
                for (int j = n - 2; j >= 0; j--)
                {
                    tc[j] = c[j + 1] + tc[j + 1] * x[i];
                    coefficients[j] += t * tc[j];
                }
            }

            coefficientsComputed = true;
        }

        public static Boolean VerifyInterpolationArray(double[] x, double[] y, Boolean abort)
        {
            if (x.Length != y.Length)
            {
                throw new DimensionMismatchException(x.Length, y.Length);
            }
            if (x.Length < 2)
            {
                throw new NumberIsTooSmallException(LocalizedResources.Instance().WRONG_NUMBER_OF_POINTS, 2, x.Length, true);
            }

            //return MathArrays.checkOrder(x, MathArrays.OrderDirection.INCREASING, true, abort);
            return x.IsSorted();
        }

        #endregion

        #region Local Private Methods

        private static double EvaluateInternal(double[] x, double[] y, double z)
        {
            int nearest = 0;
            int n = x.Length;
            double[] c = new double[n];
            double[] d = new double[n];
            double min_dist = Double.PositiveInfinity;
            for (int i = 0; i < n; i++)
            {
                // initialize the difference arrays
                c[i] = y[i];
                d[i] = y[i];
                // find out the abscissa closest to z
                double dist = System.Math.Abs(z - x[i]);
                if (dist < min_dist)
                {
                    nearest = i;
                    min_dist = dist;
                }
            }

            // initial approximation to the function value at z
            double value = y[nearest];

            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < n - i; j++)
                {
                    double tc = x[j] - z;
                    double td = x[i + j] - z;
                    double divider = x[j] - x[i + j];
                    // update the difference arrays
                    double w = (c[j + 1] - d[j]) / divider;
                    c[j] = tc * w;
                    d[j] = td * w;
                }
                // sum up the difference terms to get the value
                if (nearest < 0.5 * (n - i + 1))
                {
                    value += c[nearest];    // fork down
                }
                else
                {
                    nearest--;
                    value += d[nearest];    // fork up
                }
            }

            return value;
        }
        #endregion
    }
}
