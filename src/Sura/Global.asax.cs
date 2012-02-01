using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Autofac;
using Autofac.Integration.Mvc;
using Sura.Areas.Admin.Infrastructure;
using Sura.Models;
using Raven.Client;
using Raven.Client.Document;
using Sura.Services;

namespace Sura
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            // register controllers 
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // register raven
            builder.Register(x => new DocumentStore { ConnectionStringName = "Sura-Blog" }.Initialize()).SingleInstance();
            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession()).InstancePerLifetimeScope();

            // register settings
            builder.RegisterType<SettingsService>().As<ISettingsService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            SuraSetup(container);
        }

        private void SuraSetup(IContainer container)
        {
            var store = container.Resolve<IDocumentStore>();

            // comment out/delete this line to prevent sample data being created
            BuildSampleData(store);
           
            Mapper.Initialize(x =>
            {
                // blog
                x.CreateMap<Post, Sura.Views.Blog.Models.List>()
                    .ForMember(dest => dest.Content, opt => opt.MapFrom(src => MvcHtmlString.Create(src.Body)));

                // dasboard
                x.CreateMap<User, Sura.Areas.Admin.Views.Dashboard.Models.User>()
                    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Id));

                // settings
                x.CreateMap<Settings, Sura.Areas.Admin.Views.Settings.Models.Edit>();

                // users
                x.CreateMap<User, Sura.Areas.Admin.Views.Users.Models.List>();
                x.CreateMap<User, Sura.Areas.Admin.Views.Users.Models.Edit>();

                // account
                x.CreateMap<User, Sura.Areas.Admin.Views.Account.Models.Edit>();

                // posts
                x.CreateMap<Post, Sura.Areas.Admin.Views.Posts.Models.List>();
                x.CreateMap<Post, Sura.Areas.Admin.Views.Posts.Models.Edit>()
                    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => string.Join(", ", src.Tags.ToArray())))
                    .ForMember(dest => dest.EnableComments, opt => opt.MapFrom(src => src.EnableComments ? Status.Enabled : Status.Disabled))
                    .ForMember(dest => dest.IsTrashed, opt => opt.MapFrom(src => src.IsTrashed()))
                    .ForMember(dest => dest.Availability, opt => opt.MapFrom(
                                                        src => src.PublishedAt.HasValue && src.PublishedAt.Value <= DateTimeOffset.UtcNow
                                                            ? Availability.Publish : src.PublishedAt.HasValue && src.PublishedAt.Value > DateTimeOffset.UtcNow
                                                                ? Availability.Scheduled : Availability.Draft))
                    .ForMember(dest => dest.ScheduleFor, opt => opt.MapFrom(src => src.PublishedAt.HasValue && src.PublishedAt.Value > DateTimeOffset.UtcNow
                                                                                ? src.PublishedAt : null));
            });

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void BuildSampleData(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var settings = new Settings();
                if (session.Load<Settings>(settings.Id) == null)
                {
                    session.Store(settings);
                }

                if (session.Load<User>("admin") == null)
                {
                    session.Store(new User("admin", "password"));
                }

                if (session.Query<Post>().Count() == 0)
                {
                    var post1 = new Post("Sample Blog Post", "sample-blog-post", "admin", 
                        "<p>Welcome to the Sura Blog system</p><p>This is an automatically generated sample post</p>");
                    post1.Publish();
                    post1.Tags.Add("sample");
                    session.Store(post1);

                    var post2 = new Post("Another sample post", "another-sample-post", "admin",
                        "<p>This is another sample blog post</p>");
                    post2.Publish();
                    post2.Tags.Add("sample");
                    post2.Tags.Add("amazing");
                    session.Store(post2);

                    var post3 = new Post("A scheduled sample post", "scheduled-sample-post", "admin",
                        "<p>This is a sample blog post scheduled to post on 1st January 2013.</p>");
                    post3.ScheduleFor(new DateTimeOffset(new DateTime(2013, 1, 1)));
                    post3.Tags.Add("scheduled");
                    session.Store(post3);

                    var post4 = new Post("A draft sample post", "draft-sample-post", "admin",
                        "<p>This sample post is marked as a draft copy.</p>");
                    post4.MarkAsDraft();
                    post4.Tags.Add("sample");
                    post4.Tags.Add("draft");
                    session.Store(post4);
                }

                session.SaveChanges();
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            const string matchPositiveInteger = @"\d{1,10}";

            // posts
            routes.MapRoute("Blog_Post", "{slug}", new { controller = "Blog", action = "Post" });
            routes.MapRoute("Blog_Post_Comment", "{slug}/comment", new { controller = "Blog", action = "Comment" });

            // tags
            routes.MapRoute("Blog_Tag", "tag/{tag}", new { controller = "Blog", action = "Tag" });

            // archives
            routes.MapRoute("Blog_Archive_Day", "archive/{year}/{month}/{day}", new { controller = "Blog", action = "Archive" },
                    new { Year = matchPositiveInteger, Month = matchPositiveInteger, Day = matchPositiveInteger });
            routes.MapRoute("Blog_Archive_Month", "archive/{year}/{month}", new { controller = "Blog", action = "Archive" },
                    new { Year = matchPositiveInteger, Month = matchPositiveInteger });
            routes.MapRoute("Blog_Archive_Year", "archive/{year}", new { controller = "Blog", action = "Archive" },
                    new { Year = matchPositiveInteger });

            routes.MapRoute(
             "Blog", // Route name
             "", // URL with parameters
             new { controller = "Blog", action = "List" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Blog", action = "List", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}