using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    public class Move
    {
        public int Column { get; }
        public int Row { get; }
        public MoveOrientation Orientation { get; }
        public string Word { get; }
        private Board Board {get;}
        private Hand Hand {get;}

        private int? score;

        public Move(int column, int row, MoveOrientation orientation, string word, Hand hand, Board board)
        {
            this.Column = column;
            this.Row = row;
            this.Orientation = orientation;
            this.Word = word;
            this.Hand = hand;
            this.Board = board;
        }

        public int Score
        {
            get
            {
                if (!score.HasValue)
                {
                    int workInProgress = 0;
                    int wordMultiplier = 1;
                    var currentBoardPosition = new Tuple<int, int>(Column, Row);
                    var lettersInHand = new List<char>(Hand.Letters);

                    foreach (char c in Word)
                    {
                        Cell currentCell = Board.GetCell(currentBoardPosition);
                        if (currentCell.Letter.HasValue)
                        {
                            Debug.Assert(currentCell.Letter == c);
                            workInProgress += GetSimpleLetterScore(c);
                        }
                        else
                        {
                            Debug.Assert(lettersInHand.Remove(c));

                            int letterScore, wordMultiplierFromLetter;
                            GetLetterEffect(c, currentCell.Type, out letterScore, out wordMultiplierFromLetter);
                            workInProgress += letterScore;
                            wordMultiplier *= wordMultiplierFromLetter;

                            workInProgress += AdditionalWordScores(currentCell, letterScore, wordMultiplierFromLetter, c);
                        }
                        currentBoardPosition = AdvancePosition(currentBoardPosition, Orientation);
                    }
                    int bonus = (Hand.Letters.Count == 7 && lettersInHand.Count == 0) ? 50 : 0;
                    score = (workInProgress * wordMultiplier) + bonus;
                }
                
                return score.Value;
            }
        }

        private int AdditionalWordScores(Logic.Cell currentCell, int letterScore, int wordMultiplierFromLetter, char placedLetter)
        {
            // Find Additional words 
            string word = Board.GetWord(InverseOrientation, currentCell, placedLetter);
            if (word.Length < 2)
            {
                return 0;
            }
            // And Scores thme using the special letters
            int additional = (word.Sum(c => GetSimpleLetterScore(c)) - GetSimpleLetterScore(placedLetter) + letterScore);
            return additional * wordMultiplierFromLetter;
        }

        private MoveOrientation InverseOrientation
        {
            get { return Orientation == MoveOrientation.HORIZONTAL ? MoveOrientation.VERTICAL : MoveOrientation.HORIZONTAL; }
        }

        private void GetLetterEffect(char c, CellType cellType, out int letterScore, out int wordMultiplierFromLetter)
        {
            int simpleLetterScore = GetSimpleLetterScore(c);

            switch (cellType)
            {
                case CellType.DOUBLE_LETTER:
                    letterScore = 2 * simpleLetterScore;
                    wordMultiplierFromLetter = 1;
                    return;
                case CellType.TRIPPLE_LETTER:
                    letterScore = 3 * simpleLetterScore;
                    wordMultiplierFromLetter = 1;
                    return;
                case CellType.DOUBLE_WORD:
                    letterScore = simpleLetterScore;
                    wordMultiplierFromLetter = 2;
                    return;
                case CellType.TRIPPLE_WORD:
                    letterScore = simpleLetterScore;
                    wordMultiplierFromLetter = 3;
                    return;
                case CellType.NORMAL:
                    letterScore = simpleLetterScore;
                    wordMultiplierFromLetter = 1;
                    return;
                default:
                    throw new ApplicationException("Unkown CellType");
            }
        }

        private int AdditionalWordScores(char c, Cell currentCell)
        {
            throw new NotImplementedException();
        }

        #region GetSimpleLetterScore
        private static int GetSimpleLetterScore(char c)
        {
            switch (c)
            {
                case 'a':
                    return 1;
                case 'b':
                    return 3;
                case 'c':
                    return 3;
                case 'd':
                    return 2;
                case 'e':
                    return 1;
                case 'f':
                    return 4;
                case 'g':
                    return 2;
                case 'h':
                    return 4;
                case 'i':
                    return 1;
                case 'j':
                    return 8;
                case 'k':
                    return 5;
                case 'l':
                    return 1;
                case 'm':
                    return 3;
                case 'n':
                    return 1;
                case 'o':
                    return 1;
                case 'p':
                    return 3;
                case 'q':
                    return 10;
                case 'r':
                    return 1;
                case 's':
                    return 1;
                case 't':
                    return 1;
                case 'u':
                    return 1;
                case 'v':
                    return 4;
                case 'w':
                    return 4;
                case 'x':
                    return 8;
                case 'y':
                    return 4;
                case 'z':
                    return 10;
                default:
                    throw new ApplicationException("No such letter I'm sure " + c);
            }
        }
        #endregion

        public char? GetLetterAt(int row, int column)
        {
            switch (Orientation)
            {
                case MoveOrientation.HORIZONTAL:
                    if (row != Row || column < Column || column > Column + Word.Length -1)
                    {
                        return null;
                    }
                    return Word[column - Column];
                case MoveOrientation.VERTICAL:
                    if (column != Column || row < Row || row > Row + Word.Length -1)
                    {
                        return null;
                    }
                    return Word[row - Row];
                default:
                    throw new ApplicationException("Unkown Orientation");
            }
        }

        private static Tuple<int, int> AdvancePosition(Tuple<int, int> current, MoveOrientation orientation)
        {
            switch (orientation)
            {
                case MoveOrientation.HORIZONTAL:
                    return new Tuple<int, int>(current.Item1 + 1, current.Item2);
                case MoveOrientation.VERTICAL:
                    return new Tuple<int, int>(current.Item1, current.Item2 + 1);
                default:
                    throw new ApplicationException("Unkown Orientation.");
            }
        }

        public override string ToString()
        {
            return string.Format("{4} - ({0},{1}), {2} - {3}", Column, Row, Orientation, Word, Score);
        }

        public bool Equals(Move other)
        {
            return Object.ReferenceEquals(this, other) ||( 
               other != null && 
               other.Orientation == this.Orientation &&
               other.Column == this.Column &&
               other.Row == this.Row &&
               other.Word == this.Word &&
               object.ReferenceEquals(other.Board, this.Board) &&
               object.ReferenceEquals(other.Hand, this.Hand));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Move);
        }

        public static bool operator ==(Move a, Move b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(Move a, Move b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Row, Column, Word, Orientation).GetHashCode();
        }      
    }

    public enum MoveOrientation
    {
        HORIZONTAL,
        VERTICAL
    }
}
