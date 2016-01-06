using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    public class MoveFinder
    {
        private readonly Board board;
        private readonly Hand hand;
        private readonly WordList wordList;
        private readonly ImmutableDictionary<char, int> availableLetters;

        public MoveFinder(Board board, Hand hand, WordList wordList)
        {
            this.board = board;
            this.hand = hand;
            this.wordList = wordList;
            availableLetters = this.board.GetAvalableLetterCount(hand.Letters).ToImmutableDictionary();
        }

        public IEnumerable<Move> FindMoves()
        {
            return GetPossibleMoves(wordList.Words.Where(s => HasLettersAvailable(s)))
                .OrderByDescending(move => move.Score);
        }

        private IEnumerable<Move> GetPossibleMoves(IEnumerable<string> possibleWords)
        {
            foreach (string word in possibleWords)
            {
                IEnumerable<char> requiredLetters = GetLettersRequiredFromBoard(word);
                IEnumerable<Cell> cellsWithLetters = board.AllCells.Where(c => c.Letter.HasValue);
                if (!cellsWithLetters.Any())
                {
                    foreach (Move m in GetPossibleStartingMoves(word))
                    {
                        yield return m;
                    }
                }
                else if (requiredLetters.Any())
                {
                    foreach (Cell cell in cellsWithLetters.Where(c => requiredLetters.Contains(c.Letter.Value)))
                    {
                        foreach (Move m in GetPossibleMoves(word, cell))
                        {
                            yield return m;
                        }
                    }
                }
                else
                {
                    foreach (Cell cell in cellsWithLetters.Where(c => word.Contains(c.Letter.Value)))
                    {
                        foreach (Move m in GetPossibleMoves(word, cell))
                        {
                            yield return m;
                        }
                    }
                }
            }

            yield break;
        }

        private IEnumerable<Move> GetPossibleStartingMoves(string word)
        {
            for (int x = 0; x < word.Length; x++)
            {
                Move possibleMove = new Move(8 - x, 8, MoveOrientation.HORIZONTAL, word, hand, board);
                if (board.IsValidMove(possibleMove, hand))
                {
                    yield return possibleMove;
                }
            }

            for (int y = 0; y < word.Length; y++)
            {
                Move possibleMove = new Move(8, 8 - y, MoveOrientation.VERTICAL, word, hand, board);
                if (board.IsValidMove(possibleMove, hand))
                {
                    yield return possibleMove;
                }
            }
        }

        private IEnumerable<Move> GetPossibleMoves(string word, Cell cell)
        {
            foreach (int index in word.AllIndexesOf(cell.Letter.Value))
            {
                Move possibleMove = new Move(cell.XPosition - index, cell.YPosition, MoveOrientation.HORIZONTAL, word, hand, board);
                if (board.IsValidMove(possibleMove, hand))
                {
                    yield return possibleMove;
                }
                possibleMove = new Move(cell.XPosition, cell.YPosition - index, MoveOrientation.VERTICAL, word, hand, board);
                if (board.IsValidMove(possibleMove, hand))
                {
                    yield return possibleMove;
                }
            }
            yield break;
        }

        private bool HasLettersAvailable(string word)
        {
            return word
                .GroupBy(c => c)
                .ToDictionary(grp => grp.Key, grp => grp.Count())
                .All(pair => availableLetters.ContainsKey(pair.Key) && availableLetters[pair.Key] >= pair.Value);
        }

        private IEnumerable<char> GetLettersRequiredFromBoard(string word)
        {
            var muttableLetters = new List<char>(this.hand.Letters);
            var requiredLetters = new List<char>(word.Length);
            foreach (char letter in word)
            {
                if (muttableLetters.Contains(letter))
                {
                    muttableLetters.Remove(letter);
                }
                else
                {
                    requiredLetters.Add(letter);
                }
            }
            return requiredLetters;
        }
    }
}
