namespace SPipeline.Core.Interfaces.Pipeline
{
    using System;

    /// <summary>
    /// Represents the message receiver
    /// </summary>
    public interface IMessageReceiver
    {
        /// <summary>
        /// Starts the receiver to receive messages.
        /// </summary>
        void Process();
    }
}
