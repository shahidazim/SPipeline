namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;
    using System.Linq;
    using System.Text;

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

        public string GetFormattedError()
        {
            var errorMessage = new StringBuilder();
            Errors.Select(x => errorMessage.Append(x.GetFormattedError()));
            return errorMessage.ToString();
        }
    }
}
