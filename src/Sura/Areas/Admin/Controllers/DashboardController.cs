using System.Web.Mvc;
using AutoMapper;
using Raven.Client;
using Sura.Controllers;
using Sura.Models;
using Sura.Services;

namespace Sura.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : RavenController
    {
        public DashboardController(IDocumentSession session) : base(session)
        {
        }

        public ActionResult Index()
        {
            var user = Session.Load<User>(HttpContext.User.Identity.Name);

            return View(Mapper.Map<User, Sura.Areas.Admin.Views.Dashboard.Models.User>(user));
        }
    }
}
