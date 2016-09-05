using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using System;
using System.Dynamic;

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

            Post["/login"] = _ =>
            {
                var userGuid = UserDatabase.ValidateUser((string) Request.Form.Username,(string) Request.Form.Password,Request.UserHostAddress.ToString());

                if (!userGuid.HasValue)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string) Request.Form.Username);
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