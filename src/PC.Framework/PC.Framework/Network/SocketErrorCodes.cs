using System;
using System.Collections.Generic;
using System.Text;

namespace PebbleCode.Framework
{
    /// <summary>
    /// Error codes for socket exceptions
    /// </summary>
    public static class SocketErrorCodes
    {
        /// <summary>
        /// WSAEINTR: 10004
        /// Interrupted function call.
        /// A blocking operation was interrupted by a call to WSACancelBlockingCall.
        /// </summary>
        public const int BLOCKING_OPERATION_INTERRUPTED = 10004;

        /// <summary>
        /// WSAECONNABORTED: 10053
        /// Software caused connection abort.
        /// An established connection was aborted by the software in your host computer, possibly 
        /// due to a data transmission time-out or protocol error.
        /// </summary>
        public const int SOFTWARE_CAUSED_CONNECTION_ABORT = 10053;

        /// <summary>
        /// WSAECONNRESET: 10054
        /// Connection reset by peer.
        /// An existing connection was forcibly closed by the remote host. This normally results if 
        /// the peer application on the remote host is suddenly stopped, the host is rebooted, the 
        /// host or remote network interface is disabled, or the remote host uses a hard close (see 
        /// setsockopt for more information on the SO_LINGER option on the remote socket). This error 
        /// may also result if a connection was broken due to keep-alive activity detecting a failure 
        /// while one or more operations are in progress. Operations that were in progress fail with 
        /// WSAENETRESET. Subsequent operations fail with WSAECONNRESET.
        /// </summary>
        public const int EXISTING_CONNECTION_FORCIBLY_CLOSED = 10054;
    }
}
