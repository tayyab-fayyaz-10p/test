namespace SSH.Core.DTO
{
    public class VerifyUserNameDTO : SingleValueDTO
    {
        public string EntityType { get; set; }

        public string OTP { get; set; }
    }
}
