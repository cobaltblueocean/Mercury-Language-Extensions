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
using Mercury.Language.Math.Analysis.Solver;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language.Extensions;
using MathNet.Numerics;

namespace Mercury.Language.Math.Optimization.General
{
    /// <summary>
    /// Non-linear conjugate gradient optimizer.
    /// <p>
    /// This class supports both the Fletcher-Reeves and the Polak-Ribi&egrave;re
    /// update formulas for the conjugate search directionsd It also supports
    /// optional preconditioning.
    /// </p>
    /// 
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 2.0
    /// 
    /// </summary>
    public class NonLinearConjugateGradientOptimizer : ScalarDifferentiableOptimizer
    {

        /// <summary>Update formula for the beta parameterd */
        private ConjugateGradientFormula updateFormula;

        /// <summary>Preconditioner (may be null)d */
        private IPreconditioner preconditioner;

        /// <summary>solver to use in the line search (may be null)d */
        private IUnivariateRealSolver<Double> solver;

        /// <summary>Initial step used to bracket the optimum in line searchd */
        private double initialStep;

        /// <summary>Simple constructor with default settings.
        /// <p>The convergence check is set to a {@link
        /// Mercury.Language.Math.Optimization.SimpleVectorialValueChecker}
        /// and the maximal number of iterations is set to
        /// {@link AbstractScalarDifferentiableOptimizer#DEFAULT_MAX_ITERATIONS}.
        /// </summary>
        /// <param Name="updateFormula">formula to use for updating the &beta; parameter,</param>
        /// must be one of {@link ConjugateGradientFormula#FLETCHER_REEVES} or {@link
        /// ConjugateGradientFormula#POLAK_RIBIERE}
        public NonLinearConjugateGradientOptimizer(ConjugateGradientFormula updateFormula)
        {
            this.updateFormula = updateFormula;
            preconditioner = null;
            solver = null;
            initialStep = 1.0;
        }

        /// <summary>
        /// Set the preconditioner.
        /// </summary>
        /// <param Name="preconditioner">preconditioner to use for next optimization,</param>
        /// may be null to remove an already registered preconditioner
        public void SetPreconditioner(IPreconditioner preconditioner)
        {
            this.preconditioner = preconditioner;
        }

        /// <summary>
        /// Set the solver to use during line search.
        /// </summary>
        /// <param Name="lineSearchSolver">solver to use during line search, may be null</param>
        /// to remove an already registered solver and fall back to the
        /// default {@link BrentSolver Brent solver}.
        public void SetLineSearchSolver(IUnivariateRealSolver<Double> lineSearchSolver)
        {
            this.solver = lineSearchSolver;
        }

        /// <summary>
        /// Set the initial step used to bracket the optimum in line search.
        /// <p>
        /// The initial step is a factor with respect to the search direction,
        /// which itself is roughly related to the gradient of the function
        /// </p>
        /// </summary>
        /// <param Name="initialStep">initial step used to bracket the optimum in line search,</param>
        /// if a non-positive value is used, the initial step is reset to its
        /// default value of 1.0
        public void setInitialStep(double initialStep)
        {
            if (initialStep <= 0)
            {
                this.initialStep = 1.0;
            }
            else
            {
                this.initialStep = initialStep;
            }
        }

