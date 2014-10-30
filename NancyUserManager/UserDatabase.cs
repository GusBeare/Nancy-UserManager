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
        public static string DeleteUser(Guid identifier)
        {
            // use Simple.Data to delete the user
            var db = Database.Open();
            var uRow = db.Users.FindByGuid(identifier);
            if (uRow !=null)
                db.Users.DeleteByGuid(identifier);

            return uRow != null ? uRow.Email : null;
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

        public static IEnumerable<Users> GetAllUsers()
        {
            // simple data used to get all the users and ordered by create date
            var db = Database.Open();
            IEnumerable<Users> uRow = db.Open().Users.All().OrderByDescending(db.Users.CreateDate);
            return uRow;
        }


        // validate user from DB
        public static Guid? ValidateUser(string email, string password)
        {
            // get the user row details from DB
            var u = GetUserByEmail(email);
          
            if (u == null) return null;
            var doesPasswordMatch = BCrypt.Net.BCrypt.Verify(password, u.Hash);

            if (doesPasswordMatch) return u.Guid;
            return null;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var u = GetUserByGuid(identifier);
            return u == null
                ? null
                : new DemoUserIdentity
                {
                    UserName = u.Email,
                    Guid = u.Guid,
                    Claims = new[]
                    {
                        "Admin",
                        "Editor",
                        "ViewOnly"
                    }
                };
        }
    }
}