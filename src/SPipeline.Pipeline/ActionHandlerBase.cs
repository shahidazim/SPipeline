namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;
    using System;

    public abstract class ActionHandlerBase<TActionRequest, TActionResponse, TTranslateRequest, TTranslateResponse>
        : IActionHandler, IActionHandler<TActionRequest, TActionResponse, TTranslateRequest, TTranslateResponse>
        where TActionRequest : IActionRequest
        where TActionResponse : IActionResponse
        where TTranslateRequest : IMessageRequest
        where TTranslateResponse : IMessageResponse
    {
        protected ActionHandlerBase(Func<TTranslateRequest, TActionRequest> requestTranslator, Func<TActionResponse, TTranslateResponse> responseTranslator)
        {
            RequestTranslator = requestTranslator;
            ResponseTranslator = responseTranslator;
        }

        public Func<TTranslateRequest, TActionRequest> RequestTranslator { get; }

        public Func<TActionResponse, TTranslateResponse> ResponseTranslator { get; }

        public virtual TActionResponse Execute(TActionRequest actionRequest)
        {
            throw new NotImplementedException();
        }

        IActionResponse IActionHandler.Execute(IActionRequest actionRequest)
        {
            return Execute((TActionRequest)actionRequest);
        }

        ITranslatable IActionHandler.TranslateRequest(ITranslatable source)
        {
            return RequestTranslator((TTranslateRequest)source);
        }

        ITranslatable IActionHandler.TranslateResponse(ITranslatable source)
        {
            return ResponseTranslator((TActionResponse)source);
        }
    }
}
