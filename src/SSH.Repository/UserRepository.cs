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
    public class UserRepository : AuditableRepository<ApplicationUser, string>, IUserRepository
    {
        private ApplicationUserManager userManager;
        private IExceptionHelper exceptionHelper;
        private ISSHRequestInfo requestInfo;

        public UserRepository(ISSHRequestInfo requestInfo, ApplicationUserManager userManager, IExceptionHelper exceptionHelper)
            : base(requestInfo)
        {
            this.requestInfo = requestInfo;
            this.userManager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
            this.userManager.UserValidator = new UserValidator<ApplicationUser>(this.userManager) { AllowOnlyAlphanumericUserNames = false };
            this.exceptionHelper = exceptionHelper;
        }

        protected override IQueryable<ApplicationUser> DefaultListQuery
        {
            get
            {
                return base.DefaultListQuery
                    .Include(x => x.Roles)
                    .Include(x => x.UserSession).OrderByDescending(x => x.Id);
            }
        }

        protected override IQueryable<ApplicationUser> DefaultSingleQuery
        {
            get
            {
                return base.DefaultSingleQuery
                    .Include(x => x.Roles)
                    .Include(x => x.UserSession).OrderByDescending(x => x.Id);
            }
        }

        public async override Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async override Task<IEnumerable<ApplicationUser>> GetAll(JsonApiRequest request)
        {
            IQueryable<ApplicationUser> queryable = this.DefaultListQuery.Include(x => x.Roles).GenerateQuery(request);

            return await queryable.ToListAsync();
        }

        public async Task<TotalResultDTO<ApplicationUser>> GetAllForAdmin(JsonApiRequest request, string search)
        {
            var admin = Roles.GetRoleId(UserRoles.Admin);
            var driver = Roles.GetRoleId(UserRoles.Reception);
            var ssa = Roles.GetRoleId(UserRoles.Pharmacy);
            List<ApplicationUser> result = null;
            List<ApplicationUser> total = null;
            if (string.IsNullOrEmpty(search))
            {
                result = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId != admin && y.RoleId != driver && y.RoleId != ssa)).GenerateQuery(request).ToListAsync();
                total = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId != admin && y.RoleId != driver && y.RoleId != ssa)).ToListAsync();
            }
            else
            {
                result = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId != admin && y.RoleId != driver && y.RoleId != ssa) && (x.Id.StartsWith(search) || x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.PhoneNumber.StartsWith(search) || x.UserName.StartsWith(search))).GenerateQuery(request).ToListAsync();
                total = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId != admin && y.RoleId != driver && y.RoleId != ssa) && (x.Id.StartsWith(search) || x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.PhoneNumber.StartsWith(search) || x.UserName.StartsWith(search))).ToListAsync();
            }

            return new TotalResultDTO<ApplicationUser> { Result = result, TotalRecords = total.Count };
        }

        public async Task<TotalResultDTO<ApplicationUser>> GetAllForITMaker(JsonApiRequest request, string search)
        {
            var reportViewer = Roles.GetRoleId(UserRoles.Reception);
            List<ApplicationUser> result = null;
            List<ApplicationUser> total = null;
            if (string.IsNullOrEmpty(search))
            {
                result = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId == reportViewer)).GenerateQuery(request).ToListAsync();
                total = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId == reportViewer)).ToListAsync();
            }
            else
            {
                result = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId == reportViewer) && (x.Id.StartsWith(search) || x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.PhoneNumber.StartsWith(search) || x.UserName.StartsWith(search))).GenerateQuery(request).ToListAsync();
                total = await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.Roles.Any(y => y.RoleId == reportViewer) && (x.Id.StartsWith(search) || x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.PhoneNumber.StartsWith(search) || x.UserName.StartsWith(search))).ToListAsync();
            }

            return new TotalResultDTO<ApplicationUser> { Result = result, TotalRecords = total.Count };
        }

        public ApplicationUser GetUser(string id)
        {
            return this.DefaultSingleQuery.Where(x => x.Id == id).FirstOrDefault();
        }

        public async override Task DeleteAsync(string id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UserName += "_" + Guid.NewGuid().ToString();

                await this.Update(entity);
            }
        }

        public async Task DeleteMultipleUsersAsync(List<UserIdDTO> dtoList)
        {
            var userIds = dtoList.Select(x => x.Id).ToList();
            var entities = await this.GetAllAsync(userIds);
            if (entities.Count() > 0)
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                    entity.UserName += "_" + Guid.NewGuid().ToString();
                    await this.userManager.UpdateAsync(entity);
                }

                await this.DBContext.SaveChangesAsync();
            }
            else
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.NotFound, "User"));
            }
        }

        public async Task UpdateUserAsync(ApplicationUser entity)
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            entity.LastModifiedBy = this.RequestInfo == null ? string.Empty : RequestInfo.UserId;

            var user = this.FindByEmail(entity.Email, entity.Id);
            if (user != null)
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.AlreadyExist, "Email"));
            }

            await this.userManager.UpdateAsync(entity);
            this.userManager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
        }

        public async Task UpdateUserStatusAsync(List<ChangeUserStatusDTO> dtoObject)
        {
            var userIds = dtoObject.Select(x => x.Id).ToList();
            var entities = await this.GetAllAsync(userIds);
            if (entities.Count() > 0)
            {
                foreach (var entity in entities)
                {
                    entity.Status = dtoObject[0].Status;
                    await this.userManager.UpdateAsync(entity);
                }

                await this.DBContext.SaveChangesAsync();
            }
            else
            {
                this.exceptionHelper.ThrowAPIException(string.Format(Message.NotFound, "User"));
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(List<string> ids)
        {
            return await this.DefaultListQuery.Include(x => x.Roles).Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetByRole(UserRoles role)
        {
            return await this.DefaultListQuery.Include(x => x.Roles).Where(x => x.AccountRole == role.ToString()).ToListAsync();
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser entity, string password, string accountRole)
        {
            try
            {
                entity.CreatedOn = DateTime.UtcNow;
                entity.CreatedBy = this.RequestInfo == null ? string.Empty : RequestInfo.UserId;
                entity.LastModifiedOn = DateTime.UtcNow;
                entity.LastModifiedBy = string.Empty;

                if (accountRole == UserRoles.Reception.ToString())
                {
                    entity.LockoutEnabled = true;
                }

                var user = await this.FindByEmailOrMobileAsync(entity.Email, entity.PhoneNumber);
                if (user != null)
                {
                    if (user.Status == (int)UserStatus.Blocked)
                    {
                        this.exceptionHelper.ThrowAPIException(string.Format(Message.UserBlocked));
                    }
                    else if (user.Email.Equals(entity.Email))
                    {
                        this.exceptionHelper.ThrowAPIException(string.Format(Message.AlreadyExist, "Email"));
                    }
                }

                IdentityResult result = await this.userManager.CreateAsync(entity, password);
                if (!result.Succeeded)
                {
                    this.exceptionHelper.ThrowAPIException(result.Errors.FirstOrDefault());
                }
            }
            catch (DbEntityValidationException databaseException)
            {
                var errors = new List<string>();
                var validationErrors = databaseException.EntityValidationErrors.Select(x => x.ValidationErrors);

                foreach (var error in validationErrors)
                {
                    errors.AddRange(error.Select(x => x.ErrorMessage));
                }

                this.exceptionHelper.ThrowAPIException(errors);
            }

            return entity;
        }

        public async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            return await this.userManager.FindAsync(userName, password);
        }

        public async Task<ApplicationUser> FindByEmailOrMobileAsync(string email, string mobileNumber)
        {
            try
            {
                var result = await this.DefaultSingleQuery.SingleOrDefaultAsync(x => ((x.Email == email && email != null) || (x.CellNumber == mobileNumber && mobileNumber != null)) && !x.IsDeleted);
                return result;
            }
            catch (Exception ex)
            {
                this.exceptionHelper.ThrowAPIException(ex.Message);
            }

            return null;
        }

        public async Task<ApplicationUser> GetByEmailOrMobileAsync(string email, string mobileNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await this.FindByEmailAsync(email);
                    if (user != null)
                    {
                        return user;
                    }

                    return null;
                }

                if (!string.IsNullOrEmpty(mobileNumber))
                {
                    var user = await this.FindByCellNumberAsync(mobileNumber);
                    if (user != null)
                    {
                        return user;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                this.exceptionHelper.ThrowAPIException(ex.Message);
            }

            return null;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            try
            {
                var result = await this.userManager.CheckPasswordAsync(user, password);
                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApplicationUser> FindByUserNameAsync(string userName)
        {
            try
            {
                var result = await this.userManager.FindByNameAsync(userName);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            try
            {
                var result = await this.userManager.FindByEmailAsync(email);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> FindByCellNumberAsync(string phoneNumber)
        {
            try
            {
                var result = await this.DefaultSingleQuery.FirstOrDefaultAsync(x => x.CellNumber == phoneNumber && !x.IsDeleted);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> FindUserNameWithIMEIAsync(string userName, string imeiNumber)
        {
            var user = await this.DefaultSingleQuery.SingleOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                this.exceptionHelper.ThrowAPIException(Message.UserInvalidUserName);
            }

            var result = await this.DefaultSingleQuery.SingleOrDefaultAsync(x => x.UserName == userName);
            if (result == null)
            {
                this.exceptionHelper.ThrowAPIException(Message.UserUnAuthorized);
            }

            return result;
        }

        public ApplicationUser FindByEmail(string email, string userId = null)
        {
            try
            {
                ApplicationUser user = null;
                if (string.IsNullOrEmpty(userId))
                {
                    user = this.DefaultListQuery.FirstOrDefault(x => x.Email == email && !x.IsDeleted);
                }
                else
                {
                    user = this.DefaultListQuery.FirstOrDefault(x => x.Email == email && !x.IsDeleted && x.Id != userId);
                }

                return user;
            }
            catch
            {
                return null;
            }
        }

        public ApplicationUser FindByUserId()
        {
            if (this.RequestInfo != null)
            {
                return this.DefaultListQuery.FirstOrDefault(x => x.Id == this.requestInfo.UserId && !x.IsDeleted);
            }

            return null;
        }

        public async Task<IList<string>> GetUserRoleAsync(string userId)
        {
            var roles = await this.userManager.GetRolesAsync(userId);
            return roles;
        }

        public async Task<ApplicationUser> FindByPasswordTokenAsync(string token)
        {
            return await this.DefaultSingleQuery.SingleOrDefaultAsync(x => x.PasswordToken == token);
        }

        public async Task ChangePasswordAsync(string userId, string newPassword)
        {
            await this.userManager.RemovePasswordAsync(userId);
            await this.userManager.AddPasswordAsync(userId, newPassword);
        }

        public async Task<ChangePasswordDTO> ChangePasswordAsync(ChangePasswordDTO dtoObject)
        {
            var user = await this.FindAsync(dtoObject.EmailId, dtoObject.OldPassword);

            if (user == null)
            {
                this.exceptionHelper.ThrowAPIException("Incorrect old password.");
            }

            var result = await this.userManager.ChangePasswordAsync(user.Id, dtoObject.OldPassword, dtoObject.NewPassword);

            if (!result.Succeeded)
            {
                this.exceptionHelper.ThrowAPIException(result.Errors.ToList());
            }

            return dtoObject;
        }

        public async Task<AssignPasswordDTO> AssignPasswordAsync(ApplicationUser entity, AssignPasswordDTO dtoObject)
        {
            entity.PasswordHash = this.userManager.PasswordHasher.HashPassword(dtoObject.Password);
            this.userManager.UpdateSecurityStamp(entity.Id);

            var result = this.userManager.Update(entity);

            if (!result.Succeeded)
            {
                this.exceptionHelper.ThrowAPIException(result.Errors.ToList());
            }

            this.userManager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
            return dtoObject;
        }
        
        public async Task UpdateUserRoleAsync(string userId, string roleId)
        {
            await this.userManager.RemoveFromRolesAsync(userId, this.userManager.GetRoles(userId).ToArray());
            await this.userManager.AddToRoleAsync(userId, Roles.GetRole(roleId));
        }
        
        public async Task<TotalResultDTO<ApplicationUser>> GetDriverUsersAsync(JsonApiRequest request, string search)
        {
            var roleId = Roles.GetRoleId(UserRoles.Reception);
            List<ApplicationUser> result = null;
            List<ApplicationUser> total = null;
            if (string.IsNullOrEmpty(search))
            {
                result = await this.DefaultListQuery.Where(x => x.Roles.Any(y => y.RoleId == roleId)).GenerateQuery(request).ToListAsync();
                total = await this.DefaultListQuery.Where(x => x.Roles.Any(y => y.RoleId == roleId)).ToListAsync();
            }
            else
            {
                result = await this.DefaultListQuery.Where(x => x.Roles.Any(y => y.RoleId == roleId) && (x.Id.StartsWith(search) || x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.CellNumber.StartsWith(search) || x.UserName.StartsWith(search))).GenerateQuery(request).ToListAsync();
                total = await this.DefaultListQuery.Where(x => x.Roles.Any(y => y.RoleId == roleId) && (x.Id.StartsWith(search) || x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.CellNumber.StartsWith(search) || x.UserName.StartsWith(search))).ToListAsync();
            }

            return new TotalResultDTO<ApplicationUser> { Result = result, TotalRecords = total.Count };
        }

        public async Task<List<ApplicationUser>> GetUnAuthorizePfosAsync()
        {
            var roleId = Roles.GetRoleId(UserRoles.Accounts);
            return await this.DefaultListQuery.Where(x => !x.IsDeleted && x.Roles.Any(y => y.RoleId == roleId)).ToListAsync();
        }

        public async Task<int> ResetOTPCountAsync(string emailId)
        {
            var user = await this.FindByEmailAsync(emailId);
            var updatedUser = await this.userManager.UpdateAsync(user);
            return await this.DBContext.SaveChangesAsync();
        }

        public async Task<int> UpdateOTPAsync(string emailId, string otp)
        {
            var user = await this.FindByEmailAsync(emailId);
            
            var updatedUser = await this.userManager.UpdateAsync(user);
            return await this.DBContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return await this.DefaultSingleQuery.SingleOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await this.DefaultListQuery.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<List<ApplicationUser>> GetAllReportViewersAsync()
        {
            var reportViewerId = Roles.GetRoleId(UserRoles.Reception);
            return await this.DefaultListQuery.Where(x => x.Roles.Any(y => y.RoleId == reportViewerId) && !x.IsDeleted).ToListAsync();
        }

        public async Task<bool> UnlockUserAsync(string userId)
        {
            this.userManager = new ApplicationUserManager(new ApplicationUserStore(this.requestInfo));
            IdentityResult result = await this.userManager.SetLockoutEndDateAsync(userId, DateTimeOffset.MinValue);
            return result.Succeeded;
        }
    }
}