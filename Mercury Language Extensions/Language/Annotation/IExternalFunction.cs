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

namespace Mercury.Language.Annotation
{
    /// <summary>
    ///  /// Annotation to document a parameter on a method (or constructor) that is
    /// exposed to the external language stack by the {@link ExternalFunction}
    /// annotation. Use of this annotation is optional, available to provide names,
    /// additional type information or descriptions.
    /// </summary>
    public interface IExternalFunction
    {
        /// <summary>
        /// The name of the parameter. If not specified, a name will be inferred from
        /// the parameter order, for example, first param = a, second = b, etc.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Whether to allow null to be passed.
        /// </summary>
        Boolean AllowNull { get; set; }

        /// <summary>
        /// The logical type of the parameter. If omitted, the actual type will be
        /// used. This is only necessary in the case of parameterized types or if the
        /// application conventions requires a stricter sub-class than the method
        /// signature indicates.
        /// </summary>
        String Type { get; set; }

        /// <summary>
        /// A brief description of the parameter to show to the user. This might be as
        /// a tooltip in an interactive environment such as Excel, or appear in
        /// generated reference artifacts such as 'man' pages, PDF or HTML
        /// documentation.
        /// 
        /// This should be written in complete sentences but with no trailing full
        /// stop, properly capitalized. For example, "The number of foos".
        /// </summary>
        String Description { get; set; }
    }
}
