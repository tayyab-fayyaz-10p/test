using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Recipe.Core.Base.Abstract;
using SSH.Common.Helper;
using SSH.Core.Constant;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class PushNotificationDTO : DTO<PushNotification, int>
    {
        public string ApplicationUserId { get; set; }

        public string Subject { get; set; }
        
        public string Body { get; set; }
        
        public bool IsRead { get; set; }

        public string CreatedOn { get; set; }

        public WebNotificationType Type { get; set; }

        public PushNotificationData Data { get; set; }

        public override void ConvertFromEntity(PushNotification entity)
        {
            base.ConvertFromEntity(entity);

            this.ApplicationUserId = entity.ApplicationUserId;
            this.Subject = entity.Subject;
            this.Body = entity.Body;
            this.IsRead = entity.IsRead;
            this.Type = (WebNotificationType)entity.Type;
            this.CreatedOn = entity.CreatedOn.ToString(Validations.GenericDateTimeFormat);
            this.Data = !string.IsNullOrEmpty(entity.Data) ? JsonConvert.DeserializeObject<PushNotificationData>(entity.Data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }) : new PushNotificationData();
        }

        public override PushNotification ConvertToEntity(PushNotification entity)
        {
            entity = base.ConvertToEntity(entity);
            entity.ApplicationUserId = this.ApplicationUserId;
            entity.Subject = this.Subject;
            entity.Body = this.Body;
            entity.IsRead = this.IsRead;
            entity.Type = (int)this.Type;
            entity.Data = JSONHelper.ConvertObjectToJSON(this.Data);

            return entity;
        }
    }

    public class PushNotificationData
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int Status { get; set; }
    }
}
