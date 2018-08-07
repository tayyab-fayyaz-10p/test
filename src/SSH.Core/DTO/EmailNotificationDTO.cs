using System.ComponentModel.DataAnnotations;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class EmailNotificationDTO : DTO<EmailNotification, int>
    {
        public EmailNotificationDTO()
        {
        }

        public EmailNotificationDTO(EmailNotification entity)
            : base(entity)
        {
        }

        [Required]
        [MinLength(1)]
        public string ToAddress { get; set; }

        [Required]
        [MinLength(1)]
        public string Subject { get; set; }

        [Required]
        [MinLength(1)]
        public string Body { get; set; }

        [Required]
        public bool IsSent { get; set; }

        public string Exception { get; set; }

        public int RetryCount { get; set; }

        public override void ConvertFromEntity(EmailNotification entity)
        {
            base.ConvertFromEntity(entity);

            this.ToAddress = entity.ToAddress;
            this.Subject = entity.Subject;
            this.Body = entity.Body;
            this.IsSent = entity.IsSent;
            this.Exception = entity.Exception;
            this.RetryCount = entity.RetryCount;
        }

        public override EmailNotification ConvertToEntity(EmailNotification entity)
        {
            entity = base.ConvertToEntity(entity);

            entity.ToAddress = this.ToAddress;
            entity.Subject = this.Subject;
            entity.Body = this.Body;
            entity.IsSent = this.IsSent;
            entity.Exception = this.Exception;
            entity.RetryCount = this.RetryCount;

            return entity;
        }
    }
}
