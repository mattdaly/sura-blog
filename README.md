Sura
============
A small and simple blogging system, built to test Raven DB.

* Posts - including scheduling, drafts, trash, post tags, archives by date
* Comments - including required moderation, disable and enable comments per post or whole blog
* Users - sliding login attempts (2 second wait, 4 second wait ... 60 second wait)

Installation
============
- Use NuGet to restore missing packages
- Set your RavenDB connection string in the web.config (connection string 'Sura-Blog')
- Edit the global user hash (key 'Sura-Users-Hash') in the web.config (see note 2)
- Build and run

**Note:** Sample data is generated if none exists in the specified database. This includes a default user: 

* username: admin
* password: password

You can login via /admin/login.

**Note 1:** To prevent sample data from being generated remove the BuildSampleData call and method inside the global.asax file. No Raven indexes are created, only dynamic indexes are used.

**Note 2:** A global user hash is used when generating user passwords (as well as per user hash), this global hash is inside the web.config file under the key 'Sura-Users-Hash'. This should be application specific and should not change after first setting it. Changing it will make it impossible for existing users to login.

Screenshots
===========

![Sura Settings](http://github.com/mattdaly/sura-blog/raw/master/screenshots/admin_settings.png)

![Sura Posts](http://github.com/mattdaly/sura-blog/raw/master/screenshots/admin_posts.png)

![Sura Tags](http://github.com/mattdaly/sura-blog/raw/master/screenshots/admin_scheduled.png)

![Sura Post Scheduling](http://github.com/mattdaly/sura-blog/raw/master/screenshots/admin_scheduler.png)

![Sura Comment Moderation](http://github.com/mattdaly/sura-blog/raw/master/screenshots/admin_comments.png)
