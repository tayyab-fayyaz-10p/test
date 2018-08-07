using SSH.Core;
using SSH.Core.IService;
using SSH.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SSH.Test.Services
{
    [TestClass]
    public class RoleServiceTest
    {
        private IRoleService _roleService;

        [TestInitialize]
        public void init()
        {
            _roleService = IoC.Resolve<IRoleService>();
        }

        [TestMethod]
        public async Task GetAll_Success()
        {
            var result = await _roleService.GetAllAsync();
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task GetAll_Fail()
        {
            var result = await _roleService.GetAllAsync();
            Assert.IsFalse(result == null);
        }

        [TestMethod]
        public async Task GetRoleMapping_Success()
        {
            var result = _roleService.GetCurrentUserRoleMapping();
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task GetRoleMapping_Fail()
        {
            var result = _roleService.GetCurrentUserRoleMapping();
            Assert.IsFalse(result == null);
        }

    }
}
