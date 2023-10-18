using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogWithoutAuth.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid[] TagIds { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public  Author Author { get; set; }
        public  Category Category { get; set; }
        public  List<PostTag> PostTags { get; } = new();
        public  List<Tag> Tags { get; set; } = new();  
    }
}