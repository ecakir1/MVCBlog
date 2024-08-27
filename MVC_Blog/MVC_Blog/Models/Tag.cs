namespace MVC_Blog.Models
{
    //Makalelere eklenebilecek etiketler
    public class Tag
    {
        public string TagID { get; set; }
        public string TagName { get; set; }
        public ICollection<Article> Articles { get; set; }

    }
}
