using Nancy;
using Nancy.Security;
using System.Linq;

namespace NancyUserManager.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule() 
        {
            Get["/showclaims"] = _ =>
            {
                if (Context.CurrentUser.IsAuthenticated())
                {
                    if (Context.CurrentUser != null)
                    {
                        var claims = Context.CurrentUser.Claims;
                        var isUserAdmin = claims.Contains("Admin");
                        return "Is user admin?:" + isUserAdmin;
                    }
                }
                return "Is user is NOT admin!";
            };

            Get["/getculture.html"] = _ =>
            {
                var culture = Context.Culture;
                var authorised = Context.CurrentUser.IsAuthenticated();

                if (Context.CurrentUser.IsAuthenticated())
                {
                    if (Context.CurrentUser != null)
                    {
                        var claims = Context.CurrentUser.Claims;
                        ViewBag.IsAdmin = claims.Contains("Admin") ? "true" : "false";
                    }
                }

                //var claimsList= claims.Aggregate("List of claims: ", (current, c) => current + " " + c);
                ViewBag.Culture = culture;
                //ViewBag.Claims = claimsList;
                ViewBag.Authorised = authorised;

                return View["culture"];
            };

            Get["/test"] = _ => "test";

           
        }
    }
}