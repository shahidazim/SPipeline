namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The base implementation for response to provide error handling.
    /// </summary>
    /// <seealso cref="IResponse" />
    public abstract class ResponseBase : IResponse
    {
        // The list of errors
        protected readonly List<MessageError> _errors = new List<MessageError>();

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
    }
}
