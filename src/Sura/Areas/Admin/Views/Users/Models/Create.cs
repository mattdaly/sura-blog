using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sura.Areas.Admin.Views.Users.Models
{
    public class Create
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords you have entered do not match.")]
        [Display(Name = "Password (Again)")]
        [Required]
        public string PasswordRetype { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }

    }
}