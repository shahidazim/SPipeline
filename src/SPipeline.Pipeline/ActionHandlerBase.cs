namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;
    using System;

    /// <summary>
    /// The base implementation for action handler for translators and message execution.
    /// </summary>
    /// <typeparam name="TActionRequest">The type of the action request.</typeparam>
    /// <typeparam name="TActionResponse">The type of the action response.</typeparam>
    /// <typeparam name="TTranslateRequest">The type of the translate request.</typeparam>
    /// <typeparam name="TTranslateResponse">The type of the translate response.</typeparam>
    /// <seealso cref="IActionHandler" />
    /// <seealso cref="IActionHandler{TActionRequest,TActionResponse,TTranslateRequest,TTranslateResponse}" />
    public abstract class ActionHandlerBase<TActionRequest, TActionResponse, TTranslateRequest, TTranslateResponse>
        : IActionHandler, IActionHandler<TActionRequest, TActionResponse, TTranslateRequest, TTranslateResponse>
        where TActionRequest : IActionRequest
        where TActionResponse : IActionResponse
        where TTranslateRequest : IMessageRequest
        where TTranslateResponse : IMessageResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionHandlerBase{TActionRequest, TActionResponse, TTranslateRequest, TTranslateResponse}"/> class.
        /// </summary>
        /// <param name="requestTranslator">The request translator.</param>
        /// <param name="responseTranslator">The response translator.</param>
        protected ActionHandlerBase(
            Func<TTranslateRequest, TActionRequest> requestTranslator,
            Func<TActionResponse, TTranslateResponse> responseTranslator)
        {
            RequestTranslator = requestTranslator;
            ResponseTranslator = responseTranslator;
        }

        /// <summary>
        /// Gets the request translator.
        /// </summary>
        /// <value>
        /// The request translator.
        /// </value>
        public Func<TTranslateRequest, TActionRequest> RequestTranslator { get; }

        /// <summary>
        /// Gets the response translator.
        /// </summary>
        /// <value>
        /// The response translator.
        /// </value>
        public Func<TActionResponse, TTranslateResponse> ResponseTranslator { get; }

        /// <summary>
        /// Executes the specified action request.
        /// </summary>
        /// <param name="actionRequest">The action request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual TActionResponse Execute(TActionRequest actionRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the specified action request.
        /// </summary>
        /// <param name="actionRequest">The action request.</param>
        /// <returns></returns>
        IActionResponse IActionHandler.Execute(IActionRequest actionRequest)
        {
            return Execute((TActionRequest)actionRequest);
        }

        /// <summary>
        /// Translates the request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        ITranslatable IActionHandler.TranslateRequest(ITranslatable source)
        {
            return RequestTranslator((TTranslateRequest)source);
        }

        /// <summary>
        /// Translates the response.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        ITranslatable IActionHandler.TranslateResponse(ITranslatable source)
        {
            return ResponseTranslator((TActionResponse)source);
        }
    }
}
