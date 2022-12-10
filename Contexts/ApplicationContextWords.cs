using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LanguageLearnBot_.NET.Contexts
{
    public class ApplicationContextWords : DbContext
    {
        public DbSet<WordEnt> WordEnt => Set<WordEnt>();
        public ApplicationContextWords() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=words.db");
        }
    }
}
