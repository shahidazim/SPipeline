namespace SPipeline.Core.Interfaces.Pipeline
{
    using SPipeline.Core.Models;

    /// <summary>
    /// Represents the message request.
    /// </summary>
    /// <seealso cref="ITranslatable" />
    public interface IMessageRequest : ITranslatable
    {
        /// <summary>
        /// Gets the pipeline configuration.
        /// </summary>
        /// <value>
        /// The pipeline configuration.
        /// </value>
        PipelineConfiguration Configuration { get; }
    }
}
