// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Integrator
{
    /// <summary>
    /// Provide a default implementation for several generic functions.
    /// </summary>
    public abstract class UnivariateRealIntegrator : ConvergingAlgorithm, IUnivariateRealIntegrator
    {
        /// <summary>
        /// minimum number of iterations
        /// </summary>
        protected int minimalIterationCount;

        /// <summary>
        /// default minimum number of iterations
        /// </summary>
        protected int defaultMinimalIterationCount;

        /// <summary>
        /// indicates whether an integral has been computed
        /// </summary>
        protected Boolean resultComputed = false;

        /// <summary>
        /// the last computed integral
        /// </summary>
        protected double result;

        /// <summary>
        /// The integrand function.
        /// </summary>
        protected IUnivariateRealFunction f;

        /// <summary>
        /// Construct an integrator with given iteration count and accuracy.
        /// </summary>
        /// <param name="f">the integrand function</param>
        /// <param name="defaultMaximalIterationCount">maximum number of iterations</param>
        /// <exception cref="NullReferenceException">if f is null or the iteration limits are not valid</exception>
        public UnivariateRealIntegrator(IUnivariateRealFunction f, int defaultMaximalIterationCount) : base(defaultMaximalIterationCount, 1.0e-15)
        {
            if (f == null)
            {
                throw new NullReferenceException(LocalizedResources.Instance().FUNCTION);
            }

            this.f = f;

            // parameters that are problem specific
            RelativeAccuracy = 1.0e-6;
            this.defaultMinimalIterationCount = 3;
            this.minimalIterationCount = defaultMinimalIterationCount;

            VerifyIterationCount();
        }

        /// <summary>
        /// Construct an integrator with given iteration count and accuracy.
        /// </summary>
        /// <param name="defaultMaximalIterationCount">maximum number of iterations</param>
        /// <exception cref="NullReferenceException">if f is null or the iteration limits are not valid</exception>
        protected UnivariateRealIntegrator(int defaultMaximalIterationCount) : base(defaultMaximalIterationCount, 1.0e-15)
        {
            // parameters that are problem specific
            RelativeAccuracy = 1.0e-6;
            this.defaultMinimalIterationCount = 3;
            this.minimalIterationCount = defaultMinimalIterationCount;

            VerifyIterationCount();
        }

        public int MinimalIterationCount
        {
            get
            {
                return minimalIterationCount;
            }

            set
            {
                minimalIterationCount = value;
            }
        }

        public virtual double Integrate(double min, double max)
        {
            throw new NotImplementedException();
        }

        public virtual double Integrate(IUnivariateRealFunction f, double min, double max)
        {
            throw new NotImplementedException();
        }

        public virtual void ResetMinimalIterationCount()
        {
            minimalIterationCount = defaultMinimalIterationCount;
        }

        /// <summary>
        /// Access the last computed integral.
        /// </summary>
        /// <returns>the last computed integral</returns>
        /// <exception cref="ArithmeticException">if no integral has been computed</exception>
        public virtual double Result()
        {
            if (resultComputed)
            {
                return result;
            }
            else
            {
                throw new ArithmeticException(LocalizedResources.Instance().NO_RESULT_AVAILABLE);
            }
        }

        /// <summary>
        /// Convenience function for implementations.
        /// </summary>
        /// <param name="newResult">the result to set</param>
        /// <param name="iterationCount">the iteration count to set</param>
        protected virtual void SetResult(double newResult, int iterationCount)
        {
            this.result = newResult;
            this.iterationCount = iterationCount;
            this.resultComputed = true;
        }

        /// <summary>
        /// Convenience function for implementations.
        /// </summary>
        protected virtual void ClearResult()
        {
            this.iterationCount = 0;
            this.resultComputed = false;
        }

        /// <summary>
        /// Verifies that the endpoints specify an interval.
        /// </summary>
        /// <param name="lower">lower endpoint</param>
        /// <param name="upper">upper endpoint</param>
        /// <exception cref="ArithmeticException">if not interval</exception>
        protected virtual void VerifyInterval(double lower, double upper)
        {
            if (lower >= upper)
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().ENDPOINTS_NOT_AN_INTERVAL, lower, upper));
            }
        }

        /// <summary>
        /// Verifies that the upper and lower limits of iterations are valid.
        /// </summary>
        /// <exception cref="ArithmeticException">if not valid</exception>
        protected virtual void VerifyIterationCount()
        {
            if ((minimalIterationCount <= 0) || (maximalIterationCount <= minimalIterationCount))
            {
                throw new ArithmeticException(String.Format(LocalizedResources.Instance().INVALID_ITERATIONS_LIMITS, minimalIterationCount, maximalIterationCount));
            }
        }
    }
}
