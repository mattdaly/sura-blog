using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Security;

namespace Sura.Areas.Admin.Helpers
{
    public static class UserHelpers
    {
        public static MvcHtmlString User(this HtmlHelper helper, bool link = true)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (link == false)
                    return MvcHtmlString.Create(((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData);

                var output = HtmlHelper.GenerateLink(helper.ViewContext.RequestContext,
                                                 System.Web.Routing.RouteTable.Routes,
                                                 ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData,
                                                 "Admin_Account",
                                                 "Edit", "Account", null, null);
                return MvcHtmlString.Create("Hello, " + output);
            }

            return MvcHtmlString.Empty;
        }
    }
}