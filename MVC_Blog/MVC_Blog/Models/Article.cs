using System;
using System.Collections.Generic;

namespace MVC_Blog.Models
{
    public class Article
    {
        public string ArticleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorID { get; set; }
        public ApplicationUser Author { get; set; }
        public string CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleView> ArticleViews { get; set; }

        //Makale için eklediğim ek özellikler
        public string ImageUrl { get; set; }
        public int CommentCount { get; set; }
    }
}
