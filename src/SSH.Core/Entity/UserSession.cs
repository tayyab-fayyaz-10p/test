using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe.Core.Base.Abstract;

namespace SSH.Core.Entity
{
    public class UserSession : EntityBase<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Token { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceType { get; set; }

        public string Refresh_Token { get; set; }

        public string AppVersion { get; set; }
    }
}
