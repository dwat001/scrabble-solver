using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kakariki.Scrabble.Logic.Tests
{
    [TestClass()]
    public class ExtensionMethodsTests
    {
        [TestMethod()]
        public void AllIndexesOEmptyTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new int[] { }, "".AllIndexesOf('b')));
        }
        [TestMethod()]
        public void AllIndexesOfNoMatchTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new int[] { }, "a".AllIndexesOf('b')));
        }
        [TestMethod()]
        public void AllIndexesOfOneMatchTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new int[] { 0 }, "a".AllIndexesOf('a')));
        }
        [TestMethod()]
        public void AllIndexesOfMultilpeMatchesTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new int[] { 0, 1, 2 }, "aaa".AllIndexesOf('a')));
        }
        [TestMethod()]
        public void AllIndexesOfMultipleMatchesWithNonMatchesTest()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(new int[] { 1, 2, 4 }, "baabab".AllIndexesOf('a')));
        }

        [TestMethod()]
        public void ToExpandedStringEmptyTest()
        {
            Assert.AreEqual("[]", new int[] { }.ToExpandedString());
        }

        [TestMethod()]
        public void ToExpandedStringOneElementTest()
        {
            Assert.AreEqual("[1]", new int[] { 1 }.ToExpandedString());
        }

        [TestMethod()]
        public void ToExpandedStringThreeElementsTest()
        {
            Assert.AreEqual("[1, 2, 3]", new int[] { 1, 2, 3 }.ToExpandedString());
        }
    }
}