using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace SSH.API.Provider
{
    public class CentralizedPrefixProvider : DefaultDirectRouteProvider
    {
        private readonly string centralizedPrefix;

        public CentralizedPrefixProvider(string centralizedPrefix)
        {
            this.centralizedPrefix = centralizedPrefix;
        }

        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);

            if (existingPrefix == null)
            {
                return this.centralizedPrefix;
            }

            return string.Format("{0}/{1}", this.centralizedPrefix, existingPrefix);
        }

        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
        }
    }
}