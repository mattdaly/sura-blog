using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sura.Helpers
{
    public static class DateHelper
    {
        public static MvcHtmlString Published(this HtmlHelper helper, DateTimeOffset? date)
        {
            if(date.HasValue == false)
                return MvcHtmlString.Empty;

            var year = date.Value.ToString("yyyy");
            var month = date.Value.ToString("MM");
            var day = date.Value.ToString("dd");

            var yearLink = HtmlHelper.GenerateLink(helper.ViewContext.RequestContext,
                                                 RouteTable.Routes,
                                                 year,
                                                 "Blog_Archive_Year",
                                                 "Archive", "Blog", 
                                                 new RouteValueDictionary { { "year", year } }, 
                                                 null);

            var monthLink = HtmlHelper.GenerateLink(helper.ViewContext.RequestContext,
                                                 RouteTable.Routes,
                                                 month,
                                                 "Blog_Archive_Month",
                                                 "Archive", "Blog", 
                                                 new RouteValueDictionary { { "year", year }, { "month", month } }, 
                                                 null);
            var dayLink = HtmlHelper.GenerateLink(helper.ViewContext.RequestContext,
                                                 RouteTable.Routes,
                                                 day,
                                                 "Blog_Archive_Day",
                                                 "Archive", "Blog", 
                                                 new RouteValueDictionary { { "year", year }, { "month", month }, { "day", day } }, 
                                                 null);

            var time = date.Value.DateTime.ToString("HH:mm");



            return MvcHtmlString.Create(string.Format("Published: {0}/{1}/{2} {3}", dayLink, monthLink, yearLink, time));
        }
    }
}