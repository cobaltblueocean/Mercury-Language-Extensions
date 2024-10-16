using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    internal static class ArgumentCheckerLocal
    {
        /// <summary>
        /// Checks that the specified boolean is true.
        /// <p>
        /// Given the input parameter, this returns normally only if it is true.
        /// This will typically be the result of a caller-specific check.
        /// For example:
        /// <pre>
        ///  ArgumentChecker.IsTrue(collection.contains("value"));
        /// </pre>
        /// </summary>
        /// <param name="validIfTrue">a boolean resulting from testing an argument, may be null</param>
        /// <exception cref="ArgumentException">if the test value is false</exception>
        public static void IsTrue(Boolean validIfTrue)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (!validIfTrue)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Checks that the specified boolean is true.
        /// <p>
        /// Given the input parameter, this returns normally only if it is true.
        /// This will typically be the result of a caller-specific check.
        /// For example:
        /// <pre>
        ///  ArgumentChecker.IsTrue(collection.contains("value"));
        /// </pre>
        /// </summary>
        /// <param name="validIfTrue">a boolean resulting from testing an argument, may be null</param>
        /// <param name="message">the error message, not null</param>
        /// <exception cref="ArgumentException">if the test value is false</exception>
        public static void IsTrue(Boolean validIfTrue, String message)
        {
            // return void, not the parameter, as no need to check a Boolean method parameter
            if (!validIfTrue)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
