using SPipeline.Core.Models;

namespace SPipeline.Pipeline.Test.Pipeline
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PipelineMultipleSequentialParallelHandlerExecutionTests
    {
        private static int _count;
        private static readonly object lockObject = new object();

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
                lock (lockObject)
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
            public MultiHandlersPipeline()
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
