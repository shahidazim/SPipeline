namespace SPipeline.Core.Interfaces.Pipeline
{
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
