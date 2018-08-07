using Microsoft.AspNet.Identity.EntityFramework;
using SSH.Core.DBContext;
using SSH.Core.Entity;

namespace SSH.Core.Infrastructure
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>
    {
        public ApplicationUserStore(ISSHRequestInfo requestInfo) : base(requestInfo.Context)
        {
        }

        public ApplicationUserStore(SSHContext context) : base(context)
        {
        }
    }
}