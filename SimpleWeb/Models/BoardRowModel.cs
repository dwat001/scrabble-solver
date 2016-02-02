using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kakariki.Scrabble.Logic;

namespace Kakariki.Scrabble.SimpleWeb.Models
{
    public class BoardRowModel
    {
        public int Row { get; }
        public Board Board { get; }

        public BoardRowModel(int row, Board board)
        {
            this.Row = row;
            this.Board = board.ThrowIfNull();
        }

        public CellModel this[int column]
        {
            get { return new CellModel(Board.GetCell(column, Row)); }
            set { Board.GetCell(column, Row).Letter = value.Cell.Letter; }
        }
    }
}