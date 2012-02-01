using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sura.Views.Blog.Models
{
    public class List
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public MvcHtmlString Content { get; set; }
        public List<string> Tags { get; set; }
        public string Author { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }

        public bool EnableComments { get; set; }
        public int Comments { get; set; }
    }
}