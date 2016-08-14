namespace SPipeline.Core.Interfaces
{
    /// <summary>
    /// Represents the message receiver
    /// </summary>
    public interface IMessageReceiver
    {
        /// <summary>
        /// Starts the receiver to receive messages.
        /// </summary>
        void Start();

        /// <summary>
        /// Close the receiver's connection.
        /// </summary>
        void Stop();
    }
}
