using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;

namespace SSH.Repository
{
    public class UnitOfWork : ISSHUnitOfWork
    {
        private readonly ISSHRequestInfo sshRequestInfo;
        private readonly IUserRepository userRepository;
        private readonly IAuditRepository auditRepository;
        private readonly IEmailNotificationRepository emailNotificationRepository;
        private readonly IOTPRepository otpRepository;
        private readonly IExceptionHelper exceptionHelper;
        private readonly ILOVRepository lovRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRequestInfo requestInfo;
        private readonly IUserRolePermissionRepository userRolePermissionRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IPushNotificationRepository pushNotificationRepository;
        private readonly IUserSessionRepository userSessionRepository;
        
        public UnitOfWork(
            ISSHRequestInfo sshRequestInfo,
            IUserRepository userRepository,
            IAuditRepository auditRepository,
            IEmailNotificationRepository emailNotificationRepository,
            IOTPRepository otpRepository,
            IExceptionHelper exceptionHelper,
            ILOVRepository lovRepository,
            ISSHRequestInfo requestInfo,
            IUserRolePermissionRepository userRolePermissionRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IPushNotificationRepository pushNotificationRepository,
            IUserSessionRepository userSessionRepository)
        {
            this.sshRequestInfo = sshRequestInfo;
            this.userRepository = userRepository;
            this.auditRepository = auditRepository;
            this.exceptionHelper = exceptionHelper;
            this.exceptionHelper = exceptionHelper;
            this.emailNotificationRepository = emailNotificationRepository;
            this.otpRepository = otpRepository;
            this.lovRepository = lovRepository;
            this.roleRepository = roleRepository;
            this.requestInfo = requestInfo;
            this.userRolePermissionRepository = userRolePermissionRepository;
            this.permissionRepository = permissionRepository;
            this.pushNotificationRepository = pushNotificationRepository;
            this.userSessionRepository = userSessionRepository;
        }

        public DbContext DBContext
        {
            get
            {
                return this.sshRequestInfo.Context;
            }
        }

        public ISSHRequestInfo SSHRequestInfo
        {
            get
            {
                return this.sshRequestInfo;
            }
        }
        
        public IUserRepository UserRepository
        {
            get
            {
                return this.userRepository;
            }
        }

        public IUserSessionRepository UserSessionRepository
        {
            get
            {
                return this.userSessionRepository;
            }
        }

        public IUserRolePermissionRepository UserRolePermissionRepository
        {
            get
            {
                return this.userRolePermissionRepository;
            }
        }

        public IAuditRepository AuditRepository
        {
            get
            {
                return this.auditRepository;
            }
        }

        public IEmailNotificationRepository EmailNotificationRepository
        {
            get
            {
                return this.emailNotificationRepository;
            }
        }

        public IOTPRepository OTPRepository
        {
            get
            {
                return this.otpRepository;
            }
        }
        
        public ILOVRepository LOVRepository
        {
            get
            {
                return this.lovRepository;
            }
        }

        public IExceptionHelper ExceptionHelper
        {
            get
            {
                return this.exceptionHelper;
            }
        }
        
        public IRoleRepository RoleRepository
        {
            get
            {
                return this.roleRepository;
            }
        }
        
        public IPermissionRepository PermissionRepository
        {
            get
            {
                return this.permissionRepository;
            }
        }
        
        public IPushNotificationRepository PushNotificationRepository
        {
            get
            {
                return this.pushNotificationRepository;
            }
        }
        
        public IRequestInfo RequestInfo
        {
            get
            {
                return this.requestInfo;
            }
        }
        
        public async Task<int> SaveAsync()
        {
            try
            {
                return await this.DBContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                this.exceptionHelper.ThrowAPIException(e.EntityValidationErrors.First().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        public int Save()
        {
            try
            {
                return this.DBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.Entity.DbContextTransaction BeginTransaction()
        {
            return this.DBContext.Database.BeginTransaction();
        }
    }
}
