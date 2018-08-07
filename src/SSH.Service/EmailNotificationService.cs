using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Common.Helper;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class EmailNotificationService : Service<IEmailNotificationRepository, EmailNotification, EmailNotificationDTO, int>, IEmailNotificationService
    {
        private IExceptionHelper exceptionHelper;
        private IConfigurationService configurationService;
        private IPushNotificationService pushNotificationService;

        public EmailNotificationService(ISSHUnitOfWork unitOfWork, IConfigurationService configurationService, IExceptionHelper exceptionHelper, IPushNotificationService pushNotificationService)
            : base(unitOfWork, unitOfWork.EmailNotificationRepository)
        {
            this.exceptionHelper = exceptionHelper;
            this.configurationService = configurationService;
            this.pushNotificationService = pushNotificationService;
        }

        public async Task SendEmailNotification(string emailAddress, string emailBody, string emailSubject)
        {
            EmailConfiguration emailConfiguration = new EmailConfiguration();
            emailConfiguration.ToAddress = emailAddress;
            emailConfiguration.EmailSubject = emailSubject;
            emailConfiguration.EmailBody = emailBody;
            emailConfiguration.FromAddress = this.configurationService.FromEmail;
            emailConfiguration.Token = this.configurationService.SENDGRID_API_KEY;
            emailConfiguration.Api = this.configurationService.SENDGRID_API;
            await this.SendEmail(emailConfiguration);
        }

        public async Task SendFCMNotification(string pushToken, string title, string body, NotificationType notificationType, int jobId = 0, string status = null, string reason = null, string reasonKey = null, string trainingId = null, string userId = null, string sound = "default")
        {
            var notificationObj = new SendNotificationDTO();
            notificationObj.Content_available = true;
            notificationObj.Priority = "high";
            notificationObj.To = pushToken;

            notificationObj.Notification = new Notification();
            notificationObj.Notification.Title = title;
            notificationObj.Notification.Body = body;
            notificationObj.Notification.Sound = sound;

            notificationObj.Data = new NotificationData();
            notificationObj.Data.NotificationType = (int)notificationType;
            notificationObj.Data.Badge = 1;
            notificationObj.Data.Id = jobId;
            notificationObj.Data.Reason = reason;
            notificationObj.Data.ReasonKey = reasonKey;
            notificationObj.Data.trainingId = trainingId;
            notificationObj.Data.DeliveryPartnerStatus = status;
            notificationObj.Data.UserId = userId;
            notificationObj.Data.Title = title;
            notificationObj.Data.Body = body;
            await this.SendNotification(notificationObj);
        }

        public async Task SendWebNotification(string userId, string title, string body, WebNotificationType type, int id = 0, string email = null, int status = 0)
        {
            PushNotificationDTO notificationDto = new PushNotificationDTO();
            notificationDto.ApplicationUserId = userId;
            notificationDto.Subject = title;
            notificationDto.Body = body;
            notificationDto.Type = type;
            notificationDto.Data = new PushNotificationData
            {
                Id = id,
                Email = email,
                Status = status
            };

            await this.pushNotificationService.CreateAsync(notificationDto);
        }

        public async Task SendSMS(string phoneNumber, string message)
        {
            SMSConfiguration notificationDto = new SMSConfiguration();
            notificationDto.UserName = this.configurationService.SMSUserName;
            notificationDto.Password = this.configurationService.SMSPassword;
            notificationDto.Message = message;
            notificationDto.PhoneNumber = phoneNumber;
            await this.SendSMS(notificationDto);
        }

        private async Task SendEmail(EmailConfiguration configuration)
        {
            try
            {
                await EmailHelper.SendGridEmail(configuration);
            }
            catch (Exception ex)
            {
                this.exceptionHelper.ThrowAPIException(ex.Message);
            }
        }

        private async Task SendNotification(SendNotificationDTO notificationObj)
        {
            try
            {
                NotificationHelper.SendNotification(notificationObj);
            }
            catch (Exception ex)
            {
                this.exceptionHelper.ThrowAPIException(ex.Message);
            }
        }

        private async Task SendSMS(SMSConfiguration notificationObj)
        {
            try
            {
                await SMSHelper.DoWork(notificationObj);
            }
            catch (Exception ex)
            {
                this.exceptionHelper.ThrowAPIException(ex.Message);
            }
        }

        #region Private Functions
        private async Task MarkEmailSend(int id)
        {
            EmailNotification entity = await this.Repository.GetAsync(id);
            entity.IsSent = true;
            await this.Repository.Update(entity);
            await this.UnitOfWork.SaveAsync();
        }

        private async Task MarkException(int id, Exception ex)
        {
            EmailNotification entity = await this.Repository.GetAsync(id);
            entity.Exception = ex.ToString();
            entity.RetryCount = entity.RetryCount + 1;
            await this.UnitOfWork.SaveAsync();
        }

        private async Task<List<EmailConfiguration>> GetAllUnSendEmailNotifications()
        {
            List<EmailConfiguration> emailsDTO = new List<EmailConfiguration>();
            IList<EmailNotification> entities = await this.Repository.GetAllUnSendEmailNotificationsAsync();

            foreach (EmailNotification entity in entities)
            {
                EmailConfiguration dto = new EmailConfiguration();

                dto.Id = entity.Id;
                dto.ToAddress = entity.ToAddress;
                dto.EmailSubject = entity.Subject;
                dto.EmailBody = entity.Body;

                emailsDTO.Add(dto);
            }

            return emailsDTO;
        }
        #endregion
    }
}
