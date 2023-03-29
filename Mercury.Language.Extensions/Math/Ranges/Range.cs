// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Math Inc.
//
// Copyright (C) 2012 - present by System.Math Inc. and the System.Math group of companies
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

// Accord Core Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © AForge.NET, 2007-2011
// contacts@aforgenet.com
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exceptions;

namespace Mercury.Language.Math.Ranges
{
    /// <summary>
    ///   Represents a double range with minimum and maximum values,
    ///   where values are single-precision longing-point (long) numbers.
    /// </summary>
    /// 
    /// <remarks>
    ///   This class represents a double range with inclusive limits, where
    ///   both minimum and maximum values of the range are included into it.
    ///   Mathematical notation of such range is <b>[min, max]</b>.
    /// </remarks>
    /// 
    /// <example>
    /// <code>
    /// // create [0.25, 1.5] range
    /// var range1 = new SingleRange(0.25f, 1.5f);
    /// 
    /// // create [1.00, 2.25] range
    /// var range2 = new SingleRange(1.00f, 2.25f);
    /// 
    /// // check if values is inside of the first range
    /// if (range1.IsInside(0.75f))
    /// {
    ///     // ...
    /// }
    /// 
    /// // check if the second range is inside of the first range
    /// if (range1.IsInside(range2))
    /// {
    ///     // ...
    /// }
    /// 
    /// // check if two ranges overlap
    /// if (range1.IsOverlapping(range2))
    /// {
    ///     // ...
    /// }
    /// </code>
    /// </example>
    /// 
    /// <seealso cref="IntRange"/>
    /// <seealso cref="DoubleRange"/>
    /// <seealso cref="ByteRange"/>
    /// 
    [Serializable]
    public struct Range : IRange<long>, IEquatable<Range>
    {
        public long MinSmallest { get; set; }
        public long MinLargest { get; set; }
        public long MaxSmallest { get; set; }
        public long MaxLargest { get; set; }

        //private BigList<long> _range = new BigList<long>();

        public Boolean IsIntValue
        {
            get
            {
                return MinSmallest >= int.MinValue && MaxLargest <= int.MaxValue;
            }
        }

        public long this[long index]
        {
            get { return this.MinSmallest + index; }
            //set { _range[index] = value; }
        }

        /// <summary>
        ///   Minimum value of the range.
        /// </summary>
        /// 
        /// <remarks>
        ///   Represents minimum value (left side limit) of the range [<b>min</b>, max].
        /// </remarks>
        /// 
        public long Min
        {
            get { return MinSmallest; }
            set { MinSmallest = value; }
        }

        /// <summary>
        ///   Maximum value of the range.
        /// </summary>
        /// 
        /// <remarks>
        ///   Represents maximum value (right side limit) of the range [min, <b>max</b>].
        /// </remarks>
        /// 
        public long Max
        {
            get { return MaxLargest; }
            set { MaxLargest = value; }
        }

