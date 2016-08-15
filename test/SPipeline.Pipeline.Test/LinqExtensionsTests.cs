namespace SPipeline.Pipeline.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Extensions;
    using System.Linq;

    [TestClass]
    public class LinqExtensionsTests
    {
        [TestMethod]
        public void LinqExtensions_Batch_GenerateListWith1000Items_SplitWith100_ShouldHave10Batches()
        {
            var list = Enumerable.Range(1, 10);
            var batchedList = list.Batch(5);
            Assert.AreEqual(list.Count() / 5, batchedList.Count());
        }
    }
}
