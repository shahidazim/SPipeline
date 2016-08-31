namespace SPipeline.Pipeline
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Extensions;
    using SPipeline.Core.Interfaces.Pipeline;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The base implementation for pipeline, which provide following functionalities:
    /// * Execute all seqential and parallel handlers
    /// * Add sequential or parallel handlers
    /// </summary>
    /// <typeparam name="TMessageRequest">The type of the message request.</typeparam>
    /// <typeparam name="TMessageResponse">The type of the message response.</typeparam>
    /// <seealso cref="IPipeline" />
    /// <seealso cref="IPipeline{TMessageRequest,TMessageResponse}" />
    public abstract class PipelineBase<TMessageRequest, TMessageResponse> : IPipeline, IPipeline<TMessageRequest, TMessageResponse>
        where TMessageRequest : IMessageRequest
        where TMessageResponse : IMessageResponse, new()
    {
        // Represents the action handlers list
        private readonly List<List<IActionHandler>> _actionHandlersList = new List<List<IActionHandler>>();

        protected readonly ILoggerService loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineBase{TMessageRequest, TMessageResponse}"/> class.
        /// </summary>
        protected PipelineBase(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
            MessageType = typeof(TMessageRequest);
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public Type MessageType { get; }

        /// <summary>
        /// Executes the specified message request.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Executes all.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
        private IEnumerable<TMessageResponse> ExecuteAll(TMessageRequest messageRequest)
        {
            var messageResponses = new List<TMessageResponse>();
            // Iterates all handlers and execute them.
            foreach (var actionHandlers in _actionHandlersList)
            {
                // If it is sequential handler then execute handler sequentially
                if (actionHandlers.Count == 1)
                {
                    messageResponses.Add(ExecuteSequentialHandler(messageRequest, actionHandlers.First()));
                }
                // Otherwise execute handlers asynchronously
                else
                {
                    messageResponses.AddRange(ExecuteParallelHandlers(messageRequest, actionHandlers));
                }

                // If any response contains an error message which represents not to continue then break the loop.
                if (!messageResponses.Any(x => x.CanContinue))
                {
                    break;
                }
            }

            // Return all responses from handlers.
            return messageResponses.AsEnumerable();
        }

        /// <summary>
        /// Executes the sequential handler.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <param name="actionHandler">The action handler.</param>
        /// <returns></returns>
        private TMessageResponse ExecuteSequentialHandler(TMessageRequest messageRequest, IActionHandler actionHandler)
        {
            // Translate the request and execute handler.
            var messageResponse = actionHandler.Execute((IActionRequest)actionHandler.TranslateRequest(messageRequest));

            if (messageResponse.HasError)
            {
                loggerService.Error(messageResponse.GetFormattedError());
            }

            // Return the translated response.
            return (TMessageResponse)actionHandler.TranslateResponse(messageResponse);
        }

        /// <summary>
        /// Executes the parallel handlers.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <param name="actionHandlers">The action handlers.</param>
        /// <returns></returns>
        private IEnumerable<TMessageResponse> ExecuteParallelHandlers(TMessageRequest messageRequest, IEnumerable<IActionHandler> actionHandlers)
        {
            // The lock object to avoid deadlock or race condition.
            var lockObject = new object();
            // List of responses.
            var messageResponses = new List<TMessageResponse>();

            var batchSizeForParallelHandlers = messageRequest.Configuration.BatchSizeForParallelHandlers;
            // If batch size is zero then get the all handlers into a single batch
            if (batchSizeForParallelHandlers == 0)
            {
                batchSizeForParallelHandlers = actionHandlers.Count();
            }

            var batchedParallelHandlers = actionHandlers.Batch(batchSizeForParallelHandlers);

            foreach (var batchedParallelHandler in batchedParallelHandlers)
            {
                // The list of tasks to run in parallel mode
                var tasks = new List<Task>();
                foreach (var actionHandler in batchedParallelHandler)
                {
                    var task = new Task(() =>
                    {
                        // Translate the request and execute handler.
                        var messageResponse = actionHandler.Execute((IActionRequest) actionHandler.TranslateRequest(messageRequest));
                        // Translate the response.
                        var translatedMessageResponse = (TMessageResponse) actionHandler.TranslateResponse(messageResponse);

                        // Add responses synchronously
                        lock (lockObject)
                        {
                            messageResponses.Add(translatedMessageResponse);
                        }

                        if (messageResponse.HasError)
                        {
                            // TODO: Implement error handling
                        }

                        // Clear the errors based on the messsage configuration.
                        translatedMessageResponse.ClearErrors(messageRequest.Configuration.ClearErrorsBeforeNextHandler);
                    });
                    // Start the task and add to tasks collection
                    task.Start();
                    tasks.Add(task);
                }
                // Wait for all tasks to be completed.
                Task.WhenAll(tasks).Wait();
            }

            return messageResponses.AsEnumerable();
        }

        /// <summary>
        /// Adds the sequential handlers.
        /// </summary>
        /// <param name="actionHandlers">The action handlers.</param>
        /// <returns></returns>
        public IPipeline<TMessageRequest, TMessageResponse> AddSequential(params IActionHandler[] actionHandlers)
        {
            foreach (var actionHandler in actionHandlers)
            {
                _actionHandlersList.Add(new List<IActionHandler> { actionHandler });
            }
            return this;
        }

        /// <summary>
        /// Adds the parallel handlers.
        /// </summary>
        /// <param name="actionHandlers">The action handlers.</param>
        /// <returns></returns>
        public IPipeline<TMessageRequest, TMessageResponse> AddParallel(params IActionHandler[] actionHandlers)
        {
            _actionHandlersList.Add(new List<IActionHandler>(actionHandlers));
            return this;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        Type IPipeline.MessageType => MessageType;

        /// <summary>
        /// Executes the specified message request.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
        IMessageResponse IPipeline.Execute(IMessageRequest messageRequest)
        {
            return Execute((TMessageRequest)messageRequest);
        }
    }
}
