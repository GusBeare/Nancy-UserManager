using Nancy;
using Nancy.Security;

namespace NancyUserManager.Modules.User
{
    public class ListUsersModule : NancyModule
    {
        public ListUsersModule()
        {
            Get["/users"] = _ =>
            {
                this.RequiresAuthentication();

                var users = UserDatabase.GetAllUsers();
                ViewBag.Title = "Application Users";
                return View["users", users];
            };
        }
    }
}