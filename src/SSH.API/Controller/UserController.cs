using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Recipe.Common.Helper;
using Recipe.Core.Base.Generic;
using SSH.API.Infrastructure;
using SSH.Common.Attribute;
using SSH.Core.Attribute;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.IService;

namespace SSH.API.Controller
{
    [CustomAuthorize]
    [RoutePrefix("User")]
    public class UserController : Controller<IUserService, UserDTO, ApplicationUser, string>
    {
        public UserController(IUserService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("InitialUserData")]
        public async Task<UserDTO> GetInitialUserData()
        {            
            return await this.Service.GetInitialUserDataAsync();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ResponseMessageResult> GetAllUsers(string search = null)
        {
            var request = Request.GetJsonApiRequest();
            var result = await this.Service.GetAllUserUpdateAsync(request, search);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }
        
        [HttpGet]
        [Route("GetSingle")]
        public async Task<ResponseMessageResult> GetSingleUser(string id)
        {
            var result = await this.Get(id);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }
        
        [HttpGet]
        [Route("GetDeliveryPartners")]
        public async Task<ResponseMessageResult> GetDriverUsers(string search = null)
        {
            var request = Request.GetJsonApiRequest();
            var result = await this.Service.GetDriverUsersAsync(request, search);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }
        
        [HttpPost]
        [Route("Create")]
        public async Task<ResponseMessageResult> CreateUser(UserDTO dtoObject)
        {
            var result = await this.Service.CreateAsync(dtoObject);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }

        [HttpPut]
        public async Task<ResponseMessageResult> Put(UserDTO dtoObject)
        {
            var result = await this.Service.UpdateAsync(dtoObject);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }

        public override Task Delete(string id)
        {
            return base.Delete(id);
        }

        [HttpPost]
        [Route("password/assign")]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> AssignPassword(AssignPasswordDTO dtoObject)
        {
            var result = await Service.AssignPasswordAsync(dtoObject);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.UserAssignPasswordSuccessfully, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ResponseMessageResult> ChangeUserPassword(ChangePasswordDTO dtoObject)
        {
            var result = await Service.ChangePasswordAsync(dtoObject);
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.UserPasswordChangeSuccessfully, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }
        
        [HttpDelete]
        [Route("DeleteList")]
        public async Task<ResponseMessageResult> DeleteMultipleUsers(List<UserIdDTO> dtoList)
        {
            await this.Service.DeleteMultipleUsersAsync(dtoList);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, null);
        }
        
        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<ResponseMessageResult> UpdateStatus(List<ChangeUserStatusDTO> dtoStatus)
        {
            await this.Service.UpdateUserStatusAsync(dtoStatus);
            return await this.JsonApiSuccessAsync(Message.SuccessResult, null);
        }

        [HttpGet]
        [Route("GetUnAuthPfos")]
        public async Task<ResponseMessageResult> GetUnAuthorizePfos()
        {
            var result = await this.Service.GetUnAuthorizePfosAsync();
            if (result != null)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }
        
        [HttpPost]
        [Route("UnlockUser")]
        public async Task<ResponseMessageResult> UnlockUser(UserIdDTO userId)
        {
            var result = await this.Service.UnlockUserAsync(userId.Id);
            if (result)
            {
                return await this.JsonApiSuccessAsync(Message.UserUnlockSuccessful, new SingleValueDTO { Value = result });
            }

            return await this.JsonApiBadRequestAsync(Message.Error, null);
        }

        [HttpGet]
        [Route("CheckEmail")]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> CheckEmail(string emailId)
        {
            var result = await this.Service.EmailExists(emailId);
            if (!result)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, result);
            }

            return await this.JsonApiBadRequestAsync(string.Format(Message.AlreadyExist, "Email"), null);
        }

        [HttpGet]
        [Route("send/otp")]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> EmailExists(string emailId, string phoneNumber, UserRoles role, bool fromWeb = false)
        {
            var result = await this.Service.EmailOrMobileExists(emailId, phoneNumber, role, fromWeb);
            if (result)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, new SingleValueDTO { Value = result });
            }

            return await this.JsonApiNotFoundAsync(string.Format(Message.NotFound, "Email/Mobile number"), null);
        }
        
        [Route("Password")]
        [HttpPost]
        public dynamic Password(SingleValueDTO dto)
        {
            return new { message = "Success" };
        }
    }
}