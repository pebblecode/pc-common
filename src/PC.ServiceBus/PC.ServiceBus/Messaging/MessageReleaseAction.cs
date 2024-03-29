﻿namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// Sepecfies how the <see cref="Microsoft.ServiceBus.Messaging.BrokeredMessage"/> should be released.
    /// </summary>
    public class MessageReleaseAction
    {
        public static readonly MessageReleaseAction CompleteMessage = new MessageReleaseAction(MessageReleaseActionKind.Complete);
        public static readonly MessageReleaseAction AbandonMessage = new MessageReleaseAction(MessageReleaseActionKind.Abandon);

        protected MessageReleaseAction(MessageReleaseActionKind kind)
        {
            Kind = kind;
        }

        public MessageReleaseActionKind Kind { get; private set; }

        public string DeadLetterReason { get; private set; }

        public string DeadLetterDescription { get; private set; }

        public static MessageReleaseAction DeadLetterMessage(string reason, string description)
        {
            return new MessageReleaseAction(MessageReleaseActionKind.DeadLetter) { DeadLetterReason = reason, DeadLetterDescription = description };
        }
    }

    public enum MessageReleaseActionKind
    {
        Complete,
        Abandon,
        DeadLetter
    }
}
