using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Common.Helper;
using Recipe.Core.Base.Generic;
using Recipe.Core.Enum;
using SSH.Common.Helper;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class AuditRepository : Repository<Audit, int>, IAuditRepository
    {
        public AuditRepository(ISSHRequestInfo requestInfo)
    : base(requestInfo)
        {
        }

        protected override IQueryable<Audit> DefaultSingleQuery
        {
            get
            {
                return base.DefaultSingleQuery.Include(x => x.User).Include(x => x.AuditDetails).OrderByDescending(x => x.Id);
            }
        }

        protected override IQueryable<Audit> DefaultListQuery
        {
            get
            {
                return base.DefaultListQuery.Include(x => x.User).Include(x => x.AuditDetails).OrderByDescending(x => x.Id);
            }
        }

        public async override Task<IEnumerable<Audit>> GetAll()
        {
            return await this.DefaultListQuery.OrderByDescending(x => x.OperationTime).Take(500).ToListAsync();
        }

        public async override Task<IEnumerable<Audit>> GetAll(JsonApiRequest request)
        {
            return await this.GetAllQueryable(request).OrderByDescending(x => x.OperationTime).Take(500).ToListAsync();
        }

        public override Task<Audit> Create(Audit entity)
        {
            foreach (var each in entity.AuditDetails)
            {
                DBContext.Entry(each).State = EntityState.Added;
            }

            return base.Create(entity);
        }

        public async Task<Audit> GetSecondLastRecordAsync(int auditId, string referenceId, string tableName)
        {
            return await this.DefaultSingleQuery.Where(x => x.ReferenceId == referenceId && x.TableName == tableName && x.Id < auditId && (x.OperationType == (int)OperationType.Update || x.OperationType == (int)OperationType.Create)).OrderByDescending(x => x.Id).FirstAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditByUserId(string userId)
        {
            return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationType != (int)OperationType.Authorization).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAccessLogByUserId(string userId)
        {
            return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationType == (int)OperationType.Authorization).ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditByUserIdAndDate(string userId, string startDate, string endDate)
        {
            var startsDate = startDate.GetDate();
            var endsDate = endDate.GetDate();
            if (endsDate == null)
            {
                return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationTime >= startsDate && x.OperationType != (int)OperationType.Authorization).ToListAsync();
            }
            else if (startsDate == null)
            {
                return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationTime <= endsDate && x.OperationType != (int)OperationType.Authorization).ToListAsync();
            }
            else
            {
                return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationTime >= startsDate && x.OperationTime <= endsDate && x.OperationType != (int)OperationType.Authorization).ToListAsync();
            }
        }

        public async Task<IEnumerable<Audit>> GetAccessLogByUserIdAndDate(string userId, string startDate, string endDate)
        {
            var startsDate = startDate.GetDate();
            var endsDate = endDate.GetDate();
            if (endsDate == null)
            {
                return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationType == (int)OperationType.Authorization && x.OperationTime >= startsDate).ToListAsync();
            }
            else if (startsDate == null)
            {
                return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationType == (int)OperationType.Authorization && x.OperationTime <= endsDate).ToListAsync();
            }
            else
            {
                return await this.DefaultListQuery.Where(x => x.UserId == userId && x.OperationType == (int)OperationType.Authorization && x.OperationTime >= startsDate && x.OperationTime <= endsDate).ToListAsync();
            }
        }
    }
}
