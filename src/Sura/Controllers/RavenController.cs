using System.Web.Mvc;
using Raven.Client;
using Sura.Models;
using Sura.Services;

namespace Sura.Controllers
{
    public class RavenController : Controller
    {
        protected new readonly IDocumentSession Session;
        protected readonly ISettingsService Settings;

        protected RavenController(IDocumentSession session, ISettingsService service = null)
        {
            Session = session;
            Settings = service;
        }
    }
}
