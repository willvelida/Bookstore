﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore.API.Models
{
    public class Book
    {
        [JsonProperty(PropertyName ="id")]
        public string Id { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}