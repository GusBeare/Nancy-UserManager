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
            // add an after hook to send the user to access denied if they are NOT admin
            After += context =>
            {
                if (context.Response.StatusCode == HttpStatusCode.Forbidden)
                    context.Response = this.Response.AsRedirect("/denied");
            };
            this.RequiresAnyClaim(new[] { "admin" });

            // show the add user form
            Get["/adduser"] = _ =>
            {
                this.RequiresAuthentication();
                return View["Views/User/AddUser"];
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
                // use default, increase WORK FACTOR to make more secure. Note that this will slow down user create a great deal and 
                // you will want to put some kind of AJAX processing gif on the page 
                var theSalt = BCrypt.Net.BCrypt.GenerateSalt(); 

                var theHash = BCrypt.Net.BCrypt.HashPassword(pwd, theSalt);
                // nb: pwd is NOT saved in the DB, only the hash

                model.CreateDate = DateTime.Now;
                model.LastUpdated = DateTime.Now;
                model.LastUpdatedBy = Context.CurrentUser.UserName;
                model.Hash = theHash;
                model.Guid = Guid.NewGuid();

                db.Users.Insert(model);

                // Create a UserRole row and insert that
                var UserRoles = new UserRolesInsert();
                UserRoles.RoleGuid = model.RoleGuid;
                UserRoles.UserGuid = model.Guid;

                db.UserRoles.Insert(UserRoles);

                return Response.AsJson("<strong>Success:</strong> user <em>" + model.Email + "</em> was added.");
            };


        }
    }
}