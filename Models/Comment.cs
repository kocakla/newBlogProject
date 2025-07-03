namespace WebApplication1.Models
{
    public class Comment
    {
        public int BlogId { get; set; }
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }

    }
}