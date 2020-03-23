using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace HighScoreAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<HighScore> HighScores { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
      : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HighScore>()
                .HasKey(h => h.HighScoreId);
        }

    }
}
