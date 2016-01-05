using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    // TODO: Unit test
    public class WordList
    {
        private readonly Dictionary<char, IList<string>> wordsByStartingLetter;
        private readonly ImmutableList<string> words;

        private WordList(Dictionary<char, IList<string>> wordsByStartingLetter, IList<string> words)
        {
            this.wordsByStartingLetter = wordsByStartingLetter;
            this.words = words.ToImmutableList();
        }

        public ImmutableList<string> Words { get { return words; } }

        public bool Contains(string candidate)
        {
            return words.BinarySearch(candidate) > -1;
        }

        public static WordList Load(FileInfo file)
        {
            var wordsByStartingLetter = new Dictionary<char, IList<string>>();
            var words = ImmutableList.CreateBuilder<string>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                wordsByStartingLetter[c] = new List<string>(512);
            }
            // Could also use File.ReadLines
            File.ReadLines(file.FullName)
                .Where(s => s.Length > 0 && !s.Contains('-') && !s.Contains('\''))
                .Select(s => s.ToLower())
                .All(s => {
                    wordsByStartingLetter[s[0]].Add(s);
                    words.Add(s);
                    return true;
                });
            return new WordList(wordsByStartingLetter, words.ToImmutable());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("WordList[");
            // TODO: can this be better done with link?
            for (char c = 'a'; c <= 'z'; c++)
            {
                sb.AppendFormat("{0}={1}, ", c, wordsByStartingLetter[c].Count);
            }
            sb.Append("]");
            return sb.ToString();
        }

    }
}
