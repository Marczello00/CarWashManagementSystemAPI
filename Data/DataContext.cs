using CarWashManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagementSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionSource> TransactionSources { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationType> StationTypes { get; set; }
    }
}
