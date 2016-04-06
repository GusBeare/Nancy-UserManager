using Nancy;
using Nancy.Security;

namespace NancyUserManager.Modules.User
{
    public class ListUsersModule : NancyModule
    {
        public ListUsersModule()
        {
            // add an after hook to send the user to access denied if they are NOT admin
            After += context =>
            {
                if (context.Response.StatusCode == HttpStatusCode.Forbidden)
                    context.Response = Response.AsRedirect("/denied");
            };
            this.RequiresAnyClaim(new[] { "admin" });

            Get["/users"] = _ =>
            {
                this.RequiresAuthentication();

                var users = UserDatabase.GetAllUsers();
                ViewBag.Title = "Application Users";
                return View["Views/User/Users", users];
            };
        }
    }
}