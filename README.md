Nancy-UserManager
=================
This is a basic web app I built to get to grips with NancyFx, forms authentication and github.

It's based on the Nancy Forms Authentication Demo solution. I built it as a prototype I could reuse for simple web sites where I needed authentication and user management but didn't need to use a full blown CMS.

It's a work in progress but currently it demonstrates the following:

1. User authentication with Forms
1. List all users.
1. Create user.
1. Edit User.
1. Delete User.
1. Give user a role when creating the user
1. Change role later
1. Show a different nav menu based on user role (uses partial views in layout view)

I became fed up with struggling to customise and work with other membership providers and decided to roll my own very simple membership system. 

I created a simple *users* table where passwords are hashed using [Bcrypt.Net](http://bcrypt.codeplex.com/ "Bcrypt.Net").
Passwords are not stored and cannot be retrieved. Verification works by comparing the password with the hash with Brypt.Net.

It's a Visual Studio 2012 project using:

- Nancy, 
- Razor, 
- Bootstrap
- jQuery form plugin.
- Simple.Data
- SQL Server

## Run the app  ##

1. Create a new DB in SQL Server.
1. Run the SQL script in the /scripts folder of the solution into your new DB.
1. Modify the connection string in web.config to point to your DB.
1. Build and run. 

nb: Nuget packages are not included but should be downloaded when you build the app.

You can log in with one of with the following sets of credentials.

**admin@admin.com/admin**

**viewer@viewer.com/viewer**

**editor@editor.com/editor**

The script contains SQL to create the membership tables, roles and users. To add/edit/delete users log is as *admin@admin.com*.