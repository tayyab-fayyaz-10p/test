using SSH.Core.DTO;

namespace SSH.Test.Mock
{
    public static class ChangePasswordMock
    {
        public static ChangePasswordDTO ChangeUserPassword()
        {
            var changePassword = new ChangePasswordDTO()
            {
                EmailId = "unit1.test1@test.com",
                OldPassword = "123456",
                NewPassword = "admin"
            };

            return changePassword;
        }
    }
}
