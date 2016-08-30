namespace SPipeline.Logger.NLog.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class LoggerServiceTests
    {
        [TestMethod]
        [TestCategory("Integration"), TestCategory("Logging")]
        public void LoggerService_LogError()
        {
            var logger = new LoggerService("mylogger");
            logger.Error("Error Message");
        }

        [TestMethod]
        [TestCategory("Integration"), TestCategory("Logging")]
        public void LoggerService_LogException()
        {
            var logger = new LoggerService("mylogger");

            try
            {
                int i = 1;
                int y = i/0;
            }
            catch (Exception exception)
            {
                logger.Exception(exception);
            }
        }
    }
}
