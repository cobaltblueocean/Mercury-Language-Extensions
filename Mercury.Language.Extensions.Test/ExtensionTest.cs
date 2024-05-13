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

namespace Mercury.Language.Extensions.Test
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

        [Test]
        public void ArrayCopyOfTest()
        {
            int N = 256;
            double[] h = new double[N];

            AutoParallel.AutoParallelFor(0, N, (i) =>
            {
                h[i] = (double)i;
            });

            double[] a = h.CopyOf(2 * N);

            Assert2.AreEqual(h.Length * 2, a.Length);

            for (int i = 0; i < N; i++)
            {
                Assert2.AreEqual(h[i], a[i]);
            }

            for (int i = N; i < N * 2; i++)
            {
                Assert2.AreEqual(a[i], 0);
            }
        }

        [Test]
        public void ArrayGetSafeTest()
        {
            int N = 16;
            Dummy[] dummy = new Dummy[N * 2];

            AutoParallel.AutoParallelFor(0, N, (i) =>
            {
                dummy[i] = new Dummy(i.ToString(), i);
            });

            Assert2.ThrowsException<IndexOutOfRangeException>(() =>
            {
                var data1 = dummy[N * 2];
            });

            var data2 = dummy.GetSafe(N * 2);

            Assert2.IsTrue(data2 == null);
        }

        [Test]
        public void GnericTypeTest()
        {
            var types = new Type[] { typeof(Dummy), typeof(String), typeof(Double) };
            var genericDummy = new DummyGeneric<Dummy, String, Double>();
            var genericDummy2 = new DummyGeneric<Dummy2, String, Double>();

            Assert2.IsTrue(genericDummy.GetType().HasGenericTypeArguments(types));
            Assert2.IsTrue(genericDummy2.GetType().HasGenericTypeArguments(types));
        }
    }

    public class DummyGeneric<Dummy, String, Double>
    {
        public DummyGeneric() { }
    }

    public class Dummy2 : Dummy
    {
        public Dummy2(String name, Double value):base(name, value) { }
    }

    public class Dummy
    {
        private String _name;
        private double _value;

        public Dummy(String name, Double value)
        {
            _name = name;
            _value = value;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
