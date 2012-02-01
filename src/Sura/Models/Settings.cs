namespace Sura.Models
{
    public class Settings
    {
        public string Id { get; protected internal set; }

        // Users
        public int KeepUsersOnlineFor { get; set; }

        // Comments
        public bool EnableComments { get; set; }
        public bool CommentsRequireApproval { get; set; }
        
        public Settings()
        {
            Id = "BlogSettings";

            KeepUsersOnlineFor = 45;

            EnableComments = true;
            CommentsRequireApproval = false;
        } 
    }
}