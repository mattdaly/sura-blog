using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Raven.Client;
using Raven.Client.Linq;
using Sura.Areas.Admin.Infrastructure;
using Sura.Areas.Admin.Views.Posts.Models;
using Sura.Controllers;
using Sura.Models;

namespace Sura.Areas.Admin.Controllers
{
    [Authorize]
    public class PostsController : RavenController
    {
        public PostsController(IDocumentSession session) : base(session)
        {
        }

        public ActionResult List()
        {
            var posts = Session.Query<Post>()
                                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                                .OrderByDescending(x => x.PublishedAt)
                                .ToList();

            return View(Mapper.Map<IEnumerable<Post>, IEnumerable<List>>(posts));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Create model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError("Required", "You must enter a title for the post.");

            if (string.IsNullOrWhiteSpace(model.Body))
                ModelState.AddModelError("Required", "You must write some content for the post.");

            if (model.Availability == Availability.Scheduled && model.ScheduleFor.HasValue == false)
                ModelState.AddModelError("Required", "To schedule a post you must select a scheduled date and time.");

            if (ModelState.IsValid == false)
                return View("Create", model);

            var slug = Helper.ConvertToSlug(model.Title);
            var post = Session.Query<Post>().Where(x => x.Slug == slug).FirstOrDefault();

            if (post != null)
            {
                if (post.Slug == slug)
                    ModelState.AddModelError("Conflict", string.Format("A post with the title '{0}' already exists.", model.Title));
            }

            if (ModelState.IsValid == false)
                return View("Create", model);

            post = new Post(model.Title, slug, HttpContext.User.Identity.Name, model.Body)
                       {
                           Description = model.Description,
                           EnableComments = model.EnableComments == Status.Enabled,
                           Tags = Helper.ConvertToTagList(model.Tags)
                       };

            if (model.Availability == Availability.Publish)
                post.Publish();
            else if (model.Availability == Availability.Scheduled && model.ScheduleFor.HasValue)
                post.ScheduleFor(model.ScheduleFor.Value);
            else if (model.Availability == Availability.Draft)
                post.MarkAsDraft();

            Session.Store(post);
            Session.SaveChanges();

            if (post.EnableComments && post.IsPublished())
            {
                Session.Store(new PostComments(post.Id, post.Title, post.Slug, post.PublishedAt.Value));
                Session.SaveChanges();
            }

            return RedirectToAction("Edit", new { slug });
        }

        [HttpGet]
        public ActionResult Edit(string slug)
        {
            var post = Session.Query<Post>()
                                .Where(x => x.Slug == slug)
                                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                                .FirstOrDefault();

            if (post == null)
                return new HttpStatusCodeResult(404, string.Format("Blog post with slug '{0}' not found.", slug));

            return View(Mapper.Map<Post, Edit>(post));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Edit model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError("Required", "You must enter a title for the post.");

            if (ModelState.IsValid == false)
                return View("Edit", model);

            var post = Session.Load<Post>(model.Id);

            if (post == null)
                return new HttpStatusCodeResult(404, string.Format("Blog post with id '{0}' not found.", model.Id));

            post.Title = model.Title;
            post.Slug = Helper.ConvertToSlug(model.Title);
            post.Description = model.Description;
            post.Body = model.Body;
            post.EnableComments = model.EnableComments == Status.Enabled;
            post.Tags = Helper.ConvertToTagList(model.Tags);

            if (model.Availability == Availability.Publish && (post.PublishedAt.HasValue == false || post.PublishedAt.Value > DateTimeOffset.UtcNow))
                post.Publish();
            else if (model.Availability == Availability.Scheduled && model.ScheduleFor.HasValue)
                post.ScheduleFor(model.ScheduleFor.Value);
            else if (post.PublishedAt.HasValue && model.Availability == Availability.Draft)
                post.MarkAsDraft();

            if (post.IsTrashed())
                post.TrashedAt = null;

            post.LastEditedBy = HttpContext.User.Identity.Name;

            if (post.EnableComments && post.IsPublished())
            {
                var postComments = Session.Query<PostComments>().Where(x => x.PostId == post.Id).FirstOrDefault();

                if (postComments == null)
                    Session.Store(new PostComments(post.Id, post.Title, post.Slug, post.PublishedAt.Value));
            }

            Session.SaveChanges();
            ViewBag.Success = true;

            return View(Mapper.Map<Post, Edit>(post));
        }


        [HttpPost]
        public HttpStatusCodeResult Trash(string slug)
        {
            var post = Session.Query<Post>().Where(x => x.Slug == slug).FirstOrDefault();

            if (post == null)
                return new HttpStatusCodeResult(404, string.Format("Blog post with slug '{0}' not found.", slug));

            post.Trash();
            post.LastEditedBy = HttpContext.User.Identity.Name;
            Session.SaveChanges();

            return new HttpStatusCodeResult(200, string.Format("Blog post with slug '{0}' trashed.", slug));
        }

        [HttpDelete]
        public HttpStatusCodeResult Delete(string slug)
        {
            var post = Session.Query<Post>().Where(x => x.Slug == slug).FirstOrDefault();

            if (post == null)
                return new HttpStatusCodeResult(404, string.Format("Blog post with slug '{0}' not found.", slug));

            var postComments = Session.Query<PostComments>().Where(x => x.PostId == post.Id).FirstOrDefault();

            if (postComments != null)
                Session.Delete(postComments);

            Session.Delete(post);
            Session.SaveChanges();

            return new HttpStatusCodeResult(200, string.Format("Blog post with slug '{0}' deleted.", slug));
        }
    }
}
