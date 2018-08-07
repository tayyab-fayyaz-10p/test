using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Common.Helper;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser, string>
    {
        Task<TotalResultDTO<ApplicationUser>> GetAllForITMaker(JsonApiRequest request, string search);

        Task<TotalResultDTO<ApplicationUser>> GetAllForAdmin(JsonApiRequest request, string search);

        Task<IEnumerable<ApplicationUser>> GetAllAsync(List<string> ids);

        Task<IEnumerable<ApplicationUser>> GetByRole(UserRoles role);

        Task DeleteMultipleUsersAsync(List<UserIdDTO> dtoList);

        Task<ApplicationUser> FindAsync(string userName, string password);

        Task<ApplicationUser> FindByUserNameAsync(string userName);

        Task<ApplicationUser> FindByEmailAsync(string email);

        Task<ApplicationUser> FindByCellNumberAsync(string phoneNumber);

        Task<ApplicationUser> GetByEmailOrMobileAsync(string email, string mobileNumber);

        Task<ApplicationUser> FindByEmailOrMobileAsync(string email, string mobileNumber);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<ApplicationUser> FindUserNameWithIMEIAsync(string userName, string imeiNumber);

        ApplicationUser FindByEmail(string email, string userId = null);

        ApplicationUser FindByUserId();

        Task<ApplicationUser> FindByPasswordTokenAsync(string token);

        Task ChangePasswordAsync(string userId, string newPassword);

        Task<ApplicationUser> CreateAsync(ApplicationUser entity, string password, string accountRole);

        Task<IList<string>> GetUserRoleAsync(string userId);
        
        Task<ChangePasswordDTO> ChangePasswordAsync(ChangePasswordDTO dtoObject);

        Task<AssignPasswordDTO> AssignPasswordAsync(ApplicationUser entity, AssignPasswordDTO dtoObject);
        
        Task UpdateUserRoleAsync(string userId, string roleId);
        
        Task<TotalResultDTO<ApplicationUser>> GetDriverUsersAsync(JsonApiRequest request, string search);

        Task UpdateUserStatusAsync(List<ChangeUserStatusDTO> dtoList);

        Task<List<ApplicationUser>> GetUnAuthorizePfosAsync();
        
        Task<int> ResetOTPCountAsync(string emailId);

        Task<int> UpdateOTPAsync(string emailId, string otp);

        Task UpdateUserAsync(ApplicationUser entity);

        Task<ApplicationUser> GetUserByUserNameAsync(string userName);

        Task<List<ApplicationUser>> GetAllReportViewersAsync();

        ApplicationUser GetUser(string id);

        Task<bool> UnlockUserAsync(string userId);

        Task<ApplicationUser> GetUserByEmailAsync(string email);       
    }
}
