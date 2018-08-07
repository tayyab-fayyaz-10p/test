using System;
using System.Linq;
using System.Threading.Tasks;
using SSH.Core.Enum;
using SSH.Core.IService;

namespace SSH.Core.Infrastructure
{
    public static class Security
    {
        private static IUserRolePermissionService userRolePermissionService;
        private static ISSHRequestInfo requestInfo;

        static Security()
        {
            userRolePermissionService = IoC.Resolve<IUserRolePermissionService>();
            requestInfo = IoC.Resolve<ISSHRequestInfo>();
        }

        public static bool HasRights(UserPermissions[] permission)
        {
            int[] permissionIds = Array.ConvertAll(permission, value => (int)value);
            var hasPermissions = userRolePermissionService.GetUserPermissionsByRole(Constant.Roles.GetRoleId(requestInfo.Role), permissionIds.ToList());
            return hasPermissions;
        }

        public static bool HasRights(UserPermissions[] permission, string loggedInUserRole)
        {
            var permissionIds = Array.ConvertAll(permission, value => (int)value).ToList();
            var hasPermissions = userRolePermissionService.GetUserPermissionsByRole(Constant.Roles.GetRoleId(loggedInUserRole), permissionIds);
            return hasPermissions;
        }
    }
}
