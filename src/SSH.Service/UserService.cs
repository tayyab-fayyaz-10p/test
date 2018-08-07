using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Recipe.Common.Helper;
using Recipe.Core.Base.Generic;
using SSH.Common.Helper;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.Helper;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class UserService : Service<IUserRepository, ApplicationUser, UserDTO, string>, IUserService
    {
        private ISSHUnitOfWork unitOfWork;
        private IExceptionHelper exceptionHelper;
        private ISSHRequestInfo requestInfo;
        private ApplicationUserManager manager;
        private IEmailNotificationService emailservice;
        private IPermissionService permissionService;
        private IRoleService roleService;
        private IUserRepository userRepository;
        private IOTPRepository otpRepository;
        private IConfigurationService configurationService;

        public UserService(
            ISSHUnitOfWork unitOfWork,
            IExceptionHelper exceptionHelper,
            ISSHRequestInfo requestInfo,
            IEmailNotificationService emailservice,
            IPermissionService permissionService,
            IRoleService roleService,
            IUserRepository userRepository,
            IOTPRepository otpRepository,
            IConfigurationService configurationService)
            : base(unitOfWork, unitOfWork.UserRepository)
        {
            this.unitOfWork = unitOfWork;
            this.exceptionHelper = exceptionHelper;
            this.requestInfo = requestInfo;
            this.emailservice = emailservice;
            this.permissionService = permissionService;
            this.roleService = roleService;
            this.userRepository = userRepository;
            this.otpRepository = otpRepository;
            this.configurationService = configurationService;
            this.manager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
        }

        public async Task<TotalResultDTO<UserDTO>> GetAllUserUpdateAsync(JsonApiRequest request, string search)
        {
            Core.Enum.UserRoles role = new Core.Enum.UserRoles();
            Enum.TryParse(this.requestInfo.Role, out role);

            if (role == UserRoles.Admin)
            {
                var users = await this.Repository.GetAllForAdmin(request, search);
                var result = UserDTO.ConvertEntityListToDTOList<UserDTO>(users.Result);
                return new TotalResultDTO<UserDTO> { Result = result, TotalRecords = users.TotalRecords };
            }

            return null;
        }

        public async Task<IList<UserDTO>> GetDriverUsersAsync(JsonApiRequest request, string search)
        {
            var users = await this.Repository.GetDriverUsersAsync(request, search);
            var result = UserDTO.ConvertEntityListToDTOList<UserDTO>(users.Result);
            return result;
        }

        public async Task<List<UserDTO>> GetUnAuthorizePfosAsync()
        {
            List<UserDTO> retObj = new List<UserDTO>();

            List<ApplicationUser> entities = await this.Repository.GetUnAuthorizePfosAsync();

            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    UserDTO dto = new UserDTO();
                    dto.ConvertFromEntity(entity);
                    retObj.Add(dto);
                }
            }

            return retObj;
        }

        public async Task<LoginDTO> FindByEmailOrMobileAsync(AuthDTO authDto)
        {
            var entity = await this.userRepository.FindByEmailOrMobileAsync(authDto.EmailId, authDto.PhoneNumber);
            if (entity != null)
            {
                if (authDto.Role != null && entity.AccountRole != authDto.Role)
                {
                    return new LoginDTO() { Error = Message.UserUnAuthorized };
                }

                if (entity.Status == (int)UserStatus.Inactive)
                {
                    return new LoginDTO() { Error = Message.UserInActivation };
                }

                if (entity.Status == (int)UserStatus.Freeze)
                {
                    return new LoginDTO() { Error = Message.UserFreeze };
                }

                if (entity.Status == (int)UserStatus.Blocked)
                {
                    return new LoginDTO() { Error = Message.UserBlocked };
                }
                
                var checkPassword = await this.userRepository.CheckPasswordAsync(entity, authDto.Password);
                if (checkPassword)
                {
                    var dto = new UserDTO();
                    dto.ConvertFromEntity(entity);
                    return new LoginDTO() { User = dto };
                }
                else
                {
                    return new LoginDTO() { Error = Message.UserInvalidUserNameOrPassword };
                }
            }

            return new LoginDTO() { Error = Message.UserAccountNotExistCreateNew };
        }

        public async Task<UserDTO> FindByUserNameAsync(string userName)
        {
            var entity = await this.Repository.FindByUserNameAsync(userName);
            if (entity == null)
            {
                return null;
            }

            var dto = new UserDTO();
            dto.ConvertFromEntity(entity);
            return dto;
        }

        public async Task<UserDTO> ValidateUserAsync(string userName, string password)
        {
            try
            {
                this.manager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
                var entity = await this.manager.FindAsync(userName, password);
                if (entity == null)
                {
                    return null;
                }

                UserDTO userDto = await this.GetAsync(entity.Id);
                
                return userDto;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserDTO> GetInitialUserDataAsync()
        {
            var userId = this.requestInfo.UserId;
            var user = await this.GetAsync(userId);
            return user;
        }

        public async Task<string> ForgotPassword(string userName)
        {
            string token = Guid.NewGuid().ToString();

            ApplicationUser user = await this.Repository.FindByUserNameAsync(userName);
            if (user != null && !user.IsDeleted)
            {
                user.PasswordToken = token;
                await this.Repository.Update(user);

                UserDTO dto = new UserDTO(user);

                //await this.notificationService.ForgotPasswordNotificationAsync(dto, token);

                return token;
            }

            return null;
        }

        public async Task DeleteMultipleUsersAsync(List<UserIdDTO> dtoList)
        {
            await this.Repository.DeleteMultipleUsersAsync(dtoList);
        }

        public async Task UpdateUserStatusAsync(List<ChangeUserStatusDTO> dtoStatus)
        {
            await this.Repository.UpdateUserStatusAsync(dtoStatus);
        }

        public async Task<string> ForgotPassword(string passwordToken, ForgotPasswordDTO dtoObject)
        {
            ApplicationUser user = await this.GetByPasswordToken(passwordToken);
            if (user != null && !user.IsDeleted)
            {
                await this.Repository.ChangePasswordAsync(user.Id, dtoObject.NewPassword);

                user.PasswordToken = string.Empty;
                await this.Repository.Update(user);

                return Message.UserPasswordChangeSuccessfully;
            }

            return null;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await this.GetByPasswordToken(token) != null;
        }

        public override async Task<UserDTO> CreateAsync(UserDTO dtoObject)
        {
            dtoObject.Status = UserStatus.Active;
            ApplicationUser user = dtoObject.ConvertToEntity();

            user = await this.Repository.CreateAsync(user, dtoObject.Password, dtoObject.AccountRole);
            dtoObject.Id = user.Id;
            return dtoObject;
        }

        public async Task<IList<string>> GetUserRole(string userId)
        {
            return await Repository.GetUserRoleAsync(userId);
        }

        public override async Task<UserDTO> UpdateAsync(UserDTO dtoObject)
        {
            var databaseEntity = await Repository.GetAsync(dtoObject.Id);
            if (databaseEntity != null)
            {
                databaseEntity.Roles.Clear();
                var entity = dtoObject.ConvertToEntity(databaseEntity);
                await this.Repository.UpdateUserAsync(entity);
            }
            else
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.InvalidObject, dtoObject.UserName));
            }

            return dtoObject;
        }
        
        public async Task<ChangePasswordDTO> ChangePasswordAsync(ChangePasswordDTO dtoObject)
        {
            if (string.IsNullOrEmpty(dtoObject.EmailId))
            {
                dtoObject.EmailId = this.requestInfo.UserName;
            }

            return await Repository.ChangePasswordAsync(dtoObject);
        }

        public async Task<AssignPasswordDTO> AssignPasswordAsync(AssignPasswordDTO dtoObject)
        {
            if (dtoObject.Password.Length < 6)
            {
                this.exceptionHelper.ThrowAPIException(Core.Constant.Message.UserPasswordLength);
            }

            var user = await this.Repository.FindByEmailOrMobileAsync(dtoObject.EmailId, dtoObject.PhoneNumber);

            if (user == null)
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.UserAccountNotExist));
            }

            if (!dtoObject.FromWeb)
            {
                if (user.AccountRole != dtoObject.Role.ToString())
                {
                    this.exceptionHelper.ThrowAPIException(string.Format(Message.UserAccountNotExist));
                }
            }

            return await Repository.AssignPasswordAsync(user, dtoObject);
        }

        public async Task<ChangePasswordDTO> ChangeUserPasswordAsync(ChangePasswordDTO dtoObject)
        {
            if (string.IsNullOrEmpty(dtoObject.EmailId) || string.IsNullOrEmpty(dtoObject.NewPassword) || string.IsNullOrEmpty(dtoObject.OldPassword))
            {
                this.exceptionHelper.ThrowAPIException(Message.UserInvalidUserNameOrPassword);
            }

            var user = await this.Repository.FindAsync(dtoObject.EmailId, dtoObject.OldPassword);
            if (user == null)
            {
                this.exceptionHelper.ThrowAPIException(Message.UserInvalidUserNameOrPassword);
            }

            await Repository.AssignPasswordAsync(user, new AssignPasswordDTO { Password = dtoObject.NewPassword });
            return dtoObject;
        }

        public override async Task DeleteAsync(string id)
        {
            await base.DeleteAsync(id);
        }

        public async Task UpdateUserRole(string userId, string roleId)
        {
            await this.Repository.UpdateUserRoleAsync(userId, roleId);
        }

        public async Task<bool> IsLockedOut(UserDTO dtoObject)
        {
            return await this.manager.IsLockedOutAsync(dtoObject.Id);
        }

        public async Task<bool> GetLockoutEnabled(UserDTO dtoObject)
        {
            return await this.manager.GetLockoutEnabledAsync(dtoObject.Id);
        }

        public async Task<IdentityResult> AccessFailed(UserDTO dtoObject)
        {
            return await this.manager.AccessFailedAsync(dtoObject.Id);
        }

        public async Task<int> GetAccessFailedCount(UserDTO dtoObject)
        {
            return await this.manager.GetAccessFailedCountAsync(dtoObject.Id);
        }

        public async Task<bool> VerifyUserNameWithIMEIForLogin(UserNameDTO dtoObject)
        {
            var user = await this.GetUser(dtoObject);
            if (user == null)
            {
                this.exceptionHelper.ThrowAPIException(Core.Constant.Message.UserInvalidUserNameOrImeiNumber);
            }

            return true;
        }
        
        public bool FindByEmail(string email, string userId = null)
        {
            var user = this.Repository.FindByEmail(email, userId);
            return user == null;
        }

        public async Task<bool> UnlockUserAsync(string userId)
        {
            return await this.Repository.UnlockUserAsync(userId);
        }

        public async Task<bool> UserNameExists(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                var user = await this.Repository.FindByUserNameAsync(userName);
                return user != null && !user.IsDeleted;
            }

            return false;
        }

        public async Task<bool> EmailExists(string emailId)
        {
            if (!string.IsNullOrWhiteSpace(emailId))
            {
                var user = await this.Repository.FindByEmailAsync(emailId);
                return user != null && !user.IsDeleted;
            }

            return false;
        }

        public async Task<bool> EmailOrMobileExists(string email, string mobileNumber, UserRoles userRole, bool fromWeb)
        {
            var user = await this.userRepository.GetByEmailOrMobileAsync(email, mobileNumber);
            if (user == null)
            {
                this.exceptionHelper.ThrowAPIException(Core.Constant.Message.UserAccountNotExist);
            }
            
            if (user.LockoutEndDateUtc != null)
            {
                this.exceptionHelper.ThrowAPIException(Core.Constant.Message.UserAccountLocked);
            }

            if (!fromWeb)
            {
                if (user.AccountRole != userRole.ToString())
                {
                    this.exceptionHelper.ThrowAPIException(string.Format(Message.UserAccountNotExist));
                }
            }

            var otpString = new CommonHelper().GenerateOTP();

            var otp = new OTPDTO
            {
                PhoneNumber = string.IsNullOrEmpty(email) ? mobileNumber : email,
                OneTimePin = otpString,
                Expiry = DateTime.UtcNow.AddMinutes(this.configurationService.OTPExpiry)
            };

            var deleteExisting = await this.otpRepository.DeleteExisting(otp);
            await this.Repository.UpdateOTPAsync(user.Email, otpString);
            await this.otpRepository.CreateOtp(otp.ConvertToEntity());
            await this.UnitOfWork.SaveAsync();

            if (!string.IsNullOrEmpty(email))
            {
                await this.emailservice.SendEmailNotification(email, string.Format(Message.OTPBody, string.Format("{0} {1}", user.FirstName, user.LastName), otpString), Message.OTPSubject);
            }

            if (!string.IsNullOrEmpty(mobileNumber))
            {
                await this.emailservice.SendSMS(mobileNumber, "Your OTP is : " + otpString);
            }

            return true;
        }
        
        #region Private Function
        private async Task<List<UserDTO>> GetReportViewerUsersAsync()
        {
            var reportViewers = await this.Repository.GetAllReportViewersAsync();
            return UserDTO.ConvertEntityListToDTOList<UserDTO>(reportViewers);
        }

        private async Task<ApplicationUser> GetByPasswordToken(string token)
        {
            ApplicationUser user = await this.Repository.FindByPasswordTokenAsync(token);

            if (user == null)
            {
                ((ISSHUnitOfWork)UnitOfWork).ExceptionHelper.ThrowAPIException(Core.Constant.Message.UserInvalidToken);
            }

            ApplicationUser entity = await this.Repository.GetAsync(user.Id);
            return entity;
        }

        private async Task<ApplicationUser> GetUser(UserNameDTO dtoObject)
        {
            if (dtoObject == null || string.IsNullOrEmpty(dtoObject.UserName) || string.IsNullOrEmpty(dtoObject.ImeiNumber))
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Core.Constant.Message.ObjectRequired, "Username or Imei"));
            }

            var user = await this.Repository.FindUserNameWithIMEIAsync(dtoObject.UserName, dtoObject.ImeiNumber);

            return user;
        }
        #endregion
    }
}
