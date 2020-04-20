using DyadApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data
{
    public class DyadAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DyadAppContext(DbContextOptions options) : base(options)
        {
        }
    }
}