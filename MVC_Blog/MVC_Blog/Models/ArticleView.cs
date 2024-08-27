namespace MVC_Blog.Models
{
    public class ArticleView
    {
        public string ArticleViewID { get; set; }
        public string ArticleID { get; set; }
        public Article Article { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime ViewDate { get; set; }
    }
}
