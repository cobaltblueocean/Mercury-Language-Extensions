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
    /// <summary>
    /// BaseMultivariateMultiStartOptimizer Description
    /// </summary>
    public class BaseMultivariateMultiStartOptimizer<F> : IBaseMultivariateOptimizer<F> where F : IMultivariateRealFunction
    {

        #region Local Variables
        /** Underlying classical optimizerd */
        private  IBaseMultivariateOptimizer<F> optimizer;
        /** Maximal number of evaluations allowedd */
        private int maxEvaluations;
        /** Number of evaluations already performed for all startsd */
        private int totalEvaluations;
        /** Number of starts to god */
        private int starts;
        /** Random generator for multi-startd */
        private Random generator;
        /** Found optimad */
        private Tuple<Double[], Double>[] optima;

        #endregion

        #region Property
        public IConvergenceChecker<Tuple<double[], double>> ConvergenceChecker
        {
            get
            {
                return optimizer.ConvergenceChecker;
            }
        }

        public int Evaluations
        {
            get
            {
                return optimizer.Evaluations;
            }
        }

        public int MaxEvaluations
        {
            get
            {
                return optimizer.MaxEvaluations;
            }

            set
            {
                optimizer.MaxEvaluations = value;
            }
        }
        #endregion

        #region Constructor

        protected BaseMultivariateMultiStartOptimizer(IBaseMultivariateOptimizer<F> optimizer,
                                                   int starts,
                                                   Random generator)
        {
            if (optimizer == null ||
                generator == null)
            {
                throw new ArgumentNullException();
            }
            if (starts < 1)
            {
                throw new NotStrictlyPositiveException(starts);
            }

            this.optimizer = optimizer;
            this.starts = starts;
            this.generator = generator;
        }

        #endregion

        #region Implement Methods


        public Tuple<double[], double> Optimize(int maxEval, F f, GoalType goalType, double[] startPoint)
        {
            maxEvaluations = maxEval;
            SystemException lastException = null;
            optima = new Tuple<double[], double>[starts];
            totalEvaluations = 0;

            // Multi-start loop.
            for (int i = 0; i < starts; ++i)
            {
                // CHECKSTYLE: stop IllegalCatch
                try
                {
                    optima[i] = optimizer.Optimize(maxEval - totalEvaluations, f, goalType, i == 0 ? startPoint : generator.NextVector(startPoint.Length));
                }
                catch (SystemException mue)
                {
                    lastException = mue;
                    optima[i] = null;
                }
                // CHECKSTYLE: resume IllegalCatch

                totalEvaluations += optimizer.Evaluations;
            }

            sortPairs(goalType);

            if (optima[0] == null)
            {
                throw lastException; // cannot be null if starts >=1
            }

            // Return the found point given the best objective function value.
            return optima[0];        }
        #endregion

        #region Local Public Methods
        public Tuple<Double[], Double>[] GetOptima()
        {
            if (optima == null)
            {
                throw new MathIllegalNumberException(LocalizedResources.Instance().NO_OPTIMUM_COMPUTED_YET);
            }
            return optima.Copy();
        }
        #endregion

        #region Local Private Methods
        private void sortPairs(GoalType goal)
        {
            Array.Sort(optima, new PointValuePairComparer(goal));
    }
        #endregion

        private class PointValuePairComparer : IComparer<Tuple<Double[], Double>>
        {
            private GoalType _goal;

            public PointValuePairComparer(GoalType goal)
            {
                _goal = goal;
            }

            public int Compare(Tuple<double[], double> o1, Tuple<double[], double> o2)
            {
                if (o1 == null)
                {
                    return (o2 == null) ? 0 : 1;
                }
                else if (o2 == null)
                {
                    return -1;
                }
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
