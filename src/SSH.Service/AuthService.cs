using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Enum;
using SSH.Core.Infrastructure;
using SSH.Core.IService;

namespace SSH.Service
{
    public class AuthService : IAuthService
    {
        private IUserService userService;
        private IConfigurationService configurationService;
        private IExceptionHelper exceptionHelper;
        private IUserSessionService userSessionService;
        private ISSHRequestInfo requestInfo;
        
        public AuthService( 
            IUserService userService, 
            IConfigurationService configurationService, 
            IExceptionHelper exceptionHelper,
            IUserSessionService userSessionService,
            ISSHRequestInfo requestInfo)
        {
            this.userService = userService;
            this.configurationService = configurationService;
            this.exceptionHelper = exceptionHelper;
            this.userSessionService = userSessionService;
            this.requestInfo = requestInfo;
        }

        public async Task<JObject> LoginUserAsync(AuthDTO loginData)
        {
            var loginDTO = await this.CheckUserAuthenticationAsync(loginData);
            if (loginDTO.Error != null)
            {
                this.exceptionHelper.ThrowAPIException(HttpStatusCode.Unauthorized, loginDTO.Error);
            }

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", loginDTO.User.UserName),
                new KeyValuePair<string, string>("password", loginData.Password)
            });

            var result = await this.LoginUser(formContent);
            var token = result["access_token"].ToString();
            var refresh_token = result["refresh_token"].ToString();
            await this.userSessionService.UpdateTokenInfo(loginDTO.User.Id, loginData.DeviceToken, token, refresh_token);
            return result;
        }

        public async Task<JObject> RefreshTokenAsync(RefreshTokenDTO refreshTokenData)
        {
            var userSession = await this.userSessionService.Repository.GetByRefreshToken(refreshTokenData.Refresh_token);
            if (userSession != null)
            {
                var formContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshTokenData.Refresh_token)
                });

                var result = await this.LoginUser(formContent);
                return result;
            }
            else
            {
                this.exceptionHelper.ThrowAPIException(HttpStatusCode.Unauthorized, Message.UnAuthrized);
            }

            return null;
        }

        public async Task<LdapAuthenticationDTO> AuthenticateAsync(AuthDTO loginDTO)
        {
            LdapAuthenticationDTO ldapAuthenticationDto = new LdapAuthenticationDTO();
            return ldapAuthenticationDto;
        }

        public async Task<string> LogoutUserAsync()
        {
            var user = await this.userService.GetAsync(this.requestInfo.UserId);
            if (user != null)
            {
                var userSessions = await this.userSessionService.GetByUserIdAndDeviceToken(user.Id, null);
                var ids = userSessions.Select(x => x.Id).ToList();
                await this.userSessionService.DeleteAsync(ids);
            }
            else
            {
                this.exceptionHelper.ThrowAPIException(HttpStatusCode.Unauthorized, string.Format(Message.NotFound, "User"));
            }

            return Message.Successful;
        }

        public async Task UpdateDeviceToken(UpdateDeviceTokenDTO dtoObject)
        {
            await this.userSessionService.UpdateDeviceToken(this.requestInfo.UserId, dtoObject.DeviceToken);
        }

        #region PrivateMethods

        private async Task<LdapAuthRolesDTO> LdapAuthentication(string loginUserName, string loginPassword, string ldapUserName, string ldapPassword, string ldapDomain, string ldapPort)
        {
            LdapAuthRolesDTO ldapAuthRoleDTO = new LdapAuthRolesDTO();
            ldapAuthRoleDTO.UserRole = UserRoles.None;
            try
            {
                using (LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(ldapDomain, Convert.ToInt32(ldapPort))))
                {
                    ldapConnection.SessionOptions.SecureSocketLayer = false;
                    ldapConnection.Credential = new NetworkCredential(ldapUserName, ldapPassword);
                    ldapConnection.AuthType = System.DirectoryServices.Protocols.AuthType.Basic;
                    ldapConnection.Bind();
                    string ldapFilter = string.Format("(&(objectClass={0})({1}={2}))", (object)"person", (object)"cn", (object)loginUserName);

                    List<string> list = new List<string>()
                      {
                        "cn",
                        "sn",
                        "mail",
                        "uid"
                      };
                    foreach (SearchResultEntry searchResultEntry in ((SearchResponse)ldapConnection.SendRequest(new SearchRequest(this.configurationService.LdapSearchType, ldapFilter, SearchScope.Subtree, list.ToArray()))).Entries)
                    {
                        ldapConnection.AuthType = System.DirectoryServices.Protocols.AuthType.Basic;
                        ldapConnection.Bind(new NetworkCredential(searchResultEntry.DistinguishedName, loginPassword));
                        ldapAuthRoleDTO.IsExists = true;
                    }

                    if (ldapAuthRoleDTO.IsExists)
                    {
                        ldapAuthRoleDTO.UserRole = await this.FindUserByRoleAsync(loginUserName, list, ldapConnection);
                    }
                }
            }
            catch (LdapException ex)
            {
                throw ex;
            }

            return ldapAuthRoleDTO;
        }

        private async Task<UserRoles> FindUserByRoleAsync(string loginUserName, List<string> list, LdapConnection ldapConnection)
        {
            string ldapFilter = string.Empty;
            foreach (UserRoles userRoles in Enum.GetValues(typeof(UserRoles)))
            {
                ldapFilter = string.Format("(&(objectClass={0})({1}={2})({3}={4}))", (object)"person", (object)"cn", (object)loginUserName, (object)"memberOf", (object)string.Format(this.configurationService.LdapRoles, userRoles));
                foreach (SearchResultEntry searchResultEntry in ((SearchResponse)ldapConnection.SendRequest(new SearchRequest(this.configurationService.LdapSearchType, ldapFilter, SearchScope.Subtree, list.ToArray()))).Entries)
                {
                    return userRoles;
                }
            }

            this.exceptionHelper.ThrowAPIException(HttpStatusCode.Unauthorized, Message.UserInvalidUserNameOrPassword);
            return UserRoles.None;
        }
        
        private async Task<LoginDTO> CheckUserAuthenticationAsync(AuthDTO authDTO)
        {
            return await this.userService.FindByEmailOrMobileAsync(authDTO);
        }

        private async Task<JObject> LoginUser(FormUrlEncodedContent formContent)
        {
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage responseMessage = await client.PostAsync("/Token", formContent);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            var response = JObject.Parse(responseJson);
            if (response["error"] != null)
            {
                this.exceptionHelper.ThrowAPIException(HttpStatusCode.Unauthorized, response["error_description"].ToString());
                return null;
            }
            else
            {
                return response;
            }
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            client.BaseAddress = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        #endregion
    }
}
