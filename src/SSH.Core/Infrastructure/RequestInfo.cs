using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using SSH.Core.Constant;
using SSH.Core.DBContext;
using SSH.Core.Enum;

namespace SSH.Core.Infrastructure
{
    public class RequestInfo : ISSHRequestInfo
    {
        private const string ApplicationConfigContextKey = "ApplicationConfigContext";
        private Dictionary<int, DbContext> threadContexts = new Dictionary<int, DbContext>();
        private DbContext context;

        public RequestInfo()
        {
        }

        public string UserId
        {
            get
            {
                return this.GetValueFromClaims(General.ClaimsUserId);
            }
        }

        public string UserName
        {
            get
            {
                return this.GetValueFromClaims(General.ClaimsUserName);
            }
        }

        public DbContext Context
        {
            get
            {
                if (this.context == null)
                {
                    this.context = new SSHContext();
                }

                return this.context;
            }

            set
            {
                if (HttpContext.Current == null)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    this.threadContexts[threadId] = value;
                }
                else
                {
                    HttpContext.Current.Items[ApplicationConfigContextKey] = value;
                }
            }
        }

        public string Role
        {
            get
            {
                return this.GetValueFromClaims(ClaimTypes.Role);
            }
        }

        public UserRoles UserRole
        {
            get
            {
                return Roles.GetRoleObject(Roles.GetRoleId(this.Role));
            }
        }

        #region Private Funcltions
        private string GetValueFromClaims(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
            {
                return string.Empty;
            }

            var claims = (HttpContext.Current.User.Identity as ClaimsIdentity).Claims;
            var value = string.Empty;
            if (claims != null && claims.Count() > 0)
            {
                value = claims.FirstOrDefault(x => x.Type == key).Value;
            }

            return value;
        }
        #endregion
    }
}
