using Model.Helper;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class WebPageContent
    {
        public HtmlDocument OriginalDocument { get; set; }
        public List<WeightedWord> Words { get; set; }

        public WebPageContent(HtmlDocument document)
        {
            OriginalDocument = document;
            Words = FetchWords(document);
        }

        private List<WeightedWord> FetchWords(HtmlDocument document)
        {
            var nodes = document.DocumentNode.SelectSingleNode("//body").DescendantsAndSelf();

            List<string> words = new List<string>();
            foreach (var node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Text && node.ParentNode.Name != "script")
                {
                    var validWords = Extractor.GetWords(node.InnerText.ToLower());
                    if (validWords.Count > 0)
                        words.AddRange(validWords);
                }
            }

            var groups = (from w in words
                         group w by w into dict
                         select
                        new { Word = dict.Key, Count = dict.Count() }).OrderByDescending(w=>w.Count).Take(100);

            var weightedWords = new List<WeightedWord>();
            foreach (var item in groups)
            {
                weightedWords.Add(new WeightedWord { Word = item.Word, Weight = item.Count });
            }

            return weightedWords;
        }


    }
}
