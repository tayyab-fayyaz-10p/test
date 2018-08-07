using Microsoft.AspNet.Identity;
using SSH.Core.Entity;

namespace SSH.Core.Infrastructure
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(ApplicationRoleStore roleStore) : base(roleStore)
        {
        }
    }   
}
