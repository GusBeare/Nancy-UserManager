using BeareAccounts.Models;

namespace BeareAccounts
{
    using Nancy;
    using Nancy.Security;

    public class SecureModule : NancyModule
    {
        public SecureModule() : base("/secure")
        {
            this.RequiresAuthentication();

            Get["/"] = x =>
            {
                ViewBag.UserName = Context.CurrentUser.UserName;
                return View["secure.cshtml"];
            };
        }
    }
}