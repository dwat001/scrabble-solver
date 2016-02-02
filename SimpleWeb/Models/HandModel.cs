using Kakariki.Scrabble.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kakariki.Scrabble.SimpleWeb.Models
{
    public class HandModel
    {
        public HandModel()
        {
            Letters = "";
        }

        public HandModel(string letters):this()
        {
            Letters = letters;
        }

        private string letters;
        public string Letters
        {
            get { return letters; }
            set
            {
                letters = (value ?? "").ToLower();
            }
        }

        public Hand Hand
        {
            get
            {
                return new Hand(Letters);
            }
        }
    }
}