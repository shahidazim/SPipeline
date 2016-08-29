namespace SPipeline.Core.Interfaces.Pipeline
{
    /// <summary>
    /// Represents the message response.
    /// </summary>
    /// <seealso cref="IResponse" />
    public interface IMessageResponse : IResponse
    {
        /// <summary>
        /// Clears the errors.
        /// </summary>
        /// <param name="clearErrorsBeforeNextHandler">if set to <c>true</c> clear errors before next handler execution.</param>
        void ClearErrors(bool clearErrorsBeforeNextHandler);
    }
}
