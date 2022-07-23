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
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Optimizer.Univariate
{
    /// <summary>
    /// Provide a default implementation for several functions useful to generic optimizers.
    /// </summary>
    public abstract class BaseAbstractUnivariateOptimizer : IUnivariateOptimizer
    {

        #region Local Variables
        private int DEFAULT_MAXIMAL_ITERATION_COUNT = 100;
        private int DEFAULT_MAX_EVALUATIONS = 1000;
        private double DEFAULT_ABSOLUTE_ACCURACY = 1e-11;
        private double DEFAULT_RELATIVE_ACCURACY = 1e-9;

        ///// <summary>
        ///// Convergence checkerd 
        ///// </summary>
        private IConvergenceChecker<double> checker;
        /// <summary>
        /// Evaluations counterd
        /// </summary>
        private Incrementor evaluations = new Incrementor();
        /// <summary>Optimization type /// </summary>
        private GoalType goal;
        /// <summary>
        /// Maximal iteration count for Solve() method 
        /// </summary>
        private int MaximalIterationCount;
        /// <summary>
        /// Lower end of search intervald 
        /// </summary>
        private double searchMin;
        /// <summary>
        /// Higher end of search intervald 
        /// </summary>
        private double searchMax;
        /// <summary>
        /// Initial guess d
        /// </summary>
        private double searchStart;
        /// <summary>
        /// Function to optimized
        /// </summary>
        private IUnivariateRealFunction function;
        /// <summary>
        /// Value of the function at the last computed result.
        /// </summary>
        protected double functionValue;
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
        public IConvergenceChecker<double> ConvergenceChecker
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

        public int MaxIterationCount
        {
            get { return MaximalIterationCount; }
            set { MaximalIterationCount = value; }
        }

        public double FunctionValue
        {
            get { return functionValue; }
            protected set { functionValue = value; }
        }

        #endregion

        #region Constructor
        protected BaseAbstractUnivariateOptimizer(IConvergenceChecker<double> checker)
        {
            evaluations.MaximalCount = DEFAULT_MAX_EVALUATIONS;
            MaximalIterationCount = DEFAULT_MAXIMAL_ITERATION_COUNT;

            this.checker = checker;
        }
        #endregion

        #region Implement Methods
        public double Optimize(IUnivariateRealFunction f, GoalType goalType, double min, double max)
        {
            return Optimize(f, goalType, min, max, min + 0.5 * (max - min));
        }

        public double Optimize(IUnivariateRealFunction f, GoalType goalType, double min, double max, double startValue)
        {
            // Checks.
            if (f == null)
            {
                throw new ArgumentNullException();
            }

            // Reset.
            searchMin = min;
            searchMax = max;
            searchStart = startValue;
            goal = goalType;
            function = f;
            evaluations.ResetCount();

            // Perform computation.
            return doOptimize();
        }
        #endregion

        #region Local Public Methods
        protected double computeObjectiveValue(double point)
        {
            incrementIterationsCounter();
            return function.Value(point);
        }

        /// <summary>
        /// Increment the iterations counter by 1.
        /// 
        /// </summary>
        /// <exception cref="MaxIterationsExceededException">if the maximal number </exception>
        /// of iterations is exceeded.
        /// @since 2.2
        protected void incrementIterationsCounter()
        {
            try
            {
                evaluations.IncrementCount();
            }
            catch (MaxCountExceededException e)
            {
                throw new MaxCountExceededException(e.Max);
            }
        }

        protected abstract double doOptimize();
        #endregion

        #region Local Private Methods

        #endregion
    }
}
