namespace SPipeline.Core.Interfaces.Pipeline
{
    using System;

    /// <summary>
    /// Represents the message receiver
    /// </summary>
    public interface IMessageReceiver
    {
        /// <summary>
        /// Receiver messages from queue and process them
        /// </summary>
        void Process();
    }
}
