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


using Mercury.Language.Math.Optimization;
using MathNet.Numerics.LinearAlgebra;


namespace Mercury.Language.Math.Optimization.Linear
{
    /// <summary>
    /// A tableau for use in the Simplex method.
    /// 
    /// <p>
    /// Example:
    /// <pre>
    ///   W |  Z |  x1 |  x2 |  x- | s1 |  s2 |  a1 |  RHS
    /// ---------------------------------------------------
    ///  -1    0    0     0     0     0     0     1     0   &lt;= phase 1 objective
    ///   0    1   -15   -10    0     0     0     0     0   &lt;= phase 2 objective
    ///   0    0    1     0     0     1     0     0     2   &lt;= constraint 1
    ///   0    0    0     1     0     0     1     0     3   &lt;= constraint 2
    ///   0    0    1     1     0     0     0     1     4   &lt;= constraint 3
    /// </pre>
    /// W: Phase 1 objective function</br>
    /// Z: Phase 2 objective function</br>
    /// x1 &amp; x2: Decision variables</br>
    /// x-: Extra decision variable to allow for negative values</br>
    /// s1 &amp; s2: Slack/Surplus variables</br>
    /// a1: Artificial variable</br>
    /// RHS: Right hand side</br>
    /// </p>
    /// @version $Revision: 922713 $ $Date: 2010-03-14 02:26:13 +0100 (dimd 14 mars 2010) $
    /// @since 2.0
    /// </summary>
    [Serializable]
    public class SimplexTableau
    {

        /// <summary>Column label for negative varsd */
        private static String NEGATIVE_VAR_COLUMN_LABEL = "x-";


        /// <summary>Linear objective functiond */
        private LinearObjectiveFunction f;

        /// <summary>Linear constraintsd */
        private List<LinearConstraint> constraints;

        /// <summary>Whether to restrict the variables to non-negative valuesd */
        private Boolean restrictToNonNegative;

        /// <summary>The variables each column represents */
        private List<String> columnLabels = new List<String>();

        /// <summary>Simple tableaud */
        [NonSerialized]
        private Matrix<Double> tableau;

        /// <summary>Number of decision variablesd */
        private int numDecisionVariables;

        /// <summary>Number of slack variablesd */
        private int numSlackVariables;

        /// <summary>Number of artificial variablesd */
        private int numArtificialVariables;

        /// <summary>Amount of error to accept in floating point comparisonsd */
        private double epsilon;

        /// <summary>
        /// Build a tableau for a linear problem.
        /// </summary>
        /// <param Name="f">linear objective function</param>
        /// <param Name="constraints">linear constraints</param>
        /// <param Name="goalType">type of optimization goal: either {@link GoalType#MAXIMIZE}</param>
        /// or {@link GoalType#MINIMIZE}
        /// <param Name="restrictToNonNegative">whether to restrict the variables to non-negative values</param>
        /// <param Name="epsilon">amount of error to accept in floating point comparisons</param>
        public SimplexTableau(LinearObjectiveFunction f, ICollection<LinearConstraint> constraints, GoalType goalType, Boolean restrictToNonNegative, double epsilon)
        {
            this.f = f;
            this.constraints = NormalizeConstraints(constraints);
            this.restrictToNonNegative = restrictToNonNegative;
            this.epsilon = epsilon;
            this.numDecisionVariables = f.Coefficients.Count +
                                          (restrictToNonNegative ? 0 : 1);
            this.numSlackVariables = getConstraintTypeCounts(Relationship.LEQ) +
                                          getConstraintTypeCounts(Relationship.GEQ);
            this.numArtificialVariables = getConstraintTypeCounts(Relationship.EQ) +
                                          getConstraintTypeCounts(Relationship.GEQ);
            this.tableau = createTableau(goalType == GoalType.MAXIMIZE);
            initializeColumnLabels();
        }

        /// <summary>
        /// Initialize the labels for the columns.
        /// </summary>
        protected void initializeColumnLabels()
        {
            if (NumObjectiveFunctions == 2)
            {
                columnLabels.Add("W");
            }
            columnLabels.Add("Z");
            for (int i = 0; i < OriginalNumDecisionVariables; i++)
            {
                columnLabels.Add("x" + i);
            }
            if (!restrictToNonNegative)
            {
                columnLabels.Add(NEGATIVE_VAR_COLUMN_LABEL);
            }
            for (int i = 0; i < NumSlackVariables; i++)
            {
                columnLabels.Add("s" + i);
            }
            for (int i = 0; i < NumArtificialVariables; i++)
            {
                columnLabels.Add("a" + i);
            }
            columnLabels.Add("RHS");
        }

