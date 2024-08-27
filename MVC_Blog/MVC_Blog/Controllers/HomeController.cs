using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Blog.Data;
using MVC_Blog.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            //SeedArticles();
        }
        /*
        private void SeedArticles()
        {
            if (!_context.Articles.Any())
            {
                var articles = new List<Article>
                {
                    new Article { ArticleID = "1", ImageUrl = "path/to/image1.jpg", Title = "Literature 1", Tags = new List<Tag> { new Tag { TagName = "Literature" } }, Comments = new List<Comment> { new Comment(), new Comment() } },
                    new Article { ArticleID = "2", ImageUrl = "path/to/image2.jpg", Title = "Literature 2", Tags = new List<Tag> { new Tag { TagName = "Literature" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "3", ImageUrl = "path/to/image3.jpg", Title = "Art 1", Tags = new List<Tag> { new Tag { TagName = "Art" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "4", ImageUrl = "path/to/image4.jpg", Title = "Art 2", Tags = new List<Tag> { new Tag { TagName = "Art" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "5", ImageUrl = "path/to/image5.jpg", Title = "Computer Science 1", Tags = new List<Tag> { new Tag { TagName = "Computer Science" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "6", ImageUrl = "path/to/image6.jpg", Title = "Computer Science 2", Tags = new List<Tag> { new Tag { TagName = "Computer Science" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "7", ImageUrl = "path/to/image7.jpg", Title = "Computer Science 3", Tags = new List<Tag> { new Tag { TagName = "Computer Science" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "8", ImageUrl = "path/to/image8.jpg", Title = "Science 1", Tags = new List<Tag> { new Tag { TagName = "Science" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "9", ImageUrl = "path/to/image9.jpg", Title = "Science 2", Tags = new List<Tag> { new Tag { TagName = "Science" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "10", ImageUrl = "path/to/image10.jpg", Title = "Freestyle 1", Tags = new List<Tag> { new Tag { TagName = "Freestyle" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "11", ImageUrl = "path/to/image11.jpg", Title = "Freestyle 2", Tags = new List<Tag> { new Tag { TagName = "Freestyle" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "12", ImageUrl = "path/to/image12.jpg", Title = "Freestyle 3", Tags = new List<Tag> { new Tag { TagName = "Freestyle" } }, Comments = new List<Comment> { new Comment() } },
                    new Article { ArticleID = "13", ImageUrl = "path/to/image13.jpg", Title = "Freestyle 4", Tags = new List<Tag> { new Tag { TagName = "Freestyle" } }, Comments = new List<Comment> { new Comment() } }
                };

                _context.Articles.AddRange(articles);
                _context.SaveChanges();
            }
        }
        */

        public async Task<IActionResult> Index(string category = "All")
        {
            var articlesQuery = _context.Articles
                 .Include(a => a.Tags)
                 .Include(a => a.Comments)
                 .AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                articlesQuery = articlesQuery.Where(a => a.Tags.Any(t => t.TagName == category));
            }

            var articles = await articlesQuery.ToListAsync();

            ViewData["SelectedCategory"] = category;

            return View(articles);
        }

        public async Task<IActionResult> ArticleDetails(string id)
        {
            var article = await _context.Articles
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.ArticleID == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string articleId, string commentText)
        {
            var article = await _context.Articles
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.ArticleID == articleId);

            if (article == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = commentText,
                UserID = User.Identity.Name ?? "Anonymous",
                ArticleID = articleId
            };

            article.Comments.Add(comment);
            article.CommentCount = article.Comments.Count;

            _context.Update(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArticleDetails", new { id = articleId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
