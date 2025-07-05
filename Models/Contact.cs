namespace WebApplication1.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        

    }
}