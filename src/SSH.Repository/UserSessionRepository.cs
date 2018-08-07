using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class UserSessionRepository : AuditableRepository<UserSession, int>, IUserSessionRepository
    {
        public UserSessionRepository(ISSHRequestInfo requestInfo)
            : base(requestInfo)
        {
        }

        protected override IQueryable<UserSession> DefaultListQuery
        {
            get
            {
                return base.DefaultListQuery
                    .Include(x => x.User).OrderByDescending(x => x.Id);
            }
        }

        protected override IQueryable<UserSession> DefaultSingleQuery
        {
            get
            {
                return base.DefaultSingleQuery
                    .Include(x => x.User).OrderByDescending(x => x.Id);
            }
        }

        public async Task<List<UserSession>> GetByUserIdOrDeviceToken(string userId, string deviceToken)
        {
            return await this.DefaultListQuery.Where(x => x.UserId == userId || x.DeviceToken == deviceToken).ToListAsync();
        }

        public async Task<UserSession> GetByUserId(string userId)
        {
            return await this.DefaultSingleQuery.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<UserSession> GetByRefreshToken(string refresh_token)
        {
            return await this.DefaultSingleQuery.FirstOrDefaultAsync(x => x.Refresh_Token == refresh_token);
        }

        public async Task<List<UserSession>> GetByUserIdOrToken(string userId, string token)
        {
            return await this.DefaultListQuery.Where(x => x.UserId == userId || x.Token == token).ToListAsync();
        }

        public List<UserSession> GetListByUserId(string userId)
        {
            return this.DefaultListQuery.Where(x => x.UserId == userId).ToList();
        }
    }
}
