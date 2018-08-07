using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class UserRolePermissionRepository : Repository<UserRolePermission, int>, IUserRolePermissionRepository
    {
        public UserRolePermissionRepository(ISSHRequestInfo requestInfo)
            : base(requestInfo)
        {
        }

        protected override IQueryable<UserRolePermission> DefaultSingleQuery
        {
            get
            {
                return base.DefaultSingleQuery.Include(x => x.Permission).Include(x => x.Role).OrderByDescending(x => x.Id);
            }
        }

        protected override IQueryable<UserRolePermission> DefaultListQuery
        {
            get
            {
                return base.DefaultListQuery.Include(x => x.Permission).Include(x => x.Role).OrderByDescending(x => x.Id);
            }
        }

        public async Task<List<string>> GetUserPermissions(List<string> roles)
        {
            return await this.DefaultListQuery.Where(x => roles.Contains(x.RoleID)).Select(x => x.Permission.Name).ToListAsync();
        }

        public bool IsUserPermitted(string roleId, List<int> permissionId)
        {
            return this.DefaultListQuery.Any(x => x.RoleID == roleId && permissionId.Contains(x.PermissionID));
        }
    }
}
