using System;
using NancyUserManager.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Simple.Data;

namespace NancyUserManager.Modules.User
{
    public class AddUserModule : NancyModule
    {
        public AddUserModule()
        {
            // show the add user form
            Get["/adduser"] = _ =>
            {
                this.RequiresAuthentication();
                return View["AddUser"];
            };

            // receive the posted add form data 

            Post["/adduser"] = parameters =>
            {
                // create an instance of the expected model and bind it to this (the posted form)
                var model = new Users();
                this.BindTo(model);

                var db = Database.Open(); // open db with Simple.Data

                // check if username/email already exists
                int uCount = Database.Open().Users.GetCount(db.Users.Email == Request.Form.Email);
                if (uCount > 0)
                    return Response.AsJson("<strong>Error:</strong> The email already exists and cannot be used!");

                // get the pwd because it is not going in the table and therefore NOT in the model
                var pwd = (string)Request.Form.Password;

                // create the BCrypt hash + salt
                string theSalt = BCrypt.Net.BCrypt.GenerateSalt();
                // GenerateSalt(50); increase the value in there to increase work factor
                string theHash = BCrypt.Net.BCrypt.HashPassword(pwd, theSalt);
                // nb: pwd is NOT saved in the DB, only the hash

                model.CreateDate = DateTime.Now;
                model.LastUpdated = DateTime.Now;
                model.LastUpdatedBy = Context.CurrentUser.UserName;
                model.Hash = theHash;
                model.Guid = Guid.NewGuid();

                db.Users.Insert(model);
                return Response.AsJson("<strong>Success:</strong> user <em>" + model.Email + "</em> was added.");
            };


        }
    }
}