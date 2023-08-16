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
using Mercury.Language.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic;

/// <summary>
/// Utility class for Collections
/// </summary>
public class CollectionsUtility
{
    /// <summary>
    /// Create an empty HashSet
    /// </summary>
    /// <typeparam name="T">Type of the HashSet value</typeparam>
    /// <returns>An empty HashSet ith type of T</returns>
    public static HashSet<T> EmptySet<T>()
    {
        return new HashSet<T>();
    }

    /// <summary>
    /// Create a HashSet with given value and type
    /// </summary>
    /// <typeparam name="T">Type of HashSet value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A HashSet with the value provided</returns>
    public static HashSet<T> HashSet<T>(T value)
    {
        return new HashSet<T>(new List<T>() { value });
    }

    /// <summary>
    /// Create a HashSet with given value and type
    /// </summary>
    /// <typeparam name="T">Type of HashSet value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A HashSet with the value provided</returns>
    public static HashSet<T> HashSet<T>(IList<T> value)
    {
        return new HashSet<T>(value);
    }

    /// <summary>
    /// Create a HashSet with given value and type
    /// </summary>
    /// <typeparam name="T">Type of HashSet value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A HashSet with the value provided</returns>
    public static HashSet<T> HashSet<T>(params T[] value)
    {
        return new HashSet<T>(value);
    }

    /// <summary>
    /// Create an empty List
    /// </summary>
    /// <typeparam name="T">Type of the List value</typeparam>
    /// <returns>An empty List ith type of T</returns>
    public static IList<T> EmptyList<T>()
    {
        return new List<T>();
    }

    /// <summary>
    /// Create a List with given value and type
    /// </summary>
    /// <typeparam name="T">Type of List value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A List with the value provided</returns>
    public static IList<T> List<T>(T value)
    {
        return new List<T>(new List<T>() { value });
    }

    /// <summary>
    /// Create a List with given value and type
    /// </summary>
    /// <typeparam name="T">Type of List value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A List with the value provided</returns>
    public static IList<T> List<T>(IList<T> value)
    {
        return new List<T>(value);
    }

    /// <summary>
    /// Create a List with given value and type
    /// </summary>
    /// <typeparam name="T">Type of List value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A List with the value provided</returns>
    public static IList<T> List<T>(params T[] value)
    {
        return new List<T>(value);
    }

    /// <summary>
    /// Create a ReadOnlyCollection with given value and type
    /// </summary>
    /// <typeparam name="T">Type of ReadOnlyCollection value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A ReadOnlyCollection with the value provided</returns>
    public static ReadOnlyCollection<T> Singleton<T>(T value)
    {
        return new ReadOnlyCollection<T>(new List<T>() { value });
    }

    /// <summary>
    /// Create a ReadOnlyCollection with given value and type
    /// </summary>
    /// <typeparam name="T">Type of ReadOnlyCollection value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A ReadOnlyCollection with the value provided</returns>
    public static ReadOnlyCollection<T> Singleton<T>(IList<T> value)
    {
        return new ReadOnlyCollection<T>(value);
    }

    /// <summary>
    /// Create a ReadOnlyCollection with given value and type
    /// </summary>
    /// <typeparam name="T">Type of ReadOnlyCollection value</typeparam>
    /// <param name="value">The value will be assigned</param>
    /// <returns>A ReadOnlyCollection with the value provided</returns>
    public static ReadOnlyCollection<T> Singleton<T>(params T[] value)
    {
        return new ReadOnlyCollection<T>(value);
    }
}
