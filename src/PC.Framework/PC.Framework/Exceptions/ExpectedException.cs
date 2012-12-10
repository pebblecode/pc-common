using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PebbleCode.Framework.Exceptions
{
    /// <summary>
    /// Used for intentional exceptions that should be caught and handled as expected situations
    /// </summary>
    public class ExpectedException : PebbleCodeException
    {
        /// <summary>
        /// Construct with no inner ex or message
        /// </summary>
        public ExpectedException() 
            : base() { }

        /// <summary>
        /// Construct with a message
        /// </summary>
        /// <param name="message"></param>
        public ExpectedException(string message) 
            : base(message) { }

        /// <summary>
        /// Construct with a message using format args
        /// </summary>
        /// <param name="message"></param>
        public ExpectedException(string message, params object[] formatArgs)
            : base(message, formatArgs) { }

        /// <summary>
        /// Construct as part of deserialisation
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ExpectedException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        /// <summary>
        /// Construct with a message and inner ex
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ExpectedException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
