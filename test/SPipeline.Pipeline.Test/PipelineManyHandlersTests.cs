namespace SPipeline.Pipeline.Test.Pipeline
{
    using Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading;

    [TestClass]
    public class PipelineManyHandlersTests
    {
        private static int _count;
        private const int LoopCount = 1000;
        private static readonly object lockObject = new object();

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
                lock (lockObject)
                {
                    _count++;
                }
                Thread.Sleep(10);
                return new FirstActionResponse();
            }
        }

        public class MultipleHandlersSequentialRequest : MessageRequestBase
        {
            public MultipleHandlersSequentialRequest(bool clearErrorsBeforeNextHandler) : base(clearErrorsBeforeNextHandler)
            {
            }
        }

        public class MultipleHandlersSequentialResponse : MessageResponseBase
        {
        }

        public class MultipleHandlersSequentialPipeline : PipelineBase<MultipleHandlersSequentialRequest, MultipleHandlersSequentialResponse>
        {
            public MultipleHandlersSequentialPipeline()
            {
                var actionHandlers = new FirstActionHandler<MultipleHandlersSequentialRequest, MultipleHandlersSequentialResponse>[LoopCount];
                for (var i = 0; i < LoopCount; i++)
                {
                    actionHandlers[i] = new FirstActionHandler<MultipleHandlersSequentialRequest, MultipleHandlersSequentialResponse>(source => new FirstActionRequest(), source => new MultipleHandlersSequentialResponse());
                }
                AddParallel(actionHandlers);
                //AddSequential(actionHandlers);
            }
        }

        [TestMethod]
        public void Pipeline_Sequential_Two_Handlers_ResponseTypeIsValid()
        {
            var pipeline = new MultipleHandlersSequentialPipeline();
            var response = pipeline.Execute(new MultipleHandlersSequentialRequest(false));
            Assert.IsInstanceOfType(response, typeof(MultipleHandlersSequentialResponse));
            Assert.AreEqual(LoopCount * 1, _count);
        }
    }
}
