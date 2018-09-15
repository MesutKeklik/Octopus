using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{

    public class ApplicationDBContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<KeyPair> KeyPairs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        => builder.UseMySql("Server=104.197.237.247;port=3306;database=WebSiteWordFrequency;uid=mesut;pwd=Deneme11");
    }
    
}
