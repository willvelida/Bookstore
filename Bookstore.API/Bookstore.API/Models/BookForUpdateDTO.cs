﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore.API.Models
{
    public class BookForUpdateDTO
    {
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public string Author { get; set; }
    }
}
