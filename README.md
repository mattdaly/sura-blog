Sura
============
A small and simple blogging system, built to test Raven DB.

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
