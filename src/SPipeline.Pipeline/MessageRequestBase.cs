namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;

    /// <summary>
    /// The base implementation for message request.
    /// </summary>
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageRequest" />
    public abstract class MessageRequestBase : IMessageRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestBase"/> class.
        /// </summary>
        /// <param name="clearErrorsBeforeNextHandler">if set to <c>true</c> [clear errors before next handler].</param>
        protected MessageRequestBase(bool clearErrorsBeforeNextHandler)
        {
            ClearErrorsBeforeNextHandler = clearErrorsBeforeNextHandler;
        }

        /// <summary>
        /// Gets a value indicating whether to clear errors before next handler execution.
        /// </summary>
        /// <value>
        /// <c>true</c> if clear errors before next handler execution; otherwise, <c>false</c>.
        /// </value>
        public bool ClearErrorsBeforeNextHandler { get; }
    }
}
