using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNetCore.Mvc;

using Kakariki.Scrabble.Logic;
using Kakariki.Scrabble.SimpleWeb.Binder;
using Kakariki.Scrabble.SimpleWeb.Configuration;
using System.Diagnostics;

namespace Kakariki.Scrabble.SimpleWeb.Models
{
    [ModelBinder(BinderType = typeof(BoardModelBinder))]
    public class BoardModel
    {
        public Board Board { get; }
        public WordList List { get; }

        public BoardModel()
        {
            WordList list = WordListConfig.Lists[WordListConfig.ENGLISH_AS_A_SECOND_LANGUAGE];
            Board = Board.InitiliseBoard(list);
            List = list;
        }

        public BoardModel(Board board, WordList list)
        {
            Board = board.ThrowIfNull("board");
            List = list.ThrowIfNull("list");
        }

        public BoardRowModel this[int row]
        {
            get { return new BoardRowModel(row, Board); }
            set
            {
                Debug.WriteLine(value);
            }
        }
    }
}