using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kakariki.Scrabble.SimpleWeb.Models
{
    public class SuggestionRequestModel
    {
        public BoardModel Board {get;set;}
        public HandModel Hand { get; set;}
        public SuggestionsModel Suggestions { get; set; }
    }
}