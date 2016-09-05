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
        public static bool UpdateLoginDetails(bool passwordInvalid, Guid guid, string IPAddress)
        {
            var db = Database.Open();

            if (passwordInvalid)
            {
                db.Users.UpdateByGuid(Guid: guid, LastSuccessfulLoginIPAddress: IPAddress);
            }
            else
            {
                db.Users.UpdateByGuid(Guid: guid, LastFailedLoginIPAddress: IPAddress);
            }
           
            return true;
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
                db.Users.CreateDate,
                db.Users.UserRoles.RoleGuid,
                db.Users.UserRoles.Roles.RoleName
                );

            return uRows;
        }


        // validate user from DB
        public static Guid? ValidateUser(string email, string password, string IPAddress)
        {
            // get the user row details from DB
            var u = GetUserByEmail(email);

            // outcomes: 
            //  1> row is found, user exists => 1a pwd matches 1b pwd fails  
            //  2> row is not found user does not exist

            if (u != null)
            {
                // check if the pwd is correct
                var doesPasswordMatch = BCrypt.Net.BCrypt.Verify(password, u.Hash);

                // update the users IP address
                UpdateLoginDetails(doesPasswordMatch, u.Guid, IPAddress);

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