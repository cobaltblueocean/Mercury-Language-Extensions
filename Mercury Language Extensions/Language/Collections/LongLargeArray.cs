using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class LongLargeArray : LargeArray<Int64>
    {
        public LongLargeArray(long capacity) : base(capacity)
        {
        }

        public LongLargeArray(long[] data):base(data.Length)
        {
            Load(data);
        }

        public void Load(long[] data)
        {
            if (data.Length > base.Capacity)
                throw new IndexOutOfRangeException();

            for(int i= 0; i<data.Length;i++)
            {
                this[i] = data[i];
            }
        }
    }
}
