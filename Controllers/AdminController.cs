using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Controllers;
using WebApplication1.Migrations;


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
            var blogs = _db.Blogs.Where(x => x.Id == id).FirstOrDefault();
            if (blogs == null)
            {
                return NotFound(); // veya: return RedirectToAction("BlogList");
            }
            return View(blogs);
        }
        public IActionResult DeleteBlog(int id)
        {
            var blogs = _db.Blogs.Where(x => x.Id == id).FirstOrDefault();
            _db.Blogs.Remove(blogs);
            _db.SaveChanges();
            return RedirectToAction("BlogList");

        }
        [HttpPost]
        public IActionResult EditBlog(Blog model)
        {
            var blog = _db.Blogs.Where(x => x.Id == model.Id).FirstOrDefault();
            if (blog == null)
            {
                return NotFound();
            }
            blog.Name = model.Name;
            blog.Description = model.Description;
            blog.Tags = model.Tags;
            blog.ImageUrl = model.ImageUrl;
            _db.SaveChanges();
            return RedirectToAction("BlogList");
        }
        public IActionResult ToggleStatus(int id)
        {
            var blog = _db.Blogs.Where(x => x.Id == id).FirstOrDefault();
            if (blog.Status == 1)
            {
                blog.Status = 0;
            }
            else
            {
                blog.Status = 1;
            }
            _db.SaveChanges();
            return RedirectToAction("BlogList");
        }
        public IActionResult CreateBlog()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateBlog(Blog model)
        {
            model.PublishDate = DateTime.Now;
            model.Status = 1;
            _db.Blogs.Add(model);
            _db.SaveChanges();
            return RedirectToAction("BlogList");
        }
        public IActionResult Comments(int? blogId)
        {
            var comments = new List<Comment>();
            if (blogId == null)
            {
                comments = _db.Comments.ToList();
            }
            else
            {
                comments = _db.Comments.Where(x => x.BlogId == blogId).ToList();
            }

            return View(comments);
        }
        public IActionResult DeleteComment(int id)
        {
            var comment = _db.Comments.Where(x => x.Id == id).FirstOrDefault();
            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return RedirectToAction("Comments");
        }

    }
}