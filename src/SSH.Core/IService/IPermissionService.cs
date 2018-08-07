using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Attribute;
using Recipe.Core.Base.Interface;
using Recipe.Core.Enum;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IPermissionService : IService<IPermissionRepository, Permission, PermissionDTO, int>
    {
        [AuditOperation(OperationType.Read)]
        Task<PermissionDTO> Get(string name);

        [AuditOperation(OperationType.Read)]
        Task<List<PermissionDTO>> Get(PermissionGroup group);

        [AuditOperation(OperationType.Read)]
        Task<List<PermissionDTO>> GetReportPermissions(string reportName);
    }
}
