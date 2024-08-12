using Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Calculation> Calculations { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
