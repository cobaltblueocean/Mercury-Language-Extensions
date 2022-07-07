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
    /// Annotation to signal that a method should be exposed through the external
    /// language stack to appear, for example, as a function in Excel or R.
    ///
    /// When the annotation is used on a method it means to expose that method. If
    /// the method is not static, it can only be exposed if there is an accessible
    /// no-arg constructor for the class.
    ///
    /// When the annotation is used on a constructor it means to expose a function
    /// that creates an instance of the object and returns it. Note that this will
    /// typically rely on the object to be Fudge serializable to work correctly.
    /// </summary>
    public interface IExternalFunctionParam
    {
        /// <summary>
        /// The name of the function exposed to callers. If not specified here, a name
        /// will be created from the method name (or class name if on a constructor).
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Alternative names for the function also exposed to callers in addition to
        /// the primary name. If omitted, default aliases are created as:
        /// 
        ///  - Class prefixed method name (for methods)
        ///  - Class name (for constructors)
        ///  - Package and class prefixed method name (for methods)
        ///  - Package and class name (for constructors)
        /// 
        /// With any of the above omitted if they match the published primary name.
        /// </summary>
        String Alias { get; set; }

        /// <summary>
        /// Category describing the function's behavior. Typically omit.
        /// </summary>
        String Category { get; set; }

        /// <summary>
        /// A brief description of the function to show to the user. This might be as
        /// a tooltip in an interactive environment such as Excel, or appear in
        /// generated reference artifacts such as 'man' pages, PDF or HTML
        /// documentation.
        /// 
        /// This should be written in complete sentences but with no trailing full
        /// stop, properly capitalized. For example, "Calculates the foo to bar ratio".
        /// </summary>
        String Description { get; set; }
    }
}
