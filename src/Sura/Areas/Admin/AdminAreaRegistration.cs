using System.Web.Mvc;

namespace Sura.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // account
            context.MapRoute("Admin", "admin", new { controller = "Account", action = "Login" });
            context.MapRoute("Admin_Login", "admin/login", new { controller = "Account", action = "Login" });
            context.MapRoute("Admin_Logout", "admin/logout", new { controller = "Account", action = "Logout" });
            context.MapRoute("Admin_Account", "admin/account", new { controller = "Account", action = "Edit" });


            // users
            context.MapRoute("Admin_Users", "admin/users", new { controller = "Users", action = "List" });
            context.MapRoute("Admin_Users_New", "admin/users/new", new { controller = "Users", action = "Create" });
            context.MapRoute("Admin_Users_Edit", "admin/users/{username}", new { controller = "Users", action = "Edit" });
            context.MapRoute("Admin_Users_Delete", "admin/users/{username}/delete", new { controller = "Users", action = "Delete" });

            // posts
            context.MapRoute("Admin_Posts", "admin/posts", new { controller = "Posts", action = "List" });
            context.MapRoute("Admin_Posts_New", "admin/posts/new", new { controller = "Posts", action = "Create" });
            context.MapRoute("Admin_Posts_Edit", "admin/posts/{slug}", new { controller = "Posts", action = "Edit" });
            context.MapRoute("Admin_Posts_Trash", "admin/posts/{slug}/trash", new { controller = "Posts", action = "Trash" });
            context.MapRoute("Admin_Posts_Delete", "admin/posts/{slug}/delete", new { controller = "Posts", action = "Delete" });

            // comments
            context.MapRoute("Admin_Comments", "admin/comments", new { controller = "Comments", action = "List" });

            // settings
            context.MapRoute("Admin_Settings", "admin/settings", new { controller = "Settings", action = "Edit" });

            context.MapRoute("Admin_Dashboard", "admin/dashboard", new { controller = "Dashboard", action = "Index" });

            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
