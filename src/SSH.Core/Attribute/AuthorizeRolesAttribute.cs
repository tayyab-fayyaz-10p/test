using System.Web.Http;
using SSH.Core.Enum;

namespace SSH.Core.Attribute
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRoles[] roles)
            : base()
        {
            this.Roles = string.Join(",", roles);
        }
    }
}
