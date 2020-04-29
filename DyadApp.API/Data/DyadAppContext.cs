using System;
using System.Threading;
using System.Threading.Tasks;
using DyadApp.API.Models;
using DyadApp.API.Services;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data
{
    public class DyadAppContext : DbContext
    {
        private readonly IUserService _userService;
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<Signup> Signups { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
        public DyadAppContext(DbContextOptions options, IUserService userService) : base(options)
        {
            _userService = userService;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            PopulateAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void PopulateAudit()
        {
            var userId = _userService.GetUserId();
            var now = DateTime.UtcNow;
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    //TODO: get current user (userId)
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entity.Modified = now;
                            entity.ModifiedBy = userId;
                            break;

                        case EntityState.Added:
                            entity.Created = now;
                            entity.CreatedBy = userId;
                            entity.Modified = now;
                            entity.ModifiedBy = userId;
                            break;
                    }
                }
            }
        }
    }
}
