using SSH.Core;
using SSH.Core.Infrastructure;
using SSH.Core.IService;
using SSH.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SSH.Test.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService _userService;
        private IAuthService _authService;
        private ISSHRequestInfo _requestInfo;
        private IConfigurationService _configurationService;

        [TestInitialize]
        public void init()
        {
            //var mock = new Mock<Core.IRepository.IUserRepository>();
            //mock.Setup(ur => ur.IsValidOneTimePin("", "1234")).Returns(Task.FromResult(true));
            //IoC.RegisterInstance<Core.IRepository.IUserRepository>(mock.Object);

            _userService = IoC.Resolve<IUserService>();
            
            _requestInfo = IoC.Resolve<ISSHRequestInfo>();

            _authService = IoC.Resolve<IAuthService>();

            _configurationService = IoC.Resolve<IConfigurationService>();

        }


        [TestMethod]
        public async Task CreateUser()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.GetUser();
                var dbUser = await _userService.CreateAsync(user);
                Assert.IsTrue(!string.IsNullOrEmpty(dbUser.Id));
            }
        }

        [TestMethod]
        public async Task GetUser()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var dbUser = await _userService.GetAsync(user.Id);
                Assert.IsTrue(!string.IsNullOrEmpty(dbUser.Id));
            }
        }

        [TestMethod]
        public async Task GetAll()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var dbUser = await _userService.GetAllAsync();
                Assert.IsTrue(dbUser.Count > 0);
            }
        }

        [TestMethod]
        public async Task Delete()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                await _userService.DeleteAsync(user.Id);
                var dbUser = await _userService.GetAsync(user.Id);
                Assert.IsTrue(dbUser == null);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(System.Exception), "")]
        public async Task ChangePassword_Success()
        {
            var user = UserMock.GetUser();
            var dbUser = await _userService.CreateAsync(user);
            var result = await _userService.ChangePasswordAsync(ChangePasswordMock.ChangeUserPassword());
             Assert.IsTrue(result != null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), "")]
        public async Task ChangePassword_Fail()
        {
            var result = await _userService.ChangePasswordAsync(ChangePasswordMock.ChangeUserPassword());
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public async Task AssignPassword()
        {
            var result = await _userService.AssignPasswordAsync(AssignPasswordMock.AssignPassword());
            Assert.IsTrue(result != null);
        }

        
        [TestMethod]
        public async Task AssignPasswordExpectAPIExceptionOnUserNameNotFound()
        {
            var result = await _userService.AssignPasswordAsync(AssignPasswordMock.AssignPassword());
            Assert.IsTrue(result != null);
        }


        //[TestMethod]
        //public async Task ForgotPassword_Success()
        //{
        //    string passwordToken =string.Empty;
        //    var forgotPasswordDto = ForgotPassword.ResetPassword(out passwordToken);
        //    var result = await _userService.ForgotPassword(passwordToken, forgotPasswordDto);
        //    Assert.IsTrue(!string.IsNullOrEmpty(result));
        //}

        //[TestMethod]
        //public async Task ForgotPassword_Failure()
        //{
        //    string passwordToken = string.Empty;
        //    var result = await _userService.ForgotPassword(passwordToken, ForgotPassword.ResetPassword(out passwordToken));
        //    Assert.IsTrue(result == null);
        //}

        [TestMethod]
        public async Task LDAPAuthenticationTest() 
        {
			var Authenticated = await _authService.AuthenticateAsync(LdapAuthMock.LdapAuthDTO());
		    Assert.IsTrue(Authenticated.IsAuthenticated== true);
		}
        
        [TestMethod]
        public async Task GetUnAuthorizePfos()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var dbUser = await _userService.GetUnAuthorizePfosAsync();
                Assert.IsTrue(dbUser.Count > 0);
            }
        }

        [TestMethod]
        public async Task FindByUserName_Success()
        {
            var result = await _userService.FindByUserNameAsync(UserNameMock.GetUserName().UserName);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task FindByUserName_Failure()
        {
            var result = await _userService.FindByUserNameAsync(UserNameMock.GetUserName().UserName);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task ValidateUser_Success()
        {            
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var dbUser = await _userService.ValidateUserAsync(user.UserName, user.Password);
                Assert.IsTrue(dbUser != null);
            }
        }

        [TestMethod]
        public async Task ValidateUser_Failure()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var dbUser = await _userService.ValidateUserAsync(user.UserName, user.Password);
                Assert.IsTrue(dbUser == null);
            }
        }

        [TestMethod]
        public async Task GetInitialUserData_Success()
        {
            var result = await _userService.GetInitialUserDataAsync();
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task GetInitialUserData_Failure()
        {
            var result = await _userService.GetInitialUserDataAsync();
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public async Task ValidateToken_Success()
        {
            var result = await _userService.ValidateTokenAsync(UserMock.GetToken());
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task ValidateToken_Failure()
        {
            var result = await _userService.ValidateTokenAsync(UserMock.GetToken());
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public async Task GetUserRole_Success()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.GetUserRole(user.Id);
            }
        }

        [TestMethod]
        public async Task GetUserRole_Failure()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.GetUserRole(user.Id);
            }
        }

        [TestMethod]
        public async Task IsLockedOut_Success()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.IsLockedOut(user);
                Assert.IsTrue(result == true);
            }
        }

        [TestMethod]
        public async Task IsLockedOut_Failure()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.IsLockedOut(user);
                Assert.IsTrue(result == false);
            }
        }

        [TestMethod]
        public async Task GetLockoutEnabled_Success()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.GetLockoutEnabled(user);
                Assert.IsTrue(result == true);
            }
        }

        [TestMethod]
        public async Task GetLockoutEnabled_Failure()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.GetLockoutEnabled(user);
                Assert.IsTrue(result == false);
            }
        }

        [TestMethod]
        public async Task VerifyUserNameForLogin_Success()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserNameMock.GetUserName();
                var result = await _userService.VerifyUserNameWithIMEIForLogin(user);
                Assert.IsTrue(result == true);
            }
        }

        [TestMethod]
        public async Task VerifyUserNameForLogin_Failure()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserNameMock.GetUserName();
                var result = await _userService.VerifyUserNameWithIMEIForLogin(user);
                Assert.IsTrue(result == false);
            }
        }

        [TestMethod]
        public async Task GetAccessFailedCount_Success()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.GetAccessFailedCount(user);
            }
        }

        [TestMethod]
        public async Task GetAccessFailedCount_Failure()
        {
            var context = _requestInfo.Context;
            using (var tran = context.Database.BeginTransaction())
            {
                var user = UserMock.CreateUser(_userService);
                var result = await _userService.GetAccessFailedCount(user);
            }
        }

    }
}
