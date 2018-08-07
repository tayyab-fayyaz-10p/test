using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Recipe.Core.Base.Generic;
using SSH.API.Infrastructure;
using SSH.Common.Helper;
using SSH.Core.Attribute;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.IService;

namespace SSH.API.Controller
{
    [CustomAuthorize]
    [RoutePrefix("Common")]
    public class CommonController : Recipe.Core.Base.Generic.Controller
    {
        private IEmailNotificationService emailService;

        public CommonController(IEmailNotificationService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost]
        [Route("SendNotification")]
        [AllowAnonymous]
        public async Task<ResponseMessageResult> SendNotification(SendNotificationDTO dtoObject)
        {
            await this.emailService.SendFCMNotification(dtoObject.To, dtoObject.Notification.Title, dtoObject.Notification.Body, (NotificationType)dtoObject.Data.NotificationType, dtoObject.Data.Id, dtoObject.Data.DeliveryPartnerStatus, dtoObject.Data.Reason, dtoObject.Data.ReasonKey, dtoObject.Data.UserId);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, null);
        }
    }
}
