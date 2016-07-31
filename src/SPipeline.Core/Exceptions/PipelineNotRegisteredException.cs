namespace SPipeline.Core.Exceptions
{
    using System;

    public class PipelineNotRegisteredException : Exception
    {
        public PipelineNotRegisteredException
        (
            Type type
        )
        : base($"Handler not found: {type.Name}")
        {
        }
    }
}
