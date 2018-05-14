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
            String input = "MyScore";
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
            Assert.AreEqual(typeof(MySelector.Selector<Subject>), Client.GetSelectorObject().GetType());
        }
        [TestMethod]
        public void GetCmdTest2()
        {
            //arrange
            String input = "random";
            //act
            String expected = null;
            String actual = Client.GetCmd(input, out Command cmd);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void IdentityCheckTest()
        {
            //arrange
            Tuple<String, String> info = Tuple.Create("headteacher", "1234");
            //act
            User expected = null;
            User result = Client.IdentityCheck(info);
            //assert
            Assert.AreNotEqual(expected, result);
        }
        [TestMethod]
        public void CheckAccountAvailabilityTest()
        {
            //arrange
            String account = "headteacher";
            //act
            User result = Client.CheckAccountAvailability(account);
            //assert
            Assert.AreNotEqual(result, null);
        }
    }
}
