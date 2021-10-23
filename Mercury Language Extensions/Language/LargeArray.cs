using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Log;

namespace System
{
    // Goal: create an array that allows for a number of elements > Int.MaxValue
    public class LargeArray<T> : IEnumerable<T>
    where T : struct
    {
        protected static int LARGEST_32BIT_INDEX = 1073741824; // 2^30;

        public static int arysize = (Int32.MaxValue >> 4) / Marshal.SizeOf<T>();

        public readonly long Capacity;
        private Boolean isConstant = true;

        private readonly T[][] content;

        public Boolean IsLarge
        {
            get
            {
                return Capacity > LARGEST_32BIT_INDEX ? true : false;
            }
        }

        public Boolean IsConstant
        {
            get { return isConstant; }
        }

        public T this[long index]
        {
            get
            {
                if (index < 0 || index >= Capacity)
                    throw new IndexOutOfRangeException();
                int chunk = (int)(index / arysize);
                int offset = (int)(index % arysize);
                return content[chunk][offset];
            }
            set
            {
                if (index < 0 || index >= Capacity)
                    throw new IndexOutOfRangeException();
                int chunk = (int)(index / arysize);
                int offset = (int)(index % arysize);
                content[chunk][offset] = value;
            }
        }

        public LargeArray(long capacity)
        {
            Capacity = capacity;
            int nChunks = (int)(capacity / arysize);
            int nRemainder = (int)(capacity % arysize);

            if (nRemainder == 0)
                content = new T[nChunks][];
            else
                content = new T[nChunks + 1][];

            for (int i = 0; i < nChunks; i++)
                content[i] = new T[arysize];
            if (nRemainder > 0)
                content[content.Length - 1] = new T[nRemainder];
        }

        public T[] ToArray()
        {
            if (Capacity > LARGEST_32BIT_INDEX)
                throw new IndexOutOfRangeException();

            T[] val = new T[Capacity];
            var size = content.GetLength(1);

            for (int i = 0; i < content.GetLength(0); i++)
            {
                Array.Copy(content[i], 0, val, size * i, size);
            }

            return val;
        }

        public static int MaxSizeOf32bitArray
        {
            get { return LARGEST_32BIT_INDEX; }
        }

        public static void ArrayCopy(LargeArray<T> source, long srcPos, LargeArray<T> destination, long destPos, long Length)
        {
            if (srcPos < 0 || srcPos >= source.Capacity)
            {
                throw new IndexOutOfRangeException("srcPos < 0 || srcPos >= source.Capacity");
            }
            if (destPos < 0 || destPos >= destination.Capacity)
            {
                throw new IndexOutOfRangeException("destPos < 0 || destPos >= destination.Capacity");
            }
            if (Length < 0)
            {
                throw new ArgumentException("Length < 0");
            }
            if (destination.IsConstant)
            {
                throw new ArgumentException("Constant arrays cannot be modified.");
            }

            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (nthreads < 2 || Length < 100000)
            {
                for (long i = srcPos, j = destPos; i < srcPos + Length; i++, j++)
                {
                    destination[j] = source[i];
                }
            }
            else
            {
                long k = Length / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int j = 0; j < nthreads; j++)
                {
                    long firstIdx = j * k;
                    long lastIdx = (j == nthreads - 1) ? Length : firstIdx + k;
                    taskArray[j] = Task.Factory.StartNew(() =>
                    {
                        {
                            for (long l = firstIdx; l < lastIdx; l++)
                            {
                                destination[destPos + l] = source[srcPos + l];
                            }
                        }
                    });
                }
            try {
                    Task.WaitAll(taskArray);
                
            } catch (TaskCanceledException ex) {
                    Logger.Information(ex.ToString());

                    for (long i = srcPos, j = destPos; i<srcPos + Length; i++, j++) {
                        destination[j] = source[i];
                }
            }
        }
        }


        public IEnumerator<T> GetEnumerator()
        {
            return content.SelectMany(c => c).GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }

    /// <summary>
    /// Supported types of large arrays.
    /// </summary>
    public enum LargeArrayType
    {
        LOGIC,
        BYTE,
        SHORT,
        INT,
        LONG,
        FLOAT,
        DOUBLE,
        COMPLEX_FLOAT,
        COMPLEX_DOUBLE,
        STRING, OBJECT
    }

    public static class LargeArrayTypeMethods
    {
        public static Int64 SizeOf(this LargeArrayType lat)
        {
            switch (lat)
            {
                case LargeArrayType.LOGIC:
                case LargeArrayType.BYTE:
                    return 1;
                case LargeArrayType.SHORT:
                    return 2;
                case LargeArrayType.INT:
                    return 4;
                case LargeArrayType.LONG:
                    return 8;
                case LargeArrayType.FLOAT:
                    return 4;
                case LargeArrayType.DOUBLE:
                    return 8;
                case LargeArrayType.COMPLEX_FLOAT:
                    return 4;
                case LargeArrayType.COMPLEX_DOUBLE:
                    return 8;
                case LargeArrayType.STRING:
                    return 1;
                case LargeArrayType.OBJECT:
                    return 1;
            }
            return 0;
        }
    }


}
