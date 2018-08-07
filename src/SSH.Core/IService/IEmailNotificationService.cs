using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Common.Helper;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IEmailNotificationService : IService<IEmailNotificationRepository, EmailNotification, EmailNotificationDTO, int>
    {
        Task SendEmailNotification(string emailAddress, string emailBody, string emailSubject);

        Task SendFCMNotification(string pushToken, string title, string body, NotificationType notificationType, int jobId = 0, string status = null, string reason = null, string reasonKey = null, string trainingId = null, string userId = null, string sound = "default");

        Task SendWebNotification(string userId, string title, string body, WebNotificationType type, int id = 0, string email = null, int status = 0);

        Task SendSMS(string phoneNumber, string message);
    }
}
