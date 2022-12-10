using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entity_Framework;

namespace LanguageLearnBot_.NET.Contexts
{


    public class ApplicationContext : DbContext
    {
        public DbSet<UserLanguageLearner> UserLanguageLearner => Set<UserLanguageLearner>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=telegramusers.db");
        }
    }




}
