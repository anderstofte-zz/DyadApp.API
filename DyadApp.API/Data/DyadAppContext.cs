using DyadApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data
{
    public class DyadAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<Signup> Signups { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DyadAppContext(DbContextOptions options) : base(options)
        {
        }
    }
}