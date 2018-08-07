using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.IRepository;

namespace SSH.Core.IService
{
    public interface IUserSessionService : IService<IUserSessionRepository, UserSession, UserSessionDTO, int>
    {
        Task<List<UserSessionDTO>> GetByUserIdAndDeviceToken(string userId, string deviceToken);

        Task<List<UserSessionDTO>> GetByUserIdAndToken(string userId, string token);

        Task<string> GetDeviceTokenByUserId(string userId);

        Task UpdateTokenInfo(string userId, string deviceToken, string token, string refresh_token);

        Task UpdateDeviceToken(string userId, string deviceToken);

        List<UserSessionDTO> GetByUserId(string userId);
    }
}
