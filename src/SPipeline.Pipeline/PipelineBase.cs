namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class PipelineBase<TMessageRequest, TMessageResponse> : IPipeline, IPipeline<TMessageRequest, TMessageResponse>
        where TMessageRequest : IMessageRequest
        where TMessageResponse : IMessageResponse, new()
    {
        private readonly List<List<IActionHandler>> _actionHandlersList = new List<List<IActionHandler>>();

        protected PipelineBase()
        {
            MessageType = typeof(TMessageRequest);
        }

        public Type MessageType { get; }

        public TMessageResponse Execute(TMessageRequest messageRequest)
        {
            var messageResponses = new List<TMessageResponse>();
            messageResponses.AddRange(ExecuteAll(messageRequest));

            var messageResponse = new TMessageResponse();
            foreach (var res in messageResponses)
            {
                messageResponse.AddErrors(res.Errors);
            }

            return messageResponse;
        }

        private IEnumerable<TMessageResponse> ExecuteAll(TMessageRequest messageRequest)
        {
            var messageResponses = new List<TMessageResponse>();
            foreach (var actionHandlers in _actionHandlersList)
            {
                if (actionHandlers.Count == 1)
                {
                    messageResponses.Add(ExecuteSequentialHandler(messageRequest, actionHandlers.First()));
                }
                else
                {
                    messageResponses.AddRange(ExecuteParallelHandlers(messageRequest, actionHandlers));
                }

                if (!messageResponses.Any(x => x.CanContinue))
                {
                    break;
                }
            }

            return messageResponses.AsEnumerable();
        }

        private static TMessageResponse ExecuteSequentialHandler(TMessageRequest messageRequest, IActionHandler actionHandler)
        {
            var messageResponse = actionHandler.Execute((IActionRequest)actionHandler.TranslateRequest(messageRequest));

            if (messageResponse.HasError)
            {
                // TODO: Implement error handling
            }

            return (TMessageResponse)actionHandler.TranslateResponse(messageResponse);
        }

        private IEnumerable<TMessageResponse> ExecuteParallelHandlers(TMessageRequest messageRequest, IEnumerable<IActionHandler> actionHandlers)
        {
            var lockObject = new object();
            var messageResponses = new List<TMessageResponse>();

            var tasks = new List<Task>();
            foreach (var actionHandler in actionHandlers)
            {
                var task = new Task(() =>
                {
                    var messageResponse = actionHandler.Execute((IActionRequest) actionHandler.TranslateRequest(messageRequest));
                    var translatedMessageResponse = (TMessageResponse) actionHandler.TranslateResponse(messageResponse);
                    lock (lockObject)
                    {
                        messageResponses.Add(translatedMessageResponse);
                    }

                    if (messageResponse.HasError)
                    {
                        // TODO: Implement error handling
                    }

                    translatedMessageResponse.ClearErrors(messageRequest.ClearErrorsBeforeNextHandler);
                });
                task.Start();
                tasks.Add(task);
            }
            Task.WhenAll(tasks).Wait();

            return messageResponses.AsEnumerable();
        }

        public IPipeline<TMessageRequest, TMessageResponse> AddSequential(params IActionHandler[] actionHandlers)
        {
            foreach (var actionHandler in actionHandlers)
            {
                _actionHandlersList.Add(new List<IActionHandler> { actionHandler });
            }
            return this;
        }

        public IPipeline<TMessageRequest, TMessageResponse> AddParallel(params IActionHandler[] actionHandlers)
        {
            _actionHandlersList.Add(new List<IActionHandler>(actionHandlers));
            return this;
        }

        Type IPipeline.MessageType => MessageType;

        IMessageResponse IPipeline.Execute(IMessageRequest messageRequest)
        {
            return Execute((TMessageRequest)messageRequest);
        }
    }
}
