using SSH.Core.DTO;

namespace SSH.Test.Mock
{
    public static class UserNameMock
    {
        public static UserNameDTO GetUserName()
        {
            var userName = new UserNameDTO
            {
                UserName = "admin",
                ImeiNumber = "123"
            };
            return userName;
        }
    }
}
