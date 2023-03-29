using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// Default comparer for dictionary entries in a sorted dictionary.
    /// Entry comparisons only look at keys and uses an externally defined comparer for that.
    /// 
    /// This file is based on part of the C5 Generic Collection Library for C# and CLI
    /// https://github.com/sestoft/C5
    /// See https://github.com/sestoft/C5/blob/master/LICENSE for licensing details.
    /// </summary>
    /// <see href="https://github.com/sestoft/C5/blob/master/C5/Comparers/KeyValuePairComparer.cs"></see>
    [Serializable]
    public class KeyValuePairComparer<TKey, V> : IComparer<KeyValuePair<TKey, V>>
    {
        private readonly IComparer<TKey> comparer;


        /// <summary>
        /// Create an entry comparer for a item comparer of the keys
        /// </summary>
        /// <param name="comparer">Comparer of keys</param>
        public KeyValuePairComparer(IComparer<TKey> comparer)
        {
            this.comparer = comparer ?? throw new NullReferenceException();
        }


        /// <summary>
        /// Compare two entries
        /// </summary>
        /// <param name="entry1">First entry</param>
        /// <param name="entry2">Second entry</param>
        /// <returns>The result of comparing the keys</returns>
        public int Compare(KeyValuePair<TKey, V> entry1, KeyValuePair<TKey, V> entry2)
        {
            return comparer.Compare(entry1.Key, entry2.Key);
        }
    }
}
