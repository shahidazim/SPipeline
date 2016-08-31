namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces.Pipeline;

    /// <summary>
    /// The base implementation for message response.
    /// </summary>
    /// <seealso cref="SPipeline.Pipeline.ResponseBase" />
    /// <seealso cref="IMessageResponse" />
    public abstract class MessageResponseBase : ResponseBase, IMessageResponse
    {
    }
}
