namespace SPipeline.Core.Interfaces
{
    using System;

    public interface IActionHandler<TActionRequest, TActionResponse, in TTranslateRequest, out TTranslateResponse>
        where TActionRequest : IActionRequest
        where TActionResponse : IActionResponse
        where TTranslateRequest : IMessageRequest
        where TTranslateResponse : IMessageResponse
    {
        Func<TTranslateRequest, TActionRequest> RequestTranslator { get; }

        Func<TActionResponse, TTranslateResponse> ResponseTranslator { get; }

        TActionResponse Execute(TActionRequest actionRequest);
    }

    public interface IActionHandler
    {
        IActionResponse Execute(IActionRequest actionRequest);

        ITranslatable TranslateRequest(ITranslatable source);

        ITranslatable TranslateResponse(ITranslatable source);
    }
}
