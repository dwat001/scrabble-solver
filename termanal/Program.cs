using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kakariki.Scrabble.Logic;

namespace Kakariki.Scrabble.Termanal
{
    class Program
    {
        private readonly Random rand = new Random();
        private const string LETTERS = "eeeeeeeeeeeeaaaaaaaaaiiiiiiiiioooooooonnnnnnrrrrrrttttttllllssssuuuuddddgggbbccmmppffhhvvwwyykjxqz"; 

        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                //p.Test();
                //WordList list = p.LoadWordList("data/3esl.txt");
                WordList list = p.LoadWordList("data/NorthAmericanLongList.txt");
                
                //p.TryHands(list);
                p.PlayAGame(list);
                //p.Foo(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail :(");
                Console.WriteLine(ex.ToString());
            }
            Console.Write("Press any key...");
            Console.Read();
        }

        private void Foo(WordList list)
        {
            foreach (string s in list.Words.Where(w => w.Length >= 2 && w[1] == 'e'))
            {
                Console.WriteLine(s);
            }
        }

        private void Test()
        {
        }

        private void TryHands(WordList list)
        {
            string input;
            do
            {
                Console.Write("Please enter a hand:");
                input = Console.ReadLine();
                if (input != null && input.Length > 0)
                {
                    Console.WriteLine(string.Format("found the following: [{0}]", FindMoves(input, list).Take(10).ToExpandedString()));
                }
            } while (input.Length > 0);
        }

        private WordList LoadWordList(string path)
        {
            WordList list = WordList.Load(new FileInfo(path));

            Console.WriteLine("Loaded Word List:" + list.ToString());
            return list;
        }

        private Board CreateBoard(WordList list)
        {
            return LoadBoard(Board.InitiliseBoard(list), "data/board.txt");
        }

        private Board LoadBoard(Board board, string path)
        {
            int row = 0;
            foreach (string line in File.ReadLines(path))
            {
                if (row == 0) // Skip documentation line
                {
                    row++;
                    continue;
                }
                for (int column = 1; column < line.Length && column <= 15; column++)
                {
                    char c = char.ToLower(line[column]);
                    if (c >= 'a' && c <= 'z')
                    {
                        board.GetCell(column, row).Letter = c;
                    }
                }
                row++;
                if (row > 15)
                {
                    break;
                }
            }
            return board;
        }

        private Hand CreateHand(IEnumerable<char> letters)
        {
            return new Hand(letters);
        }

        private IEnumerable<Move> FindMoves(IEnumerable<char> letters, WordList list)
        {
            Board board = CreateBoard(list);
            Console.WriteLine(board.ToString());
            Hand hand = CreateHand(letters);
            MoveFinder finder = new MoveFinder(board, hand, list);
            return finder.FindMoves();
        }

        private void PlayAGame(WordList list)
        {
            Board board = CreateBoard(list);
            while (true)
            {
                Console.WriteLine(board.ToString(true));
                Console.WriteLine(board.ToString(false));
                
                //Hand hand = CreateRandomHand();
                Hand hand = AskForHand();
                Console.WriteLine(hand);

                Console.Write("Enter for next move");
                Console.ReadLine();

                MoveFinder finder = new MoveFinder(board, hand, list);
                var moves = finder.FindMoves();
                if (!moves.Any())
                {
                    Console.WriteLine("No Moves Found");
                    continue;
                }
                var movesList = moves.Take(10).ToList();
                Console.WriteLine(movesList.ToExpandedString());
                board.Apply(movesList.First());
            }
        }

        private Hand AskForHand()
        {
            Console.Write("Please enter a hand:");
            string letters = Console.ReadLine();
            return CreateHand(letters);
        }

        private Hand CreateRandomHand()
        {
            List<char> hand = new List<char>(7);
            for (int i = 0; i < 7; i++)
            {
                hand.Add(LETTERS[rand.Next(0, LETTERS.Length)]);
            }
            return CreateHand(hand);
        }
    }
}
