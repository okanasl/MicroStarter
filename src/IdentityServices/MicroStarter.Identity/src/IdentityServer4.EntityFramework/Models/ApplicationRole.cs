using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer4.Entities
{
    /// <summary>
    /// Application Role Entity
    /// </summary>
    public class ApplicationRole : IdentityRole<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationRole():base()
        {}
        /// <summary>
        /// Constructor With Name
        /// </summary>
        public ApplicationRole(string name):base(name)
        {}
        /// <summary>
        /// User Roles For Include Queries
        /// </summary>
        public virtual List<ApplicationUserRole> Users { get; } = new List<ApplicationUserRole>();
        
    }
}