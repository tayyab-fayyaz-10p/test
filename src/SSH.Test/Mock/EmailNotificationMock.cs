
using SSH.Core.DTO;

namespace SSH.Test.Mock
{
    public static class EmailNotificationMock
    {
        public static EmailNotificationDTO GetEmailInformation()
        {
            var emailInfo = new EmailNotificationDTO
            {
                ToAddress= "admin@admin.com",
                Body="test body",
                Subject="test subject"
            };
            return emailInfo;
        }
    }
}
