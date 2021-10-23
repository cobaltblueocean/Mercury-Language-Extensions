using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Exception
{
    public class NonSymmetricMatrixException : System.Exception
    {
        
        

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public NonSymmetricMatrixException(int row, int column, double relativeTolerance) : base(String.Format(LocalizedResources.Instance().NON_SYMMETRIC_MATRIX, row, column, relativeTolerance))
        {

        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public NonSymmetricMatrixException(String message): base(message)
        {

        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public NonSymmetricMatrixException(String message, System.Exception cause)
            : base(message, cause)
        {

        }

    }
}
