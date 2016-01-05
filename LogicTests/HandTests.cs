using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kakariki.Scrabble.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic.Tests
{
    [TestClass()]
    public class HandTests
    {
        [TestMethod()]
        public void HandSevenLettersTest()
        {
            new Hand(new char[]{'1', '2', '3', '4', '5', '6', '7' });
        }
        [TestMethod()]
        public void HandLessLettersTest()
        {
            new Hand(new char[] { '1', '2', '3', '4', '5' });
        }

        [TestMethod()]
        [ExpectedException(typeof(ApplicationException), "Expected Application Exception due to to many letters")]
        public void HandTooManyLettersTest()
        {
            new Hand(new char[] { '1', '2', '3', '4', '5', '6', '7', '8' });
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("Hand [a, b, c]", new Hand(new char[] { 'a', 'b', 'c' }).ToString());
        }

        [TestMethod()]
        public void LettersTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new char[] {'a', 'b', 'c' }, new Hand(new char[] { 'a', 'b', 'c' }).Letters));
        }
        [TestMethod()]
        public void LettersWithNoLettersTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new char[] {}, new Hand(new char[] { }).Letters));
        }
    }
}