using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.Extensions;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class PermissionService : Service<IPermissionRepository, Permission, PermissionDTO, int>, IPermissionService
    {
        private ISSHRequestInfo requestInfo;
        private IExceptionHelper exceptionHelper;

        public PermissionService(
            ISSHUnitOfWork unitOfWork,
            ISSHRequestInfo requestInfo,
            IExceptionHelper exceptionHelper)
            : base(unitOfWork, unitOfWork.PermissionRepository)
        {
            this.requestInfo = requestInfo;
            this.exceptionHelper = exceptionHelper;
        }

        public async Task<PermissionDTO> Get(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var permissions = await this.Repository.GetAll();
                var permission = permissions.FirstOrDefault(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.IsDeleted == false);

                if (permission == null)
                {
                    this.exceptionHelper.ThrowAPIException(Message.InvalidPermission);
                }

                var dto = new PermissionDTO();
                dto.ConvertFromEntity(permission);

                return dto;
            }

            return null;
        }

        public async Task<List<PermissionDTO>> Get(PermissionGroup group)
        {
            var permissions = await this.Repository.GetAll();
            permissions = permissions.Where(x => x.Group == group.ToString());

            return PermissionDTO.ConvertEntityListToDTOList(permissions);
        }

        public async Task<List<PermissionDTO>> GetReportPermissions(string reportName)
        {
            var reportPermissions = new List<PermissionDTO>();

            if (!string.IsNullOrWhiteSpace(reportName))
            {
                var permissions = await this.Get(PermissionGroup.Report);
                foreach (var permission in permissions)
                {
                    var reportComparer = reportName.RemoveSpaces().Trim().ToLower();
                    var permissionComparer = permission.Name.Trim().ToLower();

                    if (permissionComparer.Contains(reportComparer))
                    {
                        reportPermissions.Add(permission);
                    }
                }
            }

            return reportPermissions;
        }
    }
}
