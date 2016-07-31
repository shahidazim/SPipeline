namespace SPipeline.Core.Interfaces
{
    using System;

    public interface IPipeline<in TMessageRequest, out TMessageResponse>
        where TMessageRequest : IMessageRequest
        where TMessageResponse : IMessageResponse
    {
        Type MessageType { get; }

        TMessageResponse Execute(TMessageRequest messageRequest);
    }

    public interface IPipeline
    {
        Type MessageType { get; }

        IMessageResponse Execute(IMessageRequest messageRequest);
    }
}
