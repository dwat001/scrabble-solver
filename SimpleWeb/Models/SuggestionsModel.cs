using Kakariki.Scrabble.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Web;

namespace Kakariki.Scrabble.SimpleWeb.Models
{
    public class SuggestionsModel
    {
        IEnumerable<Move> move = ImmutableList<Move>.Empty;

        public IEnumerable<Move> Moves
        {
            get { return move; }
            set { move = value ?? ImmutableList<Move>.Empty; }
        }
    }
}