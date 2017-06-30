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
                    context.Response = Response.AsRedirect("/denied");
            };

            // user must be authenticated admin user
            this.RequiresAnyClaim("admin");
            this.RequiresAuthentication();

            // show the add user form
            Get["/adduser"] = _ => View["Views/User/AddUser"];

            // receive the add form data 
            Post["/adduser"] = parameters =>
            {
                // bind users model to the posted form
                var model = new Users();
                this.BindTo(model);

                // open db with Simple.Data
                var db = Database.Open(); 

                // check if username/email already exists
                int uCount = db.Users.GetCount(db.Users.Email == Request.Form.Email);
                if (uCount > 0)
                    return Response.AsJson("<strong>Error:</strong> The email already exists and cannot be used!");

                // get the pwd from the request data. It is not going in the table and therefore NOT in the model
                var pwd = (string)Request.Form.Password;

                // create the BCrypt hash with default work factor passing in the new password
                var theHash = BCrypt.Net.BCrypt.HashPassword(pwd);
              
                // populate the model fields not captured in the form
                model.Guid = Guid.NewGuid();
                model.CreatedDate = DateTime.Now;
                model.LastUpdated = DateTime.Now;
                model.LastUpdatedBy = Context.CurrentUser.UserName;
                model.Hash = theHash;  // pwd is NOT saved in the DB, only the hash

                db.Users.Insert(model);

                // Create a UserRole row 
                var UserRoles = new UserRolesInsert();
                UserRoles.RoleGuid = model.RoleGuid;
                UserRoles.UserGuid = model.Guid;

                db.UserRoles.Insert(UserRoles);

                return Response.AsJson("<strong>Success:</strong> user <em>" + model.Email + "</em> was added.");
            };


        }
    }
}