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
            var comment = _db.Comments.Where(x => x.BlogId == id).ToList();
            ViewBag.Comments = comment.ToList();
            return View(blog);
        }
        [HttpPost]
        public IActionResult CreateComment(Comment model)
        {
            model.PublishDate = DateTime.Now;
            _db.Comments.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Detail", new {id = model.BlogId});
        }
    }
}