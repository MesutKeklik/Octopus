using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class WeightedWord
    {
        [JsonProperty(PropertyName = "size")]
        public int Weight { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Word { get; set; }
    }
}
