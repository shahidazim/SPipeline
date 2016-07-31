namespace SPipeline.Core.Interfaces
{
    public interface IMessageResponse : IResponse
    {
        void ClearErrors(bool clearErrorsBeforeNextHandler);
    }
}
