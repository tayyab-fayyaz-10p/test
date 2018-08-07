using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using SSH.API.Infrastructure;
using SSH.Common.Attribute;
using SSH.Core.Attribute;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Enum;
using SSH.Core.Infrastructure;
using SSH.Core.IService;

namespace SSH.API.Controller
{
    [CustomAuthorize]
    [RoutePrefix("Auth")]
    public class AuthController : Recipe.Core.Base.Generic.Controller
    {
        private IUserService userService;
        private IAuthService authService;
        private IConfigurationService configurationService;
        private IExceptionHelper exceptionHelper;

        public AuthController(IUserService userService, IConfigurationService configurationService, IExceptionHelper exceptionHelper, IAuthService authService)
        {
            this.userService = userService;
            this.authService = authService;
            this.configurationService = configurationService;
            this.exceptionHelper = exceptionHelper;
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> Login(AuthDTO auth)
        {
            auth.Role = null;
            var result = await this.authService.LoginUserAsync(auth);
            return await this.JsonApiSuccessAsync(Message.Successful, result);
        }

        [Route("DPLogin")]
        [HttpPost]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> DPLogin(AuthDTO auth)
        {
            auth.Role = UserRoles.Reception.ToString();
            var result = await this.authService.LoginUserAsync(auth);
            return await this.JsonApiSuccessAsync(Message.Successful, result);
        }

        [Route("SSALogin")]
        [HttpPost]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> SSALogin(AuthDTO auth)
        {
            auth.Role = UserRoles.Pharmacy.ToString();
            var result = await this.authService.LoginUserAsync(auth);
            return await this.JsonApiSuccessAsync(Message.Successful, result);
        }

        [Route("RefreshToken")]
        [HttpPost]
        [AllowAnonymous]
        [AuthorizeAnonymousApi]
        public async Task<ResponseMessageResult> RefreshToken(RefreshTokenDTO refreshTokenData)
        {
            var result = await this.authService.RefreshTokenAsync(refreshTokenData);
            return await this.JsonApiSuccessAsync(Message.Successful, result);
        }

        [Route("Logout")]
        [HttpPost]
        public async Task<ResponseMessageResult> Logout()
        {
            var result = await this.authService.LogoutUserAsync();
            return await this.JsonApiSuccessAsync(Message.Successful, result);
        }

        [Route("UpdateDeviceToken")]
        [HttpPost]
        public async Task<ResponseMessageResult> UpdateDeviceToken(UpdateDeviceTokenDTO dtoObject)
        {
            await this.authService.UpdateDeviceToken(dtoObject);
            return await this.JsonApiSuccessAsync(Message.Successful, null);
        }

        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<ResponseMessageResult> ForgetPassword(ResetPasswordLinkDTO dtoObject)
        {
            var result = await this.userService.ForgotPassword(dtoObject.UserName);
            if (!string.IsNullOrEmpty(result))
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, new SingleValueDTO { Value = result });
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }

        [HttpPut]
        [Route("ForgotPassword/{token}")]
        public async Task<ResponseMessageResult> ForgotPassword(string token, ForgotPasswordDTO dtoObject)
        {
            var result = await this.userService.ForgotPassword(token, dtoObject);
            if (!string.IsNullOrEmpty(result))
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, new SingleValueDTO { Value = result });
            }

            return await this.JsonApiNoContentAsync(Message.NoContent, null);
        }

        [HttpPut]
        [Route("ValidateToken/{token}")]
        public async Task<ResponseMessageResult> ValidateToken(string token)
        {
            var result = await this.userService.ValidateTokenAsync(token);
            if (result)
            {
                return await this.JsonApiSuccessAsync(Message.SuccessResult, new SingleValueDTO { Value = result });
            }

            return await this.JsonApiBadRequestAsync(Message.UserInvalidToken, new SingleValueDTO { Value = result });
        }
        
        [HttpGet]
        public HttpResponseMessage Angular()
        {
            try
            {
                var response = new HttpResponseMessage();
                response.Content = new StringContent(System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/index.html")));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
