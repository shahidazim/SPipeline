namespace SPipeline.Core.DebugHelper
{
    using System.Diagnostics;

    public static class TextFormatter
    {
        public static
        string GetFormattedStackFrame
        (
            StackFrame frame
        )
        {
            var parameters = frame.GetMethod().GetParameters();
            var strParams = "";
            for (var i = 0; i < parameters.Length; i++)
            {
                if (i == parameters.Length - 1)
                {
                    strParams += parameters[i].ParameterType + " " + parameters[i].Name;
                }
                else
                {
                    strParams += parameters[i].ParameterType + " " + parameters[i].Name + ", ";
                }
            }

#if DEBUG
            return CreateDebugFormattedMessage(frame, strParams);
#else
            return CreateReleaseFormattedMessage(frame, strParams);
#endif
        }

        private static
        string CreateDebugFormattedMessage(StackFrame frame, string parameters)
        {
            return $"{frame.GetFileName()}#{frame.GetFileLineNumber()} : {frame.GetMethod().DeclaringType.Name}.{frame.GetMethod().Name}({parameters})";
        }

        private static string CreateReleaseFormattedMessage(StackFrame frame, string parameters)
        {
            return $"{frame.GetMethod().DeclaringType.Name}.{frame.GetMethod().Name}({parameters})";
        }

    }
}
