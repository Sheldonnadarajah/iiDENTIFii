using System;
using System.Collections.Generic;

namespace AspireApp1.ApiService.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
    }
}
