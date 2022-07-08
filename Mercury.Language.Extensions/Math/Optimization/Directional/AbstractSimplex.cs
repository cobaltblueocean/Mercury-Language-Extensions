// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization.Directional Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization.Directional Incd and the System.Math.Optimization.Directional group of companies
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

namespace Mercury.Language.Math.Optimization.Directional
{
    /// <summary>
    /// This class implements the simplex concept.
    /// It is intended to be used in conjunction with {@link SimplexOptimizer}.
    /// <br/>
    /// The initial configuration of the simplex is set by the constructors
    /// {@link #AbstractSimplex(double[])} or {@link #AbstractSimplex(double[][])}.
    /// The other {@link #AbstractSimplex(int) constructor} will set all steps
    /// to 1, thus building a default configuration from a unit hypercube.
    /// <br/>
    /// Users <em>must</em> call the {@link #build(double[]) build} method in order
    /// to create the data structure that will be acted on by the other methods of
    /// this classd    /// 
    /// </summary>
    public abstract class AbstractSimplex
    {

        #region Local Variables
        private Tuple<double[], double>[] simplex;
        private double[,] startConfiguration;
        private int dimension;
        #endregion

        #region Property
        public int Dimension
        {
            get { return dimension; }
        }

        public int Length
        {
            get { return simplex.Length; }
        }

        /// <summary>
        /// Gets/sets the points of the simplex.
        /// 
        /// <summary>
        /// <returns>all the simplex points.</returns>

