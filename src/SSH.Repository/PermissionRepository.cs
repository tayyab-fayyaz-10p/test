using Recipe.Core.Base.Generic;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class PermissionRepository : AuditableRepository<Permission, int>, IPermissionRepository
    {
        private ISSHRequestInfo requestInfo;

        public PermissionRepository(ISSHRequestInfo requestInfo)
            : base(requestInfo)
        {
            this.requestInfo = requestInfo;
        }
    }
}