        /// <summary>
        ///   Gets the length of the range, defined as (max - min).
        /// </summary>
        /// 
        public long Length
        {
            get { return Max - Min; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// 
        /// <param name="min">Minimum value of the range.</param>
        /// <param name="max">Maximum value of the range.</param>
        /// 
        public Range(long min, long max) : this(min, min, max, max)
        {
        }

        public Range(long min, long maxSmallest, long maxLargest) : this(min, min, maxSmallest, maxLargest)
        {
        }

        public Range(long minSmallest, long minLargest, long maxSmallest, long maxLargest)
        {
            this.MinSmallest = minSmallest;
            this.MinLargest = minLargest;
            this.MaxSmallest = maxSmallest;
            this.MaxLargest = maxLargest;

            //AssignRange(MinSmallest, MaxLargest);
        }

        /// <summary>
        ///   Check if the specified value is inside of the range.
        /// </summary>
        /// 
        /// <param name="x">Value to check.</param>
        /// 
        /// <returns>
        ///   <b>True</b> if the specified value is inside of the range or <b>false</b> otherwise.
        /// </returns>
        /// 
        public bool IsInside(long x)
        {
            return ((x >= Min) && (x <= Max));
        }


        public void AssignRange(long min, long max)
        {

            this.MinSmallest = min;
            this.MinLargest = min;
            this.MaxSmallest = max;
            this.MaxLargest = max;

            //_range.Clear();

            //CancellationTokenSource cts = new CancellationTokenSource();

            //// Use ParallelOptions instance to store the CancellationToken
            //AutoParallelOptions po = new AutoParallelOptions(new ParallelOptions());
            //po.CancellationToken = cts.Token;
            //po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            //AutoParallel.AutoParallelFor(min, max + 1, po, (i, ps) =>
            //{
            //    try
            //    {
            //        _range.Add(i);
            //    }
            //    catch (OperationCanceledException e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //    catch (System.Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        cts.Cancel();
            //    }
            //    finally
            //    {
            //        cts.Dispose();
            //    }
            //}, ParallelMode.ForceParallel);
        }


        /// <summary>
        ///   Check if the specified range is inside of the range.
        /// </summary>
        /// 
        /// <param name="range">Range to check.</param>
        /// 
        /// <returns>
        ///   <b>True</b> if the specified range is inside of the range or <b>false</b> otherwise.
        /// </returns>
        /// 
        public bool IsInside(Range range)
        {
            return ((IsInside(range.Min)) && (IsInside(range.Max)));
        }

        /// <summary>
        ///   Check if the specified range overlaps with the range.
        /// </summary>
        /// 
        /// <param name="range">Range to check for overlapping.</param>
        /// 
        /// <returns>
        ///   <b>True</b> if the specified range overlaps with the range or <b>false</b> otherwise.
        /// </returns>
        /// 
        public bool IsOverlapping(Range range)
        {
            return ((IsInside(range.Min)) || (IsInside(range.Max)) ||
                     (range.IsInside(Min)) || (range.IsInside(Max)));
        }

        /// <summary>
        ///   Computes the intersection between two ranges.
        /// </summary>
        /// 
        /// <param name="range">The second range for which the intersection should be calculated.</param>
        /// 
        /// <returns>An new <see cref="Range"/> structure containing the intersection
        /// between this range and the <paramref name="range"/> given as argument.</returns>
        /// 
        public Range Intersection(Range range)
        {
            return new Range(System.Math.Max(this.Min, range.Min), System.Math.Min(this.Max, range.Max));
        }


        /// <summary>
        /// Checks if the value is within the valid range.
        /// <p>
        /// This checks that the value is within the stored range of values.
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <returns>true if the value is valid</returns>
        public Boolean IsValidValue(long value)
        {
            return (value >= MinSmallest && value <= MaxLargest);
        }

        /// <summary>
        /// Checks if the value is within the valid range and that all values
        /// in the range fit in an <see cref="int"/>.
        /// <p>
        /// This method combines <see cref="IsIntValue"/> and <see cref="IsValidValue(long)"/>.
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <returns>true if the value is valid and fits in an <see cref="int"/></returns>
        public Boolean IsValidIntValue(long value)
        {
            return IsIntValue && IsValidValue(value);
        }

        /// <summary>
        ///   Determines whether two instances are equal.
        /// </summary>
        /// 
        public static bool operator ==(Range range1, Range range2)
        {
            return ((range1.Min == range2.Min) && (range1.Max == range2.Max));
        }

        /// <summary>
        ///   Determines whether two instances are not equal.
        /// </summary>
        /// 
        public static bool operator !=(Range range1, Range range2)
        {
            return ((range1.Min != range2.Min) || (range1.Max != range2.Max));
        }

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// 
        /// <param name="other">An object to compare with this object.</param>
        /// 
        /// <returns>
        ///   true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// 
        public bool Equals(Range other)
        {
            return this == other;
        }

        /// <summary>
        ///   Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// 
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// 
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// 
        public override bool Equals(object obj)
        {
            return (obj is Range) ? (this == (Range)obj) : false;
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// 
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// 
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + this.MinSmallest.GetHashCode();
                hash = hash * 31 + this.MinLargest.GetHashCode();
                hash = hash * 31 + this.MaxSmallest.GetHashCode();
                hash = hash * 31 + this.MaxLargest.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// 
        /// <returns>
        ///   A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// 
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", this.MinSmallest, this.MinLargest, this.MaxSmallest, this.MaxLargest);
        }

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// 
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// 
        /// <returns>
        ///   A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// 
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Format("[{0}, {1}, {2}, {3}]",
                this.MinSmallest.ToString(format, formatProvider),
                this.MinLargest.ToString(format, formatProvider),
                this.MaxSmallest.ToString(format, formatProvider),
                this.MaxLargest.ToString(format, formatProvider));
        }


        /// <summary>
        ///   Performs an implicit conversion from <see cref="IntRange"/> to <see cref="DoubleRange"/>.
        /// </summary>
        /// 
        /// <param name="range">The range.</param>
        /// 
        /// <returns>
        ///   The result of the conversion.
        /// </returns>
        /// 
        public static implicit operator DoubleRange(Range range)
        {
            return new DoubleRange(range.Min, range.Max);
        }

        /// <summary>
        ///   Converts this single-precision range into an <see cref="IntRange"/>.
        /// </summary>
        /// 
        /// <param name="provideInnerRange">
        ///   Specifies if inner integer range must be returned or outer range.</param>
        /// 
        /// <returns>Returns integer version of the range.</returns>
        /// 
        /// <remarks>
        ///   If <paramref name="provideInnerRange"/> is set to <see langword="true"/>, then the
        ///   returned integer range will always fit inside of the current single precision range.
        ///   If it is set to <see langword="false"/>, then current single precision range will always
        ///   fit into the returned integer range.
        /// </remarks>
        ///
        public IntRange ToIntRange(bool provideInnerRange)
        {
            //int iMin, iMax;

            //if (provideInnerRange)
            //{
            //    iMin = (int)System.Math.Ceiling(Min);
            //    iMax = (int)System.Math.Floor(Max);
            //}
            //else
            //{
            //    iMin = (int)System.Math.Floor(Min);
            //    iMax = (int)System.Math.Ceiling(Max);
            //}


            return new IntRange((int)Min, (int)Max);
        }
    }

    public static class Ext
    {
        public static Range To(this long lower, long upper)
        {
            return new Range { MinSmallest = lower, MaxSmallest = upper };
        }

        public static bool In(this long n, Range r)
        {
            return n >= r.MinSmallest && n <= r.MaxSmallest;
        }
    }

}
