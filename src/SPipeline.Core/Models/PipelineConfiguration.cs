namespace SPipeline.Core.Models
{
    public class PipelineConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether to clear errors before next handler execution.
        /// </summary>
        /// <value>
        /// <c>true</c> if clear errors before next handler execution; otherwise, <c>false</c>.
        /// </value>
        public bool ClearErrorsBeforeNextHandler { get; set; }

        /// <summary>
        /// Gets or sets the batch size for parallel handlers, if zero then unlimited.
        /// </summary>
        /// <value>
        /// The batch size for parallel handlers, if zero then unlimited.
        /// </value>
        public int BatchSizeForParallelHandlers { get; set; }
    }
}
