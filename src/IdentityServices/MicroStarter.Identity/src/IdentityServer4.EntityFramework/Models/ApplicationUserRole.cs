using Microsoft.AspNetCore.Identity;

namespace IdentityServer4.Entities
{
    /// Map Many-To-Many Table
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        /// <summary>
        /// User Ref
        /// </summary>
        public virtual ApplicationUser User { get; set; }
        /// <summary>
        /// User Role Ref For Include Queries
        /// </summary>
        public virtual ApplicationRole Role { get; set; }
    }
}