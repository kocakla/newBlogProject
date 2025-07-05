using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext _db;

        public BlogController(BlogDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            var blogs = _db.Blogs.ToList();
            return View(blogs);
        }
        public IActionResult Detail(int id)
        {
            var blog = _db.Blogs.Where(x => x.Id == id).FirstOrDefault();
            blog.ViewCount += 1;
            _db.SaveChanges();
            var comment = _db.Comments.Where(x => x.BlogId == id).ToList();
            ViewBag.Comments = comment.ToList();
            return View(blog);
        }
        [HttpPost]
        public IActionResult CreateComment(Comment model)
        {
            model.PublishDate = DateTime.Now;
            _db.Comments.Add(model);

            var blog = _db.Blogs.Where(x => x.Id == model.BlogId).FirstOrDefault();
            blog.CommentCount += 1;

            _db.SaveChanges();
            return RedirectToAction("Detail", new { id = model.BlogId });
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateContact(Contact model)
        {
            model.CreatedAt = DateTime.Now;
            _db.Contacts.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Support()
        {
            return View();
        }
    }
}