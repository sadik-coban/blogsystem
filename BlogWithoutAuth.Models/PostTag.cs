using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWithoutAuth.Models
{
    public class PostTag
    {
        public Guid PostId { get; set; }
        public Guid TagsId { get; set; }
        public required virtual Post Post { get; set; }
        public required virtual Tag Tag { get; set; }
    }
}