        /// <summary>{@inheritDoc} */
        //@Override
        protected override RealPointValuePair doOptimize()
        {
            try
            {

                // initialization
                if (preconditioner == null)
                {
                    preconditioner = new IdentityPreconditioner();
                }
                if (solver == null)
                {
                    solver = new BrentSolver();
                }
                int n = point.Length;
                double[] r = computeObjectiveGradient(point);
                if (goal == GoalType.MINIMIZE)
                {
                    for (int i = 0; i < n; ++i)
                    {
                        r[i] = -r[i];
                    }
                }

                // initial search direction
                double[] steepestDescent = preconditioner.Precondition(point, r);
                double[] searchDirection = steepestDescent.CloneExact();

                double delta = 0;
                for (int i = 0; i < n; ++i)
                {
                    delta += r[i] * searchDirection[i];
                }

                RealPointValuePair current = null;
                while (true)
                {

                    double objective = computeObjectiveValue(point);
                    RealPointValuePair previous = current;
                    current = new RealPointValuePair(point, objective);
                    if (previous != null)
                    {
                        if (checker.Converged(Iterations, previous, current))
                        {
                            // we have found an optimum
                            return current;
                        }
                    }

                    incrementIterationsCounter();

                    double dTd = 0;
                    AutoParallel.AutoParallelForEach(searchDirection, (di) =>
                    {
                        dTd += di * di;
                    });

                    // find the optimal step in the search direction
                    IUnivariateRealFunction lsf = new LineSearchFunction(this, searchDirection);
                    double step = solver.Solve(lsf, 0, findUpperBound(lsf, 0, initialStep));

                    // validate new point
                    for (int i = 0; i < point.Length; ++i)
                    {
                        point[i] += step * searchDirection[i];
                    }
                    r = computeObjectiveGradient(point);
                    if (goal == GoalType.MINIMIZE)
                    {
                        for (int i = 0; i < n; ++i)
                        {
                            r[i] = -r[i];
                        }
                    }

                    // compute beta
                    double deltaOld = delta;
                    double[] newSteepestDescent = preconditioner.Precondition(point, r);
                    delta = 0;
                    for (int i = 0; i < n; ++i)
                    {
                        delta += r[i] * newSteepestDescent[i];
                    }

                    double beta;
                    if (updateFormula == ConjugateGradientFormula.FLETCHER_REEVES)
                    {
                        beta = delta / deltaOld;
                    }
                    else
                    {
                        double deltaMid = 0;
                        for (int i = 0; i < r.Length; ++i)
                        {
                            deltaMid += r[i] * steepestDescent[i];
                        }
                        beta = (delta - deltaMid) / deltaOld;
                    }
                    steepestDescent = newSteepestDescent;

                    // compute conjugate search direction
                    if ((Iterations % n == 0) || (beta < 0))
                    {
                        // break conjugation: reset search direction
                        searchDirection = steepestDescent.CloneExact();
                    }
                    else
                    {
                        // compute new conjugate search direction
                        for (int i = 0; i < n; ++i)
                        {
                            searchDirection[i] = steepestDescent[i] + beta * searchDirection[i];
                        }
                    }

                }

            }
            catch (NonConvergenceException ce)
            {
                throw new OptimizationException(ce);
            }
        }

        /// <summary>
        /// Find the upper bound b ensuring bracketing of a root between a and b
        /// </summary>
        /// <param Name="f">function whose root must be bracketed</param>
        /// <param Name="a">lower bound of the interval</param>
        /// <param Name="h">initial step to try</param>
        /// <returns>b such that f(a) and f(b) have opposite signs</returns>
        /// <exception cref="FunctionEvaluationException">if the function cannot be computed </exception>
        /// <exception cref="OptimizationException">if no bracket can be found </exception>
        private double findUpperBound(IUnivariateRealFunction f, double a, double h)
        {
            double yA = f.Value(a);
            double yB = yA;
            for (double step = h; step < Double.MaxValue; step *= System.Math.Max(2, yA / yB))
            {
                double b = a + step;
                yB = f.Value(b);
                if (yA * yB <= 0)
                {
                    return b;
                }
            }
            throw new OptimizationException(LocalizedResources.Instance().UNABLE_TO_BRACKET_OPTIMUM_IN_LINE_SEARCH);
        }

        /// <summary>Default identity preconditionerd */
        private class IdentityPreconditioner : IPreconditioner
        {

            /// <summary>{@inheritDoc} */
            public double[] Precondition(double[] variables, double[] r)
            {
                return r.CloneExact();
            }

        }

        /// <summary>Internal class for line search.
        /// <p>
        /// The function represented by this class is the dot product of
        /// the objective function gradient and the search directiond Its
        /// value is zero when the gradient is orthogonal to the search
        /// direction, i.ed when the objective function value is a local
        /// extremum along the search direction.
        /// </p>
        /// </summary>
        private class LineSearchFunction : IUnivariateRealFunction
        {
            NonLinearConjugateGradientOptimizer _base;

            /// <summary>Search directiond */
            private double[] searchDirection;

            /// <summary>Simple constructor.
            /// </summary>
            /// <param Name="searchDirection">search direction</param>
            public LineSearchFunction(NonLinearConjugateGradientOptimizer baseOptimizer, double[] searchDirection)
            {
                _base = baseOptimizer;
                this.searchDirection = searchDirection;
            }

            public double ParamValue => throw new NotImplementedException();

            /// <summary>
            /// {@inheritDoc} 
            /// </summary>
            public double Value(double x)
            {

                // current point in the search direction
                double[]
                shiftedPoint = _base.point.CloneExact();
                for (int i = 0; i < shiftedPoint.Length; ++i)
                {
                    shiftedPoint[i] += x * searchDirection[i];
                }

                // gradient of the objective function
                double[]
                gradient;
                gradient = _base.computeObjectiveGradient(shiftedPoint);

                // dot product with the search direction
                double dotProduct = 0;
                for (int i = 0; i < gradient.Length; ++i)
                {
                    dotProduct += gradient[i] * searchDirection[i];
                }
                return dotProduct;
            }
        }
    }
}
