using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using System;
using System.Dynamic;
using System.Web.Configuration;

namespace NancyUserManager.Modules.User
{
    public class LoginModule : NancyModule
    {
        public LoginModule() : base("/")
        {
            Get["/login"] = _ =>
            {
                dynamic model = new ExpandoObject();
                model.Errored = Request.Query.error.HasValue;

                return View["Views/User/Login", model];
            };

            Post["/login"] = p =>
            {
                var UserIPAddr = Context.Request.UserHostAddress;
                var uname = (string)Request.Form.Username;
                var pwd = (string)Request.Form.Password;

                var AllowedFailedLogins = Convert.ToInt32(WebConfigurationManager.AppSettings["NumberOfLoginAttempts"]);
                var LockoutDurationMins = Convert.ToInt32(WebConfigurationManager.AppSettings["LockoutDurationMins"]);

                var lockedYn = UserDatabase.IsAccountLocked(uname, AllowedFailedLogins,LockoutDurationMins);

                if (lockedYn)
                    return Context.GetRedirect("~/login?error=true&username=" + uname);

                var userGuid = UserDatabase.ValidateUser(uname, pwd, UserIPAddr, AllowedFailedLogins, LockoutDurationMins);

                if (!userGuid.HasValue)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + uname);
                }

                DateTime? expiry = null;
                if (Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/logout"] = _ => this.LogoutAndRedirect("~/");
        }
    }
}