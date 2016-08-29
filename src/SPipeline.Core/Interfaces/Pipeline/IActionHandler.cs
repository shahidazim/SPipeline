namespace SPipeline.Core.Interfaces.Pipeline
{
    using System;

    /// <summary>
    /// Represents the generic action handler.
    /// </summary>
    /// <typeparam name="TActionRequest">The type of the action request.</typeparam>
    /// <typeparam name="TActionResponse">The type of the action response.</typeparam>
    /// <typeparam name="TTranslateRequest">The type of the translate request.</typeparam>
    /// <typeparam name="TTranslateResponse">The type of the translate response.</typeparam>
    public interface IActionHandler<TActionRequest, TActionResponse, in TTranslateRequest, out TTranslateResponse>
        where TActionRequest : IActionRequest
        where TActionResponse : IActionResponse
        where TTranslateRequest : IMessageRequest
        where TTranslateResponse : IMessageResponse
    {
        /// <summary>
        /// Gets the request translator.
        /// </summary>
        /// <value>
        /// The request translator.
        /// </value>
        Func<TTranslateRequest, TActionRequest> RequestTranslator { get; }

        /// <summary>
        /// Gets the response translator.
        /// </summary>
        /// <value>
        /// The response translator.
        /// </value>
        Func<TActionResponse, TTranslateResponse> ResponseTranslator { get; }

        /// <summary>
        /// Executes the specified action request.
        /// </summary>
        /// <param name="actionRequest">The action request.</param>
        /// <returns></returns>
        TActionResponse Execute(TActionRequest actionRequest);
    }

    /// <summary>
    /// Represents the action handler
    /// </summary>
    public interface IActionHandler
    {
        /// <summary>
        /// Executes the specified action request.
        /// </summary>
        /// <param name="actionRequest">The action request.</param>
        /// <returns></returns>
        IActionResponse Execute(IActionRequest actionRequest);

        /// <summary>
        /// Translates the request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        ITranslatable TranslateRequest(ITranslatable source);

        /// <summary>
        /// Translates the response.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        ITranslatable TranslateResponse(ITranslatable source);
    }
}
