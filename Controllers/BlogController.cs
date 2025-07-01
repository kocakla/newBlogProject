using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;

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
            return View(blog);
        }
    }
}