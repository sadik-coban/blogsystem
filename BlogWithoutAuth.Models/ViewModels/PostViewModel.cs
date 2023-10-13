using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWithoutAuth.Models.ViewModels
{
    public class PostViewModel
    {
        public required Post Post { get; set; }
        public required string[] Tags { get; set; }
    }
}
