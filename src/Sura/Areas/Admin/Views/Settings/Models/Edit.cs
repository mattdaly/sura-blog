using System.ComponentModel.DataAnnotations;
using Sura.Areas.Admin.Infrastructure;

namespace Sura.Areas.Admin.Views.Settings.Models
{
    public class Edit
    {
        public string Id { get; protected internal set; }

        public int KeepUsersOnlineFor { get; set; }
        public Status EnableComments { get; set; }
        public Status CommentsRequireApproval { get; set; } 
    }
}