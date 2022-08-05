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
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exception;
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Language.Math.Matrix;
using Mercury.Language.Math.Optimization;
using MathNet.Numerics.LinearAlgebra;


namespace Mercury.Language.Math.Optimization.General
{
    /// <summary>
    /// Base class for implementing least squares optimizers.
    /// <p>This base class handles the boilerplate methods associated to thresholds
    /// settings, jacobian and error estimation.</p>
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    /// @since 1.2
    /// 
    /// </summary>
    public abstract class LeastSquaresOptimizer : IDifferentiableMultivariateVectorialOptimizer
    {

        /// <summary>Default maximal number of iterations allowedd */
        public static int DEFAULT_MAX_ITERATIONS = 100;

        /// <summary>Convergence checkerd */
        protected IVectorialConvergenceChecker checker;

        /// <summary>
        /// Jacobian matrix.
        /// <p>This matrix is in canonical form just after the calls to
        /// {@link #updateJacobian()}, but may be modified by the solver
        /// in the derived class (the {@link LevenbergMarquardtOptimizer
        /// Levenberg-Marquardt optimizer} does this).</p>
        /// </summary>
        protected double[][] jacobian;

        /// <summary>Number of columns of the jacobian matrixd */
        protected int cols;

        /// <summary>Number of rows of the jacobian matrixd */
        protected int rows;

        /// <summary>
        /// Target value for the objective functions at optimum.
        /// @since 2.1
        /// </summary>
        protected double[] targetValues;

        /// <summary>
        /// Weight for the least squares cost computation.
        /// @since 2.1
        /// </summary>
        protected double[] residualsWeights;

        /// <summary>Current pointd */
        protected double[] point;

        /// <summary>Current objective function valued */
        protected double[] objective;

        /// <summary>Current residualsd */
        protected double[] residuals;

        /// <summary>Weighted Jacobian */
        protected double[][] wjacobian;

        /// <summary>Weighted residuals */
        protected double[] wresiduals;

        /// <summary>Cost value (square root of the sum of the residuals)d */
        protected double cost;

        /// <summary>Maximal number of iterations allowedd */
        private int maxIterations;

        /// <summary>Number of iterations already performedd */
        private int iterations;

        /// <summary>Maximal number of evaluations allowedd */
        private int maxEvaluations;

        /// <summary>Number of evaluations already performedd */
        private int objectiveEvaluations;

        /// <summary>Number of jacobian evaluationsd */
        private int jacobianEvaluations;

        /// <summary>Objective functiond */
        private IDifferentiableMultivariateVectorialFunction function;

        /// <summary>Objective function derivativesd */
        private IMultivariateMatrixFunction jF;

        /// <summary>Simple constructor with default settings.
        /// <p>The convergence check is set to a {@link SimpleVectorialValueChecker}
        /// and the maximal number of evaluation is set to its default value.</p>
        /// </summary>
        protected LeastSquaresOptimizer()
        {
            ConvergenceChecker = (new SimpleVectorialValueChecker());
            MaxIterations = (DEFAULT_MAX_ITERATIONS);
            MaxEvaluations = (Int32.MaxValue);
        }

