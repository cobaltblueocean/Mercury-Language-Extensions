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

namespace Mercury.Language.Math.Ranges
{
    /// <summary>
    ///   Common interface for Range objects, such as <see cref="DoubleRange"/>,
    ///   <see cref="IntRange"/> or <see cref="Range"/>. A range represents the
    ///   interval between two values in the form [min, max].
    /// </summary>
    /// 
    /// <typeparam name="T">The type of the range.</typeparam>
    /// 
    /// <seealso cref="Range"/>
    /// <seealso cref="DoubleRange"/>
    /// <seealso cref="IntRange"/>
    /// <seealso cref="ByteRange"/>
    /// 
    public interface IRange<T> : IFormattable
    {
        /// <summary>
        ///   Minimum value of the range.
        /// </summary>
        /// 
        /// <remarks>
        ///   Represents minimum value (left side limit) of the range [<b>min</b>, max].
        /// </remarks>
        /// 
        T Min { get; set; }

        /// <summary>
        ///   Maximum value of the range.
        /// </summary>
        /// 
        /// <remarks>
        ///   Represents maximum value (right side limit) of the range [min, <b>max</b>].
        /// </remarks>
        /// 
        T Max { get; set; }
    }
}
