using MySelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Sebastien.Selector.TestProject
{
    [TestClass]
    public class SelectTest
    {
        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void SelectorTestAboutSelectorInfomation() => new Selector<Int32>(null, 1, 2, 3);

        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void SelectorTestAboutSelects() => new Selector<Int32>(new List<String> { "1", "2" }, null);

        [TestMethod, ExpectedException(typeof(SelectorException))]
        public void SelectorTestAboutLength() => new Selector<Int32>(new List<String> { "1", "2" }, new Int32[] { 1, 2, 3, 4 });
    }
}