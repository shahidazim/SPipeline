namespace SPipeline.Pipeline.Test
{
    using SPipeline.Core.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    class DummyMessage1 : MessageRequestBase
    {
        public DummyMessage1() : base(true)
        {
        }
    }

    class DummyMessage2 : MessageRequestBase
    {
        public DummyMessage2() : base(true)
        {
        }
    }

    class DummyResponse : MessageResponseBase
    {
    }

    class DummyPipeline : PipelineBase<DummyMessage1, DummyResponse>
    {
    }

    [TestClass]
    public class MessageDispatcherTests
    {
        [TestMethod]
        [ExpectedException(typeof(PipelineNotRegisteredException))]
        public void MessageDispatcher_Execute_Throw_NotRegisteredException()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();

            // Act

            // Assert
            var response = messageDispatcher.Execute(new DummyMessage1());
        }

        [TestMethod]
        public void MessageDispatcher_Execute_DummyMessage_With_DummyPipeline()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            messageDispatcher.RegisterPipeline(new DummyPipeline());

            // Act
            var response = messageDispatcher.Execute(new DummyMessage1());

            // Assert
            Assert.IsInstanceOfType(response, typeof(DummyResponse));
        }

        [TestMethod]
        [ExpectedException(typeof(PipelineNotRegisteredException))]
        public void MessageDispatcher_Execute_DummyMessage1_And_DummyMessage2_With_DummyPipeline_Throw_NotRegisteredException()
        {
            // Arrange
            var messageDispatcher = new MessageDispatcher();
            messageDispatcher.RegisterPipeline(new DummyPipeline());

            // Act
            messageDispatcher.Execute(new DummyMessage1());

            // Assert
            var response = messageDispatcher.Execute(new DummyMessage2());
        }
    }
}
