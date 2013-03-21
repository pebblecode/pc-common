using Microsoft.ServiceBus.Messaging;
using PebbleCode.Framework.Logging;

namespace PC.ServiceBus.Utils
{
    public static class BrokeredMessageExtensions
    {
        public static bool SafeComplete(this BrokeredMessage message)
        {
            try
            {
                message.Complete();

                return true;
            }
            catch (MessageLockLostException)
            {
                //The lock is lost, but we dont want to break the recieve loop, so we are ignoring it, because the message will be received
                //again at later point
            }
            catch (MessagingException)
            {
                //The messaging exception is something we ignore, too. If Complete() fails with this exception we just receive the next message, which 
                //could be the same one
            }

            return false;
        }

        public static bool SafeAbandon(this BrokeredMessage message)
        {
            try
            {
                message.Abandon();

                return true;
            }
            catch (MessageLockLostException)
            {
                //The lock is lost, but we dont want to break the recieve loop, so we are ignoring it, because the message will be received
                //again at later point
            }
            catch (MessagingException)
            {
                //The messaging exception is something we ignore, too. If Complete() fails with this exception we just receive the next message, which 
                //could be the same one
            }

            return false;
        }

        public static bool SafeDeadLetter(this BrokeredMessage message, string deadLetterReason, string deadLetterErrorDescription)
        {
            try
            {
                message.DeadLetter(deadLetterReason, deadLetterErrorDescription);

                Logger.WriteWarning(string.Format("Dead lettering message {0}. Reason : {1} Error description : {2}",
                    message.MessageId,
                    deadLetterReason,
                    deadLetterErrorDescription),
                    "ServiceBus");
                return true;
            }
            catch (MessageLockLostException)
            {
                //The lock is lost, but we dont want to break the recieve loop, so we are ignoring it, because the message will be received
                //again at later point
            }
            catch (MessagingException)
            {
                //The messaging exception is something we ignore, too. If Complete() fails with this exception we just receive the next message, which 
                //could be the same one
            }

            return false;
        }
    }
}
