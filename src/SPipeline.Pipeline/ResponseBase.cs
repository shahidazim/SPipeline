namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The base implementation for response to provide error handling.
    /// </summary>
    /// <seealso cref="IResponse" />
    public abstract class ResponseBase : IResponse
    {
        // The list of errors
        private readonly List<MessageError> _errors = new List<MessageError>();

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<MessageError> Errors => _errors;

        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError => _errors.Count > 0;

        /// <summary>
        /// Gets a value indicating whether this instance can continue.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can continue; otherwise, <c>false</c>.
        /// </value>
        public bool CanContinue
        {
            get { return _errors.Count == 0 || Errors.All(x => x.CanContinue); }
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public IResponse AddError(MessageError error)
        {
            _errors.Add(error);
            return this;
        }

        /// <summary>
        /// Adds the collection of errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public IResponse AddErrors(IEnumerable<MessageError> errors)
        {
            _errors.AddRange(errors);
            return this;
        }

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
