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
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Language.Math.Matrix;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Math.Optimization.Linear
{

    /// <summary>
    /// A linear constraint for a linear optimization problem.
    /// <p>
    /// A linear constraint has one of the forms:
    /// <ul>
    ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> = v</li>
    ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> &lt;= v</li>
    ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> >= v</li>
    ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> =
    ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
    ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> &lt;=
    ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
    ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> >=
    ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
    /// </ul>
    /// The c<sub>i</sub>, l<sub>i</sub> or r<sub>i</sub> are the coefficients of the constraints, the x<sub>i</sub>
    /// are the coordinates of the current point and v is the value of the constraint.
    /// </p>
    /// @version $Revision: 922713 $ $Date: 2010-03-14 02:26:13 +0100 (dimd 14 mars 2010) $
    /// @since 2.0
    /// </summary>
    [Serializable]
    public class LinearConstraint
    {

        /// <summary>Coefficients of the constraint (left hand side)d */
        [NonSerialized]
        private Vector<Double> coefficients;

        /// <summary>Relationship between left and right hand sides (=, &lt;=, >=)d */
        private Relationship relationship;

        /// <summary>Value of the constraint (right hand side)d */
        private double value;

        /// <summary>
        /// Build a constraint involving a single linear equation.
        /// <p>
        /// A linear constraint with a single linear equation has one of the forms:
        /// <ul>
        ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> = v</li>
        ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> &lt;= v</li>
        ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> >= v</li>
        /// </ul>
        /// </p>
        /// </summary>
        /// <param Name="coefficients">The coefficients of the constraint (left hand side)</param>
        /// <param Name="relationship">The type of (in)equality used in the constraint</param>
        /// <param Name="value">The value of the constraint (right hand side)</param>
        public LinearConstraint(double[] coefficients, Relationship relationship, double value) : this(MatrixUtility.CreateRealVector(coefficients), relationship, value)
        {

        }

        /// <summary>
        /// Build a constraint involving a single linear equation.
        /// <p>
        /// A linear constraint with a single linear equation has one of the forms:
        /// <ul>
        ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> = v</li>
        ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> &lt;= v</li>
        ///   <li>c<sub>1</sub>x<sub>1</sub> + [] c<sub>n</sub>x<sub>n</sub> >= v</li>
        /// </ul>
        /// </p>
        /// </summary>
        /// <param Name="coefficients">The coefficients of the constraint (left hand side)</param>
        /// <param Name="relationship">The type of (in)equality used in the constraint</param>
        /// <param Name="value">The value of the constraint (right hand side)</param>
        public LinearConstraint(Vector<Double> coefficients, Relationship relationship, double value)
        {
            this.coefficients = coefficients;
            this.relationship = relationship;
            this.value = value;
        }

        /// <summary>
        /// Build a constraint involving two linear equations.
        /// <p>
        /// A linear constraint with two linear equation has one of the forms:
        /// <ul>
        ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> =
        ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
        ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> &lt;=
        ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
        ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> >=
        ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
        /// </ul>
        /// </p>
        /// </summary>
        /// <param Name="lhsCoefficients">The coefficients of the linear expression on the left hand side of the constraint</param>
        /// <param Name="lhsConstant">The constant term of the linear expression on the left hand side of the constraint</param>
        /// <param Name="relationship">The type of (in)equality used in the constraint</param>
        /// <param Name="rhsCoefficients">The coefficients of the linear expression on the right hand side of the constraint</param>
        /// <param Name="rhsConstant">The constant term of the linear expression on the right hand side of the constraint</param>
        public LinearConstraint(double[] lhsCoefficients, double lhsConstant, Relationship relationship, double[] rhsCoefficients, double rhsConstant)
        {
            double[] sub = new double[lhsCoefficients.Length];
            for (int i = 0; i < sub.Length; ++i)
            {
                sub[i] = lhsCoefficients[i] - rhsCoefficients[i];
            }
            this.coefficients = MatrixUtility.CreateRealVector(sub);
            this.relationship = relationship;
            this.value = rhsConstant - lhsConstant;
        }

        /// <summary>
        /// Build a constraint involving two linear equations.
        /// <p>
        /// A linear constraint with two linear equation has one of the forms:
        /// <ul>
        ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> =
        ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
        ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> &lt;=
        ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
        ///   <li>l<sub>1</sub>x<sub>1</sub> + [] l<sub>n</sub>x<sub>n</sub> + l<sub>cst</sub> >=
        ///       r<sub>1</sub>x<sub>1</sub> + [] r<sub>n</sub>x<sub>n</sub> + r<sub>cst</sub></li>
        /// </ul>
        /// </p>
        /// </summary>
        /// <param Name="lhsCoefficients">The coefficients of the linear expression on the left hand side of the constraint</param>
        /// <param Name="lhsConstant">The constant term of the linear expression on the left hand side of the constraint</param>
        /// <param Name="relationship">The type of (in)equality used in the constraint</param>
        /// <param Name="rhsCoefficients">The coefficients of the linear expression on the right hand side of the constraint</param>
        /// <param Name="rhsConstant">The constant term of the linear expression on the right hand side of the constraint</param>
        public LinearConstraint(Vector<Double> lhsCoefficients, double lhsConstant, Relationship relationship, Vector<Double> rhsCoefficients, double rhsConstant)
        {
            this.coefficients = lhsCoefficients.Subtract(rhsCoefficients);
            this.relationship = relationship;
            this.value = rhsConstant - lhsConstant;
        }

        /// <summary>
        /// Get the coefficients of the constraint (left hand side).
        /// </summary>
        /// <returns>coefficients of the constraint (left hand side)</returns>
        public Vector<Double> Coefficients
        {
            get { return coefficients; }
        }

        /// <summary>
        /// Get the relationship between left and right hand sides.
        /// </summary>
        /// <returns>relationship between left and right hand sides</returns>
        public Relationship Relationship
        {
            get { return relationship; }
        }

        /// <summary>
        /// Get the value of the constraint (right hand side).
        /// </summary>
        /// <returns>value of the constraint (right hand side)</returns>
        public double Value
        {
            get { return value; }
        }
    }
}
