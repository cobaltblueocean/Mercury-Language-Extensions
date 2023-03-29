using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Extensions;

namespace Mercury.Language.Exceptions
{
    public class MatrixDimensionMismatchException : System.Exception
    {
        
        

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public MatrixDimensionMismatchException(int wrongRowDim, int wrongColDim, int expectedRowDim, int expectedColDim) : base(String.Format(LocalizedResources.Instance().DIMENSIONS_MISMATCH_2x2, wrongRowDim, wrongColDim, expectedRowDim, expectedColDim))
        {

        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public MatrixDimensionMismatchException(String message) : base(message)
        {

        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public MatrixDimensionMismatchException(String message, System.Exception cause)
            : base(message, cause)
        {

        }
    }
}
