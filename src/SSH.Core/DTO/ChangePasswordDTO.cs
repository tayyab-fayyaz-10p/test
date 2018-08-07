namespace SSH.Core.DTO
{
    public class ChangePasswordDTO
    {
        public string EmailId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