        /// <summary>{@inheritDoc} */
        public int MaxIterations
        {
            get { return maxIterations; }
            set { this.maxIterations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Iterations
        {
            get { return iterations; }
        }

        /// <summary>{@inheritDoc} */
        public int MaxEvaluations
        {
            get { return maxEvaluations; }
            set { this.maxEvaluations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Evaluations
        {
            get { return objectiveEvaluations; }
        }

        /// <summary>{@inheritDoc} */
        public int JacobianEvaluations
        {
            get { return jacobianEvaluations; }
        }

        /// <summary>{@inheritDoc} */
        public IVectorialConvergenceChecker ConvergenceChecker
        {
            get { return checker; }
            set { this.checker = value; }
        }

        /// <summary>Increment the iterations counter by 1d
        /// </summary>
        /// <exception cref="OptimizationException">if the maximal number </exception>
        /// of iterations is exceeded
        protected void incrementIterationsCounter()

        {
            if (++iterations > maxIterations)
            {
                throw new OptimizationException(new IndexOutOfRangeException(maxIterations.ToString()));
            }
        }

        /// <summary>
        /// Update the jacobian matrix.
        /// </summary>
        /// <exception cref="FunctionEvaluationException">if the function jacobian </exception>
        /// cannot be evaluated or its dimension doesn't match problem dimension
        protected void updateJacobian()
        {
            ++jacobianEvaluations;
            jacobian = jF.Value(point);
            if (jacobian.Length != rows)
            {
                throw new FunctionEvaluationException(point, LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE,
                                                      jacobian.Length, rows);
            }
            for (int i = 0; i < rows; i++)
            {
                double[] ji = jacobian[i];
                double wi = System.Math.Sqrt(residualsWeights[i]);
                for (int j = 0; j < cols; ++j)
                {
                    ji[j] *= -1.0;
                    wjacobian[i][j] = ji[j] * wi;
                }
            }
        }

        /// <summary>
        /// Update the residuals array and cost function value.
        /// </summary>
        /// <exception cref="FunctionEvaluationException">if the function cannot be evaluated </exception>
        /// or its dimension doesn't match problem dimension or maximal number of
        /// of evaluations is exceeded
        protected void updateResidualsAndCost()

        {

            if (++objectiveEvaluations > maxEvaluations)
            {
                throw new FunctionEvaluationException(point, new MaxCountExceededException(maxEvaluations));
            }
            objective = function.Value(point);
            if (objective.Length != rows)
            {
                throw new FunctionEvaluationException(point, LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE,
                                                      objective.Length, rows);
            }
            cost = 0;
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                double residual = targetValues[i] - objective[i];
                residuals[i] = residual;
                wresiduals[i] = residual * System.Math.Sqrt(residualsWeights[i]);
                cost += residualsWeights[i] * residual * residual;
                index += cols;
            }
            cost = System.Math.Sqrt(cost);

        }

        /// <summary>
        /// Get the Root Mean Square value.
        /// Get the Root Mean Square value, i.ed the root of the arithmetic
        /// mean of the square of all weighted residualsd This is related to the
        /// criterion that is minimized by the optimizer as follows: if
        /// <em>c</em> if the criterion, and <em>n</em> is the number of
        /// measurements, then the RMS is <em>sqrt (c/n)</em>.
        /// 
        /// </summary>
        /// <returns>RMS value</returns>
        public double getRMS()
        {
            return System.Math.Sqrt(ChiSquare / rows);
        }

        /// <summary>
        /// Get a Chi-Square-like value assuming the N residuals follow N
        /// distinct normal distributions centered on 0 and whose variances are
        /// the reciprocal of the weights.
        /// </summary>
        /// <returns>chi-square value</returns>
        public double ChiSquare
        {
            get { return cost * cost; }
        }

        /// <summary>
        /// Get the covariance matrix of optimized parameters.
        /// </summary>
        /// <returns>covariance matrix</returns>
        /// <exception cref="FunctionEvaluationException">if the function jacobian cannot </exception>
        /// be evaluated
        /// <exception cref="OptimizationException">if the covariance matrix </exception>
        /// cannot be computed (singular problem)
        public double[][] GetCovariances()
        {

            // set up the jacobian
            updateJacobian();

            // compute transpose(J).J, avoiding building big intermediate matrices
            double[][] jTj = ArrayUtility.CreateJaggedArray<double>(cols, cols);
            for (int i = 0; i < cols; ++i)
            {
                for (int j = i; j < cols; ++j)
                {
                    double sum = 0;
                    for (int k = 0; k < rows; ++k)
                    {
                        sum += wjacobian[k][i] * wjacobian[k][j];
                    }
                    jTj[i][j] = sum;
                    jTj[j][i] = sum;
                }
            }

            try
            {
                // compute the covariance matrix
                Matrix<Double> inverse = new LUDecomposition(MatrixUtility.CreateMatrix(jTj).AsArrayEx()).GetSolver().GetInverse();
                return inverse.AsArrayEx().ToJagged();
            }
            catch (InvalidMatrixException ime)
            {
                throw new OptimizationException(LocalizedResources.Instance().UNABLE_TO_COMPUTE_COVARIANCE_SINGULAR_PROBLEM, ime);
            }

        }

        /// <summary>
        /// Guess the errors in optimized parameters.
        /// <p>Guessing is covariance-based, it only gives rough order of magnitude.</p>
        /// </summary>
        /// <returns>errors in optimized parameters</returns>
        /// <exception cref="FunctionEvaluationException">if the function jacobian cannot b evaluated </exception>
        /// <exception cref="OptimizationException">if the covariances matrix cannot be computed </exception>
        /// or the number of degrees of freedom is not positive (number of measurements
        /// lesser or equal to number of parameters)
        public double[] guessParametersErrors()
        {
            if (rows <= cols)
            {
                throw new OptimizationException(
                        LocalizedResources.Instance().NO_DEGREES_OF_FREEDOM,
                        rows, cols);
            }
            double[] errors = new double[cols];
            double c = System.Math.Sqrt(ChiSquare / (rows - cols));
            double[][] covar = GetCovariances();
            for (int i = 0; i < errors.Length; ++i)
            {
                errors[i] = System.Math.Sqrt(covar[i][i]) * c;
            }
            return errors;
        }

        /// <summary>{@inheritDoc} */
        public VectorialPointValuePair Optimize(IDifferentiableMultivariateVectorialFunction f, double[] target, double[] weights, double[] startPoint)
        {

            if (target.Length != weights.Length)
            {
                throw new OptimizationException(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, target.Length, weights.Length);
            }

            // reset counters
            iterations = 0;
            objectiveEvaluations = 0;
            jacobianEvaluations = 0;

            // store least squares problem characteristics
            function = f;
            jF = f.Jacobian();
            targetValues = target.CloneExact();
            residualsWeights = weights.CloneExact();
            this.point = startPoint.CloneExact();
            this.residuals = new double[target.Length];

            // arrays shared with the other private methods
            rows = target.Length;
            cols = point.Length;
            jacobian = ArrayUtility.CreateJaggedArray<double>(rows, cols);

            wjacobian = ArrayUtility.CreateJaggedArray<double>(rows, cols);
            wresiduals = new double[rows];

            cost = Double.PositiveInfinity;

            return doOptimize();

        }

        /// <summary>Perform the bulk of optimization algorithm.
        /// </summary>
        /// <returns>the point/value pair giving the optimal value for objective function</returns>
        /// <exception cref="FunctionEvaluationException">if the objective function throws one during </exception>
        /// the search
        /// <exception cref="OptimizationException">if the algorithm failed to converge </exception>
        /// <exception cref="ArgumentException">if the start point dimension is wrong </exception>
        public abstract VectorialPointValuePair doOptimize();
    }
}
