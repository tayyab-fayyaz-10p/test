using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IPushNotificationService : IService<IPushNotificationRepository, PushNotification, PushNotificationDTO, int>
    {
        Task<IList<PushNotificationDTO>> GetAllByUserId();

        Task<bool> ReadAllPushNotifications();

        Task<int> GetNotificatinoCount();
    }
}
