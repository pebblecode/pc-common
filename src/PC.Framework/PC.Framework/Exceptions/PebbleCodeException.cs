using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PebbleCode.Framework.Exceptions
{
    /// <summary>
    /// Base class for all exceptions definied in PebbleCode(PC) namespace
    /// </summary>
    public abstract class PebbleCodeException : Exception
    {
        /// <summary>
        /// Construct with no inner ex or message
        /// </summary>
        public PebbleCodeException() 
            : base() { }

        /// <summary>
        /// Construct with a message
        /// </summary>
        /// <param name="message"></param>
        public PebbleCodeException(string message) 
            : base (message) {}
        
        /// <summary>
        /// Construct with a message using format args
        /// </summary>
        /// <param name="message"></param>
        public PebbleCodeException(string message, params object[] formatArgs) 
            : base(string.Format(message, formatArgs)) { }

        /// <summary>
        /// Construct as part of deserialisation
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PebbleCodeException(SerializationInfo info, StreamingContext context) 
            : base (info, context) {}
        
        /// <summary>
        /// Construct with a message and inner ex
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PebbleCodeException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
