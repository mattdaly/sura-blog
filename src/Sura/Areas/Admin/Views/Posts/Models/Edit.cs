﻿using System;
using System.ComponentModel.DataAnnotations;
using Sura.Areas.Admin.Infrastructure;

namespace Sura.Areas.Admin.Views.Posts.Models
{
    public class Edit
    {
        public string Id { get; set; }

        public string Slug { get; set; }

        public Availability Availability { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? ScheduleFor { get; set; }

        [DataType(DataType.Text)]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        public string Tags { get; set; }

        [Display(Name = "Content")]
        public string Body { get; set; }

        [Display(Name = "Comments")]
        public Status EnableComments { get; set; }

        public bool IsTrashed { get; set; }
    }
}