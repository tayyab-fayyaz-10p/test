using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceHouse.Common.Helper;
using FinanceHouse.Core.Attribute;
using FinanceHouse.Core.Enum;

namespace FinanceHouse.Core.IRepository
{
    public interface IBaseRepository
    {
    }

    public interface IBaseRepository<TEntity, TKey> : IBaseRepository
    {
        [AuditOperation(OperationType.Read)]
        Task<TEntity> GetDefaultAsync(TKey id);

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetDefaultAsync(IList<TKey> ids);

        [AuditOperation(OperationType.Read)]
        Task<TEntity> GetAsync(TKey id);

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAsync(IList<TKey> ids);

        [AuditOperation(OperationType.Read)]
        Task<TEntity> GetEntityOnly(TKey id);

        [AuditOperation(OperationType.Read)]
        Task<int> GetCount();

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAllDefault();

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAllDefault(JsonApiRequest request);

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAll();

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAll(JsonApiRequest request);

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAll(IList<TKey> keys);

        [AuditOperation(OperationType.Read)]
        Task<IEnumerable<TEntity>> GetAll(IList<TKey> keys, JsonApiRequest request);

        [AuditOperation(OperationType.Create)]
        Task<TEntity> Create(TEntity entity);

        [AuditOperation(OperationType.Update)]
        Task<TEntity> Update(TEntity entity);

        [AuditOperation(OperationType.Delete)]
        Task DeleteAsync(TKey id);
    }

    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int>
    {
    }
}
