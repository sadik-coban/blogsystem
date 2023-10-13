using System.Text.Json.Serialization;

namespace BlogWithoutAuth.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<PostTag> PostTags { get; } = new();
        public virtual List<Tag> Tags { get; set; } = new();
        public required string[] TagsString { get; set; }
    }
}