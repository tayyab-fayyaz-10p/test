using System.Collections.Generic;
using Recipe.Core.Attribute;
using Recipe.Core.Base.Interface;
using Recipe.Core.Enum;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IRoleService : IService<IRoleRepository, ApplicationRole, RoleDTO, string>
    {
        [AuditOperation(OperationType.Read)]
        List<RoleDTO> GetCurrentUserRoleMapping();
    }
}
