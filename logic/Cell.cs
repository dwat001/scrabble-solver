using System;

namespace Kakariki.Scrabble.Logic
{
    public class Cell
    {
        public Cell(CellType cellType, int xPosition, int yPosition, char? letter)
        {
            Type = cellType;
            XPosition = xPosition;
            YPosition = yPosition;
            Letter = letter;
        }

        public CellType Type { get; private set; }
        public int XPosition { get; private set; }
        public int YPosition { get; private set; }
        public char? Letter { get; set; }

        public override string ToString()
        {
            return string.Format("({0},{1}) [{2}]", XPosition, YPosition, ToChar());
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
