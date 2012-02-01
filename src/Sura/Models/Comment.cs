using System;

namespace Sura.Models
{
    public class Comment
    {
        public Guid Id { get; private set; }

        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorUrl { get; set; }

        public string Body { get; set; }

        public DateTimeOffset WrittenAt { get; set; }

        public string UserHostAddress { get; set; }
        public string UserAgent { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid();
            WrittenAt = DateTimeOffset.UtcNow;
        }
    }
}