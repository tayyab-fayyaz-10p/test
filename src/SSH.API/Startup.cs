using System;
using System.Configuration;
using System.IO;
using Hangfire;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using SSH.API.Infrastructure;
using SSH.API.Provider;
using SSH.API.Providers;
using SSH.Core;
using SSH.Core.DBContext;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

[assembly: OwinStartup(typeof(SSH.API.Startup))]

namespace SSH.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureOAuthTokenGeneration(app);
            this.ConfigureOAuthTokenConsumption(app);
            this.ConfigureHangFire(app);
        }

        private void ConfigureHangFire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnectionString");
            app.UseHangfireDashboard();

            var options = new BackgroundJobServerOptions { WorkerCount = 1, Activator = new ContainerJobActivator(IoC.Container) };
            app.UseHangfireServer(options);
            var hangfireIntegeration = IoC.Resolve<IHangfireIntegeration>();
            hangfireIntegeration.MarkJobsExpired();
            hangfireIntegeration.UpdateAverageAndTotal();
            hangfireIntegeration.UpdateRating(0);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            var userService = IoC.Resolve<IUserService>();
            var configurationService = IoC.Resolve<IConfigurationService>();
            var userRolePermissionService = IoC.Resolve<IUserRolePermissionService>();
            var requestInfo = IoC.Resolve<ISSHRequestInfo>();
            var userManager = IoC.Resolve<ApplicationUserManager>();
            OAuthAuthorizationServerOptions authServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(userService, configurationService, userRolePermissionService, userManager, requestInfo),
                AccessTokenFormat = new CustomJwtFormat("http://localhost:59822"),
                RefreshTokenProvider = new ApplicationRefreshTokenProvider(configurationService)
            };

            app.UseOAuthAuthorizationServer(authServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = "http://localhost:59822";
            string audienceId = "414e1927a3884f68abc79f7283837fd1";
            byte[] audienceSecret = TextEncodings.Base64Url.Decode("qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo");

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}