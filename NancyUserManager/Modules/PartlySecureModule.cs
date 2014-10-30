using BeareAccounts.Models;
using Nancy;
using Nancy.Security;

namespace BeareAccounts.Modules
{
    public class PartlySecureModule : NancyModule
    {
        public PartlySecureModule()
            : base("/partlysecure")
        {
            Get["/"] = _ => "No auth needed! <a href='partlysecure/secured'>Enter the secure bit!</a>";

            Get["/secured"] = _ => {

                this.RequiresAuthentication();

                var model = new Users {Email = this.Context.CurrentUser.UserName};
                return View["secure.cshtml", model];
            };

           
        }
    }
}