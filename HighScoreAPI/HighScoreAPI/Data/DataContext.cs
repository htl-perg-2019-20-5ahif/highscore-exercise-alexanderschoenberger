using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.IO;

namespace HighScoreAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<HighScore> HighScores { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
      : base(options)
        {
            //Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultContainer("HighScores");
            modelBuilder.Entity<HighScore>().HasNoDiscriminator();
        }
    }
}
