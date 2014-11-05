using Nancy;

namespace NancyUserManager.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule() 
        {
            // show access denied to any unauthorised
            Get["/denied"] = _ => View["Views/AccessDenied"];

            Get["/reports"] = _ => "no reports yet!";

            Get["/accounts"] = _ => "no accounts yet!";
        }
    }
}