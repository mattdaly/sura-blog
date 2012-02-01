using System;

namespace Sura.Areas.Admin.Views.Dashboard.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }

        public DateTimeOffset? LastLoginAt { get; set; }
    }
}