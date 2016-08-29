namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;

    /// <summary>
    /// The base implementation for message request.
    /// </summary>
    /// <seealso cref="IMessageRequest" />
    public abstract class MessageRequestBase : IMessageRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestBase" /> class.
        /// </summary>
        /// <param name="configuration">The pipeline configuration.</param>
        protected MessageRequestBase(PipelineConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the pipeline configuration.
        /// </summary>
        /// <value>
        /// The pipeline configuration.
        /// </value>
        public PipelineConfiguration Configuration { get; }
    }
}
