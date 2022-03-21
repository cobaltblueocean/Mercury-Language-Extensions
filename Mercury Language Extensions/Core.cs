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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;

namespace System
{
    /// <summary>
    /// Core utility class to provide general utility methods
    /// </summary>
    public class Core
    {
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
            return CreateInstanceFromType(GetTypeFromName(className));
        }

        /// <summary>
        /// Get a new instance of the Type from the Type
        /// </summary>
        /// <param name="className">Type's fully qualified name that is, its namespace along with its type name</param>
        /// <returns>an instance of the Type</returns>
        public static dynamic CreateInstanceFromType(Type type)
        {
            dynamic instance = null;

            if (type.IsGenericType)
            {
                List<Type> typeParams = type.GetGenericTypeParameter();
                Type constructedType = type.MakeGenericType(typeParams.ToArray());
                var c = constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, typeParams.ToArray(), null);
                if (c != null)
                {

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
                    instance = FormatterServices.GetUninitializedObject(type); //does not call ctor
                }
            }
            else
            {
                var methods = type.GetMethods();
                var constructor = methods.FirstOrDefault(x => x.IsConstructor);
                if (constructor != null)
                {
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
                else
                {
                    instance = FormatterServices.GetUninitializedObject(type); //does not call ctor
                }
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
            return CreateInstanceFromType(GetTypeFromName(className), paramValues);
        }

        /// <summary>
        /// Get a new instance of the Type from the Type
        /// </summary>
        /// <param name="className">Type's fully qualified name that is, its namespace along with its type name</param>
        /// <param name="paramValues">Parameter values which constructor will take</param>
        /// <returns>an instance of the Type</returns>
        public static dynamic CreateInstanceFromType(Type type, Object[] paramValues)
        {
            dynamic instance = null;

            if (type.IsGenericType)
            {
                List<Type> typeParams = type.GetGenericTypeParameter();
                Type constructedType = type.MakeGenericType(typeParams.ToArray());
                var c = constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, typeParams.ToArray(), null);
                if (c != null)
                {
                    object x = Activator.CreateInstance(constructedType, new object[] { paramValues });
                    var method = constructedType.GetMethod(c.Name);
                    instance = method.Invoke(x, new object[] { paramValues });
                }
                else
                {
                    instance = FormatterServices.GetUninitializedObject(type); //does not call ctor
                }
            }
            else
            {
                var methods = type.GetMethods();
                var constructor = methods.FirstOrDefault(x => x.IsConstructor);
                if (constructor != null)
                {
                    var cParams = constructor.GetParameters();
                    var tParams = cParams.Select(x => x.ParameterType).ToArray();

                    var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, tParams, null);
                    object x = Activator.CreateInstance(type, new object[] { paramValues });
                    instance = constructor.Invoke(x, new object[] { paramValues });
                }
                else
                {
                    instance = FormatterServices.GetUninitializedObject(type); //does not call ctor
                }
            }
            return instance;
        }

        public static Type GetTypeFromName(String className)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(className);
                if (type != null)
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

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <see cref="https://stackoverflow.com/questions/930433/apply-properties-values-from-one-object-to-another-of-the-same-type-automaticall"/>
        public static void CopyProperties<T>(T source, T destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception(Mercury.Language.LocalizedResources.Instance().CORE_SOURCE_DESTINATION_OBJECTS_ARE_NULL);
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}
