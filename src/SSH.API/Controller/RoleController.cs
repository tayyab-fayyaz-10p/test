using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Recipe.Core.Base.Generic;
using SSH.API.Infrastructure;
using SSH.Core.Attribute;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IService;

namespace SSH.API.Controller
{
    [CustomAuthorize]
    [RoutePrefix("Role")]
    public class RoleController : Controller<IRoleService, RoleDTO, ApplicationRole, string>
    {
        public RoleController(IRoleService service)
            : base(service) 
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ResponseMessageResult> GetAll()
        {
            var result = await this.Service.GetAllAsync();
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }

        [HttpGet]
        [Route("GetRoleMapping")]
        public async Task<ResponseMessageResult> GetRoleMapping()
        {
            var result = this.Service.GetCurrentUserRoleMapping();
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }
    }
}
