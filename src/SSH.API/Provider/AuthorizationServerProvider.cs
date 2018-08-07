using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.API.Provider
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserService userService;
        private readonly IConfigurationService configurationService;
        private readonly IUserRolePermissionService userRolePermissionService;
        private readonly ApplicationUserManager userManager;
        private readonly ISSHRequestInfo requestInfo;

        public AuthorizationServerProvider(IUserService userService, IConfigurationService configurationService, IUserRolePermissionService userRolePermissionService, ApplicationUserManager userManager, ISSHRequestInfo requestInfo)
        {
            this.userService = userService;
            this.configurationService = configurationService;
            this.userRolePermissionService = userRolePermissionService;
            this.requestInfo = requestInfo;
            this.userManager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
            this.userManager.UserValidator = new UserValidator<ApplicationUser>(this.userManager) { AllowOnlyAlphanumericUserNames = false };
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserDTO userDTO = await this.userService.FindByUserNameAsync(context.UserName);
            UserDTO validCredentials = await this.userService.ValidateUserAsync(context.UserName, context.Password);
            
            if (!context.HasError)
            {
                context.Options.AccessTokenExpireTimeSpan = new TimeSpan(24, 0, 0);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                var properties = new AuthenticationProperties();
                identity.AddClaim(new Claim(General.ClaimsUserId, validCredentials.Id));
                identity.AddClaim(new Claim(General.ClaimsUserName, validCredentials.UserName));
                identity.AddClaim(new Claim(General.ClaimsFullName, string.Format("{0} {1} {2}", validCredentials.FirstName, validCredentials.MiddleName, validCredentials.LastName)));
                identity.AddClaim(new Claim(ClaimTypes.Name, validCredentials.UserName)); 
                identity.AddClaim(new Claim(General.ClaimsEmailId, validCredentials.Email));
                identity.AddClaim(new Claim(General.ClaimsTrainingId, string.IsNullOrEmpty(userDTO.OrientationTrainingId) ? string.Empty : userDTO.OrientationTrainingId));
                var role = await this.userService.GetUserRole(validCredentials.Id);
                identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", role.ToArray())));
                var permissions = await this.userRolePermissionService.GetUserRolePermissions(userDTO.Roles.Select(x => x.RoleId).ToList());
                identity.AddClaim(new Claim(General.ClaimsPermissions, permissions));
                var ticket = new AuthenticationTicket(identity, properties);
                context.Validated(ticket);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            return base.TokenEndpoint(context);
        }
    }

    public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
    {
        private IConfigurationService configurationService;

        public ApplicationRefreshTokenProvider(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(this.configurationService.RefreshTokenExpiryTimeInSeconds));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}