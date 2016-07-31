namespace SPipeline.Core.Interfaces
{
    public interface IMessageDispatcher
    {
        IMessageResponse Execute(IMessageRequest messageRequest);
    }
}
