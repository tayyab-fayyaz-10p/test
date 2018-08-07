using SSH.Core.DTO;

namespace SSH.Test.Mock
{
    public static class OneTimePinMock
    {
        public static OneTimePinDTO GetOneTimePin()
        {
            var otp = new OneTimePinDTO
            {
               // OneTimePin = "1234",
               // UserName = "admin_admin"

                 OneTimePin = "9568",
                EmailId = "admin"
            };
            return otp;
        }
    }
}
