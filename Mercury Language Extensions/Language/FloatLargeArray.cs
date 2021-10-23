using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class FloatLargeArray : LargeArray<Single>
    {
        public FloatLargeArray(long capacity) : base(capacity)
        {
        }

        public FloatLargeArray(Single[] data):base(data.Length)
        {
            Load(data);
        }

        public void Load(Single[] data)
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
