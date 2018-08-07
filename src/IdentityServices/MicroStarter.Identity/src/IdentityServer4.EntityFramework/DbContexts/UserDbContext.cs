using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Interfaces;

namespace IdentityServer4.EntityFramework.UserContext
{
    /// <summary>
    /// DbContext for the Users
    /// </summary>
    public class UserDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>, IUserDbContext
    {
        /// <summary>
        /// Constructor of UserDbContext
        /// </summary>
        /// <param name="options"></param>
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }
        /// <summary>
        /// Save Changes Async  
        /// </summary>
        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
        /// <summary>
        /// Override Model Create
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasQueryFilter(p=> !p.IsSoftDeleted);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
                .HasMany(e => e.Users)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<ApplicationUserRole>()
            //     .HasKey(p=> new {p.UserId, p.RoleId});
        }
    }
}