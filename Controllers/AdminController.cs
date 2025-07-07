using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly BlogDbContext _db;
        public AdminController(BlogDbContext db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BlogList()
        {
            var blogs = _db.Blogs.ToList();

            return View(blogs);
        }
        public IActionResult EditBlog(int id)
        {
            var blog = _db.Blogs.Where(x => x.Id == id).FirstOrDefault();
            return View(blog);
        }
    }
}