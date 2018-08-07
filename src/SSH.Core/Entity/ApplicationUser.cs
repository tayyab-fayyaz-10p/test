using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Recipe.Core.Base.Interface;

namespace SSH.Core.Entity
{
    public class ApplicationUser : IdentityUser<string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>, IAuditModel<string>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string PasswordToken { get; set; }
        
        public DateTime? DateOfJoining { get; set; }

        public string AccountRole { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string CellNumber { get; set; }

        public int Status { get; set; }
        
        public virtual IList<UserSession> UserSession { get; set; }
        
        public virtual IList<PushNotification> PushNotification { get; set; }
    }
}
