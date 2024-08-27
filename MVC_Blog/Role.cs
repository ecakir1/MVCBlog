using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MVC_Blog.Models
{
    public class Role : IdentityRole
    {
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
