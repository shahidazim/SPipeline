namespace SPipeline.Core.DebugHelper
{
    using System;

    public interface ILoggerService
    {
        void Error(string message);

        void Exception(Exception exception);

        void Warning(string message);

        void Information(string message);

        void Verbose(string message);
    }
}
