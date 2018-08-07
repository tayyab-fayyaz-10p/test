using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class AssignPasswordDTO
    {
        public string EmailId { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public UserRoles Role { get; set; }

        public bool FromWeb { get; set; }
    }
}
