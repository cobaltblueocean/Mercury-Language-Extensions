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

namespace Mercury.Language.Math
{
    public abstract class ConvergingAlgorithm : IConvergingAlgorithm
    {
        /** Maximum absolute error. */
        protected double absoluteAccuracy;

        /** Maximum relative error. */
        protected double relativeAccuracy;

        /** Maximum number of iterations. */
        protected int maximalIterationCount;

        /** Default maximum absolute error. */
        protected double defaultAbsoluteAccuracy;

        /** Default maximum relative error. */
        protected double defaultRelativeAccuracy;

        /** Default maximum number of iterations. */
        protected int defaultMaximalIterationCount;

        /** The last iteration count. */
        protected int iterationCount;

        /// <summary>
        /// Construct an algorithm with given iteration count and accuracy.
        /// </summary>
        /// <param name="defaultMaximalIterationCount">maximum absolute error</param>
        /// <param name="defaultAbsoluteAccuracy">maximum number of iterations</param>
        protected ConvergingAlgorithm(int defaultMaximalIterationCount, double defaultAbsoluteAccuracy)
        {
            this.defaultAbsoluteAccuracy = defaultAbsoluteAccuracy;
            this.defaultRelativeAccuracy = 1.0e-14;
            this.absoluteAccuracy = defaultAbsoluteAccuracy;
            this.relativeAccuracy = defaultRelativeAccuracy;
            this.defaultMaximalIterationCount = defaultMaximalIterationCount;
            this.maximalIterationCount = defaultMaximalIterationCount;
            this.iterationCount = 0;
        }

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

        public int IterationCount
        {
            get
            {
                return iterationCount;
            }
        }

        public int MaximalIterationCount
        {
            get
            {
                return maximalIterationCount;
            }

            set
            {
                maximalIterationCount = value;
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

        public void ResetAbsoluteAccuracy()
        {
            absoluteAccuracy = defaultAbsoluteAccuracy;
        }

        public void ResetMaximalIterationCount()
        {
            maximalIterationCount = defaultMaximalIterationCount;
        }

        public void ResetRelativeAccuracy()
        {
            relativeAccuracy = defaultRelativeAccuracy;
        }

        protected void resetIterationsCounter()
        {
            iterationCount = 0;
        }

        protected void incrementIterationsCounter()
        {
        if (++iterationCount > maximalIterationCount) {
                throw new IndexOutOfRangeException(maximalIterationCount.ToString());
            }
        }
    }
}
