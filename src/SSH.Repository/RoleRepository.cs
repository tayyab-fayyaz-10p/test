using System.Data.Entity;
using System.Linq;
using Recipe.Core.Base.Generic;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class RoleRepository : AuditableRepository<ApplicationRole, string>, IRoleRepository
    {
        private ApplicationRoleManager roleManager;
        private ISSHRequestInfo requestInfo;

        public RoleRepository(ISSHRequestInfo requestInfo, ApplicationRoleManager roleManager)
            : base(requestInfo)
        {
            this.requestInfo = requestInfo;
            this.roleManager = new ApplicationRoleManager(new ApplicationRoleStore(requestInfo));
        }

        protected override IQueryable<ApplicationRole> DefaultListQuery
        {
            get
            {
                return base.DefaultListQuery
                           .Include(x => x.UserRolePermission)
                           .Include(x => x.UserRolePermission.Select(y => y.Permission)).OrderByDescending(x => x.Id);
            }
        }

        protected override IQueryable<ApplicationRole> DefaultSingleQuery
        {
            get
            {
                return base.DefaultSingleQuery
                           .Include(x => x.UserRolePermission)
                           .Include(x => x.UserRolePermission.Select(y => y.Permission)).OrderByDescending(x => x.Id); 
            }
        }
    }
}
