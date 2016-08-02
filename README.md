# SPipeline
SPipeline is .Net based pipeline solution with Sequential and Parallel Handlers

![](images/Pipeline.png)

## Key Concepts

**Pipeline** - The pipeline is the main execution block which execute connected action handlers in parallel and/or sequential form. 

**Pipeline Parameters** - The request message is the initial parameter to the pipeline; and the response message is final result from pipeline, which could contain the final value or error messages.

**Action Handler** - The action handler is the actual unit of code to be executed by the pipeline.

**Action Handler Parameters** - The request is parameter to the action handler; and the response message is result from the action handler, which could contain the final value or error messages (similar to pipeline).

**Translator** - The translator function is required to convert pipeline request message to action handler request, and action handler response to pipeline response message.

## Pipeline Message Flow

![](images/PipelineMessageFlow.png)


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

