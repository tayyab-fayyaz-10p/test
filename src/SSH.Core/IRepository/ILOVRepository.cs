using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;

namespace SSH.Core.IRepository
{
    public interface ILOVRepository : IRepository<LOV, int>
    {
        Task<bool> Exists(LOV entity);

        Task<int> Delete(List<LOVIdDTO> dtoList);

        Task<int> UpdateStatus(ChangeLOVStatusDTO dtoObject);

        Task<string[]> GetAllGroups();
    }
}
