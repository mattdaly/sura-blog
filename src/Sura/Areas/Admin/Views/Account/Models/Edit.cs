using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sura.Areas.Admin.Views.Account.Models
{
    public class Edit
    {
        [HiddenInput]
        public string Id { get; set; }

        [Display(Name = "Current Password")]
        public string Password { get; set; }
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Display(Name = "New Password (Again)")]
        public string NewPasswordRetype { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; } 
    }
}