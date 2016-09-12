namespace SPipeline.Core.Interfaces.Services
{
    using System.Collections.Generic;

    public interface IQueueService
    {
        void Send(string content);

        IEnumerable<string> Receive();
    }
}
