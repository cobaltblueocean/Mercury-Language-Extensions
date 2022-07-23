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
using Mercury.Language.Math.Optimization;

namespace Mercury.Language.Math.Optimization.Direct
{
    /// <summary>
    /// This class  :  simplex-based direct search optimization
    /// algorithms.
    /// 
    /// <p>Direct search methods only use objective function values, they don't
    /// need derivatives and don't either try to compute approximation of
    /// the derivativesd According to a 1996 paper by Margaret Hd Wright
    /// (<a href="http://cm.bell-labs.com/cm/cs/doc/96/4-02dps.gz">Direct
    /// Search Methods: Once Scorned, Now Respectable</a>), they are used
    /// when either the computation of the derivative is impossible (noisy
    /// functions, unpredictable discontinuities) or difficult (complexity,
    /// computation cost)d In the first cases, rather than an optimum, a
    /// <em>not too bad</em> point is desiredd In the latter cases, an
    /// optimum is desired but cannot be reasonably foundd In all cases
    /// direct search methods can be useful.</p>
    /// 
    /// <p>Simplex-based direct search methods are based on comparison of
    /// the objective function values at the vertices of a simplex (which is a
    /// set of n+1 points in dimension n) that is updated by the algorithms
    /// steps.<p>
    /// 
    /// <p>The initial configuration of the simplex can be set using either
    /// {@link #setStartConfiguration(double[])} or {@link
    /// #setStartConfiguration(double[][])}d If neither method has been called
    /// before optimization is attempted, an explicit call to the first method
    /// with all steps set to +1 is triggered, thus building a default
    /// configuration from a unit hypercubed Each call to {@link
    /// #Optimize(IMultivariateRealFunction, GoalType, double[]) optimize} will reuse
    /// the current start configuration and move it such that its first vertex
    /// is at the provided start point of the optimizationd If the {@code optimize}
    /// method is called to solve a different problem and the number of parameters
    /// change, the start configuration will be reset to a default one with the
    /// appropriate dimensions.</p>
    /// 
    /// <p>If {@link #setConvergenceChecker(IRealConvergenceChecker)} is not called,
    /// a default {@link SimpleScalarValueChecker} is used.</p>
    /// 
    /// <p>Convergence is checked by providing the <em>worst</em> points of
    /// previous and current simplex to the convergence checker, not the best ones.</p>
    /// 
    /// <p>This class is the base class performing the boilerplate simplex
    /// initialization and handlingd The simplex update by itself is
    /// performed by the derived classes according to the implemented
    /// algorithms.</p>
    /// 
    ///  :  IMultivariateRealOptimizer since 2.0
    /// 
    /// </summary>
    /// <see cref="IMultivariateRealFunction"></see>
    /// <see cref="NelderMead"></see>
    /// <see cref="MultiDirectional"></see>
    /// @version $Revision: 1070725 $ $Date: 2011-02-15 02:31:12 +0100 (mard 15 févrd 2011) $
    /// @since 1.2
    public abstract class DirectSearchOptimizer : IMultivariateRealOptimizer
    {
        /// <summary>Simplexd */
        protected RealPointValuePair[] _simplex;

        /// <summary>Objective functiond */
        private IMultivariateRealFunction _f;

        /// <summary>Convergence checkerd */
        private IRealConvergenceChecker _checker;

        /// <summary>Maximal number of iterations allowedd */
        private int _maxIterations;

        /// <summary>Number of iterations already performedd */
        private int _iterations;

        /// <summary>Maximal number of evaluations allowedd */
        private int _maxEvaluations;

        /// <summary>Number of evaluations already performedd */
        private int _evaluations;

        /// <summary>Start simplex configurationd */
        private double[][] _startConfiguration;

        /// <summary>Found optimad */
        private RealPointValuePair[] _optima;

