# SPipeline
SPipeline is .Net based pipeline solution with Sequential and Parallel Handlers

![](images/Pipeline.png)

## Example - Generic Pipeline

	// Action Request
    public class GenericActionRequest : ActionRequestBase
    {
    }

	// Action Reponse
    public class GenericActionResponse : ActionResponseBase
    {
    }

	// Action Handler
    public class GenericActionHandler<TTranslateRequest, TTranslateResponse>
        : ActionHandlerBase<GenericActionRequest, GenericActionResponse, TTranslateRequest, TTranslateResponse>
        where TTranslateRequest : IMessageRequest
        where TTranslateResponse : IMessageResponse
    {
        public GenericActionHandler(
            Func<TTranslateRequest, GenericActionRequest> requestTranslator,
            Func<GenericActionResponse, TTranslateResponse> responseTranslator)
            : base(requestTranslator, responseTranslator)
        {
        }

        public override GenericActionResponse Execute(GenericActionRequest actionRequest)
        {
            return new GenericActionResponse();
        }
    }

	// Pipeline Request
    public class GenericPipelineRequest : MessageRequestBase
    {
        public GenericPipelineRequest(bool clearErrorsBeforeNextHandler) : base(clearErrorsBeforeNextHandler)
        {
        }
    }

	// Pipeline Response
    public class GenericPipelineResponse : MessageResponseBase
    {
    }

	// Create pipeline
    var pipeline = new GenericPipeline<GenericPipelineRequest, GenericPipelineResponse>();
	// Add action handler
    pipeline.AddSequential(
        new GenericActionHandler<GenericPipelineRequest, GenericPipelineResponse>(req => new GenericActionRequest(), res => new GenericPipelineResponse()));
	// Execute pipeline
    var response = pipeline.Execute(new GenericPipelineRequest(false));

