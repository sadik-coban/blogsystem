﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWithoutAuth.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
