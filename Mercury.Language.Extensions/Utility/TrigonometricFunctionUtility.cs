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
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;

namespace System
{
    /// <summary>
    /// TrigonometricFunctionUtility Description
    /// </summary>
    public static class TrigonometricFunctionUtility
    {
        private static Complex NEGATIVE_I = new Complex(0, -1);
        private static Complex I = new Complex(0, 1);

        public static double Acos(double x)
        {
            return System.Math.Acos(x);
        }

        /**
         * arccos - the inverse of cos
         * @param z A complex number
         * @return acos(z)
         */
        public static Complex Acos(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return Complex.Multiply(NEGATIVE_I, Complex.Log(Complex.Add(z, Complex.Sqrt(Complex.Subtract(Complex.Multiply(z, z), 1)))));
        }

        public static double Acosh(double x)
        {
            double y = x * x - 1;
            // ArgumentChecker.IsTrue(y >= 0, "|x|>=1.0 for real solution");
            return System.Math.Log(x + System.Math.Sqrt(x * x - 1));
        }

        public static Complex Acosh(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return Complex.Log(Complex.Add(z, Complex.Sqrt(Complex.Subtract(Complex.Multiply(z, z), 1))));
        }

        public static double Asin(double x)
        {
            return System.Math.Asin(x);
        }

        public static Complex Asin(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return Complex.Multiply(NEGATIVE_I,
                Complex.Log(Complex.Add(Complex.Multiply(I, z), Complex.Sqrt(Complex.Subtract(1, Complex.Multiply(z, z))))));
        }

        public static double Asinh(double x)
        {
            return System.Math.Log(x + System.Math.Sqrt(x * x + 1));
        }

        public static Complex Asinh(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return Complex.Log(Complex.Add(z, Complex.Sqrt(Complex.Add(Complex.Multiply(z, z), 1))));
        }

        public static double Atan(double x)
        {
            return System.Math.Atan(x);
        }

        public static Complex Atan(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            Complex iZ = Complex.Multiply(z, I);
            Complex half = new Complex(0, 0.5);
            return Complex.Multiply(half, Complex.Log(Complex.Divide(Complex.Subtract(1, iZ), Complex.Add(1, iZ))));
        }

        public static double Atanh(double x)
        {
            return 0.5 * System.Math.Log((1 + x) / (1 - x));
        }

        //TODO R White 21/07/2011 not sure why this was used over the equivalent below 
        //  public static Complex atanh(Complex z) {
        //    // ArgumentChecker.NotNull(z, "z");
        //    return Complex.Log(Complex.Divide(Complex.Sqrt(Complex.Subtract(1, Complex.Multiply(z, z))), Complex.Subtract(1, z)));
        //  }

        public static Complex Atanh(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return Complex.Multiply(0.5, Complex.Log(Complex.Divide(Complex.Add(1, z), Complex.Subtract(1, z))));
        }

        public static double Cos(double x)
        {
            return System.Math.Cos(x);
        }

        public static Complex Cos(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            double x = z.Real;
            double y = z.Imaginary;
            return new Complex(System.Math.Cos(x) * System.Math.Cosh(y), -System.Math.Sin(x) * System.Math.Sinh(y));
        }

        public static double Cosh(double x)
        {
            return System.Math.Cosh(x);
        }

        public static Complex Cosh(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return new Complex(System.Math.Cosh(z.Real) * System.Math.Cos(z.Imaginary), System.Math.Sinh(z.Real) * System.Math.Sin(z.Imaginary));
        }

        public static double Sin(double x)
        {
            return System.Math.Sin(x);
        }

        public static Complex Sin(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            double x = z.Real;
            double y = z.Imaginary;
            return new Complex(System.Math.Sin(x) * System.Math.Cosh(y), System.Math.Cos(x) * System.Math.Sinh(y));
        }

        public static double Sinh(double x)
        {
            return System.Math.Sinh(x);
        }

        public static Complex Sinh(Complex z)
        {
            // ArgumentChecker.NotNull(z, "z");
            return new Complex(System.Math.Sinh(z.Real) * System.Math.Cos(z.Imaginary), System.Math.Cosh(z.Real) * System.Math.Sin(z.Imaginary));
        }

        public static double Tan(double x)
        {
            return System.Math.Tan(x);
        }

        public static Complex Tan(Complex z)
        {
            Complex b = Complex.Exp(Complex.Multiply(Complex.Multiply(I, 2), z));
            return Complex.Divide(Complex.Subtract(b, 1), Complex.Multiply(I, Complex.Add(b, 1)));
        }

        public static double Tanh(double x)
        {
            return System.Math.Tanh(x);
        }

        public static Complex Tanh(Complex z)
        {
            Complex z2 = Complex.Exp(z);
            Complex z3 = Complex.Exp(Complex.Multiply(z, -1));
            return Complex.Divide(Complex.Subtract(z2, z3), Complex.Add(z2, z3));
        }
    }
}
