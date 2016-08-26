namespace SPipeline.Core.Interfaces
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
        void Start();

        /// <summary>
        /// Close the receiver's connection.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets or sets the start callback.
        /// </summary>
        /// <value>
        /// The start callback.
        /// </value>
        Action StartCallback { get; set; }

        /// <summary>
        /// Gets or sets the stop callback.
        /// </summary>
        /// <value>
        /// The stop callback.
        /// </value>
        Action StopCallback { get; set; }
    }
}
