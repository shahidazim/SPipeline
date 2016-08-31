namespace SPipeline.Pipeline.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using System;

    [TestClass]
    public class PipelineTwoHandlerTests
    {
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
                return new FirstActionResponse();
            }
        }

        public class SecondActionRequest : ActionRequestBase
        {
        }

        public class SecondActionResponse : ActionResponseBase
        {
        }

        public class SecondActionHandler<TTranslateRequest, TTranslateResponse>
            : ActionHandlerBase<SecondActionRequest, SecondActionResponse, TTranslateRequest, TTranslateResponse>
            where TTranslateRequest : IMessageRequest
            where TTranslateResponse : IMessageResponse
        {
            public SecondActionHandler(
                Func<TTranslateRequest, SecondActionRequest> requestTranslator,
                Func<SecondActionResponse, TTranslateResponse> responseTranslator)
                : base(requestTranslator, responseTranslator)
            {
            }

            public override SecondActionResponse Execute(SecondActionRequest actionRequest)
            {
                return new SecondActionResponse();
            }
        }

        public class TwoHandlersSequentialRequest : MessageRequestBase
        {
            public TwoHandlersSequentialRequest(PipelineConfiguration configuration) : base(configuration)
            {
            }
        }

        public class TwoHandlersSequentialResponse : MessageResponseBase
        {
        }

        public class TwoHandlersSequentialPipeline : PipelineBase<TwoHandlersSequentialRequest, TwoHandlersSequentialResponse>
        {
            public TwoHandlersSequentialPipeline() : base(new LoggerService("Pipeline"))
            {
                AddSequential(
                    new FirstActionHandler<TwoHandlersSequentialRequest, TwoHandlersSequentialResponse>(
                        source => new FirstActionRequest(),
                        source => new TwoHandlersSequentialResponse()),
                    new SecondActionHandler<TwoHandlersSequentialRequest, TwoHandlersSequentialResponse>(
                        source => new SecondActionRequest(),
                        source => new TwoHandlersSequentialResponse()));
            }
        }

        [TestMethod]
        public void Pipeline_Sequential_Two_Handlers_ResponseTypeIsValid()
        {
            var pipeline = new TwoHandlersSequentialPipeline();
            var response = pipeline.Execute(new TwoHandlersSequentialRequest(new PipelineConfiguration { ClearErrorsBeforeNextHandler = false }));
            Assert.IsInstanceOfType(response, typeof(TwoHandlersSequentialResponse));
        }
    }
}
