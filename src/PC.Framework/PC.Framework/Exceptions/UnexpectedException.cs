using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PebbleCode.Framework.Exceptions
{
    public class UnexpectedException : PebbleCodeException
    {
        /// <summary>
        /// Construct with no inner ex or message
        /// </summary>
        public UnexpectedException() 
            : base() { }

        /// <summary>
        /// Construct with a message
        /// </summary>
        /// <param name="message"></param>
        public UnexpectedException(string message) 
            : base(message) { }

        /// <summary>
        /// Construct with a message using format args
        /// </summary>
        /// <param name="message"></param>
        public UnexpectedException(string message, params object[] formatArgs)
            : base(message, formatArgs) { }

        /// <summary>
        /// Construct as part of deserialisation
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnexpectedException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        /// <summary>
        /// Construct with a message and inner ex
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnexpectedException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
