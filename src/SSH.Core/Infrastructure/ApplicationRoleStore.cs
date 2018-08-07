using Microsoft.AspNet.Identity.EntityFramework;
using SSH.Core.DBContext;
using SSH.Core.Entity;

namespace SSH.Core.Infrastructure
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, string, ApplicationUserRole>
    {
        public ApplicationRoleStore(SSHContext context) : base(context)
        {
        }

        public ApplicationRoleStore(ISSHRequestInfo requestInfo) : base(requestInfo.Context)
        {
        }
    }
}
