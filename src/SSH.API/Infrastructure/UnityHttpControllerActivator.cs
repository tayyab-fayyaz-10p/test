using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.Unity;

namespace SSH.API.Infrastructure
{
    public class UnityHttpControllerActivator : IHttpControllerActivator
    {
        private IUnityContainer container;

        public UnityHttpControllerActivator(IUnityContainer container)
        {
            this.container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IUnityContainer childContainer = this.container.CreateChildContainer();
            var controller = (IHttpController)childContainer.Resolve(controllerType);
            request.RegisterForDispose(new ReleaseResource(() => childContainer.Dispose()));

            return controller;
        }
    }
}
