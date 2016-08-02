namespace SPipeline.Core.Exceptions
{
    using System;

    /// <summary>
    /// The exception that is thrown when a pipeline is not registered for particular message.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class PipelineNotRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineNotRegisteredException"/> class.
        /// </summary>
        /// <param name="type">The type which is not registered.</param>
        public PipelineNotRegisteredException
        (
            Type type
        )
        : base($"Handler not found: {type.Name}")
        {
        }
    }
}
