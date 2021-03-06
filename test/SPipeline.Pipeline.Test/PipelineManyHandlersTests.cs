﻿namespace SPipeline.Pipeline.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using System;
    using System.Threading;

    [TestClass]
    public class PipelineManyHandlersTests
    {
        private static int _count;
        private const int LoopCount = 1000;
        private static readonly object LockObject = new object();

        public class FirstActionRequest : ActionRequestBase
        {
        }

        public class FirstActionResponse : ActionResponseBase
        {
        }

        public class FirstActionHandler<TTranslateRequest, TTranslateResponse>
            : ActionHandlerBase<FirstActionRequest, FirstActionResponse, TTranslateRequest, TTranslateResponse>
            where TTranslateRequest : IMessageRequest
            where TTranslateResponse : IMessageResponse
        {
            public FirstActionHandler(
                Func<TTranslateRequest, FirstActionRequest> requestTranslator,
                Func<FirstActionResponse, TTranslateResponse> responseTranslator)
                : base(requestTranslator, responseTranslator)
            {
            }

            public override FirstActionResponse Execute(FirstActionRequest actionRequest)
            {
                lock (LockObject)
                {
                    _count++;
                }
                Thread.Sleep(10);
                return new FirstActionResponse();
            }
        }

        public class MultipleHandlersParallelRequest : MessageRequestBase
        {
            public MultipleHandlersParallelRequest(PipelineConfiguration configuration) : base(configuration)
            {
            }
        }

        public class MultipleHandlersParallelResponse : MessageResponseBase
        {
        }

        public class MultipleHandlersParallelPipeline : PipelineBase<MultipleHandlersParallelRequest, MultipleHandlersParallelResponse>
        {
            public MultipleHandlersParallelPipeline() : base(new LoggerService("Pipeline"))
            {
                var actionHandlers = new FirstActionHandler<MultipleHandlersParallelRequest, MultipleHandlersParallelResponse>[LoopCount];
                for (var i = 0; i < LoopCount; i++)
                {
                    actionHandlers[i] = new FirstActionHandler<MultipleHandlersParallelRequest, MultipleHandlersParallelResponse>(source => new FirstActionRequest(), source => new MultipleHandlersParallelResponse());
                }
                AddParallel(actionHandlers);
            }
        }

        [TestMethod]
        public void Pipeline_Parallel_Handlers_ResponseTypeIsValid()
        {
            var pipeline = new MultipleHandlersParallelPipeline();
            var response = pipeline.Execute(new MultipleHandlersParallelRequest(new PipelineConfiguration { ClearErrorsBeforeNextHandler = false, BatchSizeForParallelHandlers = 100 }));
            Assert.IsInstanceOfType(response, typeof(MultipleHandlersParallelResponse));
            Assert.AreEqual(LoopCount * 1, _count);
        }
    }
}
