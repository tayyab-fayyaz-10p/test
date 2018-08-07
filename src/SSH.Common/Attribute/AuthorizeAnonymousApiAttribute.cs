using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SSH.Common.Attribute
{
    public class AuthorizeAnonymousApiAttribute : AuthorizeAttribute
    {
        private static string anonymousApiToken = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AnonymousApiToken"]) ? string.Empty : ConfigurationManager.AppSettings["AnonymousApiToken"].ToString();

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (this.Authorize(actionContext))
            {
                return;
            }

            this.HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            actionContext.Response = challengeMessage;
        }

        private bool Authorize(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            IEnumerable<string> values;
            try
            {
                if (actionContext.Request.Headers.GetValues("token") != null)
                {
                    if (actionContext.Request.Headers.TryGetValues("token", out values) && values.First() == anonymousApiToken)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}