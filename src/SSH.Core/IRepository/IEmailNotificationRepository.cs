using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.Entity;

namespace SSH.Core.IRepository
{
    public interface IEmailNotificationRepository : IRepository<EmailNotification, int>
    {
        Task<IList<EmailNotification>> GetAllUnSendEmailNotificationsAsync();
    }
}
