Nancy-UserManager
=================
This is a basic app I built to get to grips with Nancy and forms authentication and also to learn git and github.
It's a work in progress but currently it demonstrates the following:

Authenticate the user:

login with username:  admin@admin.com/admin

Lists all users.
Enables create new user.
Enables user EDIT and DELETE.

There is a simple user table where the passwords are hash using Bcrypt.Net. Passwords are not stored just the hashed string.

It's a VS 2012 project in C# using:

Nancy, 
Razor, 
Bootstrap 
Simple.Data
SQL Server

Getting Started

Just run in the SQL script in the /scripts folder and modify the connection string in web.config.

