using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Controllers;
using WebApplication1.Migrations;
using WebApplication1.Models.ViewModels;
using WebApplication1.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly BlogDbContext _db;
        private readonly UserManager<BlogIdentityUser> _userManager;
        private readonly SignInManager<BlogIdentityUser> _signInManager;
        public AdminController(BlogDbContext db, UserManager<BlogIdentityUser> userManager, SignInManager<BlogIdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var dashboard = new DashboardViewModel();

            var toplamblogsayisi = _db.Blogs.Count();
            var toplamgoruntulenme = _db.Blogs.Select(x => x.ViewCount).Sum();
            var encokgoruntulneneblog = _db.Blogs.OrderByDescending(x => x.ViewCount).FirstOrDefault();
            var ensonyayinlananblog = _db.Blogs.OrderByDescending(x => x.PublishDate).FirstOrDefault();
            var toplamyorumsayisi = _db.Comments.Count();
            var encokyorumalanblogId = _db.Comments
                                        .GroupBy(x => x.BlogId) // BlogId'ye göre grupla
                                        .OrderByDescending(g => g.Count()) // Grupları yorum sayısına göre azalan sırala
                                        .Select(g => g.Key) // En çok yorumu olan BlogId'yi al
                                        .FirstOrDefault(); // İlk sonucu getir
            var encokyorumalanblog = _db.Blogs.Where(x=> x.Id == encokyorumalanblogId).FirstOrDefault();

            var bugunyapilanyorumsayisi = _db.Comments.Where(x => x.PublishDate.Date == DateTime.Now.Date).Count();

            dashboard.TotalBlogCount = toplamblogsayisi;
            dashboard.TotalViewCount = toplamgoruntulenme;
            dashboard.MostViewedBlog = encokgoruntulneneblog;
            dashboard.LatestBlog = ensonyayinlananblog;
            dashboard.TotalCommentCount = toplamyorumsayisi;
            dashboard.MostCommentedBlog = encokyorumalanblog;
            dashboard.TodayCommentCount = bugunyapilanyorumsayisi;

            return View(dashboard);
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //burada sifre olusturuken buyuk harf kucuk harf sayi 6 karekter ve simge gerekli
            if (model.Password == model.RePassword)
            {
                var user = new BlogIdentityUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        
    }
}