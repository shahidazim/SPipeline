namespace SPipeline.Core.Models
{
    using System;
    using System.Text;

    /// <summary>
    /// The message contains error details.
    /// </summary>
    public class MessageError
    {
        private const string ErrorLineBreak = "---------------------------------------------------";

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageError"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="canContinue">if set to <c>true</c> if the execution can be continued regardless of errors.</param>
        public MessageError(string errorMessage, bool canContinue) : this(errorMessage, null, canContinue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageError"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="canContinue">if set to <c>true</c> if the execution can be continued regardless of errors.</param>
        public MessageError(Exception exception, bool canContinue) : this(null, exception, canContinue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageError"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="canContinue">if set to <c>true</c> if the execution can be continued regardless of errors.</param>
        private MessageError(string errorMessage, Exception exception, bool canContinue)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
            CanContinue = canContinue;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; }

        /// <summary>
        /// Gets a value indicating whether this instance can continue regardless of errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can continue regardless of errors; otherwise, <c>false</c>.
        /// </value>
        public bool CanContinue { get; }

        /// <summary>
        /// Gets the formatted error.
        /// </summary>
        /// <returns></returns>
        public string GetFormattedError()
        {
            if (Exception != null)
            {
                var fullStackTrace = new StringBuilder();
                return $"{GetFullStackTrace(Exception, fullStackTrace)} {Environment.NewLine} {ErrorLineBreak} {Environment.NewLine}";
            }
            return ErrorMessage;
        }

        /// <summary>
        /// Gets the full stack trace.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fullStackTrace">The full stack trace.</param>
        /// <returns></returns>
        private static StringBuilder GetFullStackTrace(Exception exception, StringBuilder fullStackTrace)
        {
            fullStackTrace.Append(exception.Message);
            fullStackTrace.AppendLine();
            fullStackTrace.Append(exception.StackTrace);

            if (exception.InnerException == null)
            {
                return fullStackTrace;
            }

            fullStackTrace.Append(ErrorLineBreak);
            fullStackTrace.AppendLine();
            fullStackTrace = GetFullStackTrace(exception.InnerException, fullStackTrace);
            return fullStackTrace;
        }
    }
}
