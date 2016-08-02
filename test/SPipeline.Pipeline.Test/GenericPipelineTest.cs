using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPipeline.Core.Interfaces;

namespace SPipeline.Pipeline.Test
{
    [TestClass]
    public class GenericPipelineTest
    {
        public class GenericActionRequest : ActionRequestBase
        {
        }

        public class GenericActionResponse : ActionResponseBase
        {
        }

        public class GenericActionHandler<TTranslateRequest, TTranslateResponse>
            : ActionHandlerBase<GenericActionRequest, GenericActionResponse, TTranslateRequest, TTranslateResponse>
            where TTranslateRequest : IMessageRequest
            where TTranslateResponse : IMessageResponse
        {
            public GenericActionHandler(
                Func<TTranslateRequest, GenericActionRequest> requestTranslator,
                Func<GenericActionResponse, TTranslateResponse> responseTranslator)
                : base(requestTranslator, responseTranslator)
            {
            }

            public override GenericActionResponse Execute(GenericActionRequest actionRequest)
            {
                return new GenericActionResponse();
            }
        }

        public class GenericPipelineRequest : MessageRequestBase
        {
            public GenericPipelineRequest(bool clearErrorsBeforeNextHandler) : base(clearErrorsBeforeNextHandler)
            {
            }
        }

        public class GenericPipelineResponse : MessageResponseBase
        {
        }

        [TestMethod]
        public void Generic_Pipeline_Test()
        {
            var pipeline = new GenericPipeline<GenericPipelineRequest, GenericPipelineResponse>();
            pipeline.AddSequential(
                new GenericActionHandler<GenericPipelineRequest, GenericPipelineResponse>(req => new GenericActionRequest(), res => new GenericPipelineResponse()));
            var response = pipeline.Execute(new GenericPipelineRequest(false));
            Assert.IsInstanceOfType(response, typeof(GenericPipelineResponse));
        }
    }
}
