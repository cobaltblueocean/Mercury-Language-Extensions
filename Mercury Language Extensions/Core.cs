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
using System.Reflection;

namespace System
{
    /// <summary>
    /// Core utility class to provide general utility methods
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Check if the Namespace is existing in the AppDomain.
        /// </summary>
        /// <param name="desiredNamespace"></param>
        /// <returns>True if exists, otherwise returns false.</returns>
        /// <see href="https://forum.unity.com/threads/run-bit-of-code-if-namespace-exists-c.437745/"/>
        public static bool NamespaceExists(string desiredNamespace)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace == desiredNamespace)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get a new instance of the Type from the name
        /// </summary>
        /// <param name="className">Type's fully qualified name that is, its namespace along with its type name</param>
        /// <returns>an instance of the Type</returns>
        public static dynamic CreateInstanceFromName(String className)
        {
            dynamic instance = null;
            var type = GetTypeFromName(className);

            if (type.IsGenericType)
            {
                List<Type> typeParams = type.GetGenericTypeParameter();
                Type constructedType = type.MakeGenericType(typeParams.ToArray());
                var c = constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, typeParams.ToArray(), null);

                Object[] paramValues = new Object[typeParams.Count];
                for (int i = 0; i < typeParams.Count; i++)
                {
                    Type p = typeParams[i];
                    paramValues[i] = default;
                }

                object x = Activator.CreateInstance(constructedType, new object[] { paramValues });
                var method = constructedType.GetMethod(c.Name);
                instance = method.Invoke(x, new object[] { paramValues });
            }
            else
            {
                var methods = type.GetMethods();
                var constructor = methods.FirstOrDefault(x => x.IsConstructor);
                var cParams = constructor.GetParameters();
                var tParams = cParams.Select(x => x.ParameterType).ToArray();

                Object[] paramValues = new Object[cParams.Length];
                for (int i = 0; i < cParams.Length; i++)
                {
                    Type p = cParams[i].ParameterType;
                    paramValues[i] = default;
                }

                var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, tParams, null);
                object x = Activator.CreateInstance(type, new object[] { paramValues });
                instance = constructor.Invoke(x, new object[] { paramValues });
            }

            return instance;
        }

        /// <summary>
        /// Get a new instance of the Type from the name
        /// </summary>
        /// <param name="className">Type's fully qualified name that is, its namespace along with its type name</param>
        /// <param name="paramValues">Parameter values which constructor will take</param>
        /// <returns>an instance of the Type</returns>
        public static dynamic CreateInstanceFromName(String className, Object[] paramValues)
        {
            dynamic instance = null;
            var type = GetTypeFromName(className);

            if (type.IsGenericType)
            {
                List<Type> typeParams = type.GetGenericTypeParameter();
                Type constructedType = type.MakeGenericType(typeParams.ToArray());
                var c = constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, typeParams.ToArray(), null);

                object x = Activator.CreateInstance(constructedType, new object[] { paramValues });
                var method = constructedType.GetMethod(c.Name);
                instance = method.Invoke(x, new object[] { paramValues });
            }
            else
            {
                var methods = type.GetMethods();
                var constructor = methods.FirstOrDefault(x => x.IsConstructor);
                var cParams = constructor.GetParameters();
                var tParams = cParams.Select(x => x.ParameterType).ToArray();

                var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, tParams, null);
                object x = Activator.CreateInstance(type, new object[] { paramValues });
                instance = constructor.Invoke(x, new object[] { paramValues });
            }
            return instance;
        }

        public static Type GetTypeFromName(String className)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(className);
                if (type !=null)
                {
                    return type;
                }
            }

            return null;
        }

        public static Boolean IsSameClassOrImplementedInterface(Type targetType, Type compareType, Type[] typeParams)
        {
            if (targetType.IsGenericType != compareType.IsGenericType)
            {
                return false;
            }
            else if (targetType.IsGenericType && typeParams == null)
            {
                return false;
            }
            else if (targetType.GetGenericArguments().Length != compareType.GetGenericArguments().Length)
            {
                return false;
            }

            else if (targetType.GetGenericArguments().Length != typeParams.Length)
            {
                return false;
            }
            else
            {
                Type constructedType = compareType.MakeGenericType(typeParams.ToArray());
                return targetType.GetInterfaces().Contains(constructedType) || targetType == constructedType;
            }
        }
    }
}
