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

// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
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
using Mercury.Language.Math.Ranges;

namespace Mercury.Language.Math.Analysis.Integration
{
    /// <summary>
    ///   Common interface for multidimensional integration methods.
    /// </summary>
    /// 
    public interface IUnivariateIntegration : INumericalIntegration
    {

        /// <summary>
        ///   Gets or sets the unidimensional function
        ///   whose integral should be computed.
        /// </summary>
        /// 
        Func<Double, Double> Function { get; set; }

        /// <summary>
        ///   Gets or sets the input range under 
        ///   which the integral must be computed.
        /// </summary>
        /// 
        DoubleRange Range { get; set; }
    }
}

