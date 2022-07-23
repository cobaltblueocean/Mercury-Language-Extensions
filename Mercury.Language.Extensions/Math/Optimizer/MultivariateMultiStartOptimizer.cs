// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization Incd and the System.Math.Optimization group of companies
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
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimizer
{
    /// <summary>
    /// MultivariateMultiStartOptimizer Description
    /// </summary>
    public class MultivariateMultiStartOptimizer : BaseMultivariateMultiStartOptimizer<IMultivariateRealFunction>, IMultivariateOptimizer
    {
        #region Local Variables

        #endregion

        #region Property

        #endregion

        #region Constructor
        public MultivariateMultiStartOptimizer() : base(new DefaultOptimizer(), 1, new System.Random())
        {
        }

        public MultivariateMultiStartOptimizer(IBaseMultivariateOptimizer<IMultivariateRealFunction> optimizer, int starts, System.Random generator) : base(optimizer, starts, generator)
        {
        }

        #endregion

        #region Implement Methods

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods

        #endregion

        private class DefaultOptimizer : IBaseMultivariateOptimizer<IMultivariateRealFunction>
        {

            #region Local Variables
            private Tuple<double[], double>[] simplex;

            private IMultivariateRealFunction f;
            /** Maximal number of evaluations allowedd */
            private int maxEvaluations;
            /** Number of evaluations already performed for all startsd */
            private int totalEvaluations;
            /** Number of starts to god */
            //private int starts = 1;
            /** Found optimad */
            //private Tuple<Double[], Double>[] optima;
            /** Start simplex configurationd */
            private double[,] startConfiguration;
            /** Expansion coefficient. */
            private double khi;
            /** Contraction coefficient. */
            private double gamma;

            IConvergenceChecker<Tuple<double[], double>> checker = new SimpleScalarValueChecker();
            #endregion

            #region Property
            public double ExpansionCoefficient
            {
                get { return khi; }
                set { khi = value; }
            }

            public double ContractionCoefficient
            {
                get { return gamma; }
                set { gamma = value; }
            }

            public IConvergenceChecker<Tuple<double[], double>> ConvergenceChecker
            {
                get
                {
                    return checker;
                }
            }

            public int Evaluations
            {
                get
                {
                    return totalEvaluations;
                }
            }

            public int MaxEvaluations
            {
                get
                {
                    return maxEvaluations;
                }

                set
                {
                    maxEvaluations = value;
                }
            }
            #endregion

            #region Constructor

            public DefaultOptimizer()
            {
                this.khi = 2.0;
                this.gamma = 0.5;
            }

            #endregion

            #region Implement Methods
            public Tuple<double[], double> Optimize(IMultivariateRealFunction f, GoalType goalType, double[] startPoint)
            {
                if ((startConfiguration == null) || (startConfiguration.Length != startPoint.Length))
                {
                    // no initial configuration has been set up for simplex
                    // build a default one from a unit hypercube
                    double[] unit = new double[startPoint.Length];
                    unit.Fill(1.0);
                    var config = new Double[1, unit.Length];
                    config.LoadRow(0, unit);
                    SetStartConfiguration(config);
                }

                this.f = f;

                IComparer<Tuple<Double[], Double>> comparator = new RealPointValuePairComparer(goalType);

                // initialize search
                totalEvaluations = 0;
                maxEvaluations = 0;
                BuildSimplex(startPoint);
                EvaluateSimplex(comparator);

                Tuple<Double[], Double>[] previous = new Tuple<Double[], Double>[simplex.Length];
                while (true)
                {

                    if (totalEvaluations > 0)
                    {
                        Boolean converged = true;
                        for (int i = 0; i < simplex.Length; ++i)
                        {
                            converged &= checker.Converged(totalEvaluations, previous[i], simplex[i]);
                        }
                        if (converged)
                        {
                            // we have found an optimum
                            return simplex[0];
                        }
                    }

                    // we still need to search
                    Array.Copy(simplex, 0, previous, 0, simplex.Length);
                    IterateSimplex(comparator);
                }
            }

            protected double evaluate(double[] x)
            {
                if (++totalEvaluations > maxEvaluations)
                {
                    throw new FunctionEvaluationException(x, new MaxCountExceededException(maxEvaluations));
                }
                return f.Value(x);
            }

            #endregion

            #region Local Public Methods
            public void SetStartConfiguration(double[,] referenceSimplex)
            {
                // only the relative position of the n vertices with respect
                // to the first one are stored
                int n = referenceSimplex.Length - 1;
                if (n < 0)
                {
                    throw new MathArithmeticException(LocalizedResources.Instance().SIMPLEX_NEED_ONE_POINT);
                }
                startConfiguration = new double[n, n];
                double[] ref0 = referenceSimplex.GetRow(0);

                // vertices loop
                for (int i = 0; i < n + 1; ++i)
                {

                    double[] refI = referenceSimplex.GetRow(i);

                    // safety checks
                    if (refI.Length != n)
                    {
                        throw new MathArithmeticException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, refI.Length, n));
                    }
                    for (int j = 0; j < i; ++j)
                    {
                        double[] refJ = referenceSimplex.GetRow(j);
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
                            throw new MathArithmeticException(String.Format(LocalizedResources.Instance().EQUAL_VERTICES_IN_SIMPLEX, i, j));
                        }
                    }

                    // store vertex i position relative to vertex 0 position
                    if (i > 0)
                    {
                        double[] confI = startConfiguration.GetRow(i - 1);
                        for (int k = 0; k < n; ++k)
                        {
                            confI[k] = refI[k] - ref0[k];
                        }
                    }
                }
            }

            protected void EvaluateSimplex(IComparer<Tuple<Double[], Double>> comparator)
            {

                // evaluate the objective function at all non-evaluated simplex points
                for (int i = 0; i < simplex.Length; ++i)
                {
                    Tuple<Double[], Double> vertex = simplex[i];
                    double[] point = vertex.Item1;
                    if (Double.IsNaN(vertex.Item2))
                    {
                        simplex[i] = new Tuple<Double[], Double>(point, evaluate(point));
                    }
                }

                // sort the simplex from best to worst
                Array.Sort(simplex, comparator);
            }

            protected void IncrementIterationsCounter()
            {
                if (++totalEvaluations > maxEvaluations)
                {
                    throw new OptimizationException(new IndexOutOfRangeException(maxEvaluations.ToString()));
                }
            }

            #endregion

            #region Local Private Methods
            private void IterateSimplex(IComparer<Tuple<Double[], Double>> comparator)
            {
                IConvergenceChecker<Tuple<Double[], Double>> checker = ConvergenceChecker;
                while (true)
                {

                    IncrementIterationsCounter();

                    // save the original vertex
                    Tuple<Double[], Double>[] original = simplex;
                    Tuple<Double[], Double> best = original[0];

                    // perform a reflection step
                    Tuple<Double[], Double> reflected = EvaluateNewSimplex(original, 1.0, comparator);
                    if (comparator.Compare(reflected, best) < 0)
                    {

                        // compute the expanded simplex
                        Tuple<Double[], Double>[] reflectedSimplex = simplex;
                        Tuple<Double[], Double> expanded = EvaluateNewSimplex(original, khi, comparator);
                        if (comparator.Compare(reflected, expanded) <= 0)
                        {
                            // accept the reflected simplex
                            simplex = reflectedSimplex;
                        }

                        return;

                    }

                    // compute the contracted simplex
                    Tuple<Double[], Double> contracted = EvaluateNewSimplex(original, gamma, comparator);
                    if (comparator.Compare(contracted, best) < 0)
                    {
                        // accept the contracted simplex
                        return;
                    }

                    // check convergence
                    int iter = totalEvaluations;
                    Boolean converged = true;
                    for (int i = 0; i < simplex.Length; ++i)
                    {
                        converged &= checker.Converged(iter, original[i], simplex[i]);
                    }
                    if (converged)
                    {
                        return;
                    }

                }
            }

            private Tuple<Double[], Double> EvaluateNewSimplex(Tuple<Double[], Double>[] original, double coeff, IComparer<Tuple<Double[], Double>> comparator)
            {

                double[] xSmallest = original[0].Item1;
                int n = xSmallest.Length;

                // create the linearly transformed simplex
                simplex = new Tuple<Double[], Double>[n + 1];
                simplex[0] = original[0];
                for (int i = 1; i <= n; ++i)
                {
                    double[] xOriginal = original[i].Item1;
                    double[] xTransformed = new double[n];
                    for (int j = 0; j < n; ++j)
                    {
                        xTransformed[j] = xSmallest[j] + coeff * (xSmallest[j] - xOriginal[j]);
                    }
                    simplex[i] = new Tuple<Double[], Double>(xTransformed, Double.NaN);
                }

                // evaluate it
                EvaluateSimplex(comparator);
                return simplex[0];

            }

            private void BuildSimplex(double[] startPoint)
            {
                int n = startPoint.Length;
                if (n != startConfiguration.Length)
                {
                    throw new MathArithmeticException(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, n, startConfiguration.Length));
                }

                // set first vertex
                simplex = new Tuple<Double[], Double>[n + 1];
                simplex[0] = new Tuple<Double[], Double>(startPoint, Double.NaN);

                // set remaining vertices
                for (int i = 0; i < n; ++i)
                {
                    double[] confI = startConfiguration.GetRow(i);
                    double[] vertexI = new double[n];
                    for (int k = 0; k < n; ++k)
                    {
                        vertexI[k] = startPoint[k] + confI[k];
                    }
                    simplex[i + 1] = new Tuple<Double[], Double>(vertexI, Double.NaN);
                }
            }

            #endregion

            private class RealPointValuePairComparer : IComparer<Tuple<Double[], Double>>
            {
                private GoalType _goal;

                public RealPointValuePairComparer(GoalType goal)
                {
                    _goal = goal;
                }

                public int Compare(Tuple<double[], double> o1, Tuple<double[], double> o2)
                {
                    double v1 = o1.Item2;
                    double v2 = o2.Item2;
                    return (_goal == GoalType.MINIMIZE) ?
                            Compare(v1, v2) : Compare(v2, v1);
                }
                private int Compare(Double v1, Double v2)
                {
                    if (v1 == v2)
                        return 0;
                    if (v1 < v2)
                        return -1;

                    return 1;
                }

            }
        }

    }
}


