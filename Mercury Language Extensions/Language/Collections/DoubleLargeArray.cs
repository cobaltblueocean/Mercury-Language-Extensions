using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class DoubleLargeArray : LargeArray<Double>
    {
        public DoubleLargeArray(long capacity) : base(capacity)
        {
        }

        public DoubleLargeArray(Double[] data):base(data.Length)
        {
            Load(data);
        }

        public void Load(Double[] data)
        {
            if (data.Length > base.Capacity)
                throw new IndexOutOfRangeException();

            for (int i = 0; i < data.Length; i++)
            {
                this[i] = data[i];
            }
        }

    }
}
