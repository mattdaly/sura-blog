using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Raven.Client;
using Raven.Client.Linq;
using Sura.Models;
using Sura.Services;

namespace Sura.Controllers
{
    public class BlogController : RavenController
    {
        public BlogController(IDocumentSession session, ISettingsService service) : base(session, service)
        {
        }
        
        [HttpGet]
        public ActionResult List()
        {
            var posts = Session.Query<Post>()
                               .Where(x => x.PublishedAt != null &&
                                            x.TrashedAt == null &&
                                            x.PublishedAt <= DateTimeOffset.Now)
                               .OrderByDescending(x => x.PublishedAt)
                               .ToList();

            var model = Mapper.Map<IEnumerable<Post>, IEnumerable<Sura.Views.Blog.Models.List>>(posts);

            return View(model);
        }

        [HttpGet]
        public ActionResult Post(string slug)
        {
            var comments = Session.Query<PostComments>()
                                  .Include(p => p.PostId)
                                  .Where(x => x.PostSlug == slug)
                                  .FirstOrDefault();

            Post post;
            if (comments == null)
                post = Session.Query<Post>().Where(x => x.Slug == slug).FirstOrDefault();
            else
                post = Session.Load<Post>(comments.PostId);

            if (post == null)
                return new HttpStatusCodeResult(404, "We're having trouble finding that post right now.");

            return View(Map(post, comments));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Comment(Views.Blog.Models.Comment model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError("Required", "You must enter your name.");

            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Required", "You must enter an email address. Your email will not be publicly viewable.");

            if (string.IsNullOrWhiteSpace(model.Body))
                ModelState.AddModelError("Required", "You must some content for your comment.");

            var comments = Session.Query<PostComments>()
                                 .Customize(x => x.Include<PostComments>(y => y.PostId))
                                 .Where(x => x.PostId == model.PostId)
                                 .FirstOrDefault();

            if (comments == null)
                return new HttpStatusCodeResult(404, "We're having trouble loading the comments for that post.");

            var post = Session.Load<Post>(comments.PostId);

            if (post == null)
                return new HttpStatusCodeResult(404, "We're having trouble finding that post right now.");

            if (ModelState.IsValid == false)
                return RedirectToAction("Post", new { slug = post.Slug });

            if (post.TrashedAt != null || post.PublishedAt == null || post.PublishedAt.Value > DateTimeOffset.UtcNow)
                return new HttpStatusCodeResult(403, "You aren't allowed post a comment on this post right now.");


            var comment = new Comment
            {
                Author = model.Name,
                AuthorEmail = model.Email,
                AuthorUrl = model.Url,
                Body = model.Body,
                WrittenAt = DateTimeOffset.UtcNow,
                UserHostAddress = Request.UserHostAddress,
                UserAgent = Request.UserAgent
            };

            var settings = Settings.Load();

            if (settings.CommentsRequireApproval == false)
            {
                comments.ApprovedComments.Add(comment);
                post.Comments++;
            }
            else
            {
                comments.UnapprovedComments.Add(comment);
            }

            Session.SaveChanges();
            return RedirectToAction("Post", new { slug = post.Slug });
        }

        [HttpGet]
        public ActionResult Tag(string tag)
        {
            var posts = Session.Query<Post>()
                                .Where(x => x.PublishedAt != null &&
                                            x.TrashedAt == null &&
                                            x.PublishedAt <= DateTimeOffset.Now &&
                                            x.Tags.Any(y => y == tag.ToLower()))
                                .OrderByDescending(x => x.PublishedAt)
                                .ToList();

            if (posts.Count == 0)
                return new HttpStatusCodeResult(404, string.Format("No posts are tagged with the tag '{0}'.", tag));

            ViewBag.Tag = tag;

            return View(Mapper.Map<IEnumerable<Post>, IEnumerable<Views.Blog.Models.List>>(posts));
        }


        [HttpGet]
        public ActionResult Archive(int year, int? month, int? day)
        {
            var posts = Session.Query<Post>()
                                   .Where(x => x.PublishedAt != null &&
                                               x.TrashedAt == null &&
                                               x.PublishedAt <= DateTimeOffset.Now)
                                   .OrderByDescending(x => x.PublishedAt)
                                   .ToList();

            posts = posts.Where(post => post.PublishedAt.Value.Year == year).ToList();
            ViewBag.Year = year;

            if (month != null)
            {
                posts = posts.Where(post => post.PublishedAt.Value.Month == month.Value).ToList();
                ViewBag.Month = month.Value < 10 ? "0" + month.Value : month.Value.ToString();
            }

            if (day != null)
            {
                posts = posts.Where(post => post.PublishedAt.Value.Day == day.Value).ToList();
                ViewBag.Day = day.Value < 10 ? "0" + day.Value : day.ToString();
            }

            return View(Mapper.Map<IEnumerable<Post>, IEnumerable<Views.Blog.Models.List>>(posts));
        }


        private static Views.Blog.Models.Post Map(Post post, PostComments comments)
        {
            var model = new Views.Blog.Models.Post
            {
                Id = post.Id,
                Slug = post.Slug,
                Title = post.Title,
                Description = post.Description,
                Body = MvcHtmlString.Create(post.Body),
                Tags = post.Tags,
                Published = post.PublishedAt,
                Author = post.Author,
                Comments = new List<Views.Blog.Models.Comment>(),
                EnableComments = post.EnableComments,
                Comment = new Views.Blog.Models.Comment()
            };

            model.Comment.PostId = post.Id;

            if (comments != null)
            {
                model.Comments = comments.ApprovedComments.Select(
                                                    comment => new Views.Blog.Models.Comment()
                                                    {
                                                        Name = comment.Author,
                                                        Email = comment.AuthorEmail,
                                                        Url = comment.AuthorUrl,
                                                        Body = comment.Body,
                                                        WrittenAt = comment.WrittenAt
                                                    });
            }
            
            return model;
        }
    }
}
