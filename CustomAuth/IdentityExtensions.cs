using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using CustomAuth.Data;

namespace CustomAuth
{
    public static class IdentityExtensions
    {
        public static User GetUser(this IIdentity identity)
        {
            UserDb db = new UserDb(ConfigurationManager.ConnectionStrings["CustomAuthConnection"].ConnectionString);
            int userId = int.Parse(identity.Name);
            return db.GetUser(userId);
        }
    }
}