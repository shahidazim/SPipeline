namespace SPipeline.Core.Interfaces
{
    using SPipeline.Core.Models;

    /// <summary>
    /// Represents the message request.
    /// </summary>
    /// <seealso cref="SPipeline.Core.Interfaces.ITranslatable" />
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
