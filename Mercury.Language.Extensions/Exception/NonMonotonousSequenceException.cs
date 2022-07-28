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

namespace Mercury.Language.Exception
{
    /// <summary>
    /// Exception to be thrown when the a sequence of values is not monotonously
    /// increasing or decreasing.
    /// 
    /// @since 2.2
    /// @version $Revision$ $Date$
    /// </summary>
    public class NonMonotonousSequenceException : MathIllegalNumberException
    {

        /// <summary>
        /// Direction (positive for increasing, negative for decreasing).
        /// </summary>
        private Math2.OrderDirection _direction;
        /// <summary>
        /// Whether the sequence must be strictly increasing or decreasing.
        /// </summary>
        private Boolean _strict;
        /// <summary>
        /// Index of the wrong value.
        /// </summary>
        private int _index;
        /// <summary>
        /// Previous value.
        /// </summary>
        private double _previous;

        /// <summary>
        /// Construct the exception.
        /// This constructor uses default values assuming that the sequence should
        /// have been strictly increasing.
        /// 
        /// </summary>
        /// <param Name="wrong">Value that did not match the requirements.</param>
        /// <param Name="previous">Previous value in the sequence.</param>
        /// <param Name="index">Index of the value that did not match the requirements.</param>
        public NonMonotonousSequenceException(double wrong, double previous, int index) : this(wrong, previous, index, Math2.OrderDirection.INCREASING, true)
        {

        }

        /// <summary>
        /// Construct the exception.
        /// 
        /// </summary>
        /// <param Name="wrong">Value that did not match the requirements.</param>
        /// <param Name="previous">Previous value in the sequence.</param>
        /// <param Name="index">Index of the value that did not match the requirements.</param>
        /// <param Name="direction">Strictly positive for a sequence required to be</param>
        /// increasing, negative (or zero) for a decreasing sequence.
        /// <param Name="strict">Whether the sequence must be strictly increasing or</param>
        /// decreasing.
        public NonMonotonousSequenceException(double wrong, double previous, int index, Math2.OrderDirection direction, Boolean strict) : base(direction == Math2.OrderDirection.INCREASING ?
                  (strict ?
                   LocalizedResources.Instance().NOT_STRICTLY_INCREASING_SEQUENCE :
                   LocalizedResources.Instance().NOT_INCREASING_SEQUENCE) :
                  (strict ?
                   LocalizedResources.Instance().NOT_STRICTLY_DECREASING_SEQUENCE :
                   LocalizedResources.Instance().NOT_DECREASING_SEQUENCE),
                  wrong, previous, index, index - 1)
        {


            this._direction = direction;
            this._strict = strict;
            this._index = index;
            this._previous = previous;
        }

        /// <summary>
        ///
        /// <returns>the order direction.</returns>
        public Math2.OrderDirection GetDirection()
        {
            return _direction;
        }
        /// <summary>
        ///
        /// <returns>{@code true} is the sequence should be strictly monotonous.</returns>
        public Boolean getStrict()
        {
            return _strict;
        }
        /// <summary>
        /// Get the index of the wrong value.
        /// 
        /// </summary>
        /// <returns>the current index.</returns>
        public int Index
        {
            get { return _index; }
        }
        /// <summary>
        /// </summary>
        /// <returns>the previous value.</returns>
        public double Previous
        {
            get { return _previous; }
        }
    }
}
