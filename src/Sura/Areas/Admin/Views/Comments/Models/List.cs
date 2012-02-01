using System;

namespace Sura.Areas.Admin.Views.Comments.Models
{
    public class List
    {
        public string PostCommentsId { get; set; }

        public string PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostSlug { get; set; }
        public DateTimeOffset PostPublishedAt { get; set; }

        public Guid CommentId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }

        public DateTimeOffset WrittenAt { get; set; }

        public string UserHostAddress { get; set; }
        public string UserAgent { get; set; }

        public bool Approved { get; set; }
    }
}