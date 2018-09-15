using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class Word
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SaltedHashWord { get; set; }
        public byte[] EncyrptedWord { get; set; }
        public int Frequency { get; set; }
    }
}
