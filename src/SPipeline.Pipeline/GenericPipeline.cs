namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;

    /// <summary>
    /// The generic implementation for pipeline, which can be used instead of creating a custom pipeline.
    /// </summary>
    /// <typeparam name="TMessageRequest">The type of the message request.</typeparam>
    /// <typeparam name="TMessageResponse">The type of the message response.</typeparam>
    /// <seealso cref="SPipeline.Pipeline.PipelineBase{TMessageRequest, TMessageResponse}" />
    public class GenericPipeline<TMessageRequest, TMessageResponse> : PipelineBase<TMessageRequest, TMessageResponse>
        where TMessageRequest : IMessageRequest
        where TMessageResponse : IMessageResponse, new()
    {
    }
}
