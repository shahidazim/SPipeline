using NLog;

namespace SPipeline.Logger.NLog
{
    using SPipeline.Core.DebugHelper;
    using System;
    using System.Diagnostics;

    public class LoggerService : ILoggerService
    {
        private const int SkipFrame = 1;
        private readonly global::NLog.Logger _logger;

        public LoggerService(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Error(string message)
        {
            var frame = new StackFrame(SkipFrame, true);
            _logger.Error($"{TextFormatter.GetFormattedStackFrame(frame)} : {message}");
        }

        public void Exception(Exception exception)
        {
            var frame = new StackFrame(SkipFrame, true);
            _logger.Error($"{TextFormatter.GetFormattedStackFrame(frame)} : {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }

        public void Warning(string message)
        {
            var frame = new StackFrame(SkipFrame, true);
            _logger.Warn($"{TextFormatter.GetFormattedStackFrame(frame)} : {message}");
        }

        public void Information(string message)
        {
            var frame = new StackFrame(SkipFrame, true);
            _logger.Info($"{TextFormatter.GetFormattedStackFrame(frame)} : {message}");
        }

        public void Verbose(string message)
        {
            var frame = new StackFrame(SkipFrame, true);
            _logger.Trace($"{TextFormatter.GetFormattedStackFrame(frame)} : {message}");
        }
    }
}
