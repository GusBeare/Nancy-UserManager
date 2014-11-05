Nancy-UserManager
=================
This is a basic web app I built to get to grips with NancyFx with forms authentication and to learn git.

It's based on the Nancy Forms Authentication Demo app. I built it as a prototype I could reuse for simple web sites where I needed authentication and user management but didn't need to use a full blown CMS.

It's a work in progress but currently it demonstrates the following:

- User authentication with Forms
- List all users.
- Create user.
- Edit User.
- Delete User.
- Simple user roles

I became fed up with struggling to customise and work with other membership providers and decided to roll my own very simple membership system. 

I created a simple *users* table where passwords are hashed with a salt using [Bcrypt.Net](http://bcrypt.codeplex.com/ "Bcrypt.Net").
Passwords are not stored and cannot be retrieved. Verification works by comparing the password with the hash with Brypt.Net.

It's a Visual Studio 2012 project using:

- Nancy, 
- Razor, 
- Bootstrap 
- jQuery form plugin.
- Simple.Data
- SQL Server

## Getting Started  ##

1. Create a new DB in SQL Server.
1. Run the SQL script in the /scripts folder into your new DB.
1. Modify the connection string in web.config to point to your DB.
1. Build and run.

You can log one of with the following credentials.

**admin@admin.com/admin**

**viewer@viewer.com/viewer**

**editor@editor.com/viewer**

The script contains SQL to create all the membership tables, roles and users. To add/edit/delete users log is as *admin@admin.com*.

On the list to add:  

1. Roles edit
1. Better validation
1. User profile
1. Public Registration