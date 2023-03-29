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
using Mercury.Language.Math.Optimization;
using Mercury.Language.Exceptions;

namespace Mercury.Language.Math.Optimization.Linear
{
    /// <summary>
    /// Solves a linear problem using the Two-Phase Simplex Method.
    /// @version $Revision: 812831 $ $Date: 2009-09-09 10:48:03 +0200 (merd 09 septd 2009) $
    /// @since 2.0
    /// </summary>
    public class SimplexSolver : BaseLinearOptimizer
    {

        /// <summary>Default amount of error to accept in floating point comparisonsd */
        private static double DEFAULT_EPSILON = 1.0e-6;

        /// <summary>Amount of error to accept in floating point comparisonsd */
        protected double epsilon;

        /// <summary>
        /// Build a simplex solver with default settings.
        /// </summary>
        public SimplexSolver() : this(DEFAULT_EPSILON)
        {

        }

        /// <summary>
        /// Build a simplex solver with a specified accepted amount of error
        /// </summary>
        /// <param Name="epsilon">the amount of error to accept in floating point comparisons</param>
        public SimplexSolver(double epsilon)
        {
            this.epsilon = epsilon;
        }

        /// <summary>
        /// Returns the column with the most negative coefficient in the objective function row.
        /// </summary>
        /// <param Name="tableau">simple tableau for the problem</param>
        /// <returns>column with the most negative coefficient</returns>
        private Int32 getPivotColumn(SimplexTableau tableau)
        {
            double minValue = 0;
            Int32? minPos = null;
            for (int i = tableau.NumObjectiveFunctions; i < tableau.Width - 1; i++)
            {
                if (Math2.CompareTo(tableau.GetEntry(0, i), minValue, epsilon) < 0)
                {
                    minValue = tableau.GetEntry(0, i);
                    minPos = i;
                }
            }
            return minPos.Value;
        }

        /// <summary>
        /// Returns the row with the minimum ratio as given by the minimum ratio test (MRT).
        /// </summary>
        /// <param Name="tableau">simple tableau for the problem</param>
        /// <param Name="col">the column to test the ratio ofd  See {@link #getPivotColumn(SimplexTableau)}</param>
        /// <returns>row with the minimum ratio</returns>
        private Int32? getPivotRow(SimplexTableau tableau, int col)
        {
            // create a list of all the rows that tie for the lowest score in the minimum ratio test
            List<Int32> minRatioPositions = new List<Int32>();
            double minRatio = Double.MaxValue;
            for (int i = tableau.NumObjectiveFunctions; i < tableau.Height; i++)
            {
                double rhs = tableau.GetEntry(i, tableau.Width - 1);
                double entry = tableau.GetEntry(i, col);
                if (Math2.CompareTo(entry, 0, epsilon) > 0)
                {
                    double ratio = rhs / entry;
                    if (ratio.AlmostEquals(minRatio, epsilon))
                    {
                        minRatioPositions.Add(i);
                    }
                    else if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        minRatioPositions = new List<Int32>();
                        minRatioPositions.Add(i);
                    }
                }
            }

            if (minRatioPositions.Count == 0)
            {
                return null;
            }
            else if (minRatioPositions.Count > 1)
            {
                // there's a degeneracy as indicated by a tie in the minimum ratio test
                // check if there's an artificial variable that can be forced out of the basis
                foreach (Int32 row in minRatioPositions)
                {
                    for (int i = 0; i < tableau.NumArtificialVariables; i++)
                    {
                        int column = i + tableau.ArtificialVariableOffset;
                        if (tableau.GetEntry(row, column).AlmostEquals(1, epsilon) &&
                            row.Equals(tableau.GetBasicRow(column)))
                        {
                            return row;
                        }
                    }
                }
            }
            return minRatioPositions[0];
        }

        /// <summary>
        /// Runs one iteration of the Simplex method on the given model.
        /// </summary>
        /// <param Name="tableau">simple tableau for the problem</param>
        /// <exception cref="OptimizationException">if the maximal iteration count has been </exception>
        /// exceeded or if the model is found not to have a bounded solution
        protected void doIteration(SimplexTableau tableau)

        {

            incrementIterationsCounter();

            Int32? pivotCol = getPivotColumn(tableau);
            Int32? pivotRow = getPivotRow(tableau, pivotCol.Value);
            if (pivotRow == null)
            {
                throw new UnboundedSolutionException();
            }

            // set the pivot element to 1
            double pivotVal = tableau.GetEntry(pivotRow.Value, pivotCol.Value);
            tableau.DivideRow(pivotRow.Value, pivotVal);

            // set the rest of the pivot column to 0
            for (int i = 0; i < tableau.Height; i++)
            {
                if (i != pivotRow)
                {
                    double multiplier = tableau.GetEntry(i, pivotCol.Value);
                    tableau.SubtractRow(i, pivotRow.Value, multiplier);
                }
            }
        }

        /// <summary>
        /// Solves Phase 1 of the Simplex method.
        /// </summary>
        /// <param Name="tableau">simple tableau for the problem</param>
        /// <exception cref="OptimizationException">if the maximal number of iterations is </exception>
        /// exceeded, or if the problem is found not to have a bounded solution, or
        /// if there is no feasible solution
        protected void solvePhase1(SimplexTableau tableau)
        {

            // make sure we're in Phase 1
            if (tableau.NumArtificialVariables == 0)
            {
                return;
            }

            while (!tableau.IsOptimal())
            {
                doIteration(tableau);
            }

            // if W is not zero then we have no feasible solution
            if (!tableau.GetEntry(0, tableau.RhsOffset).AlmostEquals( 0, epsilon))
            {
                throw new NoFeasibleSolutionException();
            }
        }

        /// <summary>{@inheritDoc} */
        //@Override
        protected override RealPointValuePair doOptimize()
        {
            SimplexTableau tableau = new SimplexTableau(function, linearConstraints, goal, nonNegative, epsilon);

            solvePhase1(tableau);
            tableau.DropPhase1Objective();

            while (!tableau.IsOptimal())
            {
                doIteration(tableau);
            }
            return tableau.GetSolution();
        }
    }
}
