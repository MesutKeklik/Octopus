using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Helper
{
    public static class Extractor
    {
        public static List<string> GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select ValidWord(m.Value);

            return words.Where(w=>!string.IsNullOrEmpty(w)).ToList();
        }

        public static string ValidWord(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return !IsSpecialMeaningWord(word) ? word : "";
        }

        private static bool IsSpecialMeaningWord(string word)
        {
            var articles = new List<string>{ "a", "an", "the" };
            var prepositions = new List<string> { //https://www.talkenglish.com/vocabulary/top-50-prepositions.aspx 
                "of", "with", "at", "from", "into", "during", "including", "until", "against", "among",
                "throughout", "despite", "towards", "upon", "concerning", "to", "in", "for", "on", "by",
                "about", "like", "through", "over", "before", "between", "after", "since", "without", "under",
                "within", "along", "following", "across", "behind", "beyond", "plus", "except", "but",
                "up", "out", "around", "down", "off", "above", "near"
            };
            var htmlWords = new List<string> { "nbsp" };
            return articles.Contains(word) || prepositions.Contains(word) || word.Any(char.IsDigit) || htmlWords.Contains(word);
        }
    }
}
