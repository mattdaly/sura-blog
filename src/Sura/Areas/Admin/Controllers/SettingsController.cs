using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Raven.Client;
using Sura.Areas.Admin.Views.Settings.Models;
using Sura.Controllers;
using Sura.Models;
using Sura.Services;

namespace Sura.Areas.Admin.Controllers
{
    [Authorize]
    public class SettingsController : RavenController
    {
        public SettingsController(IDocumentSession session, ISettingsService service) : base(session, service)
        {
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var settings = Settings.Load();

            return View(Mapper.Map<Settings, Edit>(settings));
        }

        [HttpPost]
        public ActionResult Edit(Edit model)
        {
            var settings = new Settings
                               {
                                   KeepUsersOnlineFor = model.KeepUsersOnlineFor,
                                   EnableComments = model.EnableComments,
                                   CommentsRequireApproval = model.CommentsRequireApproval
                               };

            Settings.Save(settings);
            ViewBag.Success = true;

            return View(Mapper.Map<Settings, Edit>(settings));
        }
    }
}
