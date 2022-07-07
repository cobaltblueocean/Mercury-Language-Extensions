// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma.Analytics.Utility Inc.
//
// Copyright (C) 2012 - present by OpenGamma.Analytics.Utility Inc. and the OpenGamma.Analytics.Utility group of companies
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class MethodInfoExtension
    {
        public static Type[] GetParameterTypes(this MethodInfo method)
        {
            HashSet<Type> t = new HashSet<Type>();
            foreach (var param in method.GetParameters())
            {
                t.Add(param.ParameterType);
            }

            Type[] retArray = new Type[t.Count];
            t.CopyTo(retArray);

            return retArray;
        }
    }
}