        /// <summary>{@inheritDoc} */
        public int MaxIterations
        {
            get { return _maxIterations; }
            set { this._maxIterations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Iterations
        {
            get { return _iterations; }
        }

        /// <summary>{@inheritDoc} */
        public int MaxEvaluations
        {
            get { return _maxEvaluations; }
            set { this._maxEvaluations = value; }
        }

        /// <summary>{@inheritDoc} */
        public int Evaluations
        {
            get { return _evaluations; }
        }

        /// <summary>{@inheritDoc} */
        public IRealConvergenceChecker ConvergenceChecker
        {
            get { return _checker; }
            set { _checker = value; }
        }


        /// <summary>Simple constructor.
        /// </summary>
        protected DirectSearchOptimizer()
        {
            ConvergenceChecker = (new SimpleScalarValueChecker());
            MaxIterations = (Int32.MaxValue);
            MaxEvaluations = (Int32.MaxValue);
        }

        /// <summary>Set start configuration for simplex.
        /// <p>The start configuration for simplex is built from a box parallel to
        /// the canonical axes of the spaced The simplex is the subset of vertices
        /// of a box parallel to the canonical axesd It is built as the path followed
        /// while traveling from one vertex of the box to the diagonally opposite
        /// vertex moving only along the box edgesd The first vertex of the box will
        /// be located at the start point of the optimization.</p>
        /// <p>As an example, in dimension 3 a simplex has 4 verticesd Setting the
        /// steps to (1, 10, 2) and the start point to (1, 1, 1) would imply the
        /// start simplex would be: { (1, 1, 1), (2, 1, 1), (2, 11, 1), (2, 11, 3) }.
        /// The first vertex would be set to the start point at (1, 1, 1) and the
        /// last vertex would be set to the diagonally opposite vertex at (2, 11, 3).</p>
        /// </summary>
        /// <param Name="steps">steps along the canonical axes representing box edges,</param>
        /// they may be negative but not null
        /// <exception cref="ArgumentException">if one step is null </exception>
        public void SetStartConfiguration(double[] steps)

        {
            // only the relative position of the n vertices with respect
            // to the first one are stored
            int n = steps.Length;
            _startConfiguration = ArrayUtility.CreateJaggedArray<double>(n, n);
            for (int i = 0; i < n; ++i)
            {
                double[] vertexI = _startConfiguration[i];
                for (int j = 0; j < i + 1; ++j)
                {
                    if (steps[j] == 0.0)
                    {
                        throw new ArgumentException(String.Format(LocalizedResources.Instance().EQUAL_VERTICES_IN_SIMPLEX, j, j + 1));
                    }
                    Array.Copy(steps, 0, vertexI, 0, j + 1);
                }
            }
        }


        /// <summary>Set start configuration for simplex.
        /// <p>The real initial simplex will be set up by moving the reference
        /// simplex such that its first point is located at the start point of the
        /// optimization.</p>
        /// </summary>
        /// <param Name="referenceSimplex">reference simplex</param>
        /// <exception cref="ArgumentException">if the reference simplex does not </exception>
        /// contain at least one point, or if there is a dimension mismatch
        /// in the reference simplex or if one of its vertices is duplicated
        public void SetStartConfiguration(double[][] referenceSimplex)
        {

            // only the relative position of the n vertices with respect
            // to the first one are stored
            int n = referenceSimplex.Length - 1;
            if (n < 0)
            {
                throw new ArgumentException(
                        LocalizedResources.Instance().SIMPLEX_NEED_ONE_POINT);
            }
            _startConfiguration = ArrayUtility.CreateJaggedArray<double>(n, n);
            double[] ref0 = referenceSimplex[0];

            // vertices loop
            for (int i = 0; i < n + 1; ++i)
            {

                double[] refI = referenceSimplex[i];

                // safety checks
                if (refI.Length != n)
                {
                    throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, refI.Length, n));
                }
                for (int j = 0; j < i; j++)
                {
                    double[] refJ = referenceSimplex[j];
                    Boolean allEquals = true;
                    for (int k = 0; k < n; ++k)
                    {
                        if (refI[k] != refJ[k])
                        {
                            allEquals = false;
                            break;
                        }
                    }
                    if (allEquals)
                    {
                        throw new ArgumentException(String.Format(LocalizedResources.Instance().EQUAL_VERTICES_IN_SIMPLEX, i, j));
                    }
                }

                // store vertex i position relative to vertex 0 position
                if (i > 0)
                {
                    double[] confI = _startConfiguration[i - 1];
                    for (int k = 0; k < n; ++k)
                    {
                        confI[k] = refI[k] - ref0[k];
                    }
                }

            }

        }


        /// <summary>{@inheritDoc} */
        public RealPointValuePair Optimize(IMultivariateRealFunction function, GoalType goalType, double[] startPoint)
        {

            if ((_startConfiguration == null) ||
                (_startConfiguration.Length != startPoint.Length))
            {
                // no initial configuration has been set up for simplex
                // build a default one from a unit hypercube
                double[] unit = new double[startPoint.Length];
                unit.Fill( 1.0);
                SetStartConfiguration(unit);
            }

            this._f = function;
            var comparator = new RealPointValuePairComparator(goalType);

            // initialize search
            _iterations = 0;
            _evaluations = 0;
            buildSimplex(startPoint);
            evaluateSimplex(comparator);

            RealPointValuePair[] previous = new RealPointValuePair[_simplex.Length];
            while (true)
            {

                if (_iterations > 0)
                {
                    Boolean converged = true;
                    for (int i = 0; i < _simplex.Length; ++i)
                    {
                        converged &= _checker.Converged(_iterations, previous[i], _simplex[i]);
                    }
                    if (converged)
                    {
                        // we have found an optimum
                        return _simplex[0];
                    }
                }

                // we still need to search
                Array.Copy(_simplex, 0, previous, 0, _simplex.Length);
                iterateSimplex(comparator);

            }

        }


