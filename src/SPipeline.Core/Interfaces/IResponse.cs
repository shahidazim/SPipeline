namespace SPipeline.Core.Interfaces
{
    using SPipeline.Core.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the response.
    /// </summary>
    /// <seealso cref="SPipeline.Core.Interfaces.ITranslatable" />
    public interface IResponse : ITranslatable
    {
        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        bool HasError { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        IEnumerable<MessageError> Errors { get; }

        /// <summary>
        /// Gets a value indicating whether this instance can continue.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can continue; otherwise, <c>false</c>.
        /// </value>
        bool CanContinue { get; }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        IResponse AddError(MessageError error);

        /// <summary>
        /// Adds the collection of errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        IResponse AddErrors(IEnumerable<MessageError> errors);
    }
}
