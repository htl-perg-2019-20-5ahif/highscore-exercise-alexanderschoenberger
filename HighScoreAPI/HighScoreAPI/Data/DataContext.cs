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
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration;

        /*   protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               modelBuilder.Entity<HighScore>();
               var highscore = modelBuilder.Entity<HighScore>().Metadata;
               highscore.CosmosSql().CollectionName = nameof(HighScore);
           }*/

        /*   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           {
               optionsBuilder.UseCosmos(Configuration["CosmosDB:AccountEndpoint"],
        Configuration["CosmosDB:AccountKey"], Configuration["CosmosDB:DatabaseName"],
    options =>
    {
        options.ExecutionStrategy(d => new CosmosExecutionStrategy(d));
    });
           }*/
    }
}