        /// <summary>
        /// Create the tableau by itself.
        /// </summary>
        /// <param Name="maximize">if true, goal is to maximize the objective function</param>
        /// <returns>created tableau</returns>
        protected Matrix<Double> createTableau(Boolean maximize)
        {

            // create a matrix of the correct size
            int width = numDecisionVariables + numSlackVariables +
            numArtificialVariables + NumObjectiveFunctions + 1; // + 1 is for RHS
            int height = constraints.Count + NumObjectiveFunctions;
            var matrix = MatrixUtility.CreateMatrix<Double>(height, width);

            // initialize the objective function rows
            if (NumObjectiveFunctions == 2)
            {
                matrix[0, 0] = -1;
            }
            int zIndex = (NumObjectiveFunctions == 1) ? 0 : 1;
            matrix[zIndex, zIndex] = maximize ? 1 : -1;
            Vector<Double> objectiveCoefficients =
                maximize ? f.Coefficients.Multiply(-1) : f.Coefficients;
            //copyArray(objectiveCoefficients.Data, matrix.getDataRef()[zIndex]);
            matrix.SetRow(zIndex, objectiveCoefficients.AsArrayEx());
            matrix[zIndex, width - 1] = maximize ? f.ConstantTerm : -1 * f.ConstantTerm;

            if (!restrictToNonNegative)
            {
                matrix[zIndex, SlackVariableOffset - 1] = getInvertedCoeffiecientSum(objectiveCoefficients);
            }

            // initialize the constraint rows
            int slackVar = 0;
            int artificialVar = 0;
            for (int i = 0; i < constraints.Count; i++)
            {
                LinearConstraint constraint = constraints[i];
                int row = NumObjectiveFunctions + i;

                // decision variable coefficients
                //copyArray(constraint.Coefficients.AsArrayEx(), matrix.GetRow[row]);
                matrix.SetRow(row, constraint.Coefficients.AsArrayEx());
                // x-
                if (!restrictToNonNegative)
                {
                    matrix[row, SlackVariableOffset - 1] = getInvertedCoeffiecientSum(constraint.Coefficients);
                }

                // RHS
                matrix[row, width - 1] = constraint.Value;

                // slack variables
                if (constraint.Relationship == Relationship.LEQ)
                {
                    matrix[row, SlackVariableOffset + slackVar++] = 1;  // slack
                }
                else if (constraint.Relationship == Relationship.GEQ)
                {
                    matrix[row, SlackVariableOffset + slackVar++] = -1; // excess
                }

                // artificial variables
                if ((constraint.Relationship == Relationship.EQ) ||
                        (constraint.Relationship == Relationship.GEQ))
                {
                    matrix[0, ArtificialVariableOffset + artificialVar] = 1;
                    matrix[row, ArtificialVariableOffset + artificialVar++] = 1;
                    matrix.SetRow(0, matrix.GetRow(0).Subtract(matrix.GetRow(row)));
                }
            }

            return matrix;
        }

        /// <summary>
        /// Get new versions of the constraints which have positive right hand sides.
        /// </summary>
        /// <param Name="originalConstraints">original (not normalized) constraints</param>
        /// <returns>new versions of the constraints</returns>
        public List<LinearConstraint> NormalizeConstraints(ICollection<LinearConstraint> originalConstraints)
        {
            List<LinearConstraint> normalized = new List<LinearConstraint>();
            foreach (LinearConstraint constraint in originalConstraints)
            {
                normalized.Add(Normalize(constraint));
            }
            return normalized;
        }

        /// <summary>
        /// Get a new equation equivalent to this one with a positive right hand side.
        /// </summary>
        /// <param Name="constraint">reference constraint</param>
        /// <returns>new equation</returns>
        private LinearConstraint Normalize(LinearConstraint constraint)
        {
            if (constraint.Value < 0)
            {
                return new LinearConstraint(constraint.Coefficients.Multiply(-1),
                                            constraint.Relationship.OppositeRelationship(),
                                            -1 * constraint.Value);
            }
            return new LinearConstraint(constraint.Coefficients,
                                        constraint.Relationship, constraint.Value);
        }

        /// <summary>
        /// Get the number of objective functions in this tableau.
        /// </summary>
        /// <returns>2 for Phase 1d  1 for Phase 2d</returns>
        public int NumObjectiveFunctions
        {
            get { return this.numArtificialVariables > 0 ? 2 : 1; }
        }

