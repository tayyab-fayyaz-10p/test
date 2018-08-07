using SSH.Core;
using SSH.Core.IService;
using SSH.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SSH.Test.Services
{
    [TestClass]
    public class AuthServiceTest
    {
        private IAuthService _authService;

        [TestInitialize]
        public void init()
        {
            _authService = IoC.Resolve<IAuthService>();
        }

        [TestMethod]
        public async Task Authenticate_Success()
        {
            var result = await _authService.AuthenticateAsync(AuthenticateMock.Authenticate());
            Assert.IsTrue(result.IsAuthenticated == true);
        }

        [TestMethod]
        public async Task Authenticate_Fail()
        {
            var result = await _authService.AuthenticateAsync(AuthenticateMock.Authenticate()); 
            Assert.IsFalse(result.IsAuthenticated == false);
        }

        [TestMethod]
        public async Task LDAPAuthenticationTest()
        {
            var Authenticated = await _authService.AuthenticateAsync(LdapAuthMock.LdapAuthDTO());
            Assert.IsTrue(Authenticated.IsAuthenticated == true);
        }

    }
}
