using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Blog.Data;
using MVC_Blog.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
            new Article { ArticleID = "1", AuthorID = "1", CategoryID = "1", Content = "Content 1", ImageUrl = "path/to/image1.jpg", Title = "Literature 1", Tags = new List<Tag> { new Tag { TagID= "1", TagName = "Literature" } } },
            new Article { ArticleID = "2", AuthorID = "1", CategoryID = "1", Content = "Content 2", ImageUrl = "path/to/image2.jpg", Title = "Literature 2", Tags = new List<Tag> { new Tag { TagID = "2", TagName = "Literature" } }},
            new Article { ArticleID = "3", AuthorID = "1", CategoryID = "2", Content = "Content 3", ImageUrl = "path/to/image3.jpg", Title = "Art 1", Tags = new List<Tag> { new Tag { TagID = "3", TagName = "Art" } } },
            new Article { ArticleID = "4", AuthorID = "1", CategoryID = "2", Content = "Content 4", ImageUrl = "path/to/image4.jpg", Title = "Art 2", Tags = new List<Tag> { new Tag { TagID = "4", TagName = "Art" } } },
            new Article { ArticleID = "5", AuthorID = "1", CategoryID = "3", Content = "Content 5", ImageUrl = "path/to/image5.jpg", Title = "Computer Science 1", Tags = new List<Tag> { new Tag { TagID = "5", TagName = "Computer Science" } } },
            new Article { ArticleID = "6", AuthorID = "1", CategoryID = "3", Content = "Content 6", ImageUrl = "path/to/image6.jpg", Title = "Computer Science 2", Tags = new List<Tag> { new Tag { TagID = "6", TagName = "Computer Science" } } },
            new Article { ArticleID = "7", AuthorID = "1", CategoryID = "3", Content = "Content 7", ImageUrl = "path/to/image7.jpg", Title = "Computer Science 3", Tags = new List<Tag> { new Tag { TagID = "7", TagName = "Computer Science" } } },
            new Article { ArticleID = "8", AuthorID = "1", CategoryID = "4", Content = "Content 8", ImageUrl = "path/to/image8.jpg", Title = "Science 1", Tags = new List<Tag> { new Tag { TagID = "8", TagName = "Science" } } },
            new Article { ArticleID = "9", AuthorID = "1", CategoryID = "4", Content = "Content 9", ImageUrl = "path/to/image9.jpg", Title = "Science 2", Tags = new List<Tag> { new Tag { TagID = "9", TagName = "Science" } } },
            new Article { ArticleID = "10", AuthorID = "1", CategoryID = "5", Content = "Content 10", ImageUrl = "path/to/image10.jpg", Title = "Freestyle 1", Tags = new List<Tag> { new Tag { TagID = "10", TagName = "Freestyle" } } },
            new Article { ArticleID = "11", AuthorID = "1", CategoryID = "5", Content = "Content 11", ImageUrl = "path/to/image11.jpg", Title = "Freestyle 2", Tags = new List<Tag> { new Tag { TagID = "11", TagName = "Freestyle" } } },
            new Article { ArticleID = "12", AuthorID = "1", CategoryID = "5", Content = "Content 12", ImageUrl = "path/to/image12.jpg", Title = "Freestyle 3", Tags = new List<Tag> { new Tag { TagID = "12", TagName = "Freestyle" } } },
            new Article { ArticleID = "13", AuthorID = "1", CategoryID = "5", Content = "Content 13", ImageUrl = "path/to/image13.jpg", Title = "Freestyle 4", Tags = new List<Tag> { new Tag { TagID = "13", TagName = "Freestyle" } } }
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
                .Include(a => a.Tags)
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.ArticleID == id);

            if (article == null)
            {
                return NotFound();
            }

            if (article.Tags == null)
            {
                _logger.LogError("Tags property is null for article with ID: {ArticleID}", id);
                article.Tags = new List<Tag>(); // Initialize to avoid null reference
            }

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string articleId, string commentText)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var article = await _context.Articles
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.ArticleID == articleId);

            if (article == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the authenticated user's ID

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var comment = new Comment
            {
                Content = commentText,
                UserID = userId,
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
