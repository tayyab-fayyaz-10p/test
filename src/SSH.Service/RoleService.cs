using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipe.Core.Base.Generic;
using SSH.Core.Constant;
using SSH.Core.DTO;
using SSH.Core.Entity;
using SSH.Core.Enum;
using SSH.Core.Infrastructure;
using SSH.Core.IRepository;
using SSH.Core.IService;

namespace SSH.Service
{
    public class RoleService : Service<IRoleRepository, ApplicationRole, RoleDTO, string>, IRoleService
    {
        private ISSHRequestInfo requestInfo;
        private ApplicationRoleManager manager;

        public RoleService(
            ISSHUnitOfWork unitOfWork,
            ISSHRequestInfo requestInfo)
            : base(unitOfWork, unitOfWork.RoleRepository)
        {
            this.requestInfo = requestInfo;
            this.manager = new ApplicationRoleManager(new ApplicationRoleStore(this.requestInfo));
        }

        public override async Task<IList<RoleDTO>> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            
            return result
                .Where(x => 
                    x.Name.Trim() != UserRoles.None.ToString())
                    .ToList();
        }

        public List<RoleDTO> GetCurrentUserRoleMapping()
        {
            var role = (UserRoles)Enum.Parse(typeof(UserRoles), this.requestInfo.Role);

            var mapping = RoleMapping.GetMapping(role);
            var dtoList = new List<RoleDTO>();

            foreach (var item in mapping.Roles)
            {
                RoleDTO dto = new RoleDTO();
                dto.ConvertFromEntity(item);
                dtoList.Add(dto);
            }

            return dtoList;
        }
    }
}
