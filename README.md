Nancy-UserManager
=================
This is a basic web app I built to get to grips with Nancy and forms authentication and also to learn git and github.
It's based on the Nancy Forms Authentication Demo app. I built it as a prototype so I could use it for simple web sites
where I needed authentication and user management but didn't need to use a full blown CMS.

It's a work in progress but currently it demonstrates the following:

User authentication with Forms:

Lists all users.
Create new user.
Edit User.
Delete User.

I decided to roll my own very simple membership system. There is a simple user table where passwords are hashed with a salt 
using Bcrypt.Net. Passwords are not stored just the hashed string. Email address is used as username.

It's a VS 2012 project in C# using:

Nancy, 
Razor, 
Bootstrap 
jQuery form plugin for Ajax CRUD.
Simple.Data (I love this for simplifying all data operations with SQL Server)
SQL Server

Getting Started
Get the solution down and open in VS 2012+.
Create a new DB in SQL Server.
Run the SQL script in the /scripts folder into your new DB.
Modify the connection string in web.config.
Build and run the solution.

You can log in with the following credentials.

username:  admin@admin.com/admin

The script contains SQL to create this user. You can then edit the user to change the password.

To come, better validation, user roles and role management.