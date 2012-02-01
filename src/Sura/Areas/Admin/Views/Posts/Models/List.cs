using System;

namespace Sura.Areas.Admin.Views.Posts.Models
{
    public class List
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Author { get; set; }
        public string LastEditedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? TrashedAt { get; set; } 
    }
}