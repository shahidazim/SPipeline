namespace SPipeline.Core.Models
{
    using System;
    using System.Text;

    public class MessageError
    {
        private const string ErrorLineBreak = "---------------------------------------------------";

        public MessageError(string errorMessage, bool canContinue) : this(errorMessage, null, canContinue)
        {
        }

        public MessageError(Exception exception, bool canContinue) : this(null, exception, canContinue)
        {
        }

        private MessageError(string errorMessage, Exception exception, bool canContinue)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
            CanContinue = canContinue;
        }

        public string ErrorMessage { get; }

        public Exception Exception { get; }

        public bool CanContinue { get; }

        public string GetFormattedError()
        {
            if (Exception != null)
            {
                var fullStackTrace = new StringBuilder();
                return string.Format("{0} {1} {2} {3}",
                    GetFullStackTrace(Exception, fullStackTrace),
                    Environment.NewLine,
                    ErrorLineBreak,
                    Environment.NewLine);
            }
            return ErrorMessage;
        }

        private static StringBuilder GetFullStackTrace(Exception exception, StringBuilder fullStackTrace)
        {
            fullStackTrace.Append(exception.Message);
            fullStackTrace.AppendLine();
            fullStackTrace.Append(exception.StackTrace);
            if (exception.InnerException != null)
            {
                fullStackTrace.Append(ErrorLineBreak);
                fullStackTrace.AppendLine();
                fullStackTrace = GetFullStackTrace(exception.InnerException, fullStackTrace);
            }
            return fullStackTrace;
        }
    }
}
