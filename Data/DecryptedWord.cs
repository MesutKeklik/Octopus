using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class DecryptedWord
    {
        public string SaltedHashWord { get; set; }
        public string Word { get; set; }
        public int Frequency { get; set; }
    }
}
