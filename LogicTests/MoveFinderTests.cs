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
    public class MoveFinderTests
    {
        private Board board;

        [TestMethod()]
        public void FindMovesFindSimpleVerticalMoveTest()
        {
            var words = WordList.Load(new string[] { "cat", "zap" });
            board = Board.InitiliseBoard(words);
            board.GetCell(6, 5).Letter = 'z';
            board.GetCell(7, 5).Letter = 'a';
            board.GetCell(8, 5).Letter = 'p';
            Hand hand = new Hand("ct");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(1, moves.Count);
            var catMove = moves[0];
            Assert.AreEqual("cat", catMove.Word);
            Assert.AreEqual(MoveOrientation.VERTICAL, catMove.Orientation);
            Assert.AreEqual(4, catMove.Row);
            Assert.AreEqual(7, catMove.Column);
        }

        [TestMethod()]
        public void FindMovesVerticalDontCheckExistingWordsTest()
        {
            var words = WordList.Load(new string[] { "cat" });
            board = Board.InitiliseBoard(words);
            // Existing word (zap) not present in word list, should still form cat
            board.GetCell(6, 5).Letter = 'z';
            board.GetCell(7, 5).Letter = 'a';
            board.GetCell(8, 5).Letter = 'p';
            Hand hand = new Hand("ct");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(1, moves.Count);
            var catMove = moves[0];
            Assert.AreEqual("cat", catMove.Word);
            Assert.AreEqual(MoveOrientation.VERTICAL, catMove.Orientation);
            Assert.AreEqual(4, catMove.Row);
            Assert.AreEqual(7, catMove.Column);
        }

        [TestMethod()]
        public void FindMovesFindSimpleHorozontalMoveTest()
        {
            var words = WordList.Load(new string[] { "cat", "zap" });
            board = Board.InitiliseBoard(words);
            board.GetCell(7, 4).Letter = 'z';
            board.GetCell(7, 5).Letter = 'a';
            board.GetCell(7, 6).Letter = 'p';
            Hand hand = new Hand("ct");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(1, moves.Count);
            var catMove = moves[0];
            Assert.AreEqual("cat", catMove.Word);
            Assert.AreEqual(MoveOrientation.HORIZONTAL, catMove.Orientation);
            Assert.AreEqual(5, catMove.Row);
            Assert.AreEqual(6, catMove.Column);
        }

        [TestMethod()]
        public void FindMovesHorozontalDontCheckExistingWordsTest()
        {
            var words = WordList.Load(new string[] { "cat" });
            board = Board.InitiliseBoard(words);
            // Existing word (zap) not present in word list, should still form cat
            board.GetCell(7, 4).Letter = 'z';
            board.GetCell(7, 5).Letter = 'a';
            board.GetCell(7, 6).Letter = 'p';
            Hand hand = new Hand("ct");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(1, moves.Count);
            var catMove = moves[0];
            Assert.AreEqual("cat", catMove.Word);
            Assert.AreEqual(MoveOrientation.HORIZONTAL, catMove.Orientation);
            Assert.AreEqual(5, catMove.Row);
            Assert.AreEqual(6, catMove.Column);
        }

        [TestMethod]
        public void FindMovesStartingMovesTest()
        {
            var words = WordList.Load(new string[] { "cat" });
            board = Board.InitiliseBoard(words);
            Hand hand = new Hand("cat");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(6, moves.Count);
        }

        [TestMethod]
        public void FindMovesOrderTest()
        {
            var words = WordList.Load(new string[] { "cat", "fish", "catfish" });
            board = Board.InitiliseBoard(words);
            board.GetCell(7, 4).Letter = 'a';
            Hand hand = new Hand("ctfish");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(4, moves.Count);
            // Catfish will get more points then cat.
            Assert.AreEqual("catfish", moves[0].Word);
            Assert.AreEqual("catfish", moves[1].Word);
            Assert.AreEqual("cat", moves[2].Word);
            Assert.AreEqual("cat", moves[3].Word);
        }

        [TestMethod()]
        public void FindMovesMultipleMovesSameWordTest()
        {
            var words = WordList.Load(new string[] { "cat" });
            board = Board.InitiliseBoard(words);
            // Existing word (zap) not present in word list, should still form cat
            board.GetCell(7, 4).Letter = 'a';
            board.GetCell(7, 5).Letter = 'a';
            board.GetCell(7, 6).Letter = 'a';
            Hand hand = new Hand("ct");
            MoveFinder finder = new MoveFinder(board, hand, words);
            var moves = finder.FindMoves().ToList();
            Assert.AreEqual(3, moves.Count);
            var catMove = moves[0];
            Assert.AreEqual("cat", catMove.Word);
            Assert.AreEqual(MoveOrientation.HORIZONTAL, catMove.Orientation);
            Assert.AreEqual(6, catMove.Row);
            Assert.AreEqual(6, catMove.Column);
            Assert.AreEqual(11, catMove.Score);

            Assert.AreEqual(6, moves[1].Score);
            Assert.AreEqual(5, moves[2].Score);
        }
    }
}