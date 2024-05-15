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
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Function that  :  the
    /// <a href="http://en.wikipedia.org/wiki/Tricubic_interpolation">
    /// tricubic spline interpolation</a>, as proposed in
    /// <quote>
    ///  Tricubic interpolation in three dimensions<br/>
    ///  Fd Lekien and Jd Marsden<br/>
    ///  <em>Intd Jd Numerd Methd Engng</em> 2005; <b>63</b>:455-471
    /// </quote>
    /// 
    /// @version $Revision$ $Date$
    /// @since 2.2
    /// </summary>
    public class TricubicSplineInterpolatingFunction : ITrivariateRealFunction
    {
        /// <summary>
        /// Matrix to compute the spline coefficients from the function values
        /// and function derivatives values
        /// </summary>
        private static double[][] AINV = new double[][] {
                new double[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ -3,3,0,0,0,0,0,0,-2,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 2,-2,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ -3,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,-3,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 9,-9,-9,9,0,0,0,0,6,3,-6,-3,0,0,0,0,6,-6,3,-3,0,0,0,0,0,0,0,0,0,0,0,0,4,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ -6,6,6,-6,0,0,0,0,-3,-3,3,3,0,0,0,0,-4,4,-2,2,0,0,0,0,0,0,0,0,0,0,0,0,-2,-2,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 2,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,2,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ -6,6,6,-6,0,0,0,0,-4,-2,4,2,0,0,0,0,-3,3,-3,3,0,0,0,0,0,0,0,0,0,0,0,0,-2,-1,-2,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 4,-4,-4,4,0,0,0,0,2,2,-2,-2,0,0,0,0,2,-2,2,-2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,3,0,0,0,0,0,0,-2,-1,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,-2,0,0,0,0,0,0,1,1,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,-1,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,-1,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,-9,-9,9,0,0,0,0,0,0,0,0,0,0,0,0,6,3,-6,-3,0,0,0,0,6,-6,3,-3,0,0,0,0,4,2,2,1,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-6,6,6,-6,0,0,0,0,0,0,0,0,0,0,0,0,-3,-3,3,3,0,0,0,0,-4,4,-2,2,0,0,0,0,-2,-2,-1,-1,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-6,6,6,-6,0,0,0,0,0,0,0,0,0,0,0,0,-4,-2,4,2,0,0,0,0,-3,3,-3,3,0,0,0,0,-2,-1,-2,-1,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,-4,-4,4,0,0,0,0,0,0,0,0,0,0,0,0,2,2,-2,-2,0,0,0,0,2,-2,2,-2,0,0,0,0,1,1,1,1,0,0,0,0 },
                new double[]{-3,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,0,0,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,-3,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,0,0,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 9,-9,0,0,-9,9,0,0,6,3,0,0,-6,-3,0,0,0,0,0,0,0,0,0,0,6,-6,0,0,3,-3,0,0,0,0,0,0,0,0,0,0,4,2,0,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ -6,6,0,0,6,-6,0,0,-3,-3,0,0,3,3,0,0,0,0,0,0,0,0,0,0,-4,4,0,0,-2,2,0,0,0,0,0,0,0,0,0,0,-2,-2,0,0,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,0,0,-1,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,0,0,-1,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,-9,0,0,-9,9,0,0,0,0,0,0,0,0,0,0,6,3,0,0,-6,-3,0,0,0,0,0,0,0,0,0,0,6,-6,0,0,3,-3,0,0,4,2,0,0,2,1,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-6,6,0,0,6,-6,0,0,0,0,0,0,0,0,0,0,-3,-3,0,0,3,3,0,0,0,0,0,0,0,0,0,0,-4,4,0,0,-2,2,0,0,-2,-2,0,0,-1,-1,0,0 },
                new double[]{ 9,0,-9,0,-9,0,9,0,0,0,0,0,0,0,0,0,6,0,3,0,-6,0,-3,0,6,0,-6,0,3,0,-3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,2,0,2,0,1,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,9,0,-9,0,-9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,3,0,-6,0,-3,0,6,0,-6,0,3,0,-3,0,0,0,0,0,0,0,0,0,4,0,2,0,2,0,1,0 },
                new double[]{ -27,27,27,-27,27,-27,-27,27,-18,-9,18,9,18,9,-18,-9,-18,18,-9,9,18,-18,9,-9,-18,18,18,-18,-9,9,9,-9,-12,-6,-6,-3,12,6,6,3,-12,-6,12,6,-6,-3,6,3,-12,12,-6,6,-6,6,-3,3,-8,-4,-4,-2,-4,-2,-2,-1 },
                new double[]{ 18,-18,-18,18,-18,18,18,-18,9,9,-9,-9,-9,-9,9,9,12,-12,6,-6,-12,12,-6,6,12,-12,-12,12,6,-6,-6,6,6,6,3,3,-6,-6,-3,-3,6,6,-6,-6,3,3,-3,-3,8,-8,4,-4,4,-4,2,-2,4,4,2,2,2,2,1,1 },
                new double[]{ -6,0,6,0,6,0,-6,0,0,0,0,0,0,0,0,0,-3,0,-3,0,3,0,3,0,-4,0,4,0,-2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,-2,0,-1,0,-1,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,-6,0,6,0,6,0,-6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,-3,0,3,0,3,0,-4,0,4,0,-2,0,2,0,0,0,0,0,0,0,0,0,-2,0,-2,0,-1,0,-1,0 },
                new double[]{ 18,-18,-18,18,-18,18,18,-18,12,6,-12,-6,-12,-6,12,6,9,-9,9,-9,-9,9,-9,9,12,-12,-12,12,6,-6,-6,6,6,3,6,3,-6,-3,-6,-3,8,4,-8,-4,4,2,-4,-2,6,-6,6,-6,3,-3,3,-3,4,2,4,2,2,1,2,1 },
                new double[]{ -12,12,12,-12,12,-12,-12,12,-6,-6,6,6,6,6,-6,-6,-6,6,-6,6,6,-6,6,-6,-8,8,8,-8,-4,4,4,-4,-3,-3,-3,-3,3,3,3,3,-4,-4,4,4,-2,-2,2,2,-4,4,-4,4,-2,2,-2,2,-2,-2,-2,-2,-1,-1,-1,-1 },
                new double[]{ 2,0,0,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,2,0,0,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ -6,6,0,0,6,-6,0,0,-4,-2,0,0,4,2,0,0,0,0,0,0,0,0,0,0,-3,3,0,0,-3,3,0,0,0,0,0,0,0,0,0,0,-2,-1,0,0,-2,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 4,-4,0,0,-4,4,0,0,2,2,0,0,-2,-2,0,0,0,0,0,0,0,0,0,0,2,-2,0,0,2,-2,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-6,6,0,0,6,-6,0,0,0,0,0,0,0,0,0,0,-4,-2,0,0,4,2,0,0,0,0,0,0,0,0,0,0,-3,3,0,0,-3,3,0,0,-2,-1,0,0,-2,-1,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,-4,0,0,-4,4,0,0,0,0,0,0,0,0,0,0,2,2,0,0,-2,-2,0,0,0,0,0,0,0,0,0,0,2,-2,0,0,2,-2,0,0,1,1,0,0,1,1,0,0 },
                new double[]{ -6,0,6,0,6,0,-6,0,0,0,0,0,0,0,0,0,-4,0,-2,0,4,0,2,0,-3,0,3,0,-3,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,-1,0,-2,0,-1,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,-6,0,6,0,6,0,-6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-4,0,-2,0,4,0,2,0,-3,0,3,0,-3,0,3,0,0,0,0,0,0,0,0,0,-2,0,-1,0,-2,0,-1,0 },
                new double[]{ 18,-18,-18,18,-18,18,18,-18,12,6,-12,-6,-12,-6,12,6,12,-12,6,-6,-12,12,-6,6,9,-9,-9,9,9,-9,-9,9,8,4,4,2,-8,-4,-4,-2,6,3,-6,-3,6,3,-6,-3,6,-6,3,-3,6,-6,3,-3,4,2,2,1,4,2,2,1 },
                new double[]{ -12,12,12,-12,12,-12,-12,12,-6,-6,6,6,6,6,-6,-6,-8,8,-4,4,8,-8,4,-4,-6,6,6,-6,-6,6,6,-6,-4,-4,-2,-2,4,4,2,2,-3,-3,3,3,-3,-3,3,3,-4,4,-2,2,-4,4,-2,2,-2,-2,-1,-1,-2,-2,-1,-1 },
                new double[]{ 4,0,-4,0,-4,0,4,0,0,0,0,0,0,0,0,0,2,0,2,0,-2,0,-2,0,2,0,-2,0,2,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0 },
                new double[]{ 0,0,0,0,0,0,0,0,4,0,-4,0,-4,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,0,-2,0,-2,0,2,0,-2,0,2,0,-2,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0 },
                new double[]{ -12,12,12,-12,12,-12,-12,12,-8,-4,8,4,8,4,-8,-4,-6,6,-6,6,6,-6,6,-6,-6,6,6,-6,-6,6,6,-6,-4,-2,-4,-2,4,2,4,2,-4,-2,4,2,-4,-2,4,2,-3,3,-3,3,-3,3,-3,3,-2,-1,-2,-1,-2,-1,-2,-1 },
                new double[]{ 8,-8,-8,8,-8,8,8,-8,4,4,-4,-4,-4,-4,4,4,4,-4,4,-4,-4,4,-4,4,4,-4,-4,4,4,-4,-4,4,2,2,2,2,-2,-2,-2,-2,2,2,-2,-2,2,2,-2,-2,2,-2,2,-2,2,-2,2,-2,1,1,1,1,1,1,1,1 }
    };

        /// <summary>Samples x-coordinates */
        private double[] xval;
        /// <summary>Samples y-coordinates */
        private double[] yval;
        /// <summary>Samples z-coordinates */
        private double[] zval;
        /// <summary>Set of cubic splines pacthing the whole data grid */
        private TricubicSplineFunction[][][] splines;

        /// <summary>
        /// </summary>
        /// <param Name="x">Sample values of the x-coordinate, in increasing order.</param>
        /// <param Name="y">Sample values of the y-coordinate, in increasing order.</param>
        /// <param Name="z">Sample values of the y-coordinate, in increasing order.</param>
        /// <param Name="f">Values of the function on every grid point.</param>
        /// <param Name="dFdX">Values of the partial derivative of function with respect</param>
        /// to x on every grid point.
        /// <param Name="dFdY">Values of the partial derivative of function with respect</param>
        /// to y on every grid point.
        /// <param Name="dFdZ">Values of the partial derivative of function with respect</param>
        /// to z on every grid point.
        /// <param Name="d2FdXdY">Values of the cross partial derivative of function on</param>
        /// every grid point.
        /// <param Name="d2FdXdZ">Values of the cross partial derivative of function on</param>
        /// every grid point.
        /// <param Name="d2FdYdZ">Values of the cross partial derivative of function on</param>
        /// every grid point.
        /// <param Name="d3FdXdYdZ">Values of the cross partial derivative of function on</param>
        /// every grid point.
        /// <exception cref="DataNotFoundException">if any of the arrays has zero lengthd </exception>
        /// <exception cref="DimensionMismatchException">if the various arrays do not contain </exception>
        /// the expected number of elements.
        /// <exception cref="ArgumentException">if {@code x}, {@code y} or {@code z} </exception>
        /// are not strictly increasing.
        public TricubicSplineInterpolatingFunction(double[] x,
                                                   double[] y,
                                                   double[] z,
                                                   double[][][] f,
                                                   double[][][] dFdX,
                                                   double[][][] dFdY,
                                                   double[][][] dFdZ,
                                                   double[][][] d2FdXdY,
                                                   double[][][] d2FdXdZ,
                                                   double[][][] d2FdYdZ,
                                                   double[][][] d3FdXdYdZ)
        {
            int xLen = x.Length;
            int yLen = y.Length;
            int zLen = z.Length;

            if (xLen == 0 || yLen == 0 || z.Length == 0 || f.Length == 0 || f.GetLength(1) == 0)
            {
                throw new DataNotFoundException();
            }
            if (xLen != f.Length)
            {
                throw new DimensionMismatchException(xLen, f.Length);
            }
            if (xLen != dFdX.Length)
            {
                throw new DimensionMismatchException(xLen, dFdX.Length);
            }
            if (xLen != dFdY.Length)
            {
                throw new DimensionMismatchException(xLen, dFdY.Length);
            }
            if (xLen != dFdZ.Length)
            {
                throw new DimensionMismatchException(xLen, dFdZ.Length);
            }
            if (xLen != d2FdXdY.Length)
            {
                throw new DimensionMismatchException(xLen, d2FdXdY.Length);
            }
            if (xLen != d2FdXdZ.Length)
            {
                throw new DimensionMismatchException(xLen, d2FdXdZ.Length);
            }
            if (xLen != d2FdYdZ.Length)
            {
                throw new DimensionMismatchException(xLen, d2FdYdZ.Length);
            }
            if (xLen != d3FdXdYdZ.Length)
            {
                throw new DimensionMismatchException(xLen, d3FdXdYdZ.Length);
            }

            QuickMath.CheckOrder(x);
            QuickMath.CheckOrder(y);
            QuickMath.CheckOrder(z);

            xval = x.CloneExact();
            yval = y.CloneExact();
            zval = z.CloneExact();

            int lastI = xLen - 1;
            int lastJ = yLen - 1;
            int lastK = zLen - 1;
            splines = ArrayUtility.CreateJaggedArray<TricubicSplineFunction>(lastI, lastJ, lastK);

            AutoParallel.AutoParallelFor(0, lastI, (i) =>
            {
                if (f[i].Length != yLen)
                {
                    throw new DimensionMismatchException(f[i].Length, yLen);
                }
                if (dFdX[i].Length != yLen)
                {
                    throw new DimensionMismatchException(dFdX[i].Length, yLen);
                }
                if (dFdY[i].Length != yLen)
                {
                    throw new DimensionMismatchException(dFdY[i].Length, yLen);
                }
                if (dFdZ[i].Length != yLen)
                {
                    throw new DimensionMismatchException(dFdZ[i].Length, yLen);
                }
                if (d2FdXdY[i].Length != yLen)
                {
                    throw new DimensionMismatchException(d2FdXdY[i].Length, yLen);
                }
                if (d2FdXdZ[i].Length != yLen)
                {
                    throw new DimensionMismatchException(d2FdXdZ[i].Length, yLen);
                }
                if (d2FdYdZ[i].Length != yLen)
                {
                    throw new DimensionMismatchException(d2FdYdZ[i].Length, yLen);
                }
                if (d3FdXdYdZ[i].Length != yLen)
                {
                    throw new DimensionMismatchException(d3FdXdYdZ[i].Length, yLen);
                }

                int ip1 = i + 1;
                AutoParallel.AutoParallelFor(0, lastJ, (j) =>
                {
                    if (f[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(f[i][j].Length, zLen);
                    }
                    if (dFdX[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(dFdX[i][j].Length, zLen);
                    }
                    if (dFdY[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(dFdY[i][j].Length, zLen);
                    }
                    if (dFdZ[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(dFdZ[i][j].Length, zLen);
                    }
                    if (d2FdXdY[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(d2FdXdY[i][j].Length, zLen);
                    }
                    if (d2FdXdZ[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(d2FdXdZ[i][j].Length, zLen);
                    }
                    if (d2FdYdZ[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(d2FdYdZ[i][j].Length, zLen);
                    }
                    if (d3FdXdYdZ[i][j].Length != zLen)
                    {
                        throw new DimensionMismatchException(d3FdXdYdZ[i][j].Length, zLen);
                    }

                    int jp1 = j + 1;
                    AutoParallel.AutoParallelFor(0, lastK, (k) =>
                    {
                        int kp1 = k + 1;

                        double[] beta = new double[] {
                                    f[i][j][k], f[ip1][j][k],
                                    f[i][jp1][k], f[ip1][jp1][k],
                                    f[i][j][kp1], f[ip1][j][kp1],
                                    f[i][jp1][kp1], f[ip1][jp1][kp1],

                                    dFdX[i][j][k], dFdX[ip1][j][k],
                                    dFdX[i][jp1][k], dFdX[ip1][jp1][k],
                                    dFdX[i][j][kp1], dFdX[ip1][j][kp1],
                                    dFdX[i][jp1][kp1], dFdX[ip1][jp1][kp1],

                                    dFdY[i][j][k], dFdY[ip1][j][k],
                                    dFdY[i][jp1][k], dFdY[ip1][jp1][k],
                                    dFdY[i][j][kp1], dFdY[ip1][j][kp1],
                                    dFdY[i][jp1][kp1], dFdY[ip1][jp1][kp1],

                                    dFdZ[i][j][k], dFdZ[ip1][j][k],
                                    dFdZ[i][jp1][k], dFdZ[ip1][jp1][k],
                                    dFdZ[i][j][kp1], dFdZ[ip1][j][kp1],
                                    dFdZ[i][jp1][kp1], dFdZ[ip1][jp1][kp1],

                                    d2FdXdY[i][j][k], d2FdXdY[ip1][j][k],
                                    d2FdXdY[i][jp1][k], d2FdXdY[ip1][jp1][k],
                                    d2FdXdY[i][j][kp1], d2FdXdY[ip1][j][kp1],
                                    d2FdXdY[i][jp1][kp1], d2FdXdY[ip1][jp1][kp1],

                                    d2FdXdZ[i][j][k], d2FdXdZ[ip1][j][k],
                                    d2FdXdZ[i][jp1][k], d2FdXdZ[ip1][jp1][k],
                                    d2FdXdZ[i][j][kp1], d2FdXdZ[ip1][j][kp1],
                                    d2FdXdZ[i][jp1][kp1], d2FdXdZ[ip1][jp1][kp1],

                                    d2FdYdZ[i][j][k], d2FdYdZ[ip1][j][k],
                                    d2FdYdZ[i][jp1][k], d2FdYdZ[ip1][jp1][k],
                                    d2FdYdZ[i][j][kp1], d2FdYdZ[ip1][j][kp1],
                                    d2FdYdZ[i][jp1][kp1], d2FdYdZ[ip1][jp1][kp1],

                                    d3FdXdYdZ[i][j][k], d3FdXdYdZ[ip1][j][k],
                                    d3FdXdYdZ[i][jp1][k], d3FdXdYdZ[ip1][jp1][k],
                                    d3FdXdYdZ[i][j][kp1], d3FdXdYdZ[ip1][j][kp1],
                                    d3FdXdYdZ[i][jp1][kp1], d3FdXdYdZ[ip1][jp1][kp1],
                        };

                        splines[i][j][k] = new TricubicSplineFunction(computeSplineCoefficients(beta));
                    });
                });
            });
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public double Value(double x, double y, double z)
        {
            int i = searchIndex(x, xval);
            if (i == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, xval[0], xval[xval.Length - 1]));
            }
            int j = searchIndex(y, yval);
            if (j == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, y, yval[0], yval[yval.Length - 1]));
            }
            int k = searchIndex(z, zval);
            if (k == -1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, z, zval[0], zval[zval.Length - 1]));
            }

            double xN = (x - xval[i]) / (xval[i + 1] - xval[i]);
            double yN = (y - yval[j]) / (yval[j + 1] - yval[j]);
            double zN = (z - zval[k]) / (zval[k + 1] - zval[k]);

            return splines[i][j][k].Value(xN, yN, zN);
        }

        /// <summary>
        /// </summary>
        /// <param Name="c">Coordinate.</param>
        /// <param Name="val">Coordinate samples.</param>
        /// <returns>the index in {@code val} corresponding to the interval</returns>
        /// containing {@code c}, or {@code -1} if {@code c} is out of the
        /// range defined by the end values of {@code val}.
        private int searchIndex(double c, double[] val)
        {
            if (c < val[0])
            {
                return -1;
            }

            int max = val.Length;
            for(int i = 1; i < max; i++)
        {
                if (c <= val[i])
                {
                    return i - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Compute the spline coefficients from the list of function values and
        /// function partial derivatives values at the four corners of a grid
        /// elementd They must be specified in the following order:
        /// <ul>
        ///  <li>f(0,0,0)</li>
        ///  <li>f(1,0,0)</li>
        ///  <li>f(0,1,0)</li>
        ///  <li>f(1,1,0)</li>
        ///  <li>f(0,0,1)</li>
        ///  <li>f(1,0,1)</li>
        ///  <li>f(0,1,1)</li>
        ///  <li>f(1,1,1)</li>
        /// 
        ///  <li>f<sub>x</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>x</sub>(1,1,1)</li>
        /// 
        ///  <li>f<sub>y</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>y</sub>(1,1,1)</li>
        /// 
        ///  <li>f<sub>z</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>z</sub>(1,1,1)</li>
        /// 
        ///  <li>f<sub>xy</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>xy</sub>(1,1,1)</li>
        /// 
        ///  <li>f<sub>xz</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>xz</sub>(1,1,1)</li>
        /// 
        ///  <li>f<sub>yz</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>yz</sub>(1,1,1)</li>
        /// 
        ///  <li>f<sub>xyz</sub>(0,0,0)</li>
        ///  <li>[] <em>(same order as above)</em></li>
        ///  <li>f<sub>xyz</sub>(1,1,1)</li>
        /// </ul>
        /// where the subscripts indicate the partial derivative with respect to
        /// the corresponding variable(s).
        /// 
        /// </summary>
        /// <param Name="beta">List of function values and function partial derivatives</param>
        /// values.
        /// <returns>the spline coefficients.</returns>
        private double[] computeSplineCoefficients(double[] beta)
        {
            int sz = 64;
            double[] a = new double[sz];

            AutoParallel.AutoParallelFor(0, sz, (i) =>
            {
                double result = 0;
                double[] row = AINV[i];
                AutoParallel.AutoParallelFor(0, sz, (j) =>
                {
                    result += row[j] * beta[j];
                });
                a[i] = result;
            });

            return a;
        }
    }

    /// <summary>
    /// 3D-spline function.
    /// 
    /// @version $Revision$ $Date$
    /// </summary>
    class TricubicSplineFunction
         : ITrivariateRealFunction
    {
        /// <summary>Number of pointsd */
        private static short N = 4;
        /// <summary>Coefficients */
        private double[][][] a = ArrayUtility.CreateJaggedArray<double>(N, N, N);

        /// <summary>
        /// </summary>
        /// <param Name="aV">List of spline coefficients.</param>
        public TricubicSplineFunction(double[] aV)
        {
            AutoParallel.AutoParallelFor(0, N, (i) =>
            {
                AutoParallel.AutoParallelFor(0, N, (j) =>
                {
                    AutoParallel.AutoParallelFor(0, N, (k) =>
                    {
                        a[i][j][k] = aV[i + N * (j + N * k)];
                    });
                });
            });
        }

        /// <summary>
        /// </summary>
        /// <param Name="x">x-coordinate of the interpolation point.</param>
        /// <param Name="y">y-coordinate of the interpolation point.</param>
        /// <param Name="z">z-coordinate of the interpolation point.</param>
        /// <returns>the interpolated value.</returns>
        public double Value(double x, double y, double z)
        {
            if (x < 0 || x > 1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, x, 0, 1));
            }
            if (y < 0 || y > 1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, y, 0, 1));
            }
            if (z < 0 || z > 1)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, z, 0, 1));
            }

            double x2 = x * x;
            double x3 = x2 * x;
            double[] pX = { 1, x, x2, x3 };

            double y2 = y * y;
            double y3 = y2 * y;
            double[] pY = { 1, y, y2, y3 };

            double z2 = z * z;
            double z3 = z2 * z;
            double[] pZ = { 1, z, z2, z3 };

            double result = 0;
            AutoParallel.AutoParallelFor(0, N, (i) =>
            {
                AutoParallel.AutoParallelFor(0, N, (j) =>
                {
                    AutoParallel.AutoParallelFor(0, N, (k) =>
                    {
                        result += a[i][j][k] * pX[i] * pY[j] * pZ[k];
                    });
                });
            });

            return result;
        }
    }
}
