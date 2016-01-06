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
    public class WordListTests
    {
        [TestMethod()]
        public void ContainsDoesNoteContainTest()
        {
            var list = WordList.Load(new string[] { "a", "b", "c" });
            Assert.IsFalse(list.Contains("d"));
        }

        [TestMethod()]
        public void ContainsDoesContainTest()
        {
            var list = WordList.Load(new string[] { "a", "b", "c" });
            Assert.IsTrue(list.Contains("a"));
            Assert.IsTrue(list.Contains("c"));
        }

        [TestMethod()]
        public void ContainsStartsWithTest()
        {
            var list = WordList.Load(new string[] { "abc", "baa", "aac" });
            Assert.IsFalse(list.Contains("a"));
        }

        [TestMethod()]
        public void ContainsDoesContainMultiLetterTest()
        {
            var list = WordList.Load(new string[] { "cat", "bat", "dog" });
            Assert.IsTrue(list.Contains("cat"));
            Assert.IsTrue(list.Contains("bat"));
            Assert.IsTrue(list.Contains("dog"));
        }

        [TestMethod()]
        public void ContainsDoesContainCaseTest()
        {
            var list = WordList.Load(new string[] { "CaT", "BAt", "dOG" });
            Assert.IsTrue(list.Contains("cat"));
            Assert.IsTrue(list.Contains("bat"));
            Assert.IsTrue(list.Contains("dog"));
        }

        [TestMethod()]
        public void LoadSortedTest()
        {
            var list = WordList.Load(new string[] { "ab", "aa", "aC" });
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "aa", "ab", "ac" }, list.Words));
        }

        [TestMethod()]
        public void LoadExcludesTest()
        {
            var list = WordList.Load(new string[] { "ab","a-d", "aa","a's","", "aC" });
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "aa", "ab", "ac" }, list.Words));
        }
        
        [TestMethod()]
        public void ToStringTest()
        {
            var list = WordList.Load(new string[] { "aa", "ab", "ac", "ba", "bb", "ca", "zz", "za"});
            Assert.AreEqual("WordList[a=3, b=2, c=1, d=0, e=0, f=0, g=0, h=0, i=0, j=0, k=0, l=0, m=0, n=0, o=0, p=0, q=0, r=0, s=0, t=0, u=0, v=0, w=0, x=0, y=0, z=2]", list.ToString());
                }
    }
}