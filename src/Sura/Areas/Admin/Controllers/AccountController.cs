using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Raven.Client;
using Sura.Areas.Admin.Views.Account.Models;
using Sura.Controllers;
using Sura.Models;
using Sura.Services;

namespace Sura.Areas.Admin.Controllers
{
    public class AccountController : RavenController
    {
        private static readonly int[] FailureDelays = new [] { 0, 2, 4, 8, 16, 32, 60 };

        public AccountController(IDocumentSession session, ISettingsService service): base(session, service)
        {
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError("Error", "Please enter a username.");
            }
            else
            {
                var now = DateTimeOffset.UtcNow;
                var user = Session.Load<User>(model.Username);

                if (user == null)
                    return new HttpStatusCodeResult(404, string.Format("User '{0}' not found.", model.Username));

                if (user.LastLoginFailureAt.HasValue)
                {
                    var lockedOutUntil = user.LastLoginFailureAt.Value.AddSeconds(FailureDelays[user.LoginFailureCount]);
                    var seconds = lockedOutUntil.Subtract(now).Seconds;

                    if (lockedOutUntil > now && seconds != 0)
                        ModelState.AddModelError("Failed", string.Format("Your previous login attempt was unsuccessful. In {0} seconds you can try again.", seconds));
                }

                if (ModelState.IsValid)
                {
                    if (user.Authenticate(model.Password))
                    {
                        if ((user.LoginFailureCount > 0))
                        {
                            user.LoginFailureCount = 0;
                            user.LastLoginFailureAt = null;
                            user.LastLockoutAt = null;
                        }

                        user.LastLoginAt = now;

                        Session.SaveChanges();

                        var userdata = user.Id;

                        if (string.IsNullOrWhiteSpace(user.Nickname) == false)
                            userdata = user.Nickname;
                        else if (string.IsNullOrWhiteSpace(user.Firstname) == false)
                            userdata = user.Firstname;

                        var settings = Settings.Load();
                        var expiry = settings.KeepUsersOnlineFor > 0 ? DateTime.Now.AddMinutes(settings.KeepUsersOnlineFor) : DateTime.Now.AddYears(10);
                        var encryptedTicket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                                                                              1,
                                                                              user.Id,
                                                                              DateTime.Now,
                                                                              expiry,
                                                                              false,
                                                                              userdata));

                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Expires = expiry };

                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Dashboard");

                        return Redirect(returnUrl);
                    }

                    if (!user.LastLoginFailureAt.HasValue)
                    {
                        user.LastLoginFailureAt = now;
                        user.LoginFailureCount = 1;
                    }
                    else
                    {
                        user.LastLoginFailureAt = now;

                        if (user.LoginFailureCount == FailureDelays.Length - 1)
                            user.LoginFailureCount = FailureDelays.Length - 1;
                        else
                            user.LoginFailureCount++;
                    }

                    var lockedOutUntil = user.LastLoginFailureAt.Value.AddSeconds(FailureDelays[user.LoginFailureCount]);
                    var seconds = lockedOutUntil.Subtract(now).Seconds;

                    ModelState.AddModelError("Failed", string.Format("Your login attempt was unsuccessful. You must wait {0} seconds before trying again.",
                                                           seconds == 0 ? FailureDelays[user.LoginFailureCount] : seconds));

                    Session.SaveChanges();
                }
            }

            return View("Login", model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            var user = Session.Load<User>(HttpContext.User.Identity.Name);

            if (user == null)
                return new HttpStatusCodeResult(404, "We're having trouble loading your account right now.");

            return View(Mapper.Map<User, Edit>(user));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(Edit model)
        {
            var user = Session.Load<User>(HttpContext.User.Identity.Name);

            if (user == null)
                return new HttpStatusCodeResult(404, "We're having trouble loading your account right now.");

            bool passwordChanged = false;

            if (string.IsNullOrWhiteSpace(model.Password) == false)
            {
                if (user.Authenticate(model.Password) == false)
                    ModelState.AddModelError("Error", "The current password you entered is incorrect.");
                else if (string.IsNullOrWhiteSpace(model.NewPassword) && string.IsNullOrWhiteSpace(model.NewPasswordRetype))
                    ModelState.AddModelError("Error", "New password and new password retype are required fields.");
                else if (model.NewPassword != model.NewPasswordRetype)
                    ModelState.AddModelError("Error", "The new passwords you have entered do not match.");
                else
                    passwordChanged = true;
            }

            if (ModelState.IsValid == false)
                return View("Edit", model);

            user.Firstname = model.Firstname;
            user.Surname = model.Surname;
            user.Nickname = model.Nickname;

            if (passwordChanged)
                user.ChangePassword(model.NewPassword);

            Session.SaveChanges();

            return RedirectToAction("Edit");
        }
    }
}
