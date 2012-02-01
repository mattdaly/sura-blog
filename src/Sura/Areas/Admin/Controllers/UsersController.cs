using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Raven.Client;
using Sura.Areas.Admin.Views.Users.Models;
using Sura.Controllers;
using Sura.Models;

namespace Sura.Areas.Admin.Controllers
{
    [Authorize]
    public class UsersController : RavenController
    {
        public UsersController(IDocumentSession session) : base(session) { }

        [HttpGet]
        public ActionResult List()
        {
            var users = Session.Query<User>()
                              .OrderByDescending(x => x.CreatedAt)
                              .ToList();

            return View(Mapper.Map<IEnumerable<User>, IEnumerable<List>>(users));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Create model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                ModelState.AddModelError("Required", "You must enter a username.");

            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError("Required", "You must enter a password.");

            if (string.IsNullOrWhiteSpace(model.PasswordRetype))
                ModelState.AddModelError("Required", "You must enter the password again.");

            if (ModelState.IsValid == false)
                return View("Create", model);

            var user = Session.Load<User>(model.Username);

            if (user != null)
                ModelState.AddModelError("Conflict", string.Format("A user with the username {0} already exists.", model.Username));

            if (ModelState.IsValid == false)
                return View("Create", model);

            user = new User(model.Username, model.Password);
            user.Firstname = model.Firstname;
            user.Surname = model.Surname;
            user.Nickname = model.Nickname;

            Session.Store(user);
            Session.SaveChanges();

            return RedirectToAction("Edit", new { username = model.Username });
        }

        [HttpGet]
        public ActionResult Edit(string username)
        {
            var user = Session.Load<User>(username);

            if (user == null)
                return new HttpStatusCodeResult(404, string.Format("User '{0}' not found.", username));
            
            return View(Mapper.Map<User, Edit>(user));
        }


        [HttpPost]
        public ActionResult Edit(Edit model)
        {
            var user = Session.Load<User>(model.Id);

            if (user == null)
                return new HttpStatusCodeResult(404, string.Format("User '{0}' not found.", model.Id));

            user.Firstname = model.Firstname;
            user.Surname = model.Surname;
            user.Nickname = model.Nickname;

            if (string.IsNullOrWhiteSpace(model.Password) == false)
                user.ChangePassword(model.Password);
           
            Session.SaveChanges();
            ViewBag.Success = true;

            return View(Mapper.Map<User, Edit>(user));
        }

        [HttpDelete]
        public HttpStatusCodeResult Delete(string username)
        {
            var user = Session.Load<User>(username);

            if (user == null)
                return new HttpStatusCodeResult(404, string.Format("User '{0}' not found. Not deleted.", username));

            var users = Session.Query<User>().Count();

            if (users <= 1)
                return new HttpStatusCodeResult(403, "There is only one user left, you cannot delete it");

            Session.Delete(user);
            Session.SaveChanges();

            return new HttpStatusCodeResult(200, string.Format("User '{0}' deleted.", username));
        }
    }
}
