using System;
using Microsoft.AspNet.Identity;
using SSH.Core.Entity;

namespace SSH.Core.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(ApplicationUserStore userStore) : base(userStore)
        {
            this.MaxFailedAccessAttemptsBeforeLockout = 3;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(365 * 100);
        }
    }
}
