namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;

    public abstract class MessageRequestBase : IMessageRequest
    {
        protected MessageRequestBase(bool clearErrorsBeforeNextHandler)
        {
            ClearErrorsBeforeNextHandler = clearErrorsBeforeNextHandler;
        }

        public bool ClearErrorsBeforeNextHandler { get; }
    }
}
