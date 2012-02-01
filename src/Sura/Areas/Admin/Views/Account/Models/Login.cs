using System.ComponentModel.DataAnnotations;

namespace Sura.Areas.Admin.Views.Account.Models
{
    public class Login
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}