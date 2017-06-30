using NancyUserManager;
using NancyUserManager.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Simple.Data;
using System;

namespace BeareAccounts.Modules.User
{
    public class EditUserRoleModule : NancyModule
    {
        public EditUserRoleModule()
        {
            // add an after hook to send the user to access denied if they are NOT admin
            After += context =>
            {
                if (context.Response.StatusCode == HttpStatusCode.Forbidden)
                    context.Response = Response.AsRedirect("/denied");
            };

            this.RequiresAnyClaim("admin");
            this.RequiresAuthentication();

            // show the edit user form
            Get["/EditUserRole/{Guid}"] = parameters =>
            {
                // look up the user role from the Guid
                // nb. the view model 'Users' contains role guid and role name but the users table does not
                // so we have to look it up and populate it for the view. For now it's easier than doing a join with
                // Simple.Data #DEV - review and improve later - can be done with single method
                Users userRow = UserDatabase.GetUserByGuid(parameters.Guid);
                
                // get the users role guid and put into the user view model
                var urGuid = UserDatabase.GetRoleGuidForUser(parameters.Guid);

                userRow.RoleGuid = urGuid.RoleGuid;
                
                return View["Views/User/EditUserRole", userRow];

            };
            Post["/EditUserRole/{Guid}"] = parameters =>
            {
                var model = new Users();
                this.BindTo(model);
                var email = (string)Request.Form.Email;

                string r;
                
                try
                {
                    // create an instance of the RolesInsert and fill the data
                    var ur = new UserRolesInsert { RoleGuid = model.RoleGuid, UserGuid = model.Guid };

                    // open db and clear out old role and add new
                    var db = Database.Open();
                    db.UserRoles.DeleteByUserGuid(model.Guid); 
                    db.UserRoles.Insert(ur);

                    r = "<strong>Success:</strong> " +
                           "user: <em>" + email +
                           "</em> role was updated.  <a href=\"/users \"> return to users</a> ";
                }
                catch (Exception e)
                {
                    r = "<strong>Error:</strong> " +
                            " guid: something went wrong and the update failed!: " + e + " <a href=\"/users \"> return to users</a> ";
                }

                return Response.AsText(r);
            };


        }
    }
}