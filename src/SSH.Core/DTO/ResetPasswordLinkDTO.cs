using System.ComponentModel.DataAnnotations;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class ResetPasswordLinkDTO
    {
        [Required]
        [RegularExpression(Constant.Validations.EmailAddress, ErrorMessage = "")]
        public string UserName { get; set; }
    }
}
