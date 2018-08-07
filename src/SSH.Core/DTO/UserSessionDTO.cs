using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Abstract;
using SSH.Core.Entity;

namespace SSH.Core.DTO
{
    public class UserSessionDTO : DTO<UserSession, int>
    {
        public string UserId { get; set; }
        
        public string Token { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceType { get; set; }

        public string Refresh_Token { get; set; }

        public string AppVersion { get; set; }

        public override void ConvertFromEntity(UserSession entity)
        {
            base.ConvertFromEntity(entity);
            this.AppVersion = entity.AppVersion;
            this.DeviceToken = entity.DeviceToken;
            this.DeviceType = entity.DeviceType;
            this.Token = entity.Token;
            this.UserId = entity.UserId;
            this.Refresh_Token = entity.Refresh_Token;
        }

        public override UserSession ConvertToEntity(UserSession entity)
        {
            entity = base.ConvertToEntity(entity);
            entity.AppVersion = this.AppVersion;
            entity.DeviceToken = this.DeviceToken;
            entity.DeviceType = this.DeviceType;
            entity.Token = this.Token;
            entity.UserId = this.UserId;
            entity.Refresh_Token = this.Refresh_Token;

            return entity;
        }
    }
}
