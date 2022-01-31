﻿// Copyright (c) 2017 - presented by Kei Nakai
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
using System.Reflection;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Useful extension method utilities for Object class
    /// </summary>
    public static class ObjectExtension
    {
        public static bool IsImplementType(this Object baseObj, Type type)
        {
            var class0 = baseObj.GetType();
            return class0.GetInterfaces().Contains(type);
        }

        public static List<Type> GetGenericTypeParameter(this Type type)
        {
            List<Type> result = null;

            if (type.IsGenericType)
            {
                result = type.GetGenericArguments().ToList();
            }
            return result;
        }


        public static T NewInstance<T>(this Type type)
        {
            return (T)NewInstance(type);
        }

        public static dynamic NewInstance(this Type type)
        {
            Object[] paramValues = null;

            if (type.IsGenericType)
            {
                List<Type> typeParams = GetGenericTypeParameter(type);
                Type constructedType = type.MakeGenericType(typeParams.ToArray());
                var c = constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, typeParams.ToArray(), null);
                if (c != null)
                {
                    paramValues = new Object[typeParams.Count];
                    for (int i = 0; i < typeParams.Count; i++)
                    {
                        Type p = typeParams[i];
                        paramValues[i] = default;
                    }

                }
                return Activator.CreateInstance(constructedType, paramValues);
            }
            else
            {
                var methods = type.GetMethods();
                var constructor = methods.FirstOrDefault(x => x.IsConstructor);
                if (constructor != null)
                {
                    var cParams = constructor.GetParameters();
                    var tParams = cParams.Select(x => x.ParameterType).ToArray();

                    paramValues = new Object[cParams.Length];
                    for (int i = 0; i < cParams.Length; i++)
                    {
                        Type p = cParams[i].ParameterType;
                        paramValues[i] = default;
                    }
                }
                return Activator.CreateInstance(type, paramValues);
            }
        }

        public static T NewInstance<T>(this Type type, Object[] paramValues)
        {
            return (T)NewInstance(type, paramValues);
        }

        public static dynamic NewInstance(this Type type, Object[] paramValues)
        {
            if (type.IsGenericType)
            {
                List<Type> typeParams = GetGenericTypeParameter(type);
                Type constructedType = type.MakeGenericType(typeParams.ToArray());
                return Activator.CreateInstance(constructedType, paramValues);
            }
            else
            {
                return Activator.CreateInstance(type, paramValues);
            }
        }

        public static void SetFieldValue<T, V>(this T target, String property, V value)
        {
            var properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.Name.ToLower() == property.ToLower())
                {
                    field.SetValue(target, value);
                    return;
                }
            }
        }

        public static void SetPropertyValue<T, V>(this T target, String property, V value)
        {
            var properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var prop in properties)
            {
                if (prop.Name.ToLower() == property.ToLower())
                {
                    prop.SetValue(target, value);
                    return;
                }
            }
        }
    }
}
