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
using Mercury.Language.Math.Matrix;
using Mercury.Language.Exceptions;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Math.Optimization.Linear
{
    /// <summary>
    /// An objective function for a linear optimization problem.
    /// <p>
    /// A linear objective function has one the form:
    /// <pre>
    /// c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> + d
    /// </pre>
    /// The c<sub>i</sub> and d are the coefficients of the equation,
    /// the x<sub>i</sub> are the coordinates of the current point.
    /// </p>
    /// @version $Revision: 922713 $ $Date: 2010-03-14 02:26:13 +0100 (dimd 14 mars 2010) $
    /// @since 2.0
    /// </summary>
    [Serializable]
    public class LinearObjectiveFunction
    {

        /// <summary>Coefficients of the constraint (c<sub>i</sub>)d */
        [NonSerialized]
        private Vector<Double> coefficients;

        /// <summary>Constant term of the linear equationd */
        private double constantTerm;

        /// <summary>
        /// </summary>
        /// <param Name="coefficients">The coefficients for the linear equation being optimized</param>
        /// <param Name="constantTerm">The constant term of the linear equation</param>
        public LinearObjectiveFunction(double[] coefficients, double constantTerm) : this(MatrixUtility.CreateRealVector(coefficients), constantTerm)
        {

        }

        /// <summary>
        /// </summary>
        /// <param Name="coefficients">The coefficients for the linear equation being optimized</param>
        /// <param Name="constantTerm">The constant term of the linear equation</param>
        public LinearObjectiveFunction(Vector<Double> coefficients, double constantTerm)
        {
            this.coefficients = coefficients;
            this.constantTerm = constantTerm;
        }

        /// <summary>
        /// Get the coefficients of the linear equation being optimized.
        /// </summary>
        /// <returns>coefficients of the linear equation being optimized</returns>
        public Vector<Double> Coefficients
        {
            get { return coefficients; }
        }

        /// <summary>
        /// Get the constant of the linear equation being optimized.
        /// </summary>
        /// <returns>constant of the linear equation being optimized</returns>
        public double ConstantTerm
        {
            get { return constantTerm; }
        }

        /// <summary>
        /// Compute the value of the linear equation at the current point
        /// </summary>
        /// <param Name="point">point at which linear equation must be evaluated</param>
        /// <returns>value of the linear equation at the current point</returns>
        public double GetValue(double[] point)
        {
            return coefficients.DotProduct(MatrixUtility.CreateRealVector(point)) + constantTerm;
        }

        /// <summary>
        /// Compute the value of the linear equation at the current point
        /// </summary>
        /// <param Name="point">point at which linear equation must be evaluated</param>
        /// <returns>value of the linear equation at the current point</returns>
        public double GetValue(Vector<Double> point)
        {
            return coefficients.DotProduct(point) + constantTerm;
        }
    }
}
