// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Utility Inc.
//
// Copyright (C) 2012 - present by System.Utility Inc. and the System.Utility group of companies
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

namespace System
{
    /// <summary>
    /// Utility that increments a counter until a maximum is reached, at which point, the instance will by default throw a <see cref="MaxCountExceededException"/>.
    /// However, the user is able to override this behaviour by defining a custom <see cref="Incrementor.MaxCountExceededCallback"/> callback , in order to e.g.
    /// select which exception must be thrown.
    /// </summary>
    public class Incrementor
    {

        #region Local Variables

        /// <summary>
        /// Upper limit for the counter.
        /// </summary>
        private int maximalCount;

        /// <summary>
        /// Current count.
        /// </summary>
        private int count = 0;

        /// <summary>
        /// Function called at counter exhaustion.
        /// </summary>
        private Action<Double> MaxCountExceededCallback;

        #endregion

        #region Property
        /// <summary>
        /// Get/Sets the upper limit for the counter.
        /// This does not automatically reset the current count to zero (see <see cref="Incrementor.ResetCount()"/>.
        /// </summary>
        public int MaximalCount
        {
            get
            {
                return maximalCount;
            }
            set
            {
                maximalCount = value;
            }
        }

        /// <summary>
        /// Gets the current count.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Checks whether a single increment is allowed.
        /// return false if the next call to <see cref="Incrementor.IncrementCount(int)"/>
        /// will trigger a <see cref="MaxCountExceededException"/>, true otherise.
        /// </summary>
        public Boolean CanIncrement
        {
            get { return count < maximalCount; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// For the new instance to be useful, the maximal count must be set
        /// by calling <see cref="Incrementor.MaximalCount(int)"/> setMaximalCount.
        /// </summary>
        public Incrementor() : this(0)
        {

        }

        public Incrementor(int max) : this(max,
                 new Action<Double>((x) =>
                 {
                     throw new MaxCountExceededException(x);
                 }))
        {
        }

        public Incrementor(int max, Action<Double> cb)
        {
            if (cb == null)
            {
                throw new ArgumentNullException();
            }
            maximalCount = max;
            MaxCountExceededCallback = cb;
        }

        #endregion

        #region Implement Methods

        #endregion

        #region Local Public Methods

        /// <summary>
        /// Performs multiple increments.
        /// See the other <see cref="Incrementor.IncrementCount()"/> method).
        /// </summary>
        /// <param name="value">Number of increments.</param>
        public void IncrementCount(int value)
        {
            for (int i = 0; i < value; i++)
            {
                IncrementCount();
            }
        }

        /// <summary>
        /// Adds one to the current iteration count.
        /// At counter exhaustion, this method will call the <see cref="Incrementor.MaxCountExceededCallback"/> delegate of the
        /// callback object passed to the <see cref="Incrementor.MaxCountExceededCallback"/> delegate.
        /// If not explictly set, a default callback is used that will throw a <see cref="MaxCountExceededException"/>. 
        /// </summary>
        public void IncrementCount()
        {
            if (++count > maximalCount)
            {
                MaxCountExceededCallback(maximalCount);
            }
        }

        /// <summary>
        /// Resets the counter to 0.
        /// </summary>
        public void ResetCount()
        {
            count = 0;
        }
        #endregion

        #region Local Private Methods

        #endregion

    }
}
