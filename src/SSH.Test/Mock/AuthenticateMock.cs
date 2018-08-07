using SSH.Core.DTO;

namespace SSH.Test.Mock
{
    public static class AuthenticateMock
    {
        public static AuthDTO Authenticate()
        {
            var authenticate = new AuthDTO()
            {
                EmailId = "admin",
                Password = "admin",
                PhoneNumber = "353285062132261"
                // AuthType = AuthType.Sql
            };

            return authenticate;
        }
    }
}
