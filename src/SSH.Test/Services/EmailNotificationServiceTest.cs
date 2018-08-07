using SSH.Core;
using SSH.Core.IService;
using SSH.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SSH.Test.Services
{
    [TestClass]
    public class EmailNotificationServiceTest
    {
        private IEmailNotificationService _emailNotificationService;

        [TestInitialize]
        public void init()
        {
            _emailNotificationService = IoC.Resolve<IEmailNotificationService>();
        }

        [TestMethod]
        public async Task SendEmailNotification()
        {
            var emailInfo = EmailNotificationMock.GetEmailInformation();
            await _emailNotificationService.SendEmailNotification(emailInfo.ToAddress, emailInfo.Body, emailInfo.Subject);
        }
    }
}
