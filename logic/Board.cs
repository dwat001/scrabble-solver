using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    public class Board
    {
        private const int BOARD_START_INDEX = 1;
        private const int BOARD_END_INDEX = 15;
        private readonly IList<IList<Cell>> cells;
        private readonly WordList words;

        public Board(IList<IList<Cell>> cells, WordList words)
        {
            this.cells = cells;
            this.words = words;
        }

        public IEnumerable<Cell> AllCells
        {
            get
            {
                return cells.SelectMany(l => l);
            }
        }

        public Cell GetCell(Tuple<int, int> xPosYPos)
        {
            return cells[xPosYPos.Item2][xPosYPos.Item1];
        }

        public IDictionary<char, int> GetAvalableLetterCount(ImmutableList<char> hand)
        {
            return AllCells
               .Where(c => c.Letter.HasValue)
               .Select(c => c.Letter.Value)
               .Concat(hand)
               .GroupBy(c => c)
               .ToDictionary(grp => grp.Key, grp => grp.Count());
        }

        public ImmutableDictionary<char, int> LetterCounts { get { return null; } }

        private static Cell CreateCell(int x, int y, char? letter)
        {
            if (x == y || 8 - x == y - 8)
            {
                switch (y)
                {
                    case 1:
                    case 15:
                        return new Cell(CellType.TRIPPLE_WORD, x, y, letter);                        
                    case 7:
                    case 9:
                        return new Cell(CellType.DOUBLE_LETTER, x, y, letter);                        
                    case 6:
                    case 10:
                        return new Cell(CellType.TRIPPLE_LETTER, x, y, letter);  
                    default:
                        return new Cell(CellType.DOUBLE_WORD, x, y, letter);
                }
            }

            switch(y)
            {
                case 1:
                case 15:
                case 8:
                    if (x == 4 || x == 12)
                    {
                        return new Cell(CellType.DOUBLE_LETTER, x, y, letter);
                    }
                    if (x == 1 || x == 15 || x == 8)
                    {
                        return new Cell(CellType.TRIPPLE_WORD, x, y, letter);
                    }
                    break;
                case 2:
                case 14:
                    if ((x == 6 || x == 10))
                    {
                        return new Cell(CellType.TRIPPLE_LETTER, x, y, letter);
                    }
                    break;
                case 3:
                case 13:
                    if ((x == 7 || x == 9))
                    {
                        return new Cell(CellType.DOUBLE_LETTER, x, y, letter);
                    }
                    break;
                case 4:
                case 12:
                    if ((x == 1 || x == 8 || x == 15 ))
                    {
                        return new Cell(CellType.DOUBLE_LETTER, x, y, letter);
                    }
                    break;
                case 7:
                case 9:
                    if(x == 3 || x == 13)
                    {
                        return new Cell(CellType.DOUBLE_LETTER, x, y, letter); 
                    }
                    break;
                case 6:
                case 10:
                    if(x== 2 || x == 14)
                    {
                        return new Cell(CellType.TRIPPLE_LETTER, x, y, letter);
                    }
                    break;
            }

            return new Cell(CellType.NORMAL, x, y, null);
        }

        public static Board InitiliseBoard(WordList words)
        {
            var cells = new List<IList<Cell>>(16);
            cells.Add(ImmutableList.Create<Cell>());
            for (int row = 1; row <= 15; row++)
            {
                cells.Add(new List<Cell>(16));
                // Add dud cell to make [1..15]
                cells[row].Add(new Cell(CellType.NORMAL, 0, 0, null));

                for (int column = 1; column <= 15; column++)
                {
                    cells[row].Add(CreateCell(column, row, null));
                }
            }

            return new Board(cells, words);
        }

        public enum CellType
        {
            NORMAL,
            DOUBLE_LETTER,
            TRIPPLE_LETTER,
            DOUBLE_WORD,
            TRIPPLE_WORD
        }

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
                        throw new ApplicationException("Unkown Cell type" +  Type);
                }
            }
        }

        internal bool IsValidMove(Move possibleMove, Hand hand)
        {
            if(possibleMove.XPosition < BOARD_START_INDEX ||
                possibleMove.XPosition > BOARD_END_INDEX ||
                possibleMove.YPosition < BOARD_START_INDEX ||
                possibleMove.YPosition > BOARD_END_INDEX)
            {
                return false;
            }
            switch (possibleMove.Orientation)
            {
                case MoveOrientation.VERTICAL:
                    return IsValidVerticalMove(possibleMove, hand);
                case MoveOrientation.HORIZONTAL:
                    return IsValidHorizontialMove(possibleMove, hand);
                default:
                    throw new ApplicationException("Unsuported Orientation.");
            }
        }

        private bool IsValidHorizontialMove(Move possibleMove, Hand hand)
        {
            if (possibleMove.XPosition + possibleMove.Word.Length > BOARD_END_INDEX)
            {
                return false;
            }
            int column = possibleMove.XPosition;
            int row = possibleMove.YPosition;
            var lettersInHand = new List<char>(hand.Letters);
            bool letterFromHandUsed = false;

            foreach (char c in possibleMove.Word)
            {
                Cell currentCell = cells[row][column];
                if (currentCell.Letter.HasValue)
                {
                    // There is a letter, and it is not what we require, word does not fit.
                    if (currentCell.Letter.Value != c)
                    {
                        return false;
                    }
                }
                else
                {
                    // Current Square has not value.
                    if (!lettersInHand.Contains(c))
                    {
                        return false; // Hand does not contain the required letter. 
                    }
                    // We need to use the letter from our hand.
                    letterFromHandUsed = true;
                    lettersInHand.Remove(c);
                }
                if (!ValidWordsFormed(row, column, possibleMove))
                {
                    return false;
                }
                column++;
            }
            // If we have not used a letter from our hand this is invlaid.
            return letterFromHandUsed;
        }

        private bool IsValidVerticalMove(Move possibleMove, Hand hand)
        {
            if (possibleMove.YPosition + possibleMove.Word.Length > BOARD_END_INDEX)
            {
                return false;
            }
            int column = possibleMove.XPosition;
            int row = possibleMove.YPosition;
            var lettersInHand = new List<char>(hand.Letters);
            bool letterFromHandUsed = false;

            foreach (char c in possibleMove.Word)
            {
                Cell currentCell = cells[row][column];
                if (currentCell.Letter.HasValue)
                {
                    // There is a letter, and it is not what we require, word does not fit.
                    if (currentCell.Letter.Value != c)
                    {
                        return false;
                    }
                }
                else
                {
                    // Current Square has not value.
                    if (!lettersInHand.Contains(c))
                    {
                        return false; // Hand does not contain the required letter. 
                    }
                    // We need to use the letter from our hand.
                    letterFromHandUsed = true;
                    lettersInHand.Remove(c);
                }
                if (!ValidWordsFormed(row, column, possibleMove))
                {
                    return false;
                }
                row++;
            }
            // If we have not used a letter from our hand this is invlaid.
            return letterFromHandUsed;
        }

        private bool ValidWordsFormed(int row, int column, Move possibleMove)
        {
            return ValidWordsFormedVerticaly(row, column, possibleMove) &&
                ValidWordsFormedHorizontily(row, column, possibleMove);
        }

        private bool ValidWordsFormedVerticaly(int row, int column, Move possibleMove)
        {
            int yStart = row;
            int yEnd = row;
            for (int y = yStart; y >= BOARD_START_INDEX; y--)
            {
                if (!(possibleMove.GetLetterAt(y, column) ?? cells[y][column].Letter).HasValue)
                {
                    break;
                }
                yStart = y;
            }
            for (int y = yEnd; y <= BOARD_END_INDEX; y++)
            {
                if (!(possibleMove.GetLetterAt(y, column) ?? cells[y][column].Letter).HasValue)
                {
                    break;
                }
                yEnd = y;
            }
            if (yStart == yEnd)
            {
                return true;
            }
            StringBuilder sb = new StringBuilder();
            for (int y = yStart; y <= yEnd; y++)
            {
                sb.Append((possibleMove.GetLetterAt(y, column) ?? cells[y][column].Letter).Value);
            }
            return words.Contains(sb.ToString());
        }

        private bool ValidWordsFormedHorizontily(int row, int column, Move possibleMove)
        {
            int xStart = column;
            int xEnd = column;
            for (int x = xStart; x >= BOARD_START_INDEX; x--)
            {
                if (!(cells[row][x].Letter ?? possibleMove.GetLetterAt(row, x)).HasValue)
                {
                    break;
                }
                xStart = x;
            }
            for (int x = xEnd; x <= BOARD_END_INDEX; x++)
            {
                if (!(cells[row][x].Letter ?? possibleMove.GetLetterAt(row, x)).HasValue)
                {
                    break;
                }
                xEnd = x;
            }
            if (xStart == xEnd)
            {
                return true;
            }
            StringBuilder sb = new StringBuilder();
            for (int x = xStart; x <= xEnd; x++)
            {
                sb.Append((possibleMove.GetLetterAt(row, x) ?? cells[row][x].Letter).Value);
            }
            return words.Contains(sb.ToString());
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool includeCellType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("0123456789012345");
            int rowCount = 0;
            foreach (IList<Cell> row in cells)
            {
                if (rowCount == 0)
                {
                    rowCount++;
                    continue;
                }
                foreach (Cell cell in row)
                {
                    if (cell.XPosition == 0)
                    {
                        sb.Append(rowCount.ToString().Last());
                    }
                    else
                    {
                        if (includeCellType)
                        {
                            sb.Append(cell.ToChar());
                        }
                        else
                        {
                            sb.Append(cell.Letter ?? '.');
                        }
                    }
                }
                sb.AppendLine();
                rowCount++;
            }
            return sb.ToString();
        }

        public void Apply(Move move)
        {
            var rowColumn = new Tuple<int, int>(move.YPosition, move.XPosition);
            foreach (char c in move.Word)
            {
                cells[rowColumn.Item1][rowColumn.Item2].Letter = c;
                rowColumn = AdvanceRowColumn(rowColumn, move.Orientation);
            }
        }

        private Tuple<int, int> AdvanceRowColumn(Tuple<int, int> rowColumn, MoveOrientation moveOrientation)
        {
            switch (moveOrientation)
            {
                case MoveOrientation.VERTICAL:
                    return new Tuple<int, int>(rowColumn.Item1 + 1, rowColumn.Item2);
                case MoveOrientation.HORIZONTAL:
                    return new Tuple<int, int>(rowColumn.Item1, rowColumn.Item2 + 1);
                default:
                    throw new ApplicationException("Unkown Orientations.");
            }
        }

        internal string GetWord(MoveOrientation orientation, Cell initialCell, char proposedLetter)
        {
            Cell startCell;
            Cell previousCell = initialCell;
            do // Move startCell to first letter in word.
            {
                startCell = previousCell;
                previousCell = PreviousCell(startCell, orientation);
            } while (previousCell != null && previousCell.Letter.HasValue);

            StringBuilder sb = new StringBuilder();
            Cell currentCell = startCell;
            while (currentCell != null && (currentCell.Letter.HasValue ||
                (currentCell.XPosition == initialCell.XPosition && currentCell.YPosition == initialCell.YPosition) ))
            {
                sb.Append(currentCell.Letter ?? proposedLetter);
                currentCell = NextCell(currentCell, orientation);
            }

            return sb.ToString();
        }

        private Cell NextCell(Cell currentCell, MoveOrientation orientation)
        {
            switch (orientation)
            {
                case MoveOrientation.VERTICAL:
                    if (currentCell.YPosition >= BOARD_END_INDEX)
                    {
                        return null;
                    }
                    return cells[currentCell.YPosition + 1][currentCell.XPosition];
                case MoveOrientation.HORIZONTAL:
                    if (currentCell.XPosition >= BOARD_END_INDEX)
                    {
                        return null;
                    }
                    return cells[currentCell.YPosition][currentCell.XPosition + 1];
                default:
                    throw new ApplicationException("Unexpected Orientation");
            }
        }

        private Cell PreviousCell(Cell currentCell, MoveOrientation orientation)
        {
            switch (orientation)
            {
                case MoveOrientation.VERTICAL:
                    if (currentCell.YPosition <= BOARD_START_INDEX)
                    {
                        return null;
                    }
                    return cells[currentCell.YPosition - 1][currentCell.XPosition];
                case MoveOrientation.HORIZONTAL:
                    if (currentCell.XPosition <= BOARD_START_INDEX)
                    {
                        return null;
                    }
                    return cells[currentCell.YPosition][currentCell.XPosition - 1];
                default:
                    throw new ApplicationException("Unexpected Orientation");
            }
        }
    }
}
