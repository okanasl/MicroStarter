using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer4.Entities
{
    /// <summary>
    /// Application User Entity
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Application User Firstname
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Application User Lastname
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Application User Fullname
        /// </summary>
        public string FullName { get => FirstName == null ? null :  FirstName + " "+ LastName; }
        /// <summary>
        /// Soft Delete Property
        /// </summary>
        public bool IsSoftDeleted { get; set; }

        /// <summary>
        /// Profile Image Url
        /// </summary>
        public string ProfileImage { get; set; }
        /// <summary>
        /// Application User Register Date
        /// </summary>
        public DateTime DateAdd { get; set; }

        /// <summary>
        /// Application User Last Login Date
        /// </summary>
        public DateTime DateActive { get; set; }

        /// <summary>
        /// Application User Last Login Date
        /// </summary>
        public DateTime DateModify { get; set; }
        /// <summary>
        /// Application User Constructor
        /// </summary>
        public ApplicationUser()
        {
        }
        /// <summary>
        /// Application User Constructor
        /// </summary>
        public ApplicationUser(string userName) : base(userName)
        {
            
        }
        /// <summary>
        /// User Roles For Include Reference Queries
        /// </summary>
        public List<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();
        /// <summary>
        /// User Claims For Include Reference Queries
        /// </summary>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
        /// <summary>
        /// User Logins For Include  Reference Queries
        /// </summary>
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = new List<IdentityUserLogin<string>>();
    }
}