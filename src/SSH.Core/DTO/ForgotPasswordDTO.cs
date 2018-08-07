using System.ComponentModel.DataAnnotations;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string NewPassword { get; set; }
    }
}
