using System.ComponentModel.DataAnnotations;
using Recipe.Core.Base.Abstract;

namespace SSH.Core.Entity
{
    public class PushNotification : EntityBase<int>
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
        
        public bool IsRead { get; set; }

        public int Type { get; set; }

        public string Data { get; set; }
    }
}
