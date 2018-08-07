using System.ComponentModel.DataAnnotations;

namespace SSH.Core.DTO
{
    public class AuthDTO
    {   
        [Required]
        public string EmailId { get; set; }

        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public string DeviceToken { get; set; }
    }
}
