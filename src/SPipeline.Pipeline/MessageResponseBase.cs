namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;

    public abstract class MessageResponseBase : ResponseBase, IMessageResponse
    {
        public void ClearErrors(bool clearErrorsBeforeNextHandler)
        {
            if (clearErrorsBeforeNextHandler)
            {
                _errors.Clear();
            }
        }
    }
}
