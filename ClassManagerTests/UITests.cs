using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sebastien.ClassManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Sebastien.ClassManager.Core.Tests
{
    [TestClass]
    public class UITests
    {
        [TestMethod]
        public void MethodTest()
        {
            User.LoginWithBlock().Post(() =>Tuple.Create<String, String>("headteacher", "1234"));
            Assert.AreNotEqual(null, User.Result.Receive());
        }
    }
}