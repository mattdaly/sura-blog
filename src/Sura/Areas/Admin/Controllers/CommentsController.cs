using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;
using Sura.Areas.Admin.Views.Comments.Models;
using Sura.Controllers;
using Sura.Models;
using Sura.Services;

namespace Sura.Areas.Admin.Controllers
{
    [Authorize]
    public class CommentsController : RavenController
    {
        private Settings _settings;

        public CommentsController(IDocumentSession session, ISettingsService settings) : base(session, settings)
        {
            _settings = settings.Load();
        }

        [HttpGet]
        public ActionResult List()
        {
            var comments = Session.Query<PostComments>()
                                        .Customize(x => x.WaitForNonStaleResults())
                                        .ToList();

            ViewBag.AllowComments = _settings.EnableComments;
            ViewBag.CommentsRequireApproval = _settings.CommentsRequireApproval;

            var model = new List<List>();

            foreach (var postComment in comments)
            {
                model.AddRange(postComment.ApprovedComments.Select(comment => new List
                {
                    PostCommentsId = postComment.Id,
                    PostId = postComment.PostId,
                    PostTitle = postComment.PostTitle,
                    PostSlug = postComment.PostSlug,
                    PostPublishedAt = postComment.PostPublishedAt,
                    CommentId = comment.Id,
                    Name = comment.Author,
                    Email = comment.AuthorEmail,
                    Url = comment.AuthorUrl,
                    Body = comment.Body,
                    WrittenAt = comment.WrittenAt,
                    UserHostAddress = comment.UserHostAddress,
                    UserAgent = comment.UserAgent,
                    Approved = true
                }));

                model.AddRange(postComment.UnapprovedComments.Select(comment => new List
                {
                    PostCommentsId = postComment.Id,
                    PostId = postComment.PostId,
                    PostTitle = postComment.PostTitle,
                    PostSlug = postComment.PostSlug,
                    PostPublishedAt = postComment.PostPublishedAt,
                    CommentId = comment.Id,
                    Name = comment.Author,
                    Email = comment.AuthorEmail,
                    Url = comment.AuthorUrl,
                    Body = comment.Body,
                    WrittenAt = comment.WrittenAt,
                    UserHostAddress = comment.UserHostAddress,
                    UserAgent = comment.UserAgent,
                    Approved = false
                }));
            }

            return View(model);
        }
      
        [HttpPost]
        public ActionResult Approve(string slug, Guid commentId)
        {
            var postComments = Session.Query<PostComments>()
                                        .Where(x => x.PostSlug == slug)
                                        .Customize(x => x.Include<PostComments>(y => y.PostId))
                                        .FirstOrDefault();

            if (postComments == null)
                return new HttpStatusCodeResult(404, "Blog post comments not found.");

            var post = Session.Load<Post>(postComments.PostId);

            if (post == null)
                return new HttpStatusCodeResult(404, "Blog post not found.");

            var comment = postComments.UnapprovedComments.FirstOrDefault(c => c.Id == commentId);

            if(comment != null)
            {
                postComments.UnapprovedComments.Remove(comment);
                postComments.ApprovedComments.Add(comment);
            }

            post.NumberOfComments++;

            Session.SaveChanges();

            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(200, "Comment approved.");

            return RedirectToAction("List");
        }

        [HttpDelete]
        public ActionResult Reject(string slug, Guid commentId)
        {
            var postComments = Session.Query<PostComments>()
                                        .Where(x => x.PostSlug == slug)
                                        .Customize(x => x.Include<PostComments>(y => y.PostId))
                                        .FirstOrDefault();

            if (postComments == null)
                return new HttpStatusCodeResult(404, "Blog post comments not found.");

            var post = Session.Load<Post>(postComments.PostId);

            if (post == null)
                return new HttpStatusCodeResult(404, "Blog post not found.");

            postComments.UnapprovedComments.RemoveAll(c => c.Id == commentId);

            Session.SaveChanges();

            return new HttpStatusCodeResult(200, "Comment rejected.");
        }

        [HttpDelete]
        public ActionResult Delete(string slug, Guid commentId)
        {
            var postComments = Session.Query<PostComments>()
                                        .Where(x => x.PostSlug == slug)
                                        .Customize(x => x.Include<PostComments>(y => y.PostId))
                                        .FirstOrDefault();

            if (postComments == null)
                return new HttpStatusCodeResult(404, "Blog post comments not found.");

            var post = Session.Load<Post>(postComments.PostId);

            if (post == null)
                return new HttpStatusCodeResult(404, "Blog post not found.");

            postComments.ApprovedComments.RemoveAll(c => c.Id == commentId);

            Session.SaveChanges();

            return new HttpStatusCodeResult(200, "Comment deleted.");
        }
    }
}
