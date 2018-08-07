using SSH.Core;
using SSH.Core.IService;
using SSH.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SSH.Test.Services
{
    [TestClass]
    public class AuditServiceTest
    {
        private IAuditService _auditService;

        [TestInitialize]
        public void init()
        {
            _auditService = IoC.Resolve<IAuditService>();
        }

        [TestMethod]
        public async Task GetAll_Success()
        {
            var result = await _auditService.GetAllAsync();
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task GetAll_Fail()
        {
            var result = await _auditService.GetAllAsync();
            Assert.IsFalse(result == null);
        }

        [TestMethod]
        public async Task GetById_Success()
        {
            int id = 1;
            var result = await _auditService.GetAsync(id);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task GetById_Fail()
        {
            int id = 1;
            var result = await _auditService.GetAsync(id);
            Assert.IsFalse(result == null);
        }

    }
}
