using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kakariki.Scrabble.SimpleWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kakariki.Scrabble.Logic;
using Kakariki.Scrabble.SimpleWeb.Models;
using System.IO;

namespace Kakariki.Scrabble.SimpleWeb.Controllers.Tests
{
    [TestClass()]
    public class SimpleBoardControllerTests
    {
        private static KeyValuePair<string, WordList> defaultWordList =
          new KeyValuePair<string, WordList>(WordListConfig.ENGLISH_AS_A_SECOND_LANGUAGE,
            WordList.Load(new List<string> {"cat", "rat", "art", "bat" }));
        
        [TestMethod()]
        public void IndexTest()
        {
            ConfigureSimpleWordList();
            var undertest = new SimpleBoardController();
            ActionResult action = undertest.Index();
            Assert.AreEqual(typeof(ViewResult), action.GetType());

            ViewResult result = action as ViewResult;
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(typeof(SuggestionRequestModel), result.Model.GetType());
        }

        private static void ConfigureSimpleWordList()
        {
            var wordLists = new Dictionary<string, WordList>();
            wordLists.Add(defaultWordList.Key, defaultWordList.Value);
            WordListConfig.SetWordLists(wordLists);
        }

        [TestMethod()]
        public void SuggestWordsTest()
        {
            ConfigureSimpleWordList();
            var undertest = new SimpleBoardController();
            var model = new SuggestionRequestModel();
            var list = WordListConfig.Lists[WordListConfig.ENGLISH_AS_A_SECOND_LANGUAGE];
            var board = Board.InitiliseBoard(list);
            board.GetCell(1, 1).Letter = 'A';
            board.GetCell(1, 2).Letter = 'B';
            board.GetCell(1, 3).Letter = 'C';
            model.Board = new BoardModel(board, list);
            model.Hand = new HandModel("rtat");
            model.Suggestions = new SuggestionsModel();
            ActionResult action = undertest.SuggestWords(model);
            Assert.AreEqual(typeof(ViewResult), action.GetType());

            ViewResult result = action as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(typeof(SuggestionRequestModel), result.Model.GetType());

            var resultantModel = result.Model as SuggestionRequestModel;
            Assert.IsNotNull(resultantModel);
            Assert.IsNotNull(resultantModel.Board);
            Assert.IsNotNull(resultantModel.Hand);
            Assert.IsNotNull(resultantModel.Suggestions);
            Assert.AreEqual(3, resultantModel.Suggestions.Moves.Count());
        }
    }
}