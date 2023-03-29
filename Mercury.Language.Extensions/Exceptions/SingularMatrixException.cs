using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Extensions;

namespace Mercury.Language.Exceptions
{
    /// <summary>
    /// Exception to be thrown when a non-singular matrix is expected.
    /// </summary>
    public class SingularMatrixException : ArithmeticException
    {
        /// <summary>
        /// Creates an exception
        /// </summary>
        public SingularMatrixException():base(LocalizedResources.Instance().SINGULAR_MATRIX)
        { }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public SingularMatrixException(String message):base(message)
        { }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public SingularMatrixException(String message, System.Exception cause) : base(message, cause)
        { }
    }
}
