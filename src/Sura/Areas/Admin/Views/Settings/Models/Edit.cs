using System.ComponentModel.DataAnnotations;

namespace Sura.Areas.Admin.Views.Settings.Models
{
    public class Edit
    {
        public string Id { get; protected internal set; }

        public int KeepUsersOnlineFor { get; set; }
        public bool EnableComments { get; set; }
        public bool CommentsRequireApproval { get; set; } 
    }
}