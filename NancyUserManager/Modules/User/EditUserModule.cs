using NancyUserManager.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Simple.Data;
using System;

namespace NancyUserManager.Modules.User
{
    public class EditUserModule : NancyModule
    {
        public EditUserModule()
        {
            // show the edit user form
            Get["/EditUser/{Guid}"] = parameters =>
            {
                this.RequiresAuthentication();

                // get the user row to be edit and send it to the View
                var userRow = UserDatabase.GetUserByGuid(parameters.Guid);
                return View["EditUser", userRow];

            };

            Post["/EditUser/{Guid}"] = parameters =>
            {
                var model = new Users();
                this.BindTo(model);
                string r;

                var email = (string) Request.Form.Email;
                var pwd = (string) Request.Form.Password;
                var theSalt = BCrypt.Net.BCrypt.GenerateSalt();
                var thenewHash = BCrypt.Net.BCrypt.HashPassword(pwd, theSalt);

                // create a subset of the user model so that we are only updating the fields
                // we see on the form
                var user = new EditUser()
                {
                    Guid=model.Guid,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = Context.CurrentUser.UserName,
                    Hash = thenewHash
                };
                 
                try
                {
                    var db = Database.Open();
                    db.Users.Update(user);
                    r = "<strong>Success:</strong> " +
                           "user: <em>" + email +
                           "</em> was updated.  <a href=\"/users \"> return to users</a> ";
                }
                catch (Exception e)
                {
                    r = "<strong>Error:</strong> " +
                            " guid: something went wrong and the update failed!: " + e +" <a href=\"/users \"> return to users</a> "; 
                }
               
                return Response.AsText(r);
            };

           
        }
    }
}