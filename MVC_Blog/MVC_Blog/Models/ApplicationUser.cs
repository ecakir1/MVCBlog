using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MVC_Blog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? InterestedCategory { get; set; }
        public UserProfile UserProfile { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<ArticleView> ArticleViews { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
