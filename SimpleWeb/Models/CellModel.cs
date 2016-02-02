using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kakariki.Scrabble.Logic;

namespace Kakariki.Scrabble.SimpleWeb.Models
{
    public class CellModel
    {
        public CellModel(Cell cell)
        {
            Cell = cell.ThrowIfNull("cell");
        }

        public Cell Cell { get; }

        public string Style
        {
            get
            {
                if(Cell.Letter.HasValue)
                {
                    return "has-letter";
                }
                switch (Cell.Type)
                {
                    case CellType.DOUBLE_LETTER:
                        return "double-letter";
                    case CellType.DOUBLE_WORD:
                        return "double-word";
                    case CellType.NORMAL:
                        return "normal";
                    case CellType.TRIPPLE_LETTER:
                        return "tripple-letter";
                    case CellType.TRIPPLE_WORD:
                        return "tripple-word";
                    default:
                        throw new ApplicationException("No Such Cell Type " + Cell.Type);
                }
            }
        }
    }
}