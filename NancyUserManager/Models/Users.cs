using System;

namespace NancyUserManager.Models
{
    public class Users
    {
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hash { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }

    // we need a subset of user for the edit form so that we can just pass the fields we want to update to simple.data
    // if you pass a complete user model to simple.data it tries to update all the fields and not just the ones we have in the form
    public class EditUser
    {
        public Guid Guid { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hash { get; set; } 
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}