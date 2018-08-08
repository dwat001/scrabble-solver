using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Kakariki.Scrabble.Logic;
using System.IO;
using Kakariki.Scrabble.SimpleWeb;
using Kakariki.Scrabble.SimpleWeb.Models;

namespace Kakariki.Scrabble.SimpleWeb.Controllers
{
    public class SimpleBoardController : Controller
    {
        // GET: SimpleBoard
        public ActionResult Index()
        {
            WordList list = WordListConfig.Lists[WordListConfig.ENGLISH_AS_A_SECOND_LANGUAGE];
            var board = Board.InitiliseBoard(list);
           
            var suggestionRequest = new SuggestionRequestModel();
            suggestionRequest.Board = new BoardModel(board, list);
            suggestionRequest.Hand = new HandModel();
            suggestionRequest.Suggestions = new SuggestionsModel();

            return View(suggestionRequest);
        }

        // POST: SimpleBoard/SuggestWords
        [HttpPost]
        public ActionResult SuggestWords(SuggestionRequestModel suggestionRequest)
        {
            var suggestions = new SuggestionsModel();
            suggestions.Moves = FindMoves(suggestionRequest.Board.Board, suggestionRequest.Hand.Hand, suggestionRequest.Board.List)
                .Take(20).ToImmutableList();
            suggestionRequest.Suggestions = suggestions;

            return View("Index", suggestionRequest);
        }

        private IEnumerable<Move> FindMoves(Board board, Hand hand, WordList list)
        {
            MoveFinder finder = new MoveFinder(board.ThrowIfNull("board"), hand.ThrowIfNull("hand"), list.ThrowIfNull("list"));
            return finder.FindMoves();
        }
    }
}
