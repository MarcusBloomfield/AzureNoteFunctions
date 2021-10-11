using AzureNoteFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureNoteFunctionsTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DataBaseConnector dataBaseConnector = new DataBaseConnector();
            Assert.IsFalse(dataBaseConnector.GetNumberOfUserAccounts() == -1);
        }
        [TestMethod]
        public void TestMethod2()
        {
            DataBaseConnector dataBaseConnector = new DataBaseConnector();
            Assert.IsTrue(dataBaseConnector.TryLogin().Length > 0);
        }
        [TestMethod]
        public void TestMethod3()
        {
            DataBaseConnector dataBaseConnector = new DataBaseConnector();
            Assert.IsTrue(dataBaseConnector.OpenConnection());
        }
    }
}
