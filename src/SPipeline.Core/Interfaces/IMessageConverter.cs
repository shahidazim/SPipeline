namespace SPipeline.Core.Interfaces
{
    public interface IMessageConverter<in T>
    {
        IMessageRequest Convert(T document);
    }
}
