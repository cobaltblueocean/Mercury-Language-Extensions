using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Exception
{
    /// <summary>
    /// Runtime exception used when loading data, and the data is not found.
    /// A typical use case is when loading data by unique identifier, and the identifier is not found.
    /// </summary>
    public class DataNotFoundException : System.Exception
    {
        
        

                /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public DataNotFoundException(String message): base(message)
        {
           
        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public DataNotFoundException(String message, System.Exception cause)
            : base(message, cause)
        {
            
        }
    }
}
