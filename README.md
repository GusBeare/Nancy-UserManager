Nancy-UserManager
=================
This is a basic web app I built to get to grips with NancyFx and forms authentication and also to learn git and github.

It's based on the Nancy Forms Authentication Demo app. I built it as a prototype so I could use it for simple web sites where I needed authentication and user management but didn't need to use a full blown CMS.

It's a work in progress but currently it demonstrates the following:

- User authentication with Forms
- List all users.
- Create user.
- Edit User.
- Delete User.

I became fed up with struggling to customise and work with other membership providers and decided to roll my own very simple membership system. 

I created a simple *users* table where passwords are hashed with a salt using [Bcrypt.Net](http://bcrypt.codeplex.com/ "Bcrypt.Net"). Passwords are not stored and cannot be retrieved. Verification works by comparing the password with the hash with Brypt.Net.

The Users table primary key is a Guid and not an identity field. I wanted to get away from identity fields because they cause so much trouble when it comes to merging data sets.

It's a Visual Studio 2012 project in C# using:

- Nancy, 
- Razor, 
- Bootstrap 
- jQuery form plugin for Ajax CRUD.
- Simple.Data 
- SQL Server

## Getting Started  ##

1. Create a new DB in SQL Server.
1. Run the SQL script in the /scripts folder into your new DB.
1. Modify the connection string in web.config.
1. Build and run the solution.

You can log in with the following credentials.

**admin@admin.com/admin**

The script contains SQL to create this user. You can modify the INSERT to use your own admin user email and then log in and change the password with user edit from the user list.

*To come, better validation, user roles and role management, extended user profile, mug shots etc. *