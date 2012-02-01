using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sura.Views.Blog.Models
{
    public class Comment
    {
        public string PostId { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        [AllowHtml]
        [Required]
        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public DateTimeOffset WrittenAt { get; set; }
    }
}