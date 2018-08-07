using System.ComponentModel.DataAnnotations;
using Recipe.Core.Base.Abstract;

namespace SSH.Core.Entity
{
    public class EmailNotification : EntityBase<int>
    {
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
    }
}
