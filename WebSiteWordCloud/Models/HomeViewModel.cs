using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteWordCloud.Models
{
    public class HomeViewModel
    {
        public List<WeightedWord> Words { get; set; }

        public HomeViewModel(List<WeightedWord> words)
        {
            Words = words;
        }
    }
}
