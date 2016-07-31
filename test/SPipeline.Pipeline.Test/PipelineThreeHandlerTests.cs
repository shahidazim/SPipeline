namespace SPipeline.Pipeline.Test.Pipeline
{
    using Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class PipelineThreeHandlerTests
    {
        private static int _count;

        public class FirstActionRequest : ActionRequestBase
        {
        }

        public class FirstActionResponse : ActionResponseBase
        {
        }

        public class FirstActionHandler
            : ActionHandlerBase<FirstActionRequest, FirstActionResponse, ThreeHandlersSequentialRequest, ThreeHandlersSequentialResponse>
        {
            public FirstActionHandler()
                : base(source => new FirstActionRequest(), source => new ThreeHandlersSequentialResponse())
            {
            }

            public override FirstActionResponse Execute(FirstActionRequest actionRequest)
            {
                _count++;
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
                _count++;
                return new SecondActionResponse();
            }
        }


        public class ThirdActionRequest : ActionRequestBase
        {
        }

        public class ThirdActionResponse : ActionResponseBase
        {
        }

        public class ThirdActionHandler<TTranslateRequest, TTranslateResponse>
            : ActionHandlerBase<ThirdActionRequest, ThirdActionResponse, TTranslateRequest, TTranslateResponse>
            where TTranslateRequest : IMessageRequest
            where TTranslateResponse : IMessageResponse
        {
            public ThirdActionHandler(
                Func<TTranslateRequest, ThirdActionRequest> requestTranslator,
                Func<ThirdActionResponse, TTranslateResponse> responseTranslator)
                : base(requestTranslator, responseTranslator)
            {
            }

            public override ThirdActionResponse Execute(ThirdActionRequest actionRequest)
            {
                _count++;
                return new ThirdActionResponse();
            }
        }

        public class ThreeHandlersSequentialRequest : MessageRequestBase
        {
            public ThreeHandlersSequentialRequest(bool clearErrorsBeforeNextHandler) : base(clearErrorsBeforeNextHandler)
            {
            }
        }

        public class ThreeHandlersSequentialResponse : MessageResponseBase
        {
        }

        public class ThreeHandlersSequentialPipeline : PipelineBase<ThreeHandlersSequentialRequest, ThreeHandlersSequentialResponse>
        {
            public ThreeHandlersSequentialPipeline()
            {
                AddSequential(new FirstActionHandler());

                AddParallel(
                    new SecondActionHandler<ThreeHandlersSequentialRequest, ThreeHandlersSequentialResponse>(
                        source => new SecondActionRequest(),
                        source => new ThreeHandlersSequentialResponse()),
                    new ThirdActionHandler<ThreeHandlersSequentialRequest, ThreeHandlersSequentialResponse>(
                        source => new ThirdActionRequest(),
                        source => new ThreeHandlersSequentialResponse()));
            }
        }

        [TestMethod]
        public void Pipeline_Sequential_Three_Handlers_ResponseTypeIsValid()
        {
            var pipeline = new ThreeHandlersSequentialPipeline();
            var response = pipeline.Execute(new ThreeHandlersSequentialRequest(false));
            Assert.IsInstanceOfType(response, typeof(ThreeHandlersSequentialResponse));
            Assert.AreEqual(_count, 3);
        }
    }
}
