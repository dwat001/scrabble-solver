using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Web;

using Kakariki.Scrabble.Logic;
using Microsoft.AspNetCore.Builder;

namespace Kakariki.Scrabble.SimpleWeb.Configuration
{
    public static class WordListConfigExtension
    {
        public static void UseWordList(this IApplicationBuilder app) 
        {
            WordListConfig.RegisterWordLists();
        }
    }

    public class WordListConfig
    {
        public static IImmutableDictionary<string, WordList> Lists { get; private set; }

        public const string BRITISH_TWO_OF_FOUR = "2of4brif.txt";
        public const string ENGLISH_AS_A_SECOND_LANGUAGE = "3esl.txt";

        public static void SetWordLists(IEnumerable<KeyValuePair<string, WordList>> items)
        {
            var builder = ImmutableDictionary.CreateBuilder<string, WordList>();
            builder.AddRange(items);
            Lists = builder.ToImmutableDictionary();
        } 

        internal static void RegisterWordLists()
        {
            string dataDir = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            DirectoryInfo directory = new DirectoryInfo(dataDir);
            directory.GetFiles("*.txt");
            var builder = ImmutableDictionary.CreateBuilder<string, WordList>();
            builder.AddRange(
                from file in directory.GetFiles("*.txt")
                select new KeyValuePair<string, WordList>(file.Name.ToLower(), WordList.Load(file)));
            Lists = builder.ToImmutableDictionary();
        }


    }
}