        public Tuple<Double[], Double>[] Points
        {
            get
            {
                return simplex;
            }
            set
            {
                if (value.Length != simplex.Length)
                {
                    throw new DimensionMismatchException(value.Length, simplex.Length);
                }
                simplex = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Build a unit hypercube simplex.
        /// 
        /// <summary>
        /// <param name="n">Dimension of the simplex.</param>
        protected AbstractSimplex(int n) : this(n, 1d)
        {

        }

        /// <summary>
        /// Build a hypercube simplex with the given side Length.
        /// 
        /// <summary>
        /// <param name="n">Dimension of the simplex.</param>
        /// <param name="sideLength">Length of the sides of the hypercube.</param>
        protected AbstractSimplex(int n, double sideLength) : this(CreateHypercubeSteps(n, sideLength))
        {

        }

        protected AbstractSimplex(double[] steps)
        {
            if (steps == null)
            {
                throw new NullReferenceException();
            }
            if (steps.Length == 0)
            {
                throw new ZeroOperationException();
            }
            dimension = steps.Length;

            // Only the relative position of the n vertices with respect
            // to the first one are stored.
            startConfiguration = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                double[] vertexI = startConfiguration.GetRow(i);
                for (int j = 0; j < i + 1; j++)
                {
                    if (steps[j] == 0)
                    {
                        throw new ZeroOperationException(LocalizedResources.Instance().EQUAL_VERTICES_IN_SIMPLEX);
                    }
                    Array.Copy(steps, 0, vertexI, 0, j + 1);
                }
            }
        }

        protected AbstractSimplex(double[,] referenceSimplex)
        {
            if (referenceSimplex.Length <= 0)
            {
                throw new NotStrictlyPositiveException(LocalizedResources.Instance().SIMPLEX_NEED_ONE_POINT,
                                                       referenceSimplex.Length);
            }
            dimension = referenceSimplex.Length - 1;

            // Only the relative position of the n vertices with respect
            // to the first one are stored.
            startConfiguration = new double[dimension, dimension];
            double[] ref0 = referenceSimplex.GetRow(0);

            // Loop over vertices.
            for (int i = 0; i < referenceSimplex.Length; i++)
            {
                double[] refI = referenceSimplex.GetRow(i);

                // Safety checks.
                if (refI.Length != dimension)
                {
                    throw new DimensionMismatchException(refI.Length, dimension);
                }
                for (int j = 0; j < i; j++)
                {
                    double[] refJ = referenceSimplex.GetRow(j);
                    Boolean allEquals = true;
                    for (int k = 0; k < dimension; k++)
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

                // Store vertex i position relative to vertex 0 position.
                if (i > 0)
                {
                    double[] confI = startConfiguration.GetRow(i - 1);
                    for (int k = 0; k < dimension; k++)
                    {
                        confI[k] = refI[k] - ref0[k];
                    }
                }
            }
        }

        #endregion

        #region Abstract Methods
        public abstract void Iterate(IMultivariateRealFunction evaluationFunction, IComparer<Tuple<Double[], double>> comparator);

        #endregion

        #region Local Public Methods
        public void Build(double[] startPoint)
        {
            if (dimension != startPoint.Length)
            {
                throw new DimensionMismatchException(dimension, startPoint.Length);
            }

            // Set first vertex.
            simplex = new Tuple<Double[], Double>[dimension + 1];
            simplex[0] = new Tuple<Double[], Double>(startPoint, Double.NaN);

            // Set remaining vertices.
            for (int i = 0; i < dimension; i++)
            {
                double[] confI = startConfiguration.GetRow(i);
                double[] vertexI = new double[dimension];
                for (int k = 0; k < dimension; k++)
                {
                    vertexI[k] = startPoint[k] + confI[k];
                }
                simplex[i + 1] = new Tuple<Double[], Double>(vertexI, Double.NaN);
            }
        }

        public void Evaluate(IMultivariateRealFunction evaluationFunction, IComparer<Tuple<Double[], Double>> comparator)
        {
            // Evaluate the objective function at all non-evaluated simplex points.
            for (int i = 0; i < simplex.Length; i++)
            {
                Tuple<Double[], Double> vertex = simplex[i];
                double[] point = vertex.Item1;
                if (Double.IsNaN(vertex.Item2))
                {
                    simplex[i] = new Tuple<Double[], Double>(point, evaluationFunction.Value(point));
                }
            }

            // Sort the simplex from best to worst.
            Array.Sort(simplex, comparator);
        }

        protected void ReplaceWorstPoint(Tuple<Double[], Double> pointValuePair, IComparer<Tuple<Double[], Double>> comparator)
        {
            for (int i = 0; i < dimension; i++)
            {
                if (comparator.Compare(simplex[i], pointValuePair) > 0)
                {
                    Tuple<Double[], Double> tmp = simplex[i];
                    simplex[i] = pointValuePair;
                    pointValuePair = tmp;
                }
            }
            simplex[dimension] = pointValuePair;
        }

        /// <summary>
        /// Get the simplex point stored at the requested {@code index}.
        /// 
        /// <summary>
        /// <param name="index">Location.</param>
        /// <returns>the point at location {@code index}.</returns>
        public Tuple<Double[], Double> GetPoint(int index)
        {
            if (index < 0 ||
                index >= simplex.Length)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, index, 0, simplex.Length - 1));
            }
            return simplex[index];
        }

        /// <summary>
        /// Store a new point at location {@code index}.
        /// Note that no deep-copy of {@code point} is performed.
        /// 
        /// <summary>
        /// <param name="index">Location.</param>
        /// <param name="point">New value.</param>
        protected void SetPoint(int index, Tuple<Double[], Double> point)
        {
            if (index < 0 ||
                index >= simplex.Length)
            {
                throw new IndexOutOfRangeException(String.Format(LocalizedResources.Instance().INDEX_OUT_OF_RANGE, index, 0, simplex.Length - 1));
            }
            simplex[index] = point;
        }


        #endregion

        #region Local Private Methods

        /// <summary>
        /// Create steps for a unit hypercube.
        /// 
        /// <summary>
        /// <param name="n">Dimension of the hypercube.</param>
        /// <param name="sideLength">Length of the sides of the hypercube.</param>
        /// <returns>the steps.</returns>
        private static double[] CreateHypercubeSteps(int n, double sideLength)
        {
            double[] steps = new double[n];
            for (int i = 0; i < n; i++)
            {
                steps[i] = sideLength;
            }
            return steps;
        }
        #endregion
    }
}
