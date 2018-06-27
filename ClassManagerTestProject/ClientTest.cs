using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sebastien.ClassManager.Core;
using Sebastien.ClassManager.Enums;

namespace Sebastien.ClassManager.TestProject
{
    [TestClass]
    public class ClentTest
    {
        [TestMethod]
        public void GetCmdTest1()
        {
            //arrange
            var input = "MyScore";
            //act
            Command expected = Command.MyScore;
            Client.GetCmd(input, out Command cmd);
            //assert
            Assert.AreEqual(expected, cmd);
        }
        [TestMethod]
        public void GetSelectorObjectTest()
        {
            //arrange
        }
        [TestMethod]
        public void GetCmdTest2()
        {
            //arrange
            var input = "random";
            //act
            string expected = null;
            var actual = Client.GetCmd(input, out Command cmd);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void IdentityCheckTest()
        {
            //arrange
            var info = Tuple.Create("headteacher", "1234");
            //act
            UserCore expected = null;
            UserCore result = Client.IdentityCheck(info);
            //assert
            Assert.AreNotEqual(expected, result);
        }
        [TestMethod]
        public void CheckAccountAvailabilityTest()
        {
            //arrange
            var account = "headteacher";
            //act
            UserCore result = Client.CheckAccountAvailability(account);
            //assert
            Assert.AreNotEqual(result, null);
        }
    }
}
