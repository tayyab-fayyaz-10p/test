using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class UserRolePermissionService : Service<IUserRolePermissionRepository, UserRolePermission, UserRolePermissionDTO, int>, IUserRolePermissionService
    {
        private ISSHUnitOfWork unitOfWork;

        public UserRolePermissionService(ISSHUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.UserRolePermissionRepository)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool GetUserPermissionsByRole(string roleId, List<int> permissionId)
        {
            var result = this.Repository.IsUserPermitted(roleId, permissionId);
            return result;
        }

        public async Task<string> GetUserRolePermissions(List<string> roles)
        {
            var result = await this.Repository.GetUserPermissions(roles);
            return string.Join(",", result.ToArray());
        }
        
        #region Private Function

        #endregion
    }
}
