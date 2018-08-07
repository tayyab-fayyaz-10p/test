using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Recipe.Common.Helper;
using Recipe.Core.Attribute;
using Recipe.Core.Base.Interface;
using Recipe.Core.Enum;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IUserService : IService<IUserRepository, ApplicationUser, UserDTO, string>
    {
        Task<TotalResultDTO<UserDTO>> GetAllUserUpdateAsync(JsonApiRequest request, string search);

        Task<UserDTO> ValidateUserAsync(string userName, string password);

        Task<UserDTO> FindByUserNameAsync(string userName);

        Task<UserDTO> GetInitialUserDataAsync();

        Task<string> ForgotPassword(string userName);

        Task<string> ForgotPassword(string token, ForgotPasswordDTO dtoObject);

        Task<bool> ValidateTokenAsync(string token);

        Task<IList<string>> GetUserRole(string userId);
        
        Task<ChangePasswordDTO> ChangePasswordAsync(ChangePasswordDTO dtoObject);

        [AuditOperation(OperationType.Create)]
        Task<AssignPasswordDTO> AssignPasswordAsync(AssignPasswordDTO dtoObject);

        Task<ChangePasswordDTO> ChangeUserPasswordAsync(ChangePasswordDTO dtoObject);
        
        [AuditOperation(OperationType.Update)]
        Task UpdateUserRole(string userId, string roleId);
        
        Task<bool> IsLockedOut(UserDTO dtoObject);

        Task<bool> GetLockoutEnabled(UserDTO dtoObject);

        Task<IdentityResult> AccessFailed(UserDTO dtoObject);

        Task<int> GetAccessFailedCount(UserDTO dtoObject);

        //[AuditOperation(OperationType.Delete)]
        Task DeleteMultipleUsersAsync(List<UserIdDTO> dtoList);

        Task<IList<UserDTO>> GetDriverUsersAsync(JsonApiRequest request, string search);

        [AuditOperation(OperationType.Update)]
        Task UpdateUserStatusAsync(List<ChangeUserStatusDTO> dtoList);

        Task<List<UserDTO>> GetUnAuthorizePfosAsync();
        
        Task<bool> VerifyUserNameWithIMEIForLogin(UserNameDTO dtoObject);
        
        bool FindByEmail(string email, string userId = null);
        
        Task<LoginDTO> FindByEmailOrMobileAsync(AuthDTO authDto);

        [AuditOperation(OperationType.Create)]
        Task<bool> UnlockUserAsync(string userId);

        Task<bool> UserNameExists(string userName);

        Task<bool> EmailExists(string emailId);
        
        Task<bool> EmailOrMobileExists(string email, string mobileNumber, UserRoles userRole, bool fromWeb);
    }
}
