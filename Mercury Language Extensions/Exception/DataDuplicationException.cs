using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Exception
{
    /// <summary>
    /// Runtime exception used to indicate that the action would create a duplicate.
    /// A typical use case is when adding data and a similar item already exists.
    /// </summary>
    public class DataDuplicationException : System.Exception
    {
        
        

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public DataDuplicationException(String message): base(message)
        {
           
        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public DataDuplicationException(String message, System.Exception cause)
            : base(message, cause)
        {
            
        }
    }
}
