using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Mercury.Language.Extensions.Test.Core
{
    public class CoreTest
    {

        [Test]
        public void CreateObjectWithConstructorTest()
        {
            ValueObject v = CreateObject<ValueObject>(0.5);

            Assert.AreEqual(v.Value, 0.5);
        }

        public T CreateObject<T>(double k)
        {
            T s = System.Core.CreateInstanceFromType(typeof(T), new object[] { k });
            return s;
        }
    }
}
