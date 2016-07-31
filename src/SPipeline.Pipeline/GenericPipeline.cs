namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;

    public class GenericPipeline<TMessageRequest, TMessageResponse> : PipelineBase<TMessageRequest, TMessageResponse>
        where TMessageRequest : IMessageRequest
        where TMessageResponse : IMessageResponse, new()
    {
    }
}
