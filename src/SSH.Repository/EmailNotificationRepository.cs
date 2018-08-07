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
    public class EmailNotificationRepository : AuditableRepository<EmailNotification, int>, IEmailNotificationRepository
    {
        public EmailNotificationRepository(ISSHRequestInfo requestInfo)
            : base(requestInfo)
        {
        }

        protected override IQueryable<EmailNotification> DefaultListQuery
        {
            get
            {
                return base.DefaultListQuery.OrderByDescending(x => x.Id);
            }
        }

        protected override IQueryable<EmailNotification> DefaultSingleQuery
        {
            get
            {
                return base.DefaultSingleQuery.OrderByDescending(x => x.Id);
            }
        }

        public async Task<IList<EmailNotification>> GetAllUnSendEmailNotificationsAsync()
        {
            return await this.DefaultListQuery.Where(x => x.IsSent == false && x.RetryCount < 3).ToListAsync();
        }
    }
}
