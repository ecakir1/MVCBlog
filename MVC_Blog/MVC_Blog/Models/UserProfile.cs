namespace MVC_Blog.Models
{
    public class UserProfile
    {
        public string UserProfileID { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public string RoleID { get; set; }
        public Role Role { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<Category> FavoriteCategories { get; set; }
    }
}