        /// <summary>
        /// Get a count of constraints corresponding to a specified relationship.
        /// </summary>
        /// <param Name="relationship">relationship to count</param>
        /// <returns>number of constraint with the specified relationship</returns>
        private int getConstraintTypeCounts(Relationship relationship)
        {
            int count = 0;
            foreach (LinearConstraint constraint in constraints)
            {
                if (constraint.Relationship == relationship)
                {
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Get the -1 times the sum of all coefficients in the given array.
        /// </summary>
        /// <param Name="coefficients">coefficients to sum</param>
        /// <returns>the -1 times the sum of all coefficients in the given array.</returns>
        protected static double getInvertedCoeffiecientSum(Vector<Double> coefficients)
        {
            double sum = 0;
            foreach (double coefficient in coefficients.AsArrayEx())
            {
                sum -= coefficient;
            }
            return sum;
        }

        /// <summary>
        /// Checks whether the given column is basic.
        /// </summary>
        /// <param Name="col">index of the column to check</param>
        /// <returns>the row that the variable is basic ind  null if the column is not basic</returns>
        public Int32? GetBasicRow(int col)
        {
            Int32? row = null;
            for (int i = 0; i < Height; i++)
            {
                if (GetEntry(i, col).AlmostEquals(1.0, epsilon) && (row == null))
                {
                    row = i;
                }
                else if (!GetEntry(i, col).AlmostEquals(0.0, epsilon))
                {
                    return null;
                }
            }
            return row;
        }

        /// <summary>
        /// Removes the phase 1 objective function, positive cost non-artificial variables,
        /// and the non-basic artificial variables from this tableau.
        /// </summary>
        public void DropPhase1Objective()
        {
            if (NumObjectiveFunctions == 1)
            {
                return;
            }

            List<Int32> columnsToDrop = new List<Int32>();
            columnsToDrop.Add(0);

            // positive cost non-artificial variables
            for (int i = NumObjectiveFunctions; i < ArtificialVariableOffset; i++)
            {
                if (Math2.CompareTo(tableau[0, i], 0, epsilon) > 0)
                {
                    columnsToDrop.Add(i);
                }
            }

            // non-basic artificial variables
            for (int i = 0; i < NumArtificialVariables; i++)
            {
                int col = i + ArtificialVariableOffset;
                if (GetBasicRow(col) == null)
                {
                    columnsToDrop.Add(col);
                }
            }

            double[][] matrix = ArrayUtility.CreateJaggedArray<double>(Height - 1, Width - columnsToDrop.Count);
            for (int i = 1; i < Height; i++)
            {
                int col = 0;
                for (int j = 0; j < Width; j++)
                {
                    if (!columnsToDrop.Contains(j))
                    {
                        matrix[i - 1][col++] = tableau[i, j];
                    }
                }
            }

            for (int i = columnsToDrop.Count - 1; i >= 0; i--)
            {
                columnLabels.RemoveAt((int)columnsToDrop[i]);
            }

            this.tableau = MatrixUtility.CreateMatrix<Double>(matrix);
            this.numArtificialVariables = 0;
        }

        /// <summary>
        /// </summary>
        /// <param Name="src">the source array</param>
        /// <param Name="dest">the destination array</param>
        private void copyArray(double[] src, double[] dest)
        {
            Array.Copy(src, 0, dest, NumObjectiveFunctions, src.Length);
        }

        /// <summary>
        /// Returns whether the problem is at an optimal state.
        /// </summary>
        /// <returns>whether the model has been solved</returns>
        public Boolean IsOptimal()
        {
            for (int i = NumObjectiveFunctions; i < Width - 1; i++)
            {
                if (Math2.CompareTo(tableau[0, i], 0, epsilon) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the current solution.
        /// 
        /// </summary>
        /// <returns>current solution</returns>
        public RealPointValuePair GetSolution()
        {
            int negativeVarColumn = columnLabels.IndexOf(NEGATIVE_VAR_COLUMN_LABEL);
            Int32? negativeVarBasicRow = negativeVarColumn > 0 ? GetBasicRow(negativeVarColumn) : null;
            double mostNegative = negativeVarBasicRow == null ? 0 : GetEntry(negativeVarBasicRow.Value, RhsOffset);

            HashSet<Int32> basicRows = new HashSet<Int32>();
            double[] coefficients = new double[OriginalNumDecisionVariables];
            for (int i = 0; i < coefficients.Length; i++)
            {
                int colIndex = columnLabels.IndexOf("x" + i);
                if (colIndex < 0)
                {
                    coefficients[i] = 0;
                    continue;
                }
                Int32? basicRow = GetBasicRow(colIndex);
                if (basicRows.Contains(basicRow.Value))
                {
                    // if multiple variables can take a given value
                    // then we choose the first and set the rest equal to 0
                    coefficients[i] = 0;
                }
                else
                {
                    basicRows.Add(basicRow.Value);
                    coefficients[i] =
                        (basicRow == null ? 0 : GetEntry(basicRow.Value, RhsOffset)) -
                        (restrictToNonNegative ? 0 : mostNegative);
                }
            }
            return new RealPointValuePair(coefficients, f.GetValue(coefficients));
        }

        /// <summary>
        /// Subtracts a multiple of one row from another.
        /// <p>
        /// After application of this operation, the following will hold:
        ///   minuendRow = minuendRow - multiple * subtrahendRow
        /// </p>
        /// </summary>
        /// <param Name="dividendRow">index of the row</param>
        /// <param Name="divisor">value of the divisor</param>
        public void DivideRow(int dividendRow, double divisor)
        {
            for (int j = 0; j < Width; j++)
            {
                tableau[dividendRow, j] = tableau[dividendRow, j] / divisor;
            }
        }

        /// <summary>
        /// Subtracts a multiple of one row from another.
        /// <p>
        /// After application of this operation, the following will hold:
        ///   minuendRow = minuendRow - multiple * subtrahendRow
        /// </p>
        /// </summary>
        /// <param Name="minuendRow">row index</param>
        /// <param Name="subtrahendRow">row index</param>
        /// <param Name="multiple">multiplication factor</param>
        public void SubtractRow(int minuendRow, int subtrahendRow, double multiple)
        {
            tableau.SetRow(minuendRow, tableau.GetRow(minuendRow)
                .Subtract(tableau.GetRow(subtrahendRow).Multiply(multiple)));
        }

        /// <summary>
        /// Get the width of the tableau.
        /// </summary>
        /// <returns>width of the tableau</returns>
        public int Width
        {
            get { return tableau.ColumnCount; }
        }

        /// <summary>
        /// Get the height of the tableau.
        /// </summary>
        /// <returns>height of the tableau</returns>
        public int Height
        {
            get { return tableau.RowCount; }
        }

        /// <summary>Get an entry of the tableau.
        /// </summary>
        /// <param Name="row">row index</param>
        /// <param Name="column">column index</param>
        /// <returns>entry at (row, column)</returns>
        public double GetEntry(int row, int column)
        {
            return tableau[row, column];
        }

        /// <summary>Set an entry of the tableau.
        /// </summary>
        /// <param Name="row">row index</param>
        /// <param Name="column">column index</param>
        /// <param Name="value">for the entry</param>
        public void SetEntry(int row, int column, double value)
        {
            tableau[row, column] = value;
        }

        /// <summary>
        /// Get the offset of the first slack variable.
        /// </summary>
        /// <returns>offset of the first slack variable</returns>
        public int SlackVariableOffset
        {
            get { return NumObjectiveFunctions + numDecisionVariables; }
        }

        /// <summary>
        /// Get the offset of the first artificial variable.
        /// </summary>
        /// <returns>offset of the first artificial variable</returns>
        public int ArtificialVariableOffset
        {
            get { return NumObjectiveFunctions + numDecisionVariables + numSlackVariables; }
        }

        /// <summary>
        /// Get the offset of the right hand side.
        /// </summary>
        /// <returns>offset of the right hand side</returns>
        public int RhsOffset
        {
            get { return Width - 1; }
        }

        /// <summary>
        /// Get the number of decision variables.
        /// <p>
        /// If variables are not restricted to positive values, this will include 1
        /// extra decision variable to represent the absolute value of the most
        /// negative variable.
        /// </p>
        /// </summary>
        /// <returns>number of decision variables</returns>
        /// <see cref="#getOriginalNumDecisionVariables()"></see>
        public int NumDecisionVariables
        {
            get { return numDecisionVariables; }
        }

        /// <summary>
        /// Get the original number of decision variables.
        /// </summary>
        /// <returns>original number of decision variables</returns>
        /// <see cref="#getNumDecisionVariables()"></see>
        public int OriginalNumDecisionVariables
        {
            get { return f.Coefficients.Count; }
        }

        /// <summary>
        /// Get the number of slack variables.
        /// </summary>
        /// <returns>number of slack variables</returns>
        public int NumSlackVariables
        {
            get { return numSlackVariables; }
        }

        /// <summary>
        /// Get the number of artificial variables.
        /// </summary>
        /// <returns>number of artificial variables</returns>
        public int NumArtificialVariables
        {
            get { return numArtificialVariables; }
        }

        /// <summary>
        /// Get the tableau data.
        /// </summary>
        /// <returns>tableau data</returns>
        protected double[][] Data
        {
            get { return tableau.AsArrayEx().ToJagged(); }
        }
    }
}
