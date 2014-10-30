using Nancy;

namespace NancyUserManager.Modules.Home
{
    public class HomeModule : NancyModule
    {
        public HomeModule() : base("/")
        {
            
            Get["/"] = _ => View["home"];

            Get["/home"] = _ => View["home"];
        }
    }
}