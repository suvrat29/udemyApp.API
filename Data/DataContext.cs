using Microsoft.EntityFrameworkCore;
using udemyApp.API.Models;

namespace udemyApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Errorlog> Errorlogs { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}