        /// <summary>Increment the iterations counter by 1d
        /// </summary>
        /// <exception cref="OptimizationException">if the maximal number </exception>
        /// of iterations is exceeded
        protected void incrementIterationsCounter()

        {
            if (++_iterations > _maxIterations)
            {
                throw new OptimizationException(new IndexOutOfRangeException(_maxIterations.ToString()));
            }
        }

        /// <summary>Compute the next simplex of the algorithm.
        /// </summary>
        /// <param Name="comparator">comparator to use to sort simplex vertices from best to worst</param>
        /// <exception cref="FunctionEvaluationException">if the function cannot be evaluated at </exception>
        /// some point
        /// <exception cref="OptimizationException">if the algorithm fails to converge </exception>
        /// <exception cref="ArgumentException">if the start point dimension is wrong </exception>
        protected abstract void iterateSimplex(IComparer<RealPointValuePair> comparator)
                ;

        /// <summary>Evaluate the objective function on one point.
        /// <p>A side effect of this method is to count the number of
        /// function evaluations</p>
        /// </summary>
        /// <param Name="x">point on which the objective function should be evaluated</param>
        /// <returns>objective function value at the given point</returns>
        /// <exception cref="FunctionEvaluationException">if no value can be computed for the </exception>
        /// parameters or if the maximal number of evaluations is exceeded
        /// <exception cref="ArgumentException">if the start point dimension is wrong </exception>
        protected double Evaluate(double[] x)
        {
            if (++_evaluations > _maxEvaluations)
            {
                throw new FunctionEvaluationException(x, new MaxCountExceededException(_maxEvaluations));
            }
            return _f.Value(x);
        }

        /// <summary>Build an initial simplex.
        /// </summary>
        /// <param Name="startPoint">the start point for optimization</param>
        /// <exception cref="ArgumentException">if the start point does not match </exception>
        /// simplex dimension
        private void buildSimplex(double[] startPoint)

        {

            int n = startPoint.Length;
            if (n != _startConfiguration.Length)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, n, _startConfiguration.Length));
            }

            // set first vertex
            _simplex = new RealPointValuePair[n + 1];
            _simplex[0] = new RealPointValuePair(startPoint, Double.NaN);

            // set remaining vertices
            for (int i = 0; i < n; ++i)
            {
                double[] confI = _startConfiguration[i];
                double[] vertexI = new double[n];
                for (int k = 0; k < n; ++k)
                {
                    vertexI[k] = startPoint[k] + confI[k];
                }
                _simplex[i + 1] = new RealPointValuePair(vertexI, Double.NaN);
            }

        }

        /// <summary>Evaluate all the non-evaluated points of the simplex.
        /// </summary>
        /// <param Name="comparator">comparator to use to sort simplex vertices from best to worst</param>
        /// <exception cref="FunctionEvaluationException">if no value can be computed for the parameters </exception>
        /// <exception cref="OptimizationException">if the maximal number of evaluations is exceeded </exception>
        protected void evaluateSimplex(IComparer<RealPointValuePair> comparator)
        {

            // evaluate the objective function at all non-evaluated simplex points
            for (int i = 0; i < _simplex.Length; ++i)
            {
                RealPointValuePair vertex = _simplex[i];
                double[] point = vertex.PointRefrence;
                if (Double.IsNaN(vertex.Value))
                {
                    _simplex[i] = new RealPointValuePair(point, Evaluate(point), false);
                }
            }

            // sort the simplex from best to worst
            Array.Sort(_simplex, comparator);

        }

        /// <summary>Replace the worst point of the simplex by a new point.
        /// </summary>
        /// <param Name="pointValuePair">point to insert</param>
        /// <param Name="comparator">comparator to use to sort simplex vertices from best to worst</param>
        protected void ReplaceWorstPoint(RealPointValuePair pointValuePair, IComparer<RealPointValuePair> comparator)
        {
            int n = _simplex.Length - 1;
            for (int i = 0; i < n; ++i)
            {
                if (comparator.Compare(_simplex[i], pointValuePair) > 0)
                {
                    RealPointValuePair tmp = _simplex[i];
                    _simplex[i] = pointValuePair;
                    pointValuePair = tmp;
                }
            }
            _simplex[n] = pointValuePair;
        }
        private class RealPointValuePairComparator : IComparer<RealPointValuePair>
        {
            GoalType _goalType;

            public RealPointValuePairComparator(GoalType goalType)
            {
                _goalType = goalType;
            }

            public int Compare(RealPointValuePair o1, RealPointValuePair o2)
            {
                if (o1 == null)
                {
                    return (o2 == null) ? 0 : +1;
                }
                else if (o2 == null)
                {
                    return -1;
                }
                double v1 = o1.Value;
                double v2 = o2.Value;
                return (_goalType == GoalType.MINIMIZE) ?
                        v1.CompareTo(v2) : v2.CompareTo(v1);
            }
        }
    }
}
