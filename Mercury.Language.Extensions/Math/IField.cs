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
    /// <summary>
    /// Interface representing a <a href="http://mathworld.wolfram.com/Field.html">field</a>.
    /// 
    /// Classes implementing this interface will often be singletons.
    /// 
    /// </summary>
    /// <typeparam name="T">the type of the field elements</typeparam>
    public interface IField<T>
    {
        /// <summary>
        /// Get the additive identity of the field.
        /// The additive identity is the element e<sub>0</sub> of the field such that
        /// for all elements a of the field, the equalities a + e<sub>0</sub> =
        /// e<sub>0</sub> + a = a hold.
        /// 
        /// Return additive identity of the field
        /// </summary>
        T Zero { get; }

        /// <summary>
        /// Get the multiplicative identity of the field.
        /// The multiplicative identity is the element e<sub>1</sub> of the field such that
        /// for all elements a of the field, the equalities a &times; e<sub>1</sub> =
        /// e<sub>1</sub> &times; a = a hold.
        /// 
        /// Return multiplicative identity of the field
        /// </summary>
        T One { get; }
    }
}
