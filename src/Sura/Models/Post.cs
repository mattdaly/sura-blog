using System;
using System.Collections.Generic;

namespace Sura.Models
{
    public class Post
    {
        public string Id { get; protected internal set; }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public List<string> Tags { get; set; }

        public string Author { get; set; }

        public DateTimeOffset CreatedAt { get; protected internal set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? TrashedAt { get; set; }

        public DateTimeOffset? EditedAt { get; set; }
        public string LastEditedBy { get; set; }

        public bool EnableComments { get; set; }
        public int NumberOfComments { get; set; }

        public Post(string title, string slug, string author, string body)
        {
            Id = "posts/";

            Title = title;
            Slug = slug;
            Author = author;

            Body = body;

            Tags = new List<string>();

            CreatedAt = DateTimeOffset.UtcNow;
            PublishedAt = null;
        }

        public void Publish()
        {
            TrashedAt = null;
            PublishedAt = DateTimeOffset.UtcNow;
        }

        public void ScheduleFor(DateTimeOffset date)
        {
            TrashedAt = null;
            PublishedAt = date;
        }

        public void MarkAsDraft()
        {
            TrashedAt = null;
            PublishedAt = null;
        }

        public void Trash()
        {
            TrashedAt = DateTimeOffset.UtcNow;
        }

        public bool IsPublished()
        {
            return PublishedAt.HasValue && TrashedAt == null && PublishedAt.Value <= DateTimeOffset.UtcNow;
        }

        public bool IsDraft()
        {
            return PublishedAt == null && TrashedAt == null;
        }

        public bool IsTrashed()
        {
            return TrashedAt.HasValue;
        }
    }
}