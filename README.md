Sura
============
A small and simple blogging system, built to test Raven DB.

* Posts - including scheduling, drafts, trash
* Comments - including required moderation, disable and enable per post or whole blog

Installation
============
- Grab the referenced packages using NuGet inside VS.
- Set your RavenDB connection string in the web.config
- Build and run

**Note:** Sample data is generated if none exists in the specified database. This includes a default user: 

* username: admin
* password: demo

You can login via /admin/login.

To prevent sample data from being generated remove the BuildSampleData call and method inside the global.asax file. No Raven indexes are created, only dynamic indexes are used.

Screenshots
===========

**Sura Admin Settings**

![Sura Settings](http://github.com/mattdaly/sura-blog/raw/master/images/admin_settings.png)

**Sura Admin Posts**

![Sura Admin Posts](http://github.com/mattdaly/sura-blog/raw/master/images/admin_posts.png)

**Sura Admin Post Tags**

![Sura Admin Post Tags](http://github.com/mattdaly/sura-blog/raw/master/images/admin_posts_tags.png)

**Sura Admin Post Scheduling**

![Sura Admin Post Scheduling](http://github.com/mattdaly/sura-blog/raw/master/images/admin_scheduler.png)

**Sura Admin Comment Moderation**

![Sura Admin Comment Moderation](http://github.com/mattdaly/sura-blog/raw/master/images/admin_comments.png)
