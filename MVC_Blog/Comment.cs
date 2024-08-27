namespace MVC_Blog.Models
{
    public class Comment
    {
        public string CommentID { get; set; }
        public string Content { get; set; }
        public string ArticleID { get; set; }
        public Article Article { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLiked { get; set; }
    }
}
