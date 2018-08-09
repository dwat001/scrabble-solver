using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace Kakariki.Scrabble.Logic
{
    public class Hand
    {
        private const int MAX_HAND_SIZE = 7;
        private ImmutableList<char> letters;

        public Hand(IEnumerable<char> letters)
        {
            this.letters = new List<char>(letters).ToImmutableList();
            if (this.letters.Count() > MAX_HAND_SIZE)
            {
                throw new ApplicationException("Too many letters");
            }
        }

        public ImmutableList<char> Letters
        {
            get { return letters; }
        }

        public override string ToString()
        {
            return "Hand " + letters.ToExpandedString();
        }
    }
}
