using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Recipe.Core.Base.Generic;
using SSH.Core.Attribute;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IService;

namespace SSH.API.Controller
{
    [CustomAuthorize]
    [RoutePrefix("UserSession")]
    public class UserSessionController : Controller<IUserSessionService, UserSessionDTO, UserSession, int>
    {
        public UserSessionController(IUserSessionService service)
            : base(service)
        {
        }
    }
}
