using NancyUserManager.Models;
using Nancy;
using Simple.Data;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;

namespace NancyUserManager
{
  
    public class UserDatabase : IUserMapper
    {

        public static bool IsAccountLocked(string email, int NumberOfAllowedFailedLogins, int LockoutDurationMins)
        {
          
            var db = Database.Open();
            var user = db.Users.FindByEmail(email);
            var dtLastFailure = user?.LastFailedLoginDate?.ToString();

            // if there is no lockout timestamp then the account is not locked
            if (dtLastFailure == null) return false;

            var start = DateTime.Now;
            var oldDate = DateTime.Parse(dtLastFailure);

            bool Passed = start.Subtract(oldDate) >= TimeSpan.FromMinutes(LockoutDurationMins);

            return user.FailedLogins > NumberOfAllowedFailedLogins && !Passed;
        }


        public static void UpdateLoginDetails(bool passwordValid, Guid guid, string IPAddress, int NumberOfFailedLogins, int LockoutDurationMins)
        {
            var db = Database.Open();
            var dt = DateTime.Now;
            var user = db.Users.FindByGuid(guid);

            var nLoginAttempts = user.FailedLogins;
            var dtLastFailure = user.LastFailedLoginDate;

            // three login failures 
            if (nLoginAttempts >= NumberOfFailedLogins)
            {
                // check if the timespan has elapsed
                var start = DateTime.Now;
                var lastfail = DateTime.Parse(dtLastFailure.ToString());

                if (start.Subtract(lastfail) >= TimeSpan.FromMinutes(LockoutDurationMins))
                {
                    // reset the counter
                    db.Users.UpdateByGuid(Guid: guid, FailedLogins: 0, LastFailedLoginIPAddress: IPAddress);
                    return;
                }
               
            }

            if (passwordValid)
            {
                db.Users.UpdateByGuid(Guid: guid, LastSuccessfulLoginIPAddress: IPAddress, LastSuccessfulLoginDate: dt);
            }
            else
            {
              
                // we increment the failed logins count here
                db.Users.UpdateByGuid(Guid: guid, FailedLogins: db.Users.FailedLogins + 1, LastFailedLoginIPAddress: IPAddress, LastFailedLoginDate: dt);
             }

           
        }

      
        public static string DeleteUser(Guid identifier)
        {
            var db = Database.Open();
            var uRow = db.Users.FindByGuid(identifier);
            if (uRow == null) return null;

            db.UserRoles.DeleteByUserGuid(identifier); // must delete the user roles before we delete the user!!
            db.Users.DeleteByGuid(identifier);

            return uRow.Email;
        }

        public static Users GetUserByEmail(string email)
        {
            // use Simple.Data to get the user row
            var db = Database.Open();
            var uRow = db.Users.FindByEmail(email);
            return uRow;

        }
        public static Users GetUserByGuid(Guid identifier)
        {
            // use Simple.Data to get the user row
            var db = Database.Open();
            var uRow = db.Users.FindByGuid(identifier);
            return uRow;
        }

        public static UserRoleGuid GetRoleGuidForUser(Guid identifier)
        {
            var db = Database.Open();
            UserRoleGuid rGuid = db.UserRoles.FindAllByUserGuid(identifier).FirstOrDefault();
            return rGuid;
        }

        public static IEnumerable<Users> GetAllUsers()
        {
            // simple data used to get all the users and ordered by create date
            var db = Database.Open();

            // here we join in the UserRoles and Roles tables to get the users RoleName.
            IEnumerable<Users> uRows = db.Users.All()
              .Select(
                db.Users.Guid,
                db.Users.FirstName,
                db.Users.LastName,
                db.Users.Email,
                db.Users.CreatedDate,
                db.Users.UserRoles.RoleGuid,
                db.Users.UserRoles.Roles.RoleName
                );

            return uRows;
        }


        // validate user from DB
        public static Guid? ValidateUser(string email, string password, string IPAddress, int AllowedLoginTries, int LockoutDurationMins)
        {
            // get the user row details from DB
            var u = GetUserByEmail(email);

            if (u != null)
            {
                var doesPasswordMatch = BCrypt.Net.BCrypt.Verify(password, u.Hash);

                UpdateLoginDetails(doesPasswordMatch, u.Guid, IPAddress, AllowedLoginTries, LockoutDurationMins); 

                return doesPasswordMatch ? (Guid?) u.Guid : null;
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<Roles> GetRoles()
        {
            // simple data used to get all the roles 
            var db = Database.Open();
            IEnumerable<Roles> r = db.Open().Roles.All().OrderByDescending(db.Roles.RoleName);
            return r;

        }

        public static IEnumerable<RoleNames> GetUserRoles(Guid identifier)
        {
            var db = Database.Open();
            IEnumerable<RoleNames> uRoles = db.UserRoles.FindAllByUserGuid(identifier)
            .Select(
            db.UserRoles.Roles.RoleName);

            return uRoles;
        }
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var u = GetUserByGuid(identifier);
            var listofRoles = GetUserRoles(identifier);

            // create a simple string array and loop the list of roles and add them to it.
            var Claims = new List<string>();
            foreach (var item in listofRoles)
            {
                Claims.Add(item.RoleName);
            }

            return u == null
                ? null
                : new DemoUserIdentity
                {
                    UserName = u.Email,
                    Guid = u.Guid,
                    Claims = Claims
                };
        }
    }
}