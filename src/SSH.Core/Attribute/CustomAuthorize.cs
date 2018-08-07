using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SSH.Common.Helper;
using SSH.Core.Constant;
using SSH.Core.Enum;
using SSH.Core.IService;

namespace SSH.Core.Attribute
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private bool isUserBlocked = false;

        public CustomAuthorize()
        {
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var user = (ClaimsPrincipal)HttpContext.Current.User;
            var userId = user.Claims.Where(x => x.Type == "userId").Select(y => y.Value).FirstOrDefault();
            var userRole = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

            if (userRole == UserRoles.Reception.ToString() || userRole == UserRoles.Pharmacy.ToString())
            {
                var userSessionsService = IoC.Resolve<IUserSessionService>();
                var userSessions = userSessionsService.GetByUserId(userId);
                var token = actionContext.Request.Headers.Authorization.Parameter;
                if (userSessions != null && userSessions.Count > 0)
                {
                    foreach (var userSession in userSessions)
                    {
                        if (userSession.Token == token)
                        {
                            return;
                        }
                        else
                        {
                            this.HandleUnauthorizedRequest(actionContext);
                        }
                    }
                }
                else
                {
                    this.HandleUnauthorizedRequest(actionContext);
                }
            }
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var errorMessage = "Session Expired";
            if (this.isUserBlocked)
            {
                this.isUserBlocked = false;
                errorMessage = "Your account has been blocked. Please contact HR";
            }

            var challengeMessage = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, (object)Serializer.CreateObject(HttpStatusCode.Unauthorized, errorMessage, null));
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");

            actionContext.Response = challengeMessage;
        }
    }
}
