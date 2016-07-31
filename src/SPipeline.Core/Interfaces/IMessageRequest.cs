namespace SPipeline.Core.Interfaces
{
    public interface IMessageRequest : ITranslatable
    {
        bool ClearErrorsBeforeNextHandler { get; }
    }
}
