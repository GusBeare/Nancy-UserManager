using System;

namespace NancyUserManager
{
    using System.Collections.Generic;
    using Nancy.Security;

    public class DemoUserIdentity : IUserIdentity
    {
        public Guid Guid { get; set; }
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}