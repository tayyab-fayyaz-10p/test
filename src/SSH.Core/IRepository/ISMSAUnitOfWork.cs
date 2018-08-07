using System.Data.Entity;
using System.Threading.Tasks;
using Recipe.Core.Base.Interface;
using SSH.Core.Infrastructure;

namespace SSH.Core.IRepository
{
    public interface ISSHUnitOfWork : IUnitOfWork
    {
        IRequestInfo RequestInfo { get; }

        IUserRepository UserRepository { get; }
        
        IAuditRepository AuditRepository { get; }
        
        IEmailNotificationRepository EmailNotificationRepository { get; }

        IOTPRepository OTPRepository { get; }

        ILOVRepository LOVRepository { get; }

        IExceptionHelper ExceptionHelper { get; }

        IUserRolePermissionRepository UserRolePermissionRepository { get; }
        
        IRoleRepository RoleRepository { get; }
        
        IPermissionRepository PermissionRepository { get; }
        
        IPushNotificationRepository PushNotificationRepository { get; }

        IUserSessionRepository UserSessionRepository { get; }

        Task<int> SaveAsync();

        int Save();

        DbContextTransaction BeginTransaction();
    }
}