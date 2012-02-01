using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sura.Areas.Admin.Views.Users.Models
{
    public class Edit
    {
        [HiddenInput]
        public string Id { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords you have entered do not match.")]
        [Display(Name = "Password (Again)")]
        public string PasswordRetype { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
    }
}