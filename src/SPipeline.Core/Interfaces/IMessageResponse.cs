namespace SPipeline.Core.Interfaces
{
    /// <summary>
    /// Represents the message response.
    /// </summary>
    /// <seealso cref="SPipeline.Core.Interfaces.IResponse" />
    public interface IMessageResponse : IResponse
    {
        /// <summary>
        /// Clears the errors.
        /// </summary>
        /// <param name="clearErrorsBeforeNextHandler">if set to <c>true</c> clear errors before next handler execution.</param>
        void ClearErrors(bool clearErrorsBeforeNextHandler);
    }
}
