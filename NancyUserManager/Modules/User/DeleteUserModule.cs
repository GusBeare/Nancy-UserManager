using Nancy;
using Nancy.Security;


namespace NancyUserManager.Modules.User
{
    public class DeleteUserModule : NancyModule
    {
        public DeleteUserModule()
        {
            // show the del user form
            Get["/DeleteUser/{Guid}"] = parameters =>
            {
                this.RequiresAuthentication();

                // get the user row to be zapped and send it to the View
                var userRow = UserDatabase.GetUserByGuid(parameters.Guid);
                return View["DeleteUser", userRow];

            };

            Post["/DeleteUser/{Guid}"] = parameters =>
            {
                var guid = parameters.Guid;
                string email = UserDatabase.DeleteUser(guid);
                string r;

                if (email != null)
                {
                    r = "<strong>Success:</strong> " +
                            "user: <em>" + email +
                            "</em> was deleted. <a href=\"/users \"> return to users</a> ";

                }
                else
                {
                    r = "<strong>Error:</strong> " +
                            " guid: <em>" + guid +
                            "</em> was not found. <a href=\"/users \"> return to users</a> ";

                }
                return Response.AsText(r);
            };
           
        }
    }
}