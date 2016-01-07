using System;

namespace Kakariki.Scrabble.Logic
{
    public class Cell
    {
        public Cell(CellType cellType, int column, int row, char? letter)
        {
            Type = cellType;
            Column = column;
            Row = row;
            Letter = letter;
        }

        public CellType Type { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public char? Letter { get; set; }

        public override string ToString()
        {
            return string.Format("({0},{1}) [{2}]", Column, Row, ToChar());
        }

        public char ToChar()
        {
            if (Letter.HasValue)
            {
                return Letter.Value;
            }
            switch (Type)
            {
                case CellType.NORMAL:
                    return '.';
                case CellType.DOUBLE_LETTER:
                    return 'D';
                case CellType.TRIPPLE_LETTER:
                    return 'T';
                case CellType.DOUBLE_WORD:
                    return '2';
                case CellType.TRIPPLE_WORD:
                    return '3';
                default:
                    throw new ApplicationException("Unkown Cell type" + Type);
            }
        }
    }
    public enum CellType
    {
        NORMAL,
        DOUBLE_LETTER,
        TRIPPLE_LETTER,
        DOUBLE_WORD,
        TRIPPLE_WORD
    }
}
