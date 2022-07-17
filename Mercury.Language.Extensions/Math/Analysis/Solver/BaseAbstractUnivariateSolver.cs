// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math.Analysis.Solver Inc.
//
// Copyright (C) 2012 - present by System.Math.Analysis.Solver Incd and the System.Math.Analysis.Solver group of companies
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
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// BaseAbstractUnivariateSolver Description
    /// </summary>
    public abstract class BaseAbstractUnivariateSolver : IBaseUnivariateSolver
    {

        #region Local Variables
        /** Default relative accuracyd */
        protected static double DEFAULT_RELATIVE_ACCURACY = 1e-14;
        /** Default function value accuracyd */
        protected static double DEFAULT_FUNCTION_VALUE_ACCURACY = 1e-15;
        /** Function value accuracyd */
        protected double functionValueAccuracy;
        /** Absolute accuracyd */
        protected double absoluteAccuracy;
        /** Relative accuracyd */
        protected double relativeAccuracy;
        /** The last iteration count. */
        protected int iterationCount;
        /** Maximum number of iterations. */
        protected int maximalIterationCount;
        /** Evaluations counterd */
        protected Incrementor evaluations = new Incrementor();
        /** Lower end of search intervald */
        protected double searchMin;
        /** Higher end of search intervald */
        protected double searchMax;
        /** Initial guessd */
        protected double searchStart;
        /** Function to solved */
        protected IUnivariateRealFunction function;
        #endregion

        #region Property
        #region Implement Properties
        public double AbsoluteAccuracy
        {
            get
            {
                return absoluteAccuracy;
            }
            set
            {
                absoluteAccuracy = value;
            }
        }

        public double FunctionValueAccuracy
        {
            get
            {
                return functionValueAccuracy;
            }
            set
            {
                functionValueAccuracy = value;
            }
        }

        public double RelativeAccuracy
        {
            get
            {
                return relativeAccuracy;
            }
            set
            {
                relativeAccuracy = value;
            }
        }
        #endregion

        #region Local Properties

        /// <summary>
        /// the lower end of the search interval.
        /// </summary>
        public double Min
        {
            get
            {
                return searchMin;
            }
            set
            {
                searchMin = value;
            }
        }

        /// <summary>
        /// the higher end of the search interval.
        /// </summary>
        public double Max
        {
            get
            {
                return searchMax;
            }
            set
            {
                searchMax = value;
            }
        }

        /// <summary>
        /// the initial guess.
        /// </summary>
        public double StartValue
        {
            get
            {
                return searchStart;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Construct a solver with given absolute accuracy.
        /// </summary>
        /// <param name="absoluteAccuracy">Maximum absolute error.</param>
        protected BaseAbstractUnivariateSolver(double absoluteAccuracy) : this(DEFAULT_RELATIVE_ACCURACY, absoluteAccuracy, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {

        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy">Maximum relative error.</param>
        /// <param name="absoluteAccuracy">Maximum absolute error.</param>
        protected BaseAbstractUnivariateSolver(double relativeAccuracy, double absoluteAccuracy) : this(relativeAccuracy, absoluteAccuracy, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {

        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy">Maximum relative error.</param>
        /// <param name="absoluteAccuracy">Maximum absolute error.</param>
        /// <param name="functionValueAccuracy">Maximum function value error.</param>
        protected BaseAbstractUnivariateSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy)
        {
            this.absoluteAccuracy = absoluteAccuracy;
            this.relativeAccuracy = relativeAccuracy;
            this.functionValueAccuracy = functionValueAccuracy;
        }
        #endregion

        #region Implement Methods

        public virtual double Solve(IUnivariateRealFunction f, Double[] values, double startValue)
        {
            return Solve(f, values, Double.NaN, Double.NaN, startValue);
        }

        public virtual double Solve(IUnivariateRealFunction f, Double[] values, double min, double max)
        {
            return Solve(f, values, min, max, min + 0.5 * (max - min));
        }

        public virtual double Solve(IUnivariateRealFunction f, Double[] values, double min, double max, double startValue)
        {
            // Initialization.
            Setup(f, values, min, max, startValue);

            // Perform computation.
            return DoSolve();
        }
        #endregion

        #region Local Public Methods
        protected void Setup(IUnivariateRealFunction f, Double[] values, double min, double max, double startValue)
        {
            // Checks.
            // ArgumentChecker.NotNull(f, "f");

            // Reset.
            searchMin = min;
            searchMax = max;
            searchStart = startValue;
            function = f;
            //evaluations.setMaximalCount(maxEval);
            //evaluations.resetCount();
        }

        protected void Setup(int maxEval, IUnivariateRealFunction f, Double[] values, double min, double max, double startValue)
        {
            // Checks.
            // ArgumentChecker.NotNull(f, "f");

            // Reset.
            searchMin = min;
            searchMax = max;
            searchStart = startValue;
            function = f;
            //evaluations = evaluations.withMaximalCount(maxEval).withStart(0);
        }

        protected double ComputeObjectiveValue(double point)
        {
            return function.Value(point);
        }

        protected Boolean IsBracketing(IUnivariateRealFunction function, double lower, double upper)
        {
            if (function == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FUNCTION);
            }

            double fLo = function.Value(lower);
            double fHi = function.Value(upper);
            return (fLo >= 0 && fHi <= 0) || (fLo <= 0 && fHi >= 0);
        }

        protected Boolean IsSequence(double start, double mid, double end)
        {
            return (start < mid) && (mid < end);
        }

        protected void VerifyInterval(double lower,
                              double upper)
        {
            if (lower >= upper)
            {
                throw new OverflowException(String.Format(LocalizedResources.Instance().ENDPOINTS_NOT_AN_INTERVAL, lower, upper, false));
            }
        }

        protected void VerifySequence(double lower,
                              double initial,
                              double upper)
        {
            VerifyInterval(lower, initial);
            VerifyInterval(initial, upper);
        }

        protected void VerifyBracketing(double lower,
                                double upper)
        {
            VerifyBracketing(function, lower, upper);
        }

        protected void IncrementEvaluationCount()
        {
            try
            {
                evaluations.IncrementCount();
            }
            catch (MaxCountExceededException e)
            {
                throw new TooManyEvaluationsException(e.Max);
            }
        }
        #endregion

        #region Local Protected Methods

        protected abstract double DoSolve();

        #endregion


        #region Local Private Methods

        private void VerifyBracketing(IUnivariateRealFunction function, double lower, double upper)
        {
            if (function == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FUNCTION);
            }
            VerifyInterval(lower, upper);
            if (!IsBracketing(function, lower, upper))
            {
                throw new NoBracketingException(lower, upper, function.Value(lower), function.Value(upper));
            }
        }
        #endregion
    }
}
