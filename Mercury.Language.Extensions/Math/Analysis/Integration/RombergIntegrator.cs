// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math Inc.
//
// Copyright (C) 2012 - present by System.Math Inc. and the System.Math group of companies
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

// Accord Core Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © AForge.NET, 2007-2011
// contacts@aforgenet.com
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math;
using Mercury.Language.Math.Ranges;

namespace Mercury.Language.Math.Analysis.Integration
{
    /// <summary>
    /// Romberg Description
    /// </summary>
    public class RombergIntegrator : IUnivariateIntegration, INumericalIntegration
    {
        private double[] s;
        private DoubleRange range = new DoubleRange(Double.NegativeInfinity, Double.PositiveInfinity);

        /// <summary>
        ///   Gets or sets the unidimensional function
        ///   whose integral should be computed.
        /// </summary>
        /// 
        public Func<double, double> Function { get; set; }

        /// <summary>
        ///   Gets the numerically computed result of the
        ///   definite integral for the specified function.
        /// </summary>
        /// 
        public double Area { get; private set; }

        /// <summary>
        ///   Gets or sets the number of steps used
        ///   by Romberg's method. Default is 6.
        /// </summary>
        /// 
        public int Steps { get { return s.Length; } }

        /// <summary>
        ///   Gets or sets the input range under
        ///   which the integral must be computed.
        /// </summary>
        /// 
        public DoubleRange Range
        {
            get { return range; }
            set
            {
                if (Double.IsInfinity(value.Min) || Double.IsNaN(value.Min))
                    throw new ArgumentOutOfRangeException("value", String.Format(LocalizedResources.Instance().PARAMETER_IS_OUT_OF_RANGE, "Minimum"));

                if (Double.IsInfinity(value.Max) || Double.IsNaN(value.Max))
                    throw new ArgumentOutOfRangeException("value", String.Format(LocalizedResources.Instance().PARAMETER_IS_OUT_OF_RANGE, "Maximum"));

                range = value;
            }
        }

        /// <summary>
        ///   Constructs a new <see cref="RombergIntegrator">Romberg's integration method</see>.
        /// </summary>
        /// 
        public RombergIntegrator() : this(6)
        {
        }

        /// <summary>
        ///   Constructs a new <see cref="RombergIntegrator">Romberg's integration method</see>.
        /// </summary>
        /// 
        /// <param name="function">The unidimensional function whose integral should be computed.</param>
        /// 
        public RombergIntegrator(Func<double, double> function) : this(6, function)
        {
        }

        /// <summary>
        ///   Constructs a new <see cref="RombergIntegrator">Romberg's integration method</see>.
        /// </summary>
        /// 
        /// <param name="function">The unidimensional function whose integral should be computed.</param>
        /// <param name="a">The beginning of the integration interval.</param>
        /// <param name="b">The ending of the integration interval.</param>
        ///
        public RombergIntegrator(Func<double, double> function, double a, double b) : this(6, function, a, b)
        {
        }

        /// <summary>
        ///   Constructs a new <see cref="RombergIntegrator">Romberg's integration method</see>.
        /// </summary>
        /// 
        /// <param name="steps">The number of steps used in Romberg's method. Default is 6.</param>
        /// 
        public RombergIntegrator(int steps)
        {
            this.s = new double[steps];
            Range = new DoubleRange(0, 1);
        }

        /// <summary>
        ///   Constructs a new <see cref="RombergIntegrator">Romberg's integration method</see>.
        /// </summary>
        /// 
        /// <param name="steps">The number of steps used in Romberg's method. Default is 6.</param>
        /// <param name="function">The unidimensional function whose integral should be computed.</param>
        ///
        public RombergIntegrator(int steps, Func<double, double> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            Range = new DoubleRange(0, 1);
            Function = function;
            this.s = new double[steps];
        }

        /// <summary>
        ///   Constructs a new <see cref="RombergIntegrator">Romberg's integration method</see>.
        /// </summary>
        /// 
        /// <param name="steps">The number of steps used in Romberg's method. Default is 6.</param>
        /// <param name="function">The unidimensional function whose integral should be computed.</param>
        /// <param name="a">The beginning of the integration interval.</param>
        /// <param name="b">The ending of the integration interval.</param>
        ///
        public RombergIntegrator(int steps, Func<double, double> function, double a, double b)
        {
            if (Double.IsInfinity(a) || Double.IsNaN(a))
                throw new ArgumentOutOfRangeException("a");

            if (Double.IsInfinity(b) || Double.IsNaN(b))
                throw new ArgumentOutOfRangeException("b");

            Function = function;
            Range = new DoubleRange(a, b);
            this.s = new double[steps];
        }

        /// <summary>
        ///   Computes the area of the function under the selected <see cref="Range"/>.
        ///   The computed value will be available at this object's <see cref="Area"/>.
        /// </summary>
        /// 
        /// <returns>
        ///   True if the integration method succeeds, false otherwise.
        /// </returns>
        /// 
        public bool Compute()
        {
            for (int i = 0; i < s.Length; i++)
                s[i] = 1;

            double sum = 0;
            double a = range.Min;
            double b = range.Max;

            for (int k = 0; k < s.Length; k++)
            {
                sum = s[0];
                s[0] = TrapezoidalRule.Integrate(Function, a, b, 1 << k);

                for (int i = 1; i <= k; i++)
                {
                    int p = (int)System.Math.Pow(4, i);
                    s[k] = (p * s[i - 1] - sum) / (p - 1);

                    sum = s[i];
                    s[i] = s[k];
                }
            }

            Area = s[s.Length - 1];

            return true;
        }


        /// <summary>
        ///   Computes the area under the integral for the given function, 
        ///   in the given integration interval, using Romberg's method.
        /// </summary>
        /// 
        /// <param name="func">The unidimensional function whose integral should be computed.</param>
        /// <param name="a">The beginning of the integration interval.</param>
        /// <param name="b">The ending of the integration interval.</param>
        /// 
        /// <returns>The integral's value in the current interval.</returns>
        /// 
        public static double Integrate(Func<double, double> func, double a, double b)
        {
            return Integrate(func, a, b, 6);
        }

        /// <summary>
        ///   Computes the area under the integral for the given function, 
        ///   in the given integration interval, using Romberg's method.
        /// </summary>
        /// 
        /// <param name="steps">The number of steps used in Romberg's method. Default is 6.</param>
        /// <param name="func">The unidimensional function whose integral should be computed.</param>
        /// <param name="a">The beginning of the integration interval.</param>
        /// <param name="b">The ending of the integration interval.</param>
        /// 
        /// <returns>The integral's value in the current interval.</returns>
        /// 
        public static double Integrate(Func<double, double> func, double a, double b, int steps)
        {
            var romberg = new RombergIntegrator(steps, func, a, b);

            romberg.Compute();

            return romberg.Area;
        }


        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// 
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        /// 
        public object Clone()
        {
            var clone = new RombergIntegrator(this.Steps,
                this.Function, this.Range.Min, this.Range.Max);

            return clone;
        }
    }
}
