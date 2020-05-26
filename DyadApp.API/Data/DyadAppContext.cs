using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DyadApp.API.Extensions;
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
        public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<AwaitingMatch> AwaitingMatches { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<UserMatch> UserMatches { get; set; }
        
        public DyadAppContext(DbContextOptions options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            PopulateAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void PopulateAudit()
        {
            var userId = ClaimsPrincipal.Current.GetUserId();
            var now = DateTime.Now;
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is EntityBase entity)
                {
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
