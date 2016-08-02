namespace SPipeline.Core.Interfaces
{
    /// <summary>
    /// Represents the message request.
    /// </summary>
    /// <seealso cref="SPipeline.Core.Interfaces.ITranslatable" />
    public interface IMessageRequest : ITranslatable
    {
        /// <summary>
        /// Gets a value indicating whether to clear errors before next handler execution.
        /// </summary>
        /// <value>
        /// <c>true</c> if clear errors before next handler execution; otherwise, <c>false</c>.
        /// </value>
        bool ClearErrorsBeforeNextHandler { get; }
    }
}
