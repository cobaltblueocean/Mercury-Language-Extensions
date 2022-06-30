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
using NUnit.Framework;
using Mercury.Test.Utility;

namespace Mercury_Language_Extensions.Test
{
    /// <summary>
    /// ExtensionTest Description
    /// </summary>
    public class ExtensionTest
    {
        [Test]
        public void ConvertArrayTypeTest()
        {
            int n = 10;
            float?[] X_OBJECT = new float?[n];
            Double?[] X_OBJECT_DOUBLE = new Double?[n];
            float?[] X_OBJECT_CONVERTED = new float?[n];
            float x;
            float? xFloat;
            for (int i = 0; i < 5; i++)
            {
                x = 2 * i;
                xFloat = x.ToNullableArray()[0];
                X_OBJECT[i] = xFloat;
                X_OBJECT_DOUBLE[i] = x;
            }
            for (int i = 5, j = 0; i < n; i++, j++)
            {
                x = 2 * j + 1;
                xFloat = x.ToNullableArray()[0];
                X_OBJECT[i] = xFloat;
                X_OBJECT_DOUBLE[i] = x;
            }
            X_OBJECT_CONVERTED = X_OBJECT_DOUBLE.ConvertTypeArray<double?, float?>();

            Assert2.ArrayEquals(X_OBJECT, X_OBJECT_CONVERTED);

            X_OBJECT = X_OBJECT.Sort();
            X_OBJECT_CONVERTED = X_OBJECT_CONVERTED.Sort();

            Assert2.ArrayEquals(X_OBJECT, X_OBJECT_CONVERTED);
        }
    }
}
