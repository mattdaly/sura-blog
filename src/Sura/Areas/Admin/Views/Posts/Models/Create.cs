﻿using System;
using System.ComponentModel.DataAnnotations;
using Sura.Areas.Admin.Infrastructure;

namespace Sura.Areas.Admin.Views.Posts.Models
{
    public class Create
    {
        public Availability Availability { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? ScheduleFor { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "You must enter a title for the post")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        public string Tags { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "You must write some content for the post")]
        public string Body { get; set; }

        [Display(Name = "NumberOfComments")]
        public Status EnableComments { get; set; }
    }
}