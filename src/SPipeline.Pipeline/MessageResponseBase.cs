namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;

    /// <summary>
    /// The base implementation for message response.
    /// </summary>
    /// <seealso cref="SPipeline.Pipeline.ResponseBase" />
    /// <seealso cref="IMessageResponse" />
    public abstract class MessageResponseBase : ResponseBase, IMessageResponse
    {
        /// <summary>
        /// Clears the errors.
        /// </summary>
        /// <param name="clearErrorsBeforeNextHandler">if set to <c>true</c> clear errors before next handler execution.</param>
        public void ClearErrors(bool clearErrorsBeforeNextHandler)
        {
            if (clearErrorsBeforeNextHandler)
            {
                _errors.Clear();
            }
        }
    }
}
