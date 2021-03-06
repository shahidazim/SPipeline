﻿namespace SPipeline.Pipeline.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;

    [TestClass]
    public class PipelineMultipleSequentialParallelHandlerExecutionTests
    {
        private static int _count;
        private static readonly object LockObject = new object();

        public class FirstActionRequest : ActionRequestBase
        {
        }

        public class FirstActionResponse : ActionResponseBase
        {
        }

        public class FirstActionHandler
            : ActionHandlerBase<FirstActionRequest, FirstActionResponse, MultiHandlersRequest, MultiHandlersResponse>
        {
            public FirstActionHandler()
                : base(source => new FirstActionRequest(), source => new MultiHandlersResponse())
            {
            }

            public override FirstActionResponse Execute(FirstActionRequest actionRequest)
            {
                lock (LockObject)
                {
                    _count++;
                }
                return new FirstActionResponse();
            }
        }

        public class MultiHandlersRequest : MessageRequestBase
        {
            public MultiHandlersRequest(PipelineConfiguration configuration) : base(configuration)
            {
            }
        }

        public class MultiHandlersResponse : MessageResponseBase
        {
        }

        public class MultiHandlersPipeline : PipelineBase<MultiHandlersRequest, MultiHandlersResponse>
        {
            public MultiHandlersPipeline() : base(new LoggerService("Pipeline"))
            {
                AddSequential(new FirstActionHandler(), new FirstActionHandler());
                AddParallel(new FirstActionHandler(), new FirstActionHandler(), new FirstActionHandler());
                AddSequential(new FirstActionHandler(), new FirstActionHandler());
                AddParallel(new FirstActionHandler(), new FirstActionHandler(), new FirstActionHandler());
            }
        }

        [TestMethod]
        public void Pipeline_MultiHandlers_Handlers_ResponseTypeIsValid()
        {
            var pipeline = new MultiHandlersPipeline();
            var response = pipeline.Execute(new MultiHandlersRequest(new PipelineConfiguration {ClearErrorsBeforeNextHandler = false}));
            Assert.IsInstanceOfType(response, typeof(MultiHandlersResponse));
            Assert.AreEqual(_count, 10);
        }
    }
}
