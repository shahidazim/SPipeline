namespace SPipeline.Pipeline.Test.Pipeline
{
    using SPipeline.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            public TwoHandlersSequentialRequest(bool clearErrorsBeforeNextHandler) : base(clearErrorsBeforeNextHandler)
            {
            }
        }

        public class TwoHandlersSequentialResponse : MessageResponseBase
        {
        }

        public class TwoHandlersSequentialPipeline : PipelineBase<TwoHandlersSequentialRequest, TwoHandlersSequentialResponse>
        {
            public TwoHandlersSequentialPipeline()
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
            var response = pipeline.Execute(new TwoHandlersSequentialRequest(false));
            Assert.IsInstanceOfType(response, typeof(TwoHandlersSequentialResponse));
        }
    }
}
