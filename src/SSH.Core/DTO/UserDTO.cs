using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Recipe.Core.Base.Abstract;
using SSH.Core.Constant;
using SSH.Core.Entity;
using SSH.Core.Enum;

namespace SSH.Core.DTO
{
    public class UserDTO : DTO<ApplicationUser, string>
    {
        public UserDTO()
        {
        }

        public UserDTO(ApplicationUser entity) : base(entity)
        {
        }
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", this.FirstName, this.MiddleName, this.LastName);
            }
        }
        
        public string UserName { get; set; }

        public string Email { get; set; }

        public string CountryCode { get; set; }

        public int? CountryId { get; set; }

        public string Password { get; set; }

        public bool IsLocked { get; set; }

        [Required]
        public List<UserRoleDTO> Roles { get; set; }

        public int? DesignationId { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string AccountRole { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        public string CellNumber { get; set; }

        public string LandLineNumber { get; set; }

        public string Education { get; set; }

        public string Address { get; set; }
        
        public string CreatedOn { get; set; }

        public string LastModifiedOn { get; set; }

        public string LockoutEndDateUtc { get; set; }

        public UserStatus Status { get; set; }

        public string Reason { get; set; }

        public string ReasonKey { get; set; }

        public List<int> SubStations { get; set; }

        public string OrientationTrainingId { get; set; }

        public override ApplicationUser ConvertToEntity(ApplicationUser entity)
        {
            entity = base.ConvertToEntity(entity);

            entity.FirstName = this.FirstName;
            entity.LastName = this.LastName;
          
            if (!string.IsNullOrEmpty(entity.Id) && this.Roles.Any())
            {
                foreach (var role in this.Roles)
                {
                    entity.Roles.Add(new ApplicationUserRole { RoleId = role.RoleId, UserId = entity.Id });
                }
            }

            entity.Email = this.Email;
            entity.UserName = this.Email;
            entity.EmailConfirmed = true;
            entity.CellNumber = this.CellNumber;
            entity.AccountRole = this.AccountRole;
            entity.DateOfBirth = this.DateOfBirth;
            entity.DateOfJoining = this.DateOfJoining;
            entity.CreatedOn = DateTime.UtcNow;
            entity.Status = (int)this.Status;
            return entity;
        }

        public override void ConvertFromEntity(ApplicationUser entity)
        {
            base.ConvertFromEntity(entity);
            this.FirstName = entity.FirstName;
            this.LastName = entity.LastName;
            this.UserName = entity.UserName;
            this.Email = entity.Email;
            
            if (entity.Roles != null && entity.Roles.Count > 0)
            {
                this.Roles = entity.Roles.Select(x => new UserRoleDTO { RoleId = x.RoleId, UserId = x.UserId }).ToList();
            }

            this.CellNumber = entity.CellNumber;
            this.LandLineNumber = entity.PhoneNumber;
            this.AccountRole = entity.AccountRole;
            this.DateOfBirth = entity.DateOfBirth;
            this.DateOfJoining = entity.DateOfJoining;
            this.CreatedOn = entity.CreatedOn.ToString(Validations.DateFormat);
            this.LastModifiedOn = (entity.LastModifiedOn != null) ? entity.LastModifiedOn.ToString(Validations.DateFormat) : string.Empty;
            this.Password = entity.PasswordHash;
            this.Status = (UserStatus)entity.Status;
            this.LockoutEndDateUtc = entity.LockoutEndDateUtc.HasValue ? entity.LockoutEndDateUtc.Value.ToString(Validations.DateFormat) : null;
        }
        
        private string SetStatus(int status)
        {
            Enum.UserStatus statusText = (Enum.UserStatus)status;
            return statusText.ToString();
        }
    }
}