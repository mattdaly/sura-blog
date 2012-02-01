using System;
using System.Collections.Generic;

namespace Sura.Models
{
    public class PostComments
    {
        public string Id { get; private set; }

        public string PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostSlug { get; set; }
        public DateTimeOffset PostPublishedAt { get; set; }

        public List<Comment> ApprovedComments { get; set; }
        public List<Comment> UnapprovedComments { get; set; }

        public PostComments(string postId, string postTitle, string postSlug, DateTimeOffset postPublishedAt)
        {
            Id = "post-comments/";

            PostId = postId;
            PostTitle = postTitle;
            PostSlug = postSlug;
            PostPublishedAt = postPublishedAt;

            ApprovedComments = new List<Comment>();
            UnapprovedComments = new List<Comment>();
        } 
    }
}