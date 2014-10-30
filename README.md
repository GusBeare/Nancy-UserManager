Nancy-UserManager
=================
This is a basic app I built to get to grips with Nancy and forms authentication and also to learn git and github.
Its based on the Nancy Forms Authentication Demo app. I built it as a prototype so I could use it for simple web sites
where I needed authentication but didn't need to use a full blown CMS.

It's a work in progress but currently it demonstrates the following:

User authentication with Forms:
When you have set up the application you can log in with the following credentials.

username:  admin@admin.com/admin

Lists all users.
Enables create new user.
Enables user EDIT and DELETE.

There is a simple user table where the passwords are hashed with a salt using Bcrypt.Net. 
Passwords are not stored just the hashed string. Email address is used as username.

It's a VS 2012 project in C# using:

Nancy, 
Razor, 
Bootstrap 
jQuery form plugin for Ajax CRUD.
Simple.Data (I love this for CRUD)
SQL Server

Getting Started

Just run in the SQL script in the /scripts folder and modify the connection string in web.config.

