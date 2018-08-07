using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.DTO;
using SSH.Core.Entity;

namespace SSH.Core.IRepository
{
    public interface IUserSessionRepository : IRepository<UserSession, int>
    {
        Task<List<UserSession>> GetByUserIdOrDeviceToken(string userId, string deviceToken);

        Task<List<UserSession>> GetByUserIdOrToken(string userId, string token);

        List<UserSession> GetListByUserId(string userId);

        Task<UserSession> GetByUserId(string userId);

        Task<UserSession> GetByRefreshToken(string refresh_token);
    }
}
