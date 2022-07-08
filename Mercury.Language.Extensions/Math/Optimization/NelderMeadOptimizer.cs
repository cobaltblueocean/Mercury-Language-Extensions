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

namespace Mercury.Language.Math.Optimization
{
    public class NelderMeadOptimizer : BaseMultivariateMultiStartOptimizer<IMultivariateRealFunction>, IMultivariateOptimizer
    {

        #region Local Variables
        /** Reflection coefficientd */
        private double rho;

        /** Expansion coefficientd */
        private double khi;

        /** Contraction coefficientd */
        private double gamma;

        /** Shrinkage coefficientd */
        private double sigma;

        /** Build a Nelder-Mead optimizer with default coefficients.
         * <p>The default coefficients are 1.0 for rho, 2.0 for khi and 0.5
         * for both gamma and sigma.</p>
         */

        #endregion

        #region Property

        #endregion

        #region Constructor
        public NelderMeadOptimizer() : base(new DefaultOptimizer() { ReflectionCoefficient = 1.0, ExpansionCoefficient = 2.0, ContractionCoefficient = 0.5, ShrinkageCoefficient = 0.5 }, 1, new Random())
        {
            this.rho = 1.0;
            this.khi = 2.0;
            this.gamma = 0.5;
            this.sigma = 0.5;
        }

        public NelderMeadOptimizer(double rho, double khi, double gamma, double sigma) : base(new DefaultOptimizer() { ReflectionCoefficient = rho, ExpansionCoefficient = khi, ContractionCoefficient = gamma, ShrinkageCoefficient = sigma }, 1, new Random())
        {
            this.rho = rho;
            this.khi = khi;
            this.gamma = gamma;
            this.sigma = sigma;
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
            /** Reflection coefficient. */
            private double rho;
            /** Expansion coefficientd */
            private double khi;
            /** Contraction coefficientd */
            private double gamma;
            /** Shrinkage coefficient. */
            private double sigma;


            IConvergenceChecker<Tuple<double[], double>> checker = new SimpleScalarValueChecker();
            #endregion

            #region Property

            public double ReflectionCoefficient
            {
                get { return rho; }
                set { rho = value; }
            }

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

            public double ShrinkageCoefficient
            {
                get { return sigma; }
                set { sigma = value; }
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
            public Tuple<double[], double> Optimize(int maxEval, IMultivariateRealFunction f, GoalType goalType, double[] startPoint)
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

            protected double Evaluate(double[] x)
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
                        simplex[i] = new Tuple<Double[], Double>(point, Evaluate(point));
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
                IncrementIterationsCounter();

                // the simplex has n+1 point if dimension is n
                int n = simplex.Length - 1;

                // interesting values
                Tuple<Double[], Double> best = simplex[0];
                Tuple<Double[], Double> secondBest = simplex[n - 1];
                Tuple<Double[], Double> worst = simplex[n];
                double[] xWorst = worst.Item1;

                // compute the centroid of the best vertices
                // (dismissing the worst point at index n)
                double[] centroid = new double[n];
                for (int i = 0; i < n; ++i)
                {
                    double[] x = simplex[i].Item1;
                    for (int j = 0; j < n; ++j)
                    {
                        centroid[j] += x[j];
                    }
                }
                double scaling = 1.0 / n;
                for (int j = 0; j < n; ++j)
                {
                    centroid[j] *= scaling;
                }

                // compute the reflection point
                double[] xR = new double[n];
                for (int j = 0; j < n; ++j)
                {
                    xR[j] = centroid[j] + rho * (centroid[j] - xWorst[j]);
                }
                Tuple<Double[], Double> reflected = new Tuple<Double[], Double>(xR, Evaluate(xR));

                if ((comparator.Compare(best, reflected) <= 0) &&
                    (comparator.Compare(reflected, secondBest) < 0))
                {

                    // accept the reflected point
                    ReplaceWorstPoint(reflected, comparator);

                }
                else if (comparator.Compare(reflected, best) < 0)
                {

                    // compute the expansion point
                    double[] xE = new double[n];
                    for (int j = 0; j < n; ++j)
                    {
                        xE[j] = centroid[j] + khi * (xR[j] - centroid[j]);
                    }
                    Tuple<Double[], Double> expanded = new Tuple<Double[], Double>(xE, Evaluate(xE));

                    if (comparator.Compare(expanded, reflected) < 0)
                    {
                        // accept the expansion point
                        ReplaceWorstPoint(expanded, comparator);
                    }
                    else
                    {
                        // accept the reflected point
                        ReplaceWorstPoint(reflected, comparator);
                    }

                }
                else
                {

                    if (comparator.Compare(reflected, worst) < 0)
                    {

                        // perform an outside contraction
                        double[] xC = new double[n];
                        for (int j = 0; j < n; ++j)
                        {
                            xC[j] = centroid[j] + gamma * (xR[j] - centroid[j]);
                        }
                        Tuple<Double[], Double> outContracted = new Tuple<Double[], Double>(xC, Evaluate(xC));

                        if (comparator.Compare(outContracted, reflected) <= 0)
                        {
                            // accept the contraction point
                            ReplaceWorstPoint(outContracted, comparator);
                            return;
                        }

                    }
                    else
                    {

                        // perform an inside contraction
                        double[] xC = new double[n];
                        for (int j = 0; j < n; ++j)
                        {
                            xC[j] = centroid[j] - gamma * (centroid[j] - xWorst[j]);
                        }
                        Tuple<Double[], Double> inContracted = new Tuple<Double[], Double>(xC, Evaluate(xC));

                        if (comparator.Compare(inContracted, worst) < 0)
                        {
                            // accept the contraction point
                            ReplaceWorstPoint(inContracted, comparator);
                            return;
                        }

                    }

                    // perform a shrink
                    double[] xSmallest = simplex[0].Item1;
                    for (int i = 1; i < simplex.Length; ++i)
                    {
                        double[] x = simplex[i].Item1;
                        for (int j = 0; j < n; ++j)
                        {
                            x[j] = xSmallest[j] + sigma * (x[j] - xSmallest[j]);
                        }
                        simplex[i] = new Tuple<Double[], Double>(x, Double.NaN);
                    }
                    EvaluateSimplex(comparator);

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

            /// <summary>
            /// Replace the worst point of the simplex by a new point.
            /// </summary>
            /// <param name="pointValuePair">point to insert</param>
            /// <param name="comparator">comparator to use to sort simplex vertices from best to worst</param>
            protected void ReplaceWorstPoint(Tuple<Double[], Double> pointValuePair, IComparer<Tuple<Double[], Double>> comparator)
            {
                int n = simplex.Length - 1;
                for (int i = 0; i < n; ++i)
                {
                    if (comparator.Compare(simplex[i], pointValuePair) > 0)
                    {
                        Tuple<Double[], Double> tmp = simplex[i];
                        simplex[i] = pointValuePair;
                        pointValuePair = tmp;
                    }
                }
                simplex[n] = pointValuePair;
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
