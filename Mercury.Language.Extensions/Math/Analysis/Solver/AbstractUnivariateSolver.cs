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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// AbstractUnivariateSolver Description
    /// </summary>
    public abstract class AbstractUnivariateSolver : BaseAbstractUnivariateSolver
    {

        /// <summary>
        /// Default maximum error of function.
        /// </summary>
        protected double defaultFunctionValueAccuracy;

        /// <summary>
        /// Indicates where a root has been computed.
        /// </summary>
        protected Boolean resultComputed = false;

        /// <summary>
        /// The last computed root.
        /// </summary>
        protected double result;

        /// <summary>
        /// Value of the function at the last computed result.
        /// </summary>
        protected double functionValue;


        /// <summary>
        /// Construct a solver with given absolute accuracy.
        /// </summary>
        /// <param name="absoluteAccuracy">Maximum absolute error.</param>
        public AbstractUnivariateSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {

        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy">Maximum relative error.</param>
        /// <param name="absoluteAccuracy">Maximum absolute error.</param>
        public AbstractUnivariateSolver(double relativeAccuracy,
                                       double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {

        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy">Maximum relative error.</param>
        /// <param name="absoluteAccuracy">Maximum absolute error.</param>
        /// <param name="functionValueAccuracy">Maximum function value error.</param>
        public AbstractUnivariateSolver(double relativeAccuracy,
                                       double absoluteAccuracy,
                                       double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {

        }

        /// <summary>
        /// Convenience function for implementations.
        /// 
        /// </summary>
        /// <param name="newResult">the result to set</param>
        /// <param name="iterationCount">the iteration count to set</param>
        protected void SetResult(double newResult, int iterationCount)
        {
            this.result = newResult;
            this.iterationCount = iterationCount;
            this.resultComputed = true;
        }

        /// <summary>
        /// Convenience function for implementations.
        /// 
        /// </summary>
        /// <param name="x">the result to set</param>
        /// <param name="fx">the result to set</param>
        /// <param name="iterationCount">the iteration count to set</param>
        protected void SetResult(double x, double fx,
                                        int iterationCount)
        {
            this.result = x;
            this.functionValue = fx;
            this.iterationCount = iterationCount;
            this.resultComputed = true;
        }

        /// <summary>
        /// Convenience function for implementations.
        /// </summary>
        protected void ClearResult()
        {
            this.iterationCount = 0;
            this.resultComputed = false;
        }
    }
}
