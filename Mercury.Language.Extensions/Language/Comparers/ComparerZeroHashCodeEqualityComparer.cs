using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// An equalityComparer compatible with a given comparer. All hash codes are 0,
    /// meaning that anything based on hash codes will be quite inefficient.
    /// <para><b>Note: this will give a new EqualityComparer each time created!</b></para>
    ///  
    /// This file is based on part of the C5 Generic Collection Library for C# and CLI
    /// https://github.com/sestoft/C5
    /// See https://github.com/sestoft/C5/blob/master/LICENSE for licensing details.
    /// </summary>
    /// <see href="https://github.com/sestoft/C5/blob/master/C5/Comparers/ComparerZeroHashCodeEqualityComparer.cs"></see>
    public class ComparerZeroHashCodeEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly IComparer<T> comparer;
        /// <summary>
        /// Create a trivial <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> compatible with the
        /// <see cref="T:System.Collections.Generic.IComparer`1"/> <code>comparer</code>
        /// </summary>
        /// <param name="comparer"></param>
        public ComparerZeroHashCodeEqualityComparer(IComparer<T> comparer)
        {
            this.comparer = comparer ?? throw new NullReferenceException("Comparer cannot be null");
        }
        /// <summary>
        /// A trivial, inefficient hash function. Compatible with any equality relation.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>0</returns>
        public int GetHashCode(T item) { return 0; }
        /// <summary>
        /// Equality of two items as defined by the comparer.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public bool Equals(T item1, T item2) { return comparer.Compare(item1, item2) == 0; }
    }
}
