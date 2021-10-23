using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cern.Colt.Matrix.Implementation;

namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        #region Extension for IDictionary<T1, T2>
        public static ReadOnlyDictionary<T1, T2> ToReadOnlyDictionary<T1, T2>(this IDictionary<T1, T2> originalDictionary)
        {
            return new ReadOnlyDictionary<T1, T2>(originalDictionary);
        }
        #endregion
    }
}
