using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Mercury.Test.Utility;

namespace Mercury.Language.Extensions.Test.Collections
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class ReadOnlySetTest
    {
        [Test]
        public void AddItemTest()
        {
            Assert2.ThrowsException<NotSupportedException>(() =>
            {
                var readOnlySet = new ReadOnlySet<int>(new HashSet<int> { 1, 2, 3 });
                readOnlySet.Add(4);
            });
        }
    }
}
