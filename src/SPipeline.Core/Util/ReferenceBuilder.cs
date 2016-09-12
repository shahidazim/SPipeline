namespace SPipeline.Core.Util
{
    using System;

    public static class ReferenceBuilder
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
