using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Identity
{
    public class BlogIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}