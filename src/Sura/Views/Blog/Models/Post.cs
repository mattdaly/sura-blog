using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sura.Views.Blog.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public MvcHtmlString Body { get; set; }
        public List<string> Tags { get; set; }
        public string Author { get; set; }
        public DateTimeOffset? Published { get; set; }

        public bool EnableComments { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
    }
}