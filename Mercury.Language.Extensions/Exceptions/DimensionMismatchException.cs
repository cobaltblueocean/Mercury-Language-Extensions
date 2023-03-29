using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Extensions;

namespace Mercury.Language.Exceptions
{
    /// <summary>
    /// Exception to be thrown when two dimensions differ.
    /// </summary>
    public class DimensionMismatchException : InvalidOperationException
    {
        /// <summary>
        /// Creates an exception
        /// </summary>
        public DimensionMismatchException(): base()
        { }

        /// <summary>
        /// Creates an exception
        /// </summary>
        public DimensionMismatchException(int wrong, int expected) : base(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_SIMPLE, wrong, expected))
        { }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public DimensionMismatchException(String message):base(message)
        { }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public DimensionMismatchException(String message, System.Exception cause) : base(message, cause)
        { }
    }
}
