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
//
//-------------------------------------------------------------------------------------
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreementsd  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the Licensed  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Language.Math.Analysis.Solver;
using Mercury.Language.Math.Matrix;

namespace Mercury.Language.Math.Optimization.General
{

    /// <summary>
    /// Gauss-Newton least-squares solver.
    /// <p>
    /// This class solve a least-square problem by solving the normal equations
    /// of the linearized problem at each iterationd Either LU decomposition or
    /// QR decomposition can be used to solve the normal equationsd LU decomposition
    /// is faster but QR decomposition is more robust for difficult problems.
    /// </p>
    /// 
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    /// @since 2.0
    /// 
    /// </summary>
    public class GaussNewtonOptimizer : LeastSquaresOptimizer
    {

        /// <summary>Indicator for using LU decompositiond */
        private Boolean useLU;

        /// <summary>Simple constructor with default settings.
        /// <p>The convergence check is set to a {@link
        /// Mercury.Language.Math.Optimization.SimpleVectorialValueChecker}
        /// and the maximal number of evaluation is set to
        /// {@link AbstractLeastSquaresOptimizer#DEFAULT_MAX_ITERATIONS}.
        /// </summary>
        /// <param Name="useLU">if true, the normal equations will be solved using LU</param>
        /// decomposition, otherwise they will be solved using QR decomposition
        public GaussNewtonOptimizer(Boolean useLU)
        {
            this.useLU = useLU;
        }

        /// <summary>{@inheritDoc} */
        //@Override
        public override VectorialPointValuePair doOptimize()
        {

            // iterate until convergence is reached
            VectorialPointValuePair current = null;

            Boolean converged = false;

            while (!converged)
            //foreach (Boolean converged = false; !converged;) {
            {
                incrementIterationsCounter();

                // evaluate the objective function and its jacobian
                VectorialPointValuePair previous = current;
                updateResidualsAndCost();
                updateJacobian();
                current = new VectorialPointValuePair(point, objective);

                // build the linear problem
                double[] b = new double[cols];
                double[][] a = ArrayUtility.CreateJaggedArray<double>(cols, cols);
                for (int i = 0; i < rows; ++i)
                {

                    double[] grad = jacobian[i];
                    double weight = residualsWeights[i];
                    double residual = objective[i] - targetValues[i];

                    // compute the normal equation
                    double wr = weight * residual;
                    for (int j = 0; j < cols; ++j)
                    {
                        b[j] += wr * grad[j];
                    }

                    // build the contribution matrix for measurement i
                    for (int k = 0; k < cols; ++k)
                    {
                        double[] ak = a[k];
                        double wgk = weight * grad[k];
                        for (int l = 0; l < cols; ++l)
                        {
                            ak[l] += wgk * grad[l];
                        }
                    }
                }
                try
                {
                    // solve the linearized least squares problem
                    Matrix<Double> mA = MathNetMatrixUtility.CreateMatrix(a);
                    IDecompositionSolver solver = useLU ?
                            new LUDecomposition(mA.AsArrayEx()).GetSolver() :
                            new QRDecomposition(mA.AsArrayEx()).GetSolver();
                    double[] dX = solver.Solve(b);

                    // update the estimated parameters
                    for (int i = 0; i < cols; ++i)
                    {
                        point[i] += dX[i];
                    }

                }
                catch (InvalidMatrixException e)
                {
                    throw new OptimizationException(LocalizedResources.Instance().UNABLE_TO_SOLVE_SINGULAR_PROBLEM, e);
                }

                // check convergence
                if (previous != null)
                {
                    converged = checker.Converged(Iterations, previous, current);
                }
            }

            // we have converged
            return current;
        }
    }
}
