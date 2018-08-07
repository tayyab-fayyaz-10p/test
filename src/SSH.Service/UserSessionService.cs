using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class UserSessionService : Service<IUserSessionRepository, UserSession, UserSessionDTO, int>, IUserSessionService
    {
        private ISSHUnitOfWork unitOfWork;
        private IExceptionHelper exceptionHelper;

        public UserSessionService(ISSHUnitOfWork unitOfWork, IExceptionHelper exceptionHelper) 
            : base(unitOfWork, unitOfWork.UserSessionRepository)
        {
            this.unitOfWork = unitOfWork;
            this.exceptionHelper = exceptionHelper;
        }

        public async Task<List<UserSessionDTO>> GetByUserIdAndDeviceToken(string userId, string deviceToken)
        {
            var result = await this.Repository.GetByUserIdOrDeviceToken(userId, deviceToken);
            return UserSessionDTO.ConvertEntityListToDTOList<UserSessionDTO>(result);
        }

        public async Task<List<UserSessionDTO>> GetByUserIdAndToken(string userId, string token)
        {
            var result = await this.Repository.GetByUserIdOrToken(userId, token);
            return UserSessionDTO.ConvertEntityListToDTOList<UserSessionDTO>(result);
        }

        public List<UserSessionDTO> GetByUserId(string userId)
        {
            var result = this.Repository.GetListByUserId(userId);
            return UserSessionDTO.ConvertEntityListToDTOList<UserSessionDTO>(result);
        }

        public async Task<string> GetDeviceTokenByUserId(string userId)
        {
            var result = await this.GetByUserIdAndDeviceToken(userId, null);
            return result.Select(x => x.DeviceToken).FirstOrDefault();
        }

        public async Task UpdateDeviceToken(string userId, string deviceToken)
        {
            var result = await this.Repository.GetByUserId(userId);
            result.DeviceToken = deviceToken;
            this.UnitOfWork.DBContext.Set<UserSession>().Attach(result);
            this.UnitOfWork.DBContext.Entry(result).Property(x => x.DeviceToken).IsModified = true;
            await this.unitOfWork.SaveAsync();
        }

        public async Task UpdateTokenInfo(string userId, string deviceToken, string token, string refresh_token)
        {
            if (!string.IsNullOrEmpty(deviceToken))
            {
                var userSessions = await this.GetByUserIdAndDeviceToken(userId, deviceToken);
                if (userSessions != null && userSessions.Count() > 0)
                {
                    var ids = userSessions.Select(x => x.Id).ToList();
                    await this.DeleteAsync(ids);
                }

                await this.CreateAsync(new UserSessionDTO { DeviceToken = deviceToken, UserId = userId, Token = token, Refresh_Token = refresh_token });
            }
        }
    }
}
