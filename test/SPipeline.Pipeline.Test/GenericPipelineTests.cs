namespace SPipeline.Pipeline.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
    using System;

    [TestClass]
    public class GenericPipelineTests
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
            public GenericPipelineRequest(PipelineConfiguration configuration) : base(configuration)
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
            var response = pipeline.Execute(new GenericPipelineRequest(new PipelineConfiguration { ClearErrorsBeforeNextHandler = false }));
            Assert.IsInstanceOfType(response, typeof(GenericPipelineResponse));
        }
    }
}
