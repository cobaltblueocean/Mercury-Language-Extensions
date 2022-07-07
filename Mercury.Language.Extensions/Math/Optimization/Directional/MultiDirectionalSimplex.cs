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
    /// MultiDirectionalSimplex Description
    /// </summary>
    public class MultiDirectionalSimplex : AbstractSimplex
    {

        #region Local Variables
        /// <summary>
        /// Default value for {@link #khi}: {@value}.
        /// </summary>
        private static double DEFAULT_KHI = 2;

        /// <summary>
        /// Default value for {@link #gamma}: {@value}.
        /// </summary>
        private static double DEFAULT_GAMMA = 0.5;

        /// <summary>
        /// Expansion coefficient.
        /// </summary>
        private double khi;

        /// <summary>
        /// Contraction coefficient.
        /// </summary>
        private double gamma;
        #endregion

        #region Property

        #endregion

        #region Constructor
        public MultiDirectionalSimplex(int n) : this(n, 1d)
        {

        }

        public MultiDirectionalSimplex(int n, double sideLength) : this(n, sideLength, DEFAULT_KHI, DEFAULT_GAMMA)
        {

        }

        public MultiDirectionalSimplex(int n, double khi, double gamma) : this(n, 1d, khi, gamma)
        {

        }

        public MultiDirectionalSimplex(int n, double sideLength, double khi, double gamma) : base(n, sideLength)
        {
            this.khi = khi;
            this.gamma = gamma;
        }

        public MultiDirectionalSimplex(double[] steps) : this(steps, DEFAULT_KHI, DEFAULT_GAMMA)
        {

        }

        public MultiDirectionalSimplex(double[] steps, double khi, double gamma) : base(steps)
        {


            this.khi = khi;
            this.gamma = gamma;
        }

        public MultiDirectionalSimplex(double[,] referenceSimplex) : this(referenceSimplex, DEFAULT_KHI, DEFAULT_GAMMA)
        {

        }

        public MultiDirectionalSimplex(double[,] referenceSimplex, double khi, double gamma) : base(referenceSimplex)
        {


            this.khi = khi;
            this.gamma = gamma;
        }

        #endregion

        #region Implement Methods
        public override void Iterate(IMultivariateFunction evaluationFunction, IComparer<Tuple<double[], double>> comparator)
        {
            // Save the original simplex.
            Tuple<double[], double>[] original = base.Points;
            Tuple<double[], double> best = original[0];

            // Perform a reflection step.
            Tuple<double[], double> reflected = EvaluateNewSimplex(evaluationFunction,
                                                                    original, 1, comparator);
            if (comparator.Compare(reflected, best) < 0)
            {
                // Compute the expanded simplex.
                Tuple<double[], double>[] reflectedSimplex = base.Points;
                Tuple<double[], double> expanded = EvaluateNewSimplex(evaluationFunction,
                                                                       original, khi, comparator);
                if (comparator.Compare(reflected, expanded) <= 0)
                {
                    // Keep the reflected simplex.
                    Points = reflectedSimplex;
                }
                // Keep the expanded simplex.
                return;
            }

            // Compute the contracted simplex.
            EvaluateNewSimplex(evaluationFunction, original, gamma, comparator);
        }

        #endregion

        #region Local Public Methods

        #endregion

        #region Local Private Methods
        private Tuple<double[], double> EvaluateNewSimplex(IMultivariateFunction evaluationFunction,
                                              Tuple<double[], double>[] original,
                                              double coeff,
                                              IComparer<Tuple<double[], double>> comparator)
        {
            double[] xSmallest = original[0].Item1;
            // Perform a linear transformation on all the simplex points,
            // except the first one.
            SetPoint(0, original[0]);
            int dim = Dimension;
            for (int i = 1; i < Length; i++)
            {
                double[] xOriginal = original[i].Item1;
                double[] xTransformed = new double[dim];
                for (int j = 0; j < dim; j++)
                {
                    xTransformed[j] = xSmallest[j] + coeff * (xSmallest[j] - xOriginal[j]);
                }
                SetPoint(i, new Tuple<double[], double>(xTransformed, Double.NaN));
            }

            // Evaluate the simplex.
            Evaluate(evaluationFunction, comparator);

            return GetPoint(0);
        }
        #endregion
    }
}
