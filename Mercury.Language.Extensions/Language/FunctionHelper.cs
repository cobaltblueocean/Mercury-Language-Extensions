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
using Mercury.Language.Exception;

namespace System
{
    /// <summary>
    /// FunctionHelper Description
    /// </summary>
    public static class FunctionHelper
    {
        /// <summary>
        /// Get parameters value of Delegate/Func/Action
        /// </summary>
        /// <param name="method">Delegate/Func/Action to get the parameters value</param>
        /// <returns>Values of the parameters</returns>
        public static List<object> GetMethodParameterValues(Delegate method)
        {
            var target = method.Target;
            if (target == null) return null;
            var fields = target.GetType().GetFields();
            var valueList = fields.Select(field => field.GetValue(target)).ToList();
            return valueList;
        }

        /// <summary>
        /// Get a parameter value of given Delegate/Func/Action
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="method">Delegate/Func/Action to get the parameters value</param>
        /// <returns>Value of the parameter</returns>
        public static T GetMethodParameterValue<T>(Delegate method)
        {
            var v = GetMethodParameterValues(method);
            if ((v != null) && (v.Count > 0))
                return (T)v[0];
            else
                return default(T);
        }
    }
}
