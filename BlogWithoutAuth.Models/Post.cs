using System.Text.Json.Serialization;

namespace BlogWithoutAuth.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required virtual Author Author { get; set; }
        public required virtual Category Category { get; set; }
        public virtual List<Tag> Tags { get; set; } = new();
        public required string[] TagString { get; set; }  
    }
}