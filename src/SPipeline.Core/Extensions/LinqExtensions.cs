namespace SPipeline.Core.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The class to provide extension methods for Linq.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Split the source list into fixed size chunks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            return source.Select((s, i) => new { Value = s, Index = i })
                         .GroupBy(item => item.Index / batchSize, item => item.Value);
        }
    }
}
