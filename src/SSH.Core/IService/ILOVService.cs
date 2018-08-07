using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Common.Helper;
using Recipe.Core.Attribute;
using Recipe.Core.Base.Interface;
using Recipe.Core.Enum;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.IService
{
    public interface ILOVService : IService<LOVDTO, int>
    {
        [AuditOperation(OperationType.Read)]
        Task<IList<LOVDTO>> GetAllAsync(LOVGroup group);

        [AuditOperation(OperationType.Read)]
        Task<TotalResultDTO<LOVDTO>> GetUpdatedListAsync(JsonApiRequest request, string dateTime = null);

        //[AuditOperation(OperationType.Delete)]
        Task<int> DeleteAsync(List<LOVIdDTO> dtoList);

        [AuditOperation(OperationType.Update)]
        Task<int> UpdateStatusAsync(ChangeLOVStatusDTO dtoObject);

        [AuditOperation(OperationType.Read)]
        Task<string[]> GetAllGroups();

        [AuditOperation(OperationType.Read)]
        Task<IList<LOVDTO>> GetByGroup(string groupName);
    }
}
