namespace SIS.Framework.Attributes.Action
{
    using System;
    using System.Linq;
    using Security.Contracts;

    public class AuthorizeAttribute : Attribute
    {
        private readonly string[] roles;

        public AuthorizeAttribute()
        {
        }

        public AuthorizeAttribute(string[] roles)
        {
            this.roles = roles;
        }

        public bool IsAuthorized(IIdentity identity)
        {
            if (this.roles == null || !this.roles.Any())
            {
                return this.IsIdentityPresent(identity);
            }

            return this.IsIdentityRole(identity);
        }

        private bool IsIdentityPresent(IIdentity identity)
        {
            return identity != null;
        }

        private bool IsIdentityRole(IIdentity identity)
        {
            if (!this.IsIdentityPresent(identity))
            {
                return false;
            }

            return roles.Any(role => identity.Roles.Contains(role));
        }
    }
}