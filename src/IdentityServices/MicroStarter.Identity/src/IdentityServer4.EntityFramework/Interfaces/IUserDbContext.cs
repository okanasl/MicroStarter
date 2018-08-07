using System;
using System.Threading.Tasks;
using IdentityServer4.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.EntityFramework.Interfaces
{
    /// <summary>
    /// Interface of UserDbContext
    /// </summary>
    public interface IUserDbContext : IDisposable
    {
        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>
        /// The clients.
        /// </value>
        DbSet<ApplicationUser> Users { get; set; }        
        
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        DbSet<ApplicationRole> Roles { get; set; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}