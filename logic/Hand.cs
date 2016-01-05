using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    public class Hand
    {
        private const int MAX_HAND_SIZE = 70;
        private List<char> letters;

        public Hand(IEnumerable<char> letters)
        {
            this.letters = new List<char>(letters);
            if (this.letters.Count() > MAX_HAND_SIZE) { throw new ApplicationException("Too many letters"); }
        }

        public ImmutableList<char> Letters
        {
            get { return letters.ToImmutableList(); }
        }

        public override string ToString()
        {
            return "Hand " + letters.ToExpandedString() ;
        }
    }
}
