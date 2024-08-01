using CarWashManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagementSystem.Data
{
    public class DataContext : IdentityDbContext<IdentityUser> 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionSource> TransactionSources { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationType> StationTypes { get; set; }
        public DbSet<StationAllowedIp> StationAllowedIps { get; set; }
        public DbSet<FiscSchedule> FiscSchedule { get; set; }
    }
}
