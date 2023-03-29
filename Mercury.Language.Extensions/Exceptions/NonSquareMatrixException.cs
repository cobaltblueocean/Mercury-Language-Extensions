using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Extensions;

namespace Mercury.Language.Exceptions
{
    /// <summary>
    /// Exception to be thrown when a symmetric matrix is expected.
    /// </summary>
    public class NonSquareMatrixException : System.Exception
    {
        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public NonSquareMatrixException(int row, int column) : base(String.Format(LocalizedResources.Instance().NON_SQUARE_MATRIX, row, column))
        {

        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public NonSquareMatrixException(String message): base(message)
        {

        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public NonSquareMatrixException(String message, System.Exception cause)
            : base(message, cause)
        {

        }

    }
}
