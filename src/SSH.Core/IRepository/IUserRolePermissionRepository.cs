using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.Entity;

namespace SSH.Core.IRepository
{
    public interface IUserRolePermissionRepository : IRepository<UserRolePermission, int>
    {
        Task<List<string>> GetUserPermissions(List<string> roles);

        bool IsUserPermitted(string roleId, List<int> permissionId);
    }
}
