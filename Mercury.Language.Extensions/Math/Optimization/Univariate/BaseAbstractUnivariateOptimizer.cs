// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Optimization.Univariate Inc.
//
// Copyright (C) 2012 - present by System.Math.Optimization.Univariate Incd and the System.Math.Optimization.Univariate group of companies
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

namespace Mercury.Language.Math.Optimization.Univariate
{
    /// <summary>
    /// Provide a default implementation for several functions useful to generic optimizers.
    /// </summary>
    public abstract class BaseAbstractUnivariateOptimizer : IUnivariateOptimizer
    {

        #region Local Variables
        /** Convergence checkerd */
        private IConvergenceChecker<UnivariatePointValuePair> checker;
        /** Evaluations counterd */
        private Incrementor evaluations = new Incrementor();
        /** Optimization type */
        private GoalType goal;
        /** Lower end of search intervald */
        private double searchMin;
        /** Higher end of search intervald */
        private double searchMax;
        /** Initial guess d */
        private double searchStart;
        /** Function to optimized */
        private IUnivariateRealFunction function;
        #endregion

        #region Property
        /// <summary>
        /// <see cref="IBaseOptimizer.MaxEvaluations"/> 
        /// </summary>
        public int MaxEvaluations
        {
            get { return evaluations.MaximalCount; }
            set { evaluations.MaximalCount = value; }
        }

        /// <summary>
        /// <see cref="IBaseOptimizer.Evaluations"/> 
        /// </summary>
        public int Evaluations
        {
            get { return evaluations.Count; }
        }

        /// <summary>
        /// <see cref="IBaseOptimizer.ConvergenceChecker"/> 
        /// </summary>
        public IConvergenceChecker<UnivariatePointValuePair> ConvergenceChecker
        {
            get { return checker; }
        }

        /// <summary>
        /// the optimization type.
        /// </summary>
        public GoalType GoalType
        {
            get { return goal; }
        }

        /// <summary>
        /// the lower end of the search interval.
        /// </summary>
        public double Min
        {
            get { return searchMin; }
        }

        /// <summary>
        /// the higher end of the search interval.
        /// </summary>
        public double Max
        {
            get { return searchMax; }
        }

        /// <summary>
        /// the initial guess.
        /// </summary>
        public double StartValue
        {
            get { return searchStart; }
        }
        #endregion

        #region Constructor
        protected BaseAbstractUnivariateOptimizer(IConvergenceChecker<UnivariatePointValuePair> checker)
        {
            this.checker = checker;
        }
        #endregion

        #region Implement Methods
        public UnivariatePointValuePair Optimize(int maxEval, IUnivariateRealFunction f, GoalType goalType, double min, double max)
        {
            return Optimize(maxEval, f, goalType, min, max, min + 0.5 * (max - min));
        }

        public UnivariatePointValuePair Optimize(int maxEval, IUnivariateRealFunction f, GoalType goalType, double min, double max, double startValue)
        {
            // Checks.
            if (f == null)
            {
                throw new ArgumentNullException();
            }
            //if (goalType == null)
            //{
            //    throw new ArgumentNullException();
            //}

            // Reset.
            searchMin = min;
            searchMax = max;
            searchStart = startValue;
            goal = goalType;
            function = f;
            evaluations.MaximalCount = maxEval;
            evaluations.ResetCount();

            // Perform computation.
            return doOptimize();
        }
        #endregion

        #region Local Public Methods
        protected double computeObjectiveValue(double point)
        {
            try
            {
                evaluations.IncrementCount();
            }
            catch (MaxCountExceededException e)
            {
                throw new MaxCountExceededException(e.Max);
            }
            return function.Value(point);
        }

        protected abstract UnivariatePointValuePair doOptimize();
        #endregion

        #region Local Private Methods

        #endregion
    }
